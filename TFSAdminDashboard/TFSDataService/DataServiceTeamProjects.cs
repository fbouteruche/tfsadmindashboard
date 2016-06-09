using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Tool;

namespace TFSDataService
{
    public static class DataServiceTeamProjects
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);

        public static List<TeamProjectCollection> Collections()
        {
            List<TeamProjectCollection> ans = new List<TeamProjectCollection>();

            string surfacetpcUrl = string.Format("{0}/_apis/projectcollections", tfsServer);

            string json = JsonRequest.GetRestResponse(surfacetpcUrl);

            SurfaceTeamProjectCollectionRootobject o = JsonConvert.DeserializeObject<SurfaceTeamProjectCollectionRootobject>(json);

            foreach (SurfaceTeamProjectCollection stpc in o.value.ToList())
            {

                string tpcUrl = string.Format("{0}/_apis/projectcollections/{1}", tfsServer, stpc.id);
                string json2 = JsonRequest.GetRestResponse(tpcUrl);

                ans.Add(JsonConvert.DeserializeObject<TeamProjectCollection>(json2));
            }

            return ans;
        }

        public static List<TeamProject> Projects(string collection)
        {
            string tpcUrl = string.Format("{0}/{1}/_apis/projects?api-version=1.0", tfsServer, collection);

            string json = JsonRequest.GetRestResponse(tpcUrl);

            TeampProjectRootobject o = JsonConvert.DeserializeObject<TeampProjectRootobject>(json);

            return o.value.ToList();
        }

        public static List<TeamProject> AllProjects()
        {
            List<TeamProject> ans = new List<TeamProject>();

            foreach (var collection in Collections())
            {
                foreach(TeamProject p in Projects(collection.name))
                {
                    p.collectionName = collection.name;
                    ans.Add(p);
                }
            }

            return ans;
        }
    }
}
