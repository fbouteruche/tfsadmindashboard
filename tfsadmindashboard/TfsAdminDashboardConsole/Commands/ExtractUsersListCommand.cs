using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CsvHelper;
using Microsoft.TeamFoundation.Client;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TfsAdminDashboardConsole.Commands.IO;
using System.DirectoryServices;
using NLog;
using MoreLinq;

namespace TfsAdminDashboardConsole.Commands
{
    class ExtractUsersListCommand : iCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private TfsConfigurationServer configurationServer = new TfsConfigurationServer(
    new Uri(Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User)),
    new NetworkCredential(Environment.GetEnvironmentVariable("TfsLoginName", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsPassword", EnvironmentVariableTarget.User)));

        public ExtractUsersListCommand() { }

        public void Execute()
        {
            logger.Info("Extract Users List in progress...");
            var projectCollections = CatalogNodeBrowsingHelper.GetProjectCollections(configurationServer.CatalogNode);
            List<User> userList = IdentityServiceManagementHelper.GetAllIdentities(configurationServer, projectCollections).ToList();

            // Now get the "ou" attribute from the Active Directory (so as to gets a user's Service)
            logger.Info("Fetch the ou property in the AD");
            Fetch_OU_ADProperty(userList);

            string fileName = FileNameTool.GetFileName("TfsExtractUsersList");

            using (CsvWriter csv = new CsvWriter(new StreamWriter(fileName)))
            {
                csv.Configuration.RegisterClassMap<ProjectDefinitionCsvMap>();
                csv.WriteExcelSeparator();
                csv.WriteRecords(userList.Where(x => !string.IsNullOrEmpty(x.OU) && !x.OU.StartsWith("FT/SCE/G2S/NRS/IT LABS")).DistinctBy(x => x.OU).OrderBy(x => x.Name));
            }

            logger.Info("Extract Users done");
        }

        private void Fetch_OU_ADProperty(List<User> userList)
        {
            string password = Environment.GetEnvironmentVariable("LDAPPassword");
            DirectoryEntry de = new DirectoryEntry(Environment.GetEnvironmentVariable("LDAPAddress"), Environment.GetEnvironmentVariable("LDAPLogin"), password);

            int total = userList.Count;
            int current = 0;
            logger.Info("{0} record to process", total);

            foreach(User user in userList)
            {
                ++current;

                if(current % 100 == 0)
                    logger.Info("{0} processed on {1}", current, total);
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.PropertiesToLoad.Add("ou");
                ds.Filter = string.Format("(&((&(objectCategory=Person)(objectClass=User)))(objectSid=*{0}*))", user.SID);

                SearchResultCollection rs = ds.FindAll();

                if (rs.Count > 0)
                { 
                    DirectoryEntry entry = rs[0].GetDirectoryEntry();

                    if(entry.Properties.Contains("ou"))
                    { 
                        user.OU = entry.Properties["ou"].Value.ToString();
                    }
                }
            }
        }
    }
}
