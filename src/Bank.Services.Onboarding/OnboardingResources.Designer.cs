﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bank.Services.Onboarding {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class OnboardingResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal OnboardingResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Bank.Services.Onboarding.OnboardingResources", typeof(OnboardingResources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Congratulations! You were aproved on our onboarding process..
        /// </summary>
        internal static string WaitingListAttendanceMailBody {
            get {
                return ResourceManager.GetString("WaitingListAttendanceMailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Welcome.
        /// </summary>
        internal static string WaitingListAttendanceMailSubject {
            get {
                return ResourceManager.GetString("WaitingListAttendanceMailSubject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hi #NAME#, we are glad to have you here,you are the number #POSITION# in our waiting list. Hopefully you will be able to move forward with your on-boarding process on #DATE#. We keep in touch..
        /// </summary>
        internal static string WaitingListQueueMailBody {
            get {
                return ResourceManager.GetString("WaitingListQueueMailBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Onboarding.
        /// </summary>
        internal static string WaitingListQueueMailSubject {
            get {
                return ResourceManager.GetString("WaitingListQueueMailSubject", resourceCulture);
            }
        }
    }
}
