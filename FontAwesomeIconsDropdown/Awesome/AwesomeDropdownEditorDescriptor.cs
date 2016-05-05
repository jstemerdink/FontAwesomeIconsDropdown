namespace FontAwesomeIconsDropdown.Awesome
{
    using EPiServer.Shell.ObjectEditing.EditorDescriptors;

    /// <summary>
    /// Class AwesomeDropdownEditorDescriptor.
    /// </summary>
    /// <seealso cref="EPiServer.Shell.ObjectEditing.EditorDescriptors.EditorDescriptor" />
    /// <author>Marija Jemuovic</author>
    [EditorDescriptorRegistration(TargetType = typeof(string), UIHint = CustomUiHints.AwesomeDropdown)]
    public class AwesomeDropdownEditorDescriptor : EditorDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AwesomeDropdownEditorDescriptor"/> class.
        /// </summary>
        public AwesomeDropdownEditorDescriptor()
        {
            this.SelectionFactoryType = typeof(AwesomeSelectionFactory);
            this.ClientEditingClass = "alloy/editors/AwesomeSelectionEditor";
            this.EditorConfiguration.Add("style", "width: 72px");
        }
    }
}