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
using System.Xml.Serialization;
using Newtonsoft.Json;
using TfsAdminDashboardConsole.Service;

namespace TfsAdminDashboardConsole.Commands
{
    public class ExtractUsersListCommand : TFSAccessHelper, iCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
  
        public void Execute(CommandLineOptions args)
        {
            logger.Info("Extract Users List in progress...");
            var projectCollections = CatalogNodeBrowsingHelper.GetProjectCollections(configurationServer.CatalogNode);
            List<User> userList = IdentityServiceManagementHelper.GetAllIdentities(configurationServer, projectCollections).ToList();


            // SerializerService.Serialize(@"D:\UnfilteredUserListWithoutOU.dat", userList);


            // Now get the "ou" attribute from the Active Directory (so as to gets a user's Service)
            if (args.extractUOFromAD)
            {
                logger.Info("Fetch the ou property in the AD for AD-SUBS users");
                userList = userList.Where(x => x.Domain == "AD-SUBS" && !x.DN.Contains("Quarantaine") && !x.DN.Contains("TTTT")).ToList();
                Fetch_OU_ADProperty(userList);
            }

            string fileName = FileNameTool.GetFileName("TfsExtractUsersList", args.OutputFormat);

            IOrderedEnumerable<User> filteredUsers = null;

            if (args.extractUOFromAD)
            {
                filteredUsers = userList.OrderBy(x => x.Name).DistinctBy(x => x.Name).OrderBy(x => x.Name);
            }
            else
            {
                filteredUsers = userList.DistinctBy(x => x.Mail).OrderBy(x => x.Name);
            }

            if (args.OutputFormat == "CSV")
            {
                using (CsvWriter csv = new CsvWriter(new StreamWriter(fileName)))
                {
                    csv.Configuration.RegisterClassMap<ProjectDefinitionCsvMap>();
                    csv.WriteExcelSeparator();

                    csv.WriteRecords(filteredUsers);
                }
            }
            else
            {
                string json = JsonConvert.SerializeObject(filteredUsers);
                File.WriteAllText(fileName, json);
            }

            if (args.UploadSFTP)
            {
                logger.Info("STFP Upload");
                ISFTPService sftp = new SFTPService();
                sftp.UploadFile(fileName);
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

            foreach (User user in userList)
            {
                ++current;

                if (current % 100 == 0)
                    logger.Info("{0} processed on {1}", current, total);
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.PropertiesToLoad.Add("ou");
                ds.Filter = string.Format("(&((&(objectCategory=Person)(objectClass=User)))(objectSid=*{0}*))", user.SID);

                SearchResultCollection rs = ds.FindAll();

                if (rs.Count > 0)
                {
                    DirectoryEntry entry = rs[0].GetDirectoryEntry();

                    if (entry.Properties.Contains("ou"))
                    {
                        user.OU = entry.Properties["ou"].Value.ToString();
                    }
                }
            }
        }
    }
}
