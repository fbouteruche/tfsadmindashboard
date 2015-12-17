﻿using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;


namespace TFSAdminDashboard.DataAccess
{
    public class IdentityServiceManagementHelper
    {

        /// <summary>
        /// Gets all identities (void email filtered, considered as services accounts).
        /// </summary>
        /// <remarks>Slow as hell though, we'll have to find a solution for this.</remarks>
        /// <param name="configurationServer">The configuration server.</param>
        /// <param name="TPCs">The list of TPCs to Query</param>
        /// <returns>the number of valid users</returns>
        public static ICollection<User> GetAllIdentities(TfsConfigurationServer configurationServer, ICollection<ProjectCollectionDefinition> TPCs)
        {
            List<User> globalUserCollection = new List<User>();

            foreach (var collection in TPCs)
            {
                TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(collection.InstanceId);

                ICollection<ApplicationGroupDefinition> groupCollection = new List<ApplicationGroupDefinition>();
                ICollection<User> userCollection = new List<User>();

                var tuple = IdentityServiceManagementHelper.FeedIdentityData(tpc, null);

                globalUserCollection.AddRange(tuple.Item2);
            }

            // distinct by non null email
            return globalUserCollection.Where(x => !string.IsNullOrEmpty(x.Mail)).ToList();
        }

        public static Tuple<List<ApplicationGroupDefinition>, List<User>> FeedIdentityData(TfsTeamProjectCollection tpc, string projectUri)
        {
            IIdentityManagementService ims = tpc.GetService<IIdentityManagementService>();
            return FeedIdentityData(ims, projectUri);
        }

        public static Tuple<List<ApplicationGroupDefinition>, List<User>> FeedIdentityData(IIdentityManagementService ims, string projectUri)
        {
            List<ApplicationGroupDefinition> applicationGroupCollection = new List<ApplicationGroupDefinition>();
            List<User> userCollection = new List<User>();

            //Get the Project Collection application groups
            TeamFoundationIdentity[] lightApplicationGroups = ims.ListApplicationGroups(projectUri, ReadIdentityOptions.IncludeReadFromSource);

            //Read the Project Collection application groups identities with an expanded membership query to populate the Members properties
            TeamFoundationIdentity[] fullApplicationGroups = ims.ReadIdentities(lightApplicationGroups.Select(x => x.Descriptor).ToArray(), MembershipQuery.Expanded, ReadIdentityOptions.None);


            foreach (TeamFoundationIdentity applicationGroup in fullApplicationGroups)
            {
                ApplicationGroupDefinition applicationGroupDefinition = new ApplicationGroupDefinition() { Name = applicationGroup.DisplayName };
                TeamFoundationIdentity[] applicationGroupMembers = ims.ReadIdentities(applicationGroup.Members, MembershipQuery.None, ReadIdentityOptions.None);
                foreach (TeamFoundationIdentity applicationGroupMember in applicationGroupMembers)
                {

                    if (!applicationGroupMember.IsContainer)
                    {
                        User user = userCollection.SingleOrDefault(x => x.Name == applicationGroupMember.DisplayName);
                        if (user == null)
                        {
                            user = new User()
                            {
                                Name = applicationGroupMember.DisplayName,
                                IsActive = applicationGroupMember.IsActive,
                                Mail = applicationGroupMember.GetAttribute("Mail", string.Empty),
                                Domain = applicationGroupMember.GetAttribute("Domain", string.Empty),
                                Account = applicationGroupMember.GetAttribute("Account", string.Empty),
                                DN = applicationGroupMember.GetAttribute("DN", string.Empty),
                                SID = applicationGroupMember.Descriptor.Identifier
                            };
                            userCollection.Add(user);
                        }
                        user.ApplicationGroups.Add(applicationGroupDefinition.Name);
                        applicationGroupDefinition.UserCollection.Add(user);
                    }
                }

                applicationGroupDefinition.SortUsers();

                applicationGroupCollection.Add(applicationGroupDefinition);
            }

            applicationGroupCollection = applicationGroupCollection.OrderBy(x => x.Name).ToList();

            return Tuple.Create(applicationGroupCollection, userCollection);
        }
    }
}
