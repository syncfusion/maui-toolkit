using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class SeriesView : SfView
	{
		#region Fields

		const string AnimationName = "ChartAnimation";

		internal readonly ChartSeries _series;

		readonly IChartPlotArea _chartPlotArea;

		#endregion

		#region Constructor

		internal SeriesView(ChartSeries chartSeries, IChartPlotArea plotArea)
		{
			_series = chartSeries;
			_chartPlotArea = plotArea;
			SetBinding(IsVisibleProperty,
				BindingHelper.CreateBinding(nameof(ChartSeries.IsVisible), getter: static (ChartSeries series) => series.IsVisible, source: chartSeries));
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			canvas.CanvasSaveState();
			canvas.ClipRectangle(dirtyRect);

			_series?.DrawSeries(canvas, _series._readOnlySegments, dirtyRect);

			canvas.CanvasRestoreState();

			if (_series is IMarkerDependent series && !_series.NeedToAnimateSeries && series.ShowMarkers && series.MarkerSettings != null)
			{
				canvas.CanvasSaveState();
				canvas.ClipRectangle(dirtyRect);
				canvas.Alpha = _series.AnimationValue;
				series.OnDrawMarker(canvas, _series._readOnlySegments, dirtyRect);
				canvas.CanvasRestoreState();
			}
		}

		#endregion

		#region Internal Methods

		internal void UpdateSeries()
		{
			InternalCreateSegments();
			Invalidate();

			if (!_series.NeedToAnimateSeries)
			{
				InvalidateDrawable();
			}
		}

		internal void Animate()
		{
			if (_series.EnableAnimation && _series.SeriesAnimation == null)
			{
				_series.SeriesAnimation = new Animation(OnAnimationStart);
			}
			else
			{
				AbortAnimation();
			}

			if (_series.OldSegments != null)
			{
				_series.OldSegments.Clear();
				_series.OldSegments = null;
			}

			_series.NeedToAnimateSeries = true;

			if (_series.ShowDataLabels)
			{
				//Disable data label before the series dynamic animation.
				_chartPlotArea.DataLabelView.InvalidateDrawable();
			}

			StartAnimation();
			InvalidateDrawable();
		}

		internal void AbortAnimation()
		{
			this.AbortAnimation(AnimationName);
			_series.NeedToAnimateSeries = false;
			_series.IsDataPointAddedDynamically = false;
		}

		internal void InternalCreateSegments()
		{
			if (!_series.SegmentsCreated)
			{
				if (_series._segments.Count != 0)
				{
					_series._segments.Clear();
					_series.DataLabels.Clear();
				}

				_series.GenerateSegments(this);

				if (_series.OldSegments != null)
				{
					_series.OldSegments.Clear();
					_series.OldSegments = null;
				}

				_series.UpdateEmptyPointSettings();

				_series.SegmentsCreated = true;
			}
		}

		internal void Invalidate()
		{
			_series.OnSeriesLayout();

			foreach (var segment in _series._segments)
			{
				segment.OnLayout();

				if (_series.ShowDataLabels && !_series.NeedToAnimateSeries)
				{
					segment.OnDataLabelLayout();
				}
			}

			if (_series.CanAnimate())
			{
				StartAnimation();
			}

			//Todo: Need to check alternate solution.
			if (_series is CircularSeries circular && !(circular is RadialBarSeries) && circular.ShowDataLabels && !circular.NeedToAnimateSeries)
			{
				circular.ChangeIntersectedLabelPosition();
			}
		}

		#endregion

		#region Private Methods

		void OnAnimationStart(double value)
		{
			if (value >= 0.0)
			{
				_series.AnimationValue = (float)value;

				if (_series.NeedToAnimateSeries || (_series is IMarkerDependent series && series.NeedToAnimateMarker))
				{
					InvalidateDrawable();
				}
				else if (_series.NeedToAnimateDataLabel)
				{
					_chartPlotArea.DataLabelView.InvalidateDrawable();
				}
			}
		}

		void OnAnimationFinished(double value, bool isCompleted)
		{
			if (_series.NeedToAnimateSeries)
			{
				_series.NeedToAnimateSeries = false;
				Invalidate();
				AbortAnimation();

				if (_series.ShowDataLabels)
				{
					_series.NeedToAnimateDataLabel = true;
					_series.SeriesAnimation?.Commit(this, AnimationName, 16, 1000, null, OnAnimationFinished, () => false);
				}

				if (_series is IMarkerDependent series && series.ShowMarkers)
				{
					series.NeedToAnimateMarker = true;
					_series.SeriesAnimation?.Commit(this, AnimationName, 16, 1000, null, OnAnimationFinished, () => false);
				}
			}
			else
			{
				if (_series is IMarkerDependent series && series.ShowMarkers)
				{
					series.NeedToAnimateMarker = false;
				}

				_series.NeedToAnimateDataLabel = false;
				AbortAnimation();
			}
		}

		void StartAnimation()
		{
			//Todo: Need to move this code to series propertyChanged event. Fow now added here.
			if (_series.EnableAnimation && _series.SeriesAnimation == null)
			{
				_series.SeriesAnimation = new Animation(OnAnimationStart);
			}
			else if (!_series.EnableAnimation && _series.SeriesAnimation != null)
			{
				AbortAnimation();
			}

			if (_series.SeriesAnimation != null)
			{
				_series.AnimateSeries(OnAnimationStart);
				_series.SeriesAnimation.Commit(this, AnimationName, 1, (uint)(_series.AnimationDuration * 1000), null, OnAnimationFinished, () => false);
			}
		}

		#endregion

		#endregion

		#region Accessibility Support

		/// <summary>
		/// Provides semantic nodes for screen reader accessibility support.
		/// Creates a semantic node for each chart segment with descriptive information.
		/// </summary>
		/// <param name="width">The width of the view</param>
		/// <param name="height">The height of the view</param>
		/// <returns>List of semantic nodes for chart segments</returns>
		protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
		{
			if (_series == null || _series._segments == null || _series._segments.Count == 0)
			{
				return null;
			}

			var semanticNodes = new List<SemanticsNode>();
			int index = 0;

			foreach (var segment in _series._segments)
			{
				if (segment == null || !segment.IsVisible || segment.SegmentBounds == RectF.Zero)
				{
					continue;
				}

				var semanticsNode = new SemanticsNode
				{
					Id = index,
					Bounds = new Rect(segment.SegmentBounds.X, segment.SegmentBounds.Y, 
						segment.SegmentBounds.Width, segment.SegmentBounds.Height),
					Text = GetSegmentAccessibilityText(segment, index),
					IsTouchEnabled = true
				};

				semanticNodes.Add(semanticsNode);
				index++;
			}

			return semanticNodes.Count > 0 ? semanticNodes : null;
		}

		/// <summary>
		/// Generates accessible description text for a chart segment.
		/// Includes segment index, label, value, and percentage information when available.
		/// </summary>
		/// <param name="segment">The chart segment</param>
		/// <param name="index">The segment index</param>
		/// <returns>Descriptive text for screen readers</returns>
		string GetSegmentAccessibilityText(ChartSegment segment, int index)
		{
			var textParts = new List<string>();

			// Add segment index
			textParts.Add($"Segment {index + 1}");

			// Try to get label from data item if available
			if (segment.Item != null && !string.IsNullOrEmpty(_series.XBindingPath))
			{
				try
				{
					var labelValue = GetPropertyValue(segment.Item, _series.XBindingPath);
					if (labelValue != null)
					{
						textParts.Add(labelValue.ToString()!);
					}
				}
				catch
				{
					// Ignore if unable to get label
				}
			}

			// Add value information for circular series
			if (_series is CircularSeries circularSeries && segment is PieSegment pieSegment)
			{
				textParts.Add($"Value: {pieSegment.YValue}");

				// Calculate and add percentage
				if (circularSeries._sumOfYValues > 0 && !float.IsNaN(circularSeries._sumOfYValues))
				{
					double percentage = (pieSegment.YValue / circularSeries._sumOfYValues) * 100;
					textParts.Add($"{percentage:F1}%");
				}
			}
			// For other segment types, try to get label content
			else if (!string.IsNullOrEmpty(segment.LabelContent))
			{
				textParts.Add(segment.LabelContent);
			}

			return string.Join(", ", textParts);
		}

		/// <summary>
		/// Gets a property value from an object using the property path.
		/// </summary>
		/// <param name="obj">The source object</param>
		/// <param name="propertyPath">The property path</param>
		/// <returns>The property value or null if not found</returns>
		static object? GetPropertyValue(object obj, string propertyPath)
		{
			if (obj == null || string.IsNullOrEmpty(propertyPath))
			{
				return null;
			}

			try
			{
				var fastReflection = new FastReflection();
				if (fastReflection.SetPropertyName(propertyPath, obj))
				{
					return fastReflection.GetValue(obj);
				}
			}
			catch
			{
				// Return null if reflection fails
			}

			return null;
		}

		#endregion
	}
}