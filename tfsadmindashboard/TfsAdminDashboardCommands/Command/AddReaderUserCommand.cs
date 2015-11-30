using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Client.Catalog.Objects;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TfsAdminDashboardCommands.Service;

namespace TfsAdminDashboardCommands.Command
{
    public class AddReaderUserCommand : TFSAccessHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AddReaderUserCommand() { }

        public void Execute(CommandLineOptions args)
        {
            string userSid = ADService.GetUserSiD(args.addTeamCollectionReader);

            IdentityDescriptor tfsUserDescriptor = new IdentityDescriptor("System.Security.Principal.WindowsIdentity", userSid);


            AddUserAsReader(args, tfsUserDescriptor);
        }

        private void AddUserAsReader(CommandLineOptions args, IdentityDescriptor userSid)
        {
            ITeamProjectCollectionService collectionService = configurationServer.GetService<ITeamProjectCollectionService>();

            if (collectionService != null)
            {
                TeamProjectCollection collection = collectionService.GetCollections().Where(x => x.Name == args.TeamProjectCollection).FirstOrDefault();

                if (collection != null)
                {
                    TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(collection.Id);

                    var structService = tpc.GetService<ICommonStructureService>();
                    IIdentityManagementService ims = tpc.GetService<IIdentityManagementService>();

                    var totalProjects = structService.ListAllProjects();

                    logger.Info("   {0} project to process in collection {1}", totalProjects.Length, collection.Name);
                    foreach (ProjectInfo p in totalProjects)
                    {
                        logger.Info("       Process project {0}", p.Name);

                        List<ApplicationGroupDefinition> applicationGroupCollection = new List<ApplicationGroupDefinition>();

                        //Get the project application groups
                        TeamFoundationIdentity[] lightApplicationGroups = ims.ListApplicationGroups(p.Uri, ReadIdentityOptions.IncludeReadFromSource);
                        //Read the project application groups identities with an expended membership query to populate the Members properties
                        TeamFoundationIdentity[] fullApplicationGroups = ims.ReadIdentities(lightApplicationGroups.Select(x => x.Descriptor).ToArray(), MembershipQuery.Expanded, ReadIdentityOptions.None);


                        foreach (TeamFoundationIdentity applicationGroup in fullApplicationGroups)
                        {
                            if (applicationGroup.DisplayName.Contains("Readers"))
                            {
                                logger.Info("           Add user to Group {0}", applicationGroup.DisplayName);

                                ims.AddMemberToApplicationGroup(applicationGroup.Descriptor, userSid);
                            }
                        }
                    }
                }
            }
        }
    }
}
