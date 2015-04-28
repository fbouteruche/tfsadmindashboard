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

namespace TfsAdminDashboardConsole.Commands
{
    class ExtractProjectListCommand : iCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private TfsConfigurationServer configurationServer = new TfsConfigurationServer(
            new Uri(Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User)),
            new NetworkCredential(Environment.GetEnvironmentVariable("TfsLoginName", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsPassword", EnvironmentVariableTarget.User)));

       public ExtractProjectListCommand() {}

        public void Execute()
        {
            logger.Info("Extract Project List in progress...");
            ICollection<ProjectDefinition> projectList = TeamProjectHelper.GetAllProjects(configurationServer);

            string fileName = FileNameTool.GetFileName("TfsExtractProjectList");

            using (CsvWriter csv = new CsvWriter(new StreamWriter(fileName)))
            {
                csv.Configuration.RegisterClassMap<ProjectDefinitionCsvMap>();
                csv.WriteExcelSeparator();
                csv.WriteRecords(projectList);
            }

            logger.Info("Extract Project done");
        }
    }
}
