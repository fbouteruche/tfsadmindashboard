using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;
using TFSDataService;

namespace TFSAdminDashboard.DataAccess
{
    public class DashBuildHelper
    {
        public static List<BuildDefinition> FeedBuildData(string collectionName, string projectName)
        {
            List<BuildDefinition> buildDefinitionCollection = new List<BuildDefinition>();

            foreach (TFSDataService.JsonBusinessObjects.BuildDefinition def in DataService.BuildDefinitions(collectionName, projectName))
            {
                BuildDefinition buildDef = new BuildDefinition()
                {
                    Name = def.name,
                    type = def.type
                };

                var builds = DataService.Builds(collectionName, projectName).Where(x => x.definition.name == def.name).OrderByDescending(x => x.finishTime);

                var success = builds.FirstOrDefault(x => x.result == "succeeded");
                if (success != null)
                    buildDef.LastSuccess = success.finishTime;
                else
                    buildDef.LastSuccess = DateTime.MinValue;

                var fail = builds.FirstOrDefault(x => x.result == "failed");
                if (fail != null)
                    buildDef.LastFail = fail.finishTime;
                else
                    buildDef.LastFail = DateTime.MinValue;

                buildDef.Health = builds.Take(5).Count(x => x.result == "succeeded") * 20;

                buildDefinitionCollection.Add(buildDef);
            }

            return buildDefinitionCollection;
        }
    }
}
