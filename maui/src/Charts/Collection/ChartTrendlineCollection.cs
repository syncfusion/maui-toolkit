using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a collection of trendlines for a chart series.
	/// </summary>
	public class ChartTrendlineCollection : ObservableCollection<ChartTrendline>
	{
		#region Fields

		readonly CartesianSeries? series;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartTrendlineCollection"/> class.
		/// </summary>
		public ChartTrendlineCollection()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ChartTrendlineCollection"/> class with a parent series.
		/// </summary>
		/// <param name="parent">The parent chart series.</param>
		internal ChartTrendlineCollection(CartesianSeries parent) : this()
		{
			series = parent;
		}

		#endregion
	}
}
