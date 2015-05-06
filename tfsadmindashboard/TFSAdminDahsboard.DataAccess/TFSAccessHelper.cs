using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;

namespace TFSAdminDashboard.DataAccess
{
    /// <summary>
    /// Class giving access to the TFS Server
    /// </summary>
    public class TFSAccessHelper
    {
        protected TfsConfigurationServer configurationServer = new TfsConfigurationServer(
            new Uri(Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User)),
            new NetworkCredential(Environment.GetEnvironmentVariable("TfsLoginName", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsPassword", EnvironmentVariableTarget.User)));

        public TFSAccessHelper() { }
    }
}
