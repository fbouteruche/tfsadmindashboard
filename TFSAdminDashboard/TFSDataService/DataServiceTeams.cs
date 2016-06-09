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
    public static class DataServiceTeams
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);

        public static List<Team> List(string collection, string projectName)
        {
            string teamscUrl = string.Format("{0}/{1}/_apis/projects/{2}/teams?api-version=1.0", tfsServer, collection, projectName);

            string json = JsonRequest.GetRestResponse(teamscUrl);

            TeamRootobject o = JsonConvert.DeserializeObject<TeamRootobject>(json);

            return o.value.ToList();
        }

        public static List<TeamMember> DefaultMembers(string collection, string projectName)
        {
            Team defaultTeam = List(collection, projectName).FirstOrDefault(x => x.name == projectName);

            if (defaultTeam != null)
            {
                string teamMembersUrl = string.Format("{0}/{1}/_apis/projects/{2}/teams/{3}/members", tfsServer, collection, projectName, defaultTeam.id);

                string json = JsonRequest.GetRestResponse(teamMembersUrl);
                TeamMemberRootobject o = JsonConvert.DeserializeObject<TeamMemberRootobject>(json);

                return o.value.ToList();
            }
            else
            {
                return new List<TeamMember>();
            }
        }
    }
}
