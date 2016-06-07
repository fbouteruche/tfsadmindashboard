using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{


    public class WorkItem
    {
        public int id { get; set; }
        public int rev { get; set; }
        public Fields fields { get; set; }
        public _Linkswit _links { get; set; }
        public string url { get; set; }
    }

    public class Fields
    {
        [JsonProperty(PropertyName = "System.AreaPath")]
        public string SystemAreaPath { get; set; }
        [JsonProperty(PropertyName = "System.TeamProject")]
        public string SystemTeamProject { get; set; }
        [JsonProperty(PropertyName = "System.IterationPath")]
        public string SystemIterationPath { get; set; }
        [JsonProperty(PropertyName = "System.WorkItemType")]
        public string SystemWorkItemType { get; set; }
        [JsonProperty(PropertyName = "System.State")]
        public string SystemState { get; set; }
        [JsonProperty(PropertyName = "System.Reason")]
        public string SystemReason { get; set; }
        [JsonProperty(PropertyName = "System.AssignedTo")]
        public string SystemAssignedTo { get; set; }
        [JsonProperty(PropertyName = "System.CreatedDate")]
        public DateTime SystemCreatedDate { get; set; }
        [JsonProperty(PropertyName = "System.CreatedBy")]
        public string SystemCreatedBy { get; set; }
        [JsonProperty(PropertyName = "System.ChangedDate")]
        public DateTime SystemChangedDate { get; set; }
        [JsonProperty(PropertyName = "System.ChangedBy")]
        public string SystemChangedBy { get; set; }
        [JsonProperty(PropertyName = "System.Title")]
        public string SystemTitle { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Scheduling.RemainingWork")]
        public float MicrosoftVSTSSchedulingRemainingWork { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.CMMI.Blocked")]
        public string MicrosoftVSTSCMMIBlocked { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Severity")]
        public string MicrosoftVSTSCommonSeverity { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.StateChangeDate")]
        public DateTime MicrosoftVSTSCommonStateChangeDate { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ActivatedDate")]
        public DateTime MicrosoftVSTSCommonActivatedDate { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ActivatedBy")]
        public string MicrosoftVSTSCommonActivatedBy { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Priority")]
        public int MicrosoftVSTSCommonPriority { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ResolvedDate")]
        public DateTime MicrosoftVSTSCommonResolvedDate { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ResolvedBy")]
        public string MicrosoftVSTSCommonResolvedBy { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ResolvedReason")]
        public string MicrosoftVSTSCommonResolvedReason { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.Triage")]
        public string MicrosoftVSTSCommonTriage { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.CMMI.RootCause")]
        public string MicrosoftVSTSCMMIRootCause { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.Common.ValueArea")]
        public string MicrosoftVSTSCommonValueArea { get; set; }
        [JsonProperty(PropertyName = "Microsoft.VSTS.TCM.ReproSteps")]
        public string MicrosoftVSTSTCMReproSteps { get; set; }
    }

    public class _Linkswit
    {
        public Self self { get; set; }
        public Workitemupdates workItemUpdates { get; set; }
        public Workitemrevisions workItemRevisions { get; set; }
        public Workitemhistory workItemHistory { get; set; }
        public Html html { get; set; }
        public Workitemtype workItemType { get; set; }
        public Fields1 fields { get; set; }
    }

    public class Workitemupdates
    {
        public string href { get; set; }
    }

    public class Workitemrevisions
    {
        public string href { get; set; }
    }

    public class Workitemhistory
    {
        public string href { get; set; }
    }

    public class Html
    {
        public string href { get; set; }
    }

    public class Workitemtype
    {
        public string href { get; set; }
    }

    public class Fields1
    {
        public string href { get; set; }
    }


}
