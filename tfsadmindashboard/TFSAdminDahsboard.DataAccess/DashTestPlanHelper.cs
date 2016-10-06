using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;
using TFSDataService;

namespace TFSAdminDashboard.DataAccess
{
    public class DashTestPlanHelper
    {
        public static List<TestPlanDefinition> FeedTestPlanData(string collectionName, string projectName)
        {
            List<TestPlanDefinition> ans = new List<TestPlanDefinition>();

            foreach(var testPlan in DataServiceTests.Plans(collectionName, projectName))
            {
                TestPlanDefinition testPlanDefinition = new TestPlanDefinition()
                {
                    Name = testPlan.name,
                    AreaPath = testPlan.area.name,
                    IterationPath = testPlan.iteration,

                    State = testPlan.state,
                    LastUpdate = testPlan.rootSuiteObject.lastUpdatedDate,
                    
                    Revision = testPlan.revision
                };

                if (testPlan.owner != null)
                {
                    testPlanDefinition.Owner = testPlan.owner.ToString();
                }
                else
                {
                    testPlanDefinition.Owner = "None";
                }

                ans.Add(testPlanDefinition);
            }

            return ans;
        }

        internal static double GetTestResultsRatio(string collectionName, string projectName, List<WorkItemDefinition> workitemsdata, ref int totalTestCases)
        {
            var testResults = DataServiceTests.RunResults(collectionName, projectName);

            var testCases = workitemsdata.FirstOrDefault(x => x.Name == "Test Case");

            if (testCases != null)
            {
                totalTestCases = testCases.TotalNumber;
                int openTestCases = 0;
                foreach (KeyValuePair<string, int> kvp in testCases.StateCollection)
                {
                    if (kvp.Key != "Closed")
                    {
                        openTestCases += kvp.Value;
                    }
                }

                var tests = testResults.Select(x => new { id = x.testCase.id, outcome = x.outcome, date = x.completedDate });

                var latestOutcomes = tests.GroupBy(x => x.id).Select(y => y.OrderByDescending(z => z.date).FirstOrDefault());

                double numberpassed = latestOutcomes.Count(x => x.outcome == "Passed");

                return numberpassed / openTestCases;
            }
            else
                return 0;
        }
    }
}
