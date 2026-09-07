using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Manages drill down operations for the SunburstChart.
    /// </summary>
    internal class DrillDownManager
    {
		#region Fields

		private readonly SfSunburstChart chart;

		#endregion

        #region internal fields

        /// <summary>
        /// Gets or sets the previous items source.
        /// </summary>
        internal Stack<object> PreviousContexts { get; set; }

        /// <summary>
        /// Gets or sets the zooming state slice index values.
        /// </summary>
        internal Stack<int> PreviousSliceIndexes { get; set; }

        /// <summary>
        /// Gets or sets the zooming state levels count.
        /// </summary>
        internal Stack<int> PreviousLevelsCount { get; set; }

        /// <summary>
        /// Gets or sets the zooming state levels.
        /// </summary>
        internal Stack<int> PreviousLevels { get; set; }

        /// <summary>
        /// Gets or sets the zooming target segment at each state.
        /// </summary>
        internal Stack<SunburstSegment> SelectedSegments { get; set; }

        /// <summary>
        /// Gets or sets the previous size of the drilled segment
        /// </summary>
        internal Stack<double> PreviousRingSizes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether chart zoomed or not and animated or not.
        /// </summary>
        internal bool IsZoomed;

        internal int ZoomingLevel = -1;

        /// <summary>
        /// Gets or sets the zooming slice index.
        /// </summary>
        internal int ZoomingSlice = -1;

        /// <summary>
        /// Gets or sets the enumerate levels count.
        /// </summary>
        internal int EnumerateLevels = -1;

        internal float PreviousStartAngle, PreviousEndAngle;

        /// <summary>
        /// Gets or sets the zoom animation value for drill down operations.
        /// </summary>
        internal float ZoomAnimationValue { get; set; } = 1;

		/// <summary>
		/// Gets or sets the zoom animation value for drill down operations.
		/// </summary>
		internal float AlphaAnimationValue { get; set; } = 1;

		/// <summary>
		/// Gets or sets a value indicating whether a double-click drill down is in progress.
		/// </summary>
		internal bool IsDoubleClicked { get; set; }

        internal bool IsUnZoomAnimated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a back button drill up is in progress.
        /// </summary>
        internal bool IsBackButtonClicked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a reset button operation is in progress.
        /// </summary>
        internal bool IsResetButtonClicked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether fade-in animation is active.
        /// </summary>
        internal bool IsFadeInAnimated { get; set; }

        /// <summary>
        /// Gets or sets the first tapped segment for drill-down reference.
        /// </summary>
        internal SunburstSegment? FirstTappedSegment { get; set; }

        /// <summary>
        /// Gets or sets the drilled segment for animation reference.
        /// </summary>
        internal SunburstSegment DrilledSegment { get; set; }

       internal Stack<SunburstSegment> Parents { get; set; }

        /// <summary>
        /// Gets or sets the drill down segment for animation.
        /// </summary>
        internal SunburstSegment DrillDownSegment { get; set; }

        /// <summary>
        /// Gets or sets the previous segment for animation reference.
        /// </summary>
        internal SunburstSegment? PreviousSegment { get; set; }

        /// <summary>
        /// Gets or sets the zooming size for drill-down.
        /// </summary>
        internal double ZoomingSize { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DrillDownManager"/> class.
        /// </summary>
        /// <param name="chart">The SunburstChart instance.</param>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public DrillDownManager(SfSunburstChart chart)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            this.chart = chart;
            PreviousLevels = new Stack<int>();
            PreviousRingSizes = new Stack<double>();
            PreviousContexts = new Stack<object>();
            PreviousSliceIndexes = new Stack<int>();
            PreviousLevelsCount = new Stack<int>();
            SelectedSegments = new Stack<SunburstSegment>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Hides the toolbar.
        /// </summary>
        internal void HideToolbar()
        {
            if (chart.DrillDownToolbar == null)
                return;

            chart.DrillDownToolbar.IsVisible = false;
        }

        /// <summary>
        /// Performs a drill down operation on the specified segment.
        /// </summary>
        /// <param name="segment">The segment to drill down into.</param>
        internal void DrillDown(SunburstSegment segment)
        {
            if (IsZoomed && segment.CurrentLevel == ZoomingLevel)
            {
                return;
            }

			SelectedSegments.Push(segment);
            DrillDownSegment = segment;
            if (!IsZoomed)
            {
                FirstTappedSegment = segment;
                chart.PreviousSize = chart.RingSize;
            }

            IsZoomed = IsDoubleClicked = true;

            PreviousRingSizes.Push(chart.RingSize);
            PreviousLevels.Push(ZoomingLevel);
            PreviousSliceIndexes.Push(ZoomingSlice);
            PreviousLevelsCount.Push(EnumerateLevels);
            PreviousContexts.Push(chart.InternalDataSource ?? chart.ItemsSource);


            ZoomingLevel = segment.CurrentLevel;
            ZoomingSlice = segment.Index;
            EnumerateLevels = chart.Levels.Count - segment.CurrentLevel;
			chart.NeedToolbarPositionChange = true;
			chart.InternalDataSource = segment.SunburstItems?.Values;

        }

        /// <summary>
        /// Performs a drill up operation, moving back one level.
        /// </summary>
        internal void DrillUp()
        {
            IsBackButtonClicked = true;
            IsUnZoomAnimated = true;

            if (SelectedSegments.Count > 0)
            {
                DrilledSegment = chart.Segments[0];
                DrillDownSegment = SelectedSegments.Pop();
            }

            if (PreviousRingSizes.Count > 0)
            {
                ZoomingSize = PreviousRingSizes.Pop();
            }

            chart.AnimateDrillDown(500);
        }

        /// <summary>
        /// Changes the data source when the zoom back button is clicked
        /// </summary>
        internal void ChangeDataSourceOnZoomBack()
        {
            if (IsUnZoomAnimated)
            {
                IsUnZoomAnimated = false;
                IsFadeInAnimated = true;

                if (PreviousLevelsCount.FirstOrDefault() == -1)
                {
                    IsZoomed = false;
                    ZoomingLevel = EnumerateLevels = -1;
                    chart.InternalDataSource = (IEnumerable)PreviousContexts.Pop();
					HideToolbar();
                    return;
                }

                ZoomingLevel = PreviousLevels.Pop();
                ZoomingSlice = PreviousSliceIndexes.Pop();
                EnumerateLevels = PreviousLevelsCount.Pop();
				chart.InternalDataSource = (IEnumerable)PreviousContexts.Pop();

                // Hide toolbar if back at the top level
                if (DrillDownSegment?.CurrentLevel == 0 || PreviousContexts == null)
                {
                    HideToolbar();
                }
            }
        }

        /// <summary>
        /// Resets the drill down operation, returning to the initial chart view.
        /// </summary>
        internal void Reset()
        {
            IsResetButtonClicked = true;
            ZoomingSize = chart.PreviousSize;
            DrilledSegment = chart.Segments[0];

			UpdatePreviousSegmentOnReset();
			chart.AnimateDrillDown(500);
        }

        internal void ChangeDataSourceOnReset()
        {
            if (IsResetButtonClicked)
            {
                IsResetButtonClicked = false;
                IsFadeInAnimated = true;
                IsZoomed = false;
                EnumerateLevels = ZoomingLevel = -1;

                if (chart.Legend != null)
                {
                    chart.area.ShouldPopulateLegendItems = true;
                }
                chart.InternalDataSource = (IEnumerable)PreviousContexts.Last();

                ResetPreviousDrilledReferences();
                HideToolbar();

				if (!chart.EnableDrillDown)
					chart.DrillDownManager = null;
			}
        }

        void ResetPreviousDrilledReferences()
        {
            PreviousLevels.Clear();
            PreviousSliceIndexes.Clear();
            PreviousRingSizes.Clear();
            PreviousContexts.Clear();
            SelectedSegments.Clear();
            PreviousLevelsCount.Clear();
        }

        /// <summary>
        /// Gets the category from segment's Item property.
        /// </summary>
        /// <param name="segment">The segment to get category from.</param>
        /// <returns>The category string.</returns>
        string GetSegmentCategory(SunburstSegment? segment)
        {
            if (segment?.Item is List<object?> items && items.Count > 0)
            {
                return items[0]?.ToString() ?? String.Empty;
            }
            return String.Empty;
        }

		internal void UpdatePreviousSegmentOnReset()
		{
			// Handle reset button click
			if (FirstTappedSegment?.Childs != null)
			{
				foreach (var childSegment in FirstTappedSegment.Childs)
				{
					// Compare using Item[0] which contains the category
					var segmentCategory = GetSegmentCategory(childSegment);
					var drilledCategory = GetSegmentCategory(DrilledSegment);

					if (string.Equals(segmentCategory, drilledCategory, StringComparison.Ordinal))
					{
						PreviousSegment = childSegment;
						break;
					}
				}
			}
		}

		internal void RendererSegments(SunburstSegment segment, int index)
        {
			if (IsFadeInAnimated)
			{
				segment.IsFadeInNeeded = true;
				if (DrilledSegment.Childs != null)
				{
					foreach (var segments in DrilledSegment.Childs)
					{
						if (segment.EqualsTo(segments))
						{
							segment.IsFadeInNeeded = false;
							break;
						}
					}
				}
			}
			else
			{
				if (IsResetButtonClicked)
				{
					if(PreviousSegment?.Childs != null && index < PreviousSegment.Childs.Count)
					{
						PreviousStartAngle = (float)(PreviousSegment.Childs[index].ArcStartAngle * (180 / Math.PI));
						PreviousEndAngle = (float)(PreviousSegment.Childs[index].ArcEndAngle * (180 / Math.PI));
					}
				}

				if (IsDoubleClicked || IsBackButtonClicked)
				{
					if (DrillDownSegment?.Childs != null && index < DrillDownSegment.Childs.Count)
					{
						PreviousStartAngle = (float)(DrillDownSegment.Childs[index].ArcStartAngle * (180 / Math.PI));
						PreviousEndAngle = (float)(DrillDownSegment.Childs[index].ArcEndAngle * (180 / Math.PI));
					}
				}
			}
        }

        internal double GetDrilledSegmentRingSize()
        {
            double previousSize = 0;

            if (IsDoubleClicked)
            {
				previousSize = PreviousRingSizes.Last();
			}
            else if (IsBackButtonClicked || IsResetButtonClicked)
			{
				previousSize = ZoomingSize;
			}
            else
            {
				return chart.RingSize;
			}

			double currentSize = chart.RingSize;
			double sizeToAnimate = (currentSize - previousSize) * (1 - ZoomAnimationValue);
			return currentSize - sizeToAnimate;
		}
        #endregion
    }
}