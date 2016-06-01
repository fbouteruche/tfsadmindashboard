using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class BuildRootobject
    {
        public int count { get; set; }
        public Build[] value { get; set; }
    }

    public class Build
    {
        public _Links _links { get; set; }
        public int id { get; set; }
        public string buildNumber { get; set; }
        public string status { get; set; }
        public string result { get; set; }
        public DateTime queueTime { get; set; }
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }
        public string url { get; set; }
        public Definition definition { get; set; }
        public Project project { get; set; }
        public string uri { get; set; }
        public string sourceVersion { get; set; }
        public Controller controller { get; set; }
        public string priority { get; set; }
        public string reason { get; set; }
        public Requestedfor requestedFor { get; set; }
        public Requestedby requestedBy { get; set; }
        public DateTime lastChangedDate { get; set; }
        public Lastchangedby lastChangedBy { get; set; }
        public string parameters { get; set; }
        public Logs logs { get; set; }
        public Repository repository { get; set; }
        public bool keepForever { get; set; }
        public int buildNumberRevision { get; set; }
        public string sourceBranch { get; set; }
        public Queue queue { get; set; }
        public Orchestrationplan orchestrationPlan { get; set; }
    }

    public class _Links
    {
        public Self self { get; set; }
        public Web web { get; set; }
        public Details details { get; set; }
        public Timeline timeline { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Web
    {
        public string href { get; set; }
    }

    public class Details
    {
        public string href { get; set; }
    }

    public class Timeline
    {
        public string href { get; set; }
    }

    public class Definition
    {
        public string type { get; set; }
        public DateTime createdDate { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int revision { get; set; }
        public Project project { get; set; }
    }

    public class Controller
    {
        public string uri { get; set; }
        public string status { get; set; }
        public bool enabled { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Requestedfor
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
        public bool isContainer { get; set; }
    }

    public class Requestedby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
        public bool isContainer { get; set; }
    }

    public class Lastchangedby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
        public bool isContainer { get; set; }
    }

    public class Logs
    {
        public int id { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }

    public class Repository
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string defaultBranch { get; set; }
        public object clean { get; set; }
        public bool checkoutSubmodules { get; set; }
    }

    public class Queue
    {
        public Pool pool { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Pool
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Orchestrationplan
    {
        public string planId { get; set; }
    }

}
