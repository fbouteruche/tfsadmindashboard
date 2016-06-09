using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Tool;

namespace TFSDataService
{
    public static class DataServiceTests
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);

        // TODO: Expose also test results

        public static List<TestPlan> TestPlans(string collectionName, string projectName)
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

    }
}
