using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TFSAdminDashboard.DTO;
using MoreLinq;

namespace TfsAdminDashboardConsole.tests
{
    [TestFixture]
    public class TestData
    {
        [Test]
        public void GetUserInfosFromJSon()
        {
            string fileContent = File.ReadAllText(@"C:\Users\Vinzz\Desktop\Drop\2015_10_20_TFS2013_ExtractProject.json");

            ICollection<ProjectDefinition> projectList = JsonConvert.DeserializeObject<ICollection<ProjectDefinition>>(fileContent);

            List<User> listAll = new List<User>();

            foreach (ProjectDefinition proj in projectList)
            {
                if (proj.IdentityData != null)
                    listAll.AddRange(proj.IdentityData);
            }

            var filtered = listAll.DistinctBy(x => x.Mail).Count();
        }

        [Test]
        public void GetActiveProjectInfosFromJSon()
        {
            int yearToConsider = 2015;

            string fileContent = File.ReadAllText(@"C: \Users\Vinzz\Desktop\Drop\2015_10_16_TFS2010_ExtractProject.json");

            ICollection<ProjectDefinition> projectList = JsonConvert.DeserializeObject<ICollection<ProjectDefinition>>(fileContent);

            int spy = projectList.Count();

            var filtered = projectList.Where(
                x => x.BuildsDefinitionCollection.Where(
                    y => (y.LastFail.Year == yearToConsider || y.LastSuccess.Year == yearToConsider)).Count() > 0 ||
                    x.LastCheckinDate.Year == yearToConsider).Select(x => new { Name = x.Name, CollectionName = x.CollectionName }).ToList();

            StringBuilder ans = new StringBuilder();
            ans.AppendLine("Collection;Project");
            foreach (var obj in filtered)
            {
                ans.AppendLine(string.Format("{0};{1}", obj.CollectionName, obj.Name));
            }

            File.WriteAllText(@"C:\Users\Vinzz\Desktop\ProjectsActiveIn2015.csv", ans.ToString());
        }
    }
}
