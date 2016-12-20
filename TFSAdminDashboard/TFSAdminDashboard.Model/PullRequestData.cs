using System;

namespace TFSAdminDashboard.DTO
{
    public class PullRequestData
    {
        public PullRequestData()
        {
        }

        public DateTime CreationDate { get; set; }
        public string Sourcebranch { get; set; }
        public string Status { get; set; }
        public string Targetbranch { get; set; }
        public string Title { get; set; }
    }
}