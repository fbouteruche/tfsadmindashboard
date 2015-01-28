using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;
using Microsoft.TeamFoundation.Framework.Client;
using System.Collections.ObjectModel;
using Microsoft.TeamFoundation.Framework.Common;

namespace TFSAdminDashboard.DataAccess
{
    public class CatalogNodeBrowsingHelper
    {
        public static ICollection<DataCollectorDefinition> GetDataCollectors(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> dataCollectorNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.DataCollector }, false, CatalogQueryOptions.None);
            ICollection<DataCollectorDefinition> dataCollectorCollection = new List<DataCollectorDefinition>();
            foreach (CatalogNode item in dataCollectorNode)
            {
                DataCollectorDefinition dataCollectorDefinition = new DataCollectorDefinition();
                dataCollectorDefinition.Name = item.Resource.DisplayName;
                if (item.Resource.Properties.ContainsKey("__Reserved_DisplayName"))
                {
                    dataCollectorDefinition.Name += (" - " + item.Resource.Properties["__Reserved_DisplayName"]);
                }
                dataCollectorCollection.Add(dataCollectorDefinition);
            }
            return dataCollectorCollection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationalCatalog"></param>
        /// <returns></returns>
        /// <remarks>
        /// process guidance site definitions are collection-level data
        /// </remarks>
        public static ICollection<ProcessGuidanceSiteDefinition> GetProcessGuidanceSites(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> processGuidanceSiteNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.ProcessGuidanceSite }, false, CatalogQueryOptions.None);
            ICollection<ProcessGuidanceSiteDefinition> collection = new List<ProcessGuidanceSiteDefinition>();
            foreach (CatalogNode item in processGuidanceSiteNode)
            {
                ProcessGuidanceSiteDefinition definition = new ProcessGuidanceSiteDefinition();
                definition.Name = item.Resource.DisplayName;
                if (item.Resource.Properties.ContainsKey("RelativePath"))
                {
                    definition.RelativePath = item.Resource.Properties["RelativePath"];
                }
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get project collection definitions contained in the child nodes of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">the catalog to browse</param>
        /// <returns>the project collection definitions</returns>
        /// <remarks>
        /// the project collection definitions are server-level data
        /// </remarks>
        public static ICollection<ProjectCollectionDefinition> GetProjectCollections(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> projectCollectionNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);
            ICollection<ProjectCollectionDefinition> collection = new List<ProjectCollectionDefinition>();
            foreach (CatalogNode item in projectCollectionNode)
            {
                
                ProjectCollectionDefinition definition = new ProjectCollectionDefinition();
                definition.Name = item.Resource.DisplayName;
                definition.ProjectCount = item.QueryChildren(new[] { CatalogResourceTypes.TeamProject }, false, CatalogQueryOptions.None).Count;
                if (item.Resource.Properties.ContainsKey("InstanceId"))
                {
                    definition.InstanceId = new Guid(item.Resource.Properties["InstanceId"]);
                }
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the project portal definitions contained in the child nodes of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">the catalog to browse</param>
        /// <returns>the project portal definitions</returns>
        /// <remarks>
        /// the project portal definitions are collection-level data
        /// </remarks>
        public static ICollection<ProjectPortalDefinition> GetProjectPortals(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> projectPortalNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.ProjectPortal }, false, CatalogQueryOptions.None);
            ICollection<ProjectPortalDefinition> collection = new List<ProjectPortalDefinition>();
            foreach (CatalogNode item in projectPortalNode)
            {
                ProjectPortalDefinition definition = new ProjectPortalDefinition();
                definition.Name = item.Resource.DisplayName;
                if (item.Resource.Properties.ContainsKey("RelativePath"))
                {
                    definition.RelativePath = item.Resource.Properties["RelativePath"];
                }
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the reporting configuration definitions contained in the child node of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">the catalog to browse</param>
        /// <returns>the reporting configuration definitions</returns>
        /// <remarks>
        /// the reporting configuration definitions are server-level data
        /// </remarks>
        public static ICollection<ReportingConfigurationDefinition> GetReportingConfigurations(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> reportingConfigurationNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.ReportingConfiguration }, false, CatalogQueryOptions.None);
            ICollection<ReportingConfigurationDefinition> collection = new List<ReportingConfigurationDefinition>();
            foreach (CatalogNode item in reportingConfigurationNode)
            {
                ReportingConfigurationDefinition definition = new ReportingConfigurationDefinition();
                definition.Name = item.Resource.DisplayName;
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the reporting folder definitions contained in the child nodes of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">the catalog to browse</param>
        /// <returns>the reporting folder definitions</returns>
        /// <remarks>
        /// the reporting folder definitions are collection-level data
        /// </remarks>
        public static ICollection<ReportingFolderDefinition> GetReportingFolders(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> reportingFolderNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.ReportingFolder }, false, CatalogQueryOptions.None);
            ICollection<ReportingFolderDefinition> collection = new List<ReportingFolderDefinition>();
            foreach (CatalogNode item in reportingFolderNode)
            {
                ReportingFolderDefinition definition = new ReportingFolderDefinition();
                definition.Name = item.Resource.DisplayName;
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the reporting server definitions contained in the child nodes of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">the catalog to browse</param>
        /// <returns>the reporting server defintions</returns>
        /// <remarks>
        /// the reporting server definition are server-level data
        /// </remarks>
        public static ICollection<ReportingServerDefinition> GetReportingServers(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> reportingServerNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.ReportingServer }, false, CatalogQueryOptions.None);
            ICollection<ReportingServerDefinition> collection = new List<ReportingServerDefinition>();
            foreach (CatalogNode item in reportingServerNode)
            {
                ReportingServerDefinition definition = new ReportingServerDefinition();
                definition.Name = item.Resource.DisplayName;
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the sharepoint site creation location definitions contained in the child nodes of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">the catalog to browse</param>
        /// <returns>the sharepoint site creation location definitions</returns>
        /// <remarks>
        /// sharepoint site creation location definitions are collection-level data
        /// </remarks>
        public static ICollection<SharePointSiteCreationLocationDefinition> GetSharePointSiteCreationLocations(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> sharePointSiteCreationLocationNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.SharePointSiteCreationLocation }, false, CatalogQueryOptions.None);
            ICollection<SharePointSiteCreationLocationDefinition> collection = new List<SharePointSiteCreationLocationDefinition>();
            foreach (CatalogNode item in sharePointSiteCreationLocationNode)
            {
                SharePointSiteCreationLocationDefinition definition = new SharePointSiteCreationLocationDefinition();
                definition.Name = item.Resource.DisplayName;
                collection.Add(definition);
            }
            return collection;
        }
        /// <summary>
        /// Get the sharepoint web application definitions contained in the child nodes of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">the catalog to browse</param>
        /// <returns>the sharepoint web application definitions</returns>
        /// <remarks>
        /// sharepoint web application definitions are server-level data
        /// </remarks>
        public static ICollection<SharePointWebApplicationDefinition> GetSharePointWebApplications(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> sharePointWebApplicationNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.SharePointWebApplication }, false, CatalogQueryOptions.None);
            ICollection<SharePointWebApplicationDefinition> collection = new List<SharePointWebApplicationDefinition>();
            foreach (CatalogNode item in sharePointWebApplicationNode)
            {
                SharePointWebApplicationDefinition definition = new SharePointWebApplicationDefinition();
                definition.Name = item.Resource.DisplayName;
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the team foundation server instance definitions contained in the child nodes of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">the catalog to browse</param>
        /// <returns>
        /// the team foundation server instance definitions
        /// </returns>
        /// <remarks>
        /// team foundation server instance definitions are server-level data
        /// </remarks>
        public static ICollection<TeamFoundationServerInstanceDefinition> GetTeamFoundationServerInstances(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> teamFoundationServerInstanceNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.TeamFoundationServerInstance }, false, CatalogQueryOptions.None);
            ICollection<TeamFoundationServerInstanceDefinition> collection = new List<TeamFoundationServerInstanceDefinition>();
            foreach (CatalogNode item in teamFoundationServerInstanceNode)
            {
                TeamFoundationServerInstanceDefinition definition = new TeamFoundationServerInstanceDefinition();
                definition.Name = item.Resource.DisplayName;
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the team project definitions contained in the child nodes of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">
        /// the catalog to browse
        /// </param>
        /// <returns>
        /// the team project definitions
        /// </returns>
        /// <remarks>
        /// team project definitions are collection-level data
        /// </remarks>
        public static ICollection<ProjectDefinition> GetTeamProjects(CatalogNode organizationalCatalog, bool recurse)
        {
            ReadOnlyCollection<CatalogNode> teamProjectNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.TeamProject }, recurse, CatalogQueryOptions.None);
            ICollection<ProjectDefinition> collection = new List<ProjectDefinition>();
            foreach (CatalogNode item in teamProjectNode)
            {
                ProjectDefinition definition = new ProjectDefinition();
                definition.Name = item.Resource.DisplayName;
                definition.Id = new Guid(item.Resource.Properties["ProjectId"]);
                definition.State = item.Resource.Properties["ProjectState"];
                definition.Uri = item.Resource.Properties["ProjectUri"];
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the Team System web access definitions contained in the child nodes of the catalog
        /// </summary>
        /// <param name="organizationalCatalog">
        /// the catalog to browse
        /// </param>
        /// <returns>
        /// The Team System web access definitions
        /// </returns>
        /// <remarks>
        /// Team System web access definitions are collection-level data
        /// </remarks>
        public static ICollection<TeamSystemWebAccessDefinition> GetTeamSystemWebAccess(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> teamSystemWebAccessNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.TeamSystemWebAccess }, false, CatalogQueryOptions.None);
            ICollection<TeamSystemWebAccessDefinition> collection = new List<TeamSystemWebAccessDefinition>();
            foreach (CatalogNode item in teamSystemWebAccessNode)
            {
                TeamSystemWebAccessDefinition definition = new TeamSystemWebAccessDefinition();
                definition.Name = item.Resource.DisplayName;
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the test controller definitions contained in the child nodes of the Catalog
        /// </summary>
        /// <param name="organizationalCatalog">The catalog to browse</param>
        /// <returns>a collection of test controller definitions</returns>
        /// <remarks>
        /// test controller definitions are collection-level data
        /// </remarks>
        public static ICollection<TestControllerDefinition> GetTestControllers(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> testControllerNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.TestController }, false, CatalogQueryOptions.None);
            ICollection<TestControllerDefinition> collection = new List<TestControllerDefinition>();
            foreach (CatalogNode item in testControllerNode)
            {
                TestControllerDefinition definition = new TestControllerDefinition();
                definition.Name = item.Resource.DisplayName;
                collection.Add(definition);
            }
            return collection;
        }

        /// <summary>
        /// Get the test environment definitions contained in the child nodes of the Catalog
        /// </summary>
        /// <param name="organizationalCatalog">The catalog to browse</param>
        /// <returns>a collection of test environment definitions</returns>
        /// <remarks>
        /// test environment definitions are collection-level data
        /// </remarks>
        public static ICollection<TestEnvironmentDefinition> GetTestEnvironments(CatalogNode organizationalCatalog)
        {
            ReadOnlyCollection<CatalogNode> testEnvironmentNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.TestEnvironment }, false, CatalogQueryOptions.None);
            ICollection<TestEnvironmentDefinition> collection = new List<TestEnvironmentDefinition>();
            foreach (CatalogNode item in testEnvironmentNode)
            {
                TestEnvironmentDefinition definition = new TestEnvironmentDefinition();
                definition.Name = item.Resource.DisplayName;
                collection.Add(definition);
            }
            return collection;
        }
    }
}
