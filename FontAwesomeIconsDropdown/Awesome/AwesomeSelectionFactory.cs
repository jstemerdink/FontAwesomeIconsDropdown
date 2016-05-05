namespace FontAwesomeIconsDropdown.Awesome
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using EPiServer.Logging;
    using EPiServer.Shell.ObjectEditing;

    /// <summary>
    /// Class AwesomeSelectionFactory.
    /// </summary>
    /// <seealso cref="EPiServer.Shell.ObjectEditing.ISelectionFactory" />
    /// <seealso cref="AwesomeDropdownEditorDescriptor" />
    /// <author>Marija Jemuovic, Jeroen Stemerdink</author>
    public class AwesomeSelectionFactory : ISelectionFactory
    {
        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILogger Log = LogManager.GetLogger();

        /// <summary>
        /// Creates a list of selection items for a specific property.
        /// </summary>
        /// <param name="metadata">The metadata for a property.</param>
        /// <returns>A list of selection items for a specific property.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Font css class less than 3.</exception>
        /// <exception cref="ArgumentNullException">Format string is null.</exception>
        /// <exception cref="FormatException">Format string is invalid.-or- The index of a format item is not zero or one.</exception>
        public IEnumerable<ISelectItem> GetSelections(
            ExtendedMetadata metadata)
        {
            string path;

            try
            {
                path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    @"ClientResources\Data\FontAwesomeIcons.xml");
            }
            catch (ArgumentException argumentException)
            {
                Log.Error(argumentException.Message, argumentException);
                yield break;
            }
            catch (AppDomainUnloadedException appDomainUnloadedException)
            {
                Log.Error(appDomainUnloadedException.Message, appDomainUnloadedException);
                yield break;
            }

            XDocument xdoc = XDocument.Load(path);

            if (xdoc.Document == null)
            {
                yield break;
            }

            string awesomeCategory = string.Empty;
            AwesomeCategoryAttribute awesomeCategoryAttribute = metadata.Attributes.OfType<AwesomeCategoryAttribute>().FirstOrDefault();

            if (awesomeCategoryAttribute != null)
            {
                awesomeCategory = awesomeCategoryAttribute.Category ?? string.Empty;
            }

            List<XElement> icons;

            if (string.IsNullOrWhiteSpace(awesomeCategory))
            {
                icons =
                    xdoc.Document.Descendants("string").ToList();
            }
            else
            {
                icons = xdoc.Document.Descendants("string")
                    .Where(i => i.Attribute("category").Value.Equals(awesomeCategory, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            foreach (XElement node in icons)
            {
                string nameOfFontCssClass = node.Attribute("name").Value;

                yield return new SelectItem
                {
                    Value = nameOfFontCssClass,
                    Text = string.Format("<i class=\"fa {0}\"></i>&nbsp;&nbsp;{1}", nameOfFontCssClass, nameOfFontCssClass.Substring(3))
                };
            }
        }
    }
}