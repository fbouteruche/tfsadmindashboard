using CsvHelper;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TfsAdminDashboardConsole.Commands;
using TfsAdminDashboardConsole.Commands.IO;
using TfsAdminDashboardConsole.ExtensionsMethods;
using TfsAdminDashboardConsole.Service;

namespace TfsAdminDashboardConsole
{
    public class ExtractBuildsCommand : iCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public ExtractBuildsCommand() { }

        public void Execute(CommandLineOptions args)
        {
            try
            {
                logger.Info("Extract Builds in progress...");
                ICollection<BuildRun> buildList = DashBuildHelper.GetAllBuilds();

                string fileName = FileNameTool.GetFileName("TfsExtractBuilds", args.OutputFormat);

                if (args.OutputFormat == "CSV")
                {
                    using (CsvWriter csv = new CsvWriter(new StreamWriter(fileName)))
                    {
                        csv.Configuration.RegisterClassMap<ProjectDefinitionCsvMap>();
                        csv.WriteExcelSeparator();
                        csv.WriteRecords(buildList);
                    }
                }
                else
                {
                    string json = JsonConvert.SerializeObject(buildList);
                    File.WriteAllText(fileName, json);
                }

                if (args.UploadSFTP)
                {
                    logger.Info("STFP Upload for file {1}", fileName);
                    ISFTPService sftp = new SFTPService();
                    sftp.UploadFile(fileName, args.SFTPAuthentMode.ToEnum<AuthentMethod>());
                }

                logger.Info("Extract Builds done");
            }
            catch (Exception e)
            {
                logger.Error<Exception>(e);
                throw;
            }
        }
    }
}