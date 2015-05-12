using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSAdminDashboard.DTO
{
    public class ApplicationGroupDefinition
    {
        private List<User> users = new List<User>();

        public string Name
        {
            get;
            set;
        }

        public ICollection<User> UserCollection
        {
            get
            {
                return users;
            }
        }

        public void SortUsers()
        {
            users = users.OrderBy(x => x.Name).ToList();
        }
    }
}
