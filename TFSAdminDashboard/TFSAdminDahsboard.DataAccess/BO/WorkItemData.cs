using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.DataAccess.BO
{
    public class WorkItemData
    {
        public List<WorkItemDefinition> workItemDefinitionCollection;
        public DateTime lastmodif;
        public int modifsYesterday;
        internal int testcasemodifyesterday;
    }
}
