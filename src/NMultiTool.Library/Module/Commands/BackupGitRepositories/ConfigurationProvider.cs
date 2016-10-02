
using System.IO;
using System.Xml.Serialization;
using Common.Logging;

namespace NMultiTool.Library.Module.Commands.BackupGitRepositories
{
    public class ConfigurationProvider<T> : IConfigurationProvider<T>
    {
        private readonly ILog _logger;

        public ConfigurationProvider(ILog logger)
        {
            _logger = logger;
        }

        public T Load(string configurationFile)
        {
            _logger.InfoFormat("Loading '{0}' from '{1}'...", typeof(T).Name, configurationFile);
            var serializer = new XmlSerializer(typeof(T));
            T configuration;
            using (var sr = new StreamReader(configurationFile))
            {
                configuration = (T) serializer.Deserialize(sr);
            }
            _logger.InfoFormat("Finished loading '{0}' from '{1}'!", typeof(T).Name, configurationFile);
            return configuration;
        }

        public void Save(T configuration, string configurationFile)
        {
            _logger.InfoFormat("Saving '{0}' to '{1}'...", typeof(T).Name, configurationFile);
            var serializer = new XmlSerializer(typeof(T));
            using (var sw = new StreamWriter(configurationFile))
            {
                serializer.Serialize(sw, configuration);
            }
            _logger.InfoFormat("Finished saving '{0}' to '{1}'!", typeof(T).Name, configurationFile);
        }
    }
}