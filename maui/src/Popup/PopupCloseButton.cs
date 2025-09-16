using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Helper;
using Point = Microsoft.Maui.Graphics.Point;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using PointF = Microsoft.Maui.Graphics.PointF;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// View that holds the close button in header.
	/// </summary>
	internal class PopupCloseButton : SfContentView, ITouchListener
	{
		#region Fields

		/// <summary>
		/// Gets _popupView's instance.
		/// </summary>
		internal PopupView? _popupView;

		/// <summary>
		/// Gets the close button image for popup view.
		/// </summary>
		SfImage? _closeButtonImage;

		/// <summary>
		/// Gets the width of the close button in the header.
		/// </summary>
		int _closeButtonWidth = 40;

		/// <summary>
		/// Gets the height of the close button in the header.
		/// </summary>
		int _closeButtonHeight = 40;

		/// <summary>
		/// Gets whether the mouse hover effect is add to the close button.
		/// </summary>
		bool _isHover = false;

		/// <summary>
		/// Gets whether the pressed effect is add to the close button.
		/// </summary>
		bool _isPressed = false;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="PopupCloseButton" /> class.
		/// </summary>
		public PopupCloseButton()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PopupCloseButton"/> class.
		/// </summary>
		/// <param name="popupView">The instance of _popupView.</param>
		public PopupCloseButton(PopupView popupView)
		{
			_popupView = popupView;
			Initialize();
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Raises when mouse hover on element.
		/// </summary>
		/// <param name="e">Represents the event data.</param>
		public void OnTouch(PointerEventArgs e)
		{
			if (_popupView is null || _popupView._popup is null || _popupView._popup.PopupStyle is null)
			{
				return;
			}

			if (_popupView._popup.PopupStyle.CloseButtonIcon is null)
			{
				if (e.Action is PointerActions.Entered)
				{
					_isHover = true;
					InvalidateDrawable();
				}
				else if (e.Action is PointerActions.Exited)
				{
					_isHover = false;
					InvalidateDrawable();
				}
				else if (e.Action is PointerActions.Pressed)
				{
					_isPressed = true;
					InvalidateDrawable();
				}
				else if (e.Action is PointerActions.Released)
				{
					_isPressed = false;
					_isHover = false;
					InvalidateDrawable();
				}
			}
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Update the content for Popup close button when CloseButtonIcon is not null.
		/// </summary>
		internal void UpdatePopupCloseButtonContent()
		{
			if (_popupView is null || _popupView._popup is null || _popupView._popup.PopupStyle is null || _popupView._popup.PopupStyle.CloseButtonIcon is null || _closeButtonImage is null)
			{
				return;
			}

			if (_closeButtonImage.Source != _popupView._popup.PopupStyle.CloseButtonIcon)
			{
				_closeButtonImage.IsVisible = true;
				_closeButtonImage.Source = _popupView._popup.PopupStyle.CloseButtonIcon;
				Content = _closeButtonImage;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Add the tap gesture for popup close button.
		/// </summary>
		void AddTapGestureForPopupCloseButton()
		{
			GestureRecognizers.Add(new TapGestureRecognizer() { Command = new Command(OnCloseButtonTapped) });
		}

		/// <summary>
		/// Triggered when click the close icon.
		/// </summary>
		void OnCloseButtonTapped()
		{
			if (_popupView is not null && _popupView._popup is not null)
			{
				_popupView._popup.IsOpen = false;
			}
		}

		/// <summary>
		/// Used to initialize the popup close button.
		/// </summary>
		void Initialize()
		{
			AddTapGestureForPopupCloseButton();
			DrawingOrder = DrawingOrder.BelowContent;
			_closeButtonImage = new SfImage();
			_closeButtonImage.Style = new Style(typeof(SfImage));
			WidthRequest = _closeButtonWidth;
			HeightRequest = _closeButtonHeight;
			_closeButtonImage.HeightRequest = 24;
			_closeButtonImage.WidthRequest = 24;
			_closeButtonImage.HorizontalOptions = LayoutOptions.Center;
			_closeButtonImage.VerticalOptions = LayoutOptions.Center;
			this.AddTouchListener(this);
		}

		/// <summary>
		/// Draw an hover effect for close button.
		/// </summary>
		/// <param name="canvas">The Canvas.</param>
		/// <param name="rectF">The rectangle.</param>
		void DrawHoverHighlight(ICanvas canvas, RectF rectF)
		{
			if (_popupView is not null && _popupView._popup is not null && _popupView._popup.PopupStyle is not null && _popupView._popup.ShowCloseButton && _popupView._popup.PopupStyle.CloseButtonIcon is null && _isHover)
			{
				canvas.FillColor = _popupView._popup.PopupStyle.GetHoveredCloseButtonIconBackground();
				canvas.FillCircle(rectF.Center, 20);
			}
			else
			{
				canvas.FillColor = Colors.Transparent;
				canvas.FillCircle(rectF.Center, 20);
			}
		}

		/// <summary>
		/// Draw a touch effect for close button.
		/// </summary>
		/// <param name="canvas">The Canvas.</param>
		/// <param name="rectF">The rectangle.</param>
		void DrawPressedHighlight(ICanvas canvas, RectF rectF)
		{
			if (_popupView is not null && _popupView._popup is not null && _popupView._popup.PopupStyle is not null && _popupView._popup.ShowCloseButton && _popupView._popup.PopupStyle.CloseButtonIcon is null && _isPressed)
			{
				canvas.FillColor = _popupView._popup.PopupStyle.GetPressedCloseButtonIconBackground();
				canvas.FillCircle(rectF.Center, 20);
			}
			else
			{
				canvas.FillColor = Colors.Transparent;
				canvas.FillCircle(rectF.Center, 20);
			}
		}

		#endregion

		#region Protected Override Methods

		/// <summary>
		/// Draw method.
		/// </summary>
		/// <param name="canvas">The Canvas.</param>
		/// <param name="rectF">The rectangle.</param>
		protected override void OnDraw(ICanvas canvas, RectF rectF)
		{
			rectF.Width = 14;
			rectF.Height = 14;
			rectF.X = 13;
			rectF.Y = 13;

			if (_popupView is not null && _popupView._popup.PopupStyle.CloseButtonIcon is null)
			{
				if (_closeButtonImage is not null)
				{
					_closeButtonImage.IsVisible = false;
				}

				canvas.StrokeColor = _popupView._popup.PopupStyle.GetCloseIconColor();
				canvas.StrokeSize = (float)_popupView._popup.PopupStyle.GetCloseButtonIconStrokeThickness();
				PointF firstLine = new Point(0, 0);
				PointF secondLine = new Point(0, 0);

				firstLine.X = rectF.X;
				firstLine.Y = rectF.Y;
				secondLine.X = rectF.X + rectF.Width;
				secondLine.Y = rectF.Y + rectF.Height;

				canvas.DrawLine(firstLine, secondLine);
				firstLine.X = rectF.X;
				firstLine.Y = rectF.Y + rectF.Height;

				secondLine.X = rectF.X + rectF.Width;
				secondLine.Y = rectF.Y;

				canvas.DrawLine(firstLine, secondLine);
				DrawHoverHighlight(canvas, new RectF(0, 0, _closeButtonWidth, _closeButtonHeight));
				DrawPressedHighlight(canvas, new RectF(0, 0, _closeButtonWidth, _closeButtonHeight));
			}
		}

		#endregion
	}
}