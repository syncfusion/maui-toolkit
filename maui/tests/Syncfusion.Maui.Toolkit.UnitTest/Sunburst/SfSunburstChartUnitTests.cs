using Syncfusion.Maui.Toolkit.SunburstChart;

namespace Syncfusion.Maui.Toolkit.UnitTest.Sunburst
{
    public class SfSunburstChartUnitTests : BaseUnitTest
    {

        #region Public Properties

        [Theory]
        [InlineData(0)]
        [InlineData(10.5)]
        [InlineData(-10.25)]
        [InlineData(double.NaN)]
        public void AnimationDuration_SfSunburstChart(double expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                AnimationDuration = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.AnimationDuration);
        }

        [Fact]
        public void CenterHoleSize_SfSunburstChart()
        {
            SfSunburstChart sunburstChart = new();
            double expectedValue = 1;

            // The CenterHoleSize property is meant to be set internally and not externally.
            // This test checks if it returns the correct default value of 1.

            Assert.Equal(expectedValue, sunburstChart.CenterHoleSize);
        }

        [Fact]
        public void CenterView_SfSunburstChart()
        {
            SfSunburstChart sunburstChart = new();
            View expectedValue = new FlexLayout();
            sunburstChart.CenterView = expectedValue;

            Assert.Equal(expectedValue, sunburstChart.CenterView);
        }

        [Fact]
        public void DataLabelSettings_SfSunburstChart()
        {
            SfSunburstChart sunburstChart = new();
            SunburstDataLabelSettings expectedValue = new()
            {
                FontSize = 12,
                TextColor = Colors.Blue
            };

            sunburstChart.DataLabelSettings = expectedValue;
            Assert.Equal(expectedValue, sunburstChart.DataLabelSettings);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableAnimation_SfSunburstChart(bool expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                EnableAnimation = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.EnableAnimation);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableTooltip_SfSunburstChart(bool expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                EnableTooltip = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.EnableTooltip);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10.5)]
        [InlineData(-10.25)]
        [InlineData(double.NaN)]
        public void EndAngle_SfSunburstChart(double expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                EndAngle = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.EndAngle);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        [InlineData(-0.5)]
        [InlineData(1)]
        [InlineData(double.NaN)]
        public void InnerRadius_SfSunburstChart(double expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                InnerRadius = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.InnerRadius);
        }

        [Fact]
        public void ItemsSource_SfSunburstChart()
        {
            SfSunburstChart sunburstChart = new();
            var data = new List<dynamic>
            {
                new  { XValue = "A", YValue = 10 },
                new  { XValue = "B", YValue = 20 }
            };
            sunburstChart.ItemsSource = data;

            Assert.Equal(data, sunburstChart.ItemsSource);
        }

        [Fact]
        public void Legend_SfSunburstChart()
        {
            SfSunburstChart sunburstChart = new();
            SunburstLegend legend = new();
            sunburstChart.Legend = legend;

            Assert.Equal(legend, sunburstChart.Legend);
        }

        [Fact]
        public void Levels_SfSunburstChart()
        {
            var sunburstChart = new SfSunburstChart();
            SunburstLevelCollection expectedValue = [];
            sunburstChart.Levels = expectedValue;

            Assert.Equal(expectedValue, sunburstChart.Levels);
        }

        [Fact]
        public void PaletteBrushes_SfSunburstChart()
        {
            SfSunburstChart sunburstChart = new();

            IList<Brush> expectedValue =
            [
                new SolidColorBrush(Colors.Red),
                new SolidColorBrush(Colors.Green),
                new SolidColorBrush(Colors.Blue)
            ];
            sunburstChart.PaletteBrushes = expectedValue;

            Assert.Equal(expectedValue, sunburstChart.PaletteBrushes);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.5)]
        [InlineData(-0.5)]
        [InlineData(1)]
        [InlineData(double.NaN)]
        public void Radius_SfSunburstChart(double expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                Radius = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.Radius);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowLabels_SfSunburstChart(bool expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                ShowLabels = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.ShowLabels);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10.5)]
        [InlineData(-10.25)]
        [InlineData(double.NaN)]
        public void StartAngle_SfSunburstChart(double expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                StartAngle = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.StartAngle);
        }


        [Fact]
        public void Stroke_SfSunburstChart()
        {
            SfSunburstChart sunburstChart = new()
            {
                Stroke = new SolidColorBrush(Colors.Blue)
            };

            Assert.Equal(Colors.Blue, sunburstChart.Stroke);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(-3)]
        [InlineData(double.NaN)]
        public void StrokeWidth_SfSunburstChart(double expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                StrokeWidth = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.StrokeWidth);
        }

        [Theory]
        [InlineData("Sunburst Chart")]
        [InlineData(123)]
        [InlineData(45.67)]
        [InlineData(true)]
        public void Title_SfSunburstChart(object expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                Title = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.Title);
        }

        [Fact]
        public void TooltipSettings_SfSunburstChart()
        {
            SfSunburstChart sunburstChart = new();
            SunburstTooltipSettings expectedValue = new SunburstTooltipSettings()
            {
                TextColor = Colors.Black,
                Background = Brush.White,
                FontSize = 14,
                Duration = 5,
            };

            sunburstChart.TooltipSettings = expectedValue;
            Assert.Equal(expectedValue, sunburstChart.TooltipSettings);
        }

        [Fact]
        public void TooltipTemplate_SfSunburstChart()
        {
            DataTemplate labelTemplate = new DataTemplate(() =>
            {
                var horizontalStackLayout = new HorizontalStackLayout { Spacing = 5 };
                var salesRateLabel = new Label
                {
                    TextColor = Colors.Red,
                    FontSize = 13,
                };

                salesRateLabel.SetBinding(Label.TextProperty, "Item.SalesRate");
                horizontalStackLayout.Children.Add(salesRateLabel);

                return horizontalStackLayout;
            });

            SfSunburstChart sunburstChart = new()
            {
                TooltipTemplate = labelTemplate
            };

            Assert.Equal(labelTemplate, sunburstChart.TooltipTemplate);
        }

        [Theory]
        [InlineData("")]
        [InlineData("XValue")]
        public void ValueMemberPath_SfSunburstChart(string expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                ValueMemberPath = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.ValueMemberPath);
        }

        #endregion

        #region Internal properties

        [Fact]
        public void InternalDataSource_SfSunburstChart()
        {
            SfSunburstChart sunburstChart = new();
            var data = new List<dynamic>
            {
                new  { XValue = "A", YValue = 10 },
                new  { XValue = "B", YValue = 20 }
            };

            sunburstChart.InternalDataSource = data;
            Assert.Equal(data, sunburstChart.InternalDataSource);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void NeedToAnimate_SfSunburstChart(bool expectedValue)
        {
            SfSunburstChart sunburstChart = new()
            {
                NeedToAnimate = expectedValue
            };

            Assert.Equal(expectedValue, sunburstChart.NeedToAnimate);
        }

        #endregion

        #region SunburstChartArea

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShouldPopulateLegendItems_SunburstChartArea(bool expectedValue)
        {
            SfSunburstChart sunburstChart = new SfSunburstChart();
            SunburstChartArea chartArea = new SunburstChartArea(sunburstChart)
            {
                ShouldPopulateLegendItems = expectedValue
            };

            var actualValue = chartArea.ShouldPopulateLegendItems;
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void PlotAreaBounds_SunburstChartArea()
        {
            Rect expectedValue = new Rect(0, 0, 100, 100);

            SfSunburstChart sunburstChart = new SfSunburstChart();
            SunburstChartArea chartArea = new SunburstChartArea(sunburstChart)
            {
                PlotAreaBounds = expectedValue
            };

            var actualValue = chartArea.PlotAreaBounds;
            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region SunburstDataLabelSettings

        [Theory]
        [InlineData(Microsoft.Maui.Controls.FontAttributes.Bold)]
        [InlineData(Microsoft.Maui.Controls.FontAttributes.Italic)]
        [InlineData(Microsoft.Maui.Controls.FontAttributes.None)]
        public void FontAttributes_SunburstDataLabelSettings(FontAttributes expectedFontAttributes)
        {
            SunburstDataLabelSettings dataLabelSettings = new()
            {
                FontAttributes = expectedFontAttributes
            };

            var actualFontAttributes = dataLabelSettings.FontAttributes;
            Assert.Equal(expectedFontAttributes, actualFontAttributes);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(12.4)]
        [InlineData(-12.22)]
        [InlineData(double.NaN)]
        public void FontSize_SunburstDataLabelSettings(double expectedFontSize)
        {
            SunburstDataLabelSettings dataLabelSettings = new()
            {
                FontSize = expectedFontSize
            };

            var actualFontSize = dataLabelSettings.FontSize;
            Assert.Equal(expectedFontSize, actualFontSize);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Times New Roman")]
        public void FontFamily_SunburstDataLabelSettings(string expectedactualFontFamily)
        {
            SunburstDataLabelSettings dataLabelSettings = new()
            {
                FontFamily = expectedactualFontFamily
            };

            var actualFontFamily = dataLabelSettings.FontFamily;
            Assert.Equal(expectedactualFontFamily, actualFontFamily);
        }

        [Theory]
        [InlineData(SunburstLabelOverflowMode.Trim)]
        [InlineData(SunburstLabelOverflowMode.Hide)]
        public void OverFlowMode_SunburstDataLabelSettings(SunburstLabelOverflowMode expectedValue)
        {
            SunburstDataLabelSettings dataLabelSettings = new()
            {
                OverFlowMode = expectedValue
            };

            var actualValue = dataLabelSettings.OverFlowMode;
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(SunburstLabelRotationMode.Normal)]
        [InlineData(SunburstLabelRotationMode.Angle)]
        public void RotationMode_SunburstDataLabelSettings(SunburstLabelRotationMode expectedValue)
        {
            SunburstDataLabelSettings dataLabelSettings = new()
            {
                RotationMode = expectedValue
            };

            var actualValue = dataLabelSettings.RotationMode;
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void TextColor_SunburstDataLabelSettings()
        {
            SunburstDataLabelSettings dataLabelSettings = new()
            {
                TextColor = Colors.Red
            };

            Assert.Equal(Colors.Red, dataLabelSettings.TextColor);
        }

        #endregion

        #region SunburstHierarchicalLevel
        #region public property

        [Theory]
        [InlineData("")]
        [InlineData("sunburst")]
        public void GroupMemberPath_SunburstHierarchicalLevel(string expectedValue)
        {
            SunburstHierarchicalLevel sunburstHierarchicalLevel = new()
            {
                GroupMemberPath = expectedValue
            };

            Assert.Equal(expectedValue, sunburstHierarchicalLevel.GroupMemberPath);
        }

        #endregion

        #region Internal property

        [Theory]
        [InlineData("")]
        [InlineData("sunburst")]
        public void GroupingPath_SunburstHierarchicalLevel(string expectedValue)
        {
            SunburstHierarchicalLevel sunburstHierarchicalLevel = new()
            {
                GroupingPath = expectedValue
            };

            Assert.Equal(expectedValue, sunburstHierarchicalLevel.GroupingPath);
        }

        #endregion
        #endregion

        #region SunburstLegend
        #region Public Properties

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsVisible_SetAndGet_ReturnsExpectedValue(bool expectedVisibility)
        {
            SunburstLegend legend = new()
            {
                IsVisible = expectedVisibility
            };

            var actualVisibility = legend.IsVisible;
            Assert.Equal(expectedVisibility, actualVisibility);
        }

        [Theory]
        [InlineData(LegendPlacement.Top)]
        [InlineData(LegendPlacement.Left)]
        [InlineData(LegendPlacement.Right)]
        [InlineData(LegendPlacement.Bottom)]
        public void Placement_SetAndGet_ReturnsExpectedValue(LegendPlacement expectedPlacement)
        {
            SunburstLegend legend = new()
            {
                Placement = expectedPlacement
            };

            var actualPlacement = legend.Placement;
            Assert.Equal(expectedPlacement, actualPlacement);
        }

        #endregion

        #region ChartLegendLabelStyle

        [Theory]
        [InlineData(0)]
        [InlineData(12.4)]
        [InlineData(-12.22)]
        [InlineData(double.NaN)]
        public void FontSize_ChartLegendLabelStyle(double expectedFontSize)
        {
            ChartLegendLabelStyle labelStyle = new()
            {
                FontSize = expectedFontSize
            };

            var actualFontSize = labelStyle.FontSize;
            Assert.Equal(expectedFontSize, actualFontSize);
        }

        [Fact]
        public void TextColor_ChartLegendLabelStyle()
        {
            ChartLegendLabelStyle chartLegendLabelStyle = new()
            {
                TextColor = Colors.Red
            };

            Assert.Equal(Colors.Red, chartLegendLabelStyle.TextColor);
        }

        #endregion
        #endregion

        #region SunburstSegmentUnit

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(-10)]
        public void CurrentLevel_SunburstSegment(int expectedValue)
        {
            SunburstSegment sunburstSegment = new()
            {
                CurrentLevel = expectedValue
            };

            Assert.Equal(expectedValue, sunburstSegment.CurrentLevel);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(-10)]
        public void Index_SunburstSegment(int expectedValue)
        {
            SunburstSegment sunburstSegment = new()
            {
                Index = expectedValue
            };

            Assert.Equal(expectedValue, sunburstSegment.Index);
        }

        [Fact]
        public void Stroke_SunburstSegment()
        {
            SunburstSegment sunburstSegment = new()
            {
                Stroke = new SolidColorBrush(Colors.Blue)
            };

            Assert.Equal(Brush.Blue, sunburstSegment.Stroke);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(-3)]
        [InlineData(double.NaN)]
        public void StrokeWidth_SunburstSegment(double expectedValue)
        {
            SunburstSegment sunburstSegment = new()
            {
                StrokeWidth = expectedValue
            };

            Assert.Equal(expectedValue, sunburstSegment.StrokeWidth);
        }

        [Theory]
        [InlineData("String value")]
        [InlineData(123)]
        [InlineData(45.67)]
        [InlineData(true)]
        public void Item_SunburstSegment(object expectedValue)
        {
            SunburstSegment sunburstSegment = new()
            {
                Item = expectedValue
            };

            Assert.Equal(expectedValue, sunburstSegment.Item);
        }

        [Fact]
        public void Fill_SunburstSegment()
        {
            SunburstSegment sunburstSegment = new()
            {
                Fill = new SolidColorBrush(Colors.Blue)
            };

            Assert.Equal(Brush.Blue, sunburstSegment.Fill);
        }

        #endregion

        #region SunburstTooltipSettings

        #region public properties

        [Fact]
        public void Background_SunburstTooltipSettings()
        {
            SunburstTooltipSettings tooltipSettings = new()
            {
                Background = Brush.Red
            };

            Assert.Equal(Brush.Red, tooltipSettings.Background);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10.5)]
        [InlineData(-10.25)]
        [InlineData(double.NaN)]
        public void Duration_SunburstTooltipSettings(float expectedDuration)
        {
            SunburstTooltipSettings tooltipSettings = new()
            {
                Duration = expectedDuration
            };

            var actualDuration = tooltipSettings.Duration;
            Assert.Equal(expectedDuration, actualDuration);
        }

        [Theory]
        [InlineData(Microsoft.Maui.Controls.FontAttributes.Bold)]
        [InlineData(Microsoft.Maui.Controls.FontAttributes.Italic)]
        [InlineData(Microsoft.Maui.Controls.FontAttributes.None)]
        public void FontAttributes_SunburstTooltipSettings(FontAttributes expectedFontAttributes)
        {
            SunburstTooltipSettings tooltipSettings = new()
            {
                FontAttributes = expectedFontAttributes
            };

            var actualFontAttributes = tooltipSettings.FontAttributes;
            Assert.Equal(expectedFontAttributes, actualFontAttributes);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(12.4)]
        [InlineData(-12.22)]
        [InlineData(double.NaN)]
        public void FontSize_SunburstTooltipSettings(double expectedFontSize)
        {
            SunburstTooltipSettings tooltipSettings = new()
            {
                FontSize = expectedFontSize,
            };

            var actualFontSize = tooltipSettings.FontSize;
            Assert.Equal(expectedFontSize, actualFontSize);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Times New Roman")]
        public void FontFamily_SunburstTooltipSettings(string expectedFontFamily)
        {
            SunburstTooltipSettings tooltipSettings = new()
            {
                FontFamily = expectedFontFamily,
            };

            var actualFontFamily = tooltipSettings.FontFamily;
            Assert.Equal(expectedFontFamily, actualFontFamily);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 2, 3, 4)]
        [InlineData(-1, 0, 3, 5)]
        [InlineData(0, 6, 0, 8)]
        [InlineData(-9, -10, 11, 12)]
        [InlineData(9, 10, -11, -12)]
        public void Margin_SunburstTooltipSettings(int left, int top, int right, int bottom)
        {
            var expectedMargin = new Thickness(left, top, right, bottom);
            SunburstTooltipSettings tooltipSettings = new()
            {
                Margin = expectedMargin
            };

            var actualMargin = tooltipSettings.Margin;
            Assert.Equal(expectedMargin, actualMargin);
        }

        [Fact]
        public void TextColor_SunburstTooltipSettings()
        {
            SunburstDataLabelSettings tooltipSettings = new()
            {
                TextColor = Colors.Red
            };

            Assert.Equal(Colors.Red, tooltipSettings.TextColor);
        }

        #endregion

        #endregion

        #region Internal Selection Methods Tests

        [Fact]
        public void SelectedSegments_EmptyByDefault_ReturnsEmptyCollection()
        {
            SfSunburstChart sunburstChart = new();

            Assert.NotNull(sunburstChart.SelectedSunburstItems);
            Assert.Empty(sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void SelectedSegments_AddSegment_ContainsAddedSegment()
        {
            SfSunburstChart sunburstChart = new();
            SunburstItem segment = new();

            sunburstChart.SelectedSunburstItems.Add(segment);

            Assert.Single(sunburstChart.SelectedSunburstItems);
            Assert.Contains(segment, sunburstChart.SelectedSunburstItems);
        }



        #endregion

        #region SelectionSettings Tests

        [Fact]
        public void SelectionSettings_SelectionType_DefaultValue()
        {
            SunburstSelectionSettings selectionSettings = new SunburstSelectionSettings();

            var actualValue = selectionSettings.Type;

            Assert.Equal(SunburstSelectionType.Single, actualValue);
        }

        [Theory]
        [InlineData(SunburstSelectionType.Single)]
        [InlineData(SunburstSelectionType.Child)]
        [InlineData(SunburstSelectionType.Group)]
        [InlineData(SunburstSelectionType.Parent)]
        public void SelectionSettings_SelectionType_SetAndGet(SunburstSelectionType expectedValue)
        {
            SunburstSelectionSettings selectionSettings = new();

            selectionSettings.Type = expectedValue;

            Assert.Equal(expectedValue, selectionSettings.Type);
        }

        [Fact]
        public void SelectionSettings_SelectionDisplayMode_DefaultValue()
        {
            SunburstSelectionSettings selectionSettings = new();

            var actualValue = selectionSettings.DisplayMode;

            Assert.Equal(SunburstSelectionDisplayMode.HighlightByBrush, actualValue);
        }

        [Theory]
        [InlineData(SunburstSelectionDisplayMode.HighlightByOpacity)]
        [InlineData(SunburstSelectionDisplayMode.HighlightByBrush)]
        [InlineData(SunburstSelectionDisplayMode.HighlightByStroke)]
        public void SelectionSettings_SelectionDisplayMode_SetAndGet(SunburstSelectionDisplayMode expectedValue)
        {
            SunburstSelectionSettings selectionSettings = new();

            selectionSettings.DisplayMode = expectedValue;

            Assert.Equal(expectedValue, selectionSettings.DisplayMode);
        }

        [Fact]
        public void SelectionSettings_Opacity_DefaultValue()
        {
            SunburstSelectionSettings selectionSettings = new();

            var actualValue = selectionSettings.Opacity;

            Assert.Equal(0.7, actualValue);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.5)]
        [InlineData(1.0)]
        public void SelectionSettings_Opacity_SetAndGet(double expectedValue)
        {
            SunburstSelectionSettings selectionSettings = new();

            selectionSettings.Opacity = expectedValue;

            Assert.Equal(expectedValue, selectionSettings.Opacity);
        }

        [Fact]
        public void SelectionSettings_StrokeWidth_DefaultValue()
        {
            SunburstSelectionSettings selectionSettings = new();

            double actualValue = selectionSettings.StrokeWidth;

            Assert.Equal(2.0, actualValue);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(2.0)]
        [InlineData(4.0)]
        public void SelectionSettings_StrokeWidth_SetAndGet(double expectedValue)
        {
            SunburstSelectionSettings selectionSettings = new();

            selectionSettings.StrokeWidth = expectedValue;

            Assert.Equal(expectedValue, selectionSettings.StrokeWidth);
        }

        [Fact]
        public void SelectionSettings_SelectionBrush_DefaultValue()
        {
            SunburstSelectionSettings selectionSettings = new();

            var brush = selectionSettings.Fill as SolidColorBrush;

            Assert.NotNull(brush);
            Assert.Equal(Color.FromArgb("1C1B1F"), brush.Color);
        }

        [Fact]
        public void SelectionSettings_SelectionStroke_DefaultValue()
        {
            SunburstSelectionSettings selectionSettings = new();

            var brush = selectionSettings.Stroke as SolidColorBrush;

            Assert.NotNull(brush);
            Assert.Equal(Color.FromArgb("#1C1B1F"), brush.Color);
        }

        #endregion

        #region SunburstSegment Selection State Tests

        [Fact]
        public void SunburstSegment_IsSelected_DefaultValue()
        {
            SunburstSegment segment = new();

            Assert.False(segment.IsSelected);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SunburstSegment_IsSelected_SetAndGet(bool expectedValue)
        {
            SunburstSegment segment = new();

            // Use Reflection to set the internal property.
            var isSelectedProperty = typeof(SunburstSegment).GetProperty("IsSelected",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            isSelectedProperty?.SetValue(segment, expectedValue);

            var actualValue = isSelectedProperty?.GetValue(segment);
            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region SfSunburstChart Selection Properties

        [Fact]
        public void SfSunburstChart_SelectionSettings_SetAndGet()
        {
            SfSunburstChart chart = new();
            SunburstSelectionSettings settings = new()
            {
                Type = SunburstSelectionType.Child
            };

            chart.SelectionSettings = settings;

            Assert.Equal(settings, chart.SelectionSettings);
            Assert.Equal(SunburstSelectionType.Child, chart.SelectionSettings.Type);
        }

        [Fact]
        public void SfSunburstChart_SelectedSunburstItems_DefaultValue()
        {
            SfSunburstChart chart = new();

            Assert.NotNull(chart.SelectedSunburstItems);
            Assert.Empty(chart.SelectedSunburstItems);
        }

        [Fact]
        public void SfSunburstChart_SelectionChangedEvent_HandlerCanBeAddedAndRemoved()
        {
            SfSunburstChart chart = new();
            EventHandler<SunburstSelectionChangedEventArgs> handler = (s, e) => { };

            chart.SelectionChanged += handler;

            chart.SelectionChanged -= handler;
        }


        [Fact]
        public void SunburstSelectionChangedEventArgs_Properties()
        {
            SunburstSegment segment = new();
            bool isSelected = true;

            SunburstSelectionChangedEventArgs args = new(segment, segment, isSelected);

            Assert.Equal(segment, args.OldSegment);
            Assert.Equal(segment, args.NewSegment);
            Assert.Equal(isSelected, args.IsSelected);
        }

        #endregion

        #region SfSunburstChart Private Method Tests

        [Fact]
        public void OnTapAction_WithSelectionDisabled_DoesNotSelectSegment()
        {
            SfSunburstChart sunburstChart = new();
            Point tapPoint = new(100, 100);
            bool selectionChanged = false;
            
            sunburstChart.SelectionChanged += (s, e) => selectionChanged = true;

            InvokePrivateMethod(sunburstChart, "OnTapAction", sunburstChart, tapPoint, 1);

            Assert.False(selectionChanged);
            Assert.Empty(sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void GetSelectedSegment_WithPointOutsideSegments_ReturnsNull()
        {
            SfSunburstChart sunburstChart = new();

            // Use the two-parameter overload by specifying the exact parameter types
            var getSelectedSegmentMethod = typeof(SfSunburstChart).GetMethod(
                "GetSelectedSegment", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new Type[] { typeof(float), typeof(float) },
                null);
            
            Assert.NotNull(getSelectedSegmentMethod);
            var result = getSelectedSegmentMethod.Invoke(sunburstChart, new object[] { 100000f, 100000f });

            Assert.Null(result);
        }

        [Fact]
        public void IsSegmentSelected_WithEmptySegment_ReturnsFalse()
        {
            SfSunburstChart sunburstChart = new();

            // Act
            // Pass a new segment that's not part of the chart
            SunburstSegment emptySegment = new();
            
            var result = InvokePrivateMethod(sunburstChart, "IsSegmentSelected", new object[] { emptySegment });
            
            // Verify result is not null before casting
            Assert.NotNull(result);
            Assert.False(result is bool value && value);
        }

        [Fact]
        public void SelectSegment_WithUnrelatedSegment_DoesNotModifySelectedSegments()
        {
            SfSunburstChart sunburstChart = new();
            int initialCount = sunburstChart.SelectedSunburstItems.Count;
            bool selectionChanged = false;

            sunburstChart.SelectionChanged += (s, e) => selectionChanged = true;

            // Create a segment but disable selection to ensure it doesn't get selected
            var unrelatedSegment = new SunburstSegment();

            // The selection is disabled so it should not be selectable
            InvokePrivateMethod(sunburstChart, "SelectSegments", new object[] { unrelatedSegment });

            Assert.Equal(initialCount, sunburstChart.SelectedSunburstItems.Count);
            Assert.False(selectionChanged);
        }

        [Fact]
        public void SelectSegment_WithSelectionDisabled_DoesNotSelectSegment()
        {
            SfSunburstChart sunburstChart = new();
            SunburstSegment segment = new();
            bool selectionChanged = false;
            
            sunburstChart.SelectionChanged += (s, e) => selectionChanged = true;
            
            InvokePrivateMethod(sunburstChart, "SelectSegments", segment);
            
            Assert.Empty(sunburstChart.SelectedSunburstItems);
            Assert.False(selectionChanged);
        }

        [Fact]
        public void SelectSegment_WithAlreadySelectedSegment_DoesNothing()
        {
            SfSunburstChart sunburstChart = new();
            SunburstItem sunburstItem = new();
            SunburstSegment segment = new();

            sunburstChart.SelectedSunburstItems.Add(sunburstItem);

            bool selectionChanged = false;
            sunburstChart.SelectionChanged += (s, e) => selectionChanged = true;

            InvokePrivateMethod(sunburstChart, "SelectSegments", segment);

            // Since the SelectSegments method is called without proper setup, no actual changes should occur
            Assert.False(selectionChanged); 
        }

        [Fact]
        public void ClearSelection_WithNoSelectedSegments_DoesNothing()
        {
            SfSunburstChart sunburstChart = new();
            bool selectionChanged = false;
            
            sunburstChart.SelectionChanged += (s, e) => selectionChanged = true;
            
            InvokePrivateMethod(sunburstChart, "ClearSelection");
            
            Assert.Empty(sunburstChart.SelectedSunburstItems);
            Assert.False(selectionChanged);
        }

        [Fact]
        public void RaiseSelectionChanged_WhenCalled_InvokesEvent()
        {
            SfSunburstChart sunburstChart = new();
            SunburstSegment segment = new();
            bool eventRaised = false;
            SunburstSelectionChangedEventArgs? capturedArgs = null;
            
            sunburstChart.SelectionChanged += (s, e) => {
                eventRaised = true;
                capturedArgs = e;
            };
            
            InvokePrivateMethod(sunburstChart, "RaiseSelectionChanged", segment, segment, true);
            
            Assert.True(eventRaised);
            Assert.NotNull(capturedArgs);
            Assert.Equal(segment, capturedArgs.NewSegment);
            Assert.True(capturedArgs.IsSelected);
        }

        #endregion

        #region Comprehensive Selection Tests

        [Fact]
        public void ClearSelection_WithSelectedSegments_ClearsSelectionAndDoesNotRaiseEvent()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstItem item = new();
            sunburstChart.SelectedSunburstItems.Add(item);
            int eventCount = 0;
            sunburstChart.SelectionChanged += (s, e) => eventCount++;

            // Act
            InvokePrivateMethod(sunburstChart, "ClearSelection", Array.Empty<object>());

            // Assert
            Assert.Empty(sunburstChart.SelectedSunburstItems);
            Assert.Equal(0, eventCount); // ClearSelection does not raise SelectionChanged event
        }

        [Fact]
        public void ClearSelection_WithEmptySelection_DoesNothing()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            int eventCount = 0;
            sunburstChart.SelectionChanged += (s, e) => eventCount++;

            // Act
            InvokePrivateMethod(sunburstChart, "ClearSelection", Array.Empty<object>());

            // Assert
            Assert.Empty(sunburstChart.SelectedSunburstItems);
            Assert.Equal(0, eventCount);
        }

        [Fact]
        public void SelectSegments_WithValidSegment_AddsToSelection()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings { Type = SunburstSelectionType.Single };
            
            SunburstSegment segment = new();
            SunburstItem item = new() { Key = "Test" };
            segment.SunburstItems = item;

            // Act
            InvokePrivateMethod(sunburstChart, "SelectSegments", segment);

            // Assert
            Assert.Single(sunburstChart.SelectedSunburstItems);
            Assert.Contains(item, sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void SelectSegments_WithChildType_SelectsChildrenToo()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings { Type = SunburstSelectionType.Child };
            
            SunburstSegment segment = new();
            SunburstItem parentItem = new() { Key = "Parent" };
            SunburstItem childItem = new() { Key = "Child" };
            parentItem.ChildItems = new List<SunburstItem> { childItem };
            segment.SunburstItems = parentItem;

            // Act
            InvokePrivateMethod(sunburstChart, "SelectSegments", segment);

            // Assert
            Assert.Equal(2, sunburstChart.SelectedSunburstItems.Count);
            Assert.Contains(parentItem, sunburstChart.SelectedSunburstItems);
            Assert.Contains(childItem, sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void SelectSegments_WithGroupType_SelectsAllSegmentsWithSameIndex()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings { Type = SunburstSelectionType.Group };
            
            SunburstSegment segment1 = new() { Index = 1 };
            SunburstSegment segment2 = new() { Index = 1 };
            SunburstSegment segment3 = new() { Index = 2 };
            
            SunburstItem item1 = new() { Key = "Item1" };
            SunburstItem item2 = new() { Key = "Item2" };
            SunburstItem item3 = new() { Key = "Item3" };
            
            segment1.SunburstItems = item1;
            segment2.SunburstItems = item2;
            segment3.SunburstItems = item3;

            sunburstChart.Segments.Add(segment1);
            sunburstChart.Segments.Add(segment2);
            sunburstChart.Segments.Add(segment3);

            // Act
            InvokePrivateMethod(sunburstChart, "SelectSegments", segment1);

            // Assert
            Assert.Equal(2, sunburstChart.SelectedSunburstItems.Count); // 2 group items with same index
            Assert.Contains(item1, sunburstChart.SelectedSunburstItems);
            Assert.Contains(item2, sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void SelectSegments_WithParentType_SelectsParentChain()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings { Type = SunburstSelectionType.Parent };
            
            SunburstSegment grandParent = new();
            SunburstSegment parent = new();
            SunburstSegment child = new();
            
            SunburstItem grandParentItem = new() { Key = "GrandParent" };
            SunburstItem parentItem = new() { Key = "Parent" };
            SunburstItem childItem = new() { Key = "Child" };
            
            grandParent.SunburstItems = grandParentItem;
            parent.SunburstItems = parentItem;
            child.SunburstItems = childItem;
            
            child.Parent = parent;
            parent.Parent = grandParent;

            // Act
            InvokePrivateMethod(sunburstChart, "SelectSegments", child);

            // Assert
            Assert.Equal(3, sunburstChart.SelectedSunburstItems.Count);
            Assert.Contains(childItem, sunburstChart.SelectedSunburstItems);
            Assert.Contains(parentItem, sunburstChart.SelectedSunburstItems);
            Assert.Contains(grandParentItem, sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void RaiseSelectionChanged_WithValidArguments_InvokesEvent()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstSegment oldSegment = new();
            SunburstSegment newSegment = new();
            bool eventRaised = false;
            SunburstSelectionChangedEventArgs? capturedArgs = null;

            sunburstChart.SelectionChanged += (s, e) => {
                eventRaised = true;
                capturedArgs = e;
            };

            // Act
            InvokePrivateMethod(sunburstChart, "RaiseSelectionChanged", oldSegment, newSegment, true);

            // Assert
            Assert.True(eventRaised);
            Assert.NotNull(capturedArgs);
            Assert.Equal(oldSegment, capturedArgs.OldSegment);
            Assert.Equal(newSegment, capturedArgs.NewSegment);
            Assert.True(capturedArgs.IsSelected);
        }

        [Fact]
        public void RaiseSelectionChanging_WithNoHandler_ReturnsTrue()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstSegment oldSegment = new();
            SunburstSegment newSegment = new();

            // Act
            var result = InvokePrivateMethod(sunburstChart, "RaiseSelectionChanging", oldSegment, newSegment);

            // Assert
            Assert.NotNull(result);
            Assert.True((bool)result);
        }

        [Fact]
        public void RaiseSelectionChanging_WithCancellingHandler_ReturnsFalse()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstSegment oldSegment = new();
            SunburstSegment newSegment = new();

            sunburstChart.SelectionChanging += (s, e) => {
                e.Cancel = true;
            };

            // Act
            var result = InvokePrivateMethod(sunburstChart, "RaiseSelectionChanging", oldSegment, newSegment);

            // Assert
            Assert.NotNull(result);
            Assert.False((bool)result);
        }

        [Fact]
        public void RaiseSelectionChanging_WithNonCancellingHandler_ReturnsTrue()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstSegment oldSegment = new();
            SunburstSegment newSegment = new();
            bool eventRaised = false;
            SunburstSelectionChangingEventArgs? capturedArgs = null;

            sunburstChart.SelectionChanging += (s, e) => {
                eventRaised = true;
                capturedArgs = e;
            };

            // Act
            var result = InvokePrivateMethod(sunburstChart, "RaiseSelectionChanging", oldSegment, newSegment);

            // Assert
            Assert.NotNull(result);
            Assert.True((bool)result);
            Assert.True(eventRaised);
            Assert.NotNull(capturedArgs);
            Assert.Equal(oldSegment, capturedArgs.OldSegment);
            Assert.Equal(newSegment, capturedArgs.NewSegment);
            Assert.False(capturedArgs.Cancel);
        }

        [Fact]
        public void IsSegmentSelected_WithSelectedSegment_ReturnsTrue()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstSegment segment = new();
            SunburstItem item = new();
            segment.SunburstItems = item;
            segment.IsSelected = true;

            // Act
            var result = InvokePrivateMethod(sunburstChart, "IsSegmentSelected", segment);

            // Assert
            Assert.NotNull(result);
            Assert.True((bool)result);
        }

        [Fact]
        public void IsSegmentSelected_WithSegmentInSelectedCollection_ReturnsTrue()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstSegment segment = new();
            SunburstItem item = new();
            segment.SunburstItems = item;
            sunburstChart.SelectedSunburstItems.Add(item);

            // Act
            var result = InvokePrivateMethod(sunburstChart, "IsSegmentSelected", segment);

            // Assert
            Assert.NotNull(result);
            Assert.True((bool)result);
        }

        [Fact]
        public void IsSegmentSelected_WithUnselectedSegment_ReturnsFalse()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstSegment segment = new();
            SunburstItem item = new();
            segment.SunburstItems = item;

            // Act
            var result = InvokePrivateMethod(sunburstChart, "IsSegmentSelected", segment);

            // Assert
            Assert.NotNull(result);
            Assert.False((bool)result);
        }

        [Fact]
        public void GetSelectedSegment_WithValidItem_ReturnsSegment()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings(); // Add this to prevent null reference
            SunburstSegment segment = new();
            SunburstItem item = new();
            segment.SunburstItems = item;
            sunburstChart.Segments.Add(segment);

            // Use the one-parameter overload by specifying the exact parameter types
            var getSelectedSegmentMethod = typeof(SfSunburstChart).GetMethod(
                "GetSelectedSegment", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new Type[] { typeof(SunburstItem) },
                null);
            
            Assert.NotNull(getSelectedSegmentMethod);

            // Act
            var result = getSelectedSegmentMethod.Invoke(sunburstChart, new object[] { item });

            // Assert
            Assert.Equal(segment, result);
        }

        [Fact]
        public void GetSelectedSegment_WithNullItem_ReturnsNull()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();

            // Use the one-parameter overload by specifying the exact parameter types
            var getSelectedSegmentMethod = typeof(SfSunburstChart).GetMethod(
                "GetSelectedSegment",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance,
                null,
                new Type[] { typeof(SunburstItem) },
                null);

            Assert.NotNull(getSelectedSegmentMethod);

            // Act - passing null explicitly as object array
            var result = getSelectedSegmentMethod.Invoke(sunburstChart, new object?[] { null });

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ApplyHighlightByOpacity_WithSelectedSegments_SetsOpacityCorrectly()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings 
            { 
                DisplayMode = SunburstSelectionDisplayMode.HighlightByOpacity,
                Opacity = 0.5
            };

            SunburstSegment selectedSegment = new() { IsSelected = true };
            SunburstSegment unselectedSegment = new() { IsSelected = false };
            
            sunburstChart.Segments.Add(selectedSegment);
            sunburstChart.Segments.Add(unselectedSegment);
            
            SunburstItem item = new();
            sunburstChart.SelectedSunburstItems.Add(item);

            // Act
            InvokePrivateMethod(sunburstChart, "ApplyHighlightByOpacity");

            // Assert
            Assert.Equal(1.0f, selectedSegment.Opacity);
            Assert.Equal(0.5f, unselectedSegment.Opacity);
        }

        [Fact]
        public void ResetOpacity_WithNoSelection_ResetsAllOpacityToOne()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstSegment segment1 = new() { Opacity = 0.5f };
            SunburstSegment segment2 = new() { Opacity = 0.3f };
            
            sunburstChart.Segments.Add(segment1);
            sunburstChart.Segments.Add(segment2);

            // Act
            InvokePrivateMethod(sunburstChart, "ResetOpacity");

            // Assert
            Assert.Equal(1.0f, segment1.Opacity);
            Assert.Equal(1.0f, segment2.Opacity);
        }

        [Fact]
        public void SelectionInvalidate_CallsInvalidateDrawable()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            
            // Act - Should not throw exception
            var exception = Record.Exception(() => 
                InvokePrivateMethod(sunburstChart, "SelectionInvalidate"));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void SetFillColor_WithSelectedSegmentAndBrushMode_AppliesSelectionFill()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings
            {
                DisplayMode = SunburstSelectionDisplayMode.HighlightByBrush,
                Fill = new SolidColorBrush(Colors.Red)
            };
            
            SunburstSegment segment = new() { IsSelected = true, Index = 0 };

            // Act
            InvokePrivateMethod(sunburstChart, "SetFillColor", segment);

            // Assert
            Assert.Equal(sunburstChart.SelectionSettings.Fill, segment.Fill);
        }

        [Fact]
        public void SetOpacity_WithSelectedSegmentAndOpacityMode_MaintainsFullOpacity()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings
            {
                DisplayMode = SunburstSelectionDisplayMode.HighlightByOpacity,
                Opacity = 0.5
            };
            
            SunburstSegment segment = new() { IsSelected = true };

            // Act
            InvokePrivateMethod(sunburstChart, "SetOpacity", segment);

            // Assert
            Assert.Equal(1.0f, segment.Opacity);
        }

        [Fact]
        public void SetOpacity_WithUnselectedSegmentAndOpacityMode_AppliesSelectionOpacity()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings
            {
                DisplayMode = SunburstSelectionDisplayMode.HighlightByOpacity,
                Opacity = 0.3
            };
            
            SunburstSegment segment = new() { IsSelected = false };

            // Act
            InvokePrivateMethod(sunburstChart, "SetOpacity", segment);

            // Assert
            Assert.Equal(0.3f, segment.Opacity);
        }

        [Fact]
        public void SetStroke_WithSelectedSegmentAndStrokeMode_AppliesSelectionStroke()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings
            {
                DisplayMode = SunburstSelectionDisplayMode.HighlightByStroke,
                Stroke = new SolidColorBrush(Colors.Green),
                StrokeWidth = 3.0
            };
            
            SunburstSegment segment = new() { IsSelected = true };

            // Act
            InvokePrivateMethod(sunburstChart, "SetStroke", segment);

            // Assert
            Assert.Equal(sunburstChart.SelectionSettings.Stroke, segment.Stroke);
            Assert.Equal(3.0f, segment.StrokeWidth);
        }

        #endregion

        #region Selection Event Args Tests

        [Fact]
        public void SunburstSelectionChangedEventArgs_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            SunburstSegment oldSegment = new();
            SunburstSegment newSegment = new();
            bool isSelected = true;

            // Act
            SunburstSelectionChangedEventArgs args = new(oldSegment, newSegment, isSelected);

            // Assert
            Assert.Equal(oldSegment, args.OldSegment);
            Assert.Equal(newSegment, args.NewSegment);
            Assert.Equal(isSelected, args.IsSelected);
        }

        [Fact]
        public void SunburstSelectionChangingEventArgs_Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            SunburstSegment oldSegment = new();
            SunburstSegment newSegment = new();

            // Act
            SunburstSelectionChangingEventArgs args = new(oldSegment, newSegment);

            // Assert
            Assert.Equal(oldSegment, args.OldSegment);
            Assert.Equal(newSegment, args.NewSegment);
            Assert.False(args.Cancel); // Default should be false
        }

        [Fact]
        public void SunburstSelectionChangingEventArgs_Cancel_CanBeSetToTrue()
        {
            // Arrange
            SunburstSegment oldSegment = new();
            SunburstSegment newSegment = new();
            SunburstSelectionChangingEventArgs args = new(oldSegment, newSegment);

            // Act
            args.Cancel = true;

            // Assert
            Assert.True(args.Cancel);
        }

        #endregion

        #region Selection Settings Integration Tests

        [Fact]
        public void SelectionSettings_PropertyChanged_ClearsExistingSelection()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstItem item = new();
            sunburstChart.SelectedSunburstItems.Add(item);
            
            var settings = new SunburstSelectionSettings();
            sunburstChart.SelectionSettings = settings;

            // Act
            settings.Type = SunburstSelectionType.Child;

            // Assert
            Assert.Empty(sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void SelectionSettings_DisplayModeChange_ClearsExistingSelection()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstItem item = new();
            sunburstChart.SelectedSunburstItems.Add(item);
            
            var settings = new SunburstSelectionSettings();
            sunburstChart.SelectionSettings = settings;

            // Act
            settings.DisplayMode = SunburstSelectionDisplayMode.HighlightByOpacity;

            // Assert
            Assert.Empty(sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void OnTapAction_WithValidTapAndSelectionEnabled_ProcessesSelection()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            sunburstChart.SelectionSettings = new SunburstSelectionSettings();
            Point tapPoint = new(100, 100);
            bool selectionEventRaised = false;
            
            sunburstChart.SelectionChanged += (s, e) => selectionEventRaised = true;

            // Act
            InvokePrivateMethod(sunburstChart, "OnTapAction", sunburstChart, tapPoint, 1);

            // Assert - Should not throw and should process tap
            // Note: Without actual segments at tap point, no selection will occur
            Assert.False(selectionEventRaised); // No segments to select
        }

        #endregion

        #region Segment Collection Tests

        [Fact]
        public void SelectedSunburstItems_AddItem_TriggersCollectionChanged()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            bool collectionChangedRaised = false;
            SunburstItem item = new();

            sunburstChart.SelectedSunburstItems.CollectionChanged += (s, e) => collectionChangedRaised = true;

            // Act
            sunburstChart.SelectedSunburstItems.Add(item);

            // Assert
            Assert.True(collectionChangedRaised);
            Assert.Contains(item, sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void SelectedSunburstItems_RemoveItem_TriggersCollectionChanged()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstItem item = new();
            sunburstChart.SelectedSunburstItems.Add(item);
            bool collectionChangedRaised = false;

            sunburstChart.SelectedSunburstItems.CollectionChanged += (s, e) => collectionChangedRaised = true;

            // Act
            sunburstChart.SelectedSunburstItems.Remove(item);

            // Assert
            Assert.True(collectionChangedRaised);
            Assert.DoesNotContain(item, sunburstChart.SelectedSunburstItems);
        }

        [Fact]
        public void SelectedSunburstItems_Clear_TriggersCollectionChanged()
        {
            // Arrange
            SfSunburstChart sunburstChart = new();
            SunburstItem item = new();
            sunburstChart.SelectedSunburstItems.Add(item);
            bool collectionChangedRaised = false;

            sunburstChart.SelectedSunburstItems.CollectionChanged += (s, e) => collectionChangedRaised = true;

            // Act
            sunburstChart.SelectedSunburstItems.Clear();

            // Assert
            Assert.True(collectionChangedRaised);
            Assert.Empty(sunburstChart.SelectedSunburstItems);
        }

        #endregion


    }
}

