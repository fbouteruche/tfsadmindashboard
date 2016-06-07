using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.DataAccess
{
    public class DashTestPlanHelper
    {
        public static List<TestPlanDefinition> FeedTestPlanData(string collectionName, string projectName)
        {
            List<TestPlanDefinition> ans = new List<TestPlanDefinition>();

            //ITestManagementService tms = tpc.GetService<ITestManagementService>();
            //ITestManagementTeamProject tmtp = tms.GetTeamProject(projectName);
            //ITestPlanHelper testPlanHelper = tmtp.TestPlans;
            //ITestPlanCollection testPlanCollection = testPlanHelper.Query("Select * From TestPlan");
            //foreach (ITestPlan2 testPlan in testPlanCollection)
            //{
            //    TestPlanDefinition testPlanDefinition = new TestPlanDefinition()
            //    {
            //        Name = testPlan.Name,
            //        AreaPath = testPlan.AreaPath,
            //        IterationPath = testPlan.Iteration,
            //        Description = testPlan.Description,

            //        State = testPlan.Status,
            //        LastUpdate = testPlan.LastUpdated,
            //        StartDate = testPlan.StartDate,
            //        EndDate = testPlan.EndDate,
            //        Revision = testPlan.Revision
            //    };

            //    if (testPlan.Owner != null)
            //    {
            //        testPlanDefinition.Owner = testPlan.Owner.DisplayName;
            //    }
            //    else
            //    {
            //        testPlanDefinition.Owner = "None";
            //    }

            //    ans.Add(testPlanDefinition);
            //}

            return ans;
        }
    }
}
