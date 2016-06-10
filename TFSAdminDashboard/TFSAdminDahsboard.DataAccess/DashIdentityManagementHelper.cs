
using MoreLinq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using TFSAdminDashboard.DTO;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;

namespace TFSAdminDashboard.DataAccess
{
    public class DashIdentityManagementHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets all identities, parsing each project's default team.
        /// </summary>
        public static ICollection<User> GetAllIdentities()
        {
            List<TeamProject> projects = DataServiceTeamProjects.AllProjects();

            List<User> userList = new List<User>();

            int i = 0;
            foreach (TeamProject proj in projects)
            {
                ++i;
                logger.Info("Process project {0} {1}/{2}", proj.name, i, projects.Count);
                foreach (TeamMember member in DataServiceTeams.DefaultMembers(proj.collectionName, proj.name))
                {
                    userList.Add(new User()
                    {
                        Name = member.displayName,
                        Account = member.uniqueName
                    });
                }
            }

           return userList.DistinctBy(x => x.Name).OrderBy(x => x.Name).ToList();
        }

        public static Tuple<List<ApplicationGroupDefinition>, List<User>> FeedIdentityData(string collectionName, string projectName)
        {
            List<User> userList = new List<User>();

            foreach (TeamMember member in DataServiceTeams.DefaultMembers(collectionName, projectName))
            {
                userList.Add(new User()
                {
                    Name = member.displayName,
                    Account = member.uniqueName
                });
            }
            return null;
            //return Tuple.Create(applicationGroupCollection, userCollection);
        }
    }
}
