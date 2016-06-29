using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TFSDataService.JsonBusinessObjects;
using TFSDataService.Properties;
using TFSDataService.Tool;

namespace TFSDataService
{
    /// <summary>
    /// Get informations about the builds
    /// </summary>
    public static class DataServiceBuild
    {
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl", EnvironmentVariableTarget.User);

        public static List<Build> Builds(string collectionName, string projectName)
        {
            string buildsUrl = string.Format(Settings.Default.BuildUrl, tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(buildsUrl);

            BuildRootobject o = JsonConvert.DeserializeObject<BuildRootobject>(json);

            return o.value.ToList();
        }

        public static List<BuildDefinition> BuildDefinitions(string collectionName, string projectName)
        {
            string buildsDefUrl = string.Format(Settings.Default.BuildDefinitionUrl, tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(buildsDefUrl);

            BuildDefinitionRootobject o = JsonConvert.DeserializeObject<BuildDefinitionRootobject>(json);

            return o.value.ToList();
        }
    }
}
