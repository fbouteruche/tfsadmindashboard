using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TfsAdminDashboardCommands.Command;

namespace TfsAdminDashboardCommands
{
    class Program
    {


        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            var options = new CommandLineOptions();

            var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();

            logger.Info("{0} v.{1}", assemblyName.Name, assemblyName.Version);

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.Add)
                {
                    logger.Info("Add user {0} as a member of group {1} in each project of collection {2}", options.accountName, options.groupName, options.TeamProjectCollection);
                }

                if (options.Remove)
                {
                    logger.Info("Remove user {0} from group {1} in each project of collection {2}", options.accountName, options.groupName, options.TeamProjectCollection);
                }

                new ManageUserGroupMemberShipCommand().Execute(options);

            }
        }
    }
}
