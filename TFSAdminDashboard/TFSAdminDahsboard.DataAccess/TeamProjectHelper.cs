using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TFSAdminDashboard.DTO;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;

namespace TFSAdminDashboard.DataAccess
{
    public class TeamProjectHelper
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets all projects using the REST API
        /// </summary>
        /// <returns>the collection of projects definitions</returns>
        public static ICollection<ProjectDefinition> GetAllProjects()
        {
            List<ProjectDefinition> projectList = new List<ProjectDefinition>();

            int processedColl = 0;

            List<TeamProjectCollection> collections = new List<TeamProjectCollection>();

            foreach (TeamProjectCollection collection in DataService.ProjectCollections().Where(x => x.state == "started"))
            {
                ++processedColl;
                logger.Info("OoO Collection {2} - {0}/{1}", processedColl, collections.Count, collection.name);

                var collProjects = DataService.TeamProjects(collection.name);
                int processed = 0;

                logger.Info("   {0} project to extract in collection {1}", collProjects.Count, collection.name);
                foreach (TeamProject project in collProjects)
                {
                    ++processed;
                    logger.Info("       Process {2} - {0}/{1}", processed, collProjects.Count, project.name);
                    ProjectDefinition projectDefinition = new ProjectDefinition();

                    // General data
                    projectDefinition.Name = project.name;
                    projectDefinition.Id = project.id;
                    projectDefinition.CollectionDescription = collection.description;
                    projectDefinition.Uri = project.url;
                    projectDefinition.State = project.state;
                    projectDefinition.CollectionName = collection.name;
                    projectDefinition.UtcCreationDate = DataService.GitFirstDate(collection.name, project.name);

                    // get Workitems data
                    projectDefinition.WorkItemDefinitionCollection = DashWorkItemHelper.FeedWorkItemData(collection.name, project.name);

                    // get build data
                    projectDefinition.BuildsDefinitionCollection = DashBuildHelper.FeedBuildData(collection.name, project.name);

                    if (projectDefinition.BuildsDefinitionCollection.Count > 0)
                    {
                        var lastSuccess = projectDefinition.BuildsDefinitionCollection.Max(x => x.LastSuccess);
                        var lastfail = projectDefinition.BuildsDefinitionCollection.Max(x => x.LastFail);

                        if (lastSuccess != DateTime.MinValue)
                        {
                            projectDefinition.LastSuccessBuild = lastSuccess;
                        }

                        if (lastfail != DateTime.MinValue)
                        {
                            projectDefinition.LastFailedBuild = lastfail;
                        }
                    }

                    // get VCS data
                    projectDefinition.VersionControlData = DashGitHelper.FeedGitData(collection.name, project.name);
                    projectDefinition.LastCheckinDate = projectDefinition.VersionControlData.Max(x => x.InnerLastCheckIn);

                    // get test plan Data
                    projectDefinition.TestPlanData = DashTestPlanHelper.FeedTestPlanData(collection.name, project.name);

                    projectDefinition.Platform = "TFS2013";

                    projectDefinition.DMOrigin = collection.name;
                    projectDefinition.ProjectCode = project.name;

                    projectDefinition.ExtractDate = DateTime.Now;
                }
            }
            return projectList.OrderBy(x => x.Name).ToList();
        }
    }
}
