using System.Collections.Generic;

namespace TFSAdminDashboard.DTO
{
    public class GitData
    {
        public GitData()
        {
        }

        public List<BranchData> Branches { get; set; }
        public string DefaultBranch { get; set; }
        public int MasterCommitsYesterday { get; set; }
        public string Name { get; set; }
        public List<TagData> Tags { get; set; }
    }
}