
namespace TfsAdminDashboardConsole
{
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// The command line options class
    /// </summary>
    public class CommandLineOptions
    {
        [Option("ExtractBuilds", Required = false,
        HelpText = "Extract the TFS Platform Builds infos")]
        public bool extractBuildInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [extract projects] is needed or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [extract projects]; otherwise, <c>false</c>.
        /// </value>
        [Option("ExtractSimpleProjectList", Required = false,
        HelpText = "Extract the TFS Platform Team Projects list, simple infos")]
        public bool extractSimpleProjects { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [extract users] is needed or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [extract projects]; otherwise, <c>false</c>.
        /// </value>
        [Option("ExtractUsers", Required = false,
        HelpText = "Extract the TFS Platform Users list")]
        public bool extractUsers { get; set; }

        [Option("ExtractOUFromAD", Required = false,
        HelpText = "Extract also the OU property for each user in the Active Directory")]
        public bool extractUOFromAD { get; set; }

        [Option("SaveAs", Required = false,
        HelpText = "'CSV' or 'JSON' output, default is CSV")]
        public string OutputFormat { get; set; }

        [Option("UploadSFTP", Required = false,
        HelpText = "Upload result file via SFTP")]
        public bool UploadSFTP { get; set; }

        [Option("SFTPAuthent", Required = false,
       HelpText = "ByLoginPassword or ByCertificate")]
        public string SFTPAuthentMode { get; set; }

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
