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
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static List<Team> List(string collection, string projectName)
        {
            string teamscUrl = string.Format(Settings.Default.TeamUrl, tfsServer, collection, projectName);

            string json = JsonRequest.GetRestResponse(teamscUrl);

            TeamRootobject o = JsonConvert.DeserializeObject<TeamRootobject>(json);

            return o.value.ToList();
        }

        public static List<TeamMember> DefaultMembers(string collection, string projectName)
        {
            Team defaultTeam = List(collection, projectName).FirstOrDefault(x => x.name.Contains(projectName));
            List<TeamMember> ans = new List<TeamMember>();

            if (defaultTeam != null)
            {
                string surfaceteamMembersUrl = string.Format(Settings.Default.TeamMemberUrl, tfsServer, collection, projectName, defaultTeam.id);

                string json = JsonRequest.GetRestResponse(surfaceteamMembersUrl);
                SurfaceTeamMemberRootobject o = JsonConvert.DeserializeObject<SurfaceTeamMemberRootobject>(json);

                foreach(SurfaceTeamMember surfObject in o.value)
                {
                    string json2 = JsonRequest.GetRestResponse(surfObject.url);

                    TeamMember member = JsonConvert.DeserializeObject<TeamMember>(json2);
                    ans.Add(member);
                }
            }
            else
            {
                logger.Warn("No default team found for {0}/{1}", collection, projectName);
            }

            return ans;
        }
    }
}
