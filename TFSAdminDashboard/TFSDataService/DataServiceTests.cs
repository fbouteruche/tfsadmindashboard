using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Tool;

namespace TFSDataService
{
    public static class DataServiceTests
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);

        static DataServiceTests()
        {
            DataServiceBase.CheckVariables();
        }

        // TODO: Expose also test results

        public static List<TestPlan> Plans(string collectionName, string projectName)
        {

            List<TestPlan> ans = new List<TestPlan>();

            string testPlansURL = string.Format("{0}/{1}/{2}/_apis/test/plans", tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(testPlansURL);

            TestPlanRootobject o = JsonConvert.DeserializeObject<TestPlanRootobject>(json);

            foreach (TestPlan tp in o.value)
            {
                string testSuiteURL = string.Format("{0}/{1}/{2}/_apis/test/plans/{3}/suites/{4}", tfsServer, collectionName, projectName, tp.id, tp.rootSuite.id);

                string json2 = JsonRequest.GetRestResponse(testSuiteURL);

                TestSuite suite = JsonConvert.DeserializeObject<TestSuite>(json2);

                tp.rootSuiteObject = suite;

                ans.Add(tp);
            }

            return ans;
        }

        public static List<TestRun> Runs(string collectionName, string projectName)
        {
            string testRunsURL = string.Format("{0}/{1}/{2}/_apis/test/runs?api-version=1.0", tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(testRunsURL);

            TestRunRootobject o = JsonConvert.DeserializeObject<TestRunRootobject>(json);

            return o.value.ToList();
        }

        public static List<TestResult> RunResults(string collectionName, string projectName)
        {
            List<TestResult> ans = new List<TestResult>();

            foreach(TestRun run in Runs(collectionName, projectName))
            {
                string testRunResultsURL = string.Format("{0}/{1}/{2}/_apis/test/runs/{3}/results?api-version=3.0-preview", tfsServer, collectionName, projectName, run.id);

                string json = JsonRequest.GetRestResponse(testRunResultsURL);

                TestResultRootobject o = JsonConvert.DeserializeObject<TestResultRootobject>(json);

                ans.AddRange(o.value.ToList());
            }

            return ans;
        }
    }
}
