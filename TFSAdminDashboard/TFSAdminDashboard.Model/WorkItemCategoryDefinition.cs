using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSAdminDashboard.DTO
{
    public class WorkItemCategoryDefinition
    {
        private List<string> workItemTypeCollection = new List<string>();

        public string Name
        {
            get;
            set;
        }

        public ICollection<string> WorkItemTypeCollection
        {
            get
            {
                return workItemTypeCollection;
            }
        }

        public string DefaultWorkItemType
        {
            get;
            set;
        }
    }
}
