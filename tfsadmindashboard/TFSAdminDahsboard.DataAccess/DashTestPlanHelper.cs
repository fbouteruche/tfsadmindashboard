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

            foreach(var testPlan in DataServiceTests.TestPlans(collectionName, projectName))
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
    }
}
