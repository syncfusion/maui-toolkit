using Syncfusion.Maui.Toolkit.Charts;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;
using System.Globalization;
using GestureStatus = Microsoft.Maui.GestureStatus;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.SparkCharts
{
	/// <summary>
	/// Enables trackball functionality for SparkCharts, allowing users to view data point information on touch interaction.
	/// </summary>
	public class SparkChartTrackballBehavior : Element, IThemeElement
	{
		#region Fields

		private bool _isPressed;
		private bool _longPressActive;
		private bool _canAutoHideOnExit = true;
		private TooltipHelper? _tooltipHelper;
		private SfTooltip? _templateView;
		private SparkChartTrackballInfo? _previousTrackballInfo;

		// Cached tooltip position list to avoid repeated allocations on every draw call
		static readonly List<TooltipPosition> s_tooltipFallbackPositions =
		[
			TooltipPosition.Left,
			TooltipPosition.Top,
			TooltipPosition.Bottom
		];

		#endregion

		#region Bindable Properties

		/// <summary>
		/// Identifies the ShowLine bindable property.
		/// </summary>
		public static readonly BindableProperty ShowLineProperty =
			BindableProperty.Create(
				nameof(ShowLine),
				typeof(bool),
				typeof(SparkChartTrackballBehavior),
				true,
				BindingMode.Default);

		/// <summary>
		/// Identifies the ShowLabel bindable property.
		/// </summary>
		public static readonly BindableProperty ShowLabelProperty =
			BindableProperty.Create(
				nameof(ShowLabel),
				typeof(bool),
				typeof(SparkChartTrackballBehavior),
				true,
				BindingMode.Default);

		/// <summary>
		/// Identifies the LineStyle bindable property.
		/// </summary>
		public static readonly BindableProperty LineStyleProperty =
			BindableProperty.Create(
				nameof(LineStyle),
				typeof(SparkChartLineStyle),
				typeof(SparkChartTrackballBehavior),
				null,
				BindingMode.Default,
				propertyChanged: OnLineStyleChanged);

		/// <summary>
		/// Identifies the LabelStyle bindable property.
		/// </summary>
		public static readonly BindableProperty LabelStyleProperty =
			BindableProperty.Create(
				nameof(LabelStyle),
				typeof(SparkChartLabelStyle),
				typeof(SparkChartTrackballBehavior),
				null,
				BindingMode.Default,
				propertyChanged: OnLabelStyleChanged);

		/// <summary>
		/// Identifies the ActivationMode bindable property.
		/// </summary>
		internal static readonly BindableProperty ActivationModeProperty =
			BindableProperty.Create(
				nameof(ActivationMode),
				typeof(SparkChartActivationMode),
				typeof(SparkChartTrackballBehavior),
				null,
				BindingMode.Default,
				defaultValueCreator: DefaultActivationMode);

		/// <summary>
		/// Identifies the ShowMarkers bindable property.
		/// </summary>
		public static readonly BindableProperty ShowMarkersProperty =
			BindableProperty.Create(
				nameof(ShowMarkers),
				typeof(bool),
				typeof(SparkChartTrackballBehavior),
				true,
				BindingMode.Default);

		/// <summary>
		/// Identifies the MarkerSettings bindable property.
		/// </summary>
		public static readonly BindableProperty MarkerSettingsProperty =
			BindableProperty.Create(
				nameof(MarkerSettings),
				typeof(SparkChartMarkerSettings),
				typeof(SparkChartTrackballBehavior),
				null,
				BindingMode.Default,
				propertyChanged: OnMarkerSettingsChanged);

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets or sets a value indicating whether to show the trackball line.
		/// </summary>
		public bool ShowLine
		{
			get => (bool)GetValue(ShowLineProperty);
			set => SetValue(ShowLineProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show the trackball label.
		/// </summary>
		public bool ShowLabel
		{
			get => (bool)GetValue(ShowLabelProperty);
			set => SetValue(ShowLabelProperty, value);
		}

		/// <summary>
		/// Gets or sets the line style for the trackball vertical line.
		/// </summary>
		public SparkChartLineStyle LineStyle
		{
			get => (SparkChartLineStyle)GetValue(LineStyleProperty);
			set => SetValue(LineStyleProperty, value);
		}

		/// <summary>
		/// Gets or sets the label style for the trackball label.
		/// </summary>
		public SparkChartLabelStyle LabelStyle
		{
			get => (SparkChartLabelStyle)GetValue(LabelStyleProperty);
			set => SetValue(LabelStyleProperty, value);
		}

		/// <summary>
		/// Gets or sets the activation mode for the trackball (TouchMove or LongPress).
		/// </summary>
		internal SparkChartActivationMode ActivationMode
		{
			get => (SparkChartActivationMode)GetValue(ActivationModeProperty);
			set => SetValue(ActivationModeProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether to show markers for the trackball.
		/// </summary>
		public bool ShowMarkers
		{
			get => (bool)GetValue(ShowMarkersProperty);
			set => SetValue(ShowMarkersProperty, value);
		}

		/// <summary>
		/// Gets or sets the marker settings for customizing trackball markers.
		/// </summary>
		public SparkChartMarkerSettings MarkerSettings
		{
			get => (SparkChartMarkerSettings)GetValue(MarkerSettingsProperty);
			set => SetValue(MarkerSettingsProperty, value);
		}

		#endregion

		#region Internal Properties

		/// <summary>
		/// Gets or sets the parent SparkChart associated with this behavior.
		/// </summary>
		internal SfSparkChart? SparkChart { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the trackball is currently visible.
		/// </summary>
		internal bool IsVisible { get; set; }

		/// <summary>
		/// Gets the device type of the pointer (Mouse, Touch, Pen).
		/// </summary>
		internal PointerDeviceType DeviceType { get; private set; }

		/// <summary>
		/// Gets or sets the current trackball information for the nearest data point.
		/// </summary>
		internal SparkChartTrackballInfo? CurrentTrackballInfo { get; set; }

		/// <summary>
		/// Gets or sets the X position for drawing the trackball line.
		/// </summary>
		internal float TrackballLineX { get; set; }

		/// <summary>
		/// Gets the tooltip helper for drawing trackball labels.
		/// </summary>
		internal TooltipHelper TooltipHelper
		{
			get
			{
				if (_tooltipHelper == null)
				{
					_tooltipHelper = new TooltipHelper(Drawable) { Duration = int.MaxValue };
				}
				return _tooltipHelper;
			}
		}

		/// <summary>
		/// Identifies the <see cref="TrackballBackground"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TrackballBackground"/> bindable property determines the background
		/// color of the trackball.
		/// </remarks>
		internal static readonly BindableProperty TrackballBackgroundProperty = BindableProperty.Create(
			nameof(TrackballBackground),
			typeof(Brush),
			typeof(SparkChartTrackballBehavior),
			SolidColorBrush.Black,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="TrackballFontSize"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TrackballFontSize"/> bindable property determines the font size
		/// of the trackball labels.
		/// </remarks>
		internal static readonly BindableProperty TrackballFontSizeProperty = BindableProperty.Create(
			nameof(TrackballFontSize),
			typeof(double),
			typeof(SparkChartTrackballBehavior),
			14.0,
			BindingMode.Default);

		/// <summary>
		/// Identifies the <see cref="LineStroke"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LineStroke"/> bindable property determines the stroke color
		/// of the trackball line.
		/// </remarks>
		internal static readonly BindableProperty LineStrokeProperty = BindableProperty.Create(
			nameof(LineStroke),
			typeof(Brush),
			typeof(SparkChartTrackballBehavior),
			SolidColorBrush.Black,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="LineStrokeWidth"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="LineStrokeWidth"/> bindable property determines the stroke width
		/// of the trackball line.
		/// </remarks>
		internal static readonly BindableProperty LineStrokeWidthProperty = BindableProperty.Create(
			nameof(LineStrokeWidth),
			typeof(double),
			typeof(SparkChartTrackballBehavior),
			1d,
			BindingMode.Default,
			null,
			null);

		/// <summary>
		/// Identifies the <see cref="TrackballMarkerFill"/> bindable property.
		/// </summary>
		/// <remarks>
		/// The identifier for the <see cref="TrackballMarkerFill"/> bindable property determines the default fill color
		/// of the trackball marker when no series-specific fill is available.
		/// </remarks>
		internal static readonly BindableProperty TrackballMarkerFillProperty = BindableProperty.Create(
			nameof(TrackballMarkerFill),
			typeof(Brush),
			typeof(SparkChartTrackballBehavior),
			SolidColorBrush.Black,
			BindingMode.Default,
			null,
			null);

		internal Brush TrackballBackground
		{
			get { return (Brush)GetValue(TrackballBackgroundProperty); }
			set { SetValue(TrackballBackgroundProperty, value); }
		}

		internal double TrackballFontSize
		{
			get { return (double)GetValue(TrackballFontSizeProperty); }
			set { SetValue(TrackballFontSizeProperty, value); }
		}

		internal Brush LineStroke
		{
			get { return (Brush)GetValue(LineStrokeProperty); }
			set { SetValue(LineStrokeProperty, value); }
		}

		internal double LineStrokeWidth
		{
			get { return (double)GetValue(LineStrokeWidthProperty); }
			set { SetValue(LineStrokeWidthProperty, value); }
		}

		internal Brush TrackballMarkerFill
		{
			get { return (Brush)GetValue(TrackballMarkerFillProperty); }
			set { SetValue(TrackballMarkerFillProperty, value); }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SparkChartTrackballBehavior"/> class.
		/// </summary>
		public SparkChartTrackballBehavior()
		{
			ThemeElement.InitializeThemeResources(this, "SfSparkChartTheme");
			SetDynamicResource(TrackballBackgroundProperty, "SfSparkChartTrackballLabelBackground");
			SetDynamicResource(TrackballFontSizeProperty, "SfSparkChartTrackballLabelTextFontSize");
			SetDynamicResource(LineStrokeProperty, "SfSparkChartTrackballLineStroke");
			SetDynamicResource(TrackballMarkerFillProperty, "SfSparkChartTrackballMarkerFill");

			LineStyle = new SparkChartLineStyle
			{
				Stroke = LineStroke,
				StrokeWidth = LineStrokeWidth
			};

			LabelStyle = DefaultLabelStyle();
			InitializeTrackballDynamicResource();

			MarkerSettings = new SparkChartMarkerSettings
			{
				Fill = TrackballMarkerFill,
				Width = 8,
				Height = 8
			};
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Shows the trackball at the specified touch position.
		/// </summary>
		public void Show(float pointX, float pointY)
		{
			if (SparkChart == null || SparkChart.yValues.Count == 0)
			{
				return;
			}

			// Find nearest point
			int nearestIndex = FindNearestPointIndex(pointX, pointY);
			if (nearestIndex < 0)
			{
				Hide();
				return;
			}

			// Generate trackball info
			GenerateTrackballInfo(nearestIndex);

			IsVisible = true;
			SparkChart.ScheduleUpdateArea();
		}

		/// <summary>
		/// Hides the trackball.
		/// </summary>
		public void Hide()
		{
			if (!IsVisible)
			{
				return;
			}

			IsVisible = false;
			CurrentTrackballInfo = null;
			_previousTrackballInfo = null;

			// Hide template view if it exists
			if (_templateView != null)
			{
				_templateView.IsVisible = false;
			}

			SparkChart?.ScheduleUpdateArea();
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Updates the device type based on the pointer event.
		/// </summary>
		internal void UpdateDeviceType(Syncfusion.Maui.Toolkit.Internals.PointerEventArgs e)
		{
			DeviceType = e.PointerDeviceType;
		}

		/// <summary>
		/// Handles touch down events.
		/// </summary>
		internal void OnTouchDown(SfSparkChart chart, float pointX, float pointY)
		{
#if WINDOWS || MACCATALYST
			if (ActivationMode == SparkChartActivationMode.TouchMove && DeviceType == PointerDeviceType.Touch)
#elif IOS || ANDROID
			if (ActivationMode == SparkChartActivationMode.TouchMove)
#endif
			{
				_isPressed = true;
				Show(pointX, pointY);
			}

			if (!_isPressed)
			{
				Hide();
			}
		}

		/// <summary>
		/// Handles touch move events.
		/// </summary>
		internal void OnTouchMove(SfSparkChart chart, float pointX, float pointY)
		{
			if (ActivationMode == SparkChartActivationMode.TouchMove)
			{
				_canAutoHideOnExit = true;
				Show(pointX, pointY);
			}
			else if (ActivationMode == SparkChartActivationMode.LongPress)
			{
				_canAutoHideOnExit = true;

				if (_longPressActive)
				{
					Show(pointX, pointY);
				}
			}
		}

		/// <summary>
		/// Handles touch up events.
		/// </summary>
		internal void OnTouchUp(SfSparkChart chart, float pointX, float pointY)
		{
			_isPressed = false;
			_longPressActive = false;

			if (_canAutoHideOnExit)
			{
				Hide();
			}
		}

		/// <summary>
		/// Handles touch cancel events.
		/// </summary>
		internal void OnTouchCancel(SfSparkChart chart, float pointX, float pointY)
		{
			_isPressed = false;
			_longPressActive = false;
			Hide();
		}

		/// <summary>
		/// Handles touch exit events.
		/// </summary>
		internal void OnTouchExit(SfSparkChart chart, float pointX, float pointY)
		{
			if (_canAutoHideOnExit)
			{
				Hide();
			}
		}

		/// <summary>
		/// Handles long press activation.
		/// </summary>
		internal void OnLongPressActivation(SfSparkChart chart, float pointX, float pointY, GestureStatus status)
		{
			if ((status != GestureStatus.Completed && status != GestureStatus.Canceled) && ActivationMode == SparkChartActivationMode.LongPress)
			{
				_longPressActive = true;
				Show(pointX, pointY);
			}
		}

		/// <summary>
		/// Draws the trackball elements (line, label).
		/// </summary>
		internal void DrawElements(ICanvas canvas, RectF dirtyRect)
		{
			if (!IsVisible || CurrentTrackballInfo == null || SparkChart == null)
			{
				return;
			}

			// Draw trackball line
			if (ShowLine && LineStyle != null)
			{
				DrawTrackballLine(canvas, dirtyRect);
			}

			// Draw trackball marker first (behind label)
			if (ShowMarkers && MarkerSettings != null)
			{
				DrawTrackballMarker(canvas, dirtyRect);
			}

			// Draw trackball label with tooltip helper (with nose pointer)
			if (ShowLabel && LabelStyle != null && !string.IsNullOrEmpty(CurrentTrackballInfo.Label))
			{
				DrawTrackballLabel(canvas, dirtyRect);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Finds the nearest point index to the touch position.
		/// </summary>
		private int FindNearestPointIndex(float touchX, float touchY)
		{
			if (SparkChart == null)
			{
				return -1;
			}

			int nearestIndex = -1;
			double nearPointX = double.MaxValue;
			double delta = 0;

			var rect = SparkChart.Bounds;
			rect = SparkChart.GetTranslatedRect(rect);

			// Check if chart is column or win-loss type (column-based charts)
			bool isColumnBasedChart = SparkChart is SfSparkColumnChart || SparkChart is SfSparkWinLossChart;

			if (isColumnBasedChart)
			{
				// For column-based charts, use slot-based hit testing
				float slotWidth = (float)(rect.Width / SparkChart.DataCount);
				
				for (int i = 0; i < SparkChart.yValues.Count; i++)
				{
					// Skip invalid points
					if (double.IsNaN(SparkChart.yValues[i]) || SparkChart.GapIndexes.Contains(i))
					{
						continue;
					}

					// Calculate the column bounds
					float columnLeft = (i * slotWidth) + (float)rect.X;
					float columnRight = columnLeft + slotWidth;

					// Check if touch is within this column's slot
					if (touchX >= columnLeft && touchX <= columnRight)
					{
						double y = SparkChart.yValues[i];

						if (!double.IsNaN(SparkChart.MinimumYValue) &&
							y < SparkChart.MinimumYValue)
						{
							return -1;
						}

						if (!double.IsNaN(SparkChart.MaximumYValue) &&
							y > SparkChart.MaximumYValue)
						{
							return -1;
						}

						nearestIndex = i;
						break;
					}
				}
			}
			else
			{
				// For line/area charts, use point-based hit testing
				for (int i = 0; i < SparkChart.yValues.Count; i++)
				{
					// Transform to screen coordinates
					PointF screenPoint = SparkChart.TransformToVisible(SparkChart.xValues[i], SparkChart.yValues[i], rect);

					// Add translation offset
					double currX = screenPoint.X + rect.X;

					double leftBoundary = double.NegativeInfinity;
					if (i > 0)
					{
						PointF prevPoint = SparkChart.TransformToVisible(
							SparkChart.xValues[i - 1],
							SparkChart.yValues[i - 1],
							rect);

						leftBoundary = ((prevPoint.X + screenPoint.X) / 2.0) + rect.X;
					}

					double rightBoundary = double.PositiveInfinity;
					if (i < SparkChart.yValues.Count - 1)
					{
						PointF nextPoint = SparkChart.TransformToVisible(
							SparkChart.xValues[i + 1],
							SparkChart.yValues[i + 1],
							rect);

						rightBoundary = ((screenPoint.X + nextPoint.X) / 2.0) + rect.X;
					}

					if (touchX >= leftBoundary && touchX <= rightBoundary)
					{
						double y = SparkChart.yValues[i];

						// If this owning point is outside Y-axis range,
						// trackball must NOT be shown (Cartesian behavior)
						if (!double.IsNaN(SparkChart.MinimumYValue) &&
							y < SparkChart.MinimumYValue)
						{
							return -1;
						}

						if (!double.IsNaN(SparkChart.MaximumYValue) &&
							y > SparkChart.MaximumYValue)
						{
							return -1;
						}
					}

					// Check if this point has the same delta (same X position) as the previous nearest point
					if (delta == touchX - currX)
					{
						// Multiple points at the same X position - keep the first one found
						// This matches the behavior in CartesianSeries.FindNearestChartPoints
						continue;
					}
					// Check if this point is closer to the touch position based on X-axis distance
					else if (Math.Abs(touchX - currX) <= Math.Abs(touchX - nearPointX))
					{
						// Update the nearest point based on X-axis proximity
						nearPointX = currX;
						delta = touchX - currX;
						nearestIndex = i;
					}
				}
			}

			return nearestIndex;
		}

		/// <summary>
		/// Generates trackball information for the specified data point index.
		/// </summary>
		private void GenerateTrackballInfo(int index)
		{
			if (SparkChart == null || index < 0 || index >= SparkChart.yValues.Count)
			{
				return;
			}

			double xValue = SparkChart.xValues[index];
			double yValue = SparkChart.yValues[index];

			var rect = SparkChart.Bounds;
			rect = SparkChart.GetTranslatedRect(rect);

			PointF screenPoint = SparkChart.TransformToVisible(xValue, yValue, rect);
			float trackballX = screenPoint.X;

			// Get marker fill based on chart type
			Brush? fill = null;

			// Calculate screen position and fill color based on chart type
			if (SparkChart is SfSparkColumnChart columnChart)
			{
				// For column chart, position trackball at the center of the column
				float slotWidth = (float)(rect.Width / SparkChart.DataCount);
				float columnWidth = slotWidth * 0.8f;
				float columnLeft = (index * slotWidth) + (slotWidth - columnWidth) / 2;
				float columnCenterX = columnLeft + (columnWidth / 2);

				// Get the top of the column for marker positioning
				screenPoint = new PointF(columnCenterX, screenPoint.Y);
				trackballX = columnCenterX;

				// Get the fill color for this column
				fill = columnChart.GetFillPaint(index, yValue, GetColumnChartBaseLine(columnChart));
			}
			else if (SparkChart is SfSparkAreaChart areaChart)
			{
				trackballX = screenPoint.X;
				fill = areaChart.GetMarkerFill(index);
			}
			else if (SparkChart is SfSparkLineChart lineChart)
			{
				trackballX = screenPoint.X;
				fill = lineChart.GetMarkerFill(index, yValue);
			}

			// Get the data item from ItemsSource
			object? dataItem = GetDataItem(index);

			// Create trackball info
			CurrentTrackballInfo = new SparkChartTrackballInfo()
			{
				X = screenPoint.X,
				Y = screenPoint.Y,
				Index = index,
				XValue = xValue,
				YValue = yValue,
				Label = FormatLabel(index, yValue),
				Fill = fill ?? SparkChart.Stroke,
				DataItem = dataItem
			};

			TrackballLineX = trackballX;

			// Raise the TrackballCreated event
			SparkChart.RaiseTrackballCreatedEvent(CurrentTrackballInfo);
		}

		/// <summary>
		/// Gets the data item from the ItemsSource at the specified index.
		/// </summary>
		private object? GetDataItem(int index)
		{
			if (SparkChart?.ItemsSource == null || index < 0)
			{
				return null;
			}

			if (SparkChart.ItemsSource is System.Collections.IList list)
			{
				if (index < list.Count)
				{
					return list[index];
				}
			}
			else if (SparkChart.ItemsSource is System.Collections.IEnumerable enumerable)
			{
				int currentIndex = 0;
				foreach (var item in enumerable)
				{
					if (currentIndex == index)
					{
						return item;
					}
					currentIndex++;
				}
			}

			return null;
		}

		/// <summary>
		/// Formats the label text based on axis type and label format.
		/// </summary>
		private string FormatLabel(int index, double yValue)
		{
			if (SparkChart == null)
			{
				return string.Empty;
			}

			string labelFormat = LabelStyle?.LabelFormat ?? string.Empty;
			string label;

			switch (SparkChart.AxisType)
			{
				case SparkChartAxisType.DateTime:
					label = GetXAxisLabel(index, labelFormat);
					break;

				case SparkChartAxisType.Category:
					// For Category axis, format the yValue (data value) as a number
					// Use GetActualLabelContent for consistency with CategoryAxis number formatting
					label = GetXAxisLabel(index, labelFormat);
					break;

				case SparkChartAxisType.Numeric:
				default:
					// Match ChartTrackballBehavior's NumericalAxis formatting
					label = GetYValueLabel(yValue, labelFormat);
					break;
			}

			return label;
		}

		/// <summary>
		/// Gets the formatted Y value label.
		/// </summary>
		private string GetYValueLabel(double yValue, string labelFormat)
		{
			if (string.IsNullOrEmpty(labelFormat) || labelFormat == "#.##")
			{
				return yValue == 0 ? yValue.ToString("0.##") : yValue.ToString("#.##");
			}
			else
			{
				return yValue.ToString(labelFormat);
			}
		}

		/// <summary>
		/// Gets the formatted X axis label based on axis type.
		/// </summary>
		private string GetXAxisLabel(int index, string labelFormat)
		{
			if(SparkChart == null)
			{
				return string.Empty;
			}

			if(index < 0 || index >= SparkChart.actualXValues.Count)
			{
				return index.ToString();
			}

			object? actualXValue = SparkChart.actualXValues[index];
			double xValue = SparkChart.xValues[index];

			if(SparkChart.AxisType == SparkChartAxisType.DateTime)
			{
				if (actualXValue != null)
				{
					double oaDate = ConvertToOADate(actualXValue);
					if (!double.IsNaN(oaDate))
					{
						string format = (string.IsNullOrEmpty(labelFormat)) 
										? Charts.ChartAxis.GetSpecificFormattedLabel(Charts.DateTimeIntervalType.Auto)
										: labelFormat;
						return Charts.ChartAxis.GetFormattedAxisLabel(format, oaDate);
					}
				}
				return DateTime.FromOADate(xValue).ToString("d");
			}
			else
			{
				// Return the actual category label
				return actualXValue?.ToString() ?? index.ToString();
			}
		}

		/// <summary>
		/// Converts an object to OLE Automation date.
		/// </summary>
		private double ConvertToOADate(object value)
		{
			if (value is DateTime dateTime)
			{
				return dateTime.ToOADate();
			}
			else if (value is double d)
			{
				return d;
			}
			else
			{
				try
				{
					return Convert.ToDouble(value);
				}
				catch
				{
					return double.NaN;
				}
			}
		}

		/// <summary>
		/// Draws the vertical trackball line.
		/// </summary>
		private void DrawTrackballLine(ICanvas canvas, RectF dirtyRect)
		{
			if (LineStyle == null || SparkChart == null)
			{
				return;
			}

			canvas.SaveState();

			canvas.StrokeSize = (float)LineStyle.StrokeWidth;

			if (LineStyle.Stroke != null)
			{
				canvas.StrokeColor = LineStyle.Stroke.ToColor();
			}

			if (LineStyle.StrokeDashArray != null && LineStyle.StrokeDashArray.Count > 0)
			{
				canvas.StrokeDashPattern = LineStyle.StrokeDashArray.ToFloatArray();
			}

			var rect = SparkChart.GetTranslatedRect(SparkChart.Bounds);
			
			// Note: Canvas is already translated in the derived chart's OnDraw, 
			// so we don't add rect.X offset here
			float lineX = TrackballLineX;

			// Draw line from top to bottom of the chart area
			canvas.DrawLine(lineX, 0, lineX, (float)rect.Height);

			canvas.RestoreState();
		}

		/// <summary>
		/// Draws the trackball label using TooltipHelper (with nose pointer) or template.
		/// </summary>
		private void DrawTrackballLabel(ICanvas canvas, RectF dirtyRect)
		{
			if (CurrentTrackballInfo == null || SparkChart == null)
			{
				return;
			}

			canvas.SaveState();

			var rect = SparkChart.GetTranslatedRect(SparkChart.Bounds);

			// Check if we should use a template
			if (HasTrackballTemplate())
			{
				DrawTrackballLabelWithTemplate(rect);
			}
			else if (LabelStyle != null && !string.IsNullOrEmpty(CurrentTrackballInfo.Label))
			{
				DrawTrackballLabelDefault(canvas, rect);
			}

			canvas.RestoreState();
		}

		/// <summary>
		/// Draws the trackball label using the default TooltipHelper.
		/// </summary>
		private void DrawTrackballLabelDefault(ICanvas canvas, Rect rect)
		{
			if (LabelStyle == null || CurrentTrackballInfo == null)
			{
				return;
			}

			// Configure tooltip helper
			MapLabelStyleToTooltipHelper(TooltipHelper, LabelStyle);
			TooltipHelper.Text = CurrentTrackballInfo.Label;
			
			// Set preferred position to right (similar to ChartTrackballBehavior)
			TooltipHelper.PriorityPosition = TooltipPosition.Right;
			TooltipHelper.PriorityPositionList = s_tooltipFallbackPositions;

			// Create target rect around the marker point (small rect for nose to point to)
			float markerWidth = MarkerSettings != null ? (float)MarkerSettings.Width : 8f;
			float markerHeight = MarkerSettings != null ? (float)MarkerSettings.Height : 8f;
			
			Rect targetRect = new Rect(
				CurrentTrackballInfo.X - (markerWidth / 2),
				CurrentTrackballInfo.Y - (markerHeight / 2),
				markerWidth,
				markerHeight
			);

			// Show tooltip with nose pointer
			TooltipHelper.Show(rect, targetRect, false);
			
			// Draw the tooltip
			TooltipHelper.Draw(canvas);
		}

		/// <summary>
		/// Draws the trackball label using a custom template.
		/// </summary>
		private void DrawTrackballLabelWithTemplate(Rect rect)
		{
			if (SparkChart == null || CurrentTrackballInfo == null)
			{
				return;
			}

			var templateView = GetOrCreateTemplateView();
			if (templateView == null)
			{
				return;
			}

			// Update template content if data item changed
			if (CurrentTrackballInfo.DataItem != _previousTrackballInfo?.DataItem)
			{
				templateView.BindingContext = CurrentTrackballInfo;
				templateView.Content = GetTrackballTemplateView(SparkChart.TrackballLabelTemplate, CurrentTrackballInfo);
				SetTemplatePosition(templateView);
			}

			// Create target rect around the marker point
			float markerWidth = MarkerSettings != null ? (float)MarkerSettings.Width : 8f;
			float markerHeight = MarkerSettings != null ? (float)MarkerSettings.Height : 8f;

			// Plot rect (includes padding offsets)
			var plotRect = SparkChart.GetTranslatedRect(SparkChart.Bounds);
			var padding = SparkChart.Padding;

			// Bounds for tooltip positioning should be the full chart area in parent coordinates.
			var boundsRect = new Rect(0, 0, SparkChart.Bounds.Width, SparkChart.Bounds.Height);

			// CurrentTrackballInfo.X/Y are in plot-space (0..plotWidth/Height)
			// Convert to parent-space by adding plotRect.X/plotRect.Y
			// Adding a predefined offset to center the tooltip nose position with the marker
			var targetRect = new Rect(
				padding.Left + CurrentTrackballInfo.X + 4 - (markerWidth / 2),
				padding.Top + CurrentTrackballInfo.Y + 4 - (markerHeight / 2),
				markerWidth,
				markerHeight);

			// Show the template view
			templateView.IsVisible = true;
			templateView.Show(boundsRect, targetRect, false);

			// Store current info for next comparison
			_previousTrackballInfo = CurrentTrackballInfo;
		}

		/// <summary>
		/// Sets the position preferences for the template view.
		/// </summary>
		private static void SetTemplatePosition(SfTooltip templateView)
		{
			templateView.Helper.PriorityPosition = TooltipPosition.Right;
			templateView.Helper.PriorityPositionList = s_tooltipFallbackPositions;
		}

		/// <summary>
		/// Maps SparkChartLabelStyle properties to TooltipHelper.
		/// </summary>
		private void MapLabelStyleToTooltipHelper(TooltipHelper helper, SparkChartLabelStyle labelStyle)
		{
			helper.FontAttributes = labelStyle.FontAttributes;
			helper.FontFamily = labelStyle.FontFamily;
			helper.FontSize = labelStyle.FontSize;
			helper.Padding = labelStyle.Margin;
			helper.Stroke = labelStyle.Stroke;
			helper.StrokeWidth = (float)labelStyle.StrokeWidth;
			helper.Background = labelStyle.Background;
			helper.TextColor = labelStyle.TextColor;
			helper.Font = ((ITextElement)labelStyle).Font;
		}

		/// <summary>
		/// Draws the trackball marker at the selected point.
		/// </summary>
		private void DrawTrackballMarker(ICanvas canvas, RectF dirtyRect)
		{
			if (MarkerSettings == null || CurrentTrackballInfo == null || SparkChart == null)
			{
				return;
			}

			canvas.SaveState();

			float markerWidth = (float)MarkerSettings.Width;
			float markerHeight = (float)MarkerSettings.Height;

			// Calculate marker rectangle centered on the point
			RectF markerRect = new RectF(
				CurrentTrackballInfo.X - (markerWidth / 2),
				CurrentTrackballInfo.Y - (markerHeight / 2),
				markerWidth,
				markerHeight
			);

			// Set fill paint
			var fill = MarkerSettings.Fill ?? CurrentTrackballInfo.Fill;
			canvas.SetFillPaint(fill, markerRect);

			// Set stroke for border if needed
			bool hasBorder = MarkerSettings.HasBorder;
			if (hasBorder)
			{
				canvas.StrokeSize = (float)MarkerSettings.StrokeWidth;
				canvas.StrokeColor = MarkerSettings.Stroke.ToColor();
			}

			// Convert SparkChartMarkerShape to Charts.ShapeType and draw using extension method
			Charts.ShapeType shapeType = ToInternalShapeType(MarkerSettings.ShapeType);
			canvas.DrawShape(markerRect, shapeType, hasBorder, false);

			canvas.RestoreState();
		}

		/// <summary>
		/// Converts SparkChartMarkerShape to Charts.ShapeType.
		/// </summary>
		private Charts.ShapeType ToInternalShapeType(SparkChartMarkerShape shape)
		{
			return (Charts.ShapeType)Enum.Parse(typeof(Charts.ShapeType), shape.ToString());
		}

		/// <summary>
		/// Gets the platform-specific default activation mode.
		/// </summary>
		private static object DefaultActivationMode(BindableObject bindable)
		{
#if WINDOWS || MACCATALYST
			return SparkChartActivationMode.TouchMove;
#elif ANDROID || IOS
			return SparkChartActivationMode.LongPress;
#else
			return SparkChartActivationMode.TouchMove;
#endif
		}

		/// <summary>
		/// Creates a default <see cref="SparkChartLabelStyle"/> matching constructor defaults.
		/// </summary>
		SparkChartLabelStyle DefaultLabelStyle()
		{
			return new SparkChartLabelStyle
			{
				FontSize = TrackballFontSize,
				Background = TrackballBackground,
				Margin = 5f
			};
		}

		/// <summary>
		/// Called when LineStyle property changes.
		/// </summary>
		private static void OnLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SparkChartTrackballBehavior behavior)
			{
				if (oldValue is SparkChartLineStyle oldStyle)
				{
					SetInheritedBindingContext(oldStyle, null);
				}

				if (newValue is SparkChartLineStyle newStyle)
				{
					SetInheritedBindingContext(newStyle, behavior.BindingContext);
				}
			}
		}

		/// <summary>
		/// Called when LabelStyle property changes.
		/// </summary>
		private static void OnLabelStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SparkChartTrackballBehavior behavior)
			{
				if (oldValue is SparkChartLabelStyle oldStyle)
				{
					SetInheritedBindingContext(oldStyle, null);
				}

				if (newValue is SparkChartLabelStyle newStyle)
				{
					SetInheritedBindingContext(newStyle, behavior.BindingContext);
					behavior.InitializeTrackballDynamicResource();
				}
				else
				{
					var defaultStyle = behavior.DefaultLabelStyle();
					SetInheritedBindingContext(defaultStyle, behavior.BindingContext);
					behavior.LabelStyle = defaultStyle;
				}
			}
		}

		/// <summary>
		/// Initializes dynamic theme resources for the trackball label style.
		/// </summary>
		void InitializeTrackballDynamicResource()
		{
			if (LabelStyle != null)
			{
				LabelStyle.SetDynamicResource(SparkChartLabelStyle.StrokeProperty, "SfSparkChartTrackballLabelStroke");
				LabelStyle.SetDynamicResource(SparkChartLabelStyle.TextColorProperty, "SfSparkChartTrackballLabelTextColor");
			}
		}

		/// <summary>
		/// Called when MarkerSettings property changes.
		/// </summary>
		private static void OnMarkerSettingsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SparkChartTrackballBehavior behavior)
			{
				if (oldValue is SparkChartMarkerSettings oldSettings)
				{
					SetInheritedBindingContext(oldSettings, null);
				}

				if (newValue is SparkChartMarkerSettings newSettings)
				{
					SetInheritedBindingContext(newSettings, behavior.BindingContext);
				}
			}
		}

		/// <summary>
		/// Updates binding context for nested objects.
		/// </summary>
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			if (LineStyle != null)
			{
				SetInheritedBindingContext(LineStyle, BindingContext);
			}

			if (LabelStyle != null)
			{
				SetInheritedBindingContext(LabelStyle, BindingContext);
			}

			if (MarkerSettings != null)
			{
				SetInheritedBindingContext(MarkerSettings, BindingContext);
			}
		}

		/// <summary>
		/// Drawable callback for TooltipHelper.
		/// </summary>
		private void Drawable()
		{
			// Empty callback required by TooltipHelper
		}

		/// <summary>
		/// Gets or creates a template view for the trackball label.
		/// </summary>
		private SfTooltip? GetOrCreateTemplateView()
		{
			if (SparkChart == null)
			{
				return null;
			}

			if (_templateView == null && ShowLabel)
			{
				_templateView = new SfTooltip
				{
					Background = LabelStyle?.Background ?? new SolidColorBrush(Colors.Black)
				};

				// Add to the chart's template container
				SparkChart._templateContainer.Add(_templateView);

				_templateView.Duration = double.NaN;
				_templateView.Helper.CanNosePointTarget = true;
			}

			return _templateView;
		}

		/// <summary>
		/// Creates a view from the TrackballLabelTemplate.
		/// </summary>
		private static View? GetTrackballTemplateView(DataTemplate? trackballTemplate, object bindingContext)
		{
			if (trackballTemplate != null)
			{
				return new SfItemViewCell()
				{
					ItemTemplate = trackballTemplate,
					Item = bindingContext
				};
			}

			return null;
		}

		/// <summary>
		/// Removes the template view from the chart.
		/// </summary>
		private void RemoveTemplateView()
		{
			if (_templateView != null && SparkChart != null)
			{
				SparkChart._templateContainer.Remove(_templateView);
				_templateView = null;
			}
		}

		/// <summary>
		/// Checks if the trackball label should use a template.
		/// </summary>
		private bool HasTrackballTemplate()
		{
			return SparkChart?.TrackballLabelTemplate != null;
		}

		/// <summary>
		/// Gets the baseline value for a column chart data point.
		/// </summary>
		private double GetColumnChartBaseLine(SfSparkColumnChart columnChart)
		{
			// Determine baseline for comparison
			double baseline = 0;
			if (!double.IsNaN(columnChart.AxisOrigin))
			{
				baseline = columnChart.AxisOrigin;
			}
			else if (columnChart.minYValue < 0 && columnChart.maxYValue > 0)
			{
				baseline = 0;
			}
			else if (columnChart.maxYValue <= 0)
			{
				baseline = columnChart.maxYValue;
			}
			else
			{
				baseline = columnChart.minYValue;
			}

			return baseline;
		}

		#endregion

		#region IThemeElement Implementation

		void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
		{
		}

		void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
		{
		}

		#endregion
	}
}
