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

            foreach(GitRepository repo in DataServiceGit.GitRepositories(collectionName, projectName))
            {
                VersionControlItem v = new VersionControlItem()
                {
                    Repository = repo.name,
                    isGit = true
                };

                var commits = DataServiceGit.GitCommits(collectionName, projectName, repo.name);
                if(commits.Count > 0)
                { 
                    var lastcommit = commits.OrderByDescending(x => x.author.date).First();

                    v.LastCommit = lastcommit.commitId;
                    v.LastCommitDate = lastcommit.author.date;
                }
                else
                {
                    v.LastCommit = "Void repo";
                    v.LastCommitDate = DateTime.MinValue; 
                }
                versionControlItemCollection.Add(v);
            }
           
            return versionControlItemCollection;
        }

        internal static DateTime? GetCreationDate(string collectionName, string projectName)
        {
            return DataServiceGit.GitFirstDate(collectionName, projectName);
        }
    }
}
