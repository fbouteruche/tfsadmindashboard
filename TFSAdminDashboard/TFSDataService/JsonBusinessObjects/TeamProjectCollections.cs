using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class TeamProjectCollectionRootobject
    {
        public int count { get; set; }
        public TeamProjectCollection[] value { get; set; }
    }

    public class TeamProjectCollection
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

}
