using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class TestSuite
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Project project { get; set; }
        public Plan plan { get; set; }
        public int revision { get; set; }
        public int testCaseCount { get; set; }
        public string suiteType { get; set; }
        public string testCasesUrl { get; set; }
        public bool inheritDefaultConfigurations { get; set; }
        public Defaultconfiguration[] defaultConfigurations { get; set; }
        public string state { get; set; }
        public Lastupdatedby lastUpdatedBy { get; set; }
        public DateTime lastUpdatedDate { get; set; }
    }

    public class Plan
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Lastupdatedby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Defaultconfiguration
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}
