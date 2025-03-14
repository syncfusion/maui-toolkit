namespace Syncfusion.Maui.Toolkit.Expander
{
    /// <summary>
    /// Class used to host the <see cref="SfExpander.Content"/>/>.
    /// </summary>
    internal partial class ExpanderContent : SfView
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpanderContent" /> class.
        /// </summary>
        internal ExpanderContent()
        {
            SetUp();
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the value indicates the instance of <see cref="SfExpander"/>.
        /// </summary>
        internal SfExpander? Expander
        {
            get;
            set;
        }

		#endregion

		#region Private Methods

		/// <summary>
		/// Sets up the necessary configurations or initializations for the element.  
		/// </summary>  
		void SetUp()
        {
            BackgroundColor = Colors.Transparent;
        }

        #endregion
    }
}
