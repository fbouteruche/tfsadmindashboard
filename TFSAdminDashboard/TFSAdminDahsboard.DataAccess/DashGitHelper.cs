using LibGit2Sharp;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Git.Client;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Collections.Generic;
using System.Linq;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.DataAccess
{

    public class DashGitHelper
    {
        public static List<VersionControlItem> FeedGitData(TfsTeamProjectCollection tpc, string projectName)
        {
            List<VersionControlItem> versionControlItemCollection = new List<VersionControlItem>();


            var gitService = tpc.GetService<GitRepositoryService>();

            GitRepository repository = gitService.QueryRepositories(projectName).FirstOrDefault();

            using (var repo = new Repository(repository.RemoteUrl))
            {
                var RFC2822Format = "ddd dd MMM HH:mm:ss yyyy K";

                foreach (Commit c in repo.Commits.Take(15))
                {
                    //Console.WriteLine(string.Format("commit {0}", c.Id));

                    if (c.Parents.Count() > 1)
                    {
                        //Console.WriteLine("Merge: {0}",
                            //string.Join(" ", c.Parents.Select(p => p.Id.Sha.Substring(0, 7)).ToArray()));
                    }

                    //Console.WriteLine(string.Format("Author: {0} <{1}>", c.Author.Name, c.Author.Email));
                    //Console.WriteLine("Date:   {0}", c.Author.When.ToString(RFC2822Format, CultureInfo.InvariantCulture));
                    //Console.WriteLine();
                    //Console.WriteLine(c.Message);
                    //Console.WriteLine();
                }
            }

            return versionControlItemCollection;
        }
    }
}
