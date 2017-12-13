using MoreLinq;
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
                case "DX":
                    projectDefinition.DM = "AD";
                    break;
                case "DPS":
                case "DATA":
                    projectDefinition.DM = "ISBI";
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
                MasterCommitsYesterday = commitsData.Where(w => w.Repository == x.Repository).Sum(v => v.TotalMasterCommitYesterday),
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


            projectDefinition.GitCommitsYesterday = commitsData.Sum(x => x.TotalMasterCommitYesterday);

            projectDefinition.LastCommit = commitsData.OrderByDescending(x => x.ItemDate).First().ItemDate;

            projectDefinition.Repositories = repositories.ToList();

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

                var submodulesBuilds = buildData.Where(x => x.UsesGitSubmodules);
                if (submodulesBuilds.Any())
                {
                    projectDefinition.UsesGitSubModules = true;
                }

                projectDefinition.LastBuildOK = buildData.OrderByDescending(x => x.LastSuccess).First().LastSuccess;
                projectDefinition.LastBuildKO = buildData.OrderByDescending(x => x.LastFail).First().LastFail;
            }

            // get Workitems data
            logger.Trace("WorkItems Data");
            var workitemsdata = DashWorkItemHelper.FeedWorkItemData(currCollection.name, project.name);

            projectDefinition.WorkItemModifYesterday = workitemsdata.modifsYesterday;
            projectDefinition.FuncTestModifYesterday = workitemsdata.testcasemodifyesterday;
            projectDefinition.LastWorkItemModif = workitemsdata.lastmodif;

            projectDefinition.WorkItemNumber = workitemsdata.workItemDefinitionCollection.Sum(x => x.TotalNumber);

            double closednumber = workitemsdata.workItemDefinitionCollection.Sum(x => x.ClosedNumber);


            projectDefinition.WorkItemHealth = projectDefinition.WorkItemNumber != 0 ? closednumber / projectDefinition.WorkItemNumber : 0;



            // get test plan Data
            logger.Trace("TestPlan Data");
            int test_number = 0;
            var testResults = DataServiceTests.RunResults(currCollection.name, project.name);

            int testyesterday = 0;
            int unittestyesterday = 0;

            DateTime lastTestResult = new DateTime(2015, 06, 01);

            Parallel.ForEach(testResults, (result) =>
            {
                if (result.testCase.id != null && result.completedDate > lastTestResult)
                    lastTestResult = result.completedDate;
                if (result.completedDate.Date == DateTime.Now.AddDays(-1).Date)
                {
                    if (result.testCase.id != null)
                        testyesterday += 1;
                    else
                        unittestyesterday += 1;
                }
            });

            projectDefinition.FuncTestHealth = DashTestPlanHelper.GetTestResultsRatio(currCollection.name, project.name, workitemsdata.workItemDefinitionCollection, ref test_number, testResults);

            projectDefinition.FuncTestPassedYesterday = testyesterday;
            projectDefinition.UnitTestPassedYesterday = unittestyesterday;
            projectDefinition.FuncTestLastResult = lastTestResult;

            projectDefinition.FuncTestNumber = test_number;

            projectList.Add(projectDefinition);
        }
    }
}
