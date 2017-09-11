using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DataAccess.BO;
using TFSAdminDashboard.DTO;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;

namespace TFSAdminDashboard.DataAccess
{
    public class DashWorkItemHelper
    {
        public static WorkItemData FeedWorkItemData(string collectionName, string projectName)
        {
            List<WorkItemDefinition> workItemDefinitionCollection = new List<WorkItemDefinition>();
            int modifsYesterday = 0;
            DateTime lastModif = new DateTime(2015, 06, 01);


            foreach (WorkItemType wit in DataServiceWorkItems.Types(collectionName, projectName))
            {
                WorkItemDefinition witDefinition = new WorkItemDefinition() { Name = wit.name, Description = wit.description };

                var serviceAns = DataServiceWorkItems.StatesnModifs(collectionName, projectName, wit.name);

                foreach (KeyValuePair<string, int> kvp in serviceAns.states)
                {
                    witDefinition.StateCollection.Add(kvp.Key, kvp.Value);
                    witDefinition.TotalNumber += kvp.Value;

                    if (kvp.Key.ToUpper() == "CLOSED")
                        witDefinition.ClosedNumber += kvp.Value;
                }

                // only mention wit types which are indeed used in the project
                if (witDefinition.StateCollection.Count >= 1)
                {
                    workItemDefinitionCollection.Add(witDefinition);
                    modifsYesterday += serviceAns.modifsYesterday;
                    if (serviceAns.lastmodif > lastModif)
                        lastModif = serviceAns.lastmodif;
                }
            }

            return new WorkItemData()
            {
                workItemDefinitionCollection = workItemDefinitionCollection,
                lastmodif = lastModif,
                modifsYesterday = modifsYesterday
            };
        }
    }
}
