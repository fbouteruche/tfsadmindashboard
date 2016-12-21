using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class PullRequestRootobject
    {
        public PullRequest[] value { get; set; }
        public int count { get; set; }
    }

    public class PullRequest
    {
        public Repository1 repository { get; set; }
        public int pullRequestId { get; set; }
        public string status { get; set; }
        public Createdby createdBy { get; set; }
        public DateTime creationDate { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string sourceRefName { get; set; }
        public string targetRefName { get; set; }
        public string mergeStatus { get; set; }
        public string mergeId { get; set; }
        public Lastmergesourcecommit lastMergeSourceCommit { get; set; }
        public Lastmergetargetcommit lastMergeTargetCommit { get; set; }
        public Lastmergecommit lastMergeCommit { get; set; }
        public Reviewer[] reviewers { get; set; }
        public string url { get; set; }
        public _Links1 _links { get; set; }
    }

    public class Repository1
    {
        public string id { get; set; }
        public string url { get; set; }
    }

    public class Createdby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Lastmergesourcecommit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class Lastmergetargetcommit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class Lastmergecommit
    {
        public string commitId { get; set; }
        public string url { get; set; }
    }

    public class _Links1
    {
        public Self self { get; set; }
        public Repository2 repository { get; set; }
        public Workitems workItems { get; set; }
        public Sourcebranch sourceBranch { get; set; }
        public Targetbranch targetBranch { get; set; }
        public Sourcecommit sourceCommit { get; set; }
        public Targetcommit targetCommit { get; set; }
        public Createdby1 createdBy { get; set; }
    }


    public class Repository2
    {
        public string href { get; set; }
    }

    public class Workitems
    {
        public string href { get; set; }
    }

    public class Sourcebranch
    {
        public string href { get; set; }
    }

    public class Targetbranch
    {
        public string href { get; set; }
    }

    public class Sourcecommit
    {
        public string href { get; set; }
    }

    public class Targetcommit
    {
        public string href { get; set; }
    }

    public class Createdby1
    {
        public string href { get; set; }
    }

    public class Reviewer
    {
        public string reviewerUrl { get; set; }
        public int vote { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

}
