﻿using System;
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

        public DateTime? UtcCreationDate
        {
            get;
            set;
        }

        public List<WorkItemDefinition> WorkItemDefinitionCollection { get; set; }
        public List<BuildDefinition> BuildsDefinitionCollection { get; set; }
        public List<VersionControlItem> VersionControlData { get; set; }
        public List<TestPlanDefinition> TestPlanData { get; set; }
        public string CollectionDescription { get; set; }
        public DateTime? LastCheckinDate { get; set; }
        public DateTime? LastSuccessBuild { get; set; }
        public DateTime? LastFailedBuild { get; set; }
        public string Platform { get; set; }
        public DateTime ExtractDate { get; set; }
        public List<User> IdentityData { get; set; }
        public string DMOrigin { get; set; }
        public string ProjectCode { get; set; }
        public double BuildHealth
        {
            get
            {
                if (BuildsDefinitionCollection.Count > 0)
                {
                    return BuildsDefinitionCollection.Average(x => x.Health);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
