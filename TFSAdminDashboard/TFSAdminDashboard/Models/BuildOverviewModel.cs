using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.Models
{
    public class BuildOverviewModel
    {
        private List<BuildDefinition> buildDefinitionCollection = new List<BuildDefinition>();

        public ICollection<BuildDefinition> BuildDefinitionCollection
        {
            get
            {
                return buildDefinitionCollection;
            }
        }
    }
}