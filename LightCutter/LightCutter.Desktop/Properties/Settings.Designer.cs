﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Net.Surviveplus.LightCutter.Desktop.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool ShowActionPanelOnStartUp {
            get {
                return ((bool)(this["ShowActionPanelOnStartUp"]));
            }
            set {
                this["ShowActionPanelOnStartUp"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool Upgraded {
            get {
                return ((bool)(this["Upgraded"]));
            }
            set {
                this["Upgraded"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("3")]
        public int DefaultWaitTimeSeconds {
            get {
                return ((int)(this["DefaultWaitTimeSeconds"]));
            }
            set {
                this["DefaultWaitTimeSeconds"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Ctrl + Shift + Alt + A")]
        public string ShortcutOpenActionPanel {
            get {
                return ((string)(this["ShortcutOpenActionPanel"]));
            }
            set {
                this["ShortcutOpenActionPanel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Ctrl + Shift + Alt + Z")]
        public string ShortcutStartDefaultAction {
            get {
                return ((string)(this["ShortcutStartDefaultAction"]));
            }
            set {
                this["ShortcutStartDefaultAction"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Screen > Cut > Save")]
        public string DefaultActionName {
            get {
                return ((string)(this["DefaultActionName"]));
            }
            set {
                this["DefaultActionName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double LastMainWindowHeight {
            get {
                return ((double)(this["LastMainWindowHeight"]));
            }
            set {
                this["LastMainWindowHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double LastMainWindowWidth {
            get {
                return ((double)(this["LastMainWindowWidth"]));
            }
            set {
                this["LastMainWindowWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool GuideBackgroundTransparent {
            get {
                return ((bool)(this["GuideBackgroundTransparent"]));
            }
            set {
                this["GuideBackgroundTransparent"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("16")]
        public int GridPixel {
            get {
                return ((int)(this["GridPixel"]));
            }
            set {
                this["GridPixel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Actions {
            get {
                return ((string)(this["Actions"]));
            }
            set {
                this["Actions"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Desktop")]
        public string DefaultFolder {
            get {
                return ((string)(this["DefaultFolder"]));
            }
            set {
                this["DefaultFolder"] = value;
            }
        }
    }
}
