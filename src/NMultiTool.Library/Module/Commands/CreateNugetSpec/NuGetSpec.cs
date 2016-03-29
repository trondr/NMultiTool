using System;
using System.IO;
using System.Reflection;
using Common.Logging;
using NuGet;

namespace NMultiTool.Library.Module.Commands.CreateNugetSpec
{
    public class NuGetSpec : INuGetSpec
    {
        private readonly ILog _logger;

        public NuGetSpec(ILog logger)
        {
            _logger = logger;
        }

        public void Create(string nugetManifestTemplate, string nugetmanifestTarget, string assemblyPath)
        {
            if (!File.Exists(nugetManifestTemplate))
            {
                throw new FileNotFoundException(string.Format("Could not find nuget template manifest '{0}'.",
                    nugetmanifestTarget));
            }
            if (!File.Exists(assemblyPath))
            {
                throw new FileNotFoundException(string.Format("Could not find assembly path '{0}'.", assemblyPath));
            }
            var assemblyMetadata = AssemblyMetadataExtractor.GetMetadata(assemblyPath);
            Manifest nugetManifest;
            if (_logger.IsInfoEnabled)
                _logger.InfoFormat("Loading Nuget manifest template '{0}' into memory...", nugetManifestTemplate);
            using (var fileStream = File.OpenRead(nugetManifestTemplate))
            {
                nugetManifest = Manifest.ReadFrom(fileStream, true);
            }
            if (_logger.IsInfoEnabled) _logger.Info("Updating Nuget manifest in memory...");
            nugetManifest.Metadata.Authors = assemblyMetadata.Company;
            nugetManifest.Metadata.Owners = assemblyMetadata.Company;
            nugetManifest.Metadata.Version = GetApplicationVersion(assemblyPath); //assemblyMetadata.Version.ToString();
            nugetManifest.Metadata.Title = assemblyMetadata.Title.Substring(0, assemblyMetadata.Title.IndexOf(" for ", StringComparison.Ordinal) + 1);
            nugetManifest.Metadata.Description = assemblyMetadata.Description;
            nugetManifest.Metadata.Copyright = assemblyMetadata.Copyright;
            if (_logger.IsInfoEnabled)
                _logger.InfoFormat("Saving Nuget manifest to file '{0}'...", nugetmanifestTarget);
            if (File.Exists(nugetmanifestTarget))
            {
                File.Delete(nugetmanifestTarget);
            }
            using (var fileStream = new FileStream(nugetmanifestTarget, FileMode.OpenOrCreate))
            {
                nugetManifest.Save(fileStream);
            }
            if (_logger.IsInfoEnabled) _logger.Info("Finished!");
        }

        /// <summary>
        /// Get application version
        /// </summary>
        private static string GetApplicationVersion(string assemblyPath)
        {
            string applicationVersion = null;
            var assembly = Assembly.LoadFile(assemblyPath);
            var informationalVersionAttribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyVersionAttribute), false) as AssemblyVersionAttribute;
            if (informationalVersionAttribute != null)
            {
                applicationVersion = informationalVersionAttribute.Version;
            }
            if (string.IsNullOrEmpty(applicationVersion))
            {
                applicationVersion = assembly.GetName().Version.ToString();
            }
            return applicationVersion;
        }
    }
}