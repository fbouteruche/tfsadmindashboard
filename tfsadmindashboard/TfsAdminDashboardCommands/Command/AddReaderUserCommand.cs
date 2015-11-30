using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DataAccess;

namespace TfsAdminDashboardCommands.Command
{
    public class AddReaderUserCommand : TFSAccessHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AddReaderUserCommand() { }

        public void Execute(CommandLineOptions args)
        {
            var projectCollections = CatalogNodeBrowsingHelper.GetProjectCollections(configurationServer.CatalogNode);
        }
    }
}
