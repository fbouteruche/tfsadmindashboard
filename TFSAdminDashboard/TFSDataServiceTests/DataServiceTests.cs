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
        public void RestGetTFVSBranchesTest()
        {
            Assert.IsTrue(DataServiceVersionControl.Branches("DT").Count == 0);
            Assert.IsTrue(DataServiceVersionControl.Branches("DPO").Count > 0);
        }

        [Test()]
        public void RestIsTFVCTest()
        {
            Assert.IsFalse(DataServiceVersionControl.isTFVCBased("DT", "DemoCMMI"));
            //Assert.IsTrue(DataServiceVersionControl.isTFVCBased("DPO", "PI-Cougar"));
        }

        [Test()]
        public void RestGetChangeSetsTest()
        {
            Assert.IsTrue(DataServiceVersionControl.Changesets("DPO", "PI-Cougar").First().Value.Count > 0);
        }

        [Test()]
        public void RestIsGitest()
        {
            Assert.IsTrue(DataServiceGit.isGitBased("DT", "DemoCMMI"));
        }

        [Test()]
        public void RestGetGitRepositoriesTest()
        {
            Assert.IsTrue(DataServiceGit.Repositories("DT", "TFSAdminTool").Count > 8);
        }

        [Test()]
        public void RestGetGitBranchesTest()
        {
            Assert.IsTrue(DataServiceGit.Branches("DT", "TFSAdminTool", "TFSMigrationTool").Count(x => x.branchname == "develop") == 1 );
        }

        [Test()]
        public void RestGetGitTagsTest()
        {
            Assert.IsTrue(DataServiceGit.Tags("DT", "TFSAdminTool", "TFSMigrationTool").Count(x => x.tagname == "v1.0") == 1);
        }

        [Test()]
        public void RestGetGitCommitsTest()
        {
            Assert.IsTrue(DataServiceGit.Commits("DT", "TFSAdminTool").Count > 99);
        }

        [Test()]
        public void RestGitFirstDateTest()
        {
            Assert.IsTrue(DataServiceGit.FirstDate("DT", "DemoCMMI").Year == 2016);
            Assert.IsTrue(DataServiceGit.FirstDate("DT", "DemoCMMI").Month == 1);
            Assert.IsTrue(DataServiceGit.FirstDate("DT", "DemoCMMI").Day == 25);
        }

        [Test()]
        public void RestGitFirstDateTest2()
        {
            Assert.IsTrue(DataServiceGit.FirstDate("DT", "almnetDocs") != DateTime.MinValue);
        }

        [Test()]
        public void RestGetCollectionsTest()
        {
            var spy = DataServiceTeamProjects.Collections();
            Assert.IsTrue(DataServiceTeamProjects.Collections().Count(x => x.name == "DT") == 1);
        }


        [Test()]
        public void RestGetProjectsTest()
        {
            Assert.IsTrue(DataServiceTeamProjects.Projects("DT").Count(x => x.name == "TFSAdminTool") == 1);
        }

        [Test()]
        public void RestGetBuildDefinitionTemplatesTest()
        {
            Assert.IsTrue(DataServiceBuild.BuildDefinitionTemplates("DT", "DemoCMMI").Count(x => x.name.Contains("OAB")) > 0);
        }

        [Test()]
        public void RestCreateBuildDefinitionTemplatesTest()
        {
            var build  = DataServiceBuild.BuildDefinitionTemplates("DT", "DemoCMMI").Where(x => x.name.Contains("OAB")).FirstOrDefault();

            build.id = "myCustomTemplate";
            build.name = "Visual Studio OAB modif";

            DataServiceBuild.SetBuildDefinitionTemplate("DT", "DemoCMMI", build);
        }



        [Test()]
        public void RestGetBuildDefinitionTest()
        {
            Assert.IsTrue(DataServiceBuild.BuildDefinitions("DT", "DemoCMMI").Count(x => x.name == "VNext_CI" || x.name == "CI-DemoASP") == 2);
        }

        [Test()]
        public void RestGetBuildsTest()
        {
            Assert.IsTrue(DataServiceBuild.Builds("DT", "DemoCMMI").Count(x => x.result == "succeeded") > 60);
        }

        [Test()]
        public void RestGetWitTypesTest()
        {
            Assert.IsTrue(DataServiceWorkItems.Types("DT", "DemoCMMI").Count > 4);
        }

        [Test()]
        public void RestGetWitStatesTest()
        {
            var types = DataServiceWorkItems.Types("DT", "DemoCMMI");

            var states = DataServiceWorkItems.States("DT", "DemoCMMI", types.First().name);

            Assert.IsTrue(states["Resolved"] > 1);
        }

        [Test()]
        public void RestGetTestPlansTest()
        {
            var testPlans = DataServiceTests.Plans("DT", "DemoCMMI");

            Assert.IsTrue(testPlans.First().name == "Plan 0.2");
        }

        [Test()]
        public void RestGetDefaultTeamMembersTest()
        {
            var teamMembers = DataServiceTeams.DefaultMembers("DT", "DemoCMMI");

            Assert.IsTrue(teamMembers.Count(x => x.DisplayName.Contains("TOLLU")) == 1);
        }

        [Test()]
        public void RestGetTeamsTest()
        {
            var teams = DataServiceTeams.List("DT", "DemoCMMI");

            Assert.IsTrue(teams.Count(x => x.name.Contains("DemoCMMI")) == 1);
        }
        
    }
}