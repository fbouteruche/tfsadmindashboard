using MoreLinq;
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
    public class DashTeamProjectHelper
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static ICollection<ProjectCollectionDefinition> GetCollections()
        {
            List<ProjectCollectionDefinition> collectionList = new List<ProjectCollectionDefinition>();

            var collections = DataServiceTeamProjects.Collections().Where(x => x.state == "Started");

            foreach (TeamProjectCollection currCollection in collections)
            {
                ProjectCollectionDefinition collection = new ProjectCollectionDefinition()
                {
                    InstanceId = currCollection.id,
                    Name = currCollection.name,
                    ProjectCount = DataServiceTeamProjects.Projects(currCollection.name).Count
                };

                collectionList.Add(collection);
            }

            return collectionList;
        }

        public static ICollection<ProjectDefinition> GetProjects(string collectionName)
        {
            List<ProjectDefinition> projectList = new List<ProjectDefinition>();

            foreach (TeamProject currProject in DataServiceTeamProjects.Projects(collectionName))
            {
                projectList.Add(new ProjectDefinition()
                {
                    Name = currProject.name,
                    CollectionName = currProject.collectionName
                });
            }

            return projectList;
        }

        public static ICollection<ProjectSimpleDefinition> GetAllProjectsSimple()
        {
            List<ProjectSimpleDefinition> projectList = new List<ProjectSimpleDefinition>();
            int processedColl = 0;

            string tfsUrl = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);
            string reportUrl = "https://almnet.orangeapplicationsforbusiness.com/Reports/Pages/Report.aspx?ItemPath=%2fTfsReports%2f{0}%2f{1}%2fProject+Management%2fRequirements+Overview";

            var collections = DataServiceTeamProjects.Collections().Where(x => x.state == "Started");

            foreach (TeamProjectCollection currCollection in collections)
            {
                ++processedColl;
                logger.Info("OoO Collection {0} - {1}/{2}", currCollection.name, processedColl, collections.Count());

                var collProjects = DataServiceTeamProjects.Projects(currCollection.name);
                int processed = 0;

                logger.Info("   {0} project to extract in collection {1}", collProjects.Count, currCollection.name);
                foreach (TeamProject project in collProjects)
                {
                    ++processed;
                    logger.Info("       Process {2} - {0}/{1}", processed, collProjects.Count, project.name);
                    ProjectSimpleDefinition projectDefinition = new ProjectSimpleDefinition();

                    // General data
                    projectDefinition.Name = project.name;
                    projectDefinition.Collection = currCollection.name;
                    projectDefinition.Id = project.id;
                    projectDefinition.Url = string.Format("{0}/{1}/{2}", tfsUrl, currCollection.name, project.name);
                    projectDefinition.ReportsUrl = string.Format(reportUrl, currCollection.name, project.name);

                    foreach (WorkItemType wit in DataServiceWorkItems.Types(currCollection.name, project.name))
                    {
                        if(wit.name == "User Story")
                        {
                            projectDefinition.ReportsUrl = projectDefinition.ReportsUrl.Replace("Requirements", "Stories");
                        }
                    }

                    projectDefinition.State = project.state;
                    projectDefinition.UtcCreationDate = DataServiceGit.FirstDate(currCollection.name, project.name);
                    if (projectDefinition.UtcCreationDate == DateTime.MinValue)
                    {
                        projectDefinition.UtcCreationDate = new DateTime(2015, 06, 01); //Hack hardcode to the min date for the TFS platform
                    }

                    projectDefinition.TFVCFlag = DashVersionControlHelper.isTFVC(currCollection.name, project.name);

                    var commitsData = DashGitHelper.FeedGitData(currCollection.name, project.name);

                    var branches = DashGitHelper.FeedGitBranchData(currCollection.name, project.name);

                    projectDefinition.GitBranches = branches.Select(x => x.branchname).ToDelimitedString();

                    projectDefinition.GitCommits = commitsData.Sum(x => x.TotalCommit);

                    projectDefinition.LastCommit = commitsData.OrderBy(x => x.ItemDate).First().ItemDate;

                    projectDefinition.IsActive = projectDefinition.LastCommit > DateTime.Now.AddDays(-10);

                    // get Workitems data
                    // var workitemsdata =  DashWorkItemHelper.FeedWorkItemData(currCollection.name, project.name);


                    //// get build data
                    //projectDefinition.BuildsDefinitionCollection = DashBuildHelper.FeedBuildData(currCollection.name, project.name);

                    //if (projectDefinition.BuildsDefinitionCollection.Count > 0)
                    //{
                    //    var lastSuccess = projectDefinition.BuildsDefinitionCollection.Max(x => x.LastSuccess);
                    //    var lastfail = projectDefinition.BuildsDefinitionCollection.Max(x => x.LastFail);

                    //    if (lastSuccess != DateTime.MinValue)
                    //    {
                    //        projectDefinition.LastSuccessBuild = lastSuccess;
                    //    }

                    //    if (lastfail != DateTime.MinValue)
                    //    {
                    //        projectDefinition.LastFailedBuild = lastfail;
                    //    }
                    //}


                    //// get Wit Data
                    //projectDefinition.WorkItemDefinitionCollection = DashWorkItemHelper.FeedWorkItemData(currCollection.name, project.name);

                    

                    //projectDefinition.LastCheckinDate = projectDefinition.VersionControlData.OrderByDescending(x => x.ItemDate).First().ItemDate;

                    //// get test plan Data
                    //projectDefinition.TestPlanData = DashTestPlanHelper.FeedTestPlanData(currCollection.name, project.name);

                    //projectDefinition.Platform = Environment.GetEnvironmentVariable("TfsExtractPrefix", EnvironmentVariableTarget.User);

                    //projectDefinition.DMOrigin = currCollection.name;
                    //projectDefinition.ProjectCode = project.name;

                    //projectDefinition.ExtractDate = DateTime.Now;

                    projectList.Add(projectDefinition);
#if QUICKTEST
                    //Stop after the first project in QUICKTEST mode.
                    logger.Info("QUICKTEST mode, stop after the first project");
                    break;
#endif
                }

#if TEST
                //Stop after the first collection in TEST mode.
                logger.Info("TEST mode, stop after the first collection");
                break;
#endif
            }

            return projectList.OrderBy(x => x.Name).ToList();

            return projectList;
        }

        /// <summary>
        /// Gets all projects using the REST API
        /// </summary>
        /// <returns>the collection of projects definitions</returns>
        public static ICollection<ProjectDefinition> GetAllProjects()
        {
            List<ProjectDefinition> projectList = new List<ProjectDefinition>();

            int processedColl = 0;

            var collections = DataServiceTeamProjects.Collections().Where(x => x.state == "Started");

            foreach (TeamProjectCollection currCollection in collections)
            {
                ++processedColl;
                logger.Info("OoO Collection {0} - {1}/{2}", currCollection.name, processedColl, collections.Count());

                var collProjects = DataServiceTeamProjects.Projects(currCollection.name);
                int processed = 0;

                logger.Info("   {0} project to extract in collection {1}", collProjects.Count, currCollection.name);
                foreach (TeamProject project in collProjects)
                {
                    ++processed;
                    logger.Info("       Process {2} - {0}/{1}", processed, collProjects.Count, project.name);
                    ProjectDefinition projectDefinition = new ProjectDefinition();

                    // General data
                    projectDefinition.Name = project.name;
                    projectDefinition.Id = project.id;
                    projectDefinition.CollectionDescription = currCollection.description;
                    projectDefinition.Uri = project.url;
                    projectDefinition.State = project.state;
                    projectDefinition.CollectionName = currCollection.name;
                    projectDefinition.UtcCreationDate = DataServiceGit.FirstDate(currCollection.name, project.name);

                    if(projectDefinition.UtcCreationDate == DateTime.MinValue)
                    {
                        projectDefinition.UtcCreationDate = new DateTime(2015, 06, 01); //Hack hardcode to the min date for the TFS platform
                    }

                    // get Workitems data
                    projectDefinition.WorkItemDefinitionCollection = DashWorkItemHelper.FeedWorkItemData(currCollection.name, project.name);

                    // get build data
                    projectDefinition.BuildsDefinitionCollection = DashBuildHelper.FeedBuildData(currCollection.name, project.name);

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


                    // get Wit Data
                    projectDefinition.WorkItemDefinitionCollection = DashWorkItemHelper.FeedWorkItemData(currCollection.name, project.name);

                    // get VCS data
                    projectDefinition.isGitBased = DashGitHelper.isGit(currCollection.name, project.name);
                    projectDefinition.isTFVCBased = DashVersionControlHelper.isTFVC(currCollection.name, project.name);

                    if(projectDefinition.isGitBased)
                    {
                        projectDefinition.VersionControlData = DashGitHelper.FeedGitData(currCollection.name, project.name);
                    }

                    if (projectDefinition.isTFVCBased)
                    {
                        projectDefinition.VersionControlData.AddRange(DashVersionControlHelper.FeedVersionControlData(currCollection.name, project.name));
                    }

                    projectDefinition.LastCheckinDate = projectDefinition.VersionControlData.OrderByDescending(x => x.ItemDate).First().ItemDate;

                    // get test plan Data
                    projectDefinition.TestPlanData = DashTestPlanHelper.FeedTestPlanData(currCollection.name, project.name);

                    projectDefinition.Platform = Environment.GetEnvironmentVariable("TfsExtractPrefix", EnvironmentVariableTarget.User);

                    projectDefinition.DMOrigin = currCollection.name;
                    projectDefinition.ProjectCode = project.name;

                    projectDefinition.ExtractDate = DateTime.Now;

                    projectList.Add(projectDefinition);
#if QUICKTEST
                    //Stop after the first project in QUICKTEST mode.
                    logger.Info("QUICKTEST mode, stop after the first project");
                    break;
#endif
                }

#if TEST
                //Stop after the first collection in TEST mode.
                logger.Info("TEST mode, stop after the first collection");
break;
#endif
            }
            return projectList.OrderBy(x => x.Name).ToList();
        }
    }
}
