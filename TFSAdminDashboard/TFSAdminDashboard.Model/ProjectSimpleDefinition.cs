using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class ProjectSimpleDefinition
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the collection
        /// </summary>
        public string Collection { get; set; }

        /// <summary>
        /// TFS internal ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// TFS State
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// Project Url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// Report requirement Overview 
        /// </summary>
        public string ReportsUrl { get; set; }

        /// <summary>
        /// Date of first git commit, or date of the platform's installation
        /// </summary>
        public DateTime? UtcCreationDate { get; set; }

        /// <summary>
        /// Did any commit occur in the last 10 days?
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Last commit date
        /// </summary>
        public DateTime? LastCommit { get; set; }

        /// <summary>
        /// Commit number, grand total
        /// </summary>
        public int GitCommitsYesterday { get; set; }
       
        /// <summary>
        /// Number of build definitions
        /// </summary>
        public int BuildNumber { get; set; }

        /// <summary>
        /// Percentage of succeed build over the last five ones
        /// Mimics the Jenkins sun/cloud indicator => Average on all definitions
        /// </summary>
        public double BuildHealth { get; set; }

        /// <summary>
        /// Number of workitems
        /// </summary>
        public int WorkItemNumber { get; set; }

        /// <summary>
        /// % of closed workitems
        /// </summary>
        public double WorkItemHealth { get; set; }
        
        /// <summary>
        /// Number of test cases
        /// </summary>
        public int TestNumber { get; set; }

        /// <summary>
        /// % of passed tests
        /// </summary>
        public double TestHealth { get; set; }
        public bool TFVCFlag { get; set; }
        public string Platform { get; set; }
        public string DM { get; set; }
        public double XamlRatio { get; set; }

        public List<GitData> Repositories { get;  set; }
    }
}
