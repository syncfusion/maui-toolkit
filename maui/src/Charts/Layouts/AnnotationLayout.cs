using System.Net.Sockets;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Charts
{
	internal partial class AnnotationLayout : AbsoluteLayout
	{
		#region Fields

		readonly SfDrawableView _annotationDrawer;

		#endregion

		#region Constructor

		internal AnnotationLayout(SfCartesianChart chart)
		{
			_annotationDrawer = new AnnotationDrawableView(chart);
			AbsoluteLayout.SetLayoutBounds(_annotationDrawer, new Rect(0, 0, 1, 1));
			AbsoluteLayout.SetLayoutFlags(_annotationDrawer, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
			Children.Add(_annotationDrawer);
		}

		#endregion

		#region Methods

		#region Internal Methods

		internal void InvalidateDrawable()
		{
			_annotationDrawer.InvalidateDrawable();
		}

		#endregion

		#endregion
	}

	internal class AnnotationDrawableView : AnnotationDrawerView
	{
		#region Constructor

		internal AnnotationDrawableView(SfCartesianChart chart) : base(chart)
		{
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override bool IncludeAnnotation(ChartAnnotation annotation)
		{
			return annotation.ZIndex >= 0;
		}

		#endregion

		#endregion
	}

	internal class BackgroundAnnotationDrawableView : AnnotationDrawerView
	{
		#region Constructor

		internal BackgroundAnnotationDrawableView(SfCartesianChart chart) : base(chart)
		{
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected override bool IncludeAnnotation(ChartAnnotation annotation)
		{
			return annotation.ZIndex < 0;
		}

		#endregion

		#endregion
	}

	internal abstract class AnnotationDrawerView : SfDrawableView
	{
		#region Fields

		readonly SfCartesianChart _chart;

		readonly List<ChartAnnotation> _bucket = new List<ChartAnnotation>();

		#endregion

		#region Constructor

		internal AnnotationDrawerView(SfCartesianChart chart)
		{
			_chart = chart;
		}

		#endregion

		#region Methods

		#region Protected Methods

		protected abstract bool IncludeAnnotation(ChartAnnotation annotation);

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			if (_chart == null)
			{
				return;
			}

			_bucket.Clear();
			for (int i = 0; i < _chart.Annotations.Count; i++)
			{
				ChartAnnotation annotation = _chart.Annotations[i];
				if (annotation.IsVisible && IncludeAnnotation(annotation))
				{
					_bucket.Add(annotation);
				}
			}

			if (_bucket.Count == 0)
			{
				return;
			}

			for (int i = 1; i < _bucket.Count; i++)
			{
				ChartAnnotation annotation = _bucket[i];
				int zIndex = annotation.ZIndex;
				int j = i - 1;
				while (j >= 0 && _bucket[j].ZIndex > zIndex)
				{
					_bucket[j + 1] = _bucket[j];
					j--;
				}

				_bucket[j + 1] = annotation;
			}

			for (int i = 0; i < _bucket.Count; i++)
			{
				canvas.SaveState();
				_bucket[i].Draw(canvas, dirtyRect);
				canvas.RestoreState();
			}
		}

		#endregion

		#endregion
	}
}