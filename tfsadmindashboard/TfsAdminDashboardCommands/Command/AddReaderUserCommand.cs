using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using NLog;
using System.Collections.Generic;
using System.Linq;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;

namespace TfsAdminDashboardCommands.Command
{
    public class AddReaderUserCommand : TFSAccessHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AddReaderUserCommand() { }

        public void Execute(CommandLineOptions args)
        {
            AddUserAsReader(args);
        }

        private void AddUserAsReader(CommandLineOptions args)
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

                    // List all users in collection valid users
                    var validUsers = ims.ReadIdentities(IdentitySearchFactor.AccountName, new[] { "Project Collection Valid Users" }, MembershipQuery.Expanded, ReadIdentityOptions.None)[0][0].Members;
                    var users = ims.ReadIdentities(validUsers, MembershipQuery.None, ReadIdentityOptions.None).Where(x => !x.IsContainer).ToArray();

                    // Find the user we are interested in
                    var user = users.Where(x => x.UniqueName.ToLowerInvariant() == args.addTeamCollectionReader.ToLowerInvariant()).FirstOrDefault();

                    // Get it's descriptor
                    IdentityDescriptor userSid = null;
                    if (user != null)
                    {
                        logger.Info("   resolved user is {0}", user.DisplayName);
                        userSid = user.Descriptor;
                    }

                    if (userSid != null)
                    {
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
                                    if(!ims.IsMember(applicationGroup.Descriptor, userSid))
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
    }
}
