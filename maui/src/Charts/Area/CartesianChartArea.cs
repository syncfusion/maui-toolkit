using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Helps to define and manage the area in a Cartesian chart where data is plotted, including gridlines, axes, and layout customization for Cartesian-based data series.
	/// </summary>
	internal partial class CartesianChartArea : AreaBase, ICartesianChartArea
	{
		#region Private Fields
		readonly CartesianPlotArea _cartesianPlotArea;
		readonly AbsoluteLayout _behaviorLayout;
		bool _isSbsWithOneData;
		bool _isTransposed;
		bool _enableSideBySideSeriesPlacement = true;
		Dictionary<string, List<StackingSeriesBase>>? _seriesGroup;
		RectF _actualSeriesClipRect;
		ChartSeriesCollection? _series;
		readonly Element _sourceParent;
		#endregion

		#region Internal Properties
		internal readonly AnnotationLayout _annotationLayout;
		internal readonly CartesianAxisLayout _axisLayout;
		internal readonly ObservableCollection<ChartAxis> _xAxes;
		internal readonly ObservableCollection<RangeAxisBase> _yAxes;

		internal Thickness PlotAreaMargin { get; set; } = Thickness.Zero;

		#region Chart Properties

		/// <summary>
		/// Boolean used to clear associated axis and register series. 
		/// </summary>
		internal bool RequiredAxisReset { get; set; } = true;

		internal IList<Brush>? PaletteColors { get; set; }

		internal Rect SeriesClipRect { get; set; }

		internal RectF ActualSeriesClipRect { get { return _actualSeriesClipRect; } set { _actualSeriesClipRect = value; } }

		#endregion

		#region Public Properties

		public ChartAxis? PrimaryAxis { get; set; }

		public RangeAxisBase? SecondaryAxis { get; set; }

		public ChartSeriesCollection? Series
		{
			get
			{
				return _series;
			}
			set
			{
				if (value != _series)
				{
					_series = value;
					_cartesianPlotArea.Series = value;
				}
			}
		}

		public ReadOnlyObservableCollection<ChartSeries>? VisibleSeries => Series?.GetVisibleSeries();

		public override IPlotArea PlotArea => _cartesianPlotArea;
		#endregion

		#endregion

		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		public CartesianChartArea(SfCartesianChart chart)
		{
			BatchBegin();
			_xAxes = chart.XAxes;
			_xAxes.CollectionChanged += XAxes_CollectionChanged;
			_yAxes = chart.YAxes;
			_yAxes.CollectionChanged += YAxes_CollectionChanged;
			PaletteColors = ChartColorModel.DefaultBrushes;
			_cartesianPlotArea = new CartesianPlotArea(this)
			{
				_chart = chart
			};
			_sourceParent = chart;
			_axisLayout = new CartesianAxisLayout(this);
			_annotationLayout = new AnnotationLayout(chart);
			_behaviorLayout = chart.BehaviorLayout;
			AbsoluteLayout.SetLayoutBounds(_axisLayout, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_axisLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(_cartesianPlotArea, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_cartesianPlotArea, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(_annotationLayout, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_annotationLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(_behaviorLayout, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_behaviorLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
			Add(_cartesianPlotArea);
			Add(_axisLayout);
			_axisLayout.InputTransparent = true;
			Add(_annotationLayout);
			Add(_behaviorLayout);
			BatchCommit();
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override void UpdateAreaCore()
		{
			if (_cartesianPlotArea._chart is not IChart cartesianChart)
			{
				return;
			}

			//Hide tooltip if any update happen in chart area.
			cartesianChart.ResetTooltip();

			_axisLayout.AssignAxisToSeries();
			_axisLayout.LayoutAxis(AreaBounds);

			_cartesianPlotArea.Margin = PlotAreaMargin;
			_behaviorLayout.Margin = PlotAreaMargin;

			//Size and position of the plot area, subtracting title and legend size.
			cartesianChart.ActualSeriesClipRect = ChartUtils.GetSeriesClipRect(
				AreaBounds.SubtractThickness(PlotAreaMargin),
				_cartesianPlotArea._chart.TitleHeight);

			//Need to set the trackballview padding to display the trackball template exact position
			if (cartesianChart is SfCartesianChart chart)
				chart._trackballView.Padding = PlotAreaMargin;

			UpdateVisibleSeries(); //series create segment logics.

			if (cartesianChart is SfCartesianChart sfChart)
			{
				//Casting required as annotation only support for cartesian chart. 
				sfChart.UpdateAnnotationLayout();
			}

			//Invalidate views.
			_annotationLayout.InvalidateDrawable();
			_axisLayout.InvalidateRender();
			_cartesianPlotArea.InvalidateRender();
		}

		public void UpdateVisibleSeries()
		{
			_cartesianPlotArea.UpdateVisibleSeries();
		}

		#endregion

		internal void OnPaletteColorChanged()
		{
			if (Series?.Count > 0)
			{
				foreach (var series in Series)
				{
					if (series is CartesianSeries cartesian && cartesian.Chart != null)
					{
						series.UpdateColor();
						series.InvalidateSeries();

						if (series.ShowDataLabels)
						{
							series.InvalidateDataLabel();
						}

						series.UpdateLegendIconColor();
					}
				}
			}
		}

		#region Axes Collection Changed
		void XAxes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			e.ApplyCollectionChanges(
				(obj, index, canInsert) => AddAxes(obj),
				(obj, index) => RemoveAxes(obj),
				ResetAxes
				);

			PrimaryAxis = null;

			if (sender is ObservableCollection<ChartAxis> axes && axes.Count > 0)
			{
				PrimaryAxis = axes[0];
			}

			// Mark for relayout and schedule an update of the chart area
			NeedsRelayout = true;
			ScheduleUpdateArea();
		}

		void YAxes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			e.ApplyCollectionChanges(
				(obj, index, canInsert) => AddAxes(obj),
				(obj, index) => RemoveAxes(obj),
				ResetAxes
				);

			SecondaryAxis = null;

			if (sender is ObservableCollection<RangeAxisBase> axes && axes.Count > 0)
			{
				SecondaryAxis = axes[0];
			}

			// Mark for relayout and schedule an update of the chart area
			NeedsRelayout = true;
			ScheduleUpdateArea();
		}

		void AddAxes(object obj)
		{
			if (obj is ChartAxis axis)
			{
				axis.Parent = _sourceParent;
				axis.Area = this;
				RequiredAxisReset = true;
				SetInheritedBindingContext(axis, BindingContext);
			}
		}

		void RemoveAxes(object obj)
		{
			if (obj is ChartAxis axis)
			{
				axis.Parent = null;
				RequiredAxisReset = true;
				axis.Area = null;
				SetInheritedBindingContext(axis, null);
				//TODO:Need to unhook if any event hooked.
			}
		}

		void ResetAxes()
		{
			//Axes not be reset.
		}
		#endregion

		#endregion

		#region Destructor

		~CartesianChartArea()
		{
			_xAxes.CollectionChanged -= XAxes_CollectionChanged;
			_yAxes.CollectionChanged -= YAxes_CollectionChanged;
		}

		#endregion
	}
}
