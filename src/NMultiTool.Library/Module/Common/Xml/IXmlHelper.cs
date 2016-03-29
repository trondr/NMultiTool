using System.Xml;

namespace NMultiTool.Library.Module.Common.Xml
{
    public interface IXmlHelper
    {
        void SetAttribute(XmlNode xmlNode, string attributeName, string attributeValue);

        void SaveDocument(XmlDocument xmlDocument, string xmlFileName);
    }
}
