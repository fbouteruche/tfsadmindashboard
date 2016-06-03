using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Tool;

namespace TFSDataService
{
    public class DataService
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);

        public static List<Build> Builds(string collectionName, string projectName)
        {
            string buildsUrl = string.Format("{0}/{1}/{2}/_apis/build/builds?api-version=2.0", tfsServer, collectionName,projectName);

            string json = JsonRequest.GetRestResponse(buildsUrl);

            BuildRootobject o = JsonConvert.DeserializeObject<BuildRootobject>(json);

            return o.value.ToList();
        }

        public static List<BuildDefinition> BuildDefinitions(string collectionName, string projectName)
        {
            string buildsDefUrl = string.Format("{0}/{1}/{2}/_apis/build/definitions?api-version=2.0", tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(buildsDefUrl);

            BuildDefinitionRootobject o = JsonConvert.DeserializeObject<BuildDefinitionRootobject>(json);

            return o.value.ToList();
        }

        public static List<TeamProjectCollection> ProjectCollections()
        {
            string tpcUrl = string.Format("{0}/_apis/projectcollections", tfsServer);

            string json = JsonRequest.GetRestResponse(tpcUrl);

            TeamProjectCollectionRootobject o = JsonConvert.DeserializeObject<TeamProjectCollectionRootobject>(json);

            return o.value.ToList();
        }

        public static List<TeamProject> TeamProjects(string collection)
        {
            string tpcUrl = string.Format("{0}/{1}/_apis/projects?api-version=1.0", tfsServer, collection);

            string json = JsonRequest.GetRestResponse(tpcUrl);

            TeampProjectRootobject o = JsonConvert.DeserializeObject<TeampProjectRootobject>(json);

            return o.value.ToList();
        }

        public static DateTime GitFirstDate(string collectionName, string projectName)
        {
            List<GitCommit> commits = GitCommits(collectionName, projectName);

            if(commits.Count > 0)
            { 
                DateTime ans = commits.OrderBy(x => x.author.date).First().author.date;
                return ans;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        public static List<GitCommit> GitCommits(string collectionName, string projectName, string repoName = null)
        {
            var gitRepos = GitRepositories(collectionName, projectName);

            GitRepository gitR;

            if(string.IsNullOrEmpty(repoName))
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

        public static List<GitBranch> GitBranches(string collectionName, string projectName, string repoName)
        {
            var gitRepos = GitRepositories(collectionName, projectName);
            GitRepository gitR = gitRepos.First(x => x.name == repoName);

            string gitBranchesURL = string.Format("{0}/{1}/_apis/git/repositories/{2}/refs/heads?api-version=1.0", tfsServer, collectionName, gitR.id);

            string json = JsonRequest.GetRestResponse(gitBranchesURL);

            GitBranchRootobject o = JsonConvert.DeserializeObject<GitBranchRootobject>(json);

            return o.value.ToList();
        }

        public static List<GitTag> GitTags(string collectionName, string projectName, string repoName)
        {
            var gitRepos = GitRepositories(collectionName, projectName);
            GitRepository gitR = gitRepos.First(x => x.name == repoName);

            string gitBranchesURL = string.Format("{0}/{1}/_apis/git/repositories/{2}/refs/tags?api-version=1.0", tfsServer, collectionName, gitR.id);

            string json = JsonRequest.GetRestResponse(gitBranchesURL);

            GitTagRootobject o = JsonConvert.DeserializeObject<GitTagRootobject>(json);

            return o.value.ToList();
        }

        public static List<GitRepository> GitRepositories(string collectionName, string projectName)
        {
            string gitReposURL = string.Format("{0}/{1}/{2}/_apis/git/repositories", tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(gitReposURL);

            GitRepositoryRootobject o = JsonConvert.DeserializeObject<GitRepositoryRootobject>(json);

            return o.value.ToList();
        }
    }
}
