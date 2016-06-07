using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class TestPlanRootobject
    {
        public TestPlan[] value { get; set; }
        public int count { get; set; }
    }

    public class TestPlan
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Project project { get; set; }
        public Area area { get; set; }
        public string iteration { get; set; }
        public object owner { get; set; }
        public int revision { get; set; }
        public string state { get; set; }
        public Rootsuite rootSuite { get; set; }
        public string clientUrl { get; set; }

        public TestSuite rootSuiteObject { get; set; }
    }

    public class Area
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Rootsuite
    {
        public string id { get; set; }
    }

}
