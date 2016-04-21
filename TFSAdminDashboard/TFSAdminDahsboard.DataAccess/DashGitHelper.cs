using Microsoft.TeamFoundation.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;
using Microsoft.TeamFoundation.Git.Client;
using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace TFSAdminDashboard.DataAccess
{

    public class DashGitHelper
    {
        public static List<VersionControlItem> FeedGitData(TfsTeamProjectCollection tpc, string projectName)
        {
            List<VersionControlItem> versionControlItemCollection = new List<VersionControlItem>();


            var gitService = tpc.GetService<GitRepositoryService>();

            GitRepository repository = gitService.QueryRepositories(projectName).FirstOrDefault();

            return versionControlItemCollection;
        }
    }
}
