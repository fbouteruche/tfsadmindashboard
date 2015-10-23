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
    public class SFTPService : ISFTPService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public void UploadFile(string filePath)
        {
            using (var sftp = new SftpClient(CreateConnectionInfo()))
            {
                var fileStream = File.OpenRead(filePath);
                sftp.Connect();
                string uploadPath = string.Format("{0}/{1}", Environment.GetEnvironmentVariable("TfsExtractSSH_Path", EnvironmentVariableTarget.User), Path.GetFileName(filePath));

                sftp.UploadFile(fileStream, uploadPath);

                sftp.Disconnect();

                fileStream.Close();
            }

            logger.Info("File uploaded");
        }

        private ConnectionInfo CreateConnectionInfo()
        {
            ConnectionInfo connectionInfo;

            AuthenticationMethod authenticationMethod =
                   new PasswordAuthenticationMethod(
                       Environment.GetEnvironmentVariable("TfsExtractSSH_User", EnvironmentVariableTarget.User),
                       Environment.GetEnvironmentVariable("TfsExtractSSH_Password", EnvironmentVariableTarget.User));

            connectionInfo = new ConnectionInfo(
            Environment.GetEnvironmentVariable("TfsExtractSSH_Host", EnvironmentVariableTarget.User),
            Environment.GetEnvironmentVariable("TfsExtractSSH_User", EnvironmentVariableTarget.User),
            authenticationMethod);

            logger.Info("Created connection to {0}@{1}", Environment.GetEnvironmentVariable("TfsExtractSSH_User", EnvironmentVariableTarget.User), Environment.GetEnvironmentVariable("TfsExtractSSH_Host", EnvironmentVariableTarget.User));
            return connectionInfo;
        }
    }
}
