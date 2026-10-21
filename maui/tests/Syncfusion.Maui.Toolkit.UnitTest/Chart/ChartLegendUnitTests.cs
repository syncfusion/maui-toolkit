using Syncfusion.Maui.Toolkit.Charts;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class ChartLegendUnitTests
	{
		[Fact]
		public void LegendItems_RepeatedAccess_ReturnsSameWrapperPerPlotArea()
		{
			var plotArea = new ChartPlotArea();
			var legendItems = plotArea.LegendItems;

			for (int i = 0; i < 1000; i++)
			{
				Assert.Same(legendItems, plotArea.LegendItems);
			}

			Assert.NotSame(legendItems, new ChartPlotArea().LegendItems);
		}

		[Fact]
		public void LegendItems_CollectionChanges_ForwardsLiveContentsAndNotifications()
		{
			var plotArea = new ChartPlotArea();
			var legendItems = plotArea.LegendItems;
			var actions = new List<NotifyCollectionChangedAction>();
			var properties = new List<string?>();
			((INotifyCollectionChanged)legendItems).CollectionChanged += (sender, args) =>
			{
				Assert.Same(legendItems, sender);
				actions.Add(args.Action);
			};
			((INotifyPropertyChanged)legendItems).PropertyChanged += (_, args) => properties.Add(args.PropertyName);
			var first = new LegendItem();
			var replacement = new LegendItem();

			plotArea._legendItems.Add(first);
			Assert.Same(first, Assert.Single(legendItems));
			plotArea._legendItems[0] = replacement;
			Assert.Same(replacement, Assert.Single(legendItems));
			plotArea._legendItems.Remove(replacement);
			Assert.Empty(legendItems);
			plotArea._legendItems.Add(first);
			plotArea._legendItems.Clear();
			Assert.Empty(legendItems);
			Assert.Same(legendItems, plotArea.LegendItems);
			Assert.Equal(new[]
			{
				NotifyCollectionChangedAction.Add,
				NotifyCollectionChangedAction.Replace,
				NotifyCollectionChangedAction.Remove,
				NotifyCollectionChangedAction.Add,
				NotifyCollectionChangedAction.Reset
			}, actions);
			Assert.Equal(4, properties.Count(name => name == "Count"));
			Assert.Equal(5, properties.Count(name => name == "Item[]"));
		}

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void LegendItems_FillAndPaletteChanges_PreservesWrapperAndUpdatesIcon(bool hasLegend)
		{
			var chart = new SfCartesianChart();
			// Palette callbacks skip legend updates until the chart has plot bounds.
			((IChart)chart).ActualSeriesClipRect = new Rect(0, 0, 100, 100);
			if (hasLegend)
			{
				// Avoid native dispatcher-dependent icon bindings; assert the icon brush below.
				chart.Legend = new ChartLegend { ItemTemplate = new DataTemplate(() => new Label()) };
			}

			var series = new ColumnSeries();
			chart.Series.Add(series);
			var plotArea = Assert.IsAssignableFrom<ChartPlotArea>(chart._chartArea.PlotArea);
			var legendItems = plotArea.LegendItems;
			var legendItem = new LegendItem { Item = series };
			plotArea._legendItems.Add(legendItem);
			Brush red = new SolidColorBrush(Colors.Red);
			Brush blue = new SolidColorBrush(Colors.Blue);

			for (int i = 0; i < 1000; i++)
			{
				var brush = i % 2 == 0 ? red : blue;
				series.Fill = brush;
				Assert.Same(legendItems, plotArea.LegendItems);
				if (hasLegend)
				{
					Assert.Same(brush, legendItem.IconBrush);
				}

				series.ClearValue(ChartSeries.FillProperty);
				chart.PaletteBrushes = new List<Brush> { brush };
				series.PaletteBrushes = new List<Brush> { brush };
				Assert.Same(legendItems, plotArea.LegendItems);
				if (hasLegend)
				{
					Assert.Same(brush, legendItem.IconBrush);
				}
			}
		}

		[Fact]
		public void SfLegend_SameItemsSource_RefreshesChildrenThroughCollectionChanges()
		{
			var plotArea = new ChartPlotArea();
			var legendItems = plotArea.LegendItems;
			var first = new LegendItem { Text = "First" };
			var second = new LegendItem { Text = "Second" };
			plotArea._legendItems.Add(first);
			var legend = new SfLegend { ItemsSource = legendItems };
			Assert.Same(first, Assert.IsAssignableFrom<BindableObject>(Assert.Single(legend.ContentLayout.Children)).BindingContext);

			// Reassign the same wrapper as LegendLayout does when legend items are updated.
			legend.ItemsSource = plotArea.LegendItems;
			Assert.Same(legendItems, legend.ItemsSource);
			Assert.Same(legendItems, BindableLayout.GetItemsSource(legend.ContentLayout));

			plotArea._legendItems.Add(second);
			Assert.Equal(2, legend.ContentLayout.Children.Count);
			Assert.Same(second, Assert.IsAssignableFrom<BindableObject>(legend.ContentLayout.Children[1]).BindingContext);
			plotArea._legendItems[0] = second;
			Assert.Same(second, Assert.IsAssignableFrom<BindableObject>(legend.ContentLayout.Children[0]).BindingContext);
			plotArea._legendItems.RemoveAt(0);
			Assert.Same(second, Assert.IsAssignableFrom<BindableObject>(Assert.Single(legend.ContentLayout.Children)).BindingContext);
			plotArea._legendItems.Clear();
			Assert.Empty(legend.ContentLayout.Children);
			plotArea._legendItems.Add(first);
			Assert.Same(first, Assert.IsAssignableFrom<BindableObject>(Assert.Single(legend.ContentLayout.Children)).BindingContext);
			Assert.Same(legendItems, legend.ItemsSource);
		}

		[Fact]
		public void ChartLegend_Constructor_InitializesDefaultsCorrectly()
		{
			var chartLegend = new ChartLegend();

			Assert.True(chartLegend.IsVisible);
			Assert.Equal(LegendPlacement.Top, chartLegend.Placement);
			Assert.Null(chartLegend.ItemsLayout);
			Assert.False(chartLegend.ToggleSeriesVisibility);
			Assert.Null(chartLegend.ItemTemplate);
			Assert.Null(chartLegend.LabelStyle);
		}

		[Fact]
		public void ChartLegendLabelStyle_Constructor_InitializesDefaultsCorrectly()
		{
			var labelStyle = new ChartLegendLabelStyle();

			Assert.Equal(new Color(0.28627452f, 0.27058825f, 0.30980393f, 1), labelStyle.TextColor);
			Assert.Equal(new Thickness(0), labelStyle.Margin);
			Assert.Equal(12, labelStyle.FontSize);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
		}

		[Theory]
		[InlineData(LegendPlacement.Left)]
		[InlineData(LegendPlacement.Top)]
		[InlineData(LegendPlacement.Right)]
		[InlineData(LegendPlacement.Bottom)]
		public void Placement_SetAndGet_ReturnsExpectedValue(LegendPlacement placement)
		{
			var legend = new ChartLegend
			{
				Placement = placement
			};

			Assert.Equal(placement, legend.Placement);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ToggleSeriesVisibility_SetAndGet_ReturnsExpectedValue(bool visibility)
		{
			var legend = new ChartLegend
			{
				ToggleSeriesVisibility = visibility
			};

			Assert.Equal(visibility, legend.ToggleSeriesVisibility);
		}

		[Fact]
		public void ItemTemplate_SetAndGet_ReturnsExpectedValue()
		{
			var expectedTemplate = new DataTemplate(() => new Label { Text = "Test" });
			var legend = new ChartLegend
			{
				ItemTemplate = expectedTemplate
			};

			var actualTemplate = legend.ItemTemplate;

			Assert.Equal(expectedTemplate, actualTemplate);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsVisible_SetAndGet_ReturnsExpectedValue(bool isVisible)
		{
			var legend = new ChartLegend
			{
				IsVisible = isVisible
			};

			var actualVisibility = legend.IsVisible;

			Assert.Equal(isVisible, actualVisibility);
		}

		[Fact]
		public void ItemsLayout_SetAndGet_ReturnsExpectedValue()
		{
			var expectedLayout = new StackLayout();
			var legend = new ChartLegend
			{
				ItemsLayout = expectedLayout
			};

			var actualLayout = legend.ItemsLayout;

			Assert.Equal(expectedLayout, actualLayout);
		}

		[Fact]
		public void LabelStyle_SetAndGet_ReturnsExpectedValue()
		{
			var expectedStyle = new ChartLegendLabelStyle
			{
				FontSize = 12,
				FontFamily = "Arial",
				FontAttributes = FontAttributes.Bold,
				Margin = new Thickness(5),
				TextColor = Colors.Red
			};
			var legend = new ChartLegend
			{
				LabelStyle = expectedStyle
			};

			var actualStyle = legend.LabelStyle;

			Assert.Equal(expectedStyle, actualStyle);
		}

		[Theory]
		[InlineData(12.0)]
		[InlineData(14.5)]
		[InlineData(16.75)]
		public void FontSize_SetAndGet_ReturnsExpectedValue(double fontSize)
		{
			var labelStyle = new ChartLegendLabelStyle
			{
				FontSize = fontSize
			};

			var actualFontSize = labelStyle.FontSize;

			Assert.Equal(fontSize, actualFontSize);
		}

		[Theory]
		[InlineData("Arial")]
		[InlineData("Times New Roman")]
		[InlineData("Verdana")]
		public void FontFamily_SetAndGet_ReturnsExpectedValue(string fontFamily)
		{
			var labelStyle = new ChartLegendLabelStyle
			{
				FontFamily = fontFamily
			};

			var actualFontFamily = labelStyle.FontFamily;

			Assert.Equal(fontFamily, actualFontFamily);
		}

		[Theory]
		[InlineData(FontAttributes.Bold)]
		[InlineData(FontAttributes.Italic)]
		[InlineData(FontAttributes.None)]
		public void FontAttributes_SetAndGet_ReturnsExpectedValue(FontAttributes fontAttributes)
		{
			var labelStyle = new ChartLegendLabelStyle
			{
				FontAttributes = fontAttributes
			};

			var actualFontAttributes = labelStyle.FontAttributes;

			Assert.Equal(fontAttributes, actualFontAttributes);
		}

		[Theory]
		[InlineData(1, 2, 3, 4)]
		[InlineData(5, 6, 7, 8)]
		[InlineData(9, 10, 11, 12)]
		public void Margin_SetAndGet_ReturnsExpectedValue(int left, int top, int right, int bottom)
		{
			var expectedMargin = new Thickness(left, top, right, bottom);
			var labelStyle = new ChartLegendLabelStyle
			{
				Margin = expectedMargin
			};

			var actualMargin = labelStyle.Margin;

			Assert.Equal(expectedMargin, actualMargin);
		}
		[Fact]
		public void TextColor_SetAndGet_ReturnsExpectedValue()
		{
			var label = new ChartLegendLabelStyle();
			var expected = Colors.Green;

			label.TextColor = expected;

			Assert.Equal(expected, label.TextColor);
		}
	}
}
