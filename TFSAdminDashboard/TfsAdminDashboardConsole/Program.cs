using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TfsAdminDashboardConsole.Commands;
using NLog;

namespace TfsAdminDashboardConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
             var options = new CommandLineOptions();
             bool processed = false;

             if (CommandLine.Parser.Default.ParseArguments(args, options))
             {
                 processed = (options.extractProjects || options.extractMachines || options.extractUsers) == true;

                 if (options.extractProjects == true)
                 {
                     logger.Info("Extract Projects");
                     iCommand command = new ExtractProjectListCommand();
                     command.Execute(options.OutputFormat);
                 }

                 if (options.extractMachines == true)
                 {
                     logger.Info("Extract Build Machines");
                     iCommand command = new ExtractBuildMachineListCommand();
                     command.Execute(options.OutputFormat);
                 }

                 if (options.extractUsers == true)
                 {
                     logger.Info("Extract users");
                     logger.Warn("that's a quite long process");
                     if(options.extractUOFromAD)
                     {
                         logger.Warn("especially with the AD query");
                     }

                     iCommand command = new ExtractUsersListCommand(options.extractUOFromAD);
                     command.Execute(options.OutputFormat);
                 }
             }

             if (processed == false)
             {
                 Console.Write(options.GetUsage());
                 Console.ReadKey();
             }
        }
    }
}
