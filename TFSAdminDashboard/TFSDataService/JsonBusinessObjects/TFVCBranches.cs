using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class TFVCBranchRootobject
    {
        public int count { get; set; }
        public TFVCBranch[] value { get; set; }
    }

    public class TFVCBranch
    {
        public string path { get; set; }
        public string description { get; set; }
        public Owner owner { get; set; }
        public DateTime createdDate { get; set; }
        public string url { get; set; }
        public object[] relatedBranches { get; set; }
        public object[] mappings { get; set; }
    }

    public class Owner
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

}
