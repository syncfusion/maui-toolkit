using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Cards
{
    /// <summary>
    /// Represents the SfCardView control that is used to display the content in a card view.
    /// </summary>
    public class SfCardView : SfContentView, IPanGestureListener, IParentThemeElement
    {
        #region Fields

        /// <summary>
        /// The initial value of x and y position while touch or swipe is occur
        /// </summary>
        Point? _touchDownPoint = null;

        /// <summary>
        /// The current position of the card view while swipe is occurred.
        /// </summary>
        double _position;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="BorderWidth"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BorderWidth"/> dependency property.
        /// </value>
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(
                nameof(BorderWidth),
                typeof(double),
                typeof(SfCardView),
                1d,
                propertyChanged: OnBorderWidthPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BorderColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BorderColor"/> dependency property.
        /// </value>
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(
                nameof(BorderColor),
                typeof(Color),
                typeof(SfCardView),
                defaultValueCreator: bindable => Color.FromArgb("#CAC4D0"),
                propertyChanged: OnBorderColorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerRadius"/> dependency property.
        /// </value>
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(SfCardView),
                new CornerRadius(5),
                propertyChanged: OnCornerRadiusPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IndicatorColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IndicatorColor"/> dependency property.
        /// </value>
        public static readonly BindableProperty IndicatorColorProperty =
            BindableProperty.Create(
                nameof(IndicatorColor),
                typeof(Color),
                typeof(SfCardView),
                defaultValueCreator: bindable => Colors.Transparent,
                propertyChanged: OnIndicatorColorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IndicatorThickness"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IndicatorThickness"/> dependency property.
        /// </value>
        public static readonly BindableProperty IndicatorThicknessProperty =
            BindableProperty.Create(
                nameof(IndicatorThickness),
                typeof(double),
                typeof(SfCardView),
                0d,
                propertyChanged: OnIndicatorThicknessPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IndicatorPosition"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IndicatorPosition"/> dependency property.
        /// </value>
        public static readonly BindableProperty IndicatorPositionProperty =
            BindableProperty.Create(
                nameof(IndicatorPosition),
                typeof(CardIndicatorPosition),
                typeof(SfCardView),
                CardIndicatorPosition.Left,
                propertyChanged: OnIndicatorPositionPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="SwipeToDismiss"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SwipeToDismiss"/> dependency property.
        /// </value>
        public static readonly BindableProperty SwipeToDismissProperty =
            BindableProperty.Create(
                nameof(SwipeToDismiss),
                typeof(bool),
                typeof(SfCardView),
                false,
                propertyChanged: OnSwipeToDismissPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="FadeOutOnSwiping"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FadeOutOnSwiping"/> dependency property.
        /// </value>
        public static readonly BindableProperty FadeOutOnSwipingProperty =
            BindableProperty.Create(
                nameof(FadeOutOnSwiping),
                typeof(bool),
                typeof(SfCardView),
                true,
                propertyChanged: OnFadeOutOnSwipingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IsDismissed"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsDismissed"/> dependency property.
        /// </value>
        public static readonly BindableProperty IsDismissedProperty =
            BindableProperty.Create(
                nameof(IsDismissed),
                typeof(bool),
                typeof(SfCardView),
                false,
                propertyChanged: OnIsDismissedPropertyChanged);

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Identifies the <see cref="CardViewBackground"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CardViewBackground"/> bindable property.
        /// </value>
        internal static readonly BindableProperty CardViewBackgroundProperty =
            BindableProperty.Create(
                nameof(CardViewBackground),
                typeof(Color),
                typeof(SfCardView),
                defaultValueCreator: bindable => Color.FromArgb("#EEE8F4"),
                propertyChanged: OnCardViewBackgroundChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfCardView"/> class.
        /// </summary>
        public SfCardView()
        {
            ThemeElement.InitializeThemeResources(this, "SfCardViewTheme");
            BackgroundColor = CardViewBackground;
            DrawingOrder = DrawingOrder.AboveContent;
            ClipToBounds = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the border width of the card view.
        /// </summary>
        /// <value>The default value of <see cref="SfCardView.BorderWidth"/> is 1d. </value>
        /// <example>
        /// The following code demonstrates, how to use the BorderWidth property in the card view.
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardView x:Name="CardView"
        ///                   BorderWidth="10">
        /// </Cards:SfCardView>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// CardView.BorderWidth = 10;
        /// ]]></code>
        /// </example>
        public double BorderWidth
        {
            get { return (double)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the border color of the card view.
        /// </summary>
        /// <value>The default value of <see cref="SfCardView.BorderColor"/> is "#CAC4D0". </value>
        /// <example>
        /// The following code demonstrates, how to use the BorderColor property in the card view.
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardView x:Name="CardView"
        ///                   BorderColor="Blue">
        /// </Cards:SfCardView>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// CardView.BorderColor = Colors.Blue;
        /// ]]></code>
        /// </example>
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the corner radius of the card view.
        /// </summary>
        /// <value>The default value of <see cref="SfCardView.CornerRadius"/> is 5. </value>
        /// <example>
        /// The following code demonstrates, how to use the CornerRadius property in the card view.
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardView x:Name="CardView"
        ///                   CornerRadius="10">
        /// </Cards:SfCardView>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// CardView.CornerRadius = 10;
        /// ]]></code>
        /// </example>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the indicator color of the card view.
        /// </summary>
        /// <value>The default value of <see cref="SfCardView.IndicatorColor"/> is Transparent. </value>
        /// <example>
        /// The following code demonstrates, how to use the IndicatorColor property in the card view.
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardView x:Name="CardView"
        ///                   IndicatorColor="Blue">
        /// </Cards:SfCardView>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// CardView.IndicatorColor = Colors.Blue;
        /// ]]></code>
        /// </example>
        public Color IndicatorColor
        {
            get { return (Color)GetValue(IndicatorColorProperty); }
            set { SetValue(IndicatorColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the indicator thickness of the card view.
        /// </summary>
        /// <value>The default value of <see cref="SfCardView.IndicatorThickness"/> is 0d. </value>
        /// <example>
        /// The following code demonstrates, how to use the IndicatorThickness property in the card view.
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardView x:Name="CardView"
        ///                   IndicatorThickness="10">
        /// </Cards:SfCardView>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// CardView.IndicatorThickness = 10;
        /// ]]></code>
        /// </example>
        public double IndicatorThickness
        {
            get { return (double)GetValue(IndicatorThicknessProperty); }
            set { SetValue(IndicatorThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the indicator position of the card view.
        /// </summary>
        /// <value>The default value of <see cref="SfCardView.IndicatorPosition"/> is CardIndicatorPosition.Left. </value>
        /// <example>
        /// The following code demonstrates, how to use the IndicatorPosition property in the card view.
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardView x:Name="CardView"
        ///                   IndicatorPosition="Right">
        /// </Cards:SfCardView>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// CardView.IndicatorPosition = CardIndicatorPosition.Right;
        /// ]]></code>
        /// </example>
        public CardIndicatorPosition IndicatorPosition
        {
            get { return (CardIndicatorPosition)GetValue(IndicatorPositionProperty); }
            set { SetValue(IndicatorPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the card view can be dismissed by swiping.
        /// </summary>
        /// <value>The default value of <see cref="SfCardView.SwipeToDismiss"/> is false. </value>
        /// <example>
        /// The following code demonstrates, how to use the SwipeToDismiss property in the card view.
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardView x:Name="CardView"
        ///                   SwipeToDismiss="True">
        /// </Cards:SfCardView>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// CardView.SwipeToDismiss = true;
        /// ]]></code>
        /// </example>
        public bool SwipeToDismiss
        {
            get { return (bool)GetValue(SwipeToDismissProperty); }
            set { SetValue(SwipeToDismissProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the card view fade out or not while swiping.
        /// </summary>
        /// <value>The default value of <see cref="SfCardView.FadeOutOnSwiping"/> is true. </value>
        /// <example>
        /// The following code demonstrates, how to use the FadeOutOnSwiping property in the card view.
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardView x:Name="CardView"
        ///                   FadeOutOnSwiping="False">
        /// </Cards:SfCardView>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// CardView.FadeOutOnSwiping = false;
        /// ]]></code>
        /// </example>
        public bool FadeOutOnSwiping
        {
            get { return (bool)GetValue(FadeOutOnSwipingProperty); }
            set { SetValue(FadeOutOnSwipingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the card view is dismissed or not.
        /// </summary>
        /// <value>The default value of <see cref="SfCardView.IsDismissed"/> is false. </value>
        /// <example>
        /// The following code demonstrates, how to use the IsDismissed property in the card view.
        /// #[XAML](#tab/tabid-11)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardView x:Name="CardView"
        ///                   IsDismissed="True">
        /// </Cards:SfCardView>
        /// ]]></code>
        /// # [C#](#tab/tabid-12)
        /// <code Lang="C#"><![CDATA[
        /// CardView.IsDismissed = true;
        /// ]]></code>
        /// </example>
        public bool IsDismissed
        {
            get { return (bool)GetValue(IsDismissedProperty); }
            set { SetValue(IsDismissedProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the background color of the card view.
        /// </summary>
        internal Color CardViewBackground
        {
            get { return (Color)GetValue(CardViewBackgroundProperty); }
            set { SetValue(CardViewBackgroundProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method invokes on pan gesture action is occur.
        /// </summary>
        /// <param name="e">Pan event argument details</param>
        public void OnPan(PanEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                _touchDownPoint = new Point(e.TouchPoint.X, e.TouchPoint.Y);
            }
            else if (e.Status == GestureStatus.Running)
            {
                if (_touchDownPoint == null)
                {
                    _touchDownPoint = new Point(e.TouchPoint.X, e.TouchPoint.Y);
                }

                //// It calculates the new position of the card view by subtracting the initial touch point from the current touch point and adding the current translation in the X-axis.
                //// This gives the updated position of the card view as it is being dragged or swiped horizontally.
                _position = e.TouchPoint.X - _touchDownPoint.Value.X + TranslationX;
                //// Create dismissing event arguments to get the dismissing event details.
                CardDismissingEventArgs dismissingEventArgs = new CardDismissingEventArgs();
                //// Determine the dismiss direction based on the swipe position.
                dismissingEventArgs.DismissDirection = _position > 0 ? CardDismissDirection.Right : CardDismissDirection.Left;
                //// Raise the dismissing event.
                Dismissing?.Invoke(this, dismissingEventArgs);
                //// If the dismissing event is canceled, the card view will not be dismissed.
                if (dismissingEventArgs.Cancel)
                {
                    _position = 0;
                    TranslationX = _position;
                    Opacity = 1;
                    return;
                }

                //// If the fade out on swiping is enabled, the opacity of the card view will be changed based on the swipe position.
                if (FadeOutOnSwiping)
                {
                    Opacity = 1 - (Math.Abs(_position) / Width);
                }

                //// Update the translation x value of the card view.
                TranslationX = _position;
            }
            else if (e.Status == GestureStatus.Completed || e.Status == GestureStatus.Canceled)
            {
                TouchCompleted(e.Velocity);
                _touchDownPoint = null;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to draw the indicator of the card view.
        /// </summary>
        /// <param name="canvas">The draw canvas</param>
        /// <param name="rect">The draw rectangle</param>
        /// <param name="isRTL">Indicates whether the Flow direction is RTL or not.</param>
        void DrawIndicator(ICanvas canvas, RectF rect, bool isRTL)
        {
            if (IndicatorThickness <= 0 || IndicatorColor == Colors.Transparent)
            {
                return;
            }

            //// Space occupied by the border, the indicator has to be drawn within the border, so that we need to add border space from the indicator start position.
            float borderSize = (float)BorderWidth;
            float borderSpace = borderSize / 2;
            float indicatorXPosition = borderSpace;
            float indicatorYPosition = borderSpace;
            canvas.FillColor = IndicatorColor;

            float x = 0;
            float y = 0;
            float width = 0;
            float height = 0;

            switch (IndicatorPosition)
            {
                case CardIndicatorPosition.Left:
                    x = (float)(isRTL ? rect.Width - IndicatorThickness - borderSpace : indicatorXPosition);
                    y = indicatorYPosition;
                    width = (float)IndicatorThickness;
                    height = rect.Height - borderSize;
                    break;

                case CardIndicatorPosition.Right:
                    x = (float)(isRTL ? indicatorXPosition : rect.Width - IndicatorThickness - borderSpace);
                    y = indicatorYPosition;
                    width = (float)IndicatorThickness;
                    height = rect.Height - borderSize;
                    break;

                case CardIndicatorPosition.Top:
                    x = indicatorXPosition;
                    y = indicatorYPosition;
                    width = rect.Width - borderSize;
                    height = (float)IndicatorThickness;
                    break;

                case CardIndicatorPosition.Bottom:
                    x = indicatorXPosition;
                    y = (float)(rect.Height - IndicatorThickness - borderSpace);
                    width = rect.Width - borderSize;
                    height = (float)IndicatorThickness;
                    break;
            }

            canvas.FillRectangle(x, y, width, height);
        }

        /// <summary>
        /// Method to add or remove the gesture listener.
        /// </summary>
        void AddOrRemoveGestureListener()
        {
            if (SwipeToDismiss)
            {
                this.AddGestureListener(this);
            }
            else
            {
                this.RemoveGestureListener(this);
            }
        }

        /// <summary>
        /// Method occurs when touch is completed.
        /// </summary>
        /// <param name="velocity">The velocity of the touch</param>
        void TouchCompleted(Point velocity)
        {
            //// The card view is not considered for swiping/fling when the card is at its original position (position = 0).
            if (_position == 0)
            {
                return;
            }

            //// Bool value to get whether the card view is need to swipe or not based on the velocity.
            //// The 1000 specifies the units of the velocity.The velocity is in pixels/second. The velocity value greater than offset value 1000 then the card view is swiped.
            bool isValidFling = Math.Abs(velocity.X) > 1000;
            //// If the swiping position is greater than minimum swipe offset(width divided by 2) then the card view is swiped. Otherwise the card view retains the existing position.
            if ((Math.Abs(_position) > Width / 2) || isValidFling)
            {
                // To calculate the difference between the width and the position, this calculation is performed to determine how far the card view should move during the animation.
                double difference = Width - Math.Abs(_position);
                //// The position of the card when the touch is completed.
                double startPosition = _position;
                //// Boolean value to get whether the card view is swiped right or left.
                bool isSwipedRight = _position > 0;
                //// Animation when the card view is dismissed.
                Animation animation = new Animation(value =>
                {
                    //// Updating the position of the card based on the animation value.
                    _position = isSwipedRight ? startPosition + (difference * value) : startPosition - (difference * value);
                    TranslationX = _position;
                    if (FadeOutOnSwiping)
                    {
                        Opacity = 1 - (Math.Abs(_position) / Width);
                    }
                });

                //// Updating the visibility and dismissed value of the card view when the animation is finished.
                void Finished(double value, bool isFinished)
                {
                    IsDismissed = true;
                    //// Here we have updated the position value to 0 after the IsDismissed value is set to true.
                    //// Because we need to get the dismiss direction based on the position to invoke the Dismissed event when IsDismissed property is changed.
                    _position = 0;
                }

                this.Animate("CardSwipeAnimation", animation, 16, 250, Easing.Linear, Finished, null);
            }
            else
            {
                //// The position of the card when the touch is completed.
                double startPosition = _position;
                //// Animation for the card view to retain the existing position.
                Animation animation = new Animation(value =>
                {
                    //// Updating the position of the card based on the animation value.
                    _position = startPosition - (startPosition * value);
                    TranslationX = _position;
                    //// Updating the opacity of the card view based on the animation value.
                    if (FadeOutOnSwiping)
                    {
                        Opacity = 1 - (Math.Abs(_position) / Width);
                    }
                });

                this.Animate("CardSwipeAnimation", animation, 16, 250, Easing.Linear, null, null);
            }
        }

        /// <summary>
        /// Trigger the layout measure and arrange operations.
        /// </summary>
        void InvalidateCardView()
        {
            //// In android platform sometime the InvalidateMeasure doesn't trigger the layout measure.So the view doesn't renderer properly.
            //// Hence calling measure and arrange directly without InvalidateMeasure.
            //// Example: If we add the card view in the grid, stack or other layout and change it's properties dynamically, the card view updates properly.
            //// But if we renders the card view alone and change its properties dynamically, the card view doesn't update properly.
            //// Because the InvalidateMeasure doesn't trigger the layout measure in android. So the view doesn't renderer properly.
#if ANDROID
            MeasureContent(Width, Height);
            ArrangeContent(new Rect(0, 0, Width, Height));
#else
            InvalidateMeasure();
#endif
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the card view.
        /// </summary>
        /// <param name="canvas">The draw canvas</param>
        /// <param name="dirtyRect">The rectangle</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            bool isRTL = this.IsRTL();
            DrawIndicator(canvas, dirtyRect, isRTL);
            if (BorderWidth > 0 && BorderColor != Colors.Transparent)
            {
                //// Border color of the card view
                canvas.StrokeColor = BorderColor;
                //// Border width of the card view
                canvas.StrokeSize = (float)BorderWidth;
                //// If the flow direction is right to left, then draw the rounded rectangle with the corner radius in the reverse order
                //// else draw the rounded rectangle with the corner radius in the normal order
                if (isRTL)
                {
                    canvas.DrawRoundedRectangle(dirtyRect, CornerRadius.TopRight, CornerRadius.TopLeft, CornerRadius.BottomRight, CornerRadius.BottomLeft);
                }
                else
                {
                    canvas.DrawRoundedRectangle(dirtyRect, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight);
                }
            }

            //// Set the clip to the card view
            //// The clip is used to ensure that the content within the card view stays within the boundaries of the rounded rectangle shape,
            //// Any content or elements that extend beyond the rounded corners will be cropped or hidden by the clip, meaning they are not visible in the canvas.
            RoundRectangleGeometry currentClip;
#if WINDOWS
            currentClip = new RoundRectangleGeometry(CornerRadius, dirtyRect);
#else
            if (isRTL)
            {
                currentClip = new RoundRectangleGeometry(new CornerRadius(CornerRadius.TopRight, CornerRadius.TopLeft, CornerRadius.BottomRight, CornerRadius.BottomLeft), dirtyRect);
            }
            else
            {
                currentClip = new RoundRectangleGeometry(CornerRadius, dirtyRect);
            }
#endif
            RoundRectangleGeometry? previousClip = null;
            if (Clip != null && Clip is RoundRectangleGeometry)
            {
                previousClip = (RoundRectangleGeometry)Clip;
            }

            //// Here we are comparing the previous clip with the current clip value.
            //// Because If we update the property and the clip is the same, then we don't need to update the clip.
            if (previousClip == null || previousClip.CornerRadius != currentClip.CornerRadius || previousClip.Rect != currentClip.Rect)
            {
                Clip = currentClip;
#if NET9_0 && ANDROID
                InvalidateMeasure();
#endif
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// Method to measure the content of the card view.
        /// </summary>
        /// <param name="widthConstraint">The available width</param>
        /// <param name="heightConstraint">The available height</param>
        /// <returns>The actual size</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            //// Measure behavior :
            //// 1. If the widthConstraint and heightConstraint are finite, then provided height and width will be returned.
            //// 2. If the width is infinite then content width will be returned along with finite height constraint.
            //// 3. If the height is infinite then content height will be returned along with finite width constraint.
            //// 4. If the width and height are infinite then content width and height will be returned.
            //// 5. If the width or height, or both are not finite, and also the measured content size is also zero in that case default size will be returned.

            // Boolean to check whether the indicator is enabled or not
            bool isIndicatorEnabled = IndicatorThickness > 0 && IndicatorColor != Colors.Transparent;
            bool isValidHeight = double.IsFinite(heightConstraint);
            bool isValidWidth = double.IsFinite(widthConstraint);
            double additionalWidth = BorderWidth;
            double additionalHeight = BorderWidth;
            additionalWidth = isIndicatorEnabled && (IndicatorPosition == CardIndicatorPosition.Left || IndicatorPosition == CardIndicatorPosition.Right) ? additionalWidth + IndicatorThickness : additionalWidth;
            additionalHeight = isIndicatorEnabled && (IndicatorPosition == CardIndicatorPosition.Top || IndicatorPosition == CardIndicatorPosition.Bottom) ? additionalHeight + IndicatorThickness : additionalHeight;
            additionalWidth += Padding.HorizontalThickness;
            additionalHeight += Padding.VerticalThickness;
            double width = isValidWidth ? widthConstraint : 200;
            double height = isValidHeight ? heightConstraint : 200;
            double totalWidth = width - additionalWidth;
            double totalHeight = height - additionalHeight;
            Size defaultSize = new Size(width, height);
            if (totalWidth <= 0)
            {
                totalWidth = 0;
            }

            if (totalHeight <= 0)
            {
                totalHeight = 0;
            }

            foreach (var child in Children)
            {
                if (child != Content)
                {
                    continue;
                }

                //// Get the size of the content
                Size contentSize = child.Measure(totalWidth, totalHeight);
                //// Add the additional width and height to the content size
                double contentWidth = contentSize.Width + additionalWidth;
                double contentHeight = contentSize.Height + additionalHeight;
                //// If the width and height are not finite and also the content size is zero, then return the default size
                contentSize = contentSize == Size.Zero ? defaultSize : new Size(contentWidth, contentHeight);
                //// If the width and height are finite, then return the content size
                defaultSize = new Size(isValidWidth ? defaultSize.Width : contentSize.Width, isValidHeight ? defaultSize.Height : contentSize.Height);
            }

            return defaultSize;
        }

        /// <summary>
        /// Method to arrange the content of the card view.
        /// </summary>
        /// <param name="bounds">The available size</param>
        /// <returns>The actual size</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            //// Space occupied by the border
            double borderSpace = BorderWidth / 2;
            double width = bounds.Width - BorderWidth;
            double height = bounds.Height - BorderWidth;
            bool isIndicatorEnabled = IndicatorThickness > 0 && IndicatorColor != Colors.Transparent;
            double totalWidth = (isIndicatorEnabled && (IndicatorPosition == CardIndicatorPosition.Left || IndicatorPosition == CardIndicatorPosition.Right) ? width - IndicatorThickness : width) - Padding.HorizontalThickness;
            double totalHeight = (isIndicatorEnabled && (IndicatorPosition == CardIndicatorPosition.Top || IndicatorPosition == CardIndicatorPosition.Bottom) ? height - IndicatorThickness : height) - Padding.VerticalThickness;
            double startXPosition = borderSpace + Padding.Left;
            double startYPosition = borderSpace + Padding.Top;
            foreach (var child in Children)
            {
                if (child != Content)
                {
                    continue;
                }

                //// If the indicator is enabled, then the content will be arranged based on the indicator position, else no need to consider the indicator position.
                if (isIndicatorEnabled)
                {
                    startXPosition = IndicatorPosition == CardIndicatorPosition.Left ? startXPosition + IndicatorThickness : startXPosition;
                    startYPosition = IndicatorPosition == CardIndicatorPosition.Top ? startYPosition + IndicatorThickness : startYPosition;
                    child.Arrange(new Rect(startXPosition, startYPosition, totalWidth, totalHeight));
                }
                else
                {
                    child.Arrange(new Rect(startXPosition, startYPosition, totalWidth, totalHeight));
                }
            }

            return bounds.Size;
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Invokes on border color property changed.
        /// </summary>
        /// <param name="bindable">The SfCardView object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnBorderColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardView = bindable as SfCardView;
            if (cardView == null)
            {
                return;
            }

            if (cardView.BorderWidth <= 0)
            {
                return;
            }

            cardView.InvalidateDrawable();
        }

        /// <summary>
        /// Invokes on corner radius property changed.
        /// </summary>
        /// <param name="bindable">The SfCardView object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardView = bindable as SfCardView;
            if (cardView == null)
            {
                return;
            }

            cardView.InvalidateDrawable();
        }

        /// <summary>
        /// Invokes on border width property changed.
        /// </summary>
        /// <param name="bindable">The SfCardView object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnBorderWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardView = bindable as SfCardView;
            if (cardView == null)
            {
                return;
            }

            if (cardView.BorderColor == Colors.Transparent)
            {
                return;
            }

            //// Here we have to invalidate the card view and draw the card view with updated border width.
            //// Because if we only draw the card view, the content of the card view is not rendering based on the updated border width.
            cardView.InvalidateCardView();
            cardView.InvalidateDrawable();
        }

        /// <summary>
        /// Invokes on indicator color property changed
        /// </summary>
        /// <param name="bindable">The SfCardView object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnIndicatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardView = bindable as SfCardView;
            if (cardView == null)
            {
                return;
            }

            if (cardView.IndicatorThickness <= 0)
            {
                return;
            }

            cardView.InvalidateDrawable();
        }

        /// <summary>
        /// Invokes on indicator thickness property changed
        /// </summary>
        /// <param name="bindable">The SfCardView object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnIndicatorThicknessPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardView = bindable as SfCardView;
            if (cardView == null)
            {
                return;
            }

            if (cardView.IndicatorColor == Colors.Transparent)
            {
                return;
            }

            //// Here we have to invalidate the card view and draw the card view with updated indicator thickness.
            //// Because if we only draw the card view, the content of the card view is not rendering based on the updated indicator thickness.
            cardView.InvalidateCardView();
            cardView.InvalidateDrawable();
        }

        /// <summary>
        /// Invokes on indicator position property changed
        /// </summary>
        /// <param name="bindable">The SfCardView object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnIndicatorPositionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardView = bindable as SfCardView;
            if (cardView == null)
            {
                return;
            }

            if (cardView.IndicatorThickness <= 0 || cardView.IndicatorColor == Colors.Transparent)
            {
                return;
            }

            //// Here we have to invalidate the card view and draw the card view with updated indicator position.
            //// Because if we only draw the card view, the content of the card view is not rendering based on the updated indicator position.
            cardView.InvalidateCardView();
            cardView.InvalidateDrawable();
        }

        /// <summary>
        /// Invokes on swipe to dismiss property changed.
        /// </summary>
        /// <param name="bindable">The SfCardView object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnSwipeToDismissPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCardView? cardView = bindable as SfCardView;
            if (cardView == null)
            {
                return;
            }

            //// Need to add or remove the gesture listener based on the swipe to dismiss property.
            cardView.AddOrRemoveGestureListener();
        }

        /// <summary>
        /// Invokes on fade out on swiping property changed.
        /// </summary>
        /// <param name="bindable">The SfCardView object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnFadeOutOnSwipingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        /// <summary>
        /// Invokes on is dismissed property changed.
        /// </summary>
        /// <param name="bindable">The SfCardView object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnIsDismissedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            SfCardView? cardView = bindable as SfCardView;
            if (cardView == null)
            {
                return;
            }

            //// If the card view is dismissed then we need to hide the card view and update its position.
            if ((bool)newValue)
            {
                //// Here we have handled the opacity of the card view and enabled property of the card view.
                //// If we handle the opacity then the touch is working on the card view even though the opacity is 0 so that we have handled the enabled property.
                //// Example: Create a grid with three column
                //// Add Button in column 1 and 3, and add card view in the column 2.
                //// If We swipe left are right the card view is visible in the column 1 and 3. To avoid that we have handled the Opacity.
                //// Now if we clicked the button the button click will not work because the card view is in the top of the button. To avoid that we have handled the enabled property, which will disable the interaction of the card view.
                CardsHelper.UpdateCardOpacity(0, cardView);
                cardView.IsEnabled = false;
                //// If the card view is already dismissed through swipe interaction then we need to invoke the dismissed event and return
                cardView.Dismissed?.Invoke(cardView, new CardDismissedEventArgs() { DismissDirection = cardView._position > 0 ? CardDismissDirection.Right : cardView._position == 0 ? CardDismissDirection.None : CardDismissDirection.Left });
            }
            else
            {
                //// Updating the position, enabled property and opacity of the card view when the card view is not dismissed.
                cardView.TranslationX = 0;
                cardView.IsEnabled = true;
                CardsHelper.UpdateCardOpacity(1, cardView);
            }
        }

        /// <summary>
        /// called when <see cref="CardViewBackground"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        static void OnCardViewBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfCardView cardView)
            {
                cardView.BackgroundColor = cardView.CardViewBackground;
            }
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// This method is declared only in IParentThemeElement
        /// and you need to implement this method only in main control.
        /// </summary>
        /// <returns>ResourceDictionary</returns>
        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfCardViewStyles();
        }

        /// <summary>
        /// This method will be called when users merge a theme dictionary
        /// that contains value for “SyncfusionTheme” dynamic resource key.
        /// </summary>
        /// <param name="oldTheme">Old theme.</param>
        /// <param name="newTheme">New theme.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
            SetDynamicResource(CardViewBackgroundProperty, "SfCardViewNormalBackround");
        }

        /// <summary>
        /// This method will be called when a theme dictionary
        /// that contains the value for your control key is merged in application.
        /// </summary>
        /// <param name="oldTheme">The old value.</param>
        /// <param name="newTheme">The new value.</param>
        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever the card view dismissed.
        /// </summary>
        public event EventHandler<CardDismissedEventArgs>? Dismissed;

        /// <summary>
        /// Occurs whenever the card view is dismissing.
        /// </summary>
        public event EventHandler<CardDismissingEventArgs>? Dismissing;

        #endregion
    }
}