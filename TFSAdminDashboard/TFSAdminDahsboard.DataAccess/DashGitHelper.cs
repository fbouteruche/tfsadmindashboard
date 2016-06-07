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

                var commits = DataService.GitCommits(collectionName, projectName, repo.name);
                if(commits.Count > 0)
                { 
                    var lastcommit = commits.OrderByDescending(x => x.author.date).First();

                    v.InnerChangeSetId = lastcommit.commitId;
                    v.InnerLastCheckIn = lastcommit.author.date;

                    v.ItemChangeSetId = v.InnerChangeSetId;
                    v.ItemLastCheckIn = v.InnerLastCheckIn;
                }
                else
                {
                    v.InnerChangeSetId = "Void repo";
                    v.InnerLastCheckIn = DateTime.MinValue;

                    v.ItemChangeSetId = v.InnerChangeSetId;
                    v.ItemLastCheckIn = v.InnerLastCheckIn;
                }
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
