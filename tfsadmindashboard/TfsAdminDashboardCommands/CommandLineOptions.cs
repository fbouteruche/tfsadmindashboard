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
        [Option("Add", Required = false,
       HelpText = "Add the provided user's AccountName in the given group in each project of the collection")]
        public bool Add { get; set; }

        [Option("Remove", Required = false,
      HelpText = "Remove the provided user's AccountName in the given group in each project of the collection")]
        public bool Remove { get; set; }

        [Option("GroupName", Required = false,
  HelpText = "Name of the UserGroup to consider, say Readers or Project Administrators")]
        public string groupName { get; set; }

        [Option("AccountName", Required = false,
 HelpText = "The user's AccountName to deal with")]
        public string accountName { get; set; }

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
