using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace YuYu.Components
{
    /// <summary>
    /// Xml提供程序
    /// </summary>
    public class XmlProvider
    {
        /// <summary>
        /// Xml文档路径
        /// </summary>
        public string XmlFilePath { get; private set; }

        internal XmlProvider(string xmlFilePath)
        {
            this.XmlFilePath = xmlFilePath;
        }

        internal IEnumerable<XElement> GetElements(Type type)
        {
            if (File.Exists(this.XmlFilePath))
                try
                {
                    XDocument xDoc = XDocument.Load(this.XmlFilePath, LoadOptions.None);
                    if (xDoc != null)
                        return xDoc.Document.Root.Elements(Keywords.ENTITIESNODENAME).Where(m => m.Attribute(Keywords.ENTITYTYPEATTRIBUTENAME) != null && m.Attribute(Keywords.ENTITYTYPEATTRIBUTENAME).Value == type.FullName).Elements(Keywords.ENTITYNODENAME);
                }
                catch (XmlException)
                {
                    return null;
                }
            return null;
        }

        internal void WriteElementsToFile(IEnumerable<XElement> elements)
        {
            using (FileStream fs = File.Open(this.XmlFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                XElement root = new XElement(Keywords.ROOTNODENAME, elements);
                XDocument xDoc = new XDocument(new XDeclaration(null) { Encoding = "utf-8", Version = "1.0" }, root);
                xDoc.Save(fs);
                fs.Flush();
                fs.Close();
            }
        }
    }
}
