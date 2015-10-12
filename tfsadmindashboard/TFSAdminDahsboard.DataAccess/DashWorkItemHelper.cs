using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.DataAccess
{
    public class DashWorkItemHelper
    {
        public static List<WorkItemDefinition> FeedWorkItemData(TfsTeamProjectCollection tpc, string projectName)
        {
            List<WorkItemDefinition> workItemDefinitionCollection = new List<WorkItemDefinition>();

            WorkItemStore wis = tpc.GetService<WorkItemStore>();
            Microsoft.TeamFoundation.WorkItemTracking.Client.Project project = wis.Projects[projectName];

            foreach (WorkItemType wit in project.WorkItemTypes)
            {
                WorkItemDefinition witDefinition = new WorkItemDefinition() { Name = wit.Name, Description = wit.Description };

                IEnumerable<Category> categories = project.Categories.Where(x => x.WorkItemTypes.Contains(wit));
                foreach (Category item in categories)
                {
                    witDefinition.Categories.Add(item.Name);
                }

                FieldDefinition systemState = wit.FieldDefinitions.TryGetByName("System.State");
                foreach (string allowedValue in systemState.AllowedValues)
                {
                    int stateCount = wis.QueryCount("Select System.Id From WorkItems Where System.TeamProject = '"
                        + projectName
                        + "' And System.WorkItemType = '"
                        + witDefinition.Name
                        + "' And System.State = '"
                        + allowedValue
                        + "'");

                    witDefinition.StateCollection.Add(allowedValue, stateCount);
                }
                workItemDefinitionCollection.Add(witDefinition);

            }

            return workItemDefinitionCollection;
        }
    }
}
