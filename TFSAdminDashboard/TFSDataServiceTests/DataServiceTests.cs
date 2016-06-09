using NUnit.Framework;
using TFSDataService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.Tests
{
    [TestFixture()]
    public class DataServicesTests
    {
        [Test()]
        public void RestGetGitRepositoriesTest()
        {
            Assert.IsTrue(DataServiceGit.GitRepositories("DT", "TFSAdminTool").Count > 8);
        }

        [Test()]
        public void RestGetGitBranchesTest()
        {
            Assert.IsTrue(DataServiceGit.GitBranches("DT", "TFSAdminTool", "TFSMigrationTool").Count(x => x.branchname == "develop") == 1 );
        }

        [Test()]
        public void RestGetGitTagsTest()
        {
            Assert.IsTrue(DataServiceGit.GitTags("DT", "TFSAdminTool", "TFSMigrationTool").Count(x => x.tagname == "v1.0") == 1);
        }

        [Test()]
        public void RestGetGitCommitsTest()
        {
            Assert.IsTrue(DataServiceGit.GitCommits("DT", "TFSAdminTool").Count > 99);
        }

        [Test()]
        public void RestGitFirstDateTest()
        {
            Assert.IsTrue(DataServiceGit.GitFirstDate("DT", "DemoCMMI").Year == 2016);
            Assert.IsTrue(DataServiceGit.GitFirstDate("DT", "DemoCMMI").Month == 1);
            Assert.IsTrue(DataServiceGit.GitFirstDate("DT", "DemoCMMI").Day == 25);
        }

        [Test()]
        public void RestGetCollectionsTest()
        {
            var spy = DataServiceTeamProjects.ProjectCollections();
            Assert.IsTrue(DataServiceTeamProjects.ProjectCollections().Count(x => x.name == "DT") == 1);
        }


        [Test()]
        public void RestGetProjectsTest()
        {
            Assert.IsTrue(DataServiceTeamProjects.TeamProjects("DT").Count(x => x.name == "TFSAdminTool") == 1);
        }

        [Test()]
        public void RestGetBuildDefinitionTest()
        {
            Assert.IsTrue(DataServiceBuild.BuildDefinitions("DT", "DemoCMMI").Count(x => x.name == "VNext_CI" || x.name == "CI-DemoASP") == 2);
        }

        [Test()]
        public void RestGetBuildsTest()
        {
            Assert.IsTrue(DataServiceBuild.Builds("DT", "DemoCMMI").Count(x => x.buildNumber == "20160531.1" && x.result == "succeeded") == 1);
        }

        [Test()]
        public void RestGetWitTypesTest()
        {
            Assert.IsTrue(DataServiceWorkItems.WorkItemTypes("DT", "DemoCMMI").Count > 4);
        }

        [Test()]
        public void RestGetWitStatesTest()
        {
            var types = DataServiceWorkItems.WorkItemTypes("DT", "DemoCMMI");

            var states = DataServiceWorkItems.WorkitemStates("DT", "DemoCMMI", types.First().name);

            Assert.IsTrue(states["Resolved"] > 1);
        }

        [Test()]
        public void RestGetTestPlansTest()
        {
            var testPlans = DataServiceTests.TestPlans("DT", "DemoCMMI");

            Assert.IsTrue(testPlans.First().name == "Plan 0.2");
        }
    }
}