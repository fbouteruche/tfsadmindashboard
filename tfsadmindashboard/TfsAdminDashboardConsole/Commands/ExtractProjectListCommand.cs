using CsvHelper;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.IO;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TfsAdminDashboardConsole.Commands.IO;
using TfsAdminDashboardConsole.ExtensionsMethods;
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
            ICollection<ProjectDefinition> projectList = TeamProjectHelper.GetAllProjects();

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
                logger.Info("STFP Upload for file {1}", fileName);
                ISFTPService sftp = new SFTPService();
                sftp.UploadFile(fileName, args.SFTPAuthentMode.ToEnum<AuthentMethod>());
            }

            logger.Info("Extract Project done");
        }
    }
}
