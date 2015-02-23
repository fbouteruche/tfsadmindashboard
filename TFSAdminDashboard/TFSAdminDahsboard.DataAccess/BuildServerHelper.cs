using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.DataAccess
{
    public class BuildServerHelper
    {

        public static void FeedBuildDefinition(ICollection<BuildDefinition> collection, IBuildServer bs, string projectName)
        {


            IBuildDefinition[] buildDefinitions = bs.QueryBuildDefinitions(projectName);
            foreach (IBuildDefinition buildDefinition in buildDefinitions)
            {
                IBuildDetailSpec buildDetailSpec = bs.CreateBuildDetailSpec(buildDefinition);
                buildDetailSpec.QueryDeletedOption = QueryDeletedOption.ExcludeDeleted;
                buildDetailSpec.MaxBuildsPerDefinition = 1;
                buildDetailSpec.Reason = BuildReason.All; ;
                buildDetailSpec.Status = BuildStatus.Failed;
                buildDetailSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending;
                buildDetailSpec.MinFinishTime = DateTime.Now.AddMonths(-1);
                IBuildQueryResult overallBuildDetailsQueryResults = bs.QueryBuilds(buildDetailSpec);


                IBuildDetail[] buildDetails = bs.QueryBuilds(buildDefinition);
                int failedOrPartialCount = buildDetails.Count(x => x.Status == BuildStatus.Failed || x.Status == BuildStatus.PartiallySucceeded);
                IBuildDetail lastFailedBuild = buildDetails.Where(x => x.Status == BuildStatus.Failed).OrderBy(x => x.FinishTime).LastOrDefault();
                IBuildDetail lastSucceededBuild = buildDetails.Where(x => x.Status == BuildStatus.Succeeded).OrderBy(x => x.FinishTime).LastOrDefault();
                int buildCount = buildDetails.Count();

                BuildDefinition buildDef = new BuildDefinition()
                {
                    Name = buildDefinition.Name,
                    Enabled = buildDefinition.Enabled,
                    ContinuousIntegrationType = buildDefinition.ContinuousIntegrationType.ToString(),
                    FailedOrPartialRatio = buildCount != 0 ? failedOrPartialCount / buildCount : 0,
                    RetainedBuild = buildCount,
                    LastSuccess = lastSucceededBuild != null ? lastSucceededBuild.FinishTime : DateTime.MinValue,
                    LastFail = lastFailedBuild != null ? lastFailedBuild.FinishTime : DateTime.MinValue

                };
                collection.Add(buildDef);
            }
        }

        private static string TFSDomain;


        private static void FeedBuildMachineData(ICollection<BuildServiceHostDefinition> serverHosts, IBuildServer bs)
        {
            IBuildController[] controllers = bs.QueryBuildControllers(true);
            foreach (IBuildController controller in controllers)
            {
                BuildServiceHostDefinition buildServiceHostDefinitionForController = null;
                if (!serverHosts.Any(x => x.Name == controller.ServiceHost.Name))
                {
                    buildServiceHostDefinitionForController = new BuildServiceHostDefinition();
                    buildServiceHostDefinitionForController.Name = controller.ServiceHost.Name;
                    buildServiceHostDefinitionForController.CollectionName = bs.TeamProjectCollection.Name;
                    serverHosts.Add(buildServiceHostDefinitionForController);
                }
                else
                {
                    buildServiceHostDefinitionForController = serverHosts.FirstOrDefault(x => x.Name == controller.ServiceHost.Name);
                }

                // Get domain from collection name for rdp links
                if (string.IsNullOrEmpty(TFSDomain))
                {
                    Uri uri = new Uri(buildServiceHostDefinitionForController.CollectionName);

                    var list = uri.Host.Split('.').ToList();
                    list.RemoveAt(0); // remove hostname

                    TFSDomain = string.Join(".", list.ToArray());
                }

                BuildControllerDefinition buildControllerDefinition = new BuildControllerDefinition();
                buildControllerDefinition.Name = controller.Name;
                buildControllerDefinition.RDPUri = string.Format("rdp://{0}.{1}", buildServiceHostDefinitionForController.Name, TFSDomain);
                buildControllerDefinition.Status = controller.Status.ToString();
                buildServiceHostDefinitionForController.BuildControllers.Add(buildControllerDefinition);

                foreach (IBuildAgent agent in controller.Agents)
                {
                    BuildServiceHostDefinition buildServiceHostDefinitionForAgent = null;
                    if (!serverHosts.Any(x => x.Name == agent.ServiceHost.Name))
                    {
                        buildServiceHostDefinitionForAgent = new BuildServiceHostDefinition();
                        buildServiceHostDefinitionForAgent.Name = agent.ServiceHost.Name;
                        buildServiceHostDefinitionForAgent.CollectionName = bs.TeamProjectCollection.Name;
                        serverHosts.Add(buildServiceHostDefinitionForAgent);
                    }
                    else
                    {
                        buildServiceHostDefinitionForAgent = serverHosts.FirstOrDefault(x => x.Name == agent.ServiceHost.Name);
                    }

                    BuildAgentDefinition buildAgentDefinition = new BuildAgentDefinition();
                    buildAgentDefinition.Name = agent.Name;
                    buildAgentDefinition.Status = agent.Status.ToString();
                    buildAgentDefinition.RDPUri = string.Format("rdp://{0}.{1}", buildServiceHostDefinitionForAgent.Name, TFSDomain);
                    buildServiceHostDefinitionForAgent.BuildAgents.Add(buildAgentDefinition);
                }
            }
        }

        public static ICollection<BuildServiceHostDefinition> GetAllBuildServiceHosts(TfsTeamProjectCollection tpc)
        {
            List<BuildServiceHostDefinition> buildServiceHostCollection = new List<BuildServiceHostDefinition>();
            IBuildServer bs = tpc.GetService<IBuildServer>();
            BuildServerHelper.FeedBuildMachineData(buildServiceHostCollection, bs);
            return buildServiceHostCollection;
        }

        public static ICollection<BuildServiceHostDefinition> GetAllBuildServiceHosts(TfsConfigurationServer configurationServer)
        {
            List<BuildServiceHostDefinition> buildServiceHostCollection = new List<BuildServiceHostDefinition>();
            ITeamProjectCollectionService collectionService = configurationServer.GetService<ITeamProjectCollectionService>();
            if (collectionService != null)
            {
                IList<TeamProjectCollection> collections = collectionService.GetCollections();
                foreach (TeamProjectCollection collection in collections)
                {
                    if (collection.State == TeamFoundationServiceHostStatus.Started)
                    {
                        TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(collection.Id);
                        buildServiceHostCollection.AddRange(BuildServerHelper.GetAllBuildServiceHosts(tpc));
                    }
                }
            }
            return buildServiceHostCollection;
        }
    }
}
