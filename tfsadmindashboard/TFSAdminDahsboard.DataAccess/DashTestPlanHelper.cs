﻿using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
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
        public static List<TestPlanDefinition> FeedTestPlanData(TfsTeamProjectCollection tpc, string projectName)
        {
            List<TestPlanDefinition> ans = new List<TestPlanDefinition>();

            ITestManagementService tms = tpc.GetService<ITestManagementService>();
            ITestManagementTeamProject tmtp = tms.GetTeamProject(projectName);
            ITestPlanHelper testPlanHelper = tmtp.TestPlans;
            ITestPlanCollection testPlanCollection = testPlanHelper.Query("Select * From TestPlan");
            foreach (ITestPlan2 testPlan in testPlanCollection)
            {
                TestPlanDefinition testPlanDefinition = new TestPlanDefinition()
                {
                    Name = testPlan.Name,
                    AreaPath = testPlan.AreaPath,
                    IterationPath = testPlan.Iteration,
                    Description = testPlan.Description,
                    Owner = testPlan.Owner.DisplayName,
                    State = testPlan.Status,
                    LastUpdate = testPlan.LastUpdated,
                    StartDate = testPlan.StartDate,
                    EndDate = testPlan.EndDate,
                    Revision = testPlan.Revision
                };
                ans.Add(testPlanDefinition);
            }

            return ans;
        }
    }
}
