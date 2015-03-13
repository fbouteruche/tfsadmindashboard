using Microsoft.TeamFoundation.Client;
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
        public static int GetAllIdentityCount(IIdentityManagementService ims)
        {
            // TODO
            return 0;
            //ims.ReadIdentities();
        }

        public static void FeedIdentityData(ICollection<ApplicationGroupDefinition> applicationGroupCollection, ICollection<User> userCollection, IIdentityManagementService ims, string projectUri)
        {
            //Get the project application groups
            TeamFoundationIdentity[] lightApplicationGroups = ims.ListApplicationGroups(projectUri, ReadIdentityOptions.IncludeReadFromSource);
            //Read the project application groups identities with an expended membership query to populate the Members properties
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
                            user = new User() { 
                                Name = applicationGroupMember.DisplayName, 
                                IsActive = applicationGroupMember.IsActive,
                                Mail = applicationGroupMember.GetAttribute("Mail", string.Empty),
                                Domain = applicationGroupMember.GetAttribute("Domain", string.Empty),
                                Account = applicationGroupMember.GetAttribute("Account", string.Empty),
                                DN = applicationGroupMember.GetAttribute("DN", string.Empty)
                            };
                            userCollection.Add(user);
                        }
                        user.ApplicationGroups.Add(applicationGroupDefinition.Name);
                        applicationGroupDefinition.UserCollection.Add(user);
                    }
                }
                applicationGroupCollection.Add(applicationGroupDefinition);
            }
        }
    }
}
