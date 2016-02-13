using Common.Logging;

namespace NMultiTool.Library.Commands.Folder2Wxs
{
    public class IdFromNameGenerator : IIdFromNameGenerator
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly ILog _logger;

        public IdFromNameGenerator(IRandomNumberGenerator randomNumberGenerator, ILog logger)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
        }

        public string GetId(string name, string postfix)
        {
            var normalizeName = name
                .Replace('&', '_')
                .Replace('%', '_')
                .Replace('.', '_')
                .Replace(' ', '_')
                .Replace('-', '_')
                .Replace('+', '_')
                .Replace('·', '_')
                .Replace("___", "_")
                .Replace("__", "_")
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Replace("{", string.Empty)
                .Replace("}", string.Empty);
            if (normalizeName.Length > 40)
            {
                normalizeName = normalizeName.Substring(0, 40);
            }
            return string.Concat(new object[] { "Id_" + normalizeName, "_", _randomNumberGenerator.GetRandomNumber(), "_", postfix });
        }
    }
}