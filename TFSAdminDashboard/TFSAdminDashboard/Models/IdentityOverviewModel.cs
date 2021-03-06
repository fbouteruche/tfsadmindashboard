﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.Models
{
    public class IdentityOverviewModel
    {
        private List<ApplicationGroupDefinition> applicationGroupCollection = new List<ApplicationGroupDefinition>();

        private List<User> userCollection = new List<User>();

        public ICollection<ApplicationGroupDefinition> ApplicationGroupCollection
        {
            get
            {
                return applicationGroupCollection;
            }
        }

        public ICollection<User> UserCollection
        {
            get
            {
                return userCollection;
            }
        }
    }
}