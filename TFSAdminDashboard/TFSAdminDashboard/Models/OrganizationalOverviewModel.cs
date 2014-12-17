using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.Models
{
    public class OrganizationalOverviewModel
    {
        public ICollection<ProjectCollectionDefinition> ProjectCollectionCollection
        {
            get;
            set;
        }
    }
}