using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TfsAdminDashboardConsole.Commands;
using TfsAdminDashboardConsole.Commands.IO;
using TFSAdminDashboard.DTO;

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
            command.Execute("CSV");
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportProjectCommandJson()
        {
            iCommand command = new ExtractProjectListCommand();
            command.Execute("JSON");
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportMachinesCommand()
        {
            iCommand command = new ExtractBuildMachineListCommand();
            command.Execute("CSV");
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportUsersCommand()
        {
            iCommand command = new ExtractUsersListCommand(false);
            command.Execute("CSV");
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportUsersCommandWithOUFromAD()
        {
            iCommand command = new ExtractUsersListCommand(true);
            command.Execute("CSV");
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void TestFilterUserListCommand()
        {
            List<User> userList = (List < User >) SerializerService.Deserialize(@"D:\UnfilteredUserListWithoutOU.dat");


            var results = userList.Where(x => x.Domain == "AD-SUBS" && !x.DN.Contains("Quarantaine") && !x.DN.Contains("TTTT")).OrderBy(x => x.Name).ToList();
                   
        }
    }
}
