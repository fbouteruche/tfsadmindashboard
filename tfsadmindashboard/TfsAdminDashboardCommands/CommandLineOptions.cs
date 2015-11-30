using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsAdminDashboardCommands
{
    public class CommandLineOptions
    {
        [Option("AddTeamCollectionReader", Required = false,
       HelpText = "Add the provided user's AccountName as a reader in each project of the collection")]
        public string addTeamCollectionReader { get; set; }

        [Option("TPC", Required = true,
      HelpText = "Name of the TeamProjectCollection to manipulate")]
        public string TeamProjectCollection { get; set; }

        /// <summary>
        /// Gets the usage.
        /// </summary>
        /// <returns>the command line documentation</returns>
        [HelpOption]
        public string GetUsage()
        {
            string st = HelpText.AutoBuild(
               this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));

            return st;
        }
    }
}
