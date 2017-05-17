using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TfsAdminDashboardCommands
{
    internal class ManageUserGroupTool
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal void ManageUserInIdentifiedGroup(CommandLineOptions args, ITeamProjectCollectionService collectionService, TfsConfigurationServer configurationServer)
        {
            if (collectionService == null)
                return;

            TeamProjectCollection collection = collectionService.GetCollections().Where(x => x.Name == args.TeamProjectCollection).FirstOrDefault();

            if (collection == null)
                return;

            TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(collection.Id);

            var structService = tpc.GetService<ICommonStructureService>();
            IIdentityManagementService ims = tpc.GetService<IIdentityManagementService>();

            // List all users in collection valid users
            var validUsers = ims.ReadIdentities(IdentitySearchFactor.AccountName, new[] { "Project Collection Valid Users" }, MembershipQuery.Expanded, ReadIdentityOptions.None)[0][0].Members;
            var users = ims.ReadIdentities(validUsers, MembershipQuery.None, ReadIdentityOptions.None).Where(x => !x.IsContainer).ToArray();

            // Find the user we are interested in
            var user = users.Where(x => x.UniqueName.ToLowerInvariant() == args.accountName.ToLowerInvariant()).FirstOrDefault();

            // Get it's descriptor
            IdentityDescriptor userSid = null;
            if (user != null)
            {
                logger.Info("   resolved user is {0}", user.DisplayName);
                userSid = user.Descriptor;
            }

            if (userSid == null)
                return;


            var totalProjects = structService.ListAllProjects();

            logger.Info("   {0} project to process in collection {1}", totalProjects.Length, collection.Name);
            foreach (ProjectInfo p in totalProjects)
            {
                ManageProject(args, ims, userSid, p);
            }
        }

        private static void ManageProject(CommandLineOptions args, IIdentityManagementService ims, IdentityDescriptor userSid, ProjectInfo p)
        {
            logger.Info("       Process project {0}", p.Name);

            //Get the project application groups
            TeamFoundationIdentity[] lightApplicationGroups = ims.ListApplicationGroups(p.Uri, ReadIdentityOptions.IncludeReadFromSource);
            //Read the project application groups identities with an expended membership query to populate the Members properties
            TeamFoundationIdentity[] fullApplicationGroups = ims.ReadIdentities(lightApplicationGroups.Select(x => x.Descriptor).ToArray(), MembershipQuery.Expanded, ReadIdentityOptions.None);

            foreach (TeamFoundationIdentity applicationGroup in fullApplicationGroups)
            {
                if (args.Add)
                {
                    if (applicationGroup.DisplayName.Contains(args.groupName) && !ims.IsMember(applicationGroup.Descriptor, userSid))
                    {
                        logger.Info("           Add user to Group {0}", applicationGroup.DisplayName);
                        ims.AddMemberToApplicationGroup(applicationGroup.Descriptor, userSid);
                    }
                }
                else
                {
                    if (applicationGroup.DisplayName.Contains(args.groupName) && ims.IsMember(applicationGroup.Descriptor, userSid))
                    {
                        logger.Info("           Remove user from Group {0}", applicationGroup.DisplayName);
                        ims.RemoveMemberFromApplicationGroup(applicationGroup.Descriptor, userSid);
                    }
                }
            }
        }
    }
}



