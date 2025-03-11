using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class EmptyPointSettingUnitTest
	{
		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsEmpty_SetValue_ReturnsExpectedValue(bool expectedIsEmpty)
		{
			ColumnSegment columnSegment = new ColumnSegment
			{
				IsZero = expectedIsEmpty
			};
			Assert.Equal(expectedIsEmpty, columnSegment.IsZero);
		}

		[Fact]
		public void ReValidateYValues_SetsValuesToNaN_Correctly()
		{
			var columnSeries = new ColumnSeries();
			columnSeries.YValues = new List<double> { 1, 2, 3, 4, 5 };
			IList<int>[] emptyPointIndex = new IList<int>[]
											{
											  new List<int> { 1, 3 },
											  new List<int>()
											};
			columnSeries.EmptyPointIndexes = emptyPointIndex;
			columnSeries.ResetEmptyPointIndexes();
			Assert.Equal(new List<double> { 1.0, double.NaN, 3.0, double.NaN, 5.0 }, columnSeries.YValues);
		}

		[Fact]
		public void EmptyPointIndexes_ShouldBeInitializedByDefault()
		{
			var series = new ColumnSeries();
			var emptyPointIndexes = series.EmptyPointIndexes;
			Assert.NotNull(emptyPointIndexes);
		}

		[Fact]
		public void EmptyPointIndexes_CanBeSetAndRetrieved()
		{
			var series = new ColumnSeries();
			var indexes = new List<int>[]
			{
			new List<int> { 2, 4 },
			new List<int> {3 },
			new List<int>()
			};

			series.EmptyPointIndexes = indexes;
			var retrievedIndexes = series.EmptyPointIndexes;
			Assert.NotNull(retrievedIndexes);
			Assert.Equal(3, retrievedIndexes.Length);
			Assert.Equal(new List<int> { 2, 4 }, retrievedIndexes[0]);
			Assert.Equal(new List<int> { 3 }, retrievedIndexes[1]);
			Assert.Empty(retrievedIndexes[2]);
		}

		[Fact]
		public void EmptyPointSettings_DefaultValue_ShouldBeInitialized()
		{
			var series = new ColumnSeries();
			var settings = series.EmptyPointSettings;
			var point = series._emptyPointSettings;
			Assert.Null(settings);
			Assert.NotNull(point);
		}

		[Fact]
		public void EmptyPointMode_DefaultValue_ShouldBeNone()
		{
			var series = new ColumnSeries();
			var mode = series.EmptyPointMode;
			Assert.Equal(EmptyPointMode.None, mode);
		}

		[Theory]
		[InlineData(EmptyPointMode.Zero)]
		[InlineData(EmptyPointMode.Average)]
		[InlineData(EmptyPointMode.None)]
		public void EmptyPointMode_SetAndRetrieveValue(EmptyPointMode emptyPointMode)
		{
			var series = new ColumnSeries();
			series.EmptyPointMode = emptyPointMode;
			var mode = series.EmptyPointMode;
			Assert.Equal(emptyPointMode, mode);
		}

		[Fact]
		public void EmptyPointSettings_CanBeSetAndRetrieved()
		{
			var series = new ColumnSeries();
			var emptyPointSettings = new EmptyPointSettings
			{
				Fill = new SolidColorBrush(Colors.Red),
				Stroke = new SolidColorBrush(Colors.Blue),
				StrokeWidth = 2
			};

			series.EmptyPointSettings = emptyPointSettings;
			var settings = series.EmptyPointSettings;
			Assert.NotNull(settings);
			Assert.Equal(emptyPointSettings, settings);
			Assert.Equal(Colors.Red, ((SolidColorBrush)settings.Fill).Color);
			Assert.Equal(Colors.Blue, ((SolidColorBrush)settings.Stroke).Color);
			Assert.Equal(2, settings.StrokeWidth);
		}

		[Fact]
		public void ValidateYPoints_With10Points_EmptyPointMode_Average_ShouldReplaceNaNWithAverage()
		{
			var series = new ColumnSeries() { YValues = new List<double> { 1.0, double.NaN, 2.0, 3.0, double.NaN, 5.5, 6.0, 7.0, double.NaN, 10.0 } };
			series.EmptyPointMode = EmptyPointMode.Average;
			series.ValidateYValues();
			Assert.Equal(new List<double> { 1.0, 1.5, 2.0, 3.0, 4.25, 5.5, 6.0, 7.0, 8.5, 10.0 }, series.YValues);
		}

		[Fact]
		public void ValidateYPoints_With10Points_EmptyPointMode_Zero_ShouldReplaceNaNWithZero()
		{
			var series = new ColumnSeries
			{
				YValues = new List<double> { 1.0, double.NaN, 2.0, 3.0, double.NaN, 5.5, 6.0, 7.0, double.NaN, 10.0 },
				EmptyPointMode = EmptyPointMode.Zero
			};
			series.ValidateYValues();
			Assert.Equal(new List<double> { 1.0, 0, 2.0, 3.0, 0, 5.5, 6.0, 7.0, 0, 10.0 }, series.YValues);
		}

		[Fact]
		public void ValidateDataPoints_With10Points_EmptyPointMode_None_ShouldRemainUnchanged()
		{
			var series = new ColumnSeries();
			series.EmptyPointMode = EmptyPointMode.None;
			var yValues = new List<double> { 1.0, double.NaN, 2.0, 3.0, double.NaN, 5.5, 6.0, 7.0, double.NaN, 10.0 };
			series.ValidateDataPoints(yValues);
			Assert.Equal(new List<double> { 1.0, double.NaN, 2.0, 3.0, double.NaN, 5.5, 6.0, 7.0, double.NaN, 10.0 }, yValues);
		}

		[Fact]
		public void ValidateDataPoints_With10Points_EmptyPointMode_Zero_ShouldReplaceNaNWithZero()
		{
			var series = new ColumnSeries();
			series.EmptyPointMode = EmptyPointMode.Zero;
			var yValues = new List<double> { 1.0, double.NaN, 2.0, 3.0, double.NaN, 5.5, 6.0, 7.0, double.NaN, 10.0 };
			series.ValidateDataPoints(yValues);
			Assert.Equal(new List<double> { 1.0, 0.0, 2.0, 3.0, 0.0, 5.5, 6.0, 7.0, 0.0, 10.0 }, yValues);
		}

		[Fact]
		public void ValidateDataPoints_With10Points_EmptyPointMode_Average_ShouldReplaceNaNWithAverage()
		{
			var series = new ColumnSeries();
			series.EmptyPointMode = EmptyPointMode.Average;
			var yValues = new List<double> { 1.0, double.NaN, 2.0, 3.0, double.NaN, 5.5, 6.0, 7.0, double.NaN, 10.0 };
			series.ValidateDataPoints(yValues);
			Assert.Equal(new List<double> { 1.0, 1.5, 2.0, 3.0, 4.25, 5.5, 6.0, 7.0, 8.5, 10.0 }, yValues);
		}

		[Fact]
		public void FillProperty_DefaultValue_ShouldBeDefaultBrush()
		{
			var emptyPointSettings = new EmptyPointSettings();
			var defaultBrush = new SolidColorBrush(Color.FromArgb("FF4E4E"));
			var defaultFillValue = emptyPointSettings.Fill;
			Assert.Equal(defaultBrush.Color, ((SolidColorBrush)defaultFillValue).Color);
		}

		[Fact]
		public void FillProperty_SetValue_ShouldReturnSetValue()
		{
			var emptyPointSettings = new EmptyPointSettings();
			var expectedBrush = new SolidColorBrush(Colors.Red);
			emptyPointSettings.Fill = expectedBrush;
			var fillValue = emptyPointSettings.Fill;
			Assert.Equal(expectedBrush, fillValue);
		}

		[Fact]
		public void StrokeProperty_DefaultValue_ShouldBeTransparent()
		{
			var emptyPointSettings = new EmptyPointSettings();
			var defaultStroke = new SolidColorBrush(Colors.Transparent);
			var defaultStrokeValue = emptyPointSettings.Stroke;
			Assert.Equal(defaultStroke.Color, ((SolidColorBrush)defaultStrokeValue).Color);
		}

		[Fact]
		public void StrokeProperty_SetValue_ShouldReturnSetValue()
		{
			var emptyPointSettings = new EmptyPointSettings();
			var expectedStrokeBrush = new SolidColorBrush(Colors.Blue);
			emptyPointSettings.Stroke = expectedStrokeBrush;
			var strokeValue = emptyPointSettings.Stroke;
			Assert.Equal(expectedStrokeBrush.Color, ((SolidColorBrush)strokeValue).Color);
		}

		[Fact]
		public void StrokeWidthProperty_DefaultValue_ShouldBeOne()
		{
			var emptyPointSettings = new EmptyPointSettings();
			var defaultStrokeWidth = emptyPointSettings.StrokeWidth;
			Assert.Equal(1.0, defaultStrokeWidth);
		}

		[Fact]
		public void StrokeWidthProperty_SetValue_ShouldReturnSetValue()
		{
			var emptyPointSettings = new EmptyPointSettings();
			var expectedStrokeWidth = 2.5;
			emptyPointSettings.StrokeWidth = expectedStrokeWidth;
			var strokeWidth = emptyPointSettings.StrokeWidth;
			Assert.Equal(expectedStrokeWidth, strokeWidth);
		}
	}
}
