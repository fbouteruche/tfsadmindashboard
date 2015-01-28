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

namespace TfsAdminDashboardConsole
{
    class Program
    {

        static void Main(string[] args)
        {
         
            if(args.Length > 0 && args[0] == "ExtractProjectList")
            {
                ExtractProjectListCommand command = new ExtractProjectListCommand();
                command.Execute(args);
            }
            else if(args.Length > 0 && args[0] == "ExtractBuildMachineList")
            {
                ExtractBuildMachineListCommand command = new ExtractBuildMachineListCommand();
                command.Execute(args);
            }
        }
    }
}
