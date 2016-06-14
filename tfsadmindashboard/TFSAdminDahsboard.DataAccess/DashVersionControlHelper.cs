using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;

namespace TFSAdminDashboard.DataAccess
{
    public class DashVersionControlHelper
    {
        public static List<VersionControlItem> FeedVersionControlData(string collectionName, string projectName)
        {
            List<VersionControlItem> ans = new List<VersionControlItem>();

            var changesets = DataServiceVersionControl.Changesets(collectionName, projectName);

            foreach (string branchName in changesets.Keys)
            {
                var lastChangeset = changesets[branchName].OrderByDescending(x => x.createdDate).First();

                VersionControlItem vci = new VersionControlItem()
                {
                    Path = branchName,
                    ItemChangeSetId = lastChangeset.changesetId,
                    ItemDate = lastChangeset.createdDate
                };

                ans.Add(vci);
            }

            return ans;
        }

        internal static bool isTFVC(string collectionName, string projectName)
        {
            return DataServiceVersionControl.isTFVCBased(collectionName, projectName);
        }
    }
}
