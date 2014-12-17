using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.Models
{
    public class CollectionOverviewModel
    {
        public ICollection<ProjectCollectionDefinition> ProjectCollections
        {
            get;
            set;
        }
    }
}