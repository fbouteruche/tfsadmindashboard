﻿using System.Collections.Generic;
using System.Linq;
using TFSAdminDashboard.DTO;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;
using System;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DataAccess
{

    public class DashGitHelper
    {
        internal static List<PullRequest> FeedPullRequestData(string collectionName, string projectName, string repoName)
        {
            return DataServiceGit.PullRequests(collectionName, projectName, repoName);
        }

        internal static List<GitTag> FeedGitTagData(string collectionName, string projectName, string repoName)
        {
            return DataServiceGit.Tags(collectionName, projectName, repoName);
        }

        internal static List<GitBranch> FeedGitBranchData(string collectionName, string projectName)
        {
            List<GitBranch> ans = new List<GitBranch>();

            Parallel.ForEach(DataServiceGit.Repositories(collectionName, projectName), (repo) =>
             {
                 ans.AddRange(DataServiceGit.Branches(collectionName, projectName, repo.name));
             });

            return ans;
        }

        public static List<VersionControlItem> FeedGitData(string collectionName, string projectName)
        {
            List<VersionControlItem> versionControlItemCollection = new List<VersionControlItem>();

            Parallel.ForEach(DataServiceGit.Repositories(collectionName, projectName), (repo) =>
           {
               VersionControlItem v = new VersionControlItem()
               {
                   Repository = repo.name,
                   isGit = true,
                   DefaultBranch = repo.defaultBranch
               };

               var commits = DataServiceGit.Commits(collectionName, projectName, repo.name);
               if (commits.Count > 0)
               {
                   var lastcommit = commits.OrderByDescending(x => x.author.date).First();

                   v.LastCommit = lastcommit.commitId;
                   v.ItemDate = lastcommit.author.date;
               }
               else
               {
                   v.LastCommit = "Void repo";
                   v.ItemDate = DateTime.MinValue;
               }

               v.TotalMasterCommitYesterday = commits.Where(x => x.author.date.Date == DateTime.Now.AddDays(-1).Date).ToList().Count;

               versionControlItemCollection.Add(v);
           });

            return versionControlItemCollection;
        }

        internal static DateTime? GetCreationDate(string collectionName, string projectName)
        {
            return DataServiceGit.FirstDate(collectionName, projectName);
        }

        internal static bool isGit(string collectionName, string projectName)
        {
            return DataServiceGit.isGitBased(collectionName, projectName);
        }


    }
}
