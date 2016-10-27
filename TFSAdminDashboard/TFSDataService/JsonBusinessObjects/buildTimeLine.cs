using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class BuildTimeLine
    {
        public Record[] records { get; set; }
        public string lastChangedBy { get; set; }
        public DateTime lastChangedOn { get; set; }
        public string id { get; set; }
        public int changeId { get; set; }
        public string url { get; set; }
    }

    public class Record
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }
        public object currentOperation { get; set; }
        public int? percentComplete { get; set; }
        public string state { get; set; }
        public string result { get; set; }
        public object resultCode { get; set; }
        public int changeId { get; set; }
        public DateTime lastModified { get; set; }
        public string workerName { get; set; }
        public int order { get; set; }
        public tDetails details { get; set; }
        public int errorCount { get; set; }
        public int warningCount { get; set; }
        public object url { get; set; }
        public Log log { get; set; }
        public Issue[] issues { get; set; }
    }

    public class tDetails
    {
        public string id { get; set; }
        public int changeId { get; set; }
        public string url { get; set; }
    }

    public class Log
    {
        public int id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Issue
    {
        public string type { get; set; }
        public string category { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class Data
    {
        public string type { get; set; }
        public string code { get; set; }
    }

}
