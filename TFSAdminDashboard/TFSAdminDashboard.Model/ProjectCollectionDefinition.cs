using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TFSAdminDashboard.DTO
{
    public class ProjectCollectionDefinition
    {
        public ProjectCollectionDefinition()
        {
        }

        public string InstanceId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int ProjectCount
        {
            get;
            set;
        }
    }
}
