using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class BuildDefinition
    {
        public string Name
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public string ContinuousIntegrationType
        {
            get;
            set;
        }

        public int FailedOrPartial { get; set; }

        public int RetainedBuild { get; set; }

        public DateTime LastSuccess { get; set; }

        public DateTime LastFail { get; set; }
    }
}
