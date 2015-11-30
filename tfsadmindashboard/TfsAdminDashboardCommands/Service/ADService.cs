
namespace TfsAdminDashboardCommands.Service
{
    using System.DirectoryServices;
    using System.IO;
    using System.Security.Principal;
    using NLog;
    using System;

    /// <summary>
    /// Service used to deal with Active Directory info retrieving
    /// </summary>
    public static class ADService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The main Directory Entry
        /// </summary>
        private static DirectoryEntry de = null;

        /// <summary>
        /// Gets the user sid.
        /// </summary>
        /// <param name="userAccount">The user account.</param>
        /// <param name="userdomain">The userdomain.</param>
        /// <param name="ldapAddress">The LDAP address.</param>
        /// <param name="ldapUser">The LDAP user.</param>
        /// <param name="ldapPassword">The LDAP password.</param>
        /// <param name="ldapextension">The ldapextension (com or fr or whatever). Used for matching the userPrincipalName</param>
        /// <returns>
        /// The User's SID
        /// </returns>
        public static string GetUserSiD(string userAccount)
        {
            logger.Info("Get user SID for {0} in domain {1}", userAccount, Environment.GetEnvironmentVariable("LDAPAddress"));

            if (de == null)
            {
                string password = Environment.GetEnvironmentVariable("LDAPPassword");
                de = new DirectoryEntry(Environment.GetEnvironmentVariable("LDAPAddress"), Environment.GetEnvironmentVariable("LDAPLogin"), password);
            }

            DirectorySearcher ds = new DirectorySearcher(de);
            ds.PropertiesToLoad.Add("name");
            ds.PropertiesToLoad.Add("sn");
            ds.PropertiesToLoad.Add("givenname");
            ds.PropertiesToLoad.Add("objectSid");
            ds.Filter = string.Format("(&((&(objectCategory=Person)(objectClass=User)))(sAMAccountName={0}))", string.Format(@"{0}", userAccount));

            ds.SearchScope = SearchScope.Subtree;

            SearchResultCollection rs = ds.FindAll();

            if (rs.Count != 1)
            {
                logger.Error("{0} informations found for user {1}@{2}.{3}", rs.Count, userAccount);
            }

            string ans = string.Empty;

            SearchResult result = null;

            if (rs.Count > 0)
            {
                result = rs[0];

                var sn = rs[0].GetDirectoryEntry().Properties["sn"].Value;
                var name = rs[0].GetDirectoryEntry().Properties["name"].Value;
                var givenname = rs[0].GetDirectoryEntry().Properties["givenname"].Value; ;

                if (rs[0].GetDirectoryEntry().Properties["objectSid"].Value != null)
                {
                    var sid = new SecurityIdentifier((byte[])rs[0].GetDirectoryEntry().Properties["objectSid"].Value, 0);
                    ans = sid.ToString();
                    logger.Info("SID found: {0}", ans);
                }
            }

            return ans;
        }
    }
}
