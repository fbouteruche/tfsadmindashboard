using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSAdminDashboard.DTO
{
    public class WorkItemDefinition
    {
        private Dictionary<string, int> stateCollection = new Dictionary<string, int>();

        private List<string> categories = new List<string>();

        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public ICollection<string> Categories
        {
            get
            {
                return categories;
            }
        }

        public IDictionary<string, int> StateCollection
        {
            get
            {
                return stateCollection;
            }
        }
    }
}
