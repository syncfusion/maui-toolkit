using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts; 

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class DefaultSegmentUnitTest : BaseUnitTest
	{
		 
		[Fact]
		public void AreaSegment_InitializesBasicPropertiesCorrectly()
		{
			var segment = new AreaSegment(); 
			Assert.Null(segment.XValues);
			Assert.Null(segment.YValues);
			Assert.Null(segment.FillPoints);
			Assert.Null(segment.StrokePoints);
			Assert.Null(segment.PreviousFillPoints);
			Assert.Null(segment.PreviousStrokePoints);
			Assert.Null(segment.Series);
			Assert.False(segment.Empty);
			 
		}

		[Fact]
		public void AreaSegment_InitializesStylePropertiesCorrectly()
		{
			var segment = new AreaSegment(); 
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Null(segment.StrokeDashArray);
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Null(segment.Item); 
		}

		[Fact]
		public void AreaSegment_InitializesInternalsPropertiesCorrectly()
		{
			var segment = new AreaSegment();
			Assert.True(segment.IsVisible); 
			Assert.False(segment.HasStroke);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.False(segment.IsSelected);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition); 
			Assert.Empty(segment.DataLabels); 
			Assert.IsAssignableFrom<IMarkerDependentSegment>(segment);

		}

		[Fact]
		public void AreaSegment_InitializesInternalPropertiesCorrectly()
		{
			var segment = new AreaSegment(); 	
			Assert.Null(segment.LabelContent);
			Assert.NotNull(segment.XPoints);
			Assert.Empty(segment.XPoints);
			Assert.NotNull(segment.YPoints);
			Assert.Empty(segment.YPoints);
			Assert.True(segment.InVisibleRange);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.False(segment.IsZero);
			Assert.Null(segment.SeriesView);
		}

		[Fact]
		public void BoxAndWhiskerSegment_InitializesDefaultsCorrectly()
		{ 
			var segment = new BoxAndWhiskerSegment(); 
			Assert.Equal(0, segment.Maximum);
			Assert.Equal(0, segment.Minimum);
			Assert.Equal(0, segment.Median);
			Assert.Equal(0, segment.LowerQuartile);
			Assert.Equal(0, segment.UpperQuartile);
			Assert.Equal(0, segment.Left);
			Assert.Equal(0, segment.Right);
			Assert.Equal(0, segment.Top);
			Assert.Equal(0, segment.Bottom);
			Assert.Equal(0, segment.Center); 
			Assert.Null(segment.Series); 
		}

		[Fact]
		public void BoxAndWhiskerSegment_InitializesStylePropertiesCorrectly()
		{
			var segment = new BoxAndWhiskerSegment(); 
			Assert.False(segment.Empty);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Item);
			Assert.IsAssignableFrom<CartesianSegment>(segment);
		}

		[Fact]
		public void BoxAndWhiskerSegment_InitializesInternalsPropertiesCorrectly()
		{
			var segment = new BoxAndWhiskerSegment(); 
			Assert.Equal(0, segment._outlierIndex);  
			Assert.Empty(segment._outliers);
			Assert.True(segment.IsVisible); 
			Assert.False(segment.HasStroke);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.False(segment.IsSelected);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition); 
		}

		[Fact]
		public void BoxAndWhiskerSegment_InitializesInternalPropertiesCorrectly()
		{
			var segment = new BoxAndWhiskerSegment(); 
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.NotNull(segment.XPoints);
			Assert.Empty(segment.XPoints);
			Assert.NotNull(segment.YPoints);
			Assert.Empty(segment.YPoints);
			Assert.True(segment.InVisibleRange);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.False(segment.IsZero);
			Assert.Null(segment.SeriesView);
		} 

		[Fact]
		public void BubbleSegment_InitializesDefaultsCorrectly()
		{ 
			var segment = new BubbleSegment(); 
			Assert.Equal(0f, segment.CenterX);
			Assert.Equal(0f, segment.CenterY);
			Assert.Equal(0f, segment.Radius);
			Assert.True(float.IsNaN(segment.PreviousCenterX));
			Assert.True(float.IsNaN(segment.PreviousCenterY));
			Assert.True(float.IsNaN(segment.PreviousRadius)); 
			Assert.Null(segment.Series); 
			Assert.Equal(0.0, (double?)GetPrivateField(segment, "_x"));
			Assert.Equal(0.0, (double?)GetPrivateField(segment, "_y"));
			Assert.Equal(0.0, (double?)GetPrivateField(segment, "_sizeValue"));
		}

		[Fact]
		public void BubbleSegment_InitializesStylePropertiesCorrectly()
		{
			var segment = new BubbleSegment(); 
			Assert.False(segment.Empty);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Item);
		}
		[Fact]
		public void BubbleSegment_InitializesInternalsPropertiesCorrectly()
		{
			var segment = new BubbleSegment();
			Assert.True(segment.IsVisible);
			Assert.False(segment.HasStroke);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.False(segment.IsSelected);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition); 
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent); 
		}

		[Fact]
		public void BubbleSegment_InitializesInternalPropertiesCorrectly()
		{
			var segment = new BubbleSegment(); 
			Assert.Empty(segment.XPoints); 
			Assert.Empty(segment.YPoints);
			Assert.True(segment.InVisibleRange);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.False(segment.IsZero);
			Assert.Null(segment.SeriesView); 
			Assert.Equal(new RectF(0, 0, 0, 0), segment.SegmentBounds);
			Assert.IsAssignableFrom<CartesianSegment>(segment);
		}  

		[Fact]
		public void CandleSegment_InitializesDefaultsCorrectly()
		{
			var segment = new CandleSegment(); 
			Assert.Equal(0, segment.StartX);
			Assert.Equal(0, segment.CenterX);
			Assert.Equal(0, segment.EndX);
			Assert.Equal(0, segment.Open);
			Assert.Equal(0, segment.High);
			Assert.Equal(0, segment.Low);
			Assert.Equal(0, segment.Close);
			Assert.Equal(0, segment.XValue);
			Assert.False(segment.IsFill); 
		}

		[Fact]
		public void CandleSegment_InitializesDefaultCorrectly()
		{
			var segment = new CandleSegment(); 
			Assert.Equal(0, segment.Left);
			Assert.Equal(0, segment.Right);
			Assert.Equal(0, segment.Top);
			Assert.Equal(0, segment.Bottom);
			Assert.Equal(0, segment.CenterHigh);
			Assert.Equal(0, segment.CenterLow);
			Assert.Equal(0, segment.HighPointY);
			Assert.Equal(0, segment.LowPointY);
			Assert.Null(segment.Series);
		}

		[Fact]
		public void CandleSegment_InitializesStylePropertiesCorrectly()
		{
			var segment = new CandleSegment();
			Assert.False(segment.Empty);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Item); 
		}

		[Fact]
		public void CandleSegment_InitializesInternalsPropertiesCorrectly()
		{
			var segment = new CandleSegment();
			Assert.True(segment.IsVisible);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.False(segment.IsSelected);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition); 
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent); 
			Assert.Empty(segment.XPoints);  
		}

		[Fact]
		public void CandleSegment_InitializesInternalPropertiesCorrectly()
		{
			var segment = new CandleSegment(); 
			Assert.Empty(segment.YPoints);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.HasStroke);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.False(segment.IsZero);
			Assert.Null(segment.SeriesView); 
			Assert.False(segment.IsBull);
			Assert.Equal(new RectF(0, 0, 0, 0), segment.SegmentBounds);
			Assert.IsAssignableFrom<HiLoOpenCloseSegment>(segment);
		} 

		[Fact]
		public void ColumnSegment_InitializesBasicPropertiesCorrectly()
		{
			var segment = new ColumnSegment(); 
			Assert.Equal(0f, segment.Left);
			Assert.Equal(0f, segment.Top);
			Assert.Equal(0f, segment.Right);
			Assert.Equal(0f, segment.Bottom);
			Assert.True(float.IsNaN(segment.Y1));
			Assert.True(float.IsNaN(segment.Y2));
			Assert.True(float.IsNaN(segment.PreviousY1));
			Assert.True(float.IsNaN(segment.PreviousY2));
		}

		[Fact]
		public void ColumnSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new ColumnSegment(); 
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void ColumnSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new ColumnSegment(); 
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		}

		[Fact]
		public void ColumnSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new ColumnSegment(); 
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint); 
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
		}
		[Fact]
		public void DoughnutSegment_InitializesDoughnutSpecificPropertiesCorrectly()
		{
			var segment = new DoughnutSegment(); 
			Assert.Equal(0f, segment.InnerRadius);
			Assert.Equal(0, segment.StartAngle);
			Assert.Equal(0, segment.EndAngle);
			Assert.Equal(0, segment.YValue);
			Assert.Equal(0, segment.Radius);
			Assert.False(segment.Exploded);
			Assert.True(double.IsNaN(segment.PreviousStartAngle));
			Assert.True(double.IsNaN(segment.PreviousEndAngle));
			Assert.Equal(0, segment.MidAngle); 
			Assert.Equal(0, segment.SegmentStartAngle);
			Assert.Equal(0, segment.SegmentEndAngle);
			Assert.Equal(0, segment.SegmentMidAngle);
			Assert.Equal(0, segment.SegmentNewAngle);
		}
		 
		[Fact]
		public void DoughnutSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new DoughnutSegment(); 
			Assert.Equal(default(Rect), segment.LabelRect);
			Assert.False(segment.IsLeft);
			Assert.Equal(default(RenderingPosition), segment.RenderingPosition);
			Assert.Equal(string.Empty, segment.TrimmedText);
			Assert.Equal(default(Position), segment.DataLabelRenderingPosition);
			Assert.Equal(0, segment._isLabelUpdated);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
		}

		[Fact]
		public void DoughnutSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new DoughnutSegment(); 
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void DoughnutSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new DoughnutSegment(); 
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero); 
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
		} 

		[Fact]
		public void ErrorBarSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new ErrorBarSegment(); 
			Assert.Empty(segment.ErrorSegmentPoints);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void ErrorBarSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new ErrorBarSegment(); 
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		}

		[Fact]
		public void ErrorBarSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new ErrorBarSegment(); 
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint); 
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints); 
			Assert.IsAssignableFrom<ChartSegment>(segment);
			 
		}

		[Fact]
		public void FastLineSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new FastLineSegment(); 
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void FastLineSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new FastLineSegment(); 
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		}

		[Fact]
		public void FastLineSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new FastLineSegment(); 
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent); 
			Assert.NotNull(segment.XPoints);
			Assert.Empty(segment.XPoints);
			Assert.NotNull(segment.YPoints);
			Assert.Empty(segment.YPoints);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
		}

		[Fact]
		public void FastScatterSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new FastScatterSegment();
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);			
			Assert.False(segment.HasStroke);
		}

		[Fact]
		public void FastScatterSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new FastScatterSegment();
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsEmpty);
		}

		[Fact]
		public void FunnelSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new FunnelSegment(); 
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void FunnelSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new FunnelSegment(); 
			Assert.Null(segment.Chart);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		}

		[Fact]
		public void FunnelSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new FunnelSegment(); 
			Assert.NotNull(segment.Points);
			Assert.Equal(6, segment.Points.Length);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels); 
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
		}

		[Fact]
		public void HiLoOpenCloseSegment_InitializesPositionPropertiesCorrectly()
		{
			var segment = new HiLoOpenCloseSegment(); 
			Assert.Equal(0f, segment.Left);
			Assert.Equal(0f, segment.Top);
			Assert.Equal(0f, segment.Right);
			Assert.Equal(0f, segment.Bottom); 
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints); 
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView); 
			Assert.Equal(0d, segment.StartX);
		}

		[Fact]
		public void HiLoOpenCloseSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new HiLoOpenCloseSegment(); 
			Assert.Equal(0d, segment.CenterX);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void HiLoOpenCloseSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new HiLoOpenCloseSegment(); 
			Assert.Equal(0f, segment.LowPointY);
			Assert.Equal(0f, segment.HighPointY);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		}

		[Fact]
		public void HiLoOpenCloseSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new HiLoOpenCloseSegment(); 
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.NotNull(segment._dataLabel);
			Assert.Equal(4, segment._dataLabel.Length);
			Assert.NotNull(segment._labelPositions);
			Assert.Equal(4, segment._labelPositions.Length);
		}
		 
		[Fact]
		public void HiLoOpenCloseSegment_InitializesHiLoSpecificPropertiesCorrectly()
		{
			var segment = new HiLoOpenCloseSegment(); 
			Assert.Equal(0d, segment.EndX);
			Assert.Equal(0d, segment.High);
			Assert.Equal(0d, segment.Low);
			Assert.Equal(0d, segment.Open);
			Assert.Equal(0d, segment.Close);
			Assert.Equal(0d, segment.XValue);
			Assert.False(segment.IsBull);
			Assert.False(segment.IsFill);
			Assert.Equal(0f, segment.CenterLow);
			Assert.Equal(0f, segment.CenterHigh);
		}

		[Fact]
		public void HistogramSegment_InitializesPositionPropertiesCorrectly()
		{
			var segment = new HistogramSegment(); 
			Assert.Equal(0f, segment.Left);
			Assert.Equal(0f, segment.Top);
			Assert.Equal(0f, segment.Right);
			Assert.Equal(0f, segment.Bottom);
			Assert.True(float.IsNaN(segment.Y1));
			Assert.True(float.IsNaN(segment.Y2));
			Assert.True(float.IsNaN(segment.PreviousY1));
			Assert.True(float.IsNaN(segment.PreviousY2));
		}

		[Fact]
		public void HistogramSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new HistogramSegment(); 
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void HistogramSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new HistogramSegment(); 
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		}

		[Fact]
		public void HistogramSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new HistogramSegment(); 
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView); 
			Assert.Equal(0, segment.PointsCount);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Equal(0, segment.HistogramLabelPosition);
		}

		[Fact]
		public void LineSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new LineSegment(); 
			Assert.Equal(0f, segment.X2);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected); 
		}

		[Fact]
		public void LineSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new LineSegment(); 
			Assert.Equal(0f, segment.X1);
			Assert.Equal(0f, segment.Y1);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		}

		[Fact]
		public void LineSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new LineSegment(); 
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
		} 

		[Fact]
		public void LineSegment_InitializesLineSpecificPropertiesCorrectly()
		{
			var segment = new LineSegment(); 
			Assert.Equal(0f, segment.Y2);
			Assert.Equal(0d, segment.X1Pos);
			Assert.Equal(0d, segment.Y1Pos);
			Assert.Equal(0d, segment.X2Pos);
			Assert.Equal(0d, segment.Y2Pos);
			Assert.True(float.IsNaN(segment.PreviousX1));
			Assert.True(float.IsNaN(segment.PreviousY1));
			Assert.True(float.IsNaN(segment.PreviousX2));
			Assert.True(float.IsNaN(segment.PreviousY2));
		}

		[Fact]
		public void PieSegment_InitializesPieSpecificPropertiesCorrectly()
		{
			var segment = new PieSegment(); 
			Assert.Equal(0, segment.StartAngle);
			Assert.Equal(0, segment.EndAngle);
			Assert.Equal(0, segment.YValue);
			Assert.Equal(0, segment.Radius);
			Assert.False(segment.Exploded);
			Assert.True(double.IsNaN(segment.PreviousStartAngle));
			Assert.True(double.IsNaN(segment.PreviousEndAngle));
			Assert.Equal(0, segment.MidAngle);
			Assert.Equal(0, segment.SegmentStartAngle);
			Assert.Equal(0, segment.SegmentEndAngle);
			Assert.Equal(0, segment.SegmentMidAngle);
			Assert.Equal(0, segment.SegmentNewAngle);
		}

		[Fact]
		public void PieSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new PieSegment(); 
			Assert.Equal(default(Rect), segment.LabelRect);
			Assert.False(segment.IsLeft);
			Assert.Equal(default(RenderingPosition), segment.RenderingPosition);
			Assert.Equal(string.Empty, segment.TrimmedText);
			Assert.Equal(default(Position), segment.DataLabelRenderingPosition);
			Assert.Equal(0, segment._isLabelUpdated);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
		}

		[Fact]
		public void PieSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new PieSegment(); 
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void PieSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new PieSegment(); 
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView); 
			Assert.IsAssignableFrom<PieSegment>(segment);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		}

		[Fact]
		public void PolarAreaSegment_InitializesVisualPropertiesCorrectly()
		{
			var segment = new PolarAreaSegment(); 
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void PolarAreaSegment_InitializesDataPropertiesCorrectly()
		{
			var segment = new PolarAreaSegment(); 
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero); 
			Assert.Null(segment.Series);
		}

		[Fact]
		public void PolarAreaSegment_InitializesLabelPropertiesCorrectly()
		{
			var segment = new PolarAreaSegment(); 
			Assert.Null(segment.SeriesView);
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
		}

		[Fact]
		public void PolarLineSegment_InitializesBasicPropertiesCorrectly()
		{
			var segment = new PolarLineSegment(); 
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.Series);
			Assert.False(segment.Empty);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
		}

		[Fact]
		public void PolarLineSegment_InitializesAnimationAndIndexPropertiesCorrectly()
		{
			var segment = new PolarLineSegment(); 
			Assert.Null(segment.Series);
			Assert.Empty(segment.XPoints);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void PolarLineSegment_InitializesDataLabelPropertiesCorrectly()
		{
			var segment = new PolarLineSegment(); 
			Assert.Empty(segment.YPoints);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
		} 

		[Fact]
		public void PyramidSegment_InitializesBasicPropertiesCorrectly()
		{
			var segment = new PyramidSegment();  
			Assert.Null(segment.Chart);
			Assert.NotNull(segment.Points);
			Assert.Equal(4, segment.Points.Length);
			Assert.Null(segment.Series);
			Assert.False(segment.Empty);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
		}

		[Fact]
		public void PyramidSegment_InitializesAnimationAndIndexPropertiesCorrectly()
		{
			var segment = new PyramidSegment(); 
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.Series);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void PyramidSegment_InitializesDataLabelPropertiesCorrectly()
		{
			var segment = new PyramidSegment(); 
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
		}

		[Fact]
		public void RadialBarSegment_InitializesBasicPropertiesCorrectly()
		{
			var segment = new RadialBarSegment(); 
			Assert.Null(segment.Series);
			Assert.False(segment.Empty);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
		}

		[Fact]
		public void RadialBarSegment_InitializesAnimationAndIndexPropertiesCorrectly()
		{
			var segment = new RadialBarSegment(); 
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		} 

		[Fact]
		public void RadialBarSegment_InitializesPointsAndRangePropertiesCorrectly()
		{
			var segment = new RadialBarSegment();
			Assert.Equal(0f, segment.StartAngle);
			Assert.Equal(0f, segment.EndAngle); 
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.Series);
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		} 

		[Fact]
		public void RadialBarSegment_InitializesRadialBarSpecificPropertiesCorrectly()
		{
			var segment = new RadialBarSegment();
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Equal(0f, segment.InnerRingRadius);
			Assert.Equal(0f, segment.OuterRingRadius);
			Assert.Null(segment.TrackStroke);
			Assert.Equal(0f, segment.TrackStrokeWidth);
			Assert.Null(segment.TrackFill);
			Assert.False(segment.HasTrackStroke);
			Assert.Equal(0d, segment.YValue);
			Assert.Equal(0, segment.VisibleSegmentIndex);
		}
		[Fact]
		public void RangeAreaSegment_InitializesRangeSpecificPropertiesCorrectly()
		{
			var segment = new RangeAreaSegment(); 
			Assert.Null(segment.HighValues);
			Assert.Null(segment.LowValues);
			Assert.Null(segment.HighStrokePoints);
			Assert.Null(segment.LowStrokePoints);
			Assert.Equal(0, segment.LabelIndex);
			Assert.Null(segment.XValues);
			Assert.Null(segment.YValues); 
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.Null(segment.FillPoints);
		}

		[Fact]
		public void RangeAreaSegment_InitializesPointsPropertiesCorrectly()
		{
			var segment = new RangeAreaSegment(); 
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Null(segment.StrokePoints);
			Assert.Null(segment.PreviousFillPoints);
			Assert.Null(segment.PreviousStrokePoints);
			Assert.NotNull(segment.XPoints);
			Assert.Empty(segment.XPoints);
			Assert.NotNull(segment.YPoints);
			Assert.Empty(segment.YPoints);
		}

		[Fact]
		public void RangeAreaSegment_InitializesBasicPropertiesCorrectly()
		{
			var segment = new RangeAreaSegment(); 
			Assert.Null(segment.Item);
			Assert.False(segment.HasStroke);
			Assert.Null(segment.Series);
			Assert.False(segment.Empty);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Null(segment.StrokeDashArray);
			Assert.True(segment.IsVisible);
		}

		[Fact]
		public void RangeAreaSegment_InitializesAnimationAndIndexPropertiesCorrectly()
		{
			var segment = new RangeAreaSegment(); 
			Assert.False(segment.IsSelected); 
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
			Assert.Null(segment.SeriesView);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.IsAssignableFrom<IMarkerDependentSegment>(segment);
		} 

		[Fact]
		public void RangeColumnSegment_InitializesPositionPropertiesCorrectly()
		{
			var segment = new RangeColumnSegment(); 
			Assert.NotNull(segment._labelPositionPoints);
			Assert.Equal(2, segment._labelPositionPoints.Length);
			Assert.Equal(0f, segment.Left);
			Assert.Equal(0f, segment.Top);
			Assert.Equal(0f, segment.Right);
			Assert.Equal(0f, segment.Bottom);
			Assert.True(float.IsNaN(segment.Y1));
			Assert.True(float.IsNaN(segment.Y2));
			Assert.True(float.IsNaN(segment.PreviousY1));
			Assert.True(float.IsNaN(segment.PreviousY2));
		}

		[Fact]
		public void RangeColumnSegment_InitializesBasicPropertiesCorrectly()
		{
			var segment = new RangeColumnSegment(); 
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.Series);
			Assert.False(segment.Empty);
			Assert.Equal(1f, segment.Opacity);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray);
		}

		[Fact]
		public void RangeColumnSegment_InitializesAnimationAndIndexPropertiesCorrectly()
		{
			var segment = new RangeColumnSegment(); 
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected); 
			Assert.NotNull(segment._dataLabel);
		}
		 

		[Fact]
		public void RangeColumnSegment_InitializesPointsAndRangePropertiesCorrectly()
		{
			var segment = new RangeColumnSegment(); 
			Assert.Equal(2, segment._dataLabel.Length);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.Series);
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		}

		[Fact]
		public void ScatterSegment_InitializesScatterSpecificPropertiesCorrectly()
		{
			var segment = new ScatterSegment(); 
			Assert.Equal(1f, segment.Opacity);
			Assert.Equal(Charts.ShapeType.Custom, segment.Type);
			Assert.Equal(0f, segment.PointWidth);
			Assert.Equal(0f, segment.PointHeight);
			Assert.Equal(0f, segment.CenterX);
			Assert.Equal(0f, segment.CenterY);
			Assert.Equal(RectF.Zero, segment.PreviousSegmentBounds); 
			Assert.Null(segment.Series);
			Assert.False(segment.Empty);
		}

		[Fact]
		public void ScatterSegment_InitializesBasicPropertiesCorrectly()
		{
			var segment = new ScatterSegment(); 
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(segment.IsVisible);
			Assert.Null(segment.StrokeDashArray); 
			Assert.Null(segment.SeriesView); 
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.LabelContent);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Empty(segment.YPoints);
		}

		[Fact]
		public void ScatterSegment_InitializesAnimationAndIndexPropertiesCorrectly()
		{
			var segment = new ScatterSegment(); 
			Assert.Empty(segment.XPoints);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected); 
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
			Assert.Equal(0, segment.DataLabelXPosition);
		}

		[Fact]
		public void SplineAreaSegment_InitializesCollectionPropertiesCorrectly()
		{
			var segment = new SplineAreaSegment(); 
			Assert.NotNull(segment.XVal);
			Assert.Empty(segment.XVal);
			Assert.NotNull(segment.YVal);
			Assert.Empty(segment.YVal);
			Assert.NotNull(segment.ControlStartX);
			Assert.Empty(segment.ControlStartX);
			Assert.NotNull(segment.ControlStartY);
			Assert.Empty(segment.ControlStartY);
			Assert.NotNull(segment.ControlEndX);
			Assert.Empty(segment.ControlEndX);
			Assert.NotNull(segment.ControlEndY);
			Assert.Empty(segment.ControlEndY);
			Assert.NotNull(segment.StartControlPoints);
			Assert.Empty(segment.StartControlPoints);
		}

		[Fact]
		public void SplineAreaSegment_InitializesNullablePropertiesCorrectly()
		{
			var segment = new SplineAreaSegment(); 
			Assert.Null(segment.XValues);
			Assert.Null(segment.YValues);
			Assert.Null(segment.FillPoints);
			Assert.Null(segment.StrokePoints);
			Assert.Null(segment.PreviousFillPoints);
			Assert.Null(segment.PreviousStrokePoints);
			Assert.Null(segment.Series);
			Assert.Null(segment.LabelContent);
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
		}

		[Fact]
		public void SplineAreaSegment_InitializesBooleanPropertiesCorrectly()
		{
			var segment = new SplineAreaSegment(); 
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.True(segment.IsVisible);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void SplineAreaSegment_InitializesNumericPropertiesCorrectly()
		{
			var segment = new SplineAreaSegment(); 
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.IsAssignableFrom<IMarkerDependentSegment>(segment);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Equal(1f, segment.Opacity);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.AnimatedValue);
		}

		[Fact]
		public void SplineAreaSegment_InitializesOtherPropertiesCorrectly()
		{
			var segment = new SplineAreaSegment(); 
			Assert.NotNull(segment.EndControlPoints);
			Assert.Empty(segment.EndControlPoints);
			Assert.NotNull(segment.StrokeControlStartPoints);
			Assert.Empty(segment.StrokeControlStartPoints);
			Assert.NotNull(segment.StrokeControlEndPoints);
			Assert.Empty(segment.StrokeControlEndPoints);
			Assert.NotNull(segment.XPoints);
			Assert.Empty(segment.XPoints);
			Assert.NotNull(segment.YPoints);
			Assert.Empty(segment.YPoints);
			Assert.Empty(segment.DataLabels);
		}

		[Fact]
		public void SplineRangeAreaSegment_InitializesNullablePropertiesCorrectly()
		{
			var segment = new SplineRangeAreaSegment(); 
			Assert.Null(segment.XVal);
			Assert.Null(segment.HighVal);
			Assert.Null(segment.LowVal);
			Assert.Null(segment.XValues);
			Assert.Null(segment.YValues);
			Assert.Null(segment.FillPoints);
			Assert.Null(segment.StrokePoints);
			Assert.Null(segment.PreviousFillPoints);
			Assert.Null(segment.PreviousStrokePoints);
			Assert.Null(segment.Series);
		}

		[Fact]
		public void SplineRangeAreaSegment_InitializesBooleanPropertiesCorrectly()
		{
			var segment = new SplineRangeAreaSegment(); 
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.True(segment.IsVisible);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void SplineRangeAreaSegment_InitializesCollectionPropertiesCorrectly()
		{
			var segment = new SplineRangeAreaSegment(); 
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.NotNull(segment.XPoints);
			Assert.Empty(segment.XPoints);
			Assert.NotNull(segment.YPoints);
			Assert.Empty(segment.YPoints);
			Assert.Empty(segment.DataLabels);
		}

		[Fact]
		public void SplineRangeAreaSegment_InitializesNumericPropertiesCorrectly()
		{
			var segment = new SplineRangeAreaSegment(); 
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.IsAssignableFrom<IMarkerDependentSegment>(segment);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Equal(1f, segment.Opacity);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.AnimatedValue); 
			Assert.Null(segment.LabelContent);
		}

		[Fact]
		public void SplineSegment_InitializesFloatPropertiesCorrectly()
		{
			var segment = new SplineSegment(); 
			Assert.Equal(0f, segment.X1);
			Assert.Equal(0f, segment.Y1);
			Assert.Equal(0f, segment.X2);
			Assert.Equal(0f, segment.Y2);
			Assert.Equal(0f, segment.StartControlX);
			Assert.Equal(0f, segment.StartControlY);
			Assert.Equal(0f, segment.EndControlX);
			Assert.Equal(0f, segment.EndControlY);
			Assert.Equal(1f, segment.Opacity);
		}

		[Fact]
		public void SplineSegment_InitializesDoublePropertiesCorrectly()
		{
			var segment = new SplineSegment(); 
			Assert.Equal(0d, segment.StartPtX);
			Assert.Equal(0d, segment.StartPtY);
			Assert.Equal(0d, segment.StartControlPtX);
			Assert.Equal(0d, segment.StartControlPtY);
			Assert.Equal(0d, segment.EndControlPtX);
			Assert.Equal(0d, segment.EndControlPtY);
			Assert.Equal(0d, segment.EndPtX);
			Assert.Equal(0d, segment.EndPtY);
			Assert.Equal(1d, segment.StrokeWidth);
		}

		[Fact]
		public void SplineSegment_InitializesPreviousFloatPropertiesCorrectly()
		{
			var segment = new SplineSegment(); 
			Assert.False(segment.IsZero);
			Assert.True(float.IsNaN(segment.PreviousX1));
			Assert.True(float.IsNaN(segment.PreviousY1));
			Assert.True(float.IsNaN(segment.PreviousX2));
			Assert.True(float.IsNaN(segment.PreviousY2));
			Assert.True(float.IsNaN(segment.PreviousStartControlX));
			Assert.True(float.IsNaN(segment.PreviousStartControlY));
			Assert.True(float.IsNaN(segment.PreviousEndControlX));
			Assert.True(float.IsNaN(segment.PreviousEndControlY));
		} 
		[Fact]
		public void SplineSegment_InitializesNullablePropertiesCorrectly()
		{
			var segment = new SplineSegment(); 
			Assert.False(segment.IsSelected);
			Assert.False(segment.Empty);
			Assert.True(segment.InVisibleRange);
			Assert.Empty(segment.DataLabels);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.LabelContent);
			Assert.Null(segment.Item);
		} 

		[Fact]
		public void SplineSegment_InitializesNumericPropertiesCorrectly()
		{
			var segment = new SplineSegment(); 
			Assert.True(segment.IsVisible);
			Assert.False(segment.HasStroke);
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index); 
			Assert.IsAssignableFrom<IMarkerDependentSegment>(segment);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Equal(default(RectF), segment.SegmentBounds);
		}

		[Fact]
		public void StackingAreaSegment_InitializesNullablePropertiesCorrectly()
		{
			var segment = new StackingAreaSegment(); 
			Assert.Null(segment.BottomValues);
			Assert.Null(segment.TopValues);
			Assert.Null(segment.XValues);
			Assert.Null(segment.YValues);
			Assert.Null(segment.FillPoints);
			Assert.Null(segment.StrokePoints);
			Assert.Null(segment.PreviousFillPoints);
			Assert.Null(segment.PreviousStrokePoints);
			Assert.Null(segment.Series);
		}

		[Fact]
		public void StackingAreaSegment_InitializesBooleanPropertiesCorrectly()
		{
			var segment = new StackingAreaSegment(); 
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.True(segment.IsVisible);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void StackingAreaSegment_InitializesCollectionPropertiesCorrectly()
		{
			var segment = new StackingAreaSegment(); 
			Assert.Null(segment.LabelContent);
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.NotNull(segment.XPoints);
			Assert.Empty(segment.XPoints);
			Assert.NotNull(segment.YPoints);
			Assert.Empty(segment.YPoints);
			Assert.Empty(segment.DataLabels);
		}

		[Fact]
		public void StackingAreaSegment_InitializesNumericPropertiesCorrectly()
		{
			var segment = new StackingAreaSegment(); 
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Equal(1f, segment.Opacity);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.AnimatedValue); 
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.IsAssignableFrom<IMarkerDependentSegment>(segment);
		}

		[Fact]
		public void StepAreaSegment_InitializesNullablePropertiesCorrectly()
		{
			var segment = new StepAreaSegment(); 
			Assert.Null(segment.XValues);
			Assert.Null(segment.YValues);
			Assert.Null(segment.FillPoints);
			Assert.Null(segment.StrokePoints);
			Assert.Null(segment.PreviousFillPoints);
			Assert.Null(segment.PreviousStrokePoints);
			Assert.Null(segment.Series);
			Assert.Null(segment.LabelContent);
		}

		[Fact]
		public void StepAreaSegment_InitializesBooleanPropertiesCorrectly()
		{
			var segment = new StepAreaSegment(); 
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Item);
			Assert.False(segment.Empty);
			Assert.True(segment.IsVisible);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
		}

		[Fact]
		public void StepAreaSegment_InitializesCollectionPropertiesCorrectly()
		{
			var segment = new StepAreaSegment(); 
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.NotNull(segment.XPoints);
			Assert.Empty(segment.XPoints);
			Assert.NotNull(segment.YPoints);
			Assert.Empty(segment.YPoints);
			Assert.Empty(segment.DataLabels);
		}

		[Fact]
		public void StepAreaSegment_InitializesNumericPropertiesCorrectly()
		{
			var segment = new StepAreaSegment(); 
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Equal(1f, segment.Opacity);
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.AnimatedValue); 
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.IsAssignableFrom<IMarkerDependentSegment>(segment);
		}
		[Fact]
		public void StepLineSegment_InitializesFloatPropertiesCorrectly()
		{
			var segment = new StepLineSegment(); 
			Assert.Equal(0f, segment.Y2);
			Assert.Equal(0f, segment.X2);
			Assert.Equal(1f, segment.Opacity);
			Assert.Equal(0f, segment.X1);
			Assert.Equal(0f, segment.Y1); 
			Assert.Equal(0d, segment.X1Pos);
			Assert.Equal(0d, segment.Y1Pos);
			Assert.Equal(0d, segment.X2Pos);
			Assert.Equal(0d, segment.Y2Pos);
		} 

		[Fact]
		public void StepLineSegment_InitializesNullablePropertiesCorrectly()
		{
			var segment = new StepLineSegment(); 
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.True(float.IsNaN(segment.PreviousX1));
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Item);
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.LabelContent);
		}

		[Fact]
		public void StepLineSegment_InitializesBooleanPropertiesCorrectly()
		{
			var segment = new StepLineSegment(); 
			Assert.True(float.IsNaN(segment.PreviousY1));
			Assert.True(float.IsNaN(segment.PreviousX2));
			Assert.True(float.IsNaN(segment.PreviousY2));
			Assert.Equal(0, segment.AnimatedValue);
			Assert.True(segment.IsVisible);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
			Assert.False(segment.Empty);
			Assert.True(segment.InVisibleRange);
		}
		 
		[Fact]
		public void StepLineSegment_InitializesNumericPropertiesCorrectly()
		{
			var segment = new StepLineSegment(); 
			Assert.False(segment.IsZero);
			Assert.Equal(0, segment.Index);
			Assert.Equal(0, segment.DataLabelXPosition);
			Assert.Equal(0, segment.DataLabelYPosition); 
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Empty(segment.DataLabels);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
		}

		[Fact]
		public void WaterfallSegment_InitializesNullablePropertiesCorrectly()
		{
			var segment = new WaterfallSegment(); 
			Assert.Null(segment._previousWaterfallSegment);
			Assert.Null(segment.Fill);
			Assert.Null(segment.Stroke);
			Assert.Null(segment.StrokeDashArray);
			Assert.Null(segment.Series);
			Assert.Null(segment.SeriesView);
			Assert.Null(segment.LabelContent);
			Assert.Null(segment.Item); 
			Assert.Equal(0, segment.Bottom);
			Assert.Equal(0, segment.DataLabelXPosition);
		}

		[Fact]
		public void WaterfallSegment_InitializesNumericPropertiesCorrectly()
		{
			var segment = new WaterfallSegment(); 
			Assert.Equal(0, segment._x1);
			Assert.Equal(0, segment._y1);
			Assert.Equal(0, segment._x2);
			Assert.Equal(0, segment._y2);
			Assert.Equal(0, segment._xValue);
			Assert.Equal(0, segment.WaterfallSum);
			Assert.Equal(0, segment.Sum);
			Assert.Equal(0, segment.Left);
			Assert.Equal(0, segment.Top);
			Assert.Equal(0, segment.Right);
		}

		[Fact]
		public void WaterfallSegment_InitializesEnumPropertiesCorrectly()
		{
			var segment = new WaterfallSegment(); 
			Assert.Equal(0, segment.DataLabelYPosition);
			Assert.Equal(0, segment.AnimatedValue);
			Assert.Equal(0, segment.Index);
			Assert.Equal(1f, segment.Opacity);
			Assert.Empty(segment.XPoints);
			Assert.Empty(segment.YPoints);
			Assert.Empty(segment.DataLabels);
			Assert.Equal(WaterfallSegmentType.Positive, segment.SegmentType);
		}
		 
		[Fact]
		public void WaterfallSegment_InitializesBooleanPropertiesCorrectly()
		{
			var segment = new WaterfallSegment(); 
			Assert.Equal(1d, segment.StrokeWidth);
			Assert.Equal(default(PointF), segment.LabelPositionPoint);
			Assert.Equal(default(RectF), segment.SegmentBounds);
			Assert.True(segment.IsVisible);
			Assert.False(segment.HasStroke);
			Assert.False(segment.IsSelected);
			Assert.False(segment.Empty);
			Assert.True(segment.InVisibleRange);
			Assert.False(segment.IsZero);
		} 
	}
}
