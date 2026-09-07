
using Syncfusion.Maui.ControlsGallery.Samples.CartesianChart.Annotation;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class ShapeAnnotationSample : SampleView
	{
		private ChartAnnotation? _selectedAnnotation;
		private Color _selectedColor = Colors.Blue;
		private readonly Dictionary<ChartAnnotation, Color> _annotationColors = new();

		public ShapeAnnotationSample()
		{
			InitializeComponent();
			verticalLine.Attach(Chart);
			horizontalLine.Attach(Chart);
			lineAnnotation.Attach(Chart);
			rectangleAnnotation.Attach(Chart);
			ellipseAnnotation.Attach(Chart);
			textAnnotation.Attach(Chart);
		}

		private void OnDragDropCheckChanged(object? sender, CheckedChangedEventArgs e)
		{
			bool isEnabled = e.Value;
			EnableDragDrop(isEnabled);
			if (isEnabled && ChangeColorRadio.IsChecked)
			{
				ChangeColorRadio.IsChecked = false;
			}
		}

		private void OnChangeColorRadioChanged(object? sender, CheckedChangedEventArgs e)
		{
			bool isEnabled = e.Value;
			ChangeColorRadio.IsChecked = isEnabled;
			if (isEnabled && DragDropCheckBox.IsChecked)
			{
				DragDropCheckBox.IsChecked = false;
			}
		}

		private void EnableDragDrop(bool isEnabled)
		{
			verticalLine.SetDragEnabled(isEnabled);
			horizontalLine.SetDragEnabled(isEnabled);
			lineAnnotation.SetDragEnabled(isEnabled);
			rectangleAnnotation.SetDragEnabled(isEnabled);
			ellipseAnnotation.SetDragEnabled(isEnabled);
			textAnnotation.SetDragEnabled(isEnabled);
		}

		private void Chart_AnnotationTapped(object? sender, AnnotationTappedEventArgs e)
		{
			if (!(ChangeColorRadio?.IsChecked ?? false))
				return;

			var annotation = e.Annotation;
			if (annotation is null)
				return;

			_selectedAnnotation = annotation;

			// Show the overlay first so the palette grid is visible/rendered
			ColorPickerOverlay.IsVisible = true;

			// Restore the last color used for this annotation after the UI is ready
			ResetColorSelectionIndicators();
			if (_annotationColors.TryGetValue(annotation, out var storedColor))
			{
				_selectedColor = storedColor;
				Dispatcher.Dispatch(() => HighlightStoredColor(storedColor));
			}
			else
			{
				// No prior color — clear any leftover selection
				_selectedColor = Colors.Blue;
			}
		}

		private void OnApplyColorClicked(object? sender, EventArgs e)
		{
			if (_selectedAnnotation != null)
				_annotationColors[_selectedAnnotation] = _selectedColor;

			ApplyColorToAnnotation();
			ColorPickerOverlay.IsVisible = false;
			_selectedAnnotation = null;
		}
		private void OnCloseTapped(object? sender, TappedEventArgs e)
		{
			ColorPickerOverlay.IsVisible = false;
			_selectedAnnotation = null;
		}
		private void ApplyColorToAnnotation()
		{
			if (_selectedAnnotation == null)
			{
				return;
			}

			Color color = _selectedColor;
			SolidColorBrush brush = new(color);
			if (_selectedAnnotation is DraggableLineAnnotation lineAnn)
			{
				lineAnn.Stroke = brush;
			}
			else if (_selectedAnnotation is DraggableHorizontalLineAnnotation hLineAnn)
			{
				hLineAnn.Stroke = brush;
			}
			else if (_selectedAnnotation is DraggableVerticalLineAnnotation vLineAnn)
			{
				vLineAnn.Stroke = brush;
			}
			else if (_selectedAnnotation is DraggableRectangleAnnotation rectAnn)
			{
				rectAnn.Fill = brush;
			}
			else if (_selectedAnnotation is DraggableEllipseAnnotation ellipseAnn)
			{
				ellipseAnn.Fill = brush;
			}
			else if (_selectedAnnotation is DraggableTextAnnotation textAnn)
			{
				textAnn.LabelStyle = new ChartAnnotationLabelStyle
				{
					TextColor = color
				};
			}
		}

		private void OnColorPickerClosed(object? sender, EventArgs e)
		{
			ColorPickerOverlay.IsVisible = false;
			_selectedAnnotation = null;
		}

		private void OnColorOptionTapped(object? sender, TappedEventArgs e)
		{
			// Get the hex color code from the command parameter
			if (e.Parameter is string hexColor)
			{
				try
				{
					_selectedColor = Color.FromArgb(hexColor);

					// Update the visual selection indicator
					ResetColorSelectionIndicators();

					// sender is the BoxView, get its parent Border
					if (sender is BoxView boxView && boxView.Parent is Border border)
					{
						border.Stroke = Colors.Black;
						border.StrokeThickness = 2;
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Error parsing color: {hexColor}, Exception: {ex.Message}");
				}
			}
		}

		private void HighlightStoredColor(Color color)
		{
			var paletteGrid = this.FindByName("ColorPaletteGrid") as Grid;
			if (paletteGrid == null)
				return;

			foreach (var child in paletteGrid.Children)
			{
				if (child is Border border && border.Content is BoxView boxView)
				{
					if (boxView.Color.ToString() == color.ToString())
					{
						border.Stroke = Colors.Black;
						border.StrokeThickness = 2;
						break;
					}
				}
			}
		}

		private void ResetColorSelectionIndicators()
		{
			var paletteGrid = this.FindByName("ColorPaletteGrid") as Grid;
			if (paletteGrid != null)
			{
				foreach (var child in paletteGrid.Children)
				{
					if (child is Border border)
					{
						border.Stroke = Colors.Transparent;
						border.StrokeThickness = 2;
					}
				}
			}
		}
	}
}
