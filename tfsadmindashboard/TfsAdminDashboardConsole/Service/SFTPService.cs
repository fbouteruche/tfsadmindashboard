using NLog;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsAdminDashboardConsole.Service
{
    /// <summary>
    /// SSH.Net implementation of the ISFTPService
    /// </summary>
    public class SFTPService : ISFTPService
    {

        /// <summary>
        /// The logger
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Uploads the file via SFTP.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="authent">The chosen SSH authent method</param>
        public void UploadFile(string filePath, AuthentMethod authent)
        {
            using (var sftp = new SftpClient(CreateConnectionInfo(authent)))
            {
                var fileStream = File.OpenRead(filePath);
                sftp.Connect();
                string uploadPath = string.Format("{0}/{1}", Environment.GetEnvironmentVariable("TfsExtractSSH_Path", EnvironmentVariableTarget.User), Path.GetFileName(filePath));

                sftp.UploadFile(fileStream, uploadPath);

                sftp.Disconnect();

                fileStream.Close();
            }

            logger.Info("File {0} uploaded", uploadPath);
        }

        /// <summary>
        /// Creates the connection information.
        /// </summary>
        /// <param name="authent">The chosen SSH authent method</param>
        /// <returns>The ConnectionInfo object</returns>
        private ConnectionInfo CreateConnectionInfo(AuthentMethod authent)
        {
            ConnectionInfo connectionInfo;
            AuthenticationMethod authenticationMethod = null;

            if (authent == AuthentMethod.ByLoginPassword)
            { 
                authenticationMethod =
                   new PasswordAuthenticationMethod(
                       Environment.GetEnvironmentVariable("TfsExtractSSH_User", EnvironmentVariableTarget.User),
                       Environment.GetEnvironmentVariable("TfsExtractSSH_Password", EnvironmentVariableTarget.User));
            }
            else
            {
               
                using (var stream = new FileStream(Environment.GetEnvironmentVariable("TfsExtractSSH_KeyPath", EnvironmentVariableTarget.User), FileMode.Open, FileAccess.Read))
                {
                    var privateKeyFile = new PrivateKeyFile(stream);
                    authenticationMethod =
                        new PrivateKeyAuthenticationMethod(Environment.GetEnvironmentVariable("TfsExtractSSH_User", EnvironmentVariableTarget.User), privateKeyFile); 
                }
            }

            connectionInfo = new ConnectionInfo(
            Environment.GetEnvironmentVariable("TfsExtractSSH_Host", EnvironmentVariableTarget.User),
            Environment.GetEnvironmentVariable("TfsExtractSSH_User", EnvironmentVariableTarget.User),
            authenticationMethod);

            logger.Info("Created connection to {0}@{1}", Environment.GetEnvironmentVariable("TfsExtractSSH_User", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsExtractSSH_Host", EnvironmentVariableTarget.User));
            return connectionInfo;
        }
    }
}
