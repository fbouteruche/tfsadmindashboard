using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Tool;

namespace TFSDataService
{
    public static class DataServiceVersionControl
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);

        static DataServiceVersionControl()
        {
            DataServiceBase.CheckVariables();
        }

        public static List<TFVCBranch> Branches(string collectionName)
        {
            string tfvcBranchesURL = string.Format("{0}/{1}/_apis/tfvc/branches", tfsServer, collectionName);

            string json = JsonRequest.GetRestResponse(tfvcBranchesURL);

            TFVCBranchRootobject o = JsonConvert.DeserializeObject<TFVCBranchRootobject>(json);

            return o.value.ToList();
        }

        public static bool isTFVCBased(string collectionName, string projectName)
        {
            return Branches(collectionName).Any(x => x.path.Contains(projectName));
        }

        public static Dictionary<string, List<TFVCChangeSet>> Changesets(string collectionName, string projectName)
        {
            Dictionary<string, List<TFVCChangeSet>> ans = new Dictionary<string, List<TFVCChangeSet>>();
            foreach (TFVCBranch branch in Branches(collectionName).Where(x => x.path.Contains(projectName)))
            {
                string tfvcChangeSetsURL = string.Format("{0}/{1}/_apis/tfvc/changesets?searchCriteria.itemPath={2}", tfsServer, collectionName, branch.path);

                string json = JsonRequest.GetRestResponse(tfvcChangeSetsURL);

                TFVCChangeSetRootobject o = JsonConvert.DeserializeObject<TFVCChangeSetRootobject>(json);

                ans[branch.path] = o.value.ToList();
            }

            return ans;
        }
    }
}
