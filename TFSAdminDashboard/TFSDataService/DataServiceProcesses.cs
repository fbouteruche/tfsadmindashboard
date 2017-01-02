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

        /// <summary>
        /// List the process templates available in a collection
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static List<Process> Processes(string collection)
        {
            List<Process> processes = new List<Process>();

            string processesUrl = string.Format(Settings.Default.ProcessesUrl, tfsServer, collection);

            string json = JsonRequest.GetRestResponse(processesUrl);

            ProcessRootobject o = JsonConvert.DeserializeObject<ProcessRootobject>(json);

            return o.value.ToList();

        }

        /// <summary>
        /// Get the process ID from a process name
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="processName"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a process from it's ID
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="processId"></param>
        /// <returns></returns>
        public static Process Process(string collection, string processId)
        {

            string processUrl = string.Format(Settings.Default.SingleProcessUrl, tfsServer, collection, processId);

            string json = JsonRequest.GetRestResponse(processUrl);

            Process o = JsonConvert.DeserializeObject<Process>(json);

            return o;

        }

    }
}
