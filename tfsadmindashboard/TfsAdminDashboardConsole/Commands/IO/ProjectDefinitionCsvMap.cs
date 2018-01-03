using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TfsAdminDashboardConsole.Commands
{
    public sealed class ProjectDefinitionCsvMap : CsvClassMap<ProjectSimpleDefinition>
    {
        public ProjectDefinitionCsvMap()
        {
            Map(m => m.Collection).Index(0).Name("Collection");
            Map(m => m.Name).Index(1).Name("Project");
            Map(m => m.Id).Index(2).Name("ID"); 
        }
    }
}
