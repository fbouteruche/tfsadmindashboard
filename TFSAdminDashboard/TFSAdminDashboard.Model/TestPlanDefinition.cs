using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class TestPlanDefinition
    {
        public string Name
        {
            get;
            set;
        }

        public string AreaPath
        {
            get;
            set;
        }

        public string IterationPath
        {
            get;
            set;
        }

        public string Owner
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public DateTime LastUpdate
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public int Revision 
        {
            get;
            set;
        }
    }
}
