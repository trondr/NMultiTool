using System;
using System.Text;
using System.Xml;

namespace NMultiTool.Library.Common.Xml
{
    public class XmlHelper : IXmlHelper
    {
        public void SetAttribute(XmlNode xmlNode, string attributeName, string attributeValue)
        {
            if (xmlNode == null) throw new ArgumentNullException("xmlNode");
            if (xmlNode.OwnerDocument == null) throw new XmlException("Xmlnode Owner document is null.");
            XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(attributeName);
            xmlAttribute.Value = attributeValue;
            if (xmlNode.Attributes == null) throw new XmlException("Xmlnode Attributes property is null.");
            xmlNode.Attributes.SetNamedItem(xmlAttribute);     
        }

        public void SaveDocument(XmlDocument xmlDocument, string xmlFileName)
        {
            if (xmlDocument == null) throw new ArgumentNullException("xmlDocument");
            if (string.IsNullOrEmpty(xmlFileName)) throw new ArgumentNullException("xmlFileName");
            using (var xmlTextReader = new XmlTextReader(xmlDocument.OuterXml, XmlNodeType.Document, new XmlParserContext(null, null, null, XmlSpace.None)))
            {
                using (var xmlTextWriter = new XmlTextWriter(xmlFileName, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Formatting.Indented;
                    xmlTextWriter.WriteNode(xmlTextReader, true);
                    xmlTextWriter.Flush();
                }
            }
        }
    }
}