using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TFSAdminDashboard.DataAccess;
using TFSAdminDashboard.DTO;
using TFSAdminDashboard.Models;

namespace TFSAdminDashboard.Controllers
{
    public class ServerOverviewController : Controller
    {
        private static Uri tfsUri = new Uri(TFSAdminDashboard.Properties.Settings.Default.TfsUrl);

        private TfsConfigurationServer configurationServer = new TfsConfigurationServer(tfsUri, new NetworkCredential(TFSAdminDashboard.Properties.Settings.Default.TfsUserName, TFSAdminDashboard.Properties.Settings.Default.TfsPassword));

        // GET: TfsOverview
        public ActionResult Index()
        {
            ServerOverviewModel som = new ServerOverviewModel() { Name = configurationServer.Name, TimeZone = configurationServer.TimeZone };
            
            //ITeamFoundationRegistry serverRegistry = configurationServer.GetService<ITeamFoundationRegistry>();
            
            //ITeamFoundationJobService serverJobService = configurationServer.GetService<ITeamFoundationJobService>();
            //TeamFoundationJobDefinition[] jobs = serverJobService.QueryJobs();

            //IPropertyService serverPropertyService = configurationServer.GetService<IPropertyService>();
            //IEventService serverEventService = configurationServer.GetService<IEventService>();
            //ISecurityService serverSecurityService = configurationServer.GetService<ISecurityService>();
            //ILocationService serverLocationService = configurationServer.GetService<ILocationService>();
            //TswaClientHyperlinkService serverTswaClientHyperlinkService = configurationServer.GetService<TswaClientHyperlinkService>();
            //ITeamProjectCollectionService serverProjectCollectionService = configurationServer.GetService<ITeamProjectCollectionService>();
            //IAdministrationService serverAdministrationService = configurationServer.GetService<IAdministrationService>();
            
            return View(som);
        }

        public ActionResult CollectionOverview()
        {
            CollectionOverviewModel com = new CollectionOverviewModel();
            com.ProjectCollections = CatalogNodeBrowsingHelper.GetProjectCollections(configurationServer.CatalogNode);
            return PartialView(com);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// unconsidered CatalogResourceTypes : GenericLink, ProjectServerMapping, ProjectServerRegistration, ResourceFolder, DataCollector, ProcessGuidanceSite, ProjectPortal
        /// </remarks>
        public ActionResult OrganizationalOverview()
        {
            //OrganizationalOverviewModel oom = new OrganizationalOverviewModel();

            //ICatalogService serverCatalogService = configurationServer.GetService<ICatalogService>();
            //CatalogNode configurationServerCatalogNode = configurationServer.CatalogNode;
            //CatalogNode organizationalCatalog = serverCatalogService.QueryRootNode(CatalogTree.Organizational);

            //ReadOnlyCollection<CatalogNode> teamFoundationServerInstanceNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.TeamFoundationServerInstance }, false, CatalogQueryOptions.None);
            //CatalogNode organizationalCatalog2 = teamFoundationServerInstanceNode[0];

            //ICollection<DataCollectorDefinition> dataCollectors = CatalogNodeBrowsingHelper.GetDataCollectors(organizationalCatalog);
            //ICollection<DataCollectorDefinition> dataCollectors2 = CatalogNodeBrowsingHelper.GetDataCollectors(configurationServerCatalogNode);

            //ICollection<ProjectCollection> projectCollections = CatalogNodeBrowsingHelper.GetProjectCollections(organizationalCatalog);
            //ICollection<ProjectCollection> projectCollections2 = CatalogNodeBrowsingHelper.GetProjectCollections(configurationServerCatalogNode);
            //ICollection<ProjectCollection> projectCollections3 = CatalogNodeBrowsingHelper.GetProjectCollections(organizationalCatalog2);

            //ICollection<ReportingConfigurationDefinition> reportingConfigurations = CatalogNodeBrowsingHelper.GetReportingConfigurations(organizationalCatalog);
            //ICollection<ReportingConfigurationDefinition> reportingConfigurations2 = CatalogNodeBrowsingHelper.GetReportingConfigurations(configurationServerCatalogNode);

            //ICollection<ReportingServerDefinition> reportingServers = CatalogNodeBrowsingHelper.GetReportingServers(organizationalCatalog);
            //ICollection<ReportingServerDefinition> reportingServers2 = CatalogNodeBrowsingHelper.GetReportingServers(configurationServerCatalogNode);

            //ICollection<SharePointWebApplicationDefinition> sharePointWebApplications = CatalogNodeBrowsingHelper.GetSharePointWebApplications(organizationalCatalog);
            //ICollection<SharePointWebApplicationDefinition> sharePointWebApplications2 = CatalogNodeBrowsingHelper.GetSharePointWebApplications(configurationServerCatalogNode);

            //ICollection<TeamFoundationServerInstanceDefinition> teamFoundationServerInstanceDefinitions = CatalogNodeBrowsingHelper.GetTeamFoundationServerInstances(organizationalCatalog);
            //ICollection<TeamFoundationServerInstanceDefinition> teamFoundationServerInstanceDefinitions2 = CatalogNodeBrowsingHelper.GetTeamFoundationServerInstances(configurationServerCatalogNode);
            
            return PartialView();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// unconsidered CatalogResourceTypes : GenericLink, ProjectServerMapping, ProjectServerRegistration, ResourceFolder, DataCollector, ProcessGuidanceSite, ProjectPortal
        /// </remarks>
        public ActionResult InfrastructureOverview()
        {
            ICatalogService serverCatalogService = configurationServer.GetService<ICatalogService>();
            CatalogNode organizationalCatalog = serverCatalogService.QueryRootNode(CatalogTree.Infrastructure);
            ReadOnlyCollection<CatalogNode> analysisDatabaseNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.AnalysisDatabase }, true, CatalogQueryOptions.None);
            ReadOnlyCollection<CatalogNode> applicationDatabaseNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.ApplicationDatabase }, true, CatalogQueryOptions.None);
            ReadOnlyCollection<CatalogNode> machineNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.Machine }, true, CatalogQueryOptions.ExpandDependencies);
            ReadOnlyCollection<CatalogNode> projectCollectionDatabaseNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.ProjectCollectionDatabase }, true, CatalogQueryOptions.None);
            ReadOnlyCollection<CatalogNode> sqlAnalaysisInstanceNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.SqlAnalysisInstance }, true, CatalogQueryOptions.None);
            ReadOnlyCollection<CatalogNode> sqlDatabaseInstanceNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.SqlDatabaseInstance }, true, CatalogQueryOptions.None);
            ReadOnlyCollection<CatalogNode> sqlReportingNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.SqlReportingInstance }, true, CatalogQueryOptions.None);
            ReadOnlyCollection<CatalogNode> teamFoundationWebApplicationNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.TeamFoundationWebApplication }, true, CatalogQueryOptions.None);
            ReadOnlyCollection<CatalogNode> warehouseDatabaseNode = organizationalCatalog.QueryChildren(new[] { CatalogResourceTypes.WarehouseDatabase }, true, CatalogQueryOptions.None);
            return PartialView();
        }

        public ActionResult IdentityOverview()
        {
            IIdentityManagementService serverIdentityManagementService = configurationServer.GetService<IIdentityManagementService>();
            IdentityOverviewModel iom = new IdentityOverviewModel();
            IdentityServiceManagementHelper.FeedIdentityData(iom.ApplicationGroupCollection, iom.UserCollection, serverIdentityManagementService, null);
            return PartialView(iom);
        }
    }
}