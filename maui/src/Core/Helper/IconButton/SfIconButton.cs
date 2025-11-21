using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// Represents a class which contains information of button icons.
    /// </summary>
    internal class SfIconButton : Grid, ITouchListener
    {
        #region Fields

        /// <summary>
        /// Used to trigger whenever the tap gesture tap event triggered.
        /// </summary>
        internal Action<string>? Clicked;

        /// <summary>
        /// The show touch effect.
        /// </summary>
        bool _showTouchEffect;

        /// <summary>
        /// Holds that the view is visible or not.
        /// </summary>
        bool _visibility;

        /// <summary>
        /// Holds the effect view ripple selection shape.
        /// </summary>
        bool _isSquareSelection;

        /// <summary>
        /// Holds the icon view.
        /// </summary>
        readonly SfIconView? _iconView;

		/// <summary>
		/// Used to identify the button need to hover while released the press.
		/// </summary>
		readonly bool _isHoveringOnReleased;

		/// <summary>
		/// Holds the view which is used to clip.
		/// </summary>
		Grid clipView;

#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
		/// <summary>
		/// Holds the value to denotes the mouse cursor exited.
		/// </summary>
		bool _isExited = false;

#endif
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfIconButton"/> class.
        /// </summary>
        /// <param name="child">The child view.</param>
        /// <param name="showTouchEffect">The show touch effect.</param>
        /// <param name="isSquareSelection">The square selection.</param>
        /// <param name="isHoveringOnReleased">Used to identify the button need to hover while released the press</param>
        internal SfIconButton(View child, bool showTouchEffect = true, bool isSquareSelection = true, bool isHoveringOnReleased = true)
        {
            _showTouchEffect = showTouchEffect;
            _isSquareSelection = isSquareSelection;
            _isHoveringOnReleased = isHoveringOnReleased;
			clipView = new Grid();
			EffectsView = new SfEffectsView();
#if __IOS__
#if NET9_0
            this.IgnoreSafeArea = true;
            this.clipView.IgnoreSafeArea = true;
            this.EffectsView.IgnoreSafeArea = true;
#else
            this.SafeAreaEdges = SafeAreaEdges.None;
            this.clipView.SafeAreaEdges = SafeAreaEdges.None;
            this.EffectsView.IgnoreSafeArea = true;
#endif

#endif
            //// - TODO directly clip the parent view cause the crash in the view. So, we add the grid view for the clip purpose.
            clipView.Add(this.EffectsView);
            Add(EffectsView);
            EffectsView.Content = child;
            EffectsView.ShouldIgnoreTouches = true;
            this.AddTouchListener(this);
            _visibility = true;
            IsClippedToBounds = true;
            if (child is SfIconView icon)
            {
                _iconView = icon;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether is show touch effect or not.
        /// </summary>
        internal bool ShowTouchEffect
        {
            get
            {
                return _showTouchEffect;
            }

            set
            {
                if (value == _showTouchEffect)
                {
                    return;
                }

                _showTouchEffect = value;
                if (!_showTouchEffect)
                {
                    //// Continuous tapping triggers pressed and released events. if the view is disabled on async method of clicked event, then the continuous tapping triggers the pressed but the released event will skipped due to view disable, so the effect view effects does not cleared on pressed UI.
                    EffectsView.Reset();
                    EffectsView.Background = Brush.Transparent;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the ripple effect is square or not.
        /// </summary>
        internal bool IsSquareSelection
        {
            get
            {
                return _isSquareSelection;
            }

            set
            {
                if (value == _isSquareSelection)
                {
                    return;
                }

                _isSquareSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets the effective view.
        /// </summary>
        internal SfEffectsView EffectsView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the view is visible or not.
        /// TODO: IsVisible property breaks in 6.0.400 release.
        /// Issue link -https://github.com/dotnet/maui/issues/7507
        /// -https://github.com/dotnet/maui/issues/8044
        /// -https://github.com/dotnet/maui/issues/7482
        /// </summary>
        internal bool Visibility
        {
            get
            {
                return _visibility;
            }

            set
            {
#if !__MACCATALYST__
                IsVisible = value;
#endif
                _visibility = value;
            }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update icon style.
        /// </summary>
        /// <param name="textStyle"> The text style value.</param>
        internal void UpdateStyle(ITextElement textStyle)
        {
            var iconView = EffectsView.Content as SfIconView;
            iconView?.UpdateStyle(textStyle);
        }

        /// <summary>
        /// Method to update icon color.
        /// </summary>
        /// <param name="iconColor"> The icon color.</param>
        internal void UpdateIconColor(Color iconColor)
        {
            var iconView = EffectsView.Content as SfIconView;
            iconView?.UpdateIconColor(iconColor);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to update the clip while the ripple selection is circle.
        /// </summary>
        /// <param name="width">The width value.</param>
        /// <param name="height">The height value.</param>
        void UpdateClip(double width, double height)
        {
            if (_isSquareSelection || width < 0 || height < 0)
            {
                return;
            }

            double centerX = Math.Min(width, height) / 2;
            EllipseGeometry currentClip = new EllipseGeometry() { Center = new Point(width / 2, height / 2), RadiusX = centerX, RadiusY = centerX };
            EllipseGeometry? previousClip = null;
            if (clipView.Clip != null && clipView.Clip is EllipseGeometry)
            {
                previousClip = (EllipseGeometry)clipView.Clip;
            }

            //// If the previous and current clip values are same, then no need to update the effects view clip.
            if (previousClip != null && previousClip.Center == currentClip.Center && previousClip.RadiusX == currentClip.RadiusX && previousClip.RadiusY == currentClip.RadiusY)
            {
                return;
            }

            clipView.Clip = currentClip;
        }

#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
        /// <summary>
        /// Method sets clip for icons.
        /// </summary>
        void ApplyCornerClip()
        {
            //// Handles clip for rounded rectangle icons. This clip reset to null in Mac and iOS if we set in measure content.
            if (_iconView != null && _iconView.SelectionCornerRadius > 0)
            {
                RoundRectangleGeometry currentClip = new RoundRectangleGeometry()
                {
                    Rect = new Rect(0, 0, Width, Height),
                    CornerRadius = _iconView.SelectionCornerRadius,
                };

                RoundRectangleGeometry? previousClip = null;
                if (clipView.Clip != null && clipView.Clip is RoundRectangleGeometry previous)
                {
                    previousClip = previous;
                }

                //// If the previous and current clip values are same, then no need to update the effects view clip.
                if (previousClip != null && previousClip.CornerRadius == currentClip.CornerRadius && previousClip.Rect.Width == currentClip.Rect.Width && previousClip.Rect.Height == currentClip.Rect.Height)
                {
                    return;
                }

                EffectsView.Clip = currentClip;
            }
        }
#endif

        #endregion

        #region Override Methods

        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            //// Method to update the clip based on the isSquareSelection property
            //// If the isSquareSelection is false, circle effect is performed.
            //// If the isSquareSelection is true, square effect is performed.
            var size = base.MeasureOverride(widthConstraint, heightConstraint);
#if WINDOWS
                    Dispatcher.Dispatch(() =>
                    {
                        UpdateClip(widthConstraint, heightConstraint);
                    });
#else
            UpdateClip(widthConstraint, heightConstraint);
#endif
            return size;
        }

		/// <summary>
		/// Called when the size of the element is allocated.
		/// Updates the corner clip for Mac Catalyst and non-Android/iOS platforms.
		/// </summary>
		/// <param name="width">The width allocated to the element.</param>
		/// <param name="height">The height allocated to the element.</param>
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
			this.ApplyCornerClip();
#endif
		}

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Method invokes on touch interaction.
		/// </summary>
		/// <param name="e">The touch event args.</param>
		void ITouchListener.OnTouch(PointerEventArgs e)
        {
            if (!_showTouchEffect)
            {
                return;
            }

            if (e.Action == PointerActions.Pressed)
            {
#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
                _isExited = false;
#endif
                EffectsView.ApplyEffects();
            }
            else if (e.Action == PointerActions.Released)
            {
                EffectsView.Reset();
                var sfIconView = EffectsView.Content as SfIconView;
                if (sfIconView != null)
                {
                    Clicked?.Invoke(sfIconView.Text);
                }
#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
                //// Show effect will false when we reach min or max date view and
                //// the view is enabled because it was disabled when loading busy indicator.
                //// is exited bool used to identify the touch exited while long press,
                //// so did not need to maintain the hovering for the button.
                if (_showTouchEffect && IsEnabled && !_isExited && _isHoveringOnReleased)
                {
                    //// The hovering color is not maintained when you press and release the mouse pointer in navigation arrows.
                    EffectsView.Background = new SolidColorBrush(Colors.Black.WithAlpha(0.04f));
                }
                else
                {
                    EffectsView.Background = Brush.Transparent;
                }
#endif
            }
#if __MACCATALYST__ || (!__ANDROID__ && !__IOS__)
            else if (e.Action == PointerActions.Entered)
            {
                _isExited = false;
                ApplyCornerClip();
                EffectsView.ApplyEffects(SfEffects.Highlight);
            }
            else if (e.Action == PointerActions.Exited)
            {
                _isExited = true;
                EffectsView.Reset();
                EffectsView.Background = Brush.Transparent;
            }
#endif
            else if (e.Action == PointerActions.Cancelled)
            {
                EffectsView.Reset();
                EffectsView.Background = Brush.Transparent;
            }
        }

        #endregion
    }
}