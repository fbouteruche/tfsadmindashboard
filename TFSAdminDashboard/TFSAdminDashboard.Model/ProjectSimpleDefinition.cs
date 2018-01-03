using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class ProjectSimpleDefinition
    {
        /// <summary>
        /// Name of the project
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Name of the collection
        /// </summary>
        public string Collection { get; set; }

        /// <summary>
        /// TFS internal ID
        /// </summary>
        public string Id { get; set; }
    }
}
