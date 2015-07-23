using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class BuildAgentDefinition
    {
        public string Name { get; set; }

        public List<string> TagList { get; set; }

        public string Tags
        {
            get
            {
                return string.Join( ",", this.TagList.ToArray() );
            }
        }

        public string Status { get; set; }

        public string RDPUri { get; set; }
    }
}
