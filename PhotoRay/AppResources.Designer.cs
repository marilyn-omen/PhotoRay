﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PhotoRay {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class AppResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal AppResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PhotoRay.AppResources", typeof(AppResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PhotoRay.
        /// </summary>
        public static string AppName {
            get {
                return ResourceManager.GetString("AppName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://photoray.org.
        /// </summary>
        public static string AppRemoteUrl {
            get {
                return ResourceManager.GetString("AppRemoteUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We apologize, but it looks like previous application run ended up with an exception. Would you like to send the developer gathered information about this issue to help him fix it and make PhotoRay better?.
        /// </summary>
        public static string CrashMessageText {
            get {
                return ResourceManager.GetString("CrashMessageText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to What happened?.
        /// </summary>
        public static string CrashMessageTitle {
            get {
                return ResourceManager.GetString("CrashMessageTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PhotoRay crash report.
        /// </summary>
        public static string CrashReportEmailSubject {
            get {
                return ResourceManager.GetString("CrashReportEmailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing camera.
        /// </summary>
        public static string InitializingCamera {
            get {
                return ResourceManager.GetString("InitializingCamera", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Open internet browser on your computer and navigate to the address.
        /// </summary>
        public static string InstructionLine1 {
            get {
                return ResourceManager.GetString("InstructionLine1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Scan provided QR code with phone camera to begin..
        /// </summary>
        public static string InstructionLine2 {
            get {
                return ResourceManager.GetString("InstructionLine2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PhotoRay accesses your phone photo gallery and uploads photos to remote server as they are selected. Uploaded photos are not used in any way other then displaying them to you at the web page and are deleted immediately after that. PhotoRay uses your data connection to transfer photos, so standard fees may apply. For smooth and fast operation it is best to use the application with Wi-Fi connected..
        /// </summary>
        public static string NoticeText {
            get {
                return ResourceManager.GetString("NoticeText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disclaimer.
        /// </summary>
        public static string NoticeTitle {
            get {
                return ResourceManager.GetString("NoticeTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to support@photoray.org.
        /// </summary>
        public static string SupportEmail {
            get {
                return ResourceManager.GetString("SupportEmail", resourceCulture);
            }
        }
    }
}
