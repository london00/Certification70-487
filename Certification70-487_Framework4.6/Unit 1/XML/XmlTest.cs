using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Certification70_487_Framework4._6.Unit_1.XML
{
    [TestClass]
    public class XmlTest
    {
        [TestMethod]
        [DataRow("WriteXMLTest.xml")]
        public void WriteXMLTest(string fileName)
        {
            var customersList = new[]{
                new ExamplePOCOClass("Geiser", "A", "Aragon"),
                new ExamplePOCOClass("Dayana", "N", "Gallegos"),
                new ExamplePOCOClass("Dafne", "N", "Aragon"),
                new ExamplePOCOClass("Dayana", "S", "Aragon")
            };

            using (var xmlWriter = XmlWriter.Create("customer.xml"))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("Customers");

                foreach (var customerElement in customersList)
                {
                    xmlWriter.WriteStartElement("Customer");
                    xmlWriter.WriteElementString("FirstName", customerElement.FirstName);
                    xmlWriter.WriteElementString("MiddleInitial", customerElement.MiddleInitial);
                    xmlWriter.WriteElementString("LastName", customerElement.LastName);
                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }

        [TestMethod]
        [DataRow("WriteXMByXMLDocumentLTest.xml")]
        public void WriteXMByXMLDocumentLTest(string fileName)
        {
            var customersList = new[]{
                new ExamplePOCOClass("Geiser", "A", "Aragon"),
                new ExamplePOCOClass("Dayana", "N", "Gallegos"),
                new ExamplePOCOClass("Dafne", "N", "Aragon"),
                new ExamplePOCOClass("Dayana", "S", "Aragon")
            };

            XmlDocument xmlWriter = new XmlDocument();
            {
                XmlElement customersNode = xmlWriter.CreateElement("Customers");

                foreach (var customerElement in customersList)
                {
                    XmlElement customerNode = xmlWriter.CreateElement("Customer");

                    XmlElement FirstNameNode = xmlWriter.CreateElement("FirstName");
                    FirstNameNode.InnerText = customerElement.FirstName;
                    customerNode.AppendChild(FirstNameNode);

                    XmlElement MiddleInitialNode = xmlWriter.CreateElement("MiddleInitial");
                    MiddleInitialNode.InnerText = customerElement.MiddleInitial;
                    customerNode.AppendChild(MiddleInitialNode);

                    XmlElement LastNameNode = xmlWriter.CreateElement("LastName");
                    LastNameNode.InnerText = customerElement.LastName;
                    customerNode.AppendChild(LastNameNode);

                    customersNode.AppendChild(customerNode);
                }
            }

            xmlWriter.Save(fileName);
        }

        [TestMethod]
        [DataRow("WriteXMByLinqTest.xml")]
        public void WriteXMByLinqTest(string fileName)
        {
            XElement customersXML = new XElement(
                "Customers",
                new XElement("Customer",
                    new XElement("FirstName", "Geiser"),
                    new XElement("MiddleInitial", "A"),
                    new XElement("LastName", "Aragon")
                ),
                new XElement("Customer",
                    new XElement("FirstName", "Dayana"),
                    new XElement("MiddleInitial", "N"),
                    new XElement("LastName", "Gallegos")
                ),
                new XElement("Customer",
                    new XElement("FirstName", ""),
                    new XElement("MiddleInitial", ""),
                    new XElement("LastName", "")
                ),
                new XElement("Customer",
                    new XElement("FirstName", "Dayana"),
                    new XElement("MiddleInitial", "S"),
                    new XElement("LastName", "Aragon")
                )
            );

            // Get an element
            var firstName = customersXML.Elements("Customer").First().Element("FirstName").ToString();

            customersXML.Save(fileName);
        }

        [TestMethod]
        [DataRow("WriteXMLTest.xml")]
        [DataRow("WriteXMByXMLDocumentLTest.xml")]
        [DataRow("WriteXMByLinqTest.xml")]
        public void ReadXMLTest(string fileName)
        {
            using (var xmlReader = XmlReader.Create("customer.xml"))
            {
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.None:
                            break;
                        case XmlNodeType.Element:
                            Debug.WriteLine("Element: " + xmlReader.Name);
                            break;
                        case XmlNodeType.Attribute:
                            break;
                        case XmlNodeType.Text:
                            Debug.WriteLine("Element: " + xmlReader.Value);
                            break;
                        case XmlNodeType.Document:
                            Debug.WriteLine("Start document");
                            break;
                        case XmlNodeType.DocumentType:
                            Debug.WriteLine("Document type");
                            break;
                        case XmlNodeType.EndElement:
                            Debug.WriteLine("End element");
                            break;
                        case XmlNodeType.XmlDeclaration:
                            Debug.WriteLine("Declaring XML document");
                            break;
                        default:
                            break;
                    }
                }

            }
        }
    }
}
