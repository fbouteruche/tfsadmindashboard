using System;

namespace TFSDataService
{

    public class BuildDefinitionDetails
    {
        public Build1[] build { get; set; }
        public Option[] options { get; set; }
        public Trigger[] triggers { get; set; }
        public Variables variables { get; set; }
        public Retentionrule[] retentionRules { get; set; }
        public _Links _links { get; set; }
        public string buildNumberFormat { get; set; }
        public string jobAuthorizationScope { get; set; }
        public int jobTimeoutInMinutes { get; set; }
        public Repository repository { get; set; }
        public string quality { get; set; }
        public Authoredby authoredBy { get; set; }
        public Queue queue { get; set; }
        public string uri { get; set; }
        public string type { get; set; }
        public int revision { get; set; }
        public DateTime createdDate { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Project project { get; set; }
        public bool HasOwaspDependencyCheckEnabled { get; set; }
    }

    public class Variables
    {
        public Buildconfiguration BuildConfiguration { get; set; }
        public Buildplatform BuildPlatform { get; set; }
        public Deploypassword DeployPassword { get; set; }
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

    public class Deploypassword
    {
        public object value { get; set; }
        public bool isSecret { get; set; }
    }

    public class _Links
    {
        public Self self { get; set; }
        public Web web { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Web
    {
        public string href { get; set; }
    }

    public class Repository
    {
        public Properties1 properties { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string defaultBranch { get; set; }
        public string clean { get; set; }
        public bool checkoutSubmodules { get; set; }
    }

    public class Properties1
    {
        public string labelSources { get; set; }
    }

    public class Authoredby
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public string uniqueName { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
    }

    public class Queue
    {
        public Pool pool { get; set; }
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Pool
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string state { get; set; }
        public int revision { get; set; }
    }

    public class Build1
    {
        public bool enabled { get; set; }
        public bool continueOnError { get; set; }
        public bool alwaysRun { get; set; }
        public string displayName { get; set; }
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
        public string msbuildArgs { get; set; }
        public string platform { get; set; }
        public string configuration { get; set; }
        public string clean { get; set; }
        public string restoreNugetPackages { get; set; }
        public string vsVersion { get; set; }
        public string msbuildArchitecture { get; set; }
        public string logProjectEvents { get; set; }
        public string testAssembly { get; set; }
        public string testFiltercriteria { get; set; }
        public string runSettingsFile { get; set; }
        public string overrideTestrunParameters { get; set; }
        public string codeCoverageEnabled { get; set; }
        public string runInParallel { get; set; }
        public string vsTestVersion { get; set; }
        public string pathtoCustomTestAdapters { get; set; }
        public string otherConsoleOptions { get; set; }
        public string testRunTitle { get; set; }
        public string publishRunAttachments { get; set; }
        public string CopyRoot { get; set; }
        public string Contents { get; set; }
        public string ArtifactName { get; set; }
        public string ArtifactType { get; set; }
        public string TargetPath { get; set; }
    }

    public class Option
    {
        public bool enabled { get; set; }
        public Definition definition { get; set; }
        public Inputs1 inputs { get; set; }
    }

    public class Definition
    {
        public string id { get; set; }
    }

    public class Inputs1
    {
        public string multipliers { get; set; }
        public string parallel { get; set; }
        public string continueOnError { get; set; }
        public string additionalFields { get; set; }
        public string workItemType { get; set; }
        public string assignToRequestor { get; set; }
    }

    public class Trigger
    {
        public string[] branchFilters { get; set; }
        public bool batchChanges { get; set; }
        public int maxConcurrentBuildsPerBranch { get; set; }
        public string triggerType { get; set; }
    }

    public class Retentionrule
    {
        public string[] branches { get; set; }
        public string[] artifacts { get; set; }
        public int daysToKeep { get; set; }
        public int minimumToKeep { get; set; }
        public bool deleteBuildRecord { get; set; }
        public bool deleteTestResults { get; set; }
    }

}