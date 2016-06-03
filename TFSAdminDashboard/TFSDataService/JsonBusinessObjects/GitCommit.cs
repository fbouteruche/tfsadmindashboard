using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{
    public class GitCommitRootobject
    {
        public int count { get; set; }
        public GitCommit[] value { get; set; }
    }

    public class GitCommit
    {
        public string commitId { get; set; }
        public Committer author { get; set; }
        public Committer committer { get; set; }
        public string comment { get; set; }
        public Changecounts changeCounts { get; set; }
        public string url { get; set; }
        public string remoteUrl { get; set; }
    }

    public class Committer
    {
        public string name { get; set; }
        public string email { get; set; }
        public DateTime date { get; set; }
    }

    public class Changecounts
    {
        public int Add { get; set; }
        public int Edit { get; set; }
    }
}

