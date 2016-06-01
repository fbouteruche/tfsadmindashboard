using Microsoft.TeamFoundation.Client;
using System.Collections.Generic;
using System.Linq;
using TFSAdminDashboard.DTO;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;
using System;

namespace TFSAdminDashboard.DataAccess
{

    public class DashGitHelper
    {
        public static List<VersionControlItem> FeedGitData(string collectionName, string projectName)
        {
            List<VersionControlItem> versionControlItemCollection = new List<VersionControlItem>();

            foreach(GitRepository repo in DataService.GitRepositories(collectionName, projectName))
            {
                VersionControlItem v = new VersionControlItem()
                {
                    DisplayName = repo.name
                };

                var lastcommit = DataService.GitCommits(collectionName, projectName, repo.name).OrderByDescending(x => x.author.date).First();

                v.InnerChangeSetId = lastcommit.commitId;
                v.InnerLastCheckIn = lastcommit.author.date;

                v.ItemChangeSetId = v.InnerChangeSetId;
                v.ItemLastCheckIn = v.InnerLastCheckIn;

                versionControlItemCollection.Add(v);
            }
           
            return versionControlItemCollection;
        }

        internal static DateTime? GetCreationDate(string collectionName, string projectName)
        {
            return DataService.GitFirstDate(collectionName, projectName);
        }
    }
}
