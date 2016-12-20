using MoreLinq;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Properties;
using TFSDataService.Tool;

namespace TFSDataService
{
    public static class DataServiceTeams
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl");
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static DataServiceTeams()
        {
            DataServiceBase.CheckVariables();
        }

        public static List<Team> List(string collection, string projectName)
        {
            string teamscUrl = string.Format(Settings.Default.TeamUrl, tfsServer, collection, projectName);

            string json = JsonRequest.GetRestResponse(teamscUrl);

            TeamRootobject o = JsonConvert.DeserializeObject<TeamRootobject>(json);

            return o.value.ToList();
        }

        public static List<TeamMember> Members(string collection, string projectName)
        {
            List < Team >  teams = List(collection, projectName);

            List<TeamMember> ans = new List<TeamMember>();

            foreach(Team t in teams)
            {
                string surfaceteamMembersUrl = string.Format(Settings.Default.TeamMemberUrl, tfsServer, collection, projectName, t.id);

                string json = JsonRequest.GetRestResponse(surfaceteamMembersUrl);
                SurfaceTeamMemberRootobject o = JsonConvert.DeserializeObject<SurfaceTeamMemberRootobject>(json);

                foreach(SurfaceTeamMember surfObject in o.value)
                {
                    string json2 = JsonRequest.GetRestResponse(surfObject.url);

                    TeamMember member = JsonConvert.DeserializeObject<TeamMember>(json2);
                    ans.Add(member);
                }
            }

            return ans.DistinctBy(x => x.DisplayName).ToList();
        }
    }
}
