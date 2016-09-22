using CsvHelper;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TfsAdminDashboardConsole.Commands.IO;
using TfsAdminDashboardConsole.ExtensionsMethods;
using TfsAdminDashboardConsole.Service;

namespace TfsAdminDashboardConsole.Commands
{
    public class ExtractSimpleProjectListCommand : iCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ExtractSimpleProjectListCommand() { }

        public void Execute(CommandLineOptions args)
        {
            try
            {
                logger.Info("Extract Simple Project List in progress...");
                ICollection<ProjectSimpleDefinition> projectList = DashTeamProjectHelper.GetAllProjectsSimple();

                string fileName = FileNameTool.GetFileName("TfsExtractProjectSimpleList", args.OutputFormat);

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

                if (args.UploadSFTP)
                {
                    logger.Info("STFP Upload for file {1}", fileName);
                    ISFTPService sftp = new SFTPService();
                    sftp.UploadFile(fileName, args.SFTPAuthentMode.ToEnum<AuthentMethod>());
                }

                logger.Info("Extract Project done");
            }
            catch (Exception e)
            {
                logger.Error<Exception>(e);
                throw;
            }
        }
    }
}
