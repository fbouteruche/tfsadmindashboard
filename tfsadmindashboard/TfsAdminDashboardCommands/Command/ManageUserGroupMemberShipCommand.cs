using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TFSAdminDashboard.DTO;

namespace TfsAdminDashboardCommands.Command
{
    public class ManageUserGroupMemberShipCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected TfsConfigurationServer configurationServer = new TfsConfigurationServer(
            new Uri(Environment.GetEnvironmentVariable("TfsUrl")),
            new NetworkCredential(Environment.GetEnvironmentVariable("TfsLoginName"), Environment.GetEnvironmentVariable("TfsPassword")));

        public ManageUserGroupMemberShipCommand() { }

        public void Execute(CommandLineOptions args)
        {
            ITeamProjectCollectionService collectionService = configurationServer.GetService<ITeamProjectCollectionService>();

            new ManageUserGroupTool().ManageUserInIdentifiedGroup(args, collectionService, configurationServer);
        }

    }
}
