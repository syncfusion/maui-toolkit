using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Toolkit.TabView
{
	internal class TabBarBorder : SfView
	{
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="TabBarBorderColor"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty TabBarBorderColorProperty =
			BindableProperty.Create(nameof(TabBarBorderColor), typeof(Color), typeof(TabBarBorder), null, propertyChanged: TabBarBorderColorChanged);

		/// <summary>
		/// Identifies the <see cref="TabBarBorderThickness"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty TabBarBorderThicknessProperty =
			BindableProperty.Create(nameof(TabBarBorderThickness), typeof(Thickness), typeof(TabBarBorder), new Thickness(0), propertyChanged: TabBarBorderThicknessChanged);

		/// <summary>
		/// Identifies the <see cref="ShowTabBarBorder"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty ShowTabBarBorderProperty =
			BindableProperty.Create(nameof(ShowTabBarBorder), typeof(bool), typeof(TabBarBorder), false, propertyChanged: ShowTabBarBorderChanged);

		/// <summary>
		/// Identifies the <see cref="TabBarCornerRadius"/> bindable property.
		/// </summary>
		internal static readonly BindableProperty TabBarCornerRadiusProperty =
			BindableProperty.Create(nameof(TabBarCornerRadius), typeof(CornerRadius), typeof(TabBarBorder), new CornerRadius(0), propertyChanged: TabBarCornerRadiusChanged);

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="TabBarBorder"/> class.
		/// </summary>
		internal TabBarBorder()
		{
			// Set the drawing order to above content
			this.DrawingOrder = DrawingOrder.AboveContent;
			this.InputTransparent = true;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a color that can be used to customize the border color.
		/// </summary>
		internal Color? TabBarBorderColor
        {
			get => (Color?)GetValue(TabBarBorderColorProperty);
			set => SetValue(TabBarBorderColorProperty, value);
		}

		/// <summary>
		/// Gets or sets the thickness of the border with individual values for each side.
		/// </summary>
		internal Thickness TabBarBorderThickness
		{
			get => (Thickness)GetValue(TabBarBorderThicknessProperty);
			set => SetValue(TabBarBorderThicknessProperty, value);
		}

		/// <summary>
		/// Gets or sets a value indicating whether the border is visible.
		/// </summary>
		internal bool ShowTabBarBorder
		{
			get => (bool)GetValue(ShowTabBarBorderProperty);
			set => SetValue(ShowTabBarBorderProperty, value);
		}

		/// <summary>
		/// Gets or sets the corner radius of the border.
		/// </summary>
		internal CornerRadius TabBarCornerRadius
        {
			get => (CornerRadius)GetValue(TabBarCornerRadiusProperty);
			set => SetValue(TabBarCornerRadiusProperty, value);
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Draws the border when requested
		/// </summary>
		/// <param name="canvas">The canvas to draw on</param>
		/// <param name="dirtyRect">The rectangle that needs to be redrawn</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			base.OnDraw(canvas, dirtyRect);

			if (!ShowTabBarBorder)
				return;

			canvas.Antialias = true;

			if (IsUniformThickness(TabBarBorderThickness))
			{
				DrawRoundedRectangleBorder(canvas, dirtyRect, TabBarBorderThickness, TabBarCornerRadius);
			}
			else
			{
				DrawBordersWithRoundedClip(canvas, dirtyRect, TabBarBorderThickness, TabBarCornerRadius);
			}
		}

		#endregion

		#region Property Changed Handlers
		
		static void TabBarBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is TabBarBorder border)
			{
				border.TabBarBorderColor = (Color?)newValue;
				border.InvalidateDrawable();
			}
		}
		
		static void TabBarBorderThicknessChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is TabBarBorder border)
			{
				border.InvalidateDrawable();
			}
		}

		static void ShowTabBarBorderChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is TabBarBorder border)
			{
				border.InvalidateDrawable();
			}
		}

		static void TabBarCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is TabBarBorder border)
			{
				border.InvalidateDrawable();
			}
		}

		#endregion

		#region Private Methods

		private static bool IsUniformThickness(Thickness thickness)
		{
			if (thickness.Left == thickness.Top &&
				thickness.Top == thickness.Right &&
				thickness.Right == thickness.Bottom)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		static CornerRadius NormalizeCornerRadius(CornerRadius cornerRadius, RectF rect)
		{
			float maxRadius = MathF.Min(rect.Width, rect.Height) / 2f;
			double Clamp(double v) => Math.Min(v, maxRadius);
			return new CornerRadius(
				Clamp(cornerRadius.TopLeft),
				Clamp(cornerRadius.TopRight),
				Clamp(cornerRadius.BottomLeft),
				Clamp(cornerRadius.BottomRight));
		}

		static Microsoft.Maui.Graphics.PathF CreateRoundedRectPath(RectF rect, CornerRadius radius)
		{
			var path = new Microsoft.Maui.Graphics.PathF();
			float rectX = rect.X, rectY = rect.Y, width = rect.Width, height = rect.Height;

			float topLeft = (float)radius.TopLeft;
			float topRight = (float)radius.TopRight;
			float bottomLeft = (float)radius.BottomLeft;
			float bottomRight = (float)radius.BottomRight;

			path.MoveTo(rectX + topLeft, rectY);

			path.LineTo(rectX + width - topRight, rectY);
			if (topRight > 0)
				path.QuadTo(rectX + width, rectY, rectX + width, rectY + topRight);

			path.LineTo(rectX + width, rectY + height - bottomRight);
			if (bottomRight > 0)
				path.QuadTo(rectX + width, rectY + height, rectX + width - bottomRight, rectY + height);

			path.LineTo(rectX + bottomLeft, rectY + height);
			if (bottomLeft > 0)
				path.QuadTo(rectX, rectY + height, rectX, rectY + height - bottomLeft);

			path.LineTo(rectX, rectY + topLeft);
			if (topLeft > 0)
				path.QuadTo(rectX, rectY, rectX + topLeft, rectY);

			path.Close();
			return path;
		}

		void DrawRoundedRectangleBorder(ICanvas canvas, RectF rect, Thickness thickness, CornerRadius radius)
		{
			float stroke = (float)thickness.Left;
			if (stroke <= 0)
				return;

			var inset = stroke / 2f;
			var innerRect = new RectF(rect.X + inset, rect.Y + inset, rect.Width - stroke, rect.Height - stroke);

			radius = NormalizeCornerRadius(radius, innerRect);
			canvas.StrokeColor = TabBarBorderColor;
			canvas.StrokeSize = stroke;
			canvas.DrawRoundedRectangle(innerRect, (float)radius.TopLeft, (float)radius.TopRight, (float)radius.BottomLeft, (float)radius.BottomRight);
		}

		void DrawBordersWithRoundedClip(ICanvas canvas, RectF rect, Thickness thickness, CornerRadius radius)
		{
			radius = NormalizeCornerRadius(radius, rect);
			var clipPath = CreateRoundedRectPath(rect, radius);

			canvas.SaveState();
			canvas.ClipPath(clipPath);
			DrawFlatBorders(canvas, rect, thickness);
			canvas.RestoreState();
		}

		/// <summary>
		/// Draws border lines around the TabBar
		/// </summary>
		/// <param name="canvas">The canvas to draw on</param>
		/// <param name="rect">The rectangle defining the TabBar boundaries</param>
		/// <param name="thickness">The rectangle defining the TabBar boundaries</param>
		void DrawFlatBorders(ICanvas canvas, RectF rect, Thickness thickness)
		{
			// Set the border stroke color
			canvas.StrokeColor = TabBarBorderColor;

			// Draw top border
			if (thickness.Top > 0)
			{
				canvas.StrokeSize = (float)thickness.Top;
				canvas.DrawLine(0, (float)(thickness.Top / 2), rect.Width, (float)(thickness.Top / 2));
			}

			// Draw right border
			if (thickness.Right > 0)
			{
				canvas.StrokeSize = (float)thickness.Right;
				canvas.DrawLine((float)(rect.Width - thickness.Right / 2), 0, (float)(rect.Width - thickness.Right / 2), rect.Height);
			}

			// Draw bottom border
			if (thickness.Bottom > 0)
			{
				canvas.StrokeSize = (float)thickness.Bottom;
				canvas.DrawLine(0, (float)(rect.Height - thickness.Bottom / 2), rect.Width, (float)(rect.Height - thickness.Bottom / 2));
			}

			// Draw left border
			if (thickness.Left > 0)
			{
				canvas.StrokeSize = (float)thickness.Left;
				canvas.DrawLine((float)(thickness.Left / 2), 0, (float)(thickness.Left / 2), rect.Height);
			}
		}
		#endregion
	}
}
