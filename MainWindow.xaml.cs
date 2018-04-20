using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace Xpath_Query
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private XmlDocument doc;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private void Update(XmlNodeList nodes)
        {
            if (nodes == null || nodes.Count == 0)
            {
                textBlockResult.Text = "The query yielded no results";
                return;
            }
            string text = "";
            foreach (XmlNode node in nodes)
            {
                text = FormatText(node, text, " ")+"\r\n";
            }
            textBlockResult.Text = text;
        }
        private string FormatText(XmlNode node, string text, string indent)
        {
            if (node is XmlText)
            {
                text += node.Value;
                return text;
            }
            if (string.IsNullOrEmpty(indent))
            {
                indent = "    ";
            }
            else
            {
                text += "\n" + indent;
            }
            if (node is XmlComment)
            {
                text += node.OuterXml;
                return text;
            }
            text += "<" + node.Name;
            if (node.Attributes.Count > 0)
            {
                AddAttributes(node, ref text);
            }
            if (node.HasChildNodes)
            {
                text += ">";
                foreach (XmlNode child in node.ChildNodes)
                {
                    text = FormatText(child, text, indent + "       ");
                }
                if (node.ChildNodes.Count == 1 && (node.FirstChild is XmlText || node.FirstChild is XmlComment))
                    text += "</" + node.Name + ">";
                else
                    text += "\r\n" + indent + "</" + node.Name + ">";
            }
            else
                text += "/>";
            return text;
        }
        private void AddAttributes(XmlNode node, ref string text)
        {
            foreach (XmlAttribute xa in node.Attributes)
            {
                text += " " + xa.Name + "='" + xa.Value + "'";
            }
        }

        private void buttonExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                XmlNodeList nodes = doc.DocumentElement.SelectNodes(textBoxQuery.Text);
                Update(nodes);
            }
            catch (Exception err)
            {
                textBlockResult.Text = err.Message;
            }
        }

        private void buttonShow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                doc = new XmlDocument();
                if (textBoxPath.Text != "")
                {
                    doc.Load(textBoxPath.Text);
                }
                else
                    doc.Load(@"F:\c# book\Chapter19code\Chapter19\XML and Schema\Elements.xml");
                Update(doc.DocumentElement.SelectNodes("."));
            }
            catch (Exception err)
            {
                textBlockResult.Text = err.Message;
                throw;
            }
           
        }
    }
}
