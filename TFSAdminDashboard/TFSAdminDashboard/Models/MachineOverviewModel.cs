using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.Models
{
    public class MachineOverviewModel
    {

        private ICollection<BuildServiceHostDefinition> buildServiceHostCollection = new List<BuildServiceHostDefinition>();

        public ICollection<BuildServiceHostDefinition> BuildServiceHostCollection
        {
            get
            {
                return this.buildServiceHostCollection;
            }
        }

        public void SetBuildServiceHostCollection(ICollection<BuildServiceHostDefinition> buildServiceHostCollection)
        {
            this.buildServiceHostCollection = buildServiceHostCollection;
        }
    }
}