using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.Models
{
    public class WorkItemOverviewModel
    {
        private List<WorkItemDefinition> workItemDefinitionCollection = new List<WorkItemDefinition>();

        private List<WorkItemCategoryDefinition> workItemCategoryCollection = new List<WorkItemCategoryDefinition>();

        private Dictionary<string, IDictionary<string, int>> workItemStatistics = new Dictionary<string, IDictionary<string, int>>();


        public ICollection<WorkItemDefinition> WorkItemDefinitionCollection
        {
            get
            {
                return workItemDefinitionCollection;
            }
        }

        public ICollection<WorkItemCategoryDefinition> WorkItemCategoryCollection
        {
            get
            {
                return workItemCategoryCollection;
            }
        }

        public IDictionary<string, IDictionary<string, int>> WorkItemStatistics
        {
            get
            {
                return workItemStatistics;
            }
        }
    }
}
