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
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl");

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

            return o.value.ToList();

        }

        public static string ProcessId(string collection, string processName)
        {
            var process = Processes(collection).FirstOrDefault(x => x.name == processName);

            if(process != null)
            { 
                return process.id;
            }
            else
            {
                return string.Empty;
            }
        }

        public static Process Process(string collection, string processId)
        {

            string processUrl = string.Format(Settings.Default.SingleProcessUrl, tfsServer, collection, processId);

            string json = JsonRequest.GetRestResponse(processUrl);

            Process o = JsonConvert.DeserializeObject<Process>(json);

            return o;

        }

    }
}
