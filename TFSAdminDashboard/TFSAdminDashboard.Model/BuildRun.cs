using System;

namespace TFSAdminDashboard.DTO
{
    public class BuildRun
    {
        public string buildName;
        public string buildNumber;
        public int duration;
        public DateTime finishTime;
        public int latency;
        public string projectName;
        public DateTime queueTime;
        public DateTime startTime;
        public string workerName;
    }
}