#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Charts;
using Syncfusion.Maui.Toolkit;
using Xunit;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{

	[Collection("Maui")]
	public class ChartLegendTitleUnitTests : BaseUnitTest
	{
		static class ReflectionHelpers
		{
			const System.Reflection.BindingFlags Any = System.Reflection.BindingFlags.Public
					| System.Reflection.BindingFlags.NonPublic
					| System.Reflection.BindingFlags.Instance;

			static System.Reflection.MemberInfo? FindMember(object owner, string name)
			{
				for (Type? type = owner.GetType(); type != null; type = type.BaseType)
				{
					var member = type.GetProperty(name, Any | System.Reflection.BindingFlags.DeclaredOnly) as System.Reflection.MemberInfo;
					member ??= type.GetField(name, Any | System.Reflection.BindingFlags.DeclaredOnly);
					if (member != null)
					{
						return member;
					}
				}

				return null;
			}

			internal static object? GetMember(object owner, string name) => FindMember(owner, name) switch
			{
				System.Reflection.PropertyInfo p => p.GetValue(owner),
				System.Reflection.FieldInfo f => f.GetValue(owner),
				_ => null
			};

			internal static void SetMember(object owner, string name, object? value)
			{
				switch (FindMember(owner, name))
				{
					case System.Reflection.PropertyInfo p:
						p.SetValue(owner, value);
						break;
					case System.Reflection.FieldInfo f:
						f.SetValue(owner, value);
						break;
				}
			}
		}

		static SfCartesianChart ChartWithLegendTitle(out ChartLegend legend)
		{
			var chart = new SfCartesianChart();
			chart.XAxes.Add(new NumericalAxis());
			chart.YAxes.Add(new NumericalAxis());
			var series = new ColumnSeries { Label = "A" };
			chart.Series.Add(series);
			legend = new ChartLegend();
			chart.Legend = legend;
			return chart;
		}

		static SfLegendExt CreateLegendView(ChartLegend legend)
		{
			var createLegendView = typeof(ChartLegend)
				.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
				.Single(m => m.Name.EndsWith("CreateLegendView", StringComparison.Ordinal));
			createLegendView.Invoke(legend, null);
			return Assert.IsType<SfLegendExt>(legend.sfLegend!);
		}

		static void SeedTitleSize(SfLegendExt legendView, double width, double height)
		{
			var desiredSize = typeof(VisualElement).GetProperty(nameof(VisualElement.DesiredSize))!;
			desiredSize.SetValue(legendView.TitleView, new Size(width, height));
		}

		static void SeedItemsSize(SfLegendExt legendView, double width, double height)
		{
			var desiredSize = typeof(VisualElement).GetProperty(nameof(VisualElement.DesiredSize))!;
			desiredSize.SetValue((VisualElement)((IList<IView>)legendView)[1]!, new Size(width, height));
		}

		#region Defaults and property round-trips

		[Fact]
		public void Title_Default_IsNull()
		{
			var legend = new ChartLegend();

			Assert.Null(legend.Title);
		}

		[Fact]
		public void Title_RoundTrips()
		{
			var view = new Label { Text = "Sales Report" };
			var legend = new ChartLegend { Title = view };

			Assert.Same(view, legend.Title);
		}

		[Theory]
		[InlineData("Sales Report")]
		[InlineData("Region wise revenue distribution")]
		public void Title_String_RoundTrips(string title)
		{
			var legend = new ChartLegend { Title = title };

			Assert.Equal(title, legend.Title);
		}

		#endregion

		#region Title view creation (text and view)

		[Fact]
		public void Title_Text_CreatesVisibleLabelTitleView()
		{
			var legend = new ChartLegend { Title = "Sales Report" };
			var legendView = CreateLegendView(legend);

			Assert.True(legendView.TitleView.IsVisible);
			var label = Assert.IsType<Label>(legendView.TitleView.Content);
			Assert.Equal("Sales Report", label.Text);
		}


		[Fact]
		public void Title_View_HostsTheSameViewInstance()
		{
			var view = new Label { Text = "Sales Report" };
			var legend = new ChartLegend { Title = view };
			var legendView = CreateLegendView(legend);

			Assert.True(legendView.TitleView.IsVisible);
			Assert.Same(view, legendView.TitleView.Content);
		}

		[Fact]
		public void NoTitle_KeepsTheTitleViewHidden()
		{
			var legend = new ChartLegend();
			var legendView = CreateLegendView(legend);

			Assert.False(legendView.TitleView.IsVisible);
			Assert.Null(legendView.TitleView.Content);
		}

		[Fact]
		public void Title_EmptyString_KeepsTheTitleViewHidden()
		{
			var legend = new ChartLegend { Title = string.Empty };
			var legendView = CreateLegendView(legend);

			Assert.False(legendView.TitleView.IsVisible);
		}

		[Fact]
		public void Title_ResetToNull_HidesTheTitleView()
		{
			var legend = new ChartLegend { Title = "Sales Report" };
			var legendView = CreateLegendView(legend);
			Assert.True(legendView.TitleView.IsVisible);

			legend.Title = string.Empty;

			Assert.False(legendView.TitleView.IsVisible);
			Assert.Null(legendView.TitleView.Content);
		}

		[Fact]
		public void Title_RuntimeChange_ReplacesTheTitleContent()
		{
			var legend = new ChartLegend { Title = "First" };
			var legendView = CreateLegendView(legend);

			legend.Title = "Second";

			var label = Assert.IsType<Label>(legendView.TitleView.Content);
			Assert.Equal("Second", label.Text);
		}

		[Fact]
		public void Title_RuntimeViewChange_HostsTheNewView()
		{
			var firstView = new Label { Text = "First" };
			var secondView = new Label { Text = "Second" };
			var legend = new ChartLegend { Title = firstView };
			var legendView = CreateLegendView(legend);

			legend.Title = secondView;

			Assert.Same(secondView, legendView.TitleView.Content);
			Assert.NotSame(firstView, legendView.TitleView.Content);
		}

		[Fact]
		public void TitleView_IsComposedBeforeTheLegendItemsView()
		{
			var legend = new ChartLegend { Title = "Sales Report" };
			var legendView = CreateLegendView(legend);
			var children = (IList<IView>)legendView;

			Assert.Equal(2, children.Count);
			Assert.Same(legendView.TitleView, children[0]);
		}

		[Fact]
		public void LegendItemsView_IsStillHostedWithATitle()
		{
			var legend = new ChartLegend { Title = "Sales Report" };
			var legendView = CreateLegendView(legend);
			var children = (IList<IView>)legendView;

			Assert.Equal(2, children.Count);
			Assert.IsType<ScrollView>(children[1]);
		}


		#endregion

		#region Title measurement and layout

		[Fact]
		public void MeasureContentWithTitle_GrowsBeyondTheItemsOnlyMeasure()
		{
			var legend = new ChartLegend();
			var legendView = CreateLegendView(legend);
			double itemsOnlyHeight = ((Microsoft.Maui.ILayout)legendView).CrossPlatformMeasure(400, 400).Height;

			legend.Title = "Sales Report";
			SeedTitleSize(legendView, 200, 30);

			double withTitleHeight = ((Microsoft.Maui.ILayout)legendView).CrossPlatformMeasure(400, 400).Height;

			Assert.True(withTitleHeight > itemsOnlyHeight, $"withTitle: {withTitleHeight}, itemsOnly: {itemsOnlyHeight}");
			Assert.Equal(itemsOnlyHeight + 30, withTitleHeight);
		}

		[Fact]
		public void ArrangeContentWithTitle_PlacesTheTitleAboveTheItems()
		{
			var legend = new ChartLegend { Title = new Label { Text = "Sales Report", WidthRequest = 200, HeightRequest = 30 } };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 200, 30);

			var bounds = new Rect(0, 0, 400, 300);
			((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(bounds);

			double titleTop = legendView.TitleView.Bounds.Y;
			Assert.True(titleTop >= 0 && titleTop < bounds.Height, $"the title must sit inside the legend bounds, got {titleTop}");
		}

		[Fact]
		public void ArrangeContentWithTitle_LeavesTheItemsBelowTheTitleSlot()
		{
			var legend = new ChartLegend { Title = new Label { Text = "Sales Report", WidthRequest = 200, HeightRequest = 30 } };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 200, 30);

			((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(new Rect(0, 0, 400, 300));

			var children = (IList<IView>)legendView;
			var itemsBounds = ((View)children[1]).Bounds;
			var titleBounds = legendView.TitleView.Bounds;
			Assert.Equal(30, titleBounds.Height);

			Assert.True(itemsBounds.Y >= titleBounds.Bottom, $"items must start at or below the title, items.Y: {itemsBounds.Y}, title.Bottom: {titleBounds.Bottom}");
			Assert.True(itemsBounds.Y + itemsBounds.Height <= 300, "items must stay inside the legend bounds");
		}

		[Fact]
		public void Measure_WithLongTitle_WrapsWithinTheAvailableWidth()
		{
			var legend = new ChartLegend
			{
				Title = "A very long legend title that must wrap into multiple lines within the available legend space"
			};
			var legendView = CreateLegendView(legend);

			double narrowWidth = ((Microsoft.Maui.ILayout)legendView).CrossPlatformMeasure(120, double.PositiveInfinity).Width;

			Assert.True(narrowWidth <= 120, $"measured width {narrowWidth} must wrap within 120");
		}

		[Fact]
		public void Measure_WithConstrainedHeight_LeavesTheItemsTheSpaceBelowTheTitle()
		{
			var legend = new ChartLegend { Title = "Sales Report" };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 200, 30);

			var size = ((Microsoft.Maui.ILayout)legendView).CrossPlatformMeasure(400, 50);

			Assert.Equal(30, size.Height);
		}

		[Fact]
		public void Arrange_WithTruncatedHeight_ClampsTheTitleWithinBounds()
		{
			var legend = new ChartLegend { Title = new Label { Text = "Sales Report", WidthRequest = 200, HeightRequest = 40 } };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 200, 40);

			((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(new Rect(0, 0, 400, 10));

			Assert.True(legendView.TitleView.Bounds.Height <= 10, $"title clamped to the legend height, got {legendView.TitleView.Bounds.Height}");
		}

		[Fact]
		public void Arrange_InAFullHeightStrip_CentersTheTitleAndItemsBlock()
		{
			var legend = new ChartLegend { Title = new Label { Text = "Sales Report", WidthRequest = 200, HeightRequest = 30 } };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 200, 30);
			SeedItemsSize(legendView, 200, 70);

			((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(new Rect(0, 0, 400, 500));

			var titleBounds = legendView.TitleView.Bounds;
			var itemsBounds = ((View)((IList<IView>)legendView)[1]).Bounds;
			Assert.Equal(100d, titleBounds.X);             
			Assert.Equal(200d, titleBounds.Y);             
			Assert.Equal(30d, titleBounds.Height);          
			Assert.Equal(100d, itemsBounds.X);              
			Assert.Equal(titleBounds.Bottom, itemsBounds.Y); 
			Assert.Equal(70d, itemsBounds.Height);          
			Assert.Equal(500d - 200d, itemsBounds.Bottom);  
		}

		[Fact]
		public void Arrange_InAShrinkWrappedLegend_KeepsTheBlockFlushAtTheTop()
		{
			var legend = new ChartLegend { Title = new Label { Text = "Legend", WidthRequest = 120, HeightRequest = 20 } };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 120, 20);
			SeedItemsSize(legendView, 120, 50);

			((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(new Rect(0, 0, 120, 70));

			var titleBounds = legendView.TitleView.Bounds;
			var itemsBounds = ((View)((IList<IView>)legendView)[1]).Bounds;
			Assert.Equal(0d, titleBounds.Y);              
			Assert.Equal(20d, itemsBounds.Y);             
			Assert.Equal(0d, titleBounds.X);                
			Assert.Equal(120d, titleBounds.Width);          
			Assert.Equal(titleBounds.Bottom, itemsBounds.Y);
		}

		[Fact]
		public void Arrange_WithAConstrainedHeight_SplitsTheSpaceProportionally()
		{
			var legend = new ChartLegend { Title = new Label { Text = "Sales Report", WidthRequest = 200, HeightRequest = 40 } };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 200, 40);
			SeedItemsSize(legendView, 200, 60);

			var arrangedSize = ((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(new Rect(0, 0, 400, 50));

			var titleBounds = legendView.TitleView.Bounds;
			Assert.Equal(50 * (40.0 / 100.0), titleBounds.Height); 
			Assert.Equal(0d, titleBounds.Y);                     
			Assert.Equal(50 * (60.0 / 100.0), arrangedSize.Height - titleBounds.Height);
		}

		[Fact]
		public void Arrange_WithEmptyBounds_KeepsThePartsCollapsed()
		{
			var legend = new ChartLegend { Title = new Label { Text = "Sales Report", WidthRequest = 200, HeightRequest = 40 } };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 200, 40);
			SeedItemsSize(legendView, 200, 60);

			var arrangedSize = ((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(new Rect(0, 0, 400, 0));

			Assert.Equal(0, legendView.TitleView.Bounds.Height);
			Assert.Equal(0d, arrangedSize.Height);
		}

		[Fact]
		public void Arrange_WithATitleWiderThanTheLegend_ClampsTheTitleSlotToTheBounds()
		{
			var legend = new ChartLegend { Title = new Label { Text = "A very overlong report title", WidthRequest = 600, HeightRequest = 30 } };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 600, 30);
			SeedItemsSize(legendView, 120, 70);

			var arrangedSize = ((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(new Rect(0, 0, 400, 500));

			Assert.Equal(400d, arrangedSize.Width); 
		}

		[Fact]
		public void Arrange_HostlessMeasures_KeepTheSeededTitleSizeAuthoritative()
		{
			var legend = new ChartLegend { Title = new Label { Text = "Overlong", WidthRequest = 600, HeightRequest = 30 } };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 600, 30);
			SeedItemsSize(legendView, 120, 60);

			var arrangedSize = ((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(new Rect(0, 0, 400, 500));

			Assert.Equal(400d, arrangedSize.Width); 
			Assert.Equal(60d, arrangedSize.Height);
		}

		[Theory]
		[InlineData(LegendPlacement.Left)]
		[InlineData(LegendPlacement.Right)]
		[InlineData(LegendPlacement.Top)]
		[InlineData(LegendPlacement.Bottom)]
		public void Arrange_IsPlacementIndependent(LegendPlacement placement)
		{
			var legend = new ChartLegend { Title = new Label { Text = "Report", WidthRequest = 100, HeightRequest = 20 }, Placement = placement };
			var legendView = CreateLegendView(legend);
			SeedTitleSize(legendView, 100, 20);
			SeedItemsSize(legendView, 100, 60);

			((Microsoft.Maui.ILayout)legendView).CrossPlatformArrange(new Rect(0, 0, 400, 200));

			var titleBounds = legendView.TitleView.Bounds;
			var itemsBounds = ((View)((IList<IView>)legendView)[1]).Bounds;
			Assert.Equal(150d, titleBounds.X);               
			Assert.Equal(60d, titleBounds.Y);               
			Assert.Equal(titleBounds.Bottom, itemsBounds.Y); 
			Assert.Equal(150d, itemsBounds.X);              
		}

		static bool AreaNeedsRelayout(SfCartesianChart chart)
		{
			var area = ReflectionHelpers.GetMember(chart._legendLayout, "_areaBase")!;
			return (bool)ReflectionHelpers.GetMember(area, "NeedsRelayout")!;
		}

		static void ResetAreaNeedsRelayout(SfCartesianChart chart)
		{
			var area = ReflectionHelpers.GetMember(chart._legendLayout, "_areaBase")!;
			ReflectionHelpers.SetMember(area, "NeedsRelayout", false);
		}

		[Fact]
		public void Title_RuntimeChange_SchedulesAChartRelayout()
		{
			var chart = ChartWithLegendTitle(out var legend);
			ResetAreaNeedsRelayout(chart);

			legend.Title = "Sales Report";

			Assert.True(AreaNeedsRelayout(chart));
		}
		#endregion
	}
}