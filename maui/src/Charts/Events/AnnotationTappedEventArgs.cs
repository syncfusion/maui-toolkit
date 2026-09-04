namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Provides data for the <b>AnnotationClicked</b> event.
	/// </summary>
	/// <remarks>
	/// This class contains information for the event that is raised when a chart annotation is tapped.
	/// </remarks>
	public class AnnotationTappedEventArgs : EventArgs
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="AnnotationTappedEventArgs"/> class.
		/// </summary>
		/// <param name="annotation">The chart annotation that was tapped.</param>
		/// <param name="x">The horizontal coordinate of the touch point relative to the annotation's origin.</param>
		/// <param name="y">The vertical coordinate of the touch point relative to the annotation's origin.</param>
		public AnnotationTappedEventArgs(ChartAnnotation annotation, double x, double y)
		{
			Annotation = annotation;
			X = x;
			Y = y;
		}

		/// <summary>
		/// Gets the instance of the <see cref="ChartAnnotation"/> that was tapped.
		/// </summary>
		public ChartAnnotation Annotation { get; }
		
		/// <summary>
		/// Gets the horizontal coordinate of the touch point relative to the annotation's origin.
		/// </summary>
		public double X { get; }
		
		/// <summary>
		/// Gets the vertical coordinate of the touch point relative to the annotation's origin.
		/// </summary>
		public double Y { get; }
	}
}
