namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
	/// <summary>
	/// Handles the drawing view functionality for the SfDrawableView.
	/// </summary>
	public partial class SfDrawableViewHandler
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="SfDrawableViewHandler"/> class with the default view mapper.
		/// </summary>
		public SfDrawableViewHandler() : base(SfDrawableViewHandler.ViewMapper)
        {

        }

		/// <summary>
		/// Initializes a new instance of the <see cref="SfDrawableViewHandler"/> class with a specified property mapper.
		/// </summary>
		/// <param name="mapper">The property mapper to use for this handler.</param>
		public SfDrawableViewHandler(PropertyMapper mapper) : base(mapper)
        {
        }
    }
}
