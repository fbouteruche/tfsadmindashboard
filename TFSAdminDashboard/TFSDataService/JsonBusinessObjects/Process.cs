

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{
    public class ProcessRootobject
    {
        public int count { get; set; }
        public Process[] value { get; set; }
    }

    public class Process
    {
        public string id { get; set; }
        public string description { get; set; }
        public bool isDefault { get; set; }
        public string url { get; set; }
        public string name { get; set; }

        public _Links _links { get; set; }
    }

    public class _Links
    {
        public Self self { get; set; }
    }

}
