
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using TFSAdminDashboard.DTO;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;

namespace TFSAdminDashboard.DataAccess
{
    public class DashBuildHelper
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static List<Build_Definition> FeedBuildData(string collectionName, string projectName)
        {
            List<Build_Definition> buildDefinitionCollection = new List<Build_Definition>();

            foreach (BuildDefinition def in DataServiceBuild.BuildDefinitions(collectionName, projectName))
            {
                Build_Definition buildDef = new Build_Definition()
                {
                    Name = def.name,
                    type = def.type
                };

                var builds = DataServiceBuild.Builds(collectionName, projectName).Where(x => x.definition.name == def.name).OrderByDescending(x => x.finishTime);

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

                double succeeded = builds.Take(5).Count(x => x.result == "succeeded");
                int buildCount = builds.Count() >= 5 ? 5 : builds.Count();
                if (builds.Any())
                    buildDef.Health = (int)(succeeded / buildCount * 100);
                else
                    buildDef.Health = 0;

                buildDefinitionCollection.Add(buildDef);
            }

            return buildDefinitionCollection;
        }

        public static ICollection<BuildRun> GetAllBuilds()
        {

            int processedColl = 0;
            int processed = 0;

            List<BuildRun> builds = new List<BuildRun>();

            var collections = DataServiceTeamProjects.Collections().Where(x => x.state == "Started");

            foreach (TeamProjectCollection currCollection in collections)
            {
                ++processedColl;
                processed = 0;
                logger.Info("OoO Collection {0} - {1}/{2}", currCollection.name, processedColl, collections.Count());

                var collProjects = DataServiceTeamProjects.Projects(currCollection.name);

                logger.Info("   {0} project to process in collection {1}", collProjects.Count, currCollection.name);

                foreach (TeamProject project in collProjects)
                {
                    ++processed;

                    if(processed % 10 == 0)
                        logger.Info("  project  {0} / {1}", processed, collProjects.Count);

                    // Consider only yesterday's build
                    var JSonbuilds = DataServiceBuild.Builds(currCollection.name, project.name).Where(x => x.queueTime.Date == DateTime.Now.AddDays(-1).Date);

                    foreach (Build buildRun in JSonbuilds)
                    {
                        BuildRun b = new BuildRun();

                        builds.Add(new BuildRun()
                        {
                            startTime = buildRun.startTime,
                            queueTime = buildRun.queueTime,
                            finishTime = buildRun.finishTime,
                            duration = (buildRun.finishTime - buildRun.startTime).Milliseconds,
                            latency = (buildRun.startTime - buildRun.queueTime).Milliseconds,
                            projectName = currCollection.name + "/" + project.name,
                            buildName = buildRun.definition.name,
                            buildNumber = buildRun.buildNumber,
                            workerName = DataServiceBuild.getWorkerName(buildRun)
                        });
                    }
                }
            }

            return builds;

        }

    }
}
