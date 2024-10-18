using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.TabView
{
	/// <summary>
	/// Represents an arrow icon used as a scroll button in the <see cref="SfTabView"/> control
	/// </summary>
	internal class ArrowIcon : SfView, ITouchListener
    {
        #region Fields

        internal RectF _backgroundRectF = new RectF();
        internal SfEffectsView? _sfEffectsView;
        bool _isPressed = false;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="ForegroundColor"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ForegroundColorProperty =
          BindableProperty.Create(
              nameof(ForegroundColor),
              typeof(Color),
              typeof(ArrowIcon),
              Color.FromArgb("#49454F"),
              propertyChanged: OnScrollButtonForegroundColorChanged);

        /// <summary>
        /// Identifies the <see cref="ScrollButtonBackgroundColor"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ScrollButtonBackgroundColorProperty =
          BindableProperty.Create(
              nameof(ScrollButtonBackgroundColor),
              typeof(Color),
              typeof(ArrowIcon),
              Color.FromArgb("#F7F2FB"),
              propertyChanged: OnScrollButtonBackgroundColorChanged);

        /// <summary>
        /// Identifies the <see cref="ButtonArrowType"/> bindable property.
        /// </summary>
        internal static readonly BindableProperty ButtonArrowTypeProperty =
            BindableProperty.Create(
                nameof(ButtonArrowType),
                typeof(ArrowType),
                typeof(ArrowIcon),
                null,
                propertyChanged: OnButtonArrowTypeChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that can be used to customize the scroll button’s foreground color in <see cref="SfTabView"/> .
        /// </summary>
        internal Color ForegroundColor
        {
            get => (Color)GetValue(ForegroundColorProperty);
            set => SetValue(ForegroundColorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that can be used to customize the scroll button’s background color in <see cref="SfTabView"/> .
        /// </summary>
        internal Color ScrollButtonBackgroundColor
        {
            get => (Color)GetValue(ScrollButtonBackgroundColorProperty);
            set => SetValue(ScrollButtonBackgroundColorProperty, value);
        }

        /// <summary>
        /// Gets or sets the arrow type of the scroll buttons.
        /// </summary>
        internal ArrowType ButtonArrowType
        {
            get => (ArrowType)GetValue(ButtonArrowTypeProperty);
            set => SetValue(ButtonArrowTypeProperty, value);
        }

        #endregion

        #region Events

        /// <summary>
        /// Scroll button clicked event.
        /// </summary>
        internal event EventHandler<EventArgs>? ScrollButtonClicked;

        #endregion

        #region Contructor

        internal ArrowIcon()
        {
            DrawingOrder = DrawingOrder.AboveContent;
            this.AddTouchListener(this);
            _sfEffectsView = new SfEffectsView
            {
                ClipToBounds = true,
                ShouldIgnoreTouches = true
            };
            Children.Add(_sfEffectsView);
            BackgroundColor = ScrollButtonBackgroundColor;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// This method is used to draw the forward and backward arrows.
        /// </summary>
        /// <param name="canvas">canvas.</param>
        /// <param name="dirtyRect">dirtyRect.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            base.OnDraw(canvas, dirtyRect);
            _backgroundRectF = dirtyRect;

            // Set background color and fill rectangle.
            canvas.FillColor = ScrollButtonBackgroundColor;
            canvas.FillRectangle(_backgroundRectF);

            float arrowHeight = 14;
            float arrowWidth = arrowHeight / 2;
            float centerPosition = dirtyRect.Height / 2;
            float leftPosition = (dirtyRect.Width / 2) - (arrowWidth / 2);
            float topPosition = (dirtyRect.Height / 2) - (arrowHeight / 2);
            float rightPosition = (dirtyRect.Width / 2) + (arrowWidth / 2);
            float bottomPosition = (dirtyRect.Height / 2) + (arrowHeight / 2);

            // Draw arrow.
            PathF path = new PathF();
            if (ButtonArrowType == ArrowType.Forward)
            {
                path.MoveTo(leftPosition, topPosition);
                path.LineTo(rightPosition, centerPosition);
                path.LineTo(leftPosition, bottomPosition);
            }
            else
            {
                path.MoveTo(rightPosition, topPosition);
                path.LineTo(leftPosition, centerPosition);
                path.LineTo(rightPosition, bottomPosition);
            }

            canvas.CanvasSaveState();
            canvas.StrokeSize = 1.5f;
            canvas.StrokeColor = ForegroundColor;
            canvas.DrawPath(path);
            canvas.CanvasRestoreState();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invokes <see cref="ScrollButtonClicked"/> event.
        /// </summary>
        /// <param name="args">The event args.</param>
        void RaiseScrollButtonClickedEvent(EventArgs args)
        {
            if (ScrollButtonClicked != null)
            {
                ScrollButtonClicked(this, args);
            }
        }

        #endregion

        #region Property Changed Methods

        static void OnScrollButtonForegroundColorChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as ArrowIcon)?.InvalidateDrawable();

        static void OnButtonArrowTypeChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as ArrowIcon)?.InvalidateDrawable();

        static void OnScrollButtonBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ArrowIcon arrowIcon && arrowIcon != null)
            {
                arrowIcon.InvalidateDrawable();
            }
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Handles touch events to determine if a scroll button is clicked.
        /// </summary>
        /// <param name="e">The pointer event args</param>
        public void OnTouch(PointerEventArgs e)
        {
            switch (e.Action)
            {
                case PointerActions.Pressed:
                    _isPressed = true;
                    _sfEffectsView?.ApplyEffects(SfEffects.Ripple, RippleStartPosition.Default, new System.Drawing.Point((int)e.TouchPoint.X, (int)e.TouchPoint.Y), false);
                    break;

                case PointerActions.Released:
                    if (_isPressed)
                    {
                        RaiseScrollButtonClickedEvent(new EventArgs());
                        _isPressed = false;
                        _sfEffectsView?.Reset();
                    }

                    BackgroundColor = ScrollButtonBackgroundColor;
                    break;

                case PointerActions.Entered:
                    _sfEffectsView?.ApplyEffects(SfEffects.Highlight, RippleStartPosition.Default, new System.Drawing.Point((int)e.TouchPoint.X, (int)e.TouchPoint.Y), false);
                    break;

                case PointerActions.Exited:
                    _sfEffectsView?.Reset();
                    BackgroundColor = ScrollButtonBackgroundColor;
                    break;
            }
        }

        #endregion

    }
}