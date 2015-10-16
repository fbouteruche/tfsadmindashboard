using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSAdminDashboard.DTO;

namespace TfsAdminDashboardConsole.tests
{
    [TestFixture]
    public class TestData
    {
        [Test]
        public void GetProjectInfosFromJSon()
        {
            int yearToConsider = 2015;

            string fileContent = File.ReadAllText(@"C: \Users\Vinzz\Desktop\Drop\2015_10_15_TFS2010_ExtractProject.json");

            ICollection<ProjectDefinition> projectList = JsonConvert.DeserializeObject<ICollection<ProjectDefinition>>(fileContent);

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
