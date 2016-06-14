using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class VersionControlItem
    {
        public bool isGit;

        // Git stuff
        public string Repository { get; set; }
        public string LastCommit { get; set; }
       
        // TFSVC stuff
        public string Path { get; set; }

        public int ItemChangeSetId { get; set; }

        public DateTime ItemDate { get; set; } 
    }
}
