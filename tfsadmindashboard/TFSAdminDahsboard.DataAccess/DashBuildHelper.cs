using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.DataAccess
{
    public class DashBuildHelper
    {
        public static List<BuildDefinition> FeedBuildData(TfsTeamProjectCollection tpc, string projectName)
        {
            List<BuildDefinition> buildDefinitionCollection = new List<BuildDefinition>();

            IBuildServer bs = tpc.GetService<IBuildServer>();
            IBuildDefinition[] buildDefinitions = bs.QueryBuildDefinitions(projectName, QueryOptions.All);

            foreach (IBuildDefinition buildDefinition in buildDefinitions)
            {
                IBuildDetail[] buildDetails = bs.QueryBuilds(buildDefinition);
                int failedOrPartialCount = buildDetails.Count(x => x.Status == BuildStatus.Failed || x.Status == BuildStatus.PartiallySucceeded);
                IBuildDetail lastFailedBuild = buildDetails.Where(x => x.Status == BuildStatus.Failed).OrderBy(x => x.FinishTime).LastOrDefault();
                IBuildDetail lastSucceededBuild = buildDetails.Where(x => x.Status == BuildStatus.Succeeded).OrderBy(x => x.FinishTime).LastOrDefault();
                int buildCount = buildDetails.Count();

                BuildDefinition buildDef = new BuildDefinition()
                {
                    Name = buildDefinition.Name,
                    Enabled = buildDefinition.QueueStatus == DefinitionQueueStatus.Enabled,
                    ContinuousIntegrationType = buildDefinition.ContinuousIntegrationType.ToString(),
                    FailedOrPartialRatio = failedOrPartialCount,
                    RetainedBuild = buildCount,
                    LastSuccess = lastSucceededBuild != null ? lastSucceededBuild.FinishTime : DateTime.MinValue,
                    LastFail = lastFailedBuild != null ? lastFailedBuild.FinishTime : DateTime.MinValue

                };
                buildDefinitionCollection.Add(buildDef);
            }

            return buildDefinitionCollection;
        }
    }
}
