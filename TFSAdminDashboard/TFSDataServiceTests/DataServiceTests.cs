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
    public class DataServiceTests
    {
        [Test()]
        public void RestGetGitRepositoriesTest()
        {
            Assert.IsTrue(DataService.GitRepositories("DT", "TFSAdminTool").Count > 8);
        }

        [Test()]
        public void RestGetGitBranchesTest()
        {
            Assert.IsTrue(DataService.GitBranches("DT", "TFSAdminTool", "TFSMigrationTool").Count(x => x.branchname == "develop") == 1 );
        }

        [Test()]
        public void RestGetGitTagsTest()
        {
            Assert.IsTrue(DataService.GitTags("DT", "TFSAdminTool", "TFSMigrationTool").Count(x => x.tagname == "v1.0") == 1);
        }

        [Test()]
        public void RestGetGitCommitsTest()
        {
            Assert.IsTrue(DataService.GitCommits("DT", "TFSAdminTool").Count > 99);
        }

        [Test()]
        public void RestGitFirstDateTest()
        {
            Assert.IsTrue(DataService.GitFirstDate("DT", "DemoCMMI").Year == 2016);
            Assert.IsTrue(DataService.GitFirstDate("DT", "DemoCMMI").Month == 1);
            Assert.IsTrue(DataService.GitFirstDate("DT", "DemoCMMI").Day == 25);
        }

        [Test()]
        public void RestGetCollectionsTest()
        {
            var spy = DataService.ProjectCollections();
            Assert.IsTrue(DataService.ProjectCollections().Count(x => x.name == "DT") == 1);
        }


        [Test()]
        public void RestGetProjectsTest()
        {
            Assert.IsTrue(DataService.TeamProjects("DT").Count(x => x.name == "TFSAdminTool") == 1);
        }

        [Test()]
        public void RestGetBuildDefinitionTest()
        {
            Assert.IsTrue(DataService.BuildDefinitions("DT", "DemoCMMI").Count(x => x.name == "VNext_CI" || x.name == "CI-DemoASP") == 2);
        }

        [Test()]
        public void RestGetBuildsTest()
        {
            Assert.IsTrue(DataService.Builds("DT", "DemoCMMI").Count(x => x.buildNumber == "20160531.1" && x.result == "succeeded") == 1);
        }

        [Test()]
        public void RestGetWitTypesTest()
        {
            Assert.IsTrue(DataService.WorkItemTypes("DT", "DemoCMMI").Count > 4);
        }

        [Test()]
        public void RestGetWitStatesTest()
        {
            var types = DataService.WorkItemTypes("DT", "DemoCMMI");

            var states = DataService.WorkitemStates("DT", "DemoCMMI", types.First().name);

            Assert.IsTrue(states["Resolved"] > 1);
        }

        [Test()]
        public void RestGetTestPlansTest()
        {
            var testPlans = DataService.TestPlans("DT", "DemoCMMI");

            Assert.IsTrue(testPlans.First().name == "Plan 0.2");
        }
    }
}