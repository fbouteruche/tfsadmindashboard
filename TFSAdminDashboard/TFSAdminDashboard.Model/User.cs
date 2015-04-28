using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class User
    {
        private List<string> applicationGroups = new List<string>();
        public string Name
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }

        public ICollection<string> ApplicationGroups
        {
            get
            {
                return applicationGroups;
            }
        }

        public string Mail { get; set; }

        public string Domain { get; set; }

        public string Account { get; set; }

        public string DN { get; set; }

        public string OU { get; set; }

        public string SID { get; set; }
    }
}
