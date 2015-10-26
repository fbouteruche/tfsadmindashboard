using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Client.Catalog.Objects;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;
using Microsoft.TeamFoundation.VersionControl.Client;
using NLog;
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
        static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets all projects using the ICommonStructureService.
        /// </summary>
        /// <param name="configurationServer">The configuration server.</param>
        /// <param name="withIdentities">if set to <c>true</c> [with identities].</param>
        /// <returns>the collection of projects definitions</returns>
        public static ICollection<ProjectDefinition> GetAllProjects(TfsConfigurationServer configurationServer, bool withIdentities)
        {
            List<ProjectDefinition> projectList = new List<ProjectDefinition>();
            ITeamProjectCollectionService collectionService = configurationServer.GetService<ITeamProjectCollectionService>();
            if (collectionService != null)
            {
                IList<TeamProjectCollection> collections = collectionService.GetCollections();

                int processedColl = 0;

                foreach (TeamProjectCollection collection in collections)
                {
                    if (collection.Name == "OBS")
                    {
                        logger.Info("Filter out this collection: {0}", collection.Name);
                        continue;
                    }

                    ++processedColl;
                    logger.Info("OoO Collection {2} - {0}/{1}", processedColl, collections.Count, collection.Name);
                    if (collection.State == TeamFoundationServiceHostStatus.Started)
                    {
                        TfsTeamProjectCollection tpc = configurationServer.GetTeamProjectCollection(collection.Id);


                        VersionControlServer vcs = tpc.GetService<VersionControlServer>();
                        Microsoft.TeamFoundation.VersionControl.Client.TeamProject[] projects = vcs.GetAllTeamProjects(true);

                        //e.g. TFSVC based project
                        if (projects.Length > 0)
                        {
                            int processed = 0;

                            logger.Info("   {0} project to extract in collection {1}", projects.Length, collection.Name);
                            foreach (Microsoft.TeamFoundation.VersionControl.Client.TeamProject project in projects)
                            {
                                ++processed;

                                logger.Info("       Process {2} - {0}/{1}", processed, projects.Length, project.Name);
                                string name = project.Name;
                                IEnumerable<Changeset> changesets = vcs.QueryHistory(project.ServerItem, VersionSpec.Latest, 0, RecursionType.None, String.Empty, null, VersionSpec.Latest, int.MaxValue, true, false, false, true).OfType<Changeset>();
                                Changeset firstChangeset = changesets.FirstOrDefault();
                                if (firstChangeset != null)
                                {
                                    DateTime creationDate = firstChangeset.CreationDate;

                                    ProjectDefinition projectDefinition = new ProjectDefinition();
                                    projectDefinition.Name = project.Name;
                                    projectDefinition.Id = new Guid(project.ArtifactUri.Segments[3]);
                                    projectDefinition.CollectionDescription = collection.Description;
                                    projectDefinition.Uri = project.ArtifactUri.ToString();
                                    projectDefinition.State = "N/A"; // Todo retrieve this
                                    projectDefinition.CollectionName = collection.Name;
                                    projectDefinition.UtcCreationDate = creationDate.ToUniversalTime();

                                    // get Workitems data
                                    projectDefinition.WorkItemDefinitionCollection = DashWorkItemHelper.FeedWorkItemData(tpc, projectDefinition.Name);

                                    // get build data
                                    projectDefinition.BuildsDefinitionCollection = DashBuildHelper.FeedBuildData(tpc, projectDefinition.Name);

                                    if (projectDefinition.BuildsDefinitionCollection.Count > 0)
                                    {
                                        projectDefinition.LastSuccessBuild = projectDefinition.BuildsDefinitionCollection.Max(x => x.LastSuccess);
                                        projectDefinition.LastFailedBuild = projectDefinition.BuildsDefinitionCollection.Max(x => x.LastFail);
                                    }

                                    // get VCS data (only TFS 2010 TFSVC though)
                                    projectDefinition.VersionControlData = DashVersionControlHelper.FeedVersionControlData(tpc, projectDefinition.Name);

                                    projectDefinition.LastCheckinDate = projectDefinition.VersionControlData.Max(x => x.InnerLastCheckIn);

                                    // get test plan Data
                                    projectDefinition.TestPlanData = DashTestPlanHelper.FeedTestPlanData(tpc, projectDefinition.Name);

                                    // get identities Data
                                    if(withIdentities)
                                        projectDefinition.IdentityData = IdentityServiceManagementHelper.FeedIdentityData(tpc, projectDefinition.Uri).Item2;

                                    projectDefinition.DMOrigin = "AD"; // TODO extract from collection description

                                    projectDefinition.Platform = "TFS2010";
                                    projectDefinition.ExtractDate = DateTime.Now;
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
                            int processed = 0;

                            ProjectCollection projCollect = (ProjectCollection)tpc.GetService(typeof(ProjectCollection));

                            var structService = tpc.GetService<ICommonStructureService>();
                            var totalProjects = structService.ListAllProjects();
                            logger.Info("   {0} project to extract in collection {1}", totalProjects.Length, collection.Name);
                            foreach (ProjectInfo p in totalProjects)
                            {
                                ++processed;

                                logger.Info("       Process {2} - {0}/{1}", processed, totalProjects.Length, p.Name);
                                ProjectDefinition projectDefinition = new ProjectDefinition()
                                {
                                    Id = new Guid(new Uri(p.Uri).Segments[3]),
                                    Name = p.Name,
                                    CollectionName = collection.Name,
                                    Uri = p.Uri,
                                    State = p.Status.ToString(),
                                    UtcCreationDate = DateTime.MinValue // TODO: How to get the creation date...
                                };

                                // Here get witems data, etc.

                                // get Workitems data
                                projectDefinition.WorkItemDefinitionCollection = DashWorkItemHelper.FeedWorkItemData(tpc, projectDefinition.Name);

                                // get build data
                                projectDefinition.BuildsDefinitionCollection = DashBuildHelper.FeedBuildData(tpc, projectDefinition.Name);

                                if (projectDefinition.BuildsDefinitionCollection.Count > 0)
                                {
                                    projectDefinition.LastSuccessBuild = projectDefinition.BuildsDefinitionCollection.Max(x => x.LastSuccess);
                                    projectDefinition.LastFailedBuild = projectDefinition.BuildsDefinitionCollection.Max(x => x.LastFail);
                                }

                                // TODO get VCS data
                                //projectDefinition.VersionControlData = null;
                                //projectDefinition.LastCheckinDate = projectDefinition.VersionControlData.Max(x => x.InnerLastCheckIn);

                                // get test plan Data
                                projectDefinition.TestPlanData = DashTestPlanHelper.FeedTestPlanData(tpc, projectDefinition.Name);

                                if (withIdentities)
                                { 
                                    // get identities Data
                                    var ids = IdentityServiceManagementHelper.FeedIdentityData(tpc, projectDefinition.Uri).Item2;
                                
                                    // Do not fetch all company identities
                                    if (ids.Count < 3000)
                                        projectDefinition.IdentityData = ids;
                                }

                                projectDefinition.Platform = "TFS2013";

                                projectDefinition.DMOrigin = projectDefinition.CollectionName;

                                projectDefinition.ExtractDate = DateTime.Now;

                                projectList.Add(projectDefinition);
#if TEST
                                // Break after 1 project for tests
                                break;
#endif
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
