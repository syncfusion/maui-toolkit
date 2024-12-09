using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal interface IMarkerDependent
	{
		#region Properties

		/// <summary>
		/// Gets or sets the value indicating whether to show marker for the data point.
		/// </summary>
		bool ShowMarkers { get; set; }

		/// <summary>
		/// Gets or sets the option for customizing the data marker's settings
		/// </summary>
		ChartMarkerSettings MarkerSettings { get; }

		/// <summary>
		/// Gets or sets the value indicating whether to enable animation for data marker 
		/// </summary>
		bool NeedToAnimateMarker { get; set; }

		#endregion

		#region Methods

		void DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect);

		void InvalidateDrawable();

		void OnShowMarkersChanged(bool oldValue, bool newValue)
		{
			InvalidateDrawable();
		}

		void OnDrawMarker(ICanvas canvas, ReadOnlyObservableCollection<ChartSegment> segments, RectF dirtyRect)
		{
			if (MarkerSettings.HasBorder)
			{
				canvas.StrokeSize = (float)MarkerSettings.StrokeWidth;
				canvas.StrokeColor = MarkerSettings.Stroke.ToColor();
			}

			foreach (ChartSegment segment in segments)
			{
				if (segment is IMarkerDependentSegment markerSegment)
				{
					markerSegment.DrawMarker(canvas);
				}
			}
		}

		void Setting_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			InvalidateDrawable();
		}

		#endregion
	}

	internal interface IMarkerDependentSegment
	{
		#region Methods

		void DrawMarker(ICanvas canvas);

		Rect GetMarkerRect(double markerWidth, double markerHeight, int tooltipIndex);

		#endregion
	}
}