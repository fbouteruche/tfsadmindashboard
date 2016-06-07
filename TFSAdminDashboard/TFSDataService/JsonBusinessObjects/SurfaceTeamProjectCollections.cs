using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class SurfaceTeamProjectCollectionRootobject
    {
        public int count { get; set; }
        public SurfaceTeamProjectCollection[] value { get; set; }
    }

    public class SurfaceTeamProjectCollection
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

}
