using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Toolkit.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Defines and manages the layout and rendering logic for a Sunburst chart, enabling hierarchical data visualization through concentric segments. 
    /// Handles legend updates, layout customization, and interaction support for Sunburst-based data series.
    /// </summary>
    internal class SunburstChartArea : AreaBase, IPlotArea
    {
        #region Private Fields

        EventHandler<EventArgs>? _legendItemsUpdated;
        EventHandler<LegendItemEventArgs>? _legendItemsToggled;
        bool _shouldPopulateLegendItems = true;
        readonly SfSunburstChart _chart;
        #endregion

        #region Internal Fields

        /// <summary>
        /// Collection of legend items.
        /// </summary>
        internal readonly ObservableCollection<ILegendItem> legendItems;

        /// <summary>
        /// The legend instance.
        /// </summary>
        internal ILegend? Legend { get; set; }

        /// <summary>
        /// The view for rendering series.
        /// </summary>
        internal SeriesView SeriesView { get; set; }

        /// <summary>
        /// The view for rendering data labels.
        /// </summary>
        internal DataLabelView DataLabelView { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SunburstChartArea"/> class.
        /// </summary>
        /// <param name="chart">The parent chart instance.</param>
        public SunburstChartArea(SfSunburstChart chart)
        {
            BatchBegin();
            _chart = chart;
            legendItems = new ObservableCollection<ILegendItem>();

            SeriesView = new SeriesView(chart);
            AbsoluteLayout.SetLayoutBounds(SeriesView, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(SeriesView, AbsoluteLayoutFlags.All);
            Add(SeriesView);

            DataLabelView = new DataLabelView(chart);
            AbsoluteLayout.SetLayoutBounds(DataLabelView, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(DataLabelView, AbsoluteLayoutFlags.All);
            Add(DataLabelView);

            chart.BehaviorLayout = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutBounds(chart.BehaviorLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(chart.BehaviorLayout, AbsoluteLayoutFlags.All);
            Add(chart.BehaviorLayout);

            BatchCommit();
        }

        #endregion

        #region Override Properties

        /// <summary>
        /// Gets the plot area.
        /// </summary>
		public override IPlotArea PlotArea => this;
        event EventHandler<EventArgs> IPlotArea.LegendItemsUpdated { add { _legendItemsUpdated += value; } remove { _legendItemsUpdated -= value; } }
        event EventHandler<LegendItemEventArgs>? IPlotArea.LegendItemToggled { add { _legendItemsToggled += value; } remove { _legendItemsToggled -= value; } }

        /// <summary>
        /// Gets or sets a value indicating whether legend items should be populated.
        /// </summary>
        public bool ShouldPopulateLegendItems
        {
            get => _shouldPopulateLegendItems;
            set => _shouldPopulateLegendItems = value;
        }

        LegendHandler IPlotArea.LegendItemToggleHandler
        {
            get => ToggleLegendItem;
        }

        /// <summary>
        /// Toggles the visibility of a legend item.
        /// </summary>
        /// <param name="legendItem">The legend item to toggle.</param>
        void ToggleLegendItem(ILegendItem legendItem)
        {
            if (Legend != null && Legend.IsVisible && Legend.ToggleVisibility)
            {
			    //TODO: Implement legend item toggle functionality.
			}
		}

        ILegend? IPlotArea.Legend
        {
			get => Legend;
			set => Legend = value != Legend ? value : Legend;
        }

		/// <summary>
		/// Gets the collection of legend items.
		/// </summary>
		public ReadOnlyObservableCollection<ILegendItem> LegendItems => new ReadOnlyObservableCollection<ILegendItem>(legendItems);

		/// <summary>
		/// Gets or sets the bounds of the plot area.
		/// </summary>
		public Rect PlotAreaBounds { get; set; }
		#endregion

		#region Protected Override Methods
		/// <summary>
		/// Updates the core elements of the chart area.
		/// </summary>
		protected override void UpdateAreaCore()
		{
			if (_chart != null)
			{
				_chart.Hide();
				var seriesClip = GetSeriesClipRect(AreaBounds, _chart.TitleHeight);
				_chart.ActualSeriesClipRect = seriesClip;
				_chart.SeriesRenderBounds = new Rect(new Point(0, 0), seriesClip.Size);
				_chart.GetRadius();
				_chart.GenerateSegments();
				SeriesView?.Layout();
			    Invalidate();
			}
		}

		/// <summary>
		/// Updates the legend items source.
		/// </summary>
		protected void UpdateLegendItemsSource()
        {
            if (_chart.Levels == null || _chart.Levels.Count == 0 || legendItems == null)
            {
                return;
            }

            legendItems.Clear();
            var items = _chart.Levels[0].SunburstItems;

            if (items != null && items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var currentItem = items[i];
                    if (currentItem != null)
                    {
                        var legendItem = new LegendItem();
                        legendItem.IconType = ShapeType.Circle;
                        var solidColor = _chart.GetFillColor(i) ?? Brush.Transparent;
                        legendItem.IconBrush = solidColor ?? new SolidColorBrush(Colors.Transparent);
                        legendItem.Text = currentItem.Key?.ToString() ?? string.Empty;
                        legendItem.Index = currentItem.SliceIndex;
                        UpdateLegendItem(legendItem);
                        legendItem.Source = _chart;
                        legendItem.Item = currentItem;
                        legendItems.Add(legendItem);
                    }
                }
            }
        }

		/// <summary>
		/// Updates the visual properties of a legend item.
		/// </summary>
		/// <param name="legendItem">The legend item to update.</param>
		private void UpdateLegendItem(LegendItem legendItem)
        {
            if (_chart != null)
            {
                var color = _chart.LegendStyle.TextColor;
                legendItem.TextColor = color;
                legendItem.FontSize = (float)_chart.LegendStyle.FontSize;
                legendItem.DisableBrush = Color.FromRgba(color.Red, color.Green, color.Blue, 0.38);
            }
        }

		/// <summary>
		/// Updates the attributes of all legend items.
		/// </summary>
		internal void UpdateLegendItemAttributes()
        {
            if (_chart != null && legendItems != null && legendItems.Count > 0)
            {
                foreach (LegendItem legendItem in legendItems)
                {
                    UpdateLegendItem(legendItem);
                }

                _legendItemsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
		#endregion

		#region Internal Methods

		/// <summary>
		/// Invalidates the drawable views for the series and data labels.
		/// </summary>
		private void Invalidate()
        {
            if (!_chart.NeedToAnimate)
            {
                SeriesView?.InvalidateDrawable();
                DataLabelView?.InvalidateDrawable();
            }
        }

		/// <summary>
		/// Updates the icon color of the legend items.
		/// </summary>
        internal void UpdateLegendIconColor()
        {
            if (_chart.Legend != null && _chart.Legend.IsVisible && LegendItems != null)
            {
                foreach (LegendItem legendItem in LegendItems)
                {
                    if (legendItem != null)
                    {
                        legendItem.IconBrush = _chart.GetFillColor(legendItem.Index) ?? new SolidColorBrush(Colors.Transparent);
                        // break;
                    }
                }
            }
        }

		/// <summary>
		/// Updates the legend items.
		/// </summary>
		public void UpdateLegendItems()
        {
            if (_shouldPopulateLegendItems)
            {
                UpdateLegendItemsSource();
                _shouldPopulateLegendItems = false;
                _legendItemsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

		/// <summary>
		/// Calculates the clipping rectangle for the series.
		/// </summary>
		/// <param name="seriesClipRect">The original clipping rectangle.</param>
		/// <param name="titleHeight">The height of the chart title.</param>
		/// <returns>The adjusted clipping rectangle.</returns>
		private Rect GetSeriesClipRect(Rect seriesClipRect, double titleHeight)
        {
            return new Rect(
                seriesClipRect.X,
                seriesClipRect.Y + titleHeight,
                seriesClipRect.Width,
                seriesClipRect.Height);
        }

        #endregion
    }
}

