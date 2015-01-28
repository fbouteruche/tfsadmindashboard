using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TFSAdminDashboard.DTO
{
    public class ProjectDefinition
    {
        public ProjectDefinition()
        {
        }

        public string CollectionName
        {
            get;
            set;

        }
        public string Name
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public string State
        {
            get;
            set;
        }

        public string Uri
        {
            get;
            set;
        }

        private DateTime utcCreationDate;
        public DateTime UtcCreationDate
        {
            get
            {
                return utcCreationDate.ToUniversalTime();
            }
            set
            {
                utcCreationDate = value;
            }
        }
        
    }
}
