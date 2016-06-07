using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;

namespace TFSAdminDashboard.DataAccess
{
    public class DashWorkItemHelper
    {
        public static List<WorkItemDefinition> FeedWorkItemData(string collectionName, string projectName)
        {
            List<WorkItemDefinition> workItemDefinitionCollection = new List<WorkItemDefinition>();

            foreach (WorkItemType wit in DataService.WorkItemTypes(collectionName, projectName))
            {
                WorkItemDefinition witDefinition = new WorkItemDefinition() { Name = wit.name, Description = wit.description };

                foreach (KeyValuePair<string, int> kvp in DataService.WorkitemStates(collectionName, projectName, wit.name))
                { 
                    witDefinition.StateCollection.Add(kvp.Key, kvp.Value);
                }

                workItemDefinitionCollection.Add(witDefinition);
            }

            return workItemDefinitionCollection;
        }
    }
}
