// Copyright © 2016 Jeroen Stemerdink.
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

namespace Font.Awesome.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Xml;

    using HtmlAgilityPack;

    /// <summary>
    /// Class Program.
    /// </summary>
    /// <author>Jeroen Stemerdink</author>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            string basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            List<FontAwesomeType> types = new List<FontAwesomeType>();

            string fontAwesomeIcons;
            using (WebClient wc = new WebClient())
            {
                fontAwesomeIcons = wc.DownloadString("https://fortawesome.github.io/Font-Awesome/icons/");
            }

            if (string.IsNullOrWhiteSpace(fontAwesomeIcons))
            {
                return;
            }

            HtmlDocument hdoc = new HtmlDocument();
            hdoc.LoadHtml(fontAwesomeIcons);

            HtmlNode iconNode = hdoc.GetElementbyId("icons");

            foreach (HtmlNode section in iconNode.SelectNodes(".//section[@id]"))
            {
                string iconType = section.GetAttributeValue("id", "");

                foreach (HtmlNode iconElement in section.SelectNodes(".//i[@class]"))
                {
                    string iconName = iconElement.GetAttributeValue("class", "").Replace("fa ", string.Empty).Trim();
                    types.Add(new FontAwesomeType { Category = iconType, ClassName = iconName });
                }
            }

            types.RemoveAll(i => i.Category.Equals("new", StringComparison.OrdinalIgnoreCase));

            string fontAwesomeCheatSheet;
            using (WebClient wc = new WebClient())
            {
                fontAwesomeCheatSheet = wc.DownloadString("https://fortawesome.github.io/Font-Awesome/cheatsheet/");
            }

            if (string.IsNullOrWhiteSpace(fontAwesomeCheatSheet))
            {
                return;
            }

            hdoc = new HtmlDocument();
            hdoc.LoadHtml(fontAwesomeCheatSheet);

            HtmlNodeCollection valueNodes = hdoc.DocumentNode.SelectNodes(".//i[@class='fa fa-fw']");

            foreach (HtmlNode valueNode in valueNodes)
            {
                string nodeTitle = valueNode.GetAttributeValue("title", "").Replace("Copy to use ", string.Empty).Trim();

                List<FontAwesomeType> fa =
                    types.Where(
                        i => i.ClassName.Equals(string.Format(CultureInfo.InvariantCulture, "fa-{0}", nodeTitle)))
                        .ToList();

                foreach (FontAwesomeType fontAwesomeType in fa)
                {
                    fontAwesomeType.UniCode = valueNode.InnerText;
                }
            }

            using (XmlTextWriter writer = new XmlTextWriter(basePath + "\\FontAwesomeIcons.xml", new UTF8Encoding(false))
                )
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                writer.WriteStartDocument();
                writer.WriteStartElement("resources");

                foreach (FontAwesomeType fontAwesomeType in types)
                {
                    writer.WriteStartElement("string");
                    writer.WriteAttributeString("name", fontAwesomeType.ClassName);
                    writer.WriteAttributeString("category", fontAwesomeType.Category);
                    writer.WriteRaw(string.Format(CultureInfo.InvariantCulture, "{0};", fontAwesomeType.UniCode));
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            List<string> categories = types.Select(x => x.Category).Distinct().ToList();

            using (
                XmlTextWriter writer = new XmlTextWriter(basePath + "\\FontAwesomeCategories.xml", new UTF8Encoding(false)))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                writer.WriteStartDocument();
                writer.WriteStartElement("categories");

                foreach (string fontAwesomeCategorie in categories)
                {
                    writer.WriteElementString("category", fontAwesomeCategorie);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
    }

    /// <summary>
    /// Class FontAwesomeType.
    /// </summary>
    /// <author>Jeroen Stemerdink</author>
    internal class FontAwesomeType
    {
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>The name of the class.</value>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the uni code.
        /// </summary>
        /// <value>The uni code.</value>
        public string UniCode { get; set; }
    }
}