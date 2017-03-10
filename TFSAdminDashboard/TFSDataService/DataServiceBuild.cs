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
        private static string tfsServer = Environment.GetEnvironmentVariable("TfsUrl");

        static DataServiceBuild()
        {
            DataServiceBase.CheckVariables();
        }

        /// <summary>
        /// List a project's builds
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <returns>a list of builds</returns>
        public static List<Build> Builds(string collectionName, string projectName)
        {
            string buildsUrl = string.Format(Settings.Default.BuildUrl, tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(buildsUrl);

            BuildRootobject o = JsonConvert.DeserializeObject<BuildRootobject>(json);

            return o.value.ToList();
        }

        /// <summary>
        /// List a project's build definitions
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static List<BuildDefinition> BuildDefinitions(string collectionName, string projectName)
        {
            string buildsDefUrl = string.Format(Settings.Default.BuildDefinitionUrl, tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(buildsDefUrl);

            BuildDefinitionRootobject o = JsonConvert.DeserializeObject<BuildDefinitionRootobject>(json);

            return o.value.ToList();
        }

        /// <summary>
        /// Gets a build definition details
        /// </summary>
        /// <param name="definitionUrl"></param>
        /// <returns></returns>
        public static BuildDefinitionDetails BuildDefinitionsDetails(string definitionUrl)
        {
            string json = JsonRequest.GetRestResponse(definitionUrl);
            BuildDefinitionDetails o = new BuildDefinitionDetails();
            ;
            try
            {
                o = JsonConvert.DeserializeObject<BuildDefinitionDetails>(json);
                o.HasOwaspDependencyCheckEnabled = o.build.FirstOrDefault(x => x.task.id == "7363e406-cf6e-4f10-8200-281bce562769" && x.enabled) != null;
            }
            catch
            {
                // Ignore conversion errors
            }

            return o;
        }

        /// <summary>
        /// Get the build definitions templates
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        public static List<BuildDefinitionTemplate> BuildDefinitionTemplates(string collectionName, string projectName)
        {
            string buildsUrl = string.Format(Settings.Default.BuildDefinitionTemplateUrl, tfsServer, collectionName, projectName);

            string json = JsonRequest.GetRestResponse(buildsUrl);

            BuildDefinitionTemplateRootobject o = JsonConvert.DeserializeObject<BuildDefinitionTemplateRootobject>(json);

            return o.value.ToList();
        }

        /// <summary>
        /// Test to set a build templates (doesn't work on TFS 2015)
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="projectName"></param>
        /// <param name="build"></param>
        public static void SetBuildDefinitionTemplate(string collectionName, string projectName, BuildDefinitionTemplate build)
        {

            string content = @"{  ""name"": ""My Custom Template"",  ""description"": ""A custom template for a custom process"",  ""template"": {    ""build"": [      {        ""enabled"": true,        ""continueOnError"": false,        ""alwaysRun"": false,        ""displayName"": ""Build solution **\\*.sln"",        ""task"": {          ""id"": ""71a9a2d3-a98a-4caa-96ab-affca411ecda"",          ""versionSpec"": ""*""        },        ""inputs"": {          ""solution"": ""**\\*.sln"",          ""msbuildArgs"": """",          ""platform"": ""$(platform)"",          ""configuration"": ""$(config)"",          ""clean"": ""false"",          ""restoreNugetPackages"": ""true"",          ""vsLocationMethod"": ""version"",          ""vsVersion"": ""latest"",          ""vsLocation"": """",          ""msbuildLocationMethod"": ""version"",          ""msbuildVersion"": ""latest"",          ""msbuildArchitecture"": ""x86"",          ""msbuildLocation"": """",          ""logProjectEvents"": ""true""        }      },      {        ""enabled"": true,        ""continueOnError"": false,        ""alwaysRun"": false,        ""displayName"": ""Test Assemblies **\\*test*.dll;-:**\\obj\\**"",        ""task"": {          ""id"": ""ef087383-ee5e-42c7-9a53-ab56c98420f9"",          ""versionSpec"": ""*""        },        ""inputs"": {          ""testAssembly"": ""**\\*test*.dll;-:**\\obj\\**"",          ""testFiltercriteria"": """",          ""runSettingsFile"": """",          ""codeCoverageEnabled"": ""true"",          ""otherConsoleOptions"": """",          ""vsTestVersion"": ""14.0"",          ""pathtoCustomTestAdapters"": """"        }      }    ],    ""buildNumberFormat"": ""$(date:yyyyMMdd)$(rev:.r)"",    ""jobAuthorizationScope"": ""projectCollection"",    ""triggers"": [      {        ""batchChanges"": false,        ""branchFilters"": """",        ""triggerType"": ""continuousIntegration""      }    ],    ""variables"": {      ""forceClean"": {        ""value"": ""false"",        ""allowOverride"": true      },      ""config"": {        ""value"": ""debug, release"",        ""allowOverride"": true      },      ""platform"": {        ""value"": ""any cpu"",        ""allowOverride"": true      }    }  }}";
            string buildsUrl = string.Format(Settings.Default.BuildDefinitionTemplatePostUrl, tfsServer, collectionName, projectName, build.id.Replace(" ", "%20"));

            string json = JsonRequest.PostData(buildsUrl, content /*JsonConvert.SerializeObject(build)*/);
        }

        /// <summary>
        /// Get's the build agent name for a given run
        /// </summary>
        /// <param name="buildRun"></param>
        /// <returns></returns>
        public static string getWorkerName(Build buildRun)
        {
            try
            {
                if (buildRun._links.timeline == null)
                {
                    return "no record";
                }

                string json = JsonRequest.GetRestResponse(buildRun._links.timeline.href);

                BuildTimeLine o = null;

                o = JsonConvert.DeserializeObject<BuildTimeLine>(json);

                if (o == null)
                {
                    return "no record";
                }

                if (o.records.Any())
                    return o.records.First().workerName;
                else
                    return "no agent";

            }
            catch (Exception)
            {
                return "no record";
            }
        }
    }
}
