using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSDataService.JsonBusinessObjects
{

    public class BuildDefinitionTemplateRootobject
    {
        public int count { get; set; }
        public BuildDefinitionTemplate[] value { get; set; }
    }

    public class BuildDefinitionTemplate
    {
        public string id { get; set; }
        public string name { get; set; }
        public bool canDelete { get; set; }
        public string category { get; set; }
        public string iconTaskId { get; set; }
        public string description { get; set; }
        public Template template { get; set; }
    }

    public class Template
    {
        public BuildSurface[] build { get; set; }
        public string id { get; set; }

        public object[] options { get; set; }
        public Trigger[] triggers { get; set; }
        public Variables variables { get; set; }
        public string buildNumberFormat { get; set; }
        public string jobAuthorizationScope { get; set; }
        public string type { get; set; }
    }

    public class Variables
    {
        public SystemDebug systemdebug { get; set; }
        public Buildconfiguration BuildConfiguration { get; set; }
        public Buildplatform BuildPlatform { get; set; }
        public Targetprofile targetProfile { get; set; }
        public Configuration Configuration { get; set; }
        public SDK SDK { get; set; }
        public Machineusername machineUserName { get; set; }
        public Machinepassword machinePassword { get; set; }
    }

    public class SystemDebug
    {
        public string value { get; set; }
        public bool allowOverride { get; set; }
    }

    public class Buildconfiguration
    {
        public string value { get; set; }
        public bool allowOverride { get; set; }
    }

    public class Buildplatform
    {
        public string value { get; set; }
        public bool allowOverride { get; set; }
    }

    public class Targetprofile
    {
        public string value { get; set; }
        public bool allowOverride { get; set; }
    }

    public class Configuration
    {
        public string value { get; set; }
        public bool allowOverride { get; set; }
    }

    public class SDK
    {
        public string value { get; set; }
        public bool allowOverride { get; set; }
    }

    public class Machineusername
    {
        public string value { get; set; }
        public bool allowOverride { get; set; }
    }

    public class Machinepassword
    {
        public string value { get; set; }
        public bool allowOverride { get; set; }
    }

    public class BuildSurface
    {
        public bool enabled { get; set; }
        public bool continueOnError { get; set; }
        public bool alwaysRun { get; set; }
        public Task task { get; set; }
        public Inputs inputs { get; set; }
    }

    public class Task
    {
        public string id { get; set; }
        public string versionSpec { get; set; }
    }

    public class Inputs
    {
        public string solution { get; set; }
        public string platform { get; set; }
        public string configuration { get; set; }
        public string restoreNugetPackages { get; set; }
        public string testAssembly { get; set; }
        public string SearchPattern { get; set; }
        public string SymbolsArtifactName { get; set; }
        public string SourceFolder { get; set; }
        public string Contents { get; set; }
        public string TargetFolder { get; set; }
        public string PathtoPublish { get; set; }
        public string ArtifactName { get; set; }
        public string ArtifactType { get; set; }
        public string TargetPath { get; set; }
        public string msbuildArgs { get; set; }
        public string clean { get; set; }
        public string CsPkg { get; set; }
        public string CsCfg { get; set; }
        public string DeploymentLabel { get; set; }
        public string Package { get; set; }
        public string actions { get; set; }
        public string sdk { get; set; }
        public string xcWorkspacePath { get; set; }
        public string outputPattern { get; set; }
        public string useXctool { get; set; }
        public string xctoolReporter { get; set; }
        public string testRunner { get; set; }
        public string testResultsFiles { get; set; }
        public string action { get; set; }
        public string timeout { get; set; }
        public string project { get; set; }
        public string outputDir { get; set; }
        public string msbuildArguments { get; set; }
        public string app { get; set; }
        public string testDir { get; set; }
        public string testCloudLocation { get; set; }
        public string files { get; set; }
        public string jarsign { get; set; }
        public string jarsignerArguments { get; set; }
        public string zipalign { get; set; }
        public string mdtoolLocation { get; set; }
        public string forSimulator { get; set; }
        public string dsym { get; set; }
        public string teamApiKey { get; set; }
        public string user { get; set; }
        public string devices { get; set; }
        public string series { get; set; }
        public string parallelization { get; set; }
        public string locale { get; set; }
        public string optionalArgs { get; set; }
        public string SourcePath { get; set; }
        public string machineUserName { get; set; }
        public string machinePassword { get; set; }
        public string updateTestAgent { get; set; }
        public string dropLocation { get; set; }
        public string sourcefilters { get; set; }
        public string codeCoverageEnabled { get; set; }
    }

    public class Trigger
    {
        public object[] branchFilters { get; set; }
        public bool batchChanges { get; set; }
        public int maxConcurrentBuildsPerBranch { get; set; }
        public string triggerType { get; set; }
    }

}
