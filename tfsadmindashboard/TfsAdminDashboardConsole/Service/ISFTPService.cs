using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsAdminDashboardConsole.Service
{
    /// <summary>
    /// Methods exposed by the SFTP service
    /// </summary>
    interface ISFTPService
    {

        /// <summary>
        /// Uploads the file via SFTP.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="authent">The chosen SSH authent method</param>
        void UploadFile(string filePath, AuthentMethod authent);
    }
}
