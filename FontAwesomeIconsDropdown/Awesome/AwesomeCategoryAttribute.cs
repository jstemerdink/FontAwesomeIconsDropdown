namespace FontAwesomeIconsDropdown.Awesome
{
    using System;

    /// <summary>
    /// Class AwesomeCategoryAttribute. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="AwesomeCategories" />
    /// <seealso cref="AwesomeSelectionFactory" />
    /// <seealso cref="AwesomeDropdownEditorDescriptor" />
    /// <remarks>
    /// When available on a property marked with a <see cref="AwesomeDropdownEditorDescriptor"/>,
    /// the value will be used a selector for the icons in the <see cref="AwesomeSelectionFactory"/>
    /// </remarks>
    /// <example>[AwesomeCategory(AwesomeCategories.Webapplication)]</example>
    /// <author>Jeroen Stemerdink</author>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AwesomeCategoryAttribute : Attribute
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Attribute" /> class.</summary>
        public AwesomeCategoryAttribute(string category)
        {
            this.Category = category;
        }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>The category.</value>
        public string Category { get; private set; }
    }
}