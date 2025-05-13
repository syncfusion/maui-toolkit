using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class LabelStyleUnitTest
	{
		[Theory]
		[InlineData(0, 100)]
		[InlineData(-10.5, 10.5)]
		[InlineData("0", "100")] 
		[InlineData("0", 100.5)]
		public void ActualRangeChangedEventArgs_InitializesDefaultsCorrectly(object actualMinimum, object actualMaximum)
		{
			var args = new ActualRangeChangedEventArgs(actualMinimum, actualMaximum); 
			Assert.Equal(actualMinimum, args.ActualMinimum);
			Assert.Equal(actualMaximum, args.ActualMaximum);
			Assert.Null(args.VisibleMinimum);
			Assert.Null(args.VisibleMaximum);
			Assert.Null(args.Axis);
		} 

		[Fact]
		public void ChartAnnotationCollection_AddAndRemoveItems_ShouldWorkCorrectly()
		{
			var collection = new ChartAnnotationCollection();
			var annotation1 = new EllipseAnnotation();
			var annotation2 = new TextAnnotation(); 
			collection.Add(annotation1);
			collection.Add(annotation2); 
			Assert.Equal(2, collection.Count);
		}

		[Fact]
		public void ChartAnnotationLabelStyle_DefaultValues_ShouldBeCorrect()
		{
			var labelStyle = new ChartAnnotationLabelStyle(); 
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.VerticalTextAlignment);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.HorizontalTextAlignment);
			var defaultTextColor = labelStyle.GetDefaultTextColor();
			Assert.Equal("#49454F", defaultTextColor.ToArgbHex());
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.IsType<SolidColorBrush>(labelStyle.Background);
			Assert.Equal(Colors.Transparent, ((SolidColorBrush)labelStyle.Background).Color);
			Assert.Equal(new Thickness(3.5), labelStyle.Margin);
			Assert.Equal(0d, labelStyle.StrokeWidth);
		}

		[Fact]
		public void ChartAnnotationLabelStyle_DefaultValue_ShouldBeCorrect()
		{
			var labelStyle = new ChartAnnotationLabelStyle(); 
			Assert.Equal(Brush.Transparent, labelStyle.Stroke);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Equal(new CornerRadius(0), labelStyle.CornerRadius);
			var defaultBackgroundColor = labelStyle.GetDefaultBackgroundColor();
			Assert.IsType<SolidColorBrush>(defaultBackgroundColor);
			Assert.Equal(Colors.Transparent, ((SolidColorBrush)defaultBackgroundColor).Color);
		} 

		[Fact]
		public void ChartAxisLabel_DefaultConstructor_SetsPropertiesCorrectly()
		{
			double position = 10.5;
			object content = "Label"; 
			var label = new ChartAxisLabel(position, content); 
			Assert.Equal(position, label.Position);
			Assert.Equal(content, label.Content);
			Assert.True(label.IsVisible);
			var labelStyle = label.LabelStyle;
			Assert.Null(label.LabelStyle);
			Assert.Equal(0, label.RotateOriginX);
			Assert.Equal(0, label.RotateOriginY);
		}

		[Theory]
		[InlineData("Label1", 0)]
		[InlineData("Label2", 100.5)]
		[InlineData(null, -10.5)]
		[InlineData("", 50)]
		public void ChartAxisLabelEventArgs_InitializesDefaultsCorrectly(string? labelContent, double position)
		{
			var args = new ChartAxisLabelEventArgs(labelContent, position); 
			Assert.Equal(labelContent, args.Label);
			Assert.Equal(position, args.Position);
			Assert.Null(args.LabelStyle);
		}

		[Fact]
		public void ChartAxisLabelStyle_Default_SetsPropertiesCorrectly()
		{ 
			var labelStyle = new ChartAxisLabelStyle(); 
			Assert.Equal(ChartAxisLabelAlignment.Center, labelStyle.LabelAlignment);
			Assert.Equal(double.NaN, labelStyle.MaxWidth);
			Assert.Equal(ChartAxisLabelAlignment.Start, labelStyle.WrappedLabelAlignment);
			Assert.Equal(ChartTextWrapMode.WordWrap, labelStyle.TextWrapMode);
			Assert.Null(labelStyle.WrapWidthCollection);
			Assert.Equal(AxisLabelsIntersectAction.Hide, labelStyle.LabelsIntersectAction);
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.IsType<SolidColorBrush>(labelStyle.Background);
		}

		[Fact]
		public void ChartAxisLabelStyle_Defaults_SetsPropertiesCorrectly()
		{ 
			var labelStyle = new ChartAxisLabelStyle();
			Assert.Equal(Colors.Transparent, (labelStyle.Background as SolidColorBrush)?.Color);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Equal(12, labelStyle.FontSize);
			Assert.Equal(new Thickness(4), labelStyle.Margin);
			Assert.Equal(0, labelStyle.StrokeWidth);
			Assert.Equal(Brush.Transparent, labelStyle.Stroke);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Equal(new CornerRadius(0), labelStyle.CornerRadius);
		}

		[Fact]
		public void ChartAxisTickStyle_Default_SetsPropertiesCorrectly()
		{
			var tickStyle = new ChartAxisTickStyle(); 
			Assert.Equal(8d, tickStyle.TickSize);
			Assert.Equal(1d, tickStyle.StrokeWidth);
			Assert.IsType<SolidColorBrush>(tickStyle.Stroke);
			Assert.Equal(Color.FromArgb("#CAC4D0"), (tickStyle.Stroke as SolidColorBrush)?.Color);
		}

		[Fact]
		public void ChartAxisTitle_DefaultsAndProperties_AreCorrect()
		{
			var axisTitle = new ChartAxisTitle(); 
			Assert.Equal(string.Empty, axisTitle.Text);
			Assert.Equal(0f, axisTitle.Left);
			Assert.Equal(0f, axisTitle.Top);
			Assert.Null(axisTitle.Axis);
			Assert.Equal(Color.FromArgb("#49454F"), axisTitle.TextColor);
			Assert.IsType<SolidColorBrush>(axisTitle.Background);
			Assert.Equal(Colors.Transparent, (axisTitle.Background as SolidColorBrush)?.Color);
		}

		[Fact]
		public void ChartAxisTitle_DefaultsProperties_AreCorrect()
		{
			var axisTitle = new ChartAxisTitle(); 
			Assert.Equal(14, axisTitle.FontSize);
			Assert.Null(axisTitle.FontFamily);
			Assert.Equal(FontAttributes.None, axisTitle.FontAttributes);
			Assert.Equal(new Thickness(5, 12, 5, 2), axisTitle.Margin);
			Assert.Equal(0, axisTitle.StrokeWidth);
			Assert.Equal(Brush.Transparent, axisTitle.Stroke);
			Assert.Equal(string.Empty, axisTitle.LabelFormat);
			Assert.Equal(new CornerRadius(0), axisTitle.CornerRadius);
		}

		[Fact]
		public void ChartDataLabel_DefaultsAndProperties_AreCorrect()
		{
			var dataLabel = new ChartDataLabel(); 
			Assert.Equal(string.Empty, dataLabel.Label);
			Assert.Equal(Brush.Transparent, dataLabel.Background);
			Assert.Equal(-1, dataLabel.Index);
			Assert.Equal(double.NaN, dataLabel.XPosition);
			Assert.Equal(double.NaN, dataLabel.YPosition);
			Assert.Null(dataLabel.Item);
			Assert.Null(dataLabel.LabelStyle);
		}

		[Fact]
		public void ChartDataLabelStyle_DefaultsAndProperties_AreCorrect()
		{
			var labelStyle = new ChartDataLabelStyle(); 
			Assert.Equal(3d, labelStyle.LabelPadding);
			Assert.Equal(0d, labelStyle.OffsetX);
			Assert.Equal(0d, labelStyle.OffsetY);
			Assert.Equal(0d, labelStyle.Angle);
			Assert.Equal(new CornerRadius(8), labelStyle.CornerRadius);
			Assert.Equal(Colors.Transparent, labelStyle.TextColor);
			Assert.IsType<SolidColorBrush>(labelStyle.Background);
			Assert.Equal(Colors.Transparent, (labelStyle.Background as SolidColorBrush)?.Color);
			Assert.Equal(new Thickness(5), labelStyle.Margin);
			Assert.Equal(0d, labelStyle.StrokeWidth); 
		}

		[Fact]
		public void ChartDataLabelStyle_Defaults_AreCorrect()
		{
			var labelStyle = new ChartDataLabelStyle(); 
			Assert.Equal(Brush.Transparent, labelStyle.Stroke);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Equal(new CornerRadius(8), labelStyle.CornerRadius);
			Assert.False(labelStyle.IsBackgroundColorUpdated);
			Assert.False(labelStyle.IsStrokeColorUpdated);
			Assert.False(labelStyle.IsTextColorUpdated);
			Assert.True(labelStyle.HasCornerRadius);
			Assert.Equal(default(RectF), labelStyle.Rect);
		}

		[Fact]
		public void ChartLabelStyle_DefaultsAndProperties_AreCorrect()
		{
			var labelStyle = new ChartLabelStyle(); 
			Assert.Equal(Color.FromRgba(170, 170, 170, 255), labelStyle.TextColor);
			Assert.IsType<SolidColorBrush>(labelStyle.Background);
			Assert.Equal(Colors.Transparent, (labelStyle.Background as SolidColorBrush)?.Color);
			Assert.Equal(new Thickness(3.5), labelStyle.Margin);
			Assert.Equal(0d, labelStyle.StrokeWidth);
			Assert.Equal(Brush.Transparent, labelStyle.Stroke);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Equal(12d, labelStyle.FontSize);
		}

		[Fact]
		public void ChartLabelStyle_Defaults_AreCorrect()
		{
			var labelStyle = new ChartLabelStyle();
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Equal(new CornerRadius(0), labelStyle.CornerRadius); 
			Assert.False(labelStyle.IsBackgroundColorUpdated);
			Assert.False(labelStyle.IsStrokeColorUpdated);
			Assert.False(labelStyle.IsTextColorUpdated);
			Assert.False(labelStyle.HasCornerRadius);
			Assert.Equal(default(RectF), labelStyle.Rect);
		}

		[Fact]
		public void ChartLegend_DefaultProperties_AreCorrect()
		{
			var legend = new ChartLegend(); 
			Assert.True(legend.IsVisible);
			Assert.Equal(LegendPlacement.Top, legend.Placement);
			Assert.Null(legend.ItemsLayout);
			Assert.False(legend.ToggleSeriesVisibility);
			Assert.Equal(new Thickness(double.NaN), legend.ItemMargin);
			Assert.Null(legend.ItemTemplate);
			Assert.Null(legend.LabelStyle);
			Assert.Null(legend.sfLegend);
		}

		[Fact]
		public void ChartLegendLabelStyle_DefaultProperties_AreCorrect()
		{
			var chartLegendLabelStyle = new ChartLegendLabelStyle();
			var labelStyle = chartLegendLabelStyle;
			Assert.Equal(Color.FromArgb("#49454F"), labelStyle.TextColor);
			Assert.Equal(new Thickness(0), labelStyle.Margin);
			Assert.Equal(12.0, labelStyle.FontSize);
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Equal(Color.FromRgba("#611C1B1F"), labelStyle.DisableBrush.ToColor());
		}

		[Fact]
		public void ChartLineStyle_DefaultValues()
		{
			var lineStyle = new ChartLineStyle(); 
			Assert.Equal(Color.FromArgb("#CAC4D0"), lineStyle.Stroke.ToColor());
			Assert.Equal(1d, lineStyle.StrokeWidth);
			Assert.Null(lineStyle.StrokeDashArray);
		}

		[Fact]
		public void ConnectorLineStyle_DefaultConstructor_SetsDefaultValues()
		{
			var connectorLineStyle = new ConnectorLineStyle(); 
			Assert.Equal(ConnectorType.Line, connectorLineStyle.ConnectorType);
			Assert.Equal(Brush.Default, connectorLineStyle.Stroke);
			Assert.Equal(2d, connectorLineStyle.StrokeWidth);
			Assert.Null(connectorLineStyle.StrokeDashArray);
		}

		[Fact]
		public void ErrorBarLineStyle_DefaultConstructor_SetsDefaultValues()
		{
			var errorBarLineStyle = new ErrorBarLineStyle(); 
			Assert.Equal(ErrorBarStrokeCap.Flat, errorBarLineStyle.StrokeCap); 
			Assert.Equal(Brush.Black, errorBarLineStyle.Stroke);
			Assert.Equal(1d, errorBarLineStyle.StrokeWidth);
			Assert.Null(errorBarLineStyle.StrokeDashArray);
		}

		[Fact]
		public void ErrorBarCapLineStyle_DefaultConstructor_SetsDefaultValues()
		{
			var errorBarLineStyle = new ErrorBarCapLineStyle(); 
			Assert.Equal(10, errorBarLineStyle.CapLineSize);
			Assert.True(errorBarLineStyle.IsVisible);
			Assert.Equal(Brush.Black, errorBarLineStyle.Stroke);
			Assert.Equal(1d, errorBarLineStyle.StrokeWidth);
			Assert.Null(errorBarLineStyle.StrokeDashArray);
		}

		[Fact]
		public void ChartMarkerSettings_DefaultConstructor_SetsDefaultValues()
		{
			var markerSettings = new ChartMarkerSettings(); 
			Assert.Equal(Syncfusion.Maui.Toolkit.Charts.ShapeType.Circle, markerSettings.Type);
			Assert.Null(markerSettings.Fill);
			Assert.Null(markerSettings.Stroke);
			Assert.Equal(0d, markerSettings.StrokeWidth);
			Assert.Equal(8d, markerSettings.Width);
			Assert.Equal(8d, markerSettings.Height);
			Assert.False(markerSettings.HasBorder);

		}

		[Fact]
		public void ChartPlotBandLabelStyle_DefaultConstructor_SetsDefaultValues()
		{
			var labelStyle = new ChartPlotBandLabelStyle(); 
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.HorizontalTextAlignment);
			Assert.Equal(ChartLabelAlignment.Center, labelStyle.VerticalTextAlignment);
			Assert.Equal(0.0d, labelStyle.Angle);
			Assert.Equal(0.0d, labelStyle.OffsetX);
			Assert.Equal(0.0d, labelStyle.OffsetY);
			Assert.Equal(Color.FromRgba(170, 170, 170, 255), labelStyle.TextColor);
			Assert.IsType<SolidColorBrush>(labelStyle.Background);
			Assert.Equal(Colors.Transparent, (labelStyle.Background as SolidColorBrush)?.Color);
			Assert.Equal(new Thickness(3.5), labelStyle.Margin);
			Assert.Equal(0d, labelStyle.StrokeWidth);
			Assert.Equal(Brush.Transparent, labelStyle.Stroke);
		}

		[Fact]
		public void ChartPlotBandLabelStyle_Default_SetsDefaultValues()
		{
			var labelStyle = new ChartPlotBandLabelStyle(); 
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Equal(12d, labelStyle.FontSize); 
			Assert.Null(labelStyle.FontFamily);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
			Assert.Equal(new CornerRadius(0), labelStyle.CornerRadius); 
			Assert.False(labelStyle.IsBackgroundColorUpdated);
			Assert.False(labelStyle.IsStrokeColorUpdated);
			Assert.False(labelStyle.IsTextColorUpdated);
			Assert.False(labelStyle.HasCornerRadius);
			Assert.Equal(default(RectF), labelStyle.Rect);
		}

		[Theory]
		[InlineData(typeof(CategoryAxis), 1.0, 0.0)]
		[InlineData(typeof(NumericalAxis), 1.5, 0.5)]
		[InlineData(typeof(LogarithmicAxis), 2.0, 0.25)]
		[InlineData(typeof(DateTimeAxis), 1.2, 0.1)]
		public void ChartResetZoomEventArgs_InitializesCorrectly(Type axisType, double previousZoomFactor, double previousZoomPosition)
		{
			ChartAxis axis = (ChartAxis)Activator.CreateInstance(axisType)!; 
			var args = new ChartResetZoomEventArgs(axis, previousZoomFactor, previousZoomPosition); 
			Assert.Equal(axis.GetType(), args.Axis.GetType());
			Assert.Equal(previousZoomFactor, args.PreviousZoomFactor);
			Assert.Equal(previousZoomPosition, args.PreviousZoomPosition);
		}

		[Fact]
		public void CartesianDataLabelSettings_InitializesDefaultsCorrectly()
		{
			var settings = new CartesianDataLabelSettings();
			Assert.Equal(DataLabelAlignment.Top, settings.BarAlignment);
			Assert.True(settings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, settings.LabelPlacement);
			Assert.NotNull(settings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(settings.LabelStyle);
			var labelStyle = settings.LabelStyle;
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Equal(new Thickness(5), labelStyle.Margin);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes); 
		}

		[Fact]
		public void CartesianDataLabelSettings_InitializeDefaultsCorrectly()
		{
			var settings = new CartesianDataLabelSettings();
			var labelStyle = settings.LabelStyle;
			Assert.Null(labelStyle.FontFamily);
			Assert.NotNull(labelStyle.TextColor);
			Assert.Equal(Colors.Transparent, labelStyle.TextColor);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Contains("BarAlignment", settings.IsNeedDataLabelMeasure);
			Assert.Contains("LabelPlacement", settings.IsNeedDataLabelMeasure);
			Assert.Contains("LabelStyle", settings.IsNeedDataLabelMeasure);
			Assert.IsAssignableFrom<ChartDataLabelSettings>(settings); 
		} 

		[Fact]
		public void CircularDataLabelSettings_Constructor_SetsDefaultValues()
		{
			var settings = new CircularDataLabelSettings(); 
			Assert.NotNull(settings.ConnectorLineSettings);
			Assert.Equal(SmartLabelAlignment.Shift, settings.SmartLabelAlignment);
			Assert.Equal(ChartDataLabelPosition.Inside, settings.LabelPosition);
			var labelStyle = settings.LabelStyle;
			Assert.Null(labelStyle.FontFamily);
			Assert.NotNull(labelStyle.TextColor);
			Assert.Equal(Colors.Transparent, labelStyle.TextColor);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Contains("LabelPlacement", settings.IsNeedDataLabelMeasure); 
		}

		[Fact]
		public void CircularDataLabelSettings_Constructor_SetsValues()
		{
			var settings = new CircularDataLabelSettings(); 
			Assert.Equal(OverflowMode.None, settings.OverflowMode); 
			var labelStyle = settings.LabelStyle; 
			Assert.IsAssignableFrom<ChartDataLabelSettings>(settings);
			Assert.True(settings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, settings.LabelPlacement);
			Assert.NotNull(settings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(settings.LabelStyle);
			Assert.Equal(14d, labelStyle.FontSize);
			Assert.Equal(new Thickness(8, 6), labelStyle.Margin);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
		}

		[Fact]
		public void FunnelDataLabelSettings_SmartLabelAlignment_DefaultValue()
		{
			var settings = new FunnelDataLabelSettings(); 
			var defaultSmartLabelAlignment = settings.SmartLabelAlignment;
			Assert.Equal(SmartLabelAlignment.Shift, defaultSmartLabelAlignment);
			Assert.Equal(FunnelDataLabelContext.YValue, settings.Context);
			var labelStyle = settings.LabelStyle;
			Assert.Null(labelStyle.FontFamily);
			Assert.NotNull(labelStyle.TextColor);
			Assert.Equal(Colors.Transparent, labelStyle.TextColor);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Contains("LabelPlacement", settings.IsNeedDataLabelMeasure);
			Assert.Contains("LabelStyle", settings.IsNeedDataLabelMeasure); 
		}

		[Fact]
		public void FunnelDataLabelSettings_SmartLabelAlignment_Value()
		{
			var settings = new FunnelDataLabelSettings(); 
			var labelStyle = settings.LabelStyle; 
			Assert.IsAssignableFrom<ChartDataLabelSettings>(settings);
			Assert.True(settings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, settings.LabelPlacement);
			Assert.NotNull(settings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(settings.LabelStyle);
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Equal(new Thickness(6, 4), labelStyle.Margin);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
		}

		[Fact]
		public void PyramidDataLabelSettings_SmartLabelAlignment_DefaultValue()
		{
			var settings = new PyramidDataLabelSettings(); 
			var defaultSmartLabelAlignment = settings.SmartLabelAlignment;
			Assert.Equal(SmartLabelAlignment.Shift, defaultSmartLabelAlignment);
			Assert.Equal(PyramidDataLabelContext.YValue, settings.Context); 
			var labelStyle = settings.LabelStyle;
			Assert.Null(labelStyle.FontFamily);
			Assert.NotNull(labelStyle.TextColor);
			Assert.Equal(Colors.Transparent, labelStyle.TextColor);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Contains("LabelPlacement", settings.IsNeedDataLabelMeasure);
			Assert.Contains("LabelStyle", settings.IsNeedDataLabelMeasure); 
		}

		[Fact]
		public void PyramidDataLabelSettings_SmartLabelAlignment_Value()
		{
			var settings = new PyramidDataLabelSettings(); 
			var labelStyle = settings.LabelStyle; 
			Assert.IsAssignableFrom<ChartDataLabelSettings>(settings);
			Assert.True(settings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, settings.LabelPlacement);
			Assert.NotNull(settings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(settings.LabelStyle);
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Equal(new Thickness(6, 4), labelStyle.Margin);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
		}

		[Fact]
		public void PolarDataLabelSettings_SmartLabelAlignment_DefaultValue()
		{
			var settings = new PolarDataLabelSettings(); 
			var labelStyle = settings.LabelStyle;
			Assert.Null(labelStyle.FontFamily);
			Assert.NotNull(labelStyle.TextColor);
			Assert.Equal(Colors.Transparent, labelStyle.TextColor);
			Assert.Equal(string.Empty, labelStyle.LabelFormat);
			Assert.Contains("LabelPlacement", settings.IsNeedDataLabelMeasure);
			Assert.Contains("LabelStyle", settings.IsNeedDataLabelMeasure);
			Assert.IsAssignableFrom<ChartDataLabelSettings>(settings); 
		}

		[Fact]
		public void PolarDataLabelSettings_SmartLabelAlignment_Value()
		{
			var settings = new PolarDataLabelSettings(); 
			var labelStyle = settings.LabelStyle; 
			Assert.True(settings.UseSeriesPalette);
			Assert.Equal(DataLabelPlacement.Auto, settings.LabelPlacement);
			Assert.NotNull(settings.LabelStyle);
			Assert.IsType<ChartDataLabelStyle>(settings.LabelStyle);
			Assert.Equal(12d, labelStyle.FontSize);
			Assert.Equal(new Thickness(5), labelStyle.Margin);
			Assert.Equal(FontAttributes.None, labelStyle.FontAttributes);
		}
	}
}
