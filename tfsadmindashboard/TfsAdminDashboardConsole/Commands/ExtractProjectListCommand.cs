using CsvHelper;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TfsAdminDashboardConsole.Commands.IO;
using NLog;
using Newtonsoft.Json;
using TfsAdminDashboardConsole.Service;

namespace TfsAdminDashboardConsole.Commands
{
    public class ExtractProjectListCommand : TFSAccessHelper, iCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ExtractProjectListCommand() { }

        public void Execute(CommandLineOptions args)
        {
            logger.Info("Extract Project List in progress...");
            ICollection<ProjectDefinition> projectList = TeamProjectHelper.GetAllProjects(configurationServer, false);

            string fileName = FileNameTool.GetFileName("TfsExtractProjectList", args.OutputFormat);

            if (args.OutputFormat == "CSV")
            {
                using (CsvWriter csv = new CsvWriter(new StreamWriter(fileName)))
                {
                    csv.Configuration.RegisterClassMap<ProjectDefinitionCsvMap>();
                    csv.WriteExcelSeparator();
                    csv.WriteRecords(projectList);
                }
            }
            else
            {
                string json = JsonConvert.SerializeObject(projectList);
                File.WriteAllText(fileName, json);
            }

            if(args.UploadSFTP)
            {
                logger.Info("STFP Upload");
                ISFTPService sftp = new SFTPService();
                sftp.UploadFile(fileName);
            }

            logger.Info("Extract Project done");
        }
    }
}
