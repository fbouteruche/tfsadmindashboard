
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

        /// <summary>
        /// Gets all identities, parsing each project's default team.
        /// </summary>
        public static ICollection<User> GetAllIdentities()
        {
            List<TeamProject> projects = DataServiceTeamProjects.AllProjects();

            List<User> userList = new List<User>();

            foreach (TeamProject proj in projects)
            {
                foreach (TeamMember member in DataServiceTeams.DefaultMembers(proj.collectionName, proj.name))
                {
                    userList.Add(new User()
                    {
                        Name = member.displayName,
                        Account = member.uniqueName
                    });
                }
            }

            return userList.DistinctBy(x => x.Name);
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

            //return Tuple.Create(applicationGroupCollection, userCollection);
        }
    }
}
