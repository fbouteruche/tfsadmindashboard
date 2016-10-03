using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class TestRunRootobject
    {
        public TestRun[] value { get; set; }
        public int count { get; set; }
    }

    public class TestRun
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public bool isAutomated { get; set; }
        public string iteration { get; set; }
        public string state { get; set; }
        public int totalTests { get; set; }
        public int incompleteTests { get; set; }
        public int notApplicableTests { get; set; }
        public int passedTests { get; set; }
        public int unanalyzedTests { get; set; }
        public int revision { get; set; }
        public string webAccessUrl { get; set; }
    }

}
