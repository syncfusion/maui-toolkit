using Syncfusion.Maui.Toolkit.Cards;
using System;
using System.Reflection;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Toolkit.UnitTest
{

    public class CardsLayoutUnitTests : BaseUnitTest
    {
        #region Public Properties

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            SfCardLayout cardsLayout = new SfCardLayout();

            Assert.Equal(10, cardsLayout.HorizontalCardSpacing);
            Assert.True(cardsLayout.ShowSwipedCard);
            Assert.Equal(CardSwipeDirection.Right, cardsLayout.SwipeDirection);
            Assert.Equal(10, cardsLayout.VerticalCardSpacing);
            Assert.Equal(-1, cardsLayout.VisibleIndex);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void HorizontalCardSpacing_GetAndSet_UsingInt(int expectedValue)
        {
            SfCardLayout cardsLayout = new SfCardLayout();

            cardsLayout.HorizontalCardSpacing = expectedValue;
            int actualValue = cardsLayout.HorizontalCardSpacing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void VerticalCardSpacing_GetAndSet_UsingInt(int expectedValue)
        {
            SfCardLayout cardsLayout = new SfCardLayout();

            cardsLayout.VerticalCardSpacing = expectedValue;
            int actualValue = cardsLayout.VerticalCardSpacing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(2)]
        [InlineData(-4)]
        [InlineData(-8)]
        [InlineData(0)]
        public void VisibleIndex_GetAndSet_UsingInt(int expectedValue)
        {
            SfCardLayout cardsLayout = new SfCardLayout();

            cardsLayout.VisibleIndex = expectedValue;
            int actualValue = (int)cardsLayout.VisibleIndex;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShowSwipedCard_GetAndSet_UsingBool(bool expectedValue)
        {
            SfCardLayout cardsLayout = new SfCardLayout();

            cardsLayout.ShowSwipedCard = expectedValue;
            bool actualValue = cardsLayout.ShowSwipedCard;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(CardSwipeDirection.Left)]
        [InlineData(CardSwipeDirection.Right)]
        [InlineData(CardSwipeDirection.Top)]
        [InlineData(CardSwipeDirection.Bottom)]
        public void SwipeDirection_GetAndSet_UsingCardSwipDirection(CardSwipeDirection expectedValue)
        {
            SfCardLayout cardsLayout = new SfCardLayout();

            cardsLayout.SwipeDirection = expectedValue;
            CardSwipeDirection actualValue = cardsLayout.SwipeDirection;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Methods

        [Fact]
        public void Forward_Without_Children_ChangesCard_WhenCalled()
        {
            SfCardLayout cardLayout = new SfCardLayout();

            cardLayout.Forward();

            Assert.Equal(-1, cardLayout.VisibleIndex);
        }

        [Fact]
        public void Backward_Without_Children_ChangesCard_WhenCalled()
        {
            SfCardLayout cardLayout = new SfCardLayout();

            cardLayout.Backward();

            Assert.Equal(-1, cardLayout.VisibleIndex);
        }

        [Fact]
        public void Forward_With_Children_ChangesCard_WhenCalled()
        {
            SfCardLayout cardLayout = new SfCardLayout();

            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            Assert.Throws<ArgumentException>(() => cardLayout.VisibleIndex = 2);
            Assert.Throws<ArgumentException>(() => cardLayout.Forward());
            cardLayout.Children.Clear();
        }

        [Fact]
        public void Backward_With_Children_ChangesCard_WhenCalled()
        {
            SfCardLayout cardLayout = new SfCardLayout();

            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            Assert.Throws<ArgumentException>(() => cardLayout.VisibleIndex = 2);
            Assert.Throws<ArgumentException>(() => cardLayout.Backward());
            cardLayout.Children.Clear();
        }

        [Fact]
        public void UpdateVisibleCardIndex_If_SetsIndex_WhenCalled()
        {
            SfCardLayout cardLayout = new SfCardLayout();

            this.InvokePrivateMethod(cardLayout, "UpdateVisibleCardIndex", 5, true);

            Assert.Equal(4, cardLayout.VisibleIndex);
        }

        [Fact]
        public void UpdateVisibleCardIndex_Negative_SetsIndex_WhenCalled()
        {
            SfCardLayout cardLayout = new SfCardLayout();

            this.InvokePrivateMethod(cardLayout, "UpdateVisibleCardIndex", 0, true);

            Assert.Null(cardLayout.VisibleIndex);
        }

        [Fact]
        public void UpdateVisibleCardIndex_Positive_SetsIndex_WhenCalled()
        {
            SfCardLayout cardLayout = new SfCardLayout();

            this.InvokePrivateMethod(cardLayout, "UpdateVisibleCardIndex", 5, false);

            Assert.Equal(6, cardLayout.VisibleIndex);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, false, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void TouchCompletedCases_Starts_WhenCalled(bool swipeDirection, bool swipeHorizontal, bool isRtl )
        {
            SfCardLayout cardLayout = new SfCardLayout();

            this.SetPrivateField(cardLayout, "_position", 5);
            var exception = Assert.ThrowsAny<TargetInvocationException>(() => this.InvokePrivateMethod(cardLayout, "TouchCompleted", new Point(500, 40), swipeDirection, swipeHorizontal, isRtl));
        }

		[Fact]
		public void OnTap_TriggersTap_WhenCalled()
		{
			SfCardLayout cardsLayout = new SfCardLayout();
			var fired = false;

			cardsLayout.FlowDirection = FlowDirection.RightToLeft;
			cardsLayout.Children.Add(new SfCardView());
			cardsLayout.Children.Add(new SfCardView());
			cardsLayout.Children.Add(new SfCardView());
			cardsLayout.Children.Add(new SfCardView());
			Assert.Throws<ArgumentException>(() => cardsLayout.VisibleIndex = 2);
			cardsLayout.Tapped += (sender, e) => fired = true;
			cardsLayout.OnTap(new Internals.TapEventArgs(new Point(45, 76), 1));

			Assert.True(fired);
		}

        #endregion

        #region Events

        [Fact]
        public void Tapped_TriggersTap_WhenCalled()
        {
            SfCardLayout cardsLayout = new SfCardLayout();

            var fired = false;
            cardsLayout.Tapped += (sender, e) => fired = true;
			cardsLayout.Children.Add(new SfCardView());
			cardsLayout.Children.Add(new SfCardView());
			cardsLayout.Children.Add(new SfCardView());
			cardsLayout.Children.Add(new SfCardView());
			Assert.Throws<ArgumentException>(() => cardsLayout.VisibleIndex = 2);
			cardsLayout.OnTap(new Internals.TapEventArgs(new Point(45, 76), 1));

            Assert.True(fired);
        }

        [Fact]
        public void VisibleIndexChanging_TriggersIndexChanging_WhenCalled()
        {
            SfCardLayout cardLayout = new SfCardLayout();

            var fired = false;
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            Assert.Throws<ArgumentException>(() => cardLayout.VisibleIndex = 2);
            cardLayout.VisibleIndexChanging += (sender, e) => fired = true;
            cardLayout.OnPan(new Internals.PanEventArgs(GestureStatus.Running, new Point(56, 78), new Point(46, 68), new Point(10, 10)));

            Assert.True(fired);
            cardLayout.Children.Clear();
        }

        [Fact]
        public void VisibleIndexChanged_TriggersIndexChanging_WhenCalled()
        {
            SfCardLayout cardLayout = new SfCardLayout();

            var fired = false;
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            cardLayout.Children.Add(new SfCardView());
            cardLayout.VisibleIndexChanged += (sender, e) => fired = true;
            Assert.Throws<ArgumentException>(() => cardLayout.VisibleIndex = 2);

            Assert.True(fired);
            cardLayout.Children.Clear();
        }

        #endregion
    }
}
