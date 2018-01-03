using MoreLinq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;
using TFSDataService;
using TFSDataService.JsonBusinessObjects;

namespace TFSAdminDashboard.DataAccess
{
    public class DashTeamProjectHelper
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        public static ICollection<ProjectCollectionDefinition> GetCollections()
        {
            List<ProjectCollectionDefinition> collectionList = new List<ProjectCollectionDefinition>();

            var collections = DataServiceTeamProjects.Collections().Where(x => x.state == "Started");

            foreach (TeamProjectCollection currCollection in collections)
            {
                ProjectCollectionDefinition collection = new ProjectCollectionDefinition()
                {
                    InstanceId = currCollection.id,
                    Name = currCollection.name,
                    ProjectCount = DataServiceTeamProjects.Projects(currCollection.name).Count
                };

                collectionList.Add(collection);
            }

            return collectionList;
        }

        static int processedColl = 0;
        static int processed = 0;
        static List<ProjectSimpleDefinition> projectList = new List<ProjectSimpleDefinition>();

#if QUICKTEST
        static bool quicktested = false;
#endif
        public static ICollection<ProjectSimpleDefinition> GetAllProjectsSimple()
        {
            string tfsUrl = Environment.GetEnvironmentVariable("TfsUrl");
            string reportUrl = "https://almnet.orangeapplicationsforbusiness.com/Reports/Pages/Report.aspx?ItemPath=%2fTfsReports%2f{0}%2f{1}%2fProject+Management%2fRequirements+Overview";

            var collections = DataServiceTeamProjects.Collections().Where(x => x.state == "Started");

            foreach (TeamProjectCollection currCollection in collections)
            {
#if QUICKTEST
                if (currCollection.name != Environment.GetEnvironmentVariable("QuickTestCollection"))
                    continue;

                logger.Info("QUICKTEST mode, consider only the {0} collection", Environment.GetEnvironmentVariable("QuickTestCollection"));
#endif
                ++processedColl;
                processed = 0;
                logger.Info("OoO Collection {0} - {1}/{2}", currCollection.name, processedColl, collections.Count());

                var collProjects = DataServiceTeamProjects.Projects(currCollection.name);

                logger.Info("   {0} project to extract in collection {1}", collProjects.Count, currCollection.name);


                Parallel.ForEach(collProjects, (project) =>
                {
                    ExtractInfos(projectList, tfsUrl, reportUrl, currCollection, collProjects, project);
                });

#if QUICKTEST
                if (quicktested)
                {
                    logger.Info("TEST mode, stop after the first collection");
                    break;
                }
#endif
            }

            logger.Info("Extract done");
            return projectList.OrderBy(x => x.Name).ToList();
        }

        private static void ExtractInfos(List<ProjectSimpleDefinition> projectList, string tfsUrl, string reportUrl, TeamProjectCollection currCollection, List<TeamProject> collProjects, TeamProject project)
        {
#if QUICKTEST
            if (project.name != Environment.GetEnvironmentVariable("QuickTestProject"))
                return;

            logger.Info("QUICKTEST mode, consider only the {0} project", Environment.GetEnvironmentVariable("QuickTestProject"));
            quicktested = true;
#endif
            ++processed;

            try
            {
                logger.Info("       Process {2} - {0}/{1}", processed, collProjects.Count, project.name);
                ProjectSimpleDefinition projectDefinition = new ProjectSimpleDefinition();

                // General data
                logger.Trace($"{project.name} - General Data");
                projectDefinition.Name = string.Format("{0}/{1}", currCollection.name, project.name);
                projectDefinition.Id = project.id;

                projectList.Add(projectDefinition);
            }
            catch(Exception e)
            {
                logger.Info(e, $"error for {currCollection.name}/{project.name}");
                throw;
            }
            }
    }
}
