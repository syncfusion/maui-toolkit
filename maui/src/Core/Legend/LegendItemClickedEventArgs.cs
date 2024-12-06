
namespace Syncfusion.Maui.Toolkit
{
	internal class LegendItemClickedEventArgs : EventArgs
	{
		internal LegendItem? LegendItem { get; set; }
	}

	/// <summary>
	/// Delegate for the LegendItemToggleHandler event.  
	/// </summary>
	/// <param name="legendItem">Used to specify the legend item.</param>
	/// <exclude/>
	public delegate void LegendHandler(ILegendItem legendItem);
}
