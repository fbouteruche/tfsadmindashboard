using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TfsAdminDashboardConsole.Commands
{
    public sealed class ProjectDefinitionCsvMap : CsvClassMap<ProjectDefinition>
    {
        public ProjectDefinitionCsvMap()
        {
            Map(m => m.CollectionName).Index(0).Name("Collection");
            Map(m => m.Name).Index(1).Name("Project");
            Map(m => m.UtcCreationDate).Index(2).Name("Creation Date");

            Map(m => m.LastCheckinDate).Index(3).Name("Last Checkin Date");
            Map(m => m.LastFailedBuild).Index(4).Name("Last Failed Build Date");
            Map(m => m.LastSuccessBuild).Index(5).Name("Last OK Build Date");

        }
    }
}
