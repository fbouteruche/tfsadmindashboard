using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Tool;

namespace TFSDataService
{
    public static class DataServiceWorkItems
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl");

        static DataServiceWorkItems()
        {
            DataServiceBase.CheckVariables();
        }

        public static List<WorkItemType> Types(string collectionName, string projectName)
        {
            string witTypesURL = string.Format("{0}/{1}/{2}/_apis/wit/workItemTypes", tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(witTypesURL);

            WorkItemTypesRootobject o = JsonConvert.DeserializeObject<WorkItemTypesRootobject>(json);

            return o.value.ToList();
        }

        public static Dictionary<string, int> States(string collectionName, string projectName, string workitemType)
        {
            Dictionary<string, int> ans = new Dictionary<string, int>();

            string witQueryURL = string.Format("{0}/{1}/{2}/_apis/wit/wiql?api-version=1.0", tfsServer, collectionName, projectName);
            WiQuery q = new WiQuery()
            {
                query = string.Format("SELECT [System.Id],[System.Title],[System.State] FROM WorkItems WHERE [System.TeamProject] = '{0}' AND [System.WorkItemType] = '{1}'", projectName, workitemType)
            };

            string json = JsonRequest.PostData(witQueryURL, JsonConvert.SerializeObject(q));

            wiQueryResult queryResult = JsonConvert.DeserializeObject<wiQueryResult>(json);

            foreach (queryWorkitem wi in queryResult.workItems)
            {
                string witJson = JsonRequest.GetRestResponse(wi.url);

                WorkItem workitem = JsonConvert.DeserializeObject<WorkItem>(witJson);

                if (ans.ContainsKey(workitem.fields.SystemState))
                {
                    ans[workitem.fields.SystemState] += 1;
                }
                else
                {
                    ans[workitem.fields.SystemState] = 1;
                }
            }

            return ans;
        }
    }
}
