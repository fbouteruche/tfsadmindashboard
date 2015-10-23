
namespace TfsAdminDashboardConsole
{
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// The command line options class
    /// </summary>
    public class CommandLineOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether [extract machines] is needed or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [extract machines]; otherwise, <c>false</c>.
        /// </value>
        [Option('m', "ExtractBuildMachineList", Required = false,
        HelpText = "Extract the TFS Platform Build controllers and agent list")]
        public bool extractMachines { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [extract projects] is needed or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [extract projects]; otherwise, <c>false</c>.
        /// </value>
        [Option('p', "ExtractProjectList", Required = false,
        HelpText = "Extract the TFS Platform Team Projects list")]
        public bool extractProjects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [extract users] is needed or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [extract projects]; otherwise, <c>false</c>.
        /// </value>
        [Option('u', "ExtractUsers", Required = false,
        HelpText = "Extract the TFS Platform Users list")]
        public bool extractUsers { get; set; }

        [Option('o', "ExtractOUFromAD", Required = false,
        HelpText = "Extract also the OU property for each user in the Active Directory")]
        public bool extractUOFromAD { get; set; }

        [Option('s', "SaveAs", Required = false,
        HelpText = "'CSV' or 'JSON' output, default is CSV")]
        public string OutputFormat { get; set; }

        [Option('u', "UploadSFTP", Required = false,
        HelpText = "Upload result file via SFTP")]
        public bool UploadSFTP { get; set; }

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
