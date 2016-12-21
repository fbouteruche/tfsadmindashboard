using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class TeamProjectRequest
    {
        public string name { get; set; }
        public string description { get; set; }
        public Capabilities capabilities { get; set; }
    }

    public class Capabilities
    {
        public Versioncontrol versioncontrol { get; set; }
        public Processtemplate processTemplate { get; set; }
    }

    public class Versioncontrol
    {
        public string sourceControlType { get; set; }
    }

    public class Processtemplate
    {
        public string templateTypeId { get; set; }
    }

}
