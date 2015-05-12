using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TfsAdminDashboardConsole.Commands;

namespace TfsAdminDashboardConsole.tests
{
    [TestFixture]
    public class TestCommands
    {
        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportProjectCommand()
        {
            iCommand command = new ExtractProjectListCommand();
            command.Execute();
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportMachinesCommand()
        {
            iCommand command = new ExtractBuildMachineListCommand();
            command.Execute();
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportUsersCommand()
        {
            iCommand command = new ExtractUsersListCommand(false);
            command.Execute();
        }
    }
}
