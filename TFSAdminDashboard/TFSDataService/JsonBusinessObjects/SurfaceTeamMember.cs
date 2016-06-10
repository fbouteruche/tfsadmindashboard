using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class SurfaceTeamMemberRootobject
    {
        public SurfaceTeamMember[] value { get; set; }
        public int count { get; set; }
    }

    public class SurfaceTeamMember
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

}
