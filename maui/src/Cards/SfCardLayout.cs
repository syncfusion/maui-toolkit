using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Cards
{
    /// <summary>
    /// Represents a class for card layout, which contains card views as children.
    /// </summary>
    public class SfCardLayout : CardLayout, IPanGestureListener, ITapGestureListener
    {
        #region Fields

        /// <summary>
        /// Represents the visible cards count.
        /// </summary>
        const int VisibleCardsCount = 3;

        /// <summary>
        /// Padding value for the card view.
        /// </summary>
        const int SwipedCardSize = 5;

        /// <summary>
        /// The initial value of x and y position while touch or swipe is occur.
        /// </summary>
        Point? _touchDownPoint = null;

        /// <summary>
        /// The current position of the card view while swipe is occurred.
        /// </summary>
        double _position = 0;

        /// <summary>
        /// Boolean value to indicate whether the card view dismissal or retrieval animation is started or not.
        /// We need to remove a card(last visible card below the current visible index) when the card retrieval animation is started.
        /// And this value used to change the card position below the current visible card when the card retrieval animation is started.
        /// This value is only assigned on animation completed method and this value becomes true when the card retrieval animation is started.
        /// This value assigned to false when the card retrieval animation is completed and the card swiping is not a valid navigation(dismissed or retrieved).
        /// This value is also used in the forward method where we used to retrieve the cards with the animation.
        /// </summary>
        bool _isNavigationAnimationStarted = false;

        /// <summary>
        /// The animation value used to animate the card view when the card is swiped.
        /// </summary>
        double _animationValue = 0;

        /// <summary>
        /// Determines whether to trigger an animation when the visible index changes.
        /// Set to true to animate changes in the visible index.
        /// </summary>
        bool _isVisibleIndexAnimation = true;

        /// <summary>
        /// Indicates whether the current interaction is moving backward.
        /// Set to true if the interaction involves a backward movement.
        /// </summary>
        bool _isBackwardInteraction = false;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="SwipeDirection"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="SwipeDirection"/> dependency property.
        /// </value>
        public static readonly BindableProperty SwipeDirectionProperty =
            BindableProperty.Create(
                nameof(SwipeDirection),
                typeof(CardSwipeDirection),
                typeof(SfCardLayout),
                CardSwipeDirection.Right,
                propertyChanged: OnSwipeDirectionPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowSwipedCard"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowSwipedCard"/> dependency property.
        /// </value>
        public static readonly BindableProperty ShowSwipedCardProperty =
            BindableProperty.Create(
                nameof(ShowSwipedCard),
                typeof(bool),
                typeof(SfCardLayout),
                true,
                propertyChanged: OnShowSwipedCardPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="VisibleIndex"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="VisibleIndex"/> dependency property.
        /// </value>
        public static readonly BindableProperty VisibleIndexProperty =
            BindableProperty.Create(
                nameof(VisibleIndex),
                typeof(int?),
                typeof(SfCardLayout),
                -1,
                propertyChanged: OnVisibleIndexPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalCardSpacing"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HorizontalCardSpacing"/> dependency property.
        /// </value>
        public static readonly BindableProperty HorizontalCardSpacingProperty =
            BindableProperty.Create(
                nameof(HorizontalCardSpacing),
                typeof(int),
                typeof(SfCardLayout),
                10,
                propertyChanged: OnHorizontalCardSpacingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalCardSpacing"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="VerticalCardSpacing"/> dependency property.
        /// </value>
        public static readonly BindableProperty VerticalCardSpacingProperty =
            BindableProperty.Create(
                nameof(VerticalCardSpacing),
                typeof(int),
                typeof(SfCardLayout),
                10,
                propertyChanged: OnVerticalCardSpacingPropertyChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfCardLayout"/> class.
        /// </summary>
        public SfCardLayout()
        {
            BackgroundColor = Colors.White;
            Padding = new Thickness(10, 5);
            IsClippedToBounds = true;
            this.AddGestureListener(this);
#if IOS

#if NET10_0
            this.SafeAreaEdges = SafeAreaEdges.None;
#else
            IgnoreSafeArea = true;
#endif

#endif
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the swipe direction of the card layout.
        /// </summary>
        /// <value>The default value of <see cref="SfCardLayout.SwipeDirection"/> is CardSwipeDirection.Right. </value>
        /// <example>
        /// The following code demonstrates, how to use the SwipeDirection property in the card layout.
        /// # [XAML](#tab/tabid-1)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardLayout x:Name="CardLayout"
        ///                   SwipeDirection="Left">
        /// </Cards:SfCardLayout>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code Lang="C#"><![CDATA[
        /// CardLayout.SwipeDirection = CardSwipeDirection.Left;
        /// ]]></code>
        /// </example>
        public CardSwipeDirection SwipeDirection
        {
            get { return (CardSwipeDirection)GetValue(SwipeDirectionProperty); }
            set { SetValue(SwipeDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the swiped card or not.
        /// </summary>
        /// <value>The default value of <see cref="SfCardLayout.ShowSwipedCard"/> is true. </value>
        /// <example>
        /// The following code demonstrates, how to use the ShowSwipedCard property in the card layout.
        /// # [XAML](#tab/tabid-3)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardLayout x:Name="CardLayout"
        ///                   ShowSwipedCard="False">
        /// </Cards:SfCardLayout>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code Lang="C#"><![CDATA[
        /// CardLayout.ShowSwipedCard = false;
        /// ]]></code>
        /// </example>
        public bool ShowSwipedCard
        {
            get { return (bool)GetValue(ShowSwipedCardProperty); }
            set { SetValue(ShowSwipedCardProperty, value); }
        }

        /// <summary>
        /// Gets or sets the visible card index of the card layout.
        /// </summary>
        /// <remarks>
        /// If we set the invalid index value, then the card layout will display the last card as visible card.
        /// </remarks>
        /// <value>The default value of <see cref="SfCardLayout.VisibleIndex"/> is -1. </value>
        /// <example>
        /// The following code demonstrates, how to use the VisibleIndex property in the card layout.
        /// # [XAML](#tab/tabid-5)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardLayout x:Name="CardLayout"
        ///                   VisibleIndex="1">
        /// </Cards:SfCardLayout>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code Lang="C#"><![CDATA[
        /// CardLayout.VisibleIndex = 1;
        /// ]]></code>
        /// </example>
        public int? VisibleIndex
        {
            get { return (int?)GetValue(VisibleIndexProperty); }
            set { SetValue(VisibleIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal spacing between the cards.
        /// </summary>
        /// <value>The default value of <see cref="SfCardLayout.HorizontalCardSpacing"/> is 10. </value>
        /// <example>
        /// The following code demonstrates, how to use the HorizontalCardSpacing property in the card layout.
        /// # [XAML](#tab/tabid-7)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardLayout x:Name="CardLayout"
        ///                   HorizontalCardSpacing="20">
        /// </Cards:SfCardLayout>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code Lang="C#"><![CDATA[
        /// CardLayout.HorizontalCardSpacing = 20;
        /// ]]></code>
        /// </example>
        public int HorizontalCardSpacing
        {
            get { return (int)GetValue(HorizontalCardSpacingProperty); }
            set { SetValue(HorizontalCardSpacingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical spacing between the cards.
        /// </summary>
        /// <value>The default value of <see cref="SfCardLayout.VerticalCardSpacing"/> is 10. </value>
        /// <example>
        /// The following code demonstrates, how to use the VerticalCardSpacing property in the card layout.
        /// # [XAML](#tab/tabid-9)
        /// <code Lang="XAML"><![CDATA[
        /// <Cards:SfCardLayout x:Name="CardLayout"
        ///                   VerticalCardSpacing="20">
        /// </Cards:SfCardLayout>
        /// ]]></code>
        /// # [C#](#tab/tabid-10)
        /// <code Lang="C#"><![CDATA[
        /// CardLayout.VerticalCardSpacing = 20;
        /// ]]></code>
        /// </example>
        public int VerticalCardSpacing
        {
            get { return (int)GetValue(VerticalCardSpacingProperty); }
            set { SetValue(VerticalCardSpacingProperty, value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Method invokes on pan gesture action is occur.
        /// </summary>
        /// <param name="e">Pan event argument details</param>
        public void OnPan(PanEventArgs e)
        {
            bool canSwipeHorizontally = CanSwipeHorizontally();
            bool isRTL = this.IsRTL();
            bool isRightSwipeDirection = IsRightSwipeDirection(canSwipeHorizontally);

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

                double currentPosition;
                if (canSwipeHorizontally)
                {
                    //// Current position of the card view while the swipe is occurred horizontally.
                    currentPosition = e.TouchPoint.X - _touchDownPoint.Value.X;
                }
                else
                {
                    //// Current position of the card view while the swipe is occurred vertically.
                    currentPosition = e.TouchPoint.Y - _touchDownPoint.Value.Y;
                }

                //// Here we have inversed the current position value when the flow direction is RTL, because the swipe direction will be reversed in RTL.
#if !WINDOWS
                if (isRTL && canSwipeHorizontally)
                {
                    currentPosition = -currentPosition;
                }
#endif
                //// Checking whether the card is swiped right or left.
                bool isSwipedRightOrBottom = currentPosition > 0;
                //// Get the actual visible card index when the VisibleCardIndex is less than 0 or greater than the child count.
                int visibleCardIndex = GetValidVisibleCardIndex(VisibleIndex);
                bool isCardDismissing = IsCardDismissing(currentPosition);
                //// If we tried to dismiss the card when the VisibleCardIndex is -1 or if we tried to retrieve the card when there is no card to retrieve, then we need to restrict the animation running.
                if ((visibleCardIndex == -1 && isCardDismissing) || (visibleCardIndex == Children.Count - 1 && !isCardDismissing))
                {
                    //// If we are holding the touch at the center of the view and moving to the left and the right direction the position of the swiped card must be 0.
                    if (_position != 0)
                    {
                        _position = 0;
                        LayoutArrangeChildren(Bounds);
                    }

                    return;
                }

                int nextPossibleIndex = visibleCardIndex + (isCardDismissing ? -1 : 1);
                CardVisibleIndexChangingEventArgs args = new CardVisibleIndexChangingEventArgs
                {
                    //// Here the current visible card index is passed as the old card index.
                    OldIndex = visibleCardIndex < 0 ? null : visibleCardIndex,
                    //// Here the next possible card index is passed as the new cardindex.
                    NewIndex = nextPossibleIndex < 0 ? null : nextPossibleIndex,
                };

                //// Invoking the visible card index changing event.
                VisibleIndexChanging?.Invoke(this, args);
                //// If the event is canceled then the card should not be swiped.
                if (args.Cancel)
                {
                    //// If the card is swiped to the left or right direction then the position of the swiped card must be 0.
                    //// Here we are checking whether the position is not 0, because if we swipe continuously in the canceled direction then the layout will be arranged continuously.
                    if (_position != 0)
                    {
                        _position = 0;
                        LayoutArrangeChildren(Bounds);
                    }

                    return;
                }

                _position = currentPosition;
                LayoutArrangeChildren(Bounds);
            }
            else if (e.Status == GestureStatus.Completed || e.Status == GestureStatus.Canceled)
            {
                TouchCompleted(e.Velocity, isRightSwipeDirection, canSwipeHorizontally, isRTL);
                _touchDownPoint = null;
            }
        }

        /// <summary>
        /// Method occurs when the card layout is tapped.
        /// </summary>
        /// <param name="e">Tapped event arguments</param>
        public void OnTap(TapEventArgs e)
        {
            //// Get the actual visible card index when the VisibleCardIndex is less than 0 or greater than the child count.
            int visibleCardIndex = GetValidVisibleCardIndex(VisibleIndex);
            bool isRTL = this.IsRTL();
            //// Checking whether the swipe direction is right or not.
            bool isRightSwipeDirection = SwipeDirection == CardSwipeDirection.Right;
            bool isBottomSwipeDirection = SwipeDirection == CardSwipeDirection.Bottom;
            bool canSwipeHorizontally = CanSwipeHorizontally();
            //// In Windows platform, the children of the card layout arranged based on the flow direction instead of the swipe direction.
            //// So the swipe direction will be reversed in the Windows platform if the flow direction is RTL.
#if WINDOWS
            if (canSwipeHorizontally && isRTL)
            {
                isRightSwipeDirection = !isRightSwipeDirection;
            }
#endif

            //// Maximum height of the card view.
            double height = Height - Padding.VerticalThickness;
            double width = Width - Padding.HorizontalThickness;
            double yPosition = Padding.Top;
            double leftPadding = Padding.Left;
            double rightPadding = Padding.Right;
            //// In Windows platform the padding value is inversed based on the flow direction automatically.
            //// But in other platforms, we need to inverse the padding value based on the flow direction.
#if !WINDOWS
            if (isRTL)
            {
                leftPadding = Padding.Right;
                rightPadding = Padding.Left;
            }
#endif

            //// Checking whether the tap point is inside the swiped card bounds. If the tap point is inside the swiped card bounds, then the card tapped event will be raised with the swiped card as parameter and exits.
            //// No need to check if the ShowSwipedCard is false and the visible card index is the last index.
            if (ShowSwipedCard && visibleCardIndex < Children.Count - 1)
            {
                int swipedCardWidth = SwipedCardSize;
                //// Getting the bounds of the swiped card based on the swipe direction.
                Rect swipedCardBounds;
                if (canSwipeHorizontally)
                {
                    swipedCardBounds = new Rect(isRightSwipeDirection ? Bounds.Right - swipedCardWidth : Bounds.Left, yPosition, swipedCardWidth, height);
                }
                else
                {
                    swipedCardBounds = new Rect(Bounds.Left + leftPadding, isBottomSwipeDirection ? Bounds.Bottom - swipedCardWidth : Bounds.Top, width, swipedCardWidth);
                }

                //// Checking whether the tap point is within the bounds of the swiped card.
                if (swipedCardBounds.Contains(e.TapPoint.X, e.TapPoint.Y))
                {
                    Tapped?.Invoke(this, new TappedEventArgs(Children[visibleCardIndex + 1]));
                    return;
                }
            }

            //// Checking whether the tap point is inside the padding area. If the tap point is inside the padding area, then the card tapped event will be raised with null parameter and exits.
            if (visibleCardIndex == -1 || e.TapPoint.X < leftPadding || e.TapPoint.X > Width - rightPadding || e.TapPoint.Y < Padding.Top || e.TapPoint.Y > Height - Padding.Bottom)
            {
                Tapped?.Invoke(this, new TappedEventArgs(null));
                return;
            }

            int horizontalCardSpacing = GetValidSpacing(HorizontalCardSpacing);
            int verticalCardSpacing = GetValidSpacing(VerticalCardSpacing);
            //// Calculating the maximum padding for the card view in the horizontal direction.
            int cardHorizontalPadding = (VisibleCardsCount - 1) * horizontalCardSpacing;
            int cardVerticalPadding = (VisibleCardsCount - 1) * verticalCardSpacing;
            //// Getting the difference between the cards based on the swipe direction.
            int horizontalOffsetDifference = isRightSwipeDirection ? horizontalCardSpacing : -horizontalCardSpacing;
            int verticalOffsetDifference = isBottomSwipeDirection ? verticalCardSpacing : -verticalCardSpacing;
            //// Calculate the index difference between the maximum visible card index and the total visible cards count.
            //// The index difference gives the number of cards that are missing from the visible range.
            //// if the index difference is greater than 0 which means cards count is less than VisibleCardsCount, adjust the xPosition based on the index difference and offset difference.
            int indexDifference = VisibleCardsCount - 1 - visibleCardIndex;
            //// Getting the visible card count in the card layout to avoid the index out of range exception when the card count is less than VisibleCardsCount.
            int visibleCardCount = visibleCardIndex < VisibleCardsCount - 1 ? visibleCardIndex + 1 : VisibleCardsCount;
            //// Looping through the visible card count to check whether the tap point is inside the each card bounds.
            for (int i = 0; i < visibleCardCount; i++)
            {
                int horizontalPadding = i * horizontalCardSpacing;
                int verticalPadding = i * verticalCardSpacing;
                double cardXPosition, cardYPosition, cardHeight, cardWidth;
                if (canSwipeHorizontally)
                {
                    //// Getting the xPosition of the card view based on the swipe direction and offset difference also adjusting the xPosition when card count is less than VisibleCardsCount.
                    cardXPosition = leftPadding + ((isRightSwipeDirection ? cardHorizontalPadding : 0) - (i * horizontalOffsetDifference) - (indexDifference > 0 ? indexDifference * (horizontalOffsetDifference / 2) : 0));
                    //// Getting the yPosition of the each card view in the card layout
                    cardYPosition = yPosition + verticalPadding;
                    cardWidth = Width - cardHorizontalPadding - Padding.HorizontalThickness;
                    //// Getting the width and height of the each card view in the card layout.
                    cardHeight = height - (2 * verticalPadding);
                }
                else
                {
                    //// Getting the xPosition of the card view based on the swipe direction and offset difference also adjusting the xPosition when card count is less than VisibleCardsCount.
                    cardXPosition = leftPadding + horizontalPadding;
                    //// Getting the yPosition of the each card view in the card layout
                    cardYPosition = yPosition + ((isBottomSwipeDirection ? cardVerticalPadding : 0) - (i * verticalOffsetDifference) - (indexDifference > 0 ? indexDifference * (verticalOffsetDifference / 2) : 0));
                    //// Getting the width and height of the each card view in the card layout.
                    cardWidth = width - (2 * horizontalPadding);
                    cardHeight = Height - cardVerticalPadding - Padding.VerticalThickness;
                }

                //// Bounds of the each card view in the card layout.
                Rect bounds = new Rect(cardXPosition, cardYPosition, cardWidth, cardHeight);
                //// Checking whether the tap point is inside the bounds of the each card view. If the tap point is inside the bounds, then the card tapped event will be raised with the tapped card view and exits.
                if (bounds.Contains(e.TapPoint.X, e.TapPoint.Y))
                {
                    Tapped?.Invoke(this, new TappedEventArgs(Children[visibleCardIndex - i]));
                    return;
                }
            }

            //// If the tap point is not inside the bounds of the each card view, then the card tapped event will be raised with null parameter.
            Tapped?.Invoke(this, new TappedEventArgs(null));
        }

        /// <summary>
        /// Move to next card with animation.
        /// </summary>
        public void Forward()
        {
            int childCount = Children.Count;
            int visibleIndex = GetValidVisibleCardIndex(VisibleIndex);
            //// Here we are restricting the animation when the previous animation is running.
            //// Example: If we tries to call the Forward or Backward method before the previous animation complete the card will not arrange properly,
            //// So we are checking the position of the card view.
            //// We also need to restrict the animation when the card count is zero or the visible index is the last index.
            if (_position != 0 || childCount == 0 || visibleIndex == childCount - 1)
            {
                return;
            }

            bool isRTL = this.IsRTL();
            bool isBottomSwipeDirection = SwipeDirection == CardSwipeDirection.Bottom;
            bool canSwipeHorizontally = CanSwipeHorizontally();
            bool isRightSwipeDirection = IsRightSwipeDirection(canSwipeHorizontally);
            int horizontalCardSpacing = GetValidSpacing(HorizontalCardSpacing);
            int verticalCardSpacing = GetValidSpacing(VerticalCardSpacing);
            //// Calculating the maximum padding for the card view in the horizontal direction.
            int cardHorizontalPadding = (VisibleCardsCount - 1) * horizontalCardSpacing;
            int cardVerticalPadding = (VisibleCardsCount - 1) * verticalCardSpacing;
            double visibleCardDimensions = canSwipeHorizontally ? Bounds.Width - cardHorizontalPadding - Padding.HorizontalThickness : Bounds.Height - cardVerticalPadding - Padding.VerticalThickness;
            double padding = canSwipeHorizontally ? (isRightSwipeDirection ? Padding.Right : Padding.Left) : (isBottomSwipeDirection ? Padding.Bottom : Padding.Top);
            //// In Windows platform the padding value is inversed based on the flow direction automatically.
            //// But in other platforms, we need to inverse the padding value based on the flow direction.
#if !WINDOWS
            if (isRTL && canSwipeHorizontally)
            {
                padding = isRightSwipeDirection ? Padding.Left : Padding.Right;
            }
#endif

            double startPosition = _position;
            //// Distance to be moved by the card view when the animation is started.
            //// Here the padding is added to avoid the sudden dismissal of the card view when the position is near to the width.
            double distance = visibleCardDimensions + padding;
            Animation animation = new Animation(value =>
            {
                //// Here we need to measure for the possible cards at the middle of the animation.
                //// Example: If the card of index 3 is retrieved then we need to measure for the card of index 1 and 2 and remove the card of index 0.
                //// Before retrieval of the card of index 3, the cards of index 0, 1 and 2 are already measured.
                if (value > 0.5 && !_isNavigationAnimationStarted)
                {
                    _isNavigationAnimationStarted = true;
                    _isVisibleIndexAnimation = false;
                    LayoutMeasure(Bounds.Width, Bounds.Height);
                }

                _animationValue = value;
                _position = isRightSwipeDirection || isBottomSwipeDirection ? startPosition - (distance * value) : startPosition + (distance * value);
                LayoutArrangeChildren(Bounds);
            });

            //// Updating the visible card index when the animation is finished.
            void Finished(double value, bool isFinished)
            {
                _position = 0;
                _animationValue = 0;
                _isNavigationAnimationStarted = false;
                UpdateVisibleCardIndex(visibleIndex, false);
            }

            this.Animate("ForwardAnimation", animation, 16, 400, Easing.Linear, Finished, null);
        }

        /// <summary>
        /// Move to previous card with animation.
        /// </summary>
        public void Backward()
        {
            int childCount = Children.Count;
            int visibleIndex = GetValidVisibleCardIndex(VisibleIndex);
            //// Here we are restricting the animation when the previous animation is running.
            //// Example: If we tries to call the Forward or Backward method before the previous animation complete the card will not arrange properly,
            //// So we are checking the position of the card view.
            //// We also need to restrict the animation when the card count is zero or the visible index is the minimum index.
            if (_position != 0 || childCount == 0 || visibleIndex == -1)
            {
                return;
            }

            bool isRTL = this.IsRTL();
            bool isBottomSwipeDirection = SwipeDirection == CardSwipeDirection.Bottom;
            bool canSwipeHorizontally = CanSwipeHorizontally();
            bool isRightSwipeDirection = IsRightSwipeDirection(canSwipeHorizontally);
            int horizontalCardSpacing = GetValidSpacing(HorizontalCardSpacing);
            int verticalCardSpacing = GetValidSpacing(VerticalCardSpacing);
            //// Calculating the maximum padding for the card view in the horizontal direction.
            int cardHorizontalPadding = (VisibleCardsCount - 1) * horizontalCardSpacing;
            int cardVerticalPadding = (VisibleCardsCount - 1) * verticalCardSpacing;
            double visibleCardDimensions = canSwipeHorizontally ? Bounds.Width - cardHorizontalPadding - Padding.HorizontalThickness : Bounds.Height - cardVerticalPadding - Padding.VerticalThickness;
            double padding = canSwipeHorizontally ? (isRightSwipeDirection ? Padding.Right : Padding.Left) : (isBottomSwipeDirection ? Padding.Bottom : Padding.Top);
            //// In Windows platform the padding value is inversed based on the flow direction automatically.
            //// But in other platforms, we need to inverse the padding value based on the flow direction.
#if !WINDOWS
            if (isRTL && canSwipeHorizontally)
            {
                padding = isRightSwipeDirection ? Padding.Left : Padding.Right;
            }
#endif
            double startPosition = _position;
            //// Distance to be moved by the card view when the animation is started.
            //// Here the padding is added to avoid the sudden dismissal of the card view when the position is near to the width.
            double distance = visibleCardDimensions + padding;
            Animation animation = new Animation(value =>
            {
                _animationValue = value;
                _isNavigationAnimationStarted = true;
                _isVisibleIndexAnimation = false;
                _position = isRightSwipeDirection || isBottomSwipeDirection ? startPosition + (distance * value) : startPosition - (distance * value);
                LayoutArrangeChildren(Bounds);
            });

            //// Updating the visible card index when the animation is finished.
            void Finished(double value, bool isFinished)
            {
                _position = 0;
                _animationValue = 0;
                _isNavigationAnimationStarted = false;
                UpdateVisibleCardIndex(visibleIndex, true);
            }

            this.Animate("BackwardAnimation", animation, 16, 400, Easing.Linear, Finished, null);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to get the valid value for horizontal and vertical spacing.
        /// </summary>
        /// <param name="spacing">Horizontal or vertical spacing.</param>
        /// <returns>Returns valid spacing value of the cards.</returns>
        static int GetValidSpacing(int spacing)
        {
            return spacing < 0 ? 10 : spacing;
        }

        /// <summary>
        /// Method to get the visible card index.
        /// </summary>
        /// <param name="index">Index value</param>
        /// <returns>Returns the actual visible card index.</returns>
        int GetValidVisibleCardIndex(int? index)
        {
            //// Here the view when all the cards are swiped is handled with the index value -1.
            if (index == null)
            {
                return -1;
            }

            int childCount = Children.Count;
            //// If we set the invalid index that is greater than the child count or less than 0 then the visible card index will be the last index.
            if (index >= childCount || index < 0)
            {
                return childCount - 1;
            }

            return (int)index;
        }

        /// <summary>
        /// Method occurs when touch is completed.
        /// </summary>
        /// <param name="velocity">The velocity of the touch</param>
        /// <param name="isRightSwipeDirection">Boolean value to get whether the swipe direction is right or not.</param>
        /// <param name="canSwipeHorizontally">Boolean value to get whether the cards in the card layout can swiped horizontally or not.</param>
        /// <param name="isRTL">Defines whether the layout is in RTL or LTR flow direction.</param>
        void TouchCompleted(Point velocity, bool isRightSwipeDirection, bool canSwipeHorizontally, bool isRTL)
        {
            if (_position == 0)
            {
                return;
            }

            // Bool value to get whether the card view is need to swipe or not based on the position and the velocity.
            // The 1000 specifies the units of the velocity.The velocity is in pixels/second. The velocity value greater than offset value 1000 then the card view is swiped.
            bool isValidFling = (canSwipeHorizontally ? Math.Abs(velocity.X) : Math.Abs(velocity.Y)) > 1000;
            _isNavigationAnimationStarted = Math.Abs(_position) > (canSwipeHorizontally ? Width / 2 : Height / 2) || isValidFling;
            //// Boolean value to get whether the card view is swiped right or left.
            bool isSwipedRightOrBottom = _position > 0;
            bool isCardDismissing = IsCardDismissing(_position);
            int visibleCardIndex = GetValidVisibleCardIndex(VisibleIndex);
            int horizontalCardSpacing = GetValidSpacing(HorizontalCardSpacing);
            int verticalCardSpacing = GetValidSpacing(VerticalCardSpacing);
            bool isBottomSwipeDirection = SwipeDirection == CardSwipeDirection.Bottom;
            //// If the swiping position is greater than minimum swipe offset(width divided by 2) then the card view is swiped. Otherwise the card view retains the existing position.
            if (_isNavigationAnimationStarted)
            {
                int cardHorizontalPadding = (VisibleCardsCount - 1) * horizontalCardSpacing;
                int cardVerticalPadding = (VisibleCardsCount - 1) * verticalCardSpacing;
                double visibleCardDimensions = canSwipeHorizontally ? Bounds.Width - cardHorizontalPadding - Padding.HorizontalThickness : Bounds.Height - cardVerticalPadding - Padding.VerticalThickness;
                double padding = isRightSwipeDirection ? Padding.Right : Padding.Left;
#if !WINDOWS
                if (isRTL)
                {
                    padding = isRightSwipeDirection ? Padding.Left : Padding.Right;
                }
#endif
                //// To calculate the difference between the width and the position, this calculation is performed to determine how far the card view should move during the animation.
                //// Here the padding is added to avoid the sudden dismissal of the card view when the position is near to the width.
                double difference = visibleCardDimensions + padding - Math.Abs(_position);
                //// The position of the card when the touch is completed.
                double startPosition = _position;
                //// Animation when the card view is dismissed.
                Animation animation = new Animation(value =>
                {
                    //// Here we need to measure for the possible cards when the animation is in progress.
                    //// Example: If the card of index 3 is retrieved then we need to measure for the card of index 1 and 2 and remove the card of index 0.
                    //// Before retrieval of the card of index 3, the cards of index 0, 1 and 2 are already measured.
                    if (value == 0 && !isCardDismissing)
                    {
                        LayoutMeasure(Bounds.Width, Bounds.Height);
                    }

                    //// Updating the position of the card based on the animation value.
                    _position = isSwipedRightOrBottom ? startPosition + (difference * value) : startPosition - (difference * value);
                    _animationValue = value;
                    _isVisibleIndexAnimation = false;
                    LayoutArrangeChildren(Bounds);
                });

                //// Updating the visibility and dismissed value of the card view when the animation is finished.
                void Finished(double value, bool isFinished)
                {
                    _position = 0;
                    _animationValue = 0;
                    _isNavigationAnimationStarted = false;
                    UpdateVisibleCardIndex(visibleCardIndex, isCardDismissing);
                }

                this.Animate("CardLayoutAnimation", animation, 16, 250, Easing.Linear, Finished, null);
            }
            else
            {
                //// The position of the card when the touch is completed.
                double startXPosition = _position;
                //// Animation for the card view to retain the existing position.
                Animation animation = new Animation(value =>
                {
                    //// Updating the position of the card based on the animation value.
                    _position = startXPosition - (startXPosition * value);
                    _isVisibleIndexAnimation = true;
                    LayoutArrangeChildren(Bounds);
                });

                this.Animate("CardLayoutAnimation", animation, 16, 250, Easing.Linear, null, null);
            }
        }

        /// <summary>
        /// Method occurs when visible index changed.
        /// </summary>
        /// <param name="isGreaterVisibleIndex">Boolean value to get whether the new visible index is greater or lesser.</param>
        /// <param name="difference">To check the difference of visible index.</param>
        /// <param name="isZerothIndex">To check the old visible index is 0.</param>
        void GetVisibleIndexAnimation(bool isGreaterVisibleIndex, int? difference, bool isZerothIndex)
        {
            int childCount = Children.Count;
            int visibleIndex = GetValidVisibleCardIndex(VisibleIndex);
            //// Here we are restricting the animation when the previous animation is running.
            //// Example: If we tries to call the Forward or Backward method before the previous animation complete the card will not arrange properly,
            //// So we are checking the position of the card view.
            //// We also need to restrict the animation when the card count is zero or the visible index is the minimum index.
            if (_position != 0 || childCount == 0 || visibleIndex == -1)
            {
                return;
            }

            bool isRTL = this.IsRTL();
            bool isBottomSwipeDirection = SwipeDirection == CardSwipeDirection.Bottom;
            bool canSwipeHorizontally = CanSwipeHorizontally();
            bool isRightSwipeDirection = IsRightSwipeDirection(canSwipeHorizontally);
            int horizontalCardSpacing = GetValidSpacing(HorizontalCardSpacing);
            int verticalCardSpacing = GetValidSpacing(VerticalCardSpacing);
            //// Calculating the maximum padding for the card view in the horizontal direction.
            int cardHorizontalPadding = (VisibleCardsCount - 1) * horizontalCardSpacing;
            int cardVerticalPadding = (VisibleCardsCount - 1) * verticalCardSpacing;
            double visibleCardDimensions = canSwipeHorizontally ? Bounds.Width - cardHorizontalPadding - Padding.HorizontalThickness : Bounds.Height - cardVerticalPadding - Padding.VerticalThickness;
            double padding = canSwipeHorizontally ? (isRightSwipeDirection ? Padding.Right : Padding.Left) : (isBottomSwipeDirection ? Padding.Bottom : Padding.Top);
            //// In Windows platform the padding value is inversed based on the flow direction automatically.
            //// But in other platforms, we need to inverse the padding value based on the flow direction.
#if !WINDOWS
            if (isRTL && canSwipeHorizontally)
            {
                padding = isRightSwipeDirection ? Padding.Left : Padding.Right;
            }
#endif
            double startPosition = _position;
            //// Distance to be moved by the card view when the animation is started.
            //// Here the padding is added to avoid the sudden dismissal of the card view when the position is near to the width.
            double distance = visibleCardDimensions + padding;
            Animation animation = new Animation(value =>
            {
#if MACCATALYST || IOS
                if (value < 0.5)
                {
                    return;
                }
#endif
                if (isGreaterVisibleIndex)
                {
                    //// Here we need to measure for the possible cards at the middle of the animation.
                    //// Example: If the card of index 3 is retrieved then we need to measure for the card of index 1 and 2 and remove the card of index 0.
                    //// Before retrieval of the card of index 3, the cards of index 0, 1 and 2 are already measured.
                    if (value > 0.5 && !_isNavigationAnimationStarted)
                    {
                        _isNavigationAnimationStarted = true;
                        LayoutMeasure(Bounds.Width, Bounds.Height);
                    }

                    _animationValue = value;
#if !WINDOWS
                    _isNavigationAnimationStarted = true;
#endif
                    _position = isRightSwipeDirection || isBottomSwipeDirection ? startPosition - (distance * value) : startPosition + (distance * value);
                    if (_position == 0 || (isZerothIndex && difference > 1))
                    {
                        isZerothIndex = false;
                        return;
                    }

                    LayoutArrangeChildren(Bounds);
                }
                else
                {
                    _animationValue = value;
                    _isNavigationAnimationStarted = true;
                    _isBackwardInteraction = true;
                    _position = isRightSwipeDirection || isBottomSwipeDirection ? startPosition + (distance * value) : startPosition - (distance * value);
#if !ANDROID
                    if (value > 0.5 && difference < -1)
                    {
                        LayoutMeasure(Bounds.Width, Bounds.Height);
                    }
#endif
                    LayoutArrangeChildren(Bounds);
                }
            });

            //// Updating the visible card index when the animation is finished.
            void Finished(double value, bool isFinished)
            {
                _position = 0;
                _animationValue = 0;
                _isNavigationAnimationStarted = false;
                _isBackwardInteraction = false;
                InvalidateCardLayout();
            }

            this.Animate("VisibleIndexAnimation", animation, 16, 800, Easing.Linear, Finished, null);
        }

        /// <summary>
        /// Method to update the visible card index.
        /// </summary>
        /// <param name="visibleIndex">Visible card index</param>
        /// <param name="isCardDismissed">Boolean value to get whether the card is dismissed or not.</param>
        void UpdateVisibleCardIndex(int visibleIndex, bool isCardDismissed)
        {
            //// If the card is swiped the card below the swiped card must appear on top. So the visible card index is decremented.
            //// If the card is retrieved the card must appear below the retrieved card. So the visible card index is incremented.
            if (isCardDismissed)
            {
                visibleIndex = visibleIndex - 1;
                VisibleIndex = visibleIndex < 0 ? null : visibleIndex;
            }
            else
            {
                VisibleIndex = visibleIndex + 1;
            }
        }

        /// <summary>
        /// Method to get whether the card is dismissing or not.
        /// </summary>
        /// <param name="position">The position of the card view.</param>
        /// <returns>Returns true if card is dismissing and false if card is not dismissing.</returns>
        bool IsCardDismissing(double position)
        {
            if (SwipeDirection == CardSwipeDirection.Left || SwipeDirection == CardSwipeDirection.Right)
            {
                //// Getting the swipe direction of the card layout.
                bool isRightSwipeDirection = SwipeDirection == CardSwipeDirection.Right;
                //// The children of the card layout arranged based on the flow direction instead of the swipe direction.
                //// So the swipe direction will be reversed in all the platform if the flow direction is RTL.
                //// Position also inversed based on the flow direction.
                if (this.IsRTL())
                {
                    isRightSwipeDirection = !isRightSwipeDirection;
                }

                //// Checking whether the card is swiped right or not.
                bool isSwipedRight = position > 0;
                //// Checking whether the card is dismissing or retrieving.
                return isRightSwipeDirection ? isSwipedRight : !isSwipedRight;
            }
            else
            {
                //// Getting the swipe direction of the card layout.
                bool isBottomSwipeDirection = SwipeDirection == CardSwipeDirection.Bottom;
                //// Checking whether the card is swiped bottom or not.
                bool isSwipedBottom = position > 0;
                //// Checking whether the card is dismissing or retrieving.
                return isBottomSwipeDirection ? isSwipedBottom : !isSwipedBottom;
            }
        }

        /// <summary>
        /// Method to get the swiped card bounds.
        /// </summary>
        /// <param name="bounds">the bounds</param>
        /// <param name="xPosition">x position of the card</param>
        /// <param name="yPosition">y position of the card</param>
        /// <param name="cardWidth">Width of the card</param>
        /// <param name="cardHeight">Height of the card</param>
        /// <param name="isRightSwipeDirection">Boolean to check whether the swipe direction is right or left</param>
        /// <param name="isBottomSwipeDirection">Boolean to check whether the swipe direction is bottom or top</param>
        /// <param name="cardHorizontalPadding">Horizontal padding for the card</param>
        /// <param name="cardVerticalPadding">Vertical padding for the card</param>
        /// <param name="canSwipeHorizontally">Boolean to check whether the card can swiped horizontally.</param>
        /// <param name="isSwipedCard">Boolean to check whether the card is swiped card.</param>
        /// <returns>Returns the bounds of the swiped card</returns>
        Rect GetSwipedCardBounds(Rect bounds, double xPosition, double yPosition, double cardWidth, double cardHeight, bool isRightSwipeDirection, bool isBottomSwipeDirection, int cardHorizontalPadding, int cardVerticalPadding, bool canSwipeHorizontally, bool isSwipedCard)
        {
            //// For cards after the visible card, arrange them as swiped cards based on the SwipeDirection property.
            int swipedCardSize = ShowSwipedCard ? SwipedCardSize : 0;
            if (canSwipeHorizontally)
            {
                double swipedCardXPosition = isRightSwipeDirection ? bounds.Width - swipedCardSize : -bounds.Width + cardHorizontalPadding + Padding.HorizontalThickness + swipedCardSize;
                return new Rect(swipedCardXPosition + (isSwipedCard ? _position : 0), yPosition, cardWidth, cardHeight);
            }
            else
            {
                double swipedCardYPosition = isBottomSwipeDirection ? bounds.Height - swipedCardSize : -bounds.Height + cardVerticalPadding + Padding.VerticalThickness + swipedCardSize;
                return new Rect(xPosition, swipedCardYPosition + (isSwipedCard ? _position : 0), cardWidth, cardHeight);
            }
        }

        /// <summary>
        /// Trigger the layout measure and arrange operations.
        /// </summary>
        void InvalidateCardLayout()
        {
            //// In android platform sometime the InvalidateMeasure doesn't trigger the layout measure.So the view doesn't renderer properly.
            //// Hence calling measure and arrange directly without InvalidateMeasure.
            //// Example: If we add the card layout in the grid, stack or other layout and change it's properties dynamically, the card layout updates properly.
            //// But if we renders the card layout alone and change it's properties dynamically, the card view doesn't update properly.
            //// Because the InvalidateMeasure doesn't trigger the layout measure in android.So the view doesn't renderer properly.
#if ANDROID
            LayoutMeasure(Width, Height);
            LayoutArrangeChildren(new Rect(0, 0, Width, Height));
#else
            InvalidateMeasure();
#endif
        }

        /// <summary>
        /// Method to check whether the card can be swiped horizontally.
        /// </summary>
        /// <returns>Returns whether cards can swipe horizontally</returns>
        bool CanSwipeHorizontally()
        {
            return SwipeDirection == CardSwipeDirection.Left || SwipeDirection == CardSwipeDirection.Right;
        }

        /// <summary>
        /// Method to check whether the card swipe direction is right.
        /// </summary>
        /// <param name="canSwipeHorizontally">Boolean to check whether the card can swiped horizontally.</param>
        /// <returns>Return the right swipe direction.</returns>
        bool IsRightSwipeDirection(bool canSwipeHorizontally)
        {
            bool isRightSwipeDirection = SwipeDirection == CardSwipeDirection.Right;
            bool isRTL = this.IsRTL();
            //// The children of the card layout arranged based on the flow direction instead of the swipe direction.
            //// So the swipe direction will be reversed in all platform if the flow direction is RTL.
            if (canSwipeHorizontally && isRTL)
            {
                isRightSwipeDirection = !isRightSwipeDirection;
            }

            return isRightSwipeDirection;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to measure the content of the card layout.
        /// </summary>
        /// <param name="widthConstraint">The available width</param>
        /// <param name="heightConstraint">The available height</param>
        /// <returns>The actual size</returns>
        internal override Size LayoutMeasure(double widthConstraint, double heightConstraint)
        {
            double width = double.IsFinite(widthConstraint) ? widthConstraint : 200;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : 200;
            double childWidth = width - Padding.HorizontalThickness;
            double childHeight = height - Padding.VerticalThickness;
            //// Getting the child count of the card layout.
            int childCount = Children.Count;
            //// Get the actual visible card index when the VisibleCardIndex is less than 0 or greater than the child count.
            int visibleCardIndex = GetValidVisibleCardIndex(VisibleIndex);
            if (_isBackwardInteraction)
            {
                visibleCardIndex = visibleCardIndex + 1;
            }

            //// Updating the visible index initially when the index is -1.
            VisibleIndex = VisibleIndex == -1 ? visibleCardIndex : VisibleIndex;
            int horizontalCardSpacing = GetValidSpacing(HorizontalCardSpacing);
            int verticalCardSpacing = GetValidSpacing(VerticalCardSpacing);
            //// Calculating the maximum padding for the card view in the horizontal and vertical direction.
            int cardHorizontalPadding = (VisibleCardsCount - 1) * horizontalCardSpacing;
            int cardVerticalPadding = (VisibleCardsCount - 1) * verticalCardSpacing;
            //// Boolean to check whether the cards can be swiped horizontally or not.
            bool canSwipeHorizontally = CanSwipeHorizontally();
            //// Calculate the maximum width and height for the card view.
            double cardWidth = canSwipeHorizontally ? childWidth - cardHorizontalPadding : childWidth;
            double cardHeight = canSwipeHorizontally ? childHeight : childHeight - cardVerticalPadding;
            if (_position == 0)
            {
                //// Calculate the maximum card index based on the visible card index and the ShowSwipedCard property.
                int maximumCardIndex = visibleCardIndex + 1;
                maximumCardIndex = maximumCardIndex >= childCount ? childCount - 1 : maximumCardIndex;
                //// Calculate the minimum card index based on the visible card index.
                //// Here the value 1 is added to the VisibleCardsCount to get the first valid card index starting from 0.
                //// Example: If the visible card index is 3, then the minimum card index will be 1.
                //// If we just subtract the VisibleCardsCount from the visible card index, then the minimum card index will be 0.
                //// But the minimum card index should be 1, and the card at index 0 will be hidden.
                //// So we are adding 1 to the VisibleCardsCount to get the first valid card index.
                int minimumCardIndex = visibleCardIndex - VisibleCardsCount + 1;
                minimumCardIndex = minimumCardIndex < 0 ? 0 : minimumCardIndex;
                for (int cardIndex = 0; cardIndex < childCount; cardIndex++)
                {
                    var child = Children[cardIndex];
                    //// Skip cards that are outside the visible range.
                    if (cardIndex > maximumCardIndex || cardIndex < minimumCardIndex)
                    {
                        if (child.Opacity != 0)
                        {
                            CardsHelper.UpdateCardOpacity(0, (View)child);
                        }

                        continue;
                    }

                    if (child.Opacity != 1)
                    {
                        CardsHelper.UpdateCardOpacity(1, (View)child);
                    }

                    if (cardIndex > visibleCardIndex)
                    {
                        //// For cards after the visible card, measure with the full card width and maximum height.
                        child.Measure(cardWidth, cardHeight);
                    }
                    else
                    {
                        //// For cards within the visible range. The cards measure with the width and height reduced by the padding based on the card index.
                        if (canSwipeHorizontally)
                        {
                            //// Top and bottom padding of the cards based on the card index.
                            double verticalPadding = (visibleCardIndex - cardIndex) * verticalCardSpacing;
                            //// Space which is need to reduced from the height based on the swipe direction.
                            double verticalSpacing = 2 * verticalPadding;
                            cardHeight = cardHeight - verticalSpacing;
                        }
                        else
                        {
                            //// Left and right padding of the cards based on the card index.
                            double horizontalPadding = (visibleCardIndex - cardIndex) * horizontalCardSpacing;
                            //// Space which is need to reduced from the width based on the swipe direction.
                            double horizontalSpacing = 2 * horizontalPadding;
                            cardWidth = cardWidth - horizontalSpacing;
                        }

                        child.Measure(cardWidth, cardHeight);
                    }
                }
            }
            else
            {
                bool isCardDismissing = IsCardDismissing(_position);
                int swipingCardIndex = isCardDismissing ? visibleCardIndex : visibleCardIndex + 1;
                //// Getting the starting index based on the card dismissing or retrieving.
                //// Example: If the visible card index is 3 and the card is dismissing, then the starting index will be 1.
                //// If the card is retrieving and the animation is not started, then the starting index will be 1.
                //// If the animation is started then last card in the view must be removed so the starting index will be 2.
                int startCardIndex = isCardDismissing ? visibleCardIndex - (VisibleCardsCount - 1) : visibleCardIndex - (VisibleCardsCount - 1) + (_isNavigationAnimationStarted ? 1 : 0);
                //// End index will be the index of the swiped card.
                int endIndex = visibleCardIndex + 1;
                //// Getting the valid starting index of the card range
                startCardIndex = startCardIndex < 0 ? 0 : startCardIndex;
                //// Getting the valid end index of the card range based on the child count.
                endIndex = endIndex >= childCount ? childCount - 1 : endIndex;
                for (int cardIndex = 0; cardIndex < childCount; cardIndex++)
                {
                    var child = Children[cardIndex];
                    if (cardIndex < startCardIndex || cardIndex > endIndex)
                    {
                        if (child.Opacity != 0)
                        {
                            CardsHelper.UpdateCardOpacity(0, (View)child);
                        }

                        continue;
                    }

                    if (child.Opacity != 1)
                    {
                        CardsHelper.UpdateCardOpacity(1, (View)child);
                    }

                    //// Here we are defining the cards that should have the maximum height.
                    //// If we are retrieving the card then that card should have the maximum height.
                    //// If we are dismissing the card then that card should have the maximum height.
                    if (cardIndex > visibleCardIndex || cardIndex == swipingCardIndex)
                    {
                        //// For cards after the visible card, measure with the full card width and maximum height.
                        child.Measure(cardWidth, cardHeight);
                    }
                    //// Here we are defining the cards that should have the reduced height.
                    //// Example: If the visible card index is 3 and that card is dismissing, then the card at index 2 and 1 should have the reduced height.
                    //// The height that need to reduced can be get from the current index that is dismissing or retrieving.
                    //// If the card is dismissing then the value reduced from the height of the card index 2 is (3 - 2) * CardPadding = 1 * CardPadding = 10,
                    //// for index 1 is (3 - 1) * CardPadding = 2 * CardPadding = 20.
                    //// If the card is retrieving then the current index visible card index till the animation started.
                    //// There will be no change in the height of the cards when the card is retrieving.
                    //// Once the animation started the current index changed to the retrieved card index, then the padding will be calculated based on that index like above.
                    //// Now the current index is 4, so the padding for the card index 3 is (4 - 3) * CardPadding = 1 * CardPadding = 10.
                    //// For card index 2 is (4 - 2) * CardPadding = 2 * CardPadding = 20.
                    else
                    {
                        int currentIndex = isCardDismissing ? visibleCardIndex : visibleCardIndex + (_isNavigationAnimationStarted ? 1 : 0);
                        //// For cards within the visible range. The cards measure with the full width or height reduced by the padding based on the card index and the swipe direction.
                        if (canSwipeHorizontally)
                        {
                            //// Top and bottom padding of the cards based on the card index.
                            double verticalPadding = (currentIndex - cardIndex) * verticalCardSpacing;
                            //// Space which is need to reduced from the height based on the swipe direction.
                            double verticalSpacing = 2 * verticalPadding;
                            cardHeight = cardHeight - verticalSpacing;
                        }
                        else
                        {
                            //// Left and right padding of the cards based on the card index.
                            double horizontalPadding = (currentIndex - cardIndex) * horizontalCardSpacing;
                            //// Space which is need to reduced from the width based on the swipe direction.
                            double horizontalSpacing = 2 * horizontalPadding;
                            cardWidth = cardWidth - horizontalSpacing;
                        }

                        child.Measure(cardWidth, cardHeight);
                    }
                }
            }

            return new Size(width, height);
        }

        /// <summary>
        /// Method to arrange the content of the card layout.
        /// </summary>
        /// <param name="bounds">The available size</param>
        /// <returns>The actual size</returns>
        internal override Size LayoutArrangeChildren(Rect bounds)
        {
            //// Calculate the available width and height for the card view.
            double width = bounds.Width - Padding.HorizontalThickness;
            double height = bounds.Height - Padding.VerticalThickness;
            //// Get the actual visible card index when the VisibleCardIndex is less than 0 or greater than the child count.
            int visibleCardIndex = GetValidVisibleCardIndex(_isVisibleIndexAnimation ? _isNavigationAnimationStarted ? VisibleIndex - 1 : VisibleIndex : VisibleIndex);
            if (_isBackwardInteraction)
            {
                visibleCardIndex = GetValidVisibleCardIndex(VisibleIndex + 1);
            }

            int horizontalCardSpacing = GetValidSpacing(HorizontalCardSpacing);
            int verticalCardSpacing = GetValidSpacing(VerticalCardSpacing);
            //// Calculating the maximum padding for the card view in the horizontal and vertical direction.
            int cardHorizontalPadding = (VisibleCardsCount - 1) * horizontalCardSpacing;
            int cardVerticalPadding = (VisibleCardsCount - 1) * verticalCardSpacing;
            //// Boolean to check whether the cards can be swiped horizontally or not.
            bool canSwipeHorizontally = CanSwipeHorizontally();
            //// Calculate the maximum width and height for the card view.
            double cardWidth = canSwipeHorizontally ? width - cardHorizontalPadding : width;
            double cardHeight = canSwipeHorizontally ? height : height - cardVerticalPadding;
            //// Boolean value to indicate whether the swipe direction is right or left.
            bool isRightSwipeDirection = IsRightSwipeDirection(canSwipeHorizontally);
            //// Boolean value to indicate whether the swipe direction is bottom or top.
            bool isBottomSwipeDirection = SwipeDirection == CardSwipeDirection.Bottom;
            int childCount = Children.Count;
            //// Get the xPosition difference between the cards based on the swipe direction.
            double horizontalOffsetDifference = isRightSwipeDirection ? horizontalCardSpacing : -horizontalCardSpacing;
            double verticalOffsetDifference = isBottomSwipeDirection ? verticalCardSpacing : -verticalCardSpacing;
            //// Calculating the x and y position for the cards.
            //// Adjust the initial xPosition based on the swipe direction and visible cards count.
            //// If the swipe direction is right then the card should be arranged from right to left based on the CardPadding
            //// else the card should be arranged from left to right based on the CardPadding.
            double xPosition = Padding.Left;
            double yPosition = Padding.Top;

            if (canSwipeHorizontally)
            {
                xPosition = xPosition + (isRightSwipeDirection ? 0 : cardHorizontalPadding);
            }
            else
            {
                yPosition = yPosition + (isBottomSwipeDirection ? 0 : cardVerticalPadding);
            }

            //// Calculate the index difference between the maximum visible card index and the total visible cards count.
            //// The index difference gives the number of cards that are missing from the visible range.
            //// if the index difference is greater than 0 which means cards count is less than VisibleCardsCount, adjust the xPosition based on the index difference and offset difference.
            int indexDifference = VisibleCardsCount - 1 - visibleCardIndex;
            //// If the index difference is greater than 0, adjust the xPosition based on the index difference and offset difference.
            //// This is to adjust the xPosition of the cards when the visible cards are less than the total visible cards count.
            if (indexDifference > 0)
            {
                if (canSwipeHorizontally)
                {
                    xPosition += indexDifference * (horizontalOffsetDifference / 2);
                }
                else
                {
                    yPosition += indexDifference * (verticalOffsetDifference / 2);
                }
            }

            if (_position == 0)
            {
                //// Calculate the maximum card index based on the visible card index and the ShowSwipedCard property.
                int maximumCardIndex = visibleCardIndex + 1;
                maximumCardIndex = maximumCardIndex >= childCount ? childCount - 1 : maximumCardIndex;
                //// Calculate the minimum card index based on the visible card index.
                //// Here the value 1 is added to the VisibleCardsCount to get the first valid card index starting from 0.
                //// Example: If the visible card index is 3, then the minimum card index will be 1.
                //// If we just subtract the VisibleCardsCount from the visible card index, then the minimum card index will be 0.
                //// But the minimum card index should be 1, and the card at index 0 will be hidden.
                //// So we are adding 1 to the VisibleCardsCount to get the first valid card index.
                int minimumCardIndex = visibleCardIndex - VisibleCardsCount + 1;
                minimumCardIndex = minimumCardIndex < 0 ? 0 : minimumCardIndex;
                for (int cardIndex = 0; cardIndex < childCount; cardIndex++)
                {
                    var child = Children[cardIndex];
                    //// Skip and hide cards that are outside the visible range by setting their opacity to 0.
                    if (cardIndex > maximumCardIndex || cardIndex < minimumCardIndex)
                    {
                        continue;
                    }

                    if (cardIndex > visibleCardIndex)
                    {
                        //// If the swipe direction is right, the swiped card is arranged at the right end of the layout by subtracting the swiped card width from the layout width.
                        //// else the swiped card is arranged at the left end of the layout by adding the specified paddings.
                        Rect swipedCardBounds = GetSwipedCardBounds(bounds, xPosition, yPosition, cardWidth, cardHeight, isRightSwipeDirection, isBottomSwipeDirection, cardHorizontalPadding, cardVerticalPadding, canSwipeHorizontally, false);
                        child.Arrange(swipedCardBounds);
                    }
                    else
                    {
                        //// For cards within the visible range. The cards measure with the full width or height reduced by the padding based on the card index and the swipe direction.
                        if (canSwipeHorizontally)
                        {
                            //// Top and bottom padding is calculated based on the card index and the vertical card spacing.
                            double verticalPadding = (visibleCardIndex - cardIndex) * verticalCardSpacing;
                            //// Space needed to be reduced from the card height based on the swipe direction.
                            double verticalSpacing = 2 * verticalPadding;
                            child.Arrange(new Rect(xPosition, yPosition + verticalPadding, cardWidth, cardHeight - verticalSpacing));
                            xPosition += horizontalOffsetDifference;
                        }
                        else
                        {
                            //// Left and right padding is calculated based on the card index and the horizontal card spacing.
                            double horizontalPadding = (visibleCardIndex - cardIndex) * horizontalCardSpacing;
                            //// Space needed to be reduced from the card width based on the swipe direction.
                            double horizontalSpacing = 2 * horizontalPadding;
                            child.Arrange(new Rect(xPosition + horizontalPadding, yPosition, cardWidth - horizontalSpacing, cardHeight));
                            yPosition += verticalOffsetDifference;
                        }
                    }
                }
            }
            else
            {
                //// Checking whether the card is dismissing or retrieving.
                bool isCardDismissing = IsCardDismissing(_position);
                int swipingCardIndex = isCardDismissing ? visibleCardIndex : visibleCardIndex + 1;
                //// Getting the starting index based on the card dismissing or retrieving.
                //// Example: If the visible card index is 3 and the card is dismissing, then the starting index will be 1.
                //// If the card is retrieving and the animation is not started, then the starting index will be 1.
                //// If the animation is started then last card in the view must be removed so the starting index will be 2.
                int startCardIndex = isCardDismissing ? visibleCardIndex - (VisibleCardsCount - 1) : visibleCardIndex - (VisibleCardsCount - 1) + (_isNavigationAnimationStarted ? 1 : 0);
                //// End index will be the index of the swiped card.
                int endIndex = _isVisibleIndexAnimation ? visibleCardIndex + 2 : visibleCardIndex + 1;
                if (_isBackwardInteraction)
                {
                    endIndex = visibleCardIndex + 1;
                }

                //// Getting the valid starting index of the card range
                startCardIndex = startCardIndex < 0 ? 0 : startCardIndex;
                //// Getting the valid end index of the card range based on the child count.
                endIndex = endIndex >= childCount ? childCount - 1 : endIndex;
                for (int cardIndex = startCardIndex; cardIndex <= endIndex; cardIndex++)
                {
                    var child = Children[cardIndex];
                    bool isSwipedCard = cardIndex == swipingCardIndex;
                    if (cardIndex > visibleCardIndex)
                    {
                        Rect swipedCardBounds = GetSwipedCardBounds(bounds, xPosition, yPosition, cardWidth, cardHeight, isRightSwipeDirection, isBottomSwipeDirection, cardHorizontalPadding, cardVerticalPadding, canSwipeHorizontally, isSwipedCard);
                        child.Arrange(swipedCardBounds);
                    }
                    //// Here we are defining the cards that should have the maximum height.
                    //// If we are dismissing the card then that card should have the maximum height.
                    //// Below condition satisfy only for the dismissing the current visible index card.
                    else if (isSwipedCard && cardIndex == visibleCardIndex)
                    {
                        if (canSwipeHorizontally)
                        {
                            xPosition = xPosition + ((indexDifference > 0 ? cardIndex : VisibleCardsCount - 1) * (isRightSwipeDirection ? horizontalCardSpacing : -horizontalCardSpacing));
                            child.Arrange(new Rect(xPosition + _position, yPosition, cardWidth, cardHeight));
                        }
                        else
                        {
                            yPosition = yPosition + ((indexDifference > 0 ? cardIndex : VisibleCardsCount - 1) * (isBottomSwipeDirection ? verticalCardSpacing : -verticalCardSpacing));
                            child.Arrange(new Rect(xPosition, yPosition + _position, cardWidth, cardHeight));
                        }
                    }
                    //// Here we are defining the cards that should have the reduced height.
                    //// Example: If the visible card index is 3 and that card is dismissing, then the card at index 2 and 1 should have the reduced height.
                    //// The height that need to reduced can be get from the current index that is dismissing or retrieving.
                    //// If the card is dismissing then the value reduced from the height of the card index 2 is (3 - 2) * CardPadding = 1 * CardPadding = 10,
                    //// for index 1 is (3 - 1) * CardPadding = 2 * CardPadding = 20.
                    //// If the card is retrieving then the current index visible card index till the animation started.
                    //// There will be no change in the height of the cards when the card is retrieving.
                    //// Once the animation started the current index changed to the retrieved card index, then the padding will be calculated based on that index like above.
                    //// Now the current index is 4, so the padding for the card index 3 is (4 - 3) * CardPadding = 1 * CardPadding = 10.
                    //// For card index 2 is (4 - 2) * CardPadding = 2 * CardPadding = 20.
                    else if (_isNavigationAnimationStarted)
                    {
                        int currentIndex = isCardDismissing ? visibleCardIndex : visibleCardIndex + (_isNavigationAnimationStarted ? 1 : 0);
                        //// For cards within the visible range. The cards measure with the full width or height reduced by the padding based on the card index and the swipe direction.
                        if (canSwipeHorizontally)
                        {
                            //// Top and bottom padding is calculated based on the card index and the vertical card spacing.
                            double verticalPadding = (currentIndex - cardIndex) * verticalCardSpacing;
                            //// Space needed to be reduced from the card height based on the swipe direction.
                            double verticalSpacing = 2 * verticalPadding;
                            //// Additional height needed to be added to the card during the animation.
                            double additionalHeight = 2 * verticalCardSpacing;
                            if (isCardDismissing)
                            {
                                child.Arrange(new Rect(xPosition + (isSwipedCard ? _position : 0) + ((indexDifference >= 0 ? horizontalOffsetDifference / 2 : horizontalOffsetDifference) * _animationValue), yPosition + verticalPadding - (verticalCardSpacing * _animationValue), cardWidth, cardHeight - verticalSpacing + (additionalHeight * _animationValue)));
                            }
                            else
                            {
                                child.Arrange(new Rect(xPosition + (indexDifference > 0 ? 0 : horizontalOffsetDifference) + (isSwipedCard ? _position : 0) - ((indexDifference > 0 ? horizontalOffsetDifference / 2 : horizontalOffsetDifference) * _animationValue), yPosition + verticalPadding - verticalCardSpacing + (verticalCardSpacing * _animationValue), cardWidth, cardHeight - verticalSpacing + additionalHeight - (additionalHeight * _animationValue)));
                            }

                            xPosition += horizontalOffsetDifference;
                        }
                        else
                        {
                            //// Left and right padding is calculated based on the card index and the horizontal card spacing.
                            double horizontalPadding = (currentIndex - cardIndex) * horizontalCardSpacing;
                            //// Space needed to be reduced from the card width based on the swipe direction.
                            double horizontalSpacing = 2 * horizontalPadding;
                            //// Additional width needed to be added to the card width during the animation.
                            double additionalWidth = 2 * horizontalCardSpacing;
                            if (isCardDismissing)
                            {
                                child.Arrange(new Rect(xPosition + horizontalPadding - (horizontalCardSpacing * _animationValue), yPosition + (isSwipedCard ? _position : 0) + ((indexDifference >= 0 ? verticalOffsetDifference / 2 : verticalOffsetDifference) * _animationValue), cardWidth - horizontalSpacing + (additionalWidth * _animationValue), cardHeight));
                            }
                            else
                            {
                                child.Arrange(new Rect(xPosition + horizontalPadding - horizontalCardSpacing + (horizontalCardSpacing * _animationValue), yPosition + (indexDifference > 0 ? 0 : verticalOffsetDifference) + (isSwipedCard ? _position : 0) - ((indexDifference > 0 ? verticalOffsetDifference / 2 : verticalOffsetDifference) * _animationValue), cardWidth - horizontalSpacing + additionalWidth - (additionalWidth * _animationValue), cardHeight));
                            }

                            yPosition += verticalOffsetDifference;
                        }
                    }
                }
            }

            return bounds.Size;
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Invokes on swipe direction property changed.
        /// </summary>
        /// <param name="bindable">The SfCardLayout object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnSwipeDirectionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardLayout = bindable as SfCardLayout;
            if (cardLayout == null || cardLayout.Children.Count == 0)
            {
                return;
            }

            cardLayout.InvalidateCardLayout();
        }

        /// <summary>
        /// Invokes on show swiped card property changed.
        /// </summary>
        /// <param name="bindable">The SfCardLayout object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnShowSwipedCardPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardLayout = bindable as SfCardLayout;
            if (cardLayout == null)
            {
                return;
            }

            int childCount = cardLayout.Children.Count;
            int visibleCardIndex = cardLayout.GetValidVisibleCardIndex(cardLayout.VisibleIndex);
            if (childCount == 0 || visibleCardIndex == childCount - 1)
            {
                return;
            }

            cardLayout.InvalidateCardLayout();
        }

        /// <summary>
        /// Invokes on visible card index property changed.
        /// </summary>
        /// <param name="bindable">The SfCardLayout object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnVisibleIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardLayout = bindable as SfCardLayout;
            if (cardLayout == null)
            {
                return;
            }

            //// Get the actual old and new visible card index when the index is less than 0 or greater than the child count.
            //// If the visibleCardIndex is updated dynamically beyond the child index then the old and new visible card index will be actual index.
            int? oldIndex = cardLayout.GetValidVisibleCardIndex((int?)oldValue);
            int? newIndex = cardLayout.GetValidVisibleCardIndex((int?)newValue);

            //// If the old index and new index are same then return.
            //// If visible card index keeps on updating dynamically then the old index and new index will be same and there is no need to update the card layout and invoke the Visible card changed event.
            if (oldIndex == newIndex)
            {
                return;
            }

            oldIndex = oldIndex >= 0 ? oldIndex : null;
            newIndex = newIndex >= 0 ? newIndex : null;
            int? difference = newIndex - oldIndex;
            cardLayout.VisibleIndexChanged?.Invoke(cardLayout, new CardVisibleIndexChangedEventArgs { OldIndex = oldIndex, NewIndex = newIndex });
            if (cardLayout._isVisibleIndexAnimation)
            {
                bool isGreaterVisibleIndex = false;
                if (newIndex > oldIndex)
                {
                    isGreaterVisibleIndex = true;
                }

                cardLayout.GetVisibleIndexAnimation(isGreaterVisibleIndex, difference, oldIndex == 0 ? true : false);
            }
            else
            {
                cardLayout.InvalidateCardLayout();
                cardLayout._isVisibleIndexAnimation = true;
            }
        }

        /// <summary>
        /// Invokes on horizontal card spacing property changed.
        /// </summary>
        /// <param name="bindable">The SfCardLayout object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnHorizontalCardSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardLayout = bindable as SfCardLayout;
            if (cardLayout == null)
            {
                return;
            }

            cardLayout.InvalidateCardLayout();
        }

        /// <summary>
        /// Invokes on vertical card spacing property changed.
        /// </summary>
        /// <param name="bindable">The SfCardLayout object</param>
        /// <param name="oldValue">Property old value</param>
        /// <param name="newValue">Property new value</param>
        static void OnVerticalCardSpacingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var cardLayout = bindable as SfCardLayout;
            if (cardLayout == null)
            {
                return;
            }

            cardLayout.InvalidateCardLayout();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever the card layout is tapped.
        /// </summary>
        public event EventHandler<TappedEventArgs>? Tapped;

        /// <summary>
        /// Occurs when the card is swiped.
        /// </summary>
        public event EventHandler<CardVisibleIndexChangedEventArgs>? VisibleIndexChanged;

        /// <summary>
        /// Occurs when the card is swiping.
        /// </summary>
        public event EventHandler<CardVisibleIndexChangingEventArgs>? VisibleIndexChanging;

        #endregion
    }
}
