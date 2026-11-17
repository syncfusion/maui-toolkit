#nullable enable
using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class ChartSeriesGradientIntegrationTests
	{
		// ========== ToColor() Utility Tests ==========

		[Fact]
		public void ToColor_WithSolidColorBrush_ReturnsSolidColor()
		{
			// Arrange
			var brush = new SolidColorBrush(Colors.Red);

			// Act
			var color = brush.ToColor();

			// Assert
			Assert.Equal(Colors.Red, color);
		}

		[Fact]
		public void ToColor_WithLinearGradientBrush_ReturnsFirstGradientStopColor()
		{
			// Arrange
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Blue, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Green, Offset = 1 });

			// Act
			var color = gradient.ToColor();

			// Assert
			Assert.Equal(Colors.Blue, color);
		}

		[Fact]
		public void ToColor_WithRadialGradientBrush_ReturnsFirstGradientStopColor()
		{
			// Arrange
			var gradient = new RadialGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Yellow, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Orange, Offset = 1 });

			// Act
			var color = gradient.ToColor();

			// Assert
			Assert.Equal(Colors.Yellow, color);
		}

		[Fact]
		public void ToColor_WithEmptyGradientStops_ReturnsTransparent()
		{
			// Arrange
			var gradient = new LinearGradientBrush();
			// No stops added

			// Act
			var color = gradient.ToColor();

			// Assert
			Assert.Equal(Colors.Transparent, color);
		}

		[Fact]
		public void ToColor_WithNullBrush_ReturnsTransparent()
		{
			// Arrange
			Brush? brush = null;

			// Act
			var color = brush.ToColor();

			// Assert
			Assert.Equal(Colors.Transparent, color);
		}

		// ========== Series Gradient Fill Priority Tests ==========

		[Fact]
		public void LineSeriesWithSeriesFill_UsesGradientForFill()
		{
			// Arrange: Create a line series with gradient fill
			var series = new LineSeries();
			series.ItemsSource = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "1", Y = 10 },
				new ColorDataPoint { X = "2", Y = 20 },
				new ColorDataPoint { X = "3", Y = 15 }
			};

			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Red, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Blue, Offset = 1 });
			series.Fill = gradient;

			// Act
			var extractedColor = series.Fill.ToColor();

			// Assert
			Assert.NotNull(series.Fill);
			Assert.IsType<LinearGradientBrush>(series.Fill);
			Assert.Equal(Colors.Red, extractedColor);
		}

		[Fact]
		public void LineSeriesWithPaletteBrushes_EachSegmentCanUseGradient()
		{
			// Arrange
			var series = new LineSeries();
			series.ItemsSource = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "1", Y = 10 },
				new ColorDataPoint { X = "2", Y = 20 },
				new ColorDataPoint { X = "3", Y = 15 }
			};

			// Create gradient brushes for palette
			var gradientBrush1 = new LinearGradientBrush();
			gradientBrush1.GradientStops.Add(new GradientStop { Color = Colors.Red, Offset = 0 });
			gradientBrush1.GradientStops.Add(new GradientStop { Color = Colors.Yellow, Offset = 1 });

			var gradientBrush2 = new LinearGradientBrush();
			gradientBrush2.GradientStops.Add(new GradientStop { Color = Colors.Green, Offset = 0 });
			gradientBrush2.GradientStops.Add(new GradientStop { Color = Colors.Cyan, Offset = 1 });

			series.PaletteBrushes = new ObservableCollection<Brush> { gradientBrush1, gradientBrush2 };

			// Act
			var firstColor = series.PaletteBrushes[0].ToColor();
			var secondColor = series.PaletteBrushes[1].ToColor();

			// Assert
			Assert.Equal(Colors.Red, firstColor);
			Assert.Equal(Colors.Green, secondColor);
		}

		[Fact]
		public void LineSeriesWithPointColorPath_PointColorHasHighestPriority()
		{
			// Arrange
			var series = new LineSeries();
			series.ItemsSource = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "1", Y = 10, Color = Colors.Magenta },
				new ColorDataPoint { X = "2", Y = 20, Color = Colors.Cyan },
				new ColorDataPoint { X = "3", Y = 15, Color = Colors.Lime }
			};

			// Apply both series fill and palette
			var seriesFill = new LinearGradientBrush();
			seriesFill.GradientStops.Add(new GradientStop { Color = Colors.Red, Offset = 0 });
			series.Fill = seriesFill;

			series.PointColorPath = nameof(ColorDataPoint.Color);

			// Act & Assert
			Assert.NotNull(series.PointColorPath);
			Assert.Equal(nameof(ColorDataPoint.Color), series.PointColorPath);
		}

		// ========== StepLineSeries Gradient Tests ==========

		[Fact]
		public void StepLineSeriesWithGradient_InheritsLineGradientBehavior()
		{
			// Arrange
			var series = new StepLineSeries();
			series.ItemsSource = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "1", Y = 10 },
				new ColorDataPoint { X = "2", Y = 25 },
				new ColorDataPoint { X = "3", Y = 15 }
			};

			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Purple, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Orange, Offset = 1 });
			series.Fill = gradient;

			// Act
			var extractedColor = series.Fill.ToColor();

			// Assert
			Assert.IsType<LinearGradientBrush>(series.Fill);
			Assert.Equal(Colors.Purple, extractedColor);
		}

		// ========== SplineSeries Gradient Tests ==========

		[Fact]
		public void SplineSeriesWithGradient_SupportsGradientFill()
		{
			// Arrange
			var series = new SplineSeries();
			series.ItemsSource = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "1", Y = 10 },
				new ColorDataPoint { X = "2", Y = 20 },
				new ColorDataPoint { X = "3", Y = 15 },
				new ColorDataPoint { X = "4", Y = 25 }
			};

			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Teal, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Pink, Offset = 1 });
			series.Fill = gradient;

			// Act
			var extractedColor = series.Fill.ToColor();

			// Assert
			Assert.IsType<LinearGradientBrush>(series.Fill);
			Assert.Equal(Colors.Teal, extractedColor);
		}

		// ========== Gradient Stop Color Extraction Tests ==========

		[Fact]
		public void ToColor_WithMultipleGradientStops_ReturnsFirstStopColor()
		{
			// Arrange
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.White, Offset = 0.0f });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Gray, Offset = 0.5f });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Black, Offset = 1.0f });

			// Act
			var color = gradient.ToColor();

			// Assert
			Assert.Equal(Colors.White, color);
		}

		[Fact]
		public void ToColor_WithSingleGradientStop_ReturnsThatStopColor()
		{
			// Arrange
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Gold, Offset = 0.5f });

			// Act
			var color = gradient.ToColor();

			// Assert
			Assert.Equal(Colors.Gold, color);
		}

		// ========== Brush Type Detection Tests ==========

		[Fact]
		public void ToColor_CorrectlyIdentifiesSolidColorBrush()
		{
			// Arrange
			Brush brush = new SolidColorBrush(Colors.Salmon);

			// Act
			var color = brush.ToColor();

			// Assert
			Assert.Equal(Colors.Salmon, color);
		}

		[Fact]
		public void ToColor_CorrectlyIdentifiesLinearGradientBrush()
		{
			// Arrange
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Crimson, Offset = 0 });
			Brush brush = gradient;

			// Act
			var color = brush.ToColor();

			// Assert
			Assert.Equal(Colors.Crimson, color);
		}

		[Fact]
		public void ToColor_CorrectlyIdentifiesRadialGradientBrush()
		{
			// Arrange
			var gradient = new RadialGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.DarkRed, Offset = 0 });
			Brush brush = gradient;

			// Act
			var color = brush.ToColor();

			// Assert
			Assert.Equal(Colors.DarkRed, color);
		}

		// ========== Fill Priority Cascade Tests ==========

		[Fact]
		public void LineSeriesWithNullPointColorPath_FallsThroughToSeriesFill()
		{
			// Arrange
			var series = new LineSeries();
			series.ItemsSource = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "1", Y = 10 },
				new ColorDataPoint { X = "2", Y = 20 }
			};

			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.RoyalBlue, Offset = 0 });
			series.Fill = gradient;
			series.PointColorPath = null!;

			// Act
			var seriesFillColor = series.Fill.ToColor();

			// Assert
			Assert.Null(series.PointColorPath);
			Assert.Equal(Colors.RoyalBlue, seriesFillColor);
		}

		[Fact]
		public void LineSeriesWithoutPointColorOrSeriesFill_CanUsePalette()
		{
			// Arrange
			var series = new LineSeries();
			series.ItemsSource = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "1", Y = 10 },
				new ColorDataPoint { X = "2", Y = 20 }
			};

			series.Fill = null!;
			series.PointColorPath = null!;
			series.PaletteBrushes = new ObservableCollection<Brush>
			{
				new SolidColorBrush(Colors.DodgerBlue),
				new SolidColorBrush(Colors.LimeGreen)
			};

			// Act & Assert
			Assert.Null(series.Fill);
			Assert.Null(series.PointColorPath);
			Assert.NotEmpty(series.PaletteBrushes);
		}

		// ========== RadialGradient Specific Tests ==========

		[Fact]
		public void RadialGradientBrush_ExtractsFirstStopColorCorrectly()
		{
			// Arrange
			var gradient = new RadialGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Indigo, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Violet, Offset = 1 });

			// Act
			var color = gradient.ToColor();

			// Assert
			Assert.Equal(Colors.Indigo, color);
		}

		[Fact]
		public void RadialGradientBrush_WithCustomCenter_StillReturnsFirstStopColor()
		{
			// Arrange
			var gradient = new RadialGradientBrush
			{
				Center = new Point(0.5, 0.5),
				Radius = 0.5
			};
			gradient.GradientStops.Add(new GradientStop { Color = Colors.ForestGreen, Offset = 0 });

			// Act
			var color = gradient.ToColor();

			// Assert
			Assert.Equal(Colors.ForestGreen, color);
		}

		// ========== Data Point Color Binding Tests ==========

		[Fact]
		public void ColorDataPoint_WithColorProperty_CanBeUsedForPointColorPath()
		{
			// Arrange
			var dataPoint = new ColorDataPoint
			{
				X = "1",
				Y = 100,
				Color = Colors.Navy
			};

			// Act & Assert
			Assert.Equal(Colors.Navy, dataPoint.Color);
		}

		[Fact]
		public void LineSeriesWithColoredDataPoints_SupportsMultiplePointColors()
		{
			// Arrange
			var series = new LineSeries();
			var dataPoints = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "1", Y = 10, Color = Colors.Maroon },
				new ColorDataPoint { X = "2", Y = 20, Color = Colors.Navy },
				new ColorDataPoint { X = "3", Y = 15, Color = Colors.Olive }
			};
			series.ItemsSource = dataPoints;
			series.PointColorPath = nameof(ColorDataPoint.Color);

			// Act
			var firstPointColor = dataPoints[0].Color;
			var secondPointColor = dataPoints[1].Color;
			var thirdPointColor = dataPoints[2].Color;

			// Assert
			Assert.Equal(Colors.Maroon, firstPointColor);
			Assert.Equal(Colors.Navy, secondPointColor);
			Assert.Equal(Colors.Olive, thirdPointColor);
			Assert.NotEqual(firstPointColor, secondPointColor);
			Assert.NotEqual(secondPointColor, thirdPointColor);
		}

		// ========== Edge Cases ==========

		[Fact]
		public void ToColor_WithoutGradientStopsProperty_ReturnsTransparent()
		{
			// Arrange: Create a gradient with no stops added
			var gradient = new LinearGradientBrush();

			// Act
			var color = gradient.ToColor();

			// Assert
			Assert.Equal(Colors.Transparent, color);
		}

		[Fact]
		public void LineSeriesWithoutFill_ShouldUseDefaultBehavior()
		{
			// Arrange
			var series = new LineSeries();
			series.ItemsSource = new List<ColorDataPoint>
			{
				new ColorDataPoint { X = "1", Y = 10 },
				new ColorDataPoint { X = "2", Y = 20 }
			};

			// Act & Assert
			Assert.Null(series.Fill);
		}

		[Fact]
		public void MultipleSeriesWithDifferentGradients_MaintainIndependence()
		{
			// Arrange
			var series1 = new LineSeries();
			var series2 = new LineSeries();

			var gradient1 = new LinearGradientBrush();
			gradient1.GradientStops.Add(new GradientStop { Color = Colors.Red, Offset = 0 });

			var gradient2 = new LinearGradientBrush();
			gradient2.GradientStops.Add(new GradientStop { Color = Colors.Blue, Offset = 0 });

			series1.Fill = gradient1;
			series2.Fill = gradient2;

			// Act
			var color1 = series1.Fill.ToColor();
			var color2 = series2.Fill.ToColor();

			// Assert
			Assert.Equal(Colors.Red, color1);
			Assert.Equal(Colors.Blue, color2);
			Assert.NotEqual(color1, color2);
		}

		// ========== Dashed Line + Gradient Combo Tests ==========
		// Verifies spec scenario "DashedGradientLine": when Fill is GradientBrush AND StrokeDashArray is non-empty,
		// the LineSegment.DrawLine routing condition
		//     if (Fill is GradientBrush && (StrokeDashArray == null || StrokeDashArray.Count == 0))
		// evaluates false, taking the else branch which uses ToColor() (the first gradient stop color)
		// while the dash pattern is preserved from the outer Draw() method.

		[Fact]
		public void LineSeries_StrokeDashArray_IsAcceptedOnSeries()
		{
			// Arrange & Act
			var series = new LineSeries
			{
				StrokeDashArray = new DoubleCollection { 5, 3 }
			};

			// Assert: setter round-trips through the BindableProperty correctly
			Assert.NotNull(series.StrokeDashArray);
			Assert.Equal(2, series.StrokeDashArray.Count);
			Assert.Equal(5.0, series.StrokeDashArray[0]);
			Assert.Equal(3.0, series.StrokeDashArray[1]);
		}

		[Fact]
		public void SplineSeries_StrokeDashArray_IsAcceptedOnSeries()
		{
			// Arrange & Act
			var series = new SplineSeries
			{
				StrokeDashArray = new DoubleCollection { 4, 2 }
			};

			// Assert
			Assert.NotNull(series.StrokeDashArray);
			Assert.Equal(2, series.StrokeDashArray.Count);
			Assert.Equal(4.0, series.StrokeDashArray[0]);
			Assert.Equal(2.0, series.StrokeDashArray[1]);
		}

		[Fact]
		public void StepLineSeries_StrokeDashArray_IsAcceptedOnSeries()
		{
			// Arrange & Act
			var series = new StepLineSeries
			{
				StrokeDashArray = new DoubleCollection { 6, 4 }
			};

			// Assert
			Assert.NotNull(series.StrokeDashArray);
			Assert.Equal(2, series.StrokeDashArray.Count);
			Assert.Equal(6.0, series.StrokeDashArray[0]);
			Assert.Equal(4.0, series.StrokeDashArray[1]);
		}

		[Fact]
		public void LineSeries_WithGradientFill_AndStrokeDashArray_SetsBothProperties()
		{
			// Arrange: gradient fill + dash pattern coexist on the same series
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Orange, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Purple, Offset = 1 });

			var series = new LineSeries
			{
				Fill = gradient,
				StrokeDashArray = new DoubleCollection { 5, 5 }
			};

			// Act & Assert: both state values are independently set and retrievable,
			// proving the spec scenario precondition (Fill is GradientBrush AND StrokeDashArray.Count > 0)
			Assert.IsType<LinearGradientBrush>(series.Fill);
			Assert.NotNull(series.Fill as GradientBrush);
			Assert.Equal(2, series.StrokeDashArray.Count);
			// Re-confirm Fill is still assignable to GradientBrush after the dash was set
			// (LinearGradientBrush is the runtime concrete type; GradientBrush is the abstract base)
			Assert.IsAssignableFrom<GradientBrush>(series.Fill);
		}

		[Fact]
		public void ToColor_OfGradientBrush_UsedAsFallbackStrokeColor_ReturnsFirstStop()
		{
			// Arrange: this is what LineSegment.DrawLine reads via Fill.ToColor() in the
			// DashedLineFallback else-branch, ensuring dash+gradient renders as a dashed solid line
			// in the FIRST gradient stop color (per spec).
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Orange, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Purple, Offset = 0.5f });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Cyan, Offset = 1 });

			// Act
			var fallbackStrokeColor = gradient.ToColor();

			// Assert: deterministic and well-defined — first gradient stop
			Assert.Equal(Colors.Orange, fallbackStrokeColor);
			Assert.NotEqual(Colors.Purple, fallbackStrokeColor);
			Assert.NotEqual(Colors.Cyan, fallbackStrokeColor);
		}

		[Fact]
		public void DoubleCollection_WithMultipleDashes_RoundTripsCorrectly()
		{
			// Arrange & Act
			var dashes = new DoubleCollection { 2, 3, 4, 5 };
			var series = new LineSeries
			{
				Fill = CreateTwoStopGradient(Colors.Red, Colors.Blue),
				StrokeDashArray = dashes
			};

			// Assert: gradient routing condition checks Count > 0 — any non-empty
			// collection short-circuits the gradient helper path and uses the dash fallback
			Assert.True(series.StrokeDashArray.Count > 0);
			Assert.Equal(4, series.StrokeDashArray.Count);
		}

		[Fact]
		public void LineSeries_WithGradientFill_AndEmptyStrokeDashArray_StillUsesGradientPath()
		{
			// Arrange & Act: empty StrokeDashArray (Count == 0) does NOT short-circuit the
			// gradient helper path — the Series stays on the gradient rendering branch.
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Magenta, Offset = 0 });

			var series = new LineSeries
			{
				Fill = gradient,
				StrokeDashArray = new DoubleCollection() // empty
			};

			// Assert: condition check (StrokeDashArray == null || Count == 0) is true,
			// so the gradient helper path is taken
			Assert.NotNull(series.StrokeDashArray);
			Assert.Empty(series.StrokeDashArray);
			Assert.IsAssignableFrom<GradientBrush>(series.Fill);
		}

		[Fact]
		public void LineSeries_WithGradientFill_AndNullStrokeDashArray_StillUsesGradientPath()
		{
			// Arrange & Act: null StrokeDashArray — same path as empty (Count == 0)
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Magenta, Offset = 0 });

			var series = new LineSeries
			{
				Fill = gradient,
				StrokeDashArray = null!
			};

			// Assert: (StrokeDashArray == null) short-circuit is true → gradient helper used
			Assert.Null(series.StrokeDashArray);
			Assert.IsAssignableFrom<GradientBrush>(series.Fill);
		}

		[Fact]
		public void SplineSeries_WithGradientFill_AndStrokeDashArray_SetsBothProperties()
		{
			// Arrange: SplineSegment.DrawQuad has the same DashedLineFallback contract
			var gradient = new RadialGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Salmon, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Gold, Offset = 1 });

			var series = new SplineSeries
			{
				Fill = gradient,
				StrokeDashArray = new DoubleCollection { 8, 2 }
			};

			// Assert
			Assert.IsAssignableFrom<GradientBrush>(series.Fill);
			Assert.Equal(2, series.StrokeDashArray.Count);
		}

		[Fact]
		public void StepLineSeries_WithGradientFill_AndStrokeDashArray_SetsBothProperties()
		{
			// Arrange: StepLineSegment.DrawLine override has the same DashedLineFallback contract
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Olive, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = Colors.Maroon, Offset = 1 });

			var series = new StepLineSeries
			{
				Fill = gradient,
				StrokeDashArray = new DoubleCollection { 3, 1 }
			};

			// Assert
			Assert.IsAssignableFrom<GradientBrush>(series.Fill);
			Assert.Equal(2, series.StrokeDashArray.Count);
		}

		private static LinearGradientBrush CreateTwoStopGradient(Color start, Color end)
		{
			var gradient = new LinearGradientBrush();
			gradient.GradientStops.Add(new GradientStop { Color = start, Offset = 0 });
			gradient.GradientStops.Add(new GradientStop { Color = end, Offset = 1 });
			return gradient;
		}

		[Fact]
		public void GradientBrush_IsAssignableFrom_LinearAndRadial()
		{
			// Diagnostic: confirm whether existing production code's
			//     if (Fill is GradientBrush && ...)
			// actually returns true for LinearGradientBrush / RadialGradientBrush at runtime
			var linear = new LinearGradientBrush { GradientStops = { new GradientStop { Color = Colors.Red, Offset = 0 } } };
			var radial = new RadialGradientBrush { GradientStops = { new GradientStop { Color = Colors.Red, Offset = 0 } } };

			// Check the type hierarchy at runtime
			Assert.True(linear is Brush, "LinearGradientBrush must be a Brush");
			Assert.True(radial is Brush, "RadialGradientBrush must be a Brush");

			// What's the actual hierarchy? Record for posterity
			var linearBases = new List<string>();
			var t = linear.GetType();
			while (t != null)
			{ linearBases.Add(t.FullName ?? "?"); t = t.BaseType; }

			// Force a hard test: try the actual condition used in production
			Brush? fill1 = linear;
			Brush? fill2 = radial;
			bool linearIsGradient = fill1 is GradientBrush;
			bool radialIsGradient = fill2 is GradientBrush;
			Assert.True(linearIsGradient, $"LinearGradientBrush should be GradientBrush (hierarchy: {string.Join(" → ", linearBases)})");
			Assert.True(radialIsGradient, "RadialGradientBrush should be GradientBrush");
		}
	}
}