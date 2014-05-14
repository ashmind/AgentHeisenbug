using System.Reflection;
using AgentHeisenbug.Properties;
using JetBrains.Application.PluginSupport;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany(AssemblyInfo.Author)]
[assembly: AssemblyProduct(AssemblyInfo.Title)]
[assembly: AssemblyCopyright("Copyright © " + AssemblyInfo.Author + ", 2014")]

[assembly: AssemblyVersion(AssemblyInfo.Version)]
[assembly: AssemblyFileVersion(AssemblyInfo.Version)]
[assembly: AssemblyInformationalVersion(AssemblyInfo.FullVersion)]

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("Agent Heisenbug")]
[assembly: PluginDescription("Provides basic [ThreadSafe]/[ReadOnly] annotations.")]
[assembly: PluginVendor(AssemblyInfo.Author)]

namespace AgentHeisenbug.Properties {
    internal static class AssemblyInfo {
        public const string Author = "Andrey Shchekin";
        public const string Title = "AgentHeisenbug";
        public const string Version = "0.5.5";
        public const string FullVersion = Version + "-alpha";
    }
}