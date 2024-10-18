using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal class SeriesView : SfDrawableView
    {
        #region Fields

        const string _animationName = "ChartAnimation";

        internal readonly ChartSeries Series;

        readonly IChartPlotArea _chartPlotArea;

        #endregion

        #region Constructor

        internal SeriesView(ChartSeries chartSeries, IChartPlotArea plotArea)
        {
            Series = chartSeries;
            _chartPlotArea = plotArea;
            SetBinding(IsVisibleProperty, new Binding() { Source = chartSeries, Path = nameof(ChartSeries.IsVisible) });
        }

        #endregion

        #region Methods

        #region Protected Methods

        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.CanvasSaveState();
            canvas.ClipRectangle(dirtyRect);

            Series?.DrawSeries(canvas, new ReadOnlyObservableCollection<ChartSegment>(Series.Segments), dirtyRect);

            canvas.CanvasRestoreState();

            if (Series is IMarkerDependent series && !Series.NeedToAnimateSeries && series.ShowMarkers && series.MarkerSettings != null)
            {
                canvas.CanvasSaveState();
                canvas.ClipRectangle(dirtyRect);
                canvas.Alpha = Series.AnimationValue;
                series.OnDrawMarker(canvas, new ReadOnlyObservableCollection<ChartSegment>(Series.Segments), dirtyRect);
                canvas.CanvasRestoreState();
            }
        }

        #endregion

        #region Internal Methods

        internal void UpdateSeries()
        {
            InternalCreateSegments();
            Invalidate();

            if (!Series.NeedToAnimateSeries)
                InvalidateDrawable();
        }

        internal void Animate()
        {
            if (Series.EnableAnimation && Series.SeriesAnimation == null)
            {
                Series.SeriesAnimation = new Animation(OnAnimationStart);
            }
            else
                AbortAnimation();

            if (Series.OldSegments != null)
            {
                Series.OldSegments.Clear();
                Series.OldSegments = null;
            }

            Series.NeedToAnimateSeries = true;

            if (Series.ShowDataLabels)
            {
                //Disable data label before the series dynamic animation.
                _chartPlotArea.DataLabelView.InvalidateDrawable();
            }

            StartAnimation();
            InvalidateDrawable();
        }

        internal void AbortAnimation()
        {
            this.AbortAnimation(_animationName);
            Series.NeedToAnimateSeries = false;
            Series.IsDataPointAddedDynamically = false;
        }

        internal void InternalCreateSegments()
        {
            if (!Series.SegmentsCreated)
            {
                if (Series.Segments.Count != 0)
                {
                    Series.Segments.Clear();
                    Series.DataLabels.Clear();
                }

                Series.GenerateSegments(this);

                if (Series.OldSegments != null)
                {
                    Series.OldSegments.Clear();
                    Series.OldSegments = null;
                }

                Series.SegmentsCreated = true;
            }
        }

        internal void Invalidate()
        {
            Series.OnSeriesLayout();

            foreach (var segment in Series.Segments)
            {
                segment.OnLayout();

                if (Series.ShowDataLabels && !Series.NeedToAnimateSeries)
                {
                    segment.OnDataLabelLayout();
                }
            }

            if (Series.CanAnimate())
                StartAnimation();

            //Todo: Need to check alternate solution.
            if (Series is CircularSeries circular && !(circular is RadialBarSeries) && circular.ShowDataLabels && !circular.NeedToAnimateSeries)
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
                Series.AnimationValue = (float)value;

                if (Series.NeedToAnimateSeries || (Series is IMarkerDependent series && series.NeedToAnimateMarker))
                {
                    InvalidateDrawable();
                }
                else if (Series.NeedToAnimateDataLabel)
                {
                    _chartPlotArea.DataLabelView.InvalidateDrawable();
                }
            }
        }

        void OnAnimationFinished(double value, bool isCompleted)
        {
            if (Series.NeedToAnimateSeries)
            {
                Series.NeedToAnimateSeries = false;
                Invalidate();
                AbortAnimation();

                if (Series.ShowDataLabels)
                {
                    Series.NeedToAnimateDataLabel = true;
                    Series.SeriesAnimation?.Commit(this, _animationName, 16, 1000, null, OnAnimationFinished, () => false);
                }

                if (Series is IMarkerDependent series && series.ShowMarkers)
                {
                    series.NeedToAnimateMarker = true;
                    Series.SeriesAnimation?.Commit(this, _animationName, 16, 1000, null, OnAnimationFinished, () => false);
                }
            }
            else
            {
                if (Series is IMarkerDependent series && series.ShowMarkers)
                {
                    series.NeedToAnimateMarker = false;
                }

                Series.NeedToAnimateDataLabel = false;
                AbortAnimation();
            }
        }

        void StartAnimation()
        {
            //Todo: Need to move this code to series propertyChanged event. Fow now added here.
            if (Series.EnableAnimation && Series.SeriesAnimation == null)
            {
                Series.SeriesAnimation = new Animation(OnAnimationStart);
            }
            else if (!Series.EnableAnimation && Series.SeriesAnimation != null)
            {
                AbortAnimation();
            }

            if (Series.SeriesAnimation != null)
            {
                Series.AnimateSeries(OnAnimationStart);
                Series.SeriesAnimation.Commit(this, _animationName, 1, (uint)(Series.AnimationDuration * 1000), null, OnAnimationFinished, () => false);
            }
        }

        #endregion

        #endregion
    }
}