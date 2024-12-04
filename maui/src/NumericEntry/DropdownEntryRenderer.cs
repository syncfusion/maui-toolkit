namespace Syncfusion.Maui.Toolkit.EntryRenderer
{
	/// <summary>
	/// The FluentSfEntryRenderer class is responsible for rendering the entry
	/// with a Fluent design style. It implements the IUpDownButtonRenderer interface, providing
	/// methods to render the dropdown, set its options, and specify the selected option.
	/// </summary>
	internal class FluentSfEntryRenderer : IUpDownButtonRenderer
    {
        /// <summary>
        /// Padding value for circles.
        /// </summary>
        internal float _padding = 13f;

        /// <summary>
        /// Padding value specifically for circles.
        /// </summary>
        internal float _circlePadding = 2f;

		/// <summary>
		/// Draws the border.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the border's bounds.</param>
		/// <param name="isFocused">Indicates if the dropdown is focused.</param>
		/// <param name="borderColor">The color of the border.</param>
		/// <param name="focusedBorderColor">The color of the border when focused.</param>
		public void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor)
        {
            canvas.StrokeColor = Color.FromRgba(240, 240, 240, 255);
            canvas.StrokeSize = 2;
            canvas.DrawRoundedRectangle(rectF, 5);
            canvas.ResetStroke();

            if (isFocused)
            {
                canvas.StrokeColor = focusedBorderColor;
                canvas.DrawLine(rectF.X + 2, rectF.Y + rectF.Height - 1.5f, rectF.X + rectF.Width - 2, rectF.Height - 1.5f);
            }
            else
            {
                canvas.StrokeColor = borderColor;
                canvas.DrawLine(rectF.X + 2, rectF.Y + rectF.Height - 1, rectF.X + rectF.Width - 2, rectF.Height - 1);
            }
            canvas.StrokeSize = 0.6f;
            canvas.DrawLine(rectF.X + 3, rectF.Y + rectF.Height - 0.6f, rectF.X + rectF.Width - 3, rectF.Height - 0.6f);
            canvas.DrawLine(rectF.X + 4, rectF.Y + rectF.Height, rectF.X + rectF.Width - 4, rectF.Height);

            canvas.StrokeSize = 1f;
        }

		/// <summary>
		/// Draws the clear button.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		public void DrawClearButton(ICanvas canvas, RectF rectF)
        {
            PointF A = new Point(0, 0);
            PointF B = new Point(0, 0);
            PointF C = new Point(0, 0);
            PointF D = new Point(0, 0);

            A.X = rectF.X + _padding;
            A.Y = rectF.Y + _padding;

            B.X = rectF.X + rectF.Width - _padding;
            B.Y = rectF.Y + rectF.Height - _padding;

            canvas.DrawLine(A, B);

            C.X = rectF.X + _padding;
            C.Y = rectF.Y + rectF.Height - _padding;

            D.X = rectF.X + rectF.Width - _padding;
            D.Y = rectF.Y + _padding;

            canvas.DrawLine(C, D);
            canvas.DrawCircle((A.X + D.X) / 2, (B.Y + D.Y) / 2, (float)Math.Ceiling((Math.Sqrt(2d) * (D.X - A.X)) / 2f) + _circlePadding);
        }

		/// <summary>
		/// Draws the down button
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		public void DrawDownButton(ICanvas canvas, RectF rectF)
        {
            var imageWidth = (rectF.Width / 2) - (_padding / 2);
            var imageHeight = (rectF.Height / 2) - (_padding / 2);

            rectF.X = rectF.Center.X - imageWidth / 2;
            rectF.Y = rectF.Center.Y - imageHeight / 4;
            rectF.Width = imageWidth;
            rectF.Height = imageHeight / 2;

            float x = rectF.X;
            float y = rectF.Y;
            float width = x + rectF.Width;
            float height = y + rectF.Height;
            float midWidth = x + (rectF.Width / 2);

            var path = new PathF();
            path.MoveTo(x, y);
            path.LineTo(midWidth, height);
            path.LineTo(width, y);
            path.LineTo(x, y);
            path.Close();
            canvas.FillPath(path);

        }

		/// <summary>
		/// Draws the up button of the dropdown.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		public void DrawUpButton(ICanvas canvas, RectF rectF) {
            var imageSize = (rectF.Width / 2) - (_padding / 2);
            rectF.X = rectF.Center.X - imageSize / 2;
            rectF.Y = rectF.Center.Y - imageSize / 4;
            rectF.Width = imageSize;
            rectF.Height = imageSize / 2;

            float x = rectF.X;
            float y = rectF.Y;
            float width = x + rectF.Width;
            float height = y + rectF.Height;
            float midWidth = x + (rectF.Width / 2);

            var path = new PathF();
            path.MoveTo(x, height);
            path.LineTo(midWidth, y);
            path.LineTo(width, height);
            path.LineTo(x, height);
            path.Close();
            canvas.FillPath(path);
        }
    }

	/// <summary>
	/// The CupertinoSfEntryRenderer class is responsible for rendering the entry
	/// with a Cupertino design style. It implements the IUpDownButtonRenderer interface, providing
	/// methods to render the dropdown, set its options, and specify the selected option.
	/// </summary>
	internal class CupertinoSfEntryRenderer : IUpDownButtonRenderer
    {
        /// <summary>
        /// General padding value between the elements.
        /// </summary>
        internal float _padding = 12;

        /// <summary>
        /// Padding value specifically for the clear button.
        /// </summary>
        internal float _clearButtonPadding = 12;

        /// <summary>
        /// Padding value for circular elements.
        /// </summary>
        internal float _circlePadding = 2;

        /// <summary>
        /// Size of the border stroke, set to zero by default.
        /// </summary>
        internal float _borderStrokeSize = 0;

		/// <summary>
		/// Draws the border.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the border's bounds.</param>
		/// <param name="isFocused">Indicates if the dropdown is focused.</param>
		/// <param name="borderColor">The color of the border.</param>
		/// <param name="focusedBorderColor">The color of the border when focused.</param>
		public void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor)
        {
            if (!isFocused)
            {
                canvas.StrokeColor = borderColor;
                canvas.StrokeSize = 1;
            }
            else
            {
#if MACCATALYST
                canvas.StrokeColor = focusedBorderColor;
                canvas.StrokeSize = _borderStrokeSize == 0 ? 6 : _borderStrokeSize;
#else
                canvas.StrokeColor = borderColor;
                canvas.StrokeSize = 1;
#endif
            }

            canvas.SaveState();
            canvas.DrawRoundedRectangle(rectF, 6);
            canvas.ResetStroke();
        }

		/// <summary>
		/// Draws the clear button.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		public void DrawClearButton(ICanvas canvas, RectF rectF)
        {
            PointF A = new Point(0, 0);
            PointF B = new Point(0, 0);
            PointF C = new Point(0, 0);
            PointF D = new Point(0, 0);

            A.X = rectF.X + _clearButtonPadding;
            A.Y = rectF.Y + _clearButtonPadding;

            B.X = rectF.X + rectF.Width - _clearButtonPadding;
            B.Y = rectF.Y + rectF.Height - _clearButtonPadding;

            canvas.DrawLine(A, B);

            C.X = rectF.X + _clearButtonPadding;
            C.Y = rectF.Y + rectF.Height - _clearButtonPadding;

            D.X = rectF.X + rectF.Width - _clearButtonPadding;
            D.Y = rectF.Y + _clearButtonPadding;

            canvas.DrawLine(C, D);
            canvas.DrawCircle((A.X + D.X) / 2, (B.Y + D.Y) / 2, (float)Math.Ceiling((Math.Sqrt(2d) * (D.X - A.X)) / 2f) + _circlePadding);
        }

		/// <summary>
		/// Draws the dropdown button of the dropdown.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		public void DrawDownButton(ICanvas canvas, RectF rectF)
        {
            var imageWidth = (rectF.Width / 2) - (_padding / 2);
            var imageHeight = (rectF.Height / 2) - (_padding / 2);

            rectF.X = rectF.Center.X - imageWidth / 2;
            rectF.Y = rectF.Center.Y - imageHeight / 4;
            rectF.Width = imageWidth;
            rectF.Height = imageHeight / 2;

            float x = rectF.X;
            float y = rectF.Y;
            float width = x + rectF.Width;
            float height = y + rectF.Height;
            float midWidth = x + (rectF.Width / 2);

            var path = new PathF();
            path.MoveTo(x, y);
            path.LineTo(midWidth, height);
            path.LineTo(width, y);
            path.LineTo(x, y);
            path.Close();
            canvas.FillPath(path);
        }

		/// <summary>
		/// Draws the up button of the dropdown.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		public void DrawUpButton(ICanvas canvas, RectF rectF) {
            var imageSize = (rectF.Width / 2) - (_padding / 2);
            rectF.X = rectF.Center.X - imageSize / 2;
            rectF.Y = rectF.Center.Y - imageSize / 4;
            rectF.Width = imageSize;
            rectF.Height = imageSize / 2;

            float x = rectF.X;
            float y = rectF.Y + rectF.Height;
            float width = x + rectF.Width;
            float height = y - rectF.Height;
            float midWidth = x + (rectF.Width / 2);

            var path = new PathF();
            path.MoveTo(x, y);
            path.LineTo(midWidth, height);
            path.LineTo(width, y);
            path.LineTo(x, y);
            path.Close();
            canvas.FillPath(path);
        }
    }

	/// <summary>
	/// The MaterialSfEntryRenderer class is responsible for rendering the entry
	/// with a Material design style. It implements the IUpDownButtonRenderer interface, providing
	/// methods to render the dropdown, set its options, and specify the selected option.
	/// </summary>
	internal class MaterialSfEntryRenderer : IUpDownButtonRenderer
    {
       /// <summary>
        /// General padding value between the elements.
        /// </summary>
        internal float _padding = 6;

        /// <summary>
        /// Padding value specifically for the clear button.
        /// </summary>
        internal float _clearButtonPadding = 10;

        /// <summary>
        /// Padding value for circular elements.
        /// </summary>
        internal float _circlePadding = 2;

		/// <summary>
		/// Draws the border.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the border's bounds.</param>
		/// <param name="isFocused">Indicates if the dropdown is focused.</param>
		/// <param name="borderColor">The color of the border.</param>
		/// <param name="focusedBorderColor">The color of the border when focused.</param>
		public void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor)
        {
            if (isFocused)
            {
                canvas.StrokeColor = focusedBorderColor;
                canvas.StrokeSize = 2;
            }
            else
            {
                canvas.StrokeSize = 1;
                canvas.StrokeColor = borderColor;
            }

            canvas.DrawLine(rectF.X + 4, rectF.Y + rectF.Height - 7, rectF.X + rectF.Width - 4, rectF.Height - 7);
            canvas.StrokeSize = 1;

        }

		/// <summary>
		/// Draws the clear button.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		public void DrawClearButton(ICanvas canvas, RectF rectF)
        {
            PointF A = new Point(0, 0);
            PointF B = new Point(0, 0);
            PointF C = new Point(0, 0);
            PointF D = new Point(0, 0);

            A.X = rectF.X + _clearButtonPadding;
            A.Y = rectF.Y + _clearButtonPadding;

            B.X = rectF.X + rectF.Width - _clearButtonPadding;
            B.Y = rectF.Y + rectF.Height - _clearButtonPadding;

            canvas.DrawLine(A, B);

            C.X = rectF.X + _clearButtonPadding;
            C.Y = rectF.Y + rectF.Height - _clearButtonPadding;

            D.X = rectF.X + rectF.Width - _clearButtonPadding;
            D.Y = rectF.Y + _clearButtonPadding;

            canvas.DrawLine(C, D);
            canvas.DrawCircle((A.X + D.X) / 2, (B.Y + D.Y) / 2, (float)Math.Ceiling((Math.Sqrt(2d) * (D.X - A.X)) / 2f) + _circlePadding);
        }

		/// <summary>
		/// Draws the down button of the entry.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		public void DrawDownButton(ICanvas canvas, RectF rectF)
        {
            // Adjust the rect width and height values to match the Material 3 design measurements using default width (32) and height (30) values for Android platform.
            float defaultWidth = 32f;
            float defaultHeight = 30f;
            
            float widthFactor = defaultWidth / _padding;
            float heightFactor = defaultHeight / _padding;

            float widthPadding = rectF.Width / widthFactor;
            float heightPadding = rectF.Height / heightFactor;

            rectF.X = rectF.Center.X - widthPadding;
            rectF.Y = rectF.Center.Y - (heightPadding / 2);
            rectF.Width = (widthPadding * 2);
            rectF.Height = heightPadding;

            float x = rectF.X;
            float y = rectF.Y;
            float width = x + rectF.Width;
            float height = y + rectF.Height;
            float midWidth = x + (rectF.Width / 2);

            var path = new PathF();
            path.MoveTo(x, y);
            path.LineTo(width, y);
            path.LineTo(midWidth, height);
            path.LineTo(x, y);
            path.Close();
            canvas.FillPath(path);

        }

		/// <summary>
		/// Draws the up button of the entry.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		public void DrawUpButton(ICanvas canvas, RectF rectF) {
            rectF.X = rectF.Center.X - _padding;
            rectF.Y = rectF.Center.Y - (_padding / 2);
            rectF.Width = (_padding * 2);
            rectF.Height = _padding;

            float x = rectF.X;
            float y = rectF.Y + rectF.Height;
            float width = x + rectF.Width;
            float height = y - rectF.Height;
            float midWidth = x + (rectF.Width / 2);

            var path = new PathF();
            path.MoveTo(x, y);
            path.LineTo(width, y);
            path.LineTo(midWidth, height);
            path.LineTo(x, y);
            path.Close();
            canvas.FillPath(path);
        }
    }

	/// <summary>
	/// The IUpDownButtonRenderer interface defines methods for rendering various components
	/// of a entry. Implementations of this interface are responsible for drawing
	/// the up button, down button, clear button, and border of the entry.
	/// </summary>
	internal interface IUpDownButtonRenderer
    {
		/// <summary>
		/// Draws the up button of the dropdown.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		void DrawUpButton(ICanvas canvas, RectF rectF);

		/// <summary>
		/// Draws the down button of the dropdown.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		void DrawDownButton(ICanvas canvas, RectF rectF);

		/// <summary>
		/// Draws the clear button.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the button's bounds.</param>
		void DrawClearButton(ICanvas canvas, RectF rectF);

		/// <summary>
		/// Draws the border.
		/// </summary>
		/// <param name="canvas">The canvas on which to draw.</param>
		/// <param name="rectF">The rectangle defining the border's bounds.</param>
		/// <param name="isFocused">Indicates if the dropdown is focused.</param>
		/// <param name="borderColor">The color of the border.</param>
		/// <param name="focusedBorderColor">The color of the border when focused.</param>
		void DrawBorder(ICanvas canvas, RectF rectF, bool isFocused, Color borderColor, Color focusedBorderColor);
    }

}
