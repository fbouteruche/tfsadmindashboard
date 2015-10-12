﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.Models
{
    public class VersionControlOverviewModel
    {
        private List<VersionControlItem> versionControlItemCollection = new List<VersionControlItem>();

        public void SetVersionControlItemCollection(List<VersionControlItem> value)
        {
            versionControlItemCollection = value;
        }
        public ICollection<VersionControlItem>  VersionControlItemCollection
        {
            get
            {
                return versionControlItemCollection;
            }
        }
    }
}