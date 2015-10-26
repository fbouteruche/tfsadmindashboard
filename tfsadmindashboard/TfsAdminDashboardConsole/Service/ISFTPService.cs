using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsAdminDashboardConsole.Service
{
    interface ISFTPService
    {
        void UploadFile(string filePath, AuthentMethod authent);
    }
}
