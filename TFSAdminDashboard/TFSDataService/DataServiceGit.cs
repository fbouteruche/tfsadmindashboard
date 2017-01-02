using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Properties;
using TFSDataService.Tool;

namespace TFSDataService
{
    public static class DataServiceGit
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl");

        static DataServiceGit()
        {
            DataServiceBase.CheckVariables();
        }

        /// <summary>
        /// get the date of the first git commit
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static DateTime FirstDate(string collectionName, string projectName)
        {
            List<GitCommit> commits = AllCommits(collectionName, projectName);

            if (commits.Count > 0)
            {
                DateTime ans = commits.OrderBy(x => x.author.date).First().author.date;
                return ans;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Get all commits, through all git repo (only the first 100 seems to be returned by the API)
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        private static List<GitCommit> AllCommits(string collectionName, string projectName)
        {
            List<GitCommit> ans = new List<GitCommit>();
            var gitRepos = Repositories(collectionName, projectName);

            foreach (GitRepository gitR in gitRepos)
            {
                string gitCommitsUrl = string.Format(Settings.Default.GitCommitUrl, tfsServer, collectionName, gitR.id);

                string json = JsonRequest.GetRestResponse(gitCommitsUrl);

                GitCommitRootobject o = JsonConvert.DeserializeObject<GitCommitRootobject>(json);

                ans.AddRange(o.value.ToList());
            }

            return ans;
        }

        /// <summary>
        /// Get all commits, for a given git repo (only the first 100 seems to be returned by the API)
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <param name="repoName"></param>
        /// <returns></returns>
        public static List<GitCommit> Commits(string collectionName, string projectName, string repoName = null)
        {
            var gitRepos = Repositories(collectionName, projectName);

            GitRepository gitR;

            if (string.IsNullOrEmpty(repoName))
            {
                gitR = gitRepos.FirstOrDefault(x => x.name == projectName);
            }
            else
            {
                gitR = gitRepos.First(x => x.name == repoName);
            }

            if (gitR != null)
            {
                string gitCommitsUrl = string.Format(Settings.Default.GitCommitUrl, tfsServer, collectionName, gitR.id);

                string json = JsonRequest.GetRestResponse(gitCommitsUrl);

                GitCommitRootobject o = JsonConvert.DeserializeObject<GitCommitRootobject>(json);

                return o.value.ToList();
            }
            else
                return new List<GitCommit>();
        }

        /// <summary>
        /// List Git branches for a git repo
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <param name="repoName"></param>
        /// <returns></returns>
        public static List<GitBranch> Branches(string collectionName, string projectName, string repoName)
        {
            var gitRepos = Repositories(collectionName, projectName);
            GitRepository gitR = gitRepos.First(x => x.name == repoName);

            string gitBranchesURL = string.Format(Settings.Default.GitBranchUrl, tfsServer, collectionName, gitR.id);

            string json = JsonRequest.GetRestResponse(gitBranchesURL);

            GitBranchRootobject o = JsonConvert.DeserializeObject<GitBranchRootobject>(json);

            o.value.ToList().ForEach(x => x.repo = repoName);

            return o.value.ToList();
        }

        /// <summary>
        /// List Git tags, for a given repo
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <param name="repoName"></param>
        /// <returns></returns>
        public static List<GitTag> Tags(string collectionName, string projectName, string repoName)
        {
            var gitRepos = Repositories(collectionName, projectName);
            GitRepository gitR = gitRepos.First(x => x.name == repoName);

            string gitTagsURL = string.Format(Settings.Default.GitTagUrl, tfsServer, collectionName, gitR.id);

            string json = JsonRequest.GetRestResponse(gitTagsURL);

            GitTagRootobject o = JsonConvert.DeserializeObject<GitTagRootobject>(json);

            return o.value.ToList();
        }

        /// <summary>
        /// List Pull requests, for a given repo
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <param name="repoName"></param>
        /// <returns></returns>
        public static List<PullRequest> PullRequests(string collectionName, string projectName, string repoName)
        {
            var gitRepos = Repositories(collectionName, projectName);
            GitRepository gitR = gitRepos.First(x => x.name == repoName);

            string gitPullRequestURL = string.Format(Settings.Default.GitPullRequestUrl, tfsServer, collectionName, gitR.id);

            string json = JsonRequest.GetRestResponse(gitPullRequestURL);

            PullRequestRootobject o = JsonConvert.DeserializeObject<PullRequestRootobject>(json);

            return o.value.ToList();
        }

        /// <summary>
        /// List Git repos
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static List<GitRepository> Repositories(string collectionName, string projectName)
        {
            string gitReposURL = string.Format(Settings.Default.GitRepoUrl, tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(gitReposURL);

            GitRepositoryRootobject o = JsonConvert.DeserializeObject<GitRepositoryRootobject>(json);

            return o.value.ToList();
        }

        /// <summary>
        /// Is a TFS Project Git based?
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static bool isGitBased(string collectionName, string projectName)
        {
            return Repositories(collectionName, projectName).Count > 0;
        }
    }
}
