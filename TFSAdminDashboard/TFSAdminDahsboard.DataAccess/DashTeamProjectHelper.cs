﻿using MoreLinq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

        static int processedColl = 0;
        static int processed = 0;
        static List<ProjectSimpleDefinition> projectList = new List<ProjectSimpleDefinition>();

#if QUICKTEST
        static bool quicktested = false;
#endif
        public static ICollection<ProjectSimpleDefinition> GetAllProjectsSimple()
        {
            string tfsUrl = Environment.GetEnvironmentVariable("TfsUrl");
            string reportUrl = "https://almnet.orangeapplicationsforbusiness.com/Reports/Pages/Report.aspx?ItemPath=%2fTfsReports%2f{0}%2f{1}%2fProject+Management%2fRequirements+Overview";

            var collections = DataServiceTeamProjects.Collections().Where(x => x.state == "Started");

            foreach (TeamProjectCollection currCollection in collections)
            {
#if QUICKTEST
                if (currCollection.name != Environment.GetEnvironmentVariable("QuickTestCollection"))
                    continue;

                logger.Info("QUICKTEST mode, consider only the {0} collection", Environment.GetEnvironmentVariable("QuickTestCollection"));
#endif
                ++processedColl;
                processed = 0;
                logger.Info("OoO Collection {0} - {1}/{2}", currCollection.name, processedColl, collections.Count());

                var collProjects = DataServiceTeamProjects.Projects(currCollection.name);

                logger.Info("   {0} project to extract in collection {1}", collProjects.Count, currCollection.name);

                foreach (TeamProject project in collProjects)
                {
                    ExtractInfos(projectList, tfsUrl, reportUrl, currCollection, collProjects, project);
                };

#if QUICKTEST
                if (quicktested)
                {
                    logger.Info("TEST mode, stop after the first collection");
                    break;
                }
#endif
            }

            logger.Info("Extract done");
            return projectList.OrderBy(x => x.Name).ToList();
        }

        private static void ExtractInfos(List<ProjectSimpleDefinition> projectList, string tfsUrl, string reportUrl, TeamProjectCollection currCollection, List<TeamProject> collProjects, TeamProject project)
        {
#if QUICKTEST
            if (project.name != Environment.GetEnvironmentVariable("QuickTestProject"))
                return;

            logger.Info("QUICKTEST mode, consider only the {0} project", Environment.GetEnvironmentVariable("QuickTestProject"));
            quicktested = true;
#endif
            ++processed;
            logger.Info("       Process {2} - {0}/{1}", processed, collProjects.Count, project.name);
            ProjectSimpleDefinition projectDefinition = new ProjectSimpleDefinition();

            // General data
            logger.Trace("General Data");
            projectDefinition.Name = string.Format("{0}/{1}", currCollection.name, project.name);
            projectDefinition.Platform = "TFSOAB";

            projectDefinition.Collection = currCollection.name;
            projectDefinition.Id = project.id;
            projectDefinition.Url = string.Format("{0}/{1}/{2}", tfsUrl, currCollection.name, project.name);
            projectDefinition.ReportsUrl = string.Format(reportUrl, currCollection.name, project.name);

            // DM
            switch (currCollection.name)
            {
                case "AlsyMig":
                    projectDefinition.DM = "AD";
                    break;
                case "DT":
                case "Formations":
                case "TestMigration":
                case "TF15":
                    projectDefinition.DM = "DPO";
                    break;
                default:
                    projectDefinition.DM = currCollection.name;
                    break;
            }

            //Report link
            foreach (WorkItemType wit in DataServiceWorkItems.Types(currCollection.name, project.name))
            {
                if (wit.name == "User Story")
                {
                    projectDefinition.ReportsUrl = projectDefinition.ReportsUrl.Replace("Requirements", "Stories");
                }
            }

            // Creation date
            projectDefinition.State = project.state;
            projectDefinition.UtcCreationDate = DataServiceGit.FirstDate(currCollection.name, project.name);
            if (projectDefinition.UtcCreationDate == DateTime.MinValue)
            {
                projectDefinition.UtcCreationDate = new DateTime(2015, 06, 01); //Hack hardcode to the min date for the TFS platform
            }

            projectDefinition.TFVCFlag = DashVersionControlHelper.isTFVC(currCollection.name, project.name);

            logger.Trace("Git Data");
            var commitsData = DashGitHelper.FeedGitData(currCollection.name, project.name);
            var branches = DashGitHelper.FeedGitBranchData(currCollection.name, project.name);

            var repositories = commitsData.Select(x => new GitData()
            {
                Name = x.Repository,
                DefaultBranch = x.DefaultBranch,
                Branches = branches.Where(y => y.repo == x.Repository).Select(z => new BranchData()
                {
                    Name = z.name
                }).ToList(),
                MasterCommitsYesterday = commitsData.Where(w => w.Repository == x.Repository).Sum(v => v.TotalMasterCommit),
                Tags = DashGitHelper.FeedGitTagData(currCollection.name, project.name, x.Repository).Select(b => new TagData()
                {
                    Name = b.tagname,
                }).ToList(),
                PullRequests = DashGitHelper.FeedPullRequestData(currCollection.name, project.name, x.Repository).Select(c => new PullRequestData()
                {
                    Title = c.title,
                    Status = c.status,
                    Sourcebranch = c.sourceRefName.Substring("refs/heads/".Length),
                    Targetbranch = c.targetRefName.Substring("refs/heads/".Length),
                    CreationDate = c.creationDate
                }).ToList()
            });


            projectDefinition.GitCommitsYesterday = commitsData.Sum(x => x.TotalMasterCommit);

            projectDefinition.LastCommit = commitsData.OrderByDescending(x => x.ItemDate).First().ItemDate;

            projectDefinition.Repositories = repositories.ToList();

            projectDefinition.LastCommit = commitsData.OrderByDescending(x => x.ItemDate).First().ItemDate;

            projectDefinition.IsActive = projectDefinition.LastCommit > DateTime.Now.AddDays(-10);

            // get build data
            logger.Trace("Build Data");
            var buildData = DashBuildHelper.FeedBuildData(currCollection.name, project.name);

            projectDefinition.BuildDefinitionNumber = buildData.Count;

            if (buildData.Count > 0)
            {
                projectDefinition.BuildHealth = buildData.Average(x => x.Health);

                projectDefinition.XamlRatio = (double)buildData.Count(x => x.type == "xaml") / buildData.Count;

                var owaspBuilds = buildData.Where(x => x.UsesDependencyCheck);

                if (owaspBuilds.Any())
                {
                    projectDefinition.OwaspDependencyCheckBuildDefinitions = owaspBuilds.Count();
                    projectDefinition.OwaspDependencyCheckLastSuccess = owaspBuilds.OrderByDescending(x => x.LastSuccess).First().LastSuccess;
                }

                projectDefinition.LastBuildOK = buildData.OrderByDescending(x => x.LastSuccess).First().LastSuccess;
                projectDefinition.LastBuildKO = buildData.OrderByDescending(x => x.LastFail).First().LastFail;
            }

            // get Workitems data
            logger.Trace("WorkItems Data");
            var workitemsdata = DashWorkItemHelper.FeedWorkItemData(currCollection.name, project.name);

            projectDefinition.WorkItemModifYesterday = workitemsdata.modifsYesterday;
            projectDefinition.LastWorkItemModif = workitemsdata.lastmodif;

            projectDefinition.WorkItemNumber = workitemsdata.workItemDefinitionCollection.Sum(x => x.TotalNumber);

            double closednumber = workitemsdata.workItemDefinitionCollection.Sum(x => x.ClosedNumber);
            projectDefinition.WorkItemHealth = closednumber / projectDefinition.WorkItemNumber;



            // get test plan Data
            logger.Trace("TestPlan Data");
            int test_number = 0;
            var testResults = DataServiceTests.RunResults(currCollection.name, project.name);

            int testyesterday = 0;
            DateTime lastTestResult = new DateTime(2015, 06, 01);

            foreach(var result in testResults)
            {
                if (result.completedDate > lastTestResult)
                    lastTestResult = result.completedDate;
                if(DateTime.Now - result.completedDate < new TimeSpan(1,0,0))
                {
                    testyesterday += 1;
                }
            }

            projectDefinition.TestHealth = DashTestPlanHelper.GetTestResultsRatio(currCollection.name, project.name, workitemsdata.workItemDefinitionCollection, ref test_number, testResults);

            projectDefinition.TestPassedYesterday = testyesterday;
            projectDefinition.LastTestResult = lastTestResult;

            projectDefinition.TestNumber = test_number;

            projectList.Add(projectDefinition);
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

                    if (projectDefinition.UtcCreationDate == DateTime.MinValue)
                    {
                        projectDefinition.UtcCreationDate = new DateTime(2015, 06, 01); //Hack hardcode to the min date for the TFS platform
                    }

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
                    projectDefinition.WorkItemDefinitionCollection = DashWorkItemHelper.FeedWorkItemData(currCollection.name, project.name).workItemDefinitionCollection;

                    // get VCS data
                    projectDefinition.isGitBased = DashGitHelper.isGit(currCollection.name, project.name);
                    projectDefinition.isTFVCBased = DashVersionControlHelper.isTFVC(currCollection.name, project.name);

                    if (projectDefinition.isGitBased)
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

                    projectDefinition.Platform = Environment.GetEnvironmentVariable("TfsExtractPrefix");

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

            logger.Info("Extract done");
            return projectList.OrderBy(x => x.Name).ToList();
        }
    }
}
