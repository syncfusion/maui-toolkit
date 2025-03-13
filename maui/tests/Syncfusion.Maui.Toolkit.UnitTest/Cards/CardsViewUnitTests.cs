using Syncfusion.Maui.Toolkit.Cards;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit;
using System;
using System.Reflection;
namespace Syncfusion.Maui.Toolkit.UnitTest
{

	public class CardsViewUnitTests : BaseUnitTest
	{
		#region Public Properties

		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			SfCardView cardsView = new SfCardView();

			Assert.Equal(Color.FromArgb("#CAC4D0"), cardsView.BorderColor);
			Assert.Equal(1d, cardsView.BorderWidth);
			Assert.Equal(new CornerRadius(5), cardsView.CornerRadius);
			Assert.True(cardsView.FadeOutOnSwiping);
			Assert.Equal(Colors.Transparent, cardsView.IndicatorColor);
			Assert.Equal(CardIndicatorPosition.Left, cardsView.IndicatorPosition);
			Assert.Equal(0d, cardsView.IndicatorThickness);
			Assert.False(cardsView.IsDismissed);
			Assert.False(cardsView.SwipeToDismiss);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		[InlineData(0, 0, 255)]
		[InlineData(255, 255, 0)]
		[InlineData(0, 255, 255)]
		public void BorderColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
		{
			SfCardView cardsView = new SfCardView();

			Color expectedValue = Color.FromRgb(red, green, blue);
			cardsView.BorderColor = expectedValue;
			Color actualValue = cardsView.BorderColor;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		[InlineData(0, 0, 255)]
		[InlineData(255, 255, 0)]
		[InlineData(0, 255, 255)]
		public void IndicatorColor_GetAndSet_UsingColor(byte red, byte green, byte blue)
		{
			SfCardView cardsView = new SfCardView();

			Color expectedValue = Color.FromRgb(red, green, blue);
			cardsView.IndicatorColor = expectedValue;
			Color actualValue = cardsView.IndicatorColor;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(50)]
		[InlineData(250)]
		[InlineData(-40)]
		[InlineData(-850)]
		[InlineData(0)]
		public void BorderWidth_GetAndSet_UsingDouble(double expectedValue)
		{
			SfCardView cardsView = new SfCardView();

			cardsView.BorderWidth = expectedValue;
			double actualValue = cardsView.BorderWidth;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(50)]
		[InlineData(250)]
		[InlineData(-40)]
		[InlineData(-850)]
		[InlineData(0)]
		public void IndicatorThickness_GetAndSet_UsingDouble(double expectedValue)
		{
			SfCardView cardsView = new SfCardView();

			cardsView.IndicatorThickness = expectedValue;
			double actualValue = cardsView.IndicatorThickness;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void FadeOutOnSwiping_GetAndSet_UsingBool(bool expectedValue)
		{
			SfCardView cardsView = new SfCardView();

			cardsView.FadeOutOnSwiping = expectedValue;
			bool actualValue = cardsView.FadeOutOnSwiping;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(50)]
		[InlineData(250)]
		[InlineData(-40)]
		[InlineData(-850)]
		[InlineData(0)]
		public void CornerRadius_GetAndSet_UsingCornerRadius(double radiusValue)
		{
			SfCardView cardsView = new SfCardView();

			CornerRadius expectedValue = new CornerRadius(radiusValue);
			cardsView.CornerRadius = expectedValue;
			CornerRadius actualValue = cardsView.CornerRadius;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsDismissed_GetAndSet_UsingBool(bool expectedValue)
		{
			SfCardView cardsView = new SfCardView();

			cardsView.IsDismissed = expectedValue;
			bool actualValue = cardsView.IsDismissed;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void SwipeToDismiss_GetAndSet_UsingBool(bool expectedValue)
		{
			SfCardView cardsView = new SfCardView();

			cardsView.SwipeToDismiss = expectedValue;
			bool actualValue = cardsView.SwipeToDismiss;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(CardIndicatorPosition.Left)]
		[InlineData(CardIndicatorPosition.Right)]
		[InlineData(CardIndicatorPosition.Top)]
		[InlineData(CardIndicatorPosition.Bottom)]
		public void IndicatorPosition_GetAndSet_UsingCardIndicatorPosition(CardIndicatorPosition expectedValue)
		{
			SfCardView cardsView = new SfCardView();

			cardsView.IndicatorPosition = expectedValue;
			CardIndicatorPosition actualValue = cardsView.IndicatorPosition;

			Assert.Equal(expectedValue, actualValue);
		}

		#endregion

		#region Internal Properties

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		[InlineData(0, 0, 255)]
		[InlineData(255, 255, 0)]
		[InlineData(0, 255, 255)]
		public void CardViewBackground_GetAndSet_UsingColor(byte red, byte green, byte blue)
		{
			SfCardView cardsView = new SfCardView();

			Color expectedValue = Color.FromRgb(red, green, blue);
			cardsView.CardViewBackground = expectedValue;
			Color actualValue = cardsView.CardViewBackground;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(CardDismissDirection.Left)]
		[InlineData(CardDismissDirection.Right)]
		[InlineData(CardDismissDirection.None)]
		public void CardDismissedEventArgs_DismissDirection_GetAndSet_UsingColor(CardDismissDirection expectedValue)
		{
			CardDismissedEventArgs cardDismissedEventArgs = new CardDismissedEventArgs();

			cardDismissedEventArgs.DismissDirection = expectedValue;
			CardDismissDirection actualValue = cardDismissedEventArgs.DismissDirection;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(CardDismissDirection.Left)]
		[InlineData(CardDismissDirection.Right)]
		[InlineData(CardDismissDirection.None)]
		public void CardDismissingEventArgs_DismissDirection_GetAndSet_UsingColor(CardDismissDirection expectedValue)
		{
			CardDismissingEventArgs cardDismissedEventArgs = new CardDismissingEventArgs();

			cardDismissedEventArgs.DismissDirection = expectedValue;
			CardDismissDirection actualValue = cardDismissedEventArgs.DismissDirection;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(2)]
		[InlineData(-4)]
		[InlineData(-8)]
		[InlineData(0)]
		public void CardVisibleIndexChangedEventArgs_OldIndex_GetAndSet_UsingInt(int expectedValue)
		{
			CardVisibleIndexChangedEventArgs cardVisibleIndexChangedEventArgs = new CardVisibleIndexChangedEventArgs();

			cardVisibleIndexChangedEventArgs.OldIndex = expectedValue;
			int actualValue = (int)cardVisibleIndexChangedEventArgs.OldIndex;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(2)]
		[InlineData(-4)]
		[InlineData(-8)]
		[InlineData(0)]
		public void CardVisibleIndexChangedEventArgs_NewIndex_GetAndSet_UsingInt(int expectedValue)
		{
			CardVisibleIndexChangedEventArgs cardVisibleIndexChangedEventArgs = new CardVisibleIndexChangedEventArgs();

			cardVisibleIndexChangedEventArgs.NewIndex = expectedValue;
			int actualValue = (int)cardVisibleIndexChangedEventArgs.NewIndex;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(2)]
		[InlineData(-4)]
		[InlineData(-8)]
		[InlineData(0)]
		public void CardVisibleIndexChangingEventArgs_OldIndex_GetAndSet_UsingInt(int expectedValue)
		{
			CardVisibleIndexChangingEventArgs cardVisibleIndexChangingEventArgs = new CardVisibleIndexChangingEventArgs();

			cardVisibleIndexChangingEventArgs.OldIndex = expectedValue;
			int actualValue = (int)cardVisibleIndexChangingEventArgs.OldIndex;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(5)]
		[InlineData(2)]
		[InlineData(-4)]
		[InlineData(-8)]
		[InlineData(0)]
		public void CardVisibleIndexChangingEventArgs_NewIndex_GetAndSet_UsingInt(int expectedValue)
		{
			CardVisibleIndexChangingEventArgs cardVisibleIndexChangingEventArgs = new CardVisibleIndexChangingEventArgs();

			cardVisibleIndexChangingEventArgs.NewIndex = expectedValue;
			int actualValue = (int)cardVisibleIndexChangingEventArgs.NewIndex;

			Assert.Equal(expectedValue, actualValue);
		}

		#endregion

		#region Methods

		[Theory]
		[InlineData(50, 1)]
		[InlineData(250, 1)]
		[InlineData(-40, 0)]
		[InlineData(-850, 0)]
		[InlineData(0, 0)]
		public void UpdateCardOpacity_SetsOpacity_WhenCalled(float setValue, float expectedValue)
		{
			Grid grid = new Grid();

			CardsHelper.UpdateCardOpacity(setValue, grid);

			Assert.Equal(expectedValue, grid.Opacity);
		}

		[Fact]
		public void IsRTL_If_ReturnsBool_WhenCalled()
		{
			SfCardView cardsView = new SfCardView();

			bool expectedValue = CardsHelper.IsRTL(cardsView);

			Assert.False(expectedValue);
		}

		[Fact]
		public void IsRTL_Else_ReturnsBool_WhenCalled()
		{
			Grid grid = new Grid();

			bool expectedValue = CardsHelper.IsRTL(grid);

			Assert.False(expectedValue);
		}

		[Theory]
		[InlineData(80)]
		[InlineData(400)]
		[InlineData(7500)]
		public void TouchCompleted_ReturnsBool_WhenCalled(int pointX)
		{
			SfCardView cardView = new SfCardView();

			this.SetPrivateField(cardView, "_position", 5);
			var exception = Assert.ThrowsAny<TargetInvocationException>(() => this.InvokePrivateMethod(cardView, "TouchCompleted", new Point(pointX, 40)));
			Assert.IsType<ArgumentException>(exception.InnerException);
		}

		[Fact]
		public void OnPan_ReturnsTouchDownPoint_WhenCalled()
		{
			SfCardView cardView = new SfCardView();

			cardView.OnPan(new Internals.PanEventArgs(GestureStatus.Started, new Point(56, 78), new Point(46, 68), new Point(10, 10)));
			var actualValue = this.GetPrivateField(cardView, "_touchDownPoint");

			Assert.Equal(new Point(56, 78), actualValue);
		}

		[Fact]
		public void OnPanComplete_ReturnsTouchDownPoint_WhenCalled()
		{
			SfCardView cardView = new SfCardView();

			cardView.OnPan(new Internals.PanEventArgs(GestureStatus.Completed, new Point(56, 78), new Point(46, 68), new Point(10, 10)));
			var actualValue = this.GetPrivateField(cardView, "_touchDownPoint");

			Assert.Null(actualValue);
		}

		[Fact]
		public void MeasureContent_ReturnsSize_WhenCalled()
		{
			SfCardView cardView = new SfCardView();

			var actualValue = this.InvokePrivateMethod(cardView, "MeasureContent", 500, 300);

			Assert.Equal(new Size(500, 300), actualValue);
		}

		[Fact]
		public void ArrangeContent_ReturnsSize_WhenCalled()
		{
			SfCardView cardView = new SfCardView();

			var actualValue = this.InvokePrivateMethod(cardView, "ArrangeContent", new Rect(55, 70, 500, 300));

			Assert.Equal(new Size(500, 300), actualValue);
		}

		#endregion

		#region Events

		[Fact]
		public void Dismissing_TriggersIndexChanging_WhenCalled()
		{
			SfCardView cardsView = new SfCardView();

			var fired = false;
			cardsView.Dismissing += (sender, e) => fired = true;
			cardsView.OnPan(new Internals.PanEventArgs(GestureStatus.Running, new Point(56, 78), new Point(46, 68), new Point(10, 10)));

			Assert.True(fired);
		}

		[Fact]
		public void Dismissed_TriggersIndexChanging_WhenCalled()
		{
			SfCardView cardsView = new SfCardView();

			var fired = false;
			cardsView.Dismissed += (sender, e) => fired = true;
			cardsView.IsDismissed = true;

			Assert.True(fired);
		}

		#endregion

		#region Combintions

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		[InlineData(0, 0, 255)]
		[InlineData(255, 255, 0)]
		[InlineData(0, 255, 255)]
		public void BorderColorWithWidth_GetAndSet_UsingColor(byte red, byte green, byte blue)
		{
			SfCardView cardsView = new SfCardView();

			Color expectedValue = Color.FromRgb(red, green, blue);
			cardsView.BorderWidth = 0;
			cardsView.BorderColor = expectedValue;
			Color actualValue = cardsView.BorderColor;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(50)]
		[InlineData(250)]
		[InlineData(-40)]
		[InlineData(-850)]
		[InlineData(0)]
		public void BorderWidthWithColor_GetAndSet_UsingDouble(double expectedValue)
		{
			SfCardView cardsView = new SfCardView();

			cardsView.BorderColor = Colors.Transparent;
			cardsView.BorderWidth = expectedValue;
			double actualValue = cardsView.BorderWidth;

			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public void IsDismissedWithContent_GetAndSet_UsingBool()
		{
			SfCardView cardsView = new SfCardView();

			cardsView.Content = new Label();
			cardsView.IsDismissed = true;
			cardsView.IsDismissed = false;

			Assert.False(cardsView.IsDismissed);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		[InlineData(0, 0, 255)]
		[InlineData(255, 255, 0)]
		[InlineData(0, 255, 255)]
		public void IndicatorColorWithWidth_GetAndSet_UsingColor(byte red, byte green, byte blue)
		{
			SfCardView cardsView = new SfCardView();

			Color expectedValue = Color.FromRgb(red, green, blue);
			cardsView.IndicatorThickness = 10;
			cardsView.IndicatorColor = expectedValue;
			Color actualValue = cardsView.IndicatorColor;

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(50)]
		[InlineData(250)]
		[InlineData(-40)]
		[InlineData(-850)]
		[InlineData(0)]
		public void IndicatorThicknessWithColor_GetAndSet_UsingDouble(double expectedValue)
		{
			SfCardView cardsView = new SfCardView();

			cardsView.IndicatorColor = Colors.Transparent;
			cardsView.IndicatorThickness = expectedValue;
			double actualValue = cardsView.IndicatorThickness;

			Assert.Equal(expectedValue, actualValue);
		}

		#endregion
	}
}
