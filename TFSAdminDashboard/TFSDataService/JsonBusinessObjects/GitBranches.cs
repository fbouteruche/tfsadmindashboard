using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class GitBranchRootobject
    {
        public GitBranch[] value { get; set; }
        public int count { get; set; }
    }

    public class GitBranch
    {
        public string name { get; set; }
        public string branchname
        {
            get
            {
                return this.name.Substring("refs/heads/".Length);
            }
        }

        public string objectId { get; set; }
        public string url { get; set; }
    }

}
