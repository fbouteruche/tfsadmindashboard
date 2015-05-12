using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TFSAdminDashboard.DataAccess;

namespace TFSAdminDropFolderRights
{
    public class DropFolderRightsManager : TFSAccessHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Fetch all projects in each TPC, construct a TPC\Project directory structure in the centralised Drop Folder location
        /// Then gives read rights to any project user.
        /// </summary>
        public void SetDropRights()
        {
            //ToDo
        }
    }
}
