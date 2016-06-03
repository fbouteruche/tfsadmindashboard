using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class GitTagRootobject
    {
        public GitTag[] value { get; set; }
        public int count { get; set; }
    }

    public class GitTag
    {
        public string name { get; set; }
        public string objectId { get; set; }
        public string tagname
        {
            get
            {
                return this.name.Substring("refs/tags/".Length);
            }
        }
        public string url { get; set; }
    }

}
