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
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/{2}/_apis/build/builds?api-version=2.0")]
        public string BuildUrl {
            get {
                return ((string)(this["BuildUrl"]));
            }
            set {
                this["BuildUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/{2}/_apis/build/definitions?api-version=2.0")]
        public string BuildDefinitionUrl {
            get {
                return ((string)(this["BuildDefinitionUrl"]));
            }
            set {
                this["BuildDefinitionUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/git/repositories/{2}/refs/heads?api-version=1.0")]
        public string GitBranchUrl {
            get {
                return ((string)(this["GitBranchUrl"]));
            }
            set {
                this["GitBranchUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/git/repositories/{2}/commits?api-version=1.0")]
        public string GitCommitUrl {
            get {
                return ((string)(this["GitCommitUrl"]));
            }
            set {
                this["GitCommitUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/_apis/git/repositories/{2}/refs/tags?api-version=1.0")]
        public string GitTagUrl {
            get {
                return ((string)(this["GitTagUrl"]));
            }
            set {
                this["GitTagUrl"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("{0}/{1}/{2}/_apis/git/repositories?api-version=1.0")]
        public string GitRepoUrl {
            get {
                return ((string)(this["GitRepoUrl"]));
            }
            set {
                this["GitRepoUrl"] = value;
            }
        }
    }
}