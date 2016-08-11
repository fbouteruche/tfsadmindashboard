using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Properties;
using TFSDataService.Tool;

namespace TFSDataService
{
    public static class DataServiceProcesses
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);

        static DataServiceProcesses()
        {
            DataServiceBase.CheckVariables();
        }

        public static List<Process> Processes(string collection)
        {
            List<Process> processes = new List<Process>();

            string processesUrl = string.Format(Settings.Default.ProcessesUrl, tfsServer, collection);

            string json = JsonRequest.GetRestResponse(processesUrl);

            ProcessRootobject o = JsonConvert.DeserializeObject<ProcessRootobject>(json);

            return o?.value.ToList();

        }

        //public static List<TeamProject> Projects(string collection)
        //{
        //    string tpcUrl = string.Format(Settings.Default.TeamProjectUrl, tfsServer, collection);

        //    string json = JsonRequest.GetRestResponse(tpcUrl);

        //    TeampProjectRootobject o = JsonConvert.DeserializeObject<TeampProjectRootobject>(json);

        //    return o.value.ToList();
        //}

        //public static List<TeamProject> AllProjects()
        //{
        //    List<TeamProject> ans = new List<TeamProject>();

        //    foreach (var collection in Collections())
        //    {
        //        foreach(TeamProject p in Projects(collection.name))
        //        {
        //            p.collectionName = collection.name;
        //            ans.Add(p);
        //        }
        //    }

        //    return ans;
        //}
    }
}
