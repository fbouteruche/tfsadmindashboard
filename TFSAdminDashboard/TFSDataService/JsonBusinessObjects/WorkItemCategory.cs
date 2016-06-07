using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class WorkItemTypesRootobject
    {
        public int count { get; set; }
        public WorkItemType[] value { get; set; }
    }

    public class WorkItemType
    {
        public string name { get; set; }
        public string description { get; set; }
        public string xmlForm { get; set; }
        public Fieldinstance[] fieldInstances { get; set; }
        public Transitions transitions { get; set; }
        public string url { get; set; }
    }

    public class Transitions
    {
        public Resolved[] Resolved { get; set; }
        public Proposed[] Proposed { get; set; }
        public Active[] Active { get; set; }
        public Closed[] Closed { get; set; }
        public Child[] _ { get; set; }
        public Requested[] Requested { get; set; }
        public Accepted[] Accepted { get; set; }
        public Ready[] Ready { get; set; }
        public Design[] Design { get; set; }
        public Inactive[] Inactive { get; set; }
        public InProgress[] InProgress { get; set; }
        public InPlanning[] InPlanning { get; set; }
        public Completed[] Completed { get; set; }
    }

    public class Resolved
    {
        public string to { get; set; }
    }

    public class Proposed
    {
        public string to { get; set; }
        public string[] actions { get; set; }
    }

    public class Active
    {
        public string to { get; set; }
        public string[] actions { get; set; }
    }

    public class Closed
    {
        public string to { get; set; }
        public string[] actions { get; set; }
    }

    public class Child
    {
        public string to { get; set; }
    }

    public class Requested
    {
        public string to { get; set; }
        public string[] actions { get; set; }
    }

    public class Accepted
    {
        public string to { get; set; }
        public string[] actions { get; set; }
    }

    public class Ready
    {
        public string to { get; set; }
    }

    public class Design
    {
        public string to { get; set; }
    }

    public class Inactive
    {
        public string to { get; set; }
    }

    public class InProgress
    {
        public string to { get; set; }
    }

    public class InPlanning
    {
        public string to { get; set; }
    }

    public class Completed
    {
        public string to { get; set; }
    }

    public class Fieldinstance
    {
        public string helpText { get; set; }
        public string referenceName { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

}
