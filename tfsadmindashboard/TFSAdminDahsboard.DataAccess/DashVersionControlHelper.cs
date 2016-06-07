using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.DataAccess
{
    public class DashVersionControlHelper
    {
        public static List<VersionControlItem> FeedVersionControlData(string collectionName, string projectName)
        {
            throw new Exception("To RESTify");
            //List<VersionControlItem> versionControlItemCollection = new List<VersionControlItem>();

            //VersionControlServer vcs = tpc.GetService<VersionControlServer>();
            //ItemSet items = vcs.GetItems("$/" + projectName + "/*", RecursionType.None);

            //foreach (Item item in items.Items)
            //{
            //    int itemChangeSetId = item.ChangesetId;
            //    DateTime lastInnerCheckInDate = DateTime.MinValue;
            //    int lastInnerChangeSetId = 0;
            //    IEnumerable history = vcs.QueryHistory(item.ServerItem, VersionSpec.Latest,
            //        item.DeletionId,
            //        RecursionType.Full,
            //        null,
            //        new ChangesetVersionSpec(itemChangeSetId),
            //        VersionSpec.Latest,
            //        Int32.MaxValue,
            //        false,
            //        false);
            //    IEnumerator enumerator = history.GetEnumerator();
            //    if (enumerator.MoveNext())
            //    {
            //        Changeset lastChangeSet = enumerator.Current as Changeset;
            //        if (lastChangeSet != null)
            //        {
            //            lastInnerCheckInDate = lastChangeSet.CreationDate;
            //            lastInnerChangeSetId = lastChangeSet.ChangesetId;
            //        }
            //    }

            //    VersionControlItem vci = new VersionControlItem()
            //    {
            //        DisplayName = item.ServerItem,
            //        ItemChangeSetId = itemChangeSetId.ToString(),
            //        ItemLastCheckIn = item.CheckinDate,
            //        InnerChangeSetId = lastInnerChangeSetId.ToString(),
            //        InnerLastCheckIn = lastInnerCheckInDate
            //    };
            //    versionControlItemCollection.Add(vci);
            //}

            //return versionControlItemCollection;
        }
    }
}
