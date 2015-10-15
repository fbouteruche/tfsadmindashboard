using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Client.Catalog.Objects;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TFSAdminDashboard.DataAccess
{
    public class TeamProjectHelper
    {
        /// <summary>
        /// Gets all projects using the ICommonStructureService.
        /// </summary>
        /// <param name="configurationServer">The configuration server.</param>
        /// <returns></returns>
        public static ICollection<ProjectDefinition> GetAllProjects(TfsConfigurationServer configurationServer)
        {
            List<ProjectDefinition> projectList = new List<ProjectDefinition>();
            ITeamProjectCollectionService collectionService = configurationServer.GetService<ITeamProjectCollectionService>();
            if (collectionService != null)
            {
                IList<TeamProjectCollection> collections = collectionService.GetCollections();

                foreach (TeamProjectCollection collection in collections)
                {
                    if (collection.State == TeamFoundationServiceHostStatus.Started)
                    {
                        TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(collection.Id);


                        VersionControlServer vcs = tpc.GetService<VersionControlServer>();
                        Microsoft.TeamFoundation.VersionControl.Client.TeamProject[] projects = vcs.GetAllTeamProjects(true);

                        //e.g. TFSVC based project
                        if (projects.Length > 0)
                        {
                            int processed = 0;

                            Console.WriteLine("{0} project to extract in collection {1}", projects.Length, collection.Name);
                            foreach (Microsoft.TeamFoundation.VersionControl.Client.TeamProject project in projects)
                            {
                                ++processed;

                                Console.WriteLine("Process {2} - {0}/{1}", processed, projects.Length, project.Name);
                                string name = project.Name;
                                IEnumerable<Changeset> changesets = vcs.QueryHistory(project.ServerItem, VersionSpec.Latest, 0, RecursionType.None, String.Empty, null, VersionSpec.Latest, int.MaxValue, true, false, false, true).OfType<Changeset>();
                                Changeset firstChangeset = changesets.FirstOrDefault();
                                if (firstChangeset != null)
                                {
                                    DateTime creationDate = firstChangeset.CreationDate;

                                    ProjectDefinition projectDefinition = new ProjectDefinition();
                                    projectDefinition.Name = project.Name;
                                    projectDefinition.CollectionDescription = collection.Description;
                                    projectDefinition.Uri = project.ArtifactUri.Segments[3]; // TODO: maybe not the nicest way to get the URI
                                    projectDefinition.CollectionName = collection.Name;
                                    projectDefinition.UtcCreationDate = creationDate.ToUniversalTime();

                                    // get Workitems data
                                    projectDefinition.WorkItemDefinitionCollection = DashWorkItemHelper.FeedWorkItemData(tpc, projectDefinition.Name);

                                    // get build data
                                    projectDefinition.BuildsDefinitionCollection = DashBuildHelper.FeedBuildData(tpc, projectDefinition.Name);

                                    // get VCS data (only TFS 2010 TFSVC though)
                                    projectDefinition.VersionControlData = DashVersionControlHelper.FeedVersionControlData(tpc, projectDefinition.Name);

                                    projectDefinition.LastCheckinDate = projectDefinition.VersionControlData.Max(x=> x.InnerLastCheckIn);

                                    // get test plan DAta
                                    projectDefinition.TestPlanData = DashTestPlanHelper.FeedTestPlanData(tpc, projectDefinition.Name);

                                    projectList.Add(projectDefinition);
                                }

#if TEST
                                // Break after 1 project for tests
                                break;
#endif

                            }
                        }
                        else
                        // Git based project
                        {
                            ProjectCollection projCollect = (ProjectCollection)tpc.GetService(typeof(ProjectCollection));

                            var structService = tpc.GetService<ICommonStructureService>();

                            foreach (ProjectInfo p in structService.ListAllProjects())
                            {
                                ProjectDefinition projectDefinition = new ProjectDefinition()
                                {
                                    Name = p.Name,
                                    CollectionName = collection.Name,
                                    Uri = p.Uri,
                                    UtcCreationDate = DateTime.MinValue // TODO: How to get the creation date...
                                };
                                projectList.Add(projectDefinition);
                                
                                // Here get witems data
                            }
                        }
                    }


#if TEST
                    // Break after 1 project for tests
                    break;
#endif
                }
            }

            return projectList.OrderBy(x => x.Name).ToList();
        }
    }
}
