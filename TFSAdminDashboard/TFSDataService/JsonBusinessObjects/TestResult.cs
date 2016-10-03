using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class TestResultRootobject
    {
        public TestResult[] value { get; set; }
        public int count { get; set; }
    }

    public class TestResult
    {
        public int id { get; set; }
        public TestConfiguration configuration { get; set; }
        public Project project { get; set; }
        public DateTime startedDate { get; set; }
        public DateTime completedDate { get; set; }
        public float durationInMs { get; set; }
        public string outcome { get; set; }
        public Owner owner { get; set; }
        public int revision { get; set; }
        public Runby runBy { get; set; }
        public string state { get; set; }
        public Testcase testCase { get; set; }
        public Testrun testRun { get; set; }
        public DateTime lastUpdatedDate { get; set; }
        public Lastupdatedby lastUpdatedBy { get; set; }
        public int priority { get; set; }
        public DateTime createdDate { get; set; }
        public string url { get; set; }
        public string failureType { get; set; }
        public Area area { get; set; }
        public string testCaseTitle { get; set; }
        public object[] customFields { get; set; }
    }

    public class TestConfiguration
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Runby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Testcase
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Testrun
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

   
    public class Area
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

}
