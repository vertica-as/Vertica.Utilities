﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vertica.Utilities_v4.Resources {
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
    internal class Exceptions {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Exceptions() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Vertica.Utilities_v4.Resources.Exceptions", typeof(Exceptions).Assembly);
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
        ///   Looks up a localized string similar to The collection of type &apos;{0}&apos; must contain at least {1} elements, but contained {2}..
        /// </summary>
        internal static string CollectionCountValidator_MessageTemplate {
            get {
                return ResourceManager.GetString("CollectionCountValidator_MessageTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find control with ID &apos;{0}&apos; from parent &apos;{1}&apos;..
        /// </summary>
        internal static string ControlExtensions_NotFoundTemplate {
            get {
                return ResourceManager.GetString("ControlExtensions_NotFoundTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The credential must have a non-empty user name and domain..
        /// </summary>
        internal static string Credential_NoDomainOrUser {
            get {
                return ResourceManager.GetString("Credential_NoDomainOrUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The enum with name &apos;{0}&apos; is already used..
        /// </summary>
        internal static string Enumerated_DuplicatedTemplate {
            get {
                return ResourceManager.GetString("Enumerated_DuplicatedTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find enum with name &apos;{0}&apos;..
        /// </summary>
        internal static string Enumerated_NotFoundTemplate {
            get {
                return ResourceManager.GetString("Enumerated_NotFoundTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enum &apos;{0}&apos; must have System.FlagsAttribute applied to it..
        /// </summary>
        internal static string Enumeration_NoFlagsTemplate {
            get {
                return ResourceManager.GetString("Enumeration_NoFlagsTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Type &apos;{0}&apos; is not an enum..
        /// </summary>
        internal static string Enumeration_NotEnumTemplate {
            get {
                return ResourceManager.GetString("Enumeration_NotEnumTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The underlying type of &apos;{0}&apos; is {1}. Only enums with underlying type of [{2}] are supported..
        /// </summary>
        internal static string Enumeration_NotSupportedUnderlyingTypeTemplate {
            get {
                return ResourceManager.GetString("Enumeration_NotSupportedUnderlyingTypeTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value &quot;{0}&quot; is not defined within type &apos;{1}&apos;..
        /// </summary>
        internal static string Enumeration_ValueNotDefinedTemplate {
            get {
                return ResourceManager.GetString("Enumeration_ValueNotDefinedTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is not part of the expected domain: &apos;[{1}]&apos;..
        /// </summary>
        internal static string InvalidDomainException_MessageTemplate {
            get {
                return ResourceManager.GetString("InvalidDomainException_MessageTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LogonUser for user &apos;{0}&apos; failed with error {1}..
        /// </summary>
        internal static string LogonUserIdentityProvider_LogonUserErrorTemplate {
            get {
                return ResourceManager.GetString("LogonUserIdentityProvider_LogonUserErrorTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value must be between {0} and {1}. That is, contained within {2}..
        /// </summary>
        internal static string Range_ArgumentAssertion_Template {
            get {
                return ResourceManager.GetString("Range_ArgumentAssertion_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The generator must generate incrementing values..
        /// </summary>
        internal static string Range_NotIncrementingGenerator {
            get {
                return ResourceManager.GetString("Range_NotIncrementingGenerator", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The start value of the range ({0}) must not be greater than its end value ({1})..
        /// </summary>
        internal static string Range_UnorderedBounds_Template {
            get {
                return ResourceManager.GetString("Range_UnorderedBounds_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parent MapNode has to be constructed within a &apos;{0}&apos; context..
        /// </summary>
        internal static string SiteMapBuilder_NoContextParentTemplate {
            get {
                return ResourceManager.GetString("SiteMapBuilder_NoContextParentTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Save() cannot be called before Create()..
        /// </summary>
        internal static string SiteMapBuilder_SaveBeforeCreate {
            get {
                return ResourceManager.GetString("SiteMapBuilder_SaveBeforeCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to At least one iteration is needed. Increase the targetRuntime value..
        /// </summary>
        internal static string Time_AverageAction_OneIteration {
            get {
                return ResourceManager.GetString("Time_AverageAction_OneIteration", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In order to set the UtcNow property, the DateTimeOffset must be UTC, but had an offset of {0} instead..
        /// </summary>
        internal static string Time_MustBeUtcTemplate {
            get {
                return ResourceManager.GetString("Time_MustBeUtcTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The string must have exactly {0} parts when split using {1}..
        /// </summary>
        internal static string Tuploids_ParseTemplate {
            get {
                return ResourceManager.GetString("Tuploids_ParseTemplate", resourceCulture);
            }
        }
    }
}
