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

namespace TfsAdminDashboardConsole.Commands
{
    public class ExtractProjectListCommand : TFSAccessHelper, iCommand
    {
       private static Logger logger = LogManager.GetCurrentClassLogger();

       public ExtractProjectListCommand() {}

        public void Execute(string outFormat = "CSV")
        {
            logger.Info("Extract Project List in progress...");
            ICollection<ProjectDefinition> projectList = TeamProjectHelper.GetAllProjects(configurationServer);

            string fileName = FileNameTool.GetFileName("TfsExtractProjectList", outFormat);


            if(outFormat == "CSV")
            { 
            using (CsvWriter csv = new CsvWriter(new StreamWriter(fileName)))
            {
                csv.Configuration.RegisterClassMap<ProjectDefinitionCsvMap>();
                csv.WriteExcelSeparator();
                csv.WriteRecords(projectList);
            }

            logger.Info("Extract Project done");
            }
            else
            {
#if DEBUG
                string json = JsonConvert.SerializeObject(projectList.First());
#else
                string json = JsonConvert.SerializeObject(projectList);
#endif
                File.WriteAllText(fileName, json);
            }
        }
    }
}
