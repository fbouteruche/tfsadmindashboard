﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TFSDataService.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/{2}/_apis/build/builds?api-version=2.0")]
        public string BuildUrl {
            get {
                return ((string)(this["BuildUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/{2}/_apis/build/definitions?api-version=2.0")]
        public string BuildDefinitionUrl {
            get {
                return ((string)(this["BuildDefinitionUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/git/repositories/{2}/refs/heads?api-version=1.0")]
        public string GitBranchUrl {
            get {
                return ((string)(this["GitBranchUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/git/repositories/{2}/commits?api-version=1.0")]
        public string GitCommitUrl {
            get {
                return ((string)(this["GitCommitUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/git/repositories/{2}/refs/tags?api-version=1.0")]
        public string GitTagUrl {
            get {
                return ((string)(this["GitTagUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/{2}/_apis/git/repositories?api-version=1.0")]
        public string GitRepoUrl {
            get {
                return ((string)(this["GitRepoUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/_apis/projectcollections?api-version=1.0")]
        public string ProjectCollectionUrl {
            get {
                return ((string)(this["ProjectCollectionUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/_apis/projectcollections/{1}?api-version=1.0")]
        public string TeamProjectCollectionUrl {
            get {
                return ((string)(this["TeamProjectCollectionUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/projects?api-version=1.0")]
        public string TeamProjectUrl {
            get {
                return ((string)(this["TeamProjectUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/projects/{2}/teams?api-version=1.0")]
        public string TeamUrl {
            get {
                return ((string)(this["TeamUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/projects/{2}/teams/{3}/members?api-version=1.0")]
        public string TeamMemberUrl {
            get {
                return ((string)(this["TeamMemberUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/{2}/_apis/build/definitions/templates?api-version=2.0")]
        public string BuildDefinitionTemplateUrl {
            get {
                return ((string)(this["BuildDefinitionTemplateUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/{2}/_apis/build/definitions/templates/{3}?api-version=2.0")]
        public string BuildDefinitionTemplatePostUrl {
            get {
                return ((string)(this["BuildDefinitionTemplatePostUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/process/processes?api-version=1.0")]
        public string ProcessesUrl {
            get {
                return ((string)(this["ProcessesUrl"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/process/processes/{2}?api-version=1.0")]
        public string SingleProcessUrl {
            get {
                return ((string)(this["SingleProcessUrl"]));
            }
        }
    }
}
