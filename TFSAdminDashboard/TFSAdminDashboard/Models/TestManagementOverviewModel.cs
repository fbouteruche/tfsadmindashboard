using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.Models
{
    public class TestManagementOverviewModel
    {

        private List<TestPlanDefinition> testPlanDefinitionCollection = new List<TestPlanDefinition>();

        public ICollection<TestPlanDefinition> TestPlanDefinitionCollection
        {
            get
            {
                return testPlanDefinitionCollection;
            }
        }

        internal void SetTestPlanDefinitionCollection(List<TestPlanDefinition> value)
        {
            testPlanDefinitionCollection = value;
        }
    }
}