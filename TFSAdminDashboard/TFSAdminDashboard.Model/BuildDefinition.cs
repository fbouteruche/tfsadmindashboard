using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class Build_Definition
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

        /// <summary>
        /// Percentage of succeed build over the last five ones
        /// Mimics the Jenkins sun/cloud indicator
        /// </summary>
        public int Health { get; set; }

        public int RetainedBuild { get; set; }

        public string type { get; set; }

        public DateTime? LastSuccess { get; set; }

        public DateTime? LastFail { get; set; }
        public bool UsesDependencyCheck { get; set; }
    }
}
