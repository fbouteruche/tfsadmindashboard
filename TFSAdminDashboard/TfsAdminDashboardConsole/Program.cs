using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsAdminDashboardConsole
{
    class Program
    {
        private static Uri tfsUri = new Uri(TFSAdminDashboard.Properties.Settings.Default.TfsUrl);

        private TfsConfigurationServer configurationServer = new TfsConfigurationServer(tfsUri, new NetworkCredential(TFSAdminDashboard.Properties.Settings.Default.TfsUserName, TFSAdminDashboard.Properties.Settings.Default.TfsPassword));

        static void Main(string[] args)
        {

            VersionControlServer vcs = tpc.GetService<VersionControlServer>();
            ItemSet items = vcs.GetItems("$/" + projectName + "/*", RecursionType.None);
        }
    }
}
