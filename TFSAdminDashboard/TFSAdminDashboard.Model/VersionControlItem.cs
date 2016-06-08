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
        public DateTime LastCommitDate { get; set; }

        // TFSVC stuff
        public string DisplayName { get; set; }
  
        public string ItemChangeSetId { get; set; }

        public DateTime ItemLastCheckIn { get; set; }

        public string InnerChangeSetId { get; set; }

        public DateTime InnerLastCheckIn { get; set; }    
    }
}
