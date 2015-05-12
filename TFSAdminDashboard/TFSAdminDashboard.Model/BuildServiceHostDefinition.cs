using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class BuildServiceHostDefinition
    {

        private List<BuildControllerDefinition> buildControllers = new List<BuildControllerDefinition>();

        public ICollection<BuildControllerDefinition> BuildControllers
        {
            get
            {
                return buildControllers;
            }
        }

        private List<BuildAgentDefinition> buildAgents = new List<BuildAgentDefinition>();

        public ICollection<BuildAgentDefinition> BuildAgents
        {
            get
            {
                return buildAgents;
            }
        }

        public string Name
        {
            get;
            set;
        }

        public string CollectionName { get; set; }

        public void SortMachines()
        {
            buildAgents.OrderBy(x => x.Name);
            buildControllers.OrderBy(x => x.Name);
        }
    }
}
