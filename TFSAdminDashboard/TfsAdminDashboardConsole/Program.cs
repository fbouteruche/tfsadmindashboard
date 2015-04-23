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

namespace TfsAdminDashboardConsole
{
    class Program
    {

        static void Main(string[] args)
        {
             var options = new CommandLineOptions();
             bool processed = false;

             if (CommandLine.Parser.Default.ParseArguments(args, options))
             {
                 processed = (options.extractProjects || options.extractMachines) == true;

                 if (options.extractProjects == true)
                 {
                     ExtractProjectListCommand command = new ExtractProjectListCommand();
                     command.Execute();
                 }

                 if (options.extractMachines == true)
                 {
                     ExtractBuildMachineListCommand command = new ExtractBuildMachineListCommand();
                     command.Execute();
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
