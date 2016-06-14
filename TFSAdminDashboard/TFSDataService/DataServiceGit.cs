using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Tool;

namespace TFSDataService
{
    public static class DataServiceGit
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);

        public static DateTime FirstDate(string collectionName, string projectName)
        {
            List<GitCommit> commits = Commits(collectionName, projectName);

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
                string gitCommitsUrl = string.Format("{0}/{1}/_apis/git/repositories/{2}/commits?api-version=1.0", tfsServer, collectionName, gitR.id);

                string json = JsonRequest.GetRestResponse(gitCommitsUrl);

                GitCommitRootobject o = JsonConvert.DeserializeObject<GitCommitRootobject>(json);

                return o.value.ToList();
            }
            else
                return new List<GitCommit>();
        }

        public static List<GitBranch> Branches(string collectionName, string projectName, string repoName)
        {
            var gitRepos = Repositories(collectionName, projectName);
            GitRepository gitR = gitRepos.First(x => x.name == repoName);

            string gitBranchesURL = string.Format("{0}/{1}/_apis/git/repositories/{2}/refs/heads?api-version=1.0", tfsServer, collectionName, gitR.id);

            string json = JsonRequest.GetRestResponse(gitBranchesURL);

            GitBranchRootobject o = JsonConvert.DeserializeObject<GitBranchRootobject>(json);

            return o.value.ToList();
        }

        public static List<GitTag> Tags(string collectionName, string projectName, string repoName)
        {
            var gitRepos = Repositories(collectionName, projectName);
            GitRepository gitR = gitRepos.First(x => x.name == repoName);

            string gitBranchesURL = string.Format("{0}/{1}/_apis/git/repositories/{2}/refs/tags?api-version=1.0", tfsServer, collectionName, gitR.id);

            string json = JsonRequest.GetRestResponse(gitBranchesURL);

            GitTagRootobject o = JsonConvert.DeserializeObject<GitTagRootobject>(json);

            return o.value.ToList();
        }

        public static List<GitRepository> Repositories(string collectionName, string projectName)
        {
            string gitReposURL = string.Format("{0}/{1}/{2}/_apis/git/repositories", tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(gitReposURL);

            GitRepositoryRootobject o = JsonConvert.DeserializeObject<GitRepositoryRootobject>(json);

            return o.value.ToList();
        }

        public static bool isGitBased(string collectionName, string projectName)
        {
            return Repositories(collectionName, projectName).Count > 0;
        }
    }
}
