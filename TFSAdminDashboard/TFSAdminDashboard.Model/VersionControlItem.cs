using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class VersionControlItem
    {
        public string DisplayName
        {
            get;
            set;
        }

        public int ItemChangeSetId
        {
            get;
            set;
        }

        public DateTime ItemLastCheckIn
        {
            get;
            set;
        }

        public int InnerChangeSetId
        {
            get;
            set;
        }

        public DateTime InnerLastCheckIn
        {
            get;
            set;
        }
    }
}
