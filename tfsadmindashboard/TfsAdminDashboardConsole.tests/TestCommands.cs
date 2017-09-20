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
        CommandLineOptions options = new CommandLineOptions();

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportBuildsCommand()
        {
            iCommand command = new ExtractBuildsCommand();
            options.OutputFormat = "JSON";
            command.Execute(options);
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportBuildsCommandCSV()
        {
            iCommand command = new ExtractBuildsCommand();
            options.OutputFormat = "CSV";
            command.Execute(options);
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportProjectSimpleCommand()
        {
            iCommand command = new ExtractSimpleProjectListCommand();
            options.OutputFormat = "JSON";
            command.Execute(options);
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportProjectSimpleCSVCommand()
        {
            iCommand command = new ExtractSimpleProjectListCommand();
            options.OutputFormat = "CSV";
            command.Execute(options);
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportUsersCommand()
        {
            iCommand command = new ExtractUsersListCommand();
            options.OutputFormat = "CSV";
            options.extractUOFromAD = false;
            command.Execute(options);
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void LaunchExportUsersCommandWithOUFromAD()
        {
            iCommand command = new ExtractUsersListCommand();
            options.OutputFormat = "CSV";
            options.extractUOFromAD = true;
            command.Execute(options);
        }

        [TestCase]
        [Category("DevFacadeNoIC")]
        public void TestFilterUserListCommand()
        {
            List<User> userList = (List<User>)SerializerService.Deserialize(@"D:\UnfilteredUserListWithoutOU.dat");


            var results = userList.Where(x => x.Domain == "AD-SUBS" && !x.DN.Contains("Quarantaine") && !x.DN.Contains("TTTT")).OrderBy(x => x.Name).ToList();

        }
    }
}
