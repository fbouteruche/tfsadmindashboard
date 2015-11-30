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
                if (! string.IsNullOrEmpty(options.addTeamCollectionReader))
                {
                    logger.Info("Add user {0} as a reader in each project of collection {1}", options.addTeamCollectionReader, options.TeamProjectCollection);
                    AddReaderUserCommand command = new AddReaderUserCommand();
                    command.Execute(options);
                }
            }
        }
    }
}
