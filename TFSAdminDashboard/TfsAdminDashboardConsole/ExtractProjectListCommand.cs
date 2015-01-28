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

namespace TfsAdminDashboardConsole
{
    class ExtractProjectListCommand
    {
        private TfsConfigurationServer configurationServer = new TfsConfigurationServer(
            new Uri(Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User)),
            new NetworkCredential(Environment.GetEnvironmentVariable("TfsLoginName", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsPassword", EnvironmentVariableTarget.User)));

       public ExtractProjectListCommand() {}

        public void Execute(string[] args)
        {
            ICollection<ProjectDefinition> projectList = TeamProjectHelper.GetAllProjects(configurationServer);


            CsvWriter csv = new CsvWriter(new StreamWriter(Path.Combine(Environment.GetEnvironmentVariable("TfsExtractPath", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsExtractProjectList", EnvironmentVariableTarget.User))));
            csv.Configuration.RegisterClassMap<ProjectDefinitionCsvMap>();
            csv.WriteRecords(projectList);
        }
    }
}
