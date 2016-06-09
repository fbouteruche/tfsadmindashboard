﻿using CsvHelper;
using MoreLinq;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TfsAdminDashboardConsole.Commands.IO;
using TfsAdminDashboardConsole.ExtensionsMethods;
using TfsAdminDashboardConsole.Service;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;

namespace TfsAdminDashboardConsole.Commands
{
    public class ExtractUsersListCommand : iCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
  
        public void Execute(CommandLineOptions args)
        {
            logger.Info("Extract Users List in progress...");
            string fileName = FileNameTool.GetFileName("TfsExtractUserList", args.OutputFormat);

            var filteredUsers = DashIdentityManagementHelper.GetAllIdentities();

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
                sftp.UploadFile(fileName, args.SFTPAuthentMode.ToEnum<AuthentMethod>());
            }

            logger.Info("Extract Users done");
        }
    }
}
