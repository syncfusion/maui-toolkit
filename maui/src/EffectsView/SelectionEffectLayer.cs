namespace Syncfusion.Maui.Toolkit.EffectsView
{
	/// <summary>
	/// Represents the SelectionEffectLayer class.
	/// </summary>
	internal class SelectionEffectLayer
	{
		#region Fields

		/// <summary>
		/// Represents the selection transparency factor.
		/// </summary>
		const float SelectionTransparencyFactor = 0.12f;

		/// <summary>
		/// Represents the default bounds.
		/// </summary>
		Rect _selectionBounds;

		/// <summary>
		/// Represents the default selection color.
		/// </summary>
		Brush _selectionColor = new SolidColorBrush(Colors.Black);

		readonly IDrawableLayout _drawable;

		#endregion
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SelectionEffectLayer"/> class.
		/// </summary>
		/// <param name="selectionColor">The selection color.</param>
		/// <param name="drawable">The drawable.</param>
		public SelectionEffectLayer(Brush selectionColor, IDrawableLayout drawable)
		{
			_selectionColor = selectionColor;
			_drawable = drawable;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the selection effect layer width.
		/// </summary>
		internal double Width { get; set; }

		/// <summary>
		/// Gets or sets the selection effect layer height.
		/// </summary>
		internal double Height { get; set; }

		#endregion

		#region Internal Methods

		/// <summary>
		/// The draw method of SelectionEffectLayer.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		internal void DrawSelection(ICanvas canvas)
		{
			if (_selectionColor != null)
			{
				canvas.Alpha = SelectionTransparencyFactor;
				canvas.SetFillPaint(_selectionColor, _selectionBounds);
				canvas.FillRectangle(_selectionBounds);
			}
		}

		/// <summary>
		/// Update selection bounds method.
		/// </summary>
		/// <param name="width">Width property.</param>
		/// <param name="height">Height property.</param>
		/// <param name="selectionColor">SelectionColor.</param>
		internal void UpdateSelectionBounds(double width = 0, double height = 0, Brush? selectionColor = null)
		{
			selectionColor ??= new SolidColorBrush(Colors.Transparent);

			_selectionColor = selectionColor;
			_selectionBounds = new Rect(0, 0, width, height);
			_drawable.InvalidateDrawable();
		}

		#endregion
	}
}
