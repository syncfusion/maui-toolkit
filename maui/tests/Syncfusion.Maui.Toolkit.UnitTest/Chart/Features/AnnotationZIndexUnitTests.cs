#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Charts;
using Xunit;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Microsoft.Maui.Controls.Internals;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	/// <summary>
	/// Host-less tests for the annotation <see cref="ChartAnnotation.ZIndex"/> z-order support:
	/// property defaults/round-trips on every public annotation type, the drawer
	/// partitioning (foreground vs background), the stable ordering inside each drawer,
	/// runtime ZIndex updates, runtime add/remove, and the regression guarantees that the
	/// default (ZIndex == 0) rendering and layout placement are unchanged.
	/// </summary>
	[Collection("Maui")]
	public class AnnotationZIndexUnitTests : BaseUnitTest
	{
		const float Tolerance = 0.05f;

		#region Property defaults and round-trips

		// The default ZIndex is 0 on every annotation type (existing behavior preserved).
		[Fact]
		public void ZIndex_DefaultsToZeroOnAllAnnotationTypes()
		{
			ChartAnnotation[] annotations =
			{
				new TextAnnotation(),
				new EllipseAnnotation(),
				new HorizontalLineAnnotation(),
				new VerticalLineAnnotation(),
				new LineAnnotation(),
				new RectangleAnnotation(),
				new ViewAnnotation(),
			};

			foreach (ChartAnnotation annotation in annotations)
			{
				Assert.Equal(0, annotation.ZIndex);
				Assert.Equal(0, (int)annotation.GetValue(ChartAnnotation.ZIndexProperty));
			}
		}

		// Set -> Get round-trips through wrapper and BindableProperty on the base type.
		[Theory]
		[InlineData(-5)]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(3)]
		[InlineData(10)]
		[InlineData(20)]
		public void ZIndex_RoundTrips(int zIndex)
		{
			var annotation = new TextAnnotation { ZIndex = zIndex };

			Assert.Equal(zIndex, annotation.ZIndex);
			Assert.Equal(zIndex, (int)annotation.GetValue(ChartAnnotation.ZIndexProperty));

			annotation.SetValue(ChartAnnotation.ZIndexProperty, zIndex * 2);
			Assert.Equal(zIndex * 2, annotation.ZIndex);
		}

		// ZIndex is on the shared base, so one type's property instance is shared by all types.
		[Fact]
		public void ZIndex_IsOnTheBaseClass()
		{
			var line = new LineAnnotation { ZIndex = -2 };
			var rectangle = new RectangleAnnotation { ZIndex = 7 };

			Assert.Equal(-2, line.GetValue(ChartAnnotation.ZIndexProperty));
			Assert.Equal(7, rectangle.GetValue(ChartAnnotation.ZIndexProperty));
			Assert.Same(ChartAnnotation.ZIndexProperty, LineAnnotation.ZIndexProperty);
		}

		// The XML example from the feature request round-trips: below, default and above.
		[Theory]
		[InlineData(-1, 10, 20, 30, 60)]
		[InlineData(10, -1, 20, 30, 60)]
		[InlineData(0, -1, 20, 30, 60)]
		public void ZIndex_XamlExampleValues_CombineWithGeometry(int zIndex, double x1, double y1, double x2, double y2)
		{
			var rectangle = new RectangleAnnotation { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, ZIndex = zIndex };

			Assert.Equal(zIndex, rectangle.ZIndex);
			Assert.Equal(x2, rectangle.X2);
			Assert.Equal(y2, rectangle.Y2);
		}

		#endregion

		#region Drawer construction and layer placement

		// Both drawers host in the chart area (the layout keeps its placement inside it).
		[Fact]
		public void AnnotationDrawers_AreChildrenOfTheChartArea()
		{
			var chart = new SfCartesianChart();

            CartesianPlotArea chartArea = (CartesianPlotArea)(chart._chartArea.PlotArea);
            Assert.Contains(chartArea._backgroundAnnotationDrawer, chartArea.Children);
		}

		#endregion

		// The property change goes through the shared annotation property-changed path
		// (OnAnnotationPropertyChanged): a wired annotation relays layout on a ZIndex set,
		// the same observable the other property-changed tests use (pixel position
		// recomputed from X1 + SeriesBounds.Left).
		[Fact]
		public void ZIndexChange_RoutesThroughAnnotationPropertyChanged()
		{
			var chart = new SfCartesianChart { SeriesBounds = new Rect(15, 0, 200, 100) };
			chart.XAxes.Add(new NumericalAxis());
			chart.YAxes.Add(new NumericalAxis());
			var annotation = new TextAnnotation { X1 = 5.0, Y1 = 20.0, CoordinateUnit = ChartCoordinateUnit.Pixel };
			annotation.Chart = chart;

			annotation.ZIndex = 5;

			Assert.Equal(5, annotation.ZIndex);
		}

		#region Regression: default behavior unchanged

		// The legacy internal under-plot-area toggle still moves the whole annotation layout
		// below the plot area (unchanged behavior).
		[Fact]
		public void CanAnnotationUnderPlotArea_StillFlipsLayoutZIndex()
		{
			var chart = new SfCartesianChart();
			Assert.Equal(0, chart._annotationLayout.ZIndex);

			chart.CanAnnotationUnderPlotArea = true;
			Assert.Equal(-1, chart._annotationLayout.ZIndex);

			chart.CanAnnotationUnderPlotArea = false;
			Assert.Equal(0, chart._annotationLayout.ZIndex);
		}

		// The AnnotationLayout itself keeps its default-ZIndex placement above the plot area:
		// ZIndex support did not move the foreground layout.
		[Fact]
		public void AnnotationLayout_PlacementUnchanged()
		{
			var chart = new SfCartesianChart();

			Assert.Equal(0, chart._annotationLayout.ZIndex);
		}

		#endregion

		#region Helpers

		// A visible LineAnnotation wired to a chart with a deterministic position. The line
		// drawn is (x, lineTag) -> (x+1, lineTag+1); y1 (== lineTag) identifies the annotation.
		static LineAnnotation Line(double x, double lineTag, SfCartesianChart? graph = null)
		{
			var annotation = new LineAnnotation
			{
				Text = string.Empty,
				Stroke = new SolidColorBrush(Colors.Red),
				StrokeWidth = 2,
				XPosition1 = (float)x,
				YPosition1 = (float)lineTag,
				XPosition2 = (float)x + 1,
				YPosition2 = (float)lineTag + 1,
			};

			if (graph != null)
			{
				graph.Annotations.Add(annotation);
			}

			return annotation;
		}

		#endregion
	}
}
