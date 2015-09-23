using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using Microsoft.TeamFoundation.Framework.Client;

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
            //Fetch each project
            ICollection<ProjectDefinition> projectList = TeamProjectHelper.GetAllProjects(configurationServer);

            IIdentityManagementService ims = configurationServer.GetService<IIdentityManagementService>();

            foreach (ProjectDefinition proj in projectList)
            {
                ICollection<ApplicationGroupDefinition> applicationGroupCollection = new List<ApplicationGroupDefinition>();
                ICollection<User> userCollection = new List<User>();

                IdentityServiceManagementHelper.FeedIdentityData(applicationGroupCollection, userCollection, ims, proj.Uri);
            }

        }
    }
}
