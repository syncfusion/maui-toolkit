using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal class CartesianAxisRenderer
	{
		#region Fields
		readonly ChartAxis _chartAxis;

		internal List<ILayoutCalculator> LayoutCalculators { get; set; }
		#endregion

		#region Constructor
		public CartesianAxisRenderer(ChartAxis axis)
		{
			LayoutCalculators = [];
			_chartAxis = axis;
		}
		#endregion

		#region Internal Methods
		internal SizeF ComputeDesiredSize(SizeF availableSize)
		{
			double width = 0;
			double height = 0;
			ChartAxisTitle title = _chartAxis.Title;
			double horizontalPadding = 0;
			double verticalPadding = 0;

			SizeF size;
			foreach (ILayoutCalculator layout in LayoutCalculators)
			{
				layout.SetLeft(0);
				layout.SetTop(0);
				layout.Measure(availableSize);
				var desiredSize = layout.GetDesiredSize();

				if (layout is CartesianAxisLabelsRenderer && _chartAxis.LabelsPosition == AxisElementPosition.Inside)
				{
					horizontalPadding += desiredSize.Width;
					verticalPadding += desiredSize.Height;
				}

				if (layout is CartesianAxisElementRenderer && _chartAxis.TickPosition == AxisElementPosition.Inside)
				{
					horizontalPadding += desiredSize.Width - _chartAxis.AxisLineStyle.StrokeWidth;
					verticalPadding += desiredSize.Height - _chartAxis.AxisLineStyle.StrokeWidth;
				}

				width += desiredSize.Width;
				height += desiredSize.Height;
			}

			if (title != null && !string.IsNullOrEmpty(title.Text))
			{
				title.Measure();
				SizeF titleSize = title.GetDesiredSize();
				width += titleSize.Width;
				height += titleSize.Height;
			}

			if (_chartAxis.IsVertical)
			{
				_chartAxis.InsidePadding = horizontalPadding;
				size = new Size(width, availableSize.Height);
			}
			else
			{
				_chartAxis.InsidePadding = verticalPadding;
				size = new Size(availableSize.Width, height);
			}

			return size;
		}

		internal void UpdateRendererVisible(bool visible)
		{
			foreach (ILayoutCalculator layout in LayoutCalculators)
			{
				if (layout is not CartesianAxisLabelsRenderer)
				{
					layout.IsVisible = visible;
				}
			}
		}

		internal void OnDraw(ICanvas canvas)
		{
			foreach (ILayoutCalculator layout in LayoutCalculators)
			{
				if (layout.IsVisible)
				{
					canvas.CanvasSaveState();
					canvas.Translate((float)layout.GetLeft(), (float)layout.GetTop());
					layout.OnDraw(canvas, layout.GetDesiredSize());
					canvas.CanvasRestoreState();
				}
			}

			ChartAxisTitle title = _chartAxis.Title;
			if (title == null || string.IsNullOrEmpty(title.Text))
			{
				return;
			}

			title.Draw(canvas);
		}

		internal void Layout(SizeF size)
		{
			ILayoutCalculator labelsRenderer = LayoutCalculators[0];
			ILayoutCalculator elementsRender = LayoutCalculators[1];

			List<object> elements = [];
			List<SizeF> sizes = [];
			bool isVertical = _chartAxis.IsVertical;
			bool isInversed = _chartAxis.IsOpposed() ^ isVertical;

			if (_chartAxis.TickPosition == AxisElementPosition.Inside)
			{
				elements.Insert(0, elementsRender);
				sizes.Insert(0, elementsRender.GetDesiredSize());
			}
			else
			{
				elements.Add(elementsRender);
				sizes.Add(elementsRender.GetDesiredSize());
			}

			if (_chartAxis.LabelsPosition == AxisElementPosition.Inside)
			{
				elements.Insert(0, labelsRenderer);
				sizes.Insert(0, labelsRenderer.GetDesiredSize());
			}
			else
			{
				elements.Add(labelsRenderer);
				sizes.Add(labelsRenderer.GetDesiredSize());
			}

			ChartAxisTitle title = _chartAxis.Title;

			if (title != null && !string.IsNullOrEmpty(title.Text))
			{
				elements.Add(title);
				sizes.Add(title.GetDesiredSize());
			}

			if (isInversed)
			{
				elements.Reverse();
				sizes.Reverse();
			}

			float currentPosition = 0;

			for (int i = 0; i < elements.Count; i++)
			{
				object element = elements[i];

				if (isVertical)
				{
					if (element == title)
					{
						title.Top = size.Height / 2;

#if !ANDROID
						if (_chartAxis.IsOpposed())
						{
							currentPosition += sizes[i].Width;
						}
#endif

						title.Left = currentPosition;
					}
					else
					{
						ILayoutCalculator layout = (ILayoutCalculator)element;
						layout.SetLeft(currentPosition);
					}

					currentPosition += sizes[i].Width;
				}
				else
				{
					if (element == title)
					{
						title.Left = size.Width / 2;
						title.Top = currentPosition;
					}
					else
					{
						ILayoutCalculator layout = (ILayoutCalculator)element;
						layout.SetTop(currentPosition);
					}

					currentPosition += sizes[i].Height;
				}
			}
		}
		#endregion
	}
}
