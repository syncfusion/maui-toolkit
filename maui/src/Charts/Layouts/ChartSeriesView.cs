using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class SeriesView : SfDrawableView
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

			_series?.DrawSeries(canvas, new ReadOnlyObservableCollection<ChartSegment>(_series._segments), dirtyRect);

			canvas.CanvasRestoreState();

			if (_series is IMarkerDependent series && !_series.NeedToAnimateSeries && series.ShowMarkers && series.MarkerSettings != null)
			{
				canvas.CanvasSaveState();
				canvas.ClipRectangle(dirtyRect);
				canvas.Alpha = _series.AnimationValue;
				series.OnDrawMarker(canvas, new ReadOnlyObservableCollection<ChartSegment>(_series._segments), dirtyRect);
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
	}
}