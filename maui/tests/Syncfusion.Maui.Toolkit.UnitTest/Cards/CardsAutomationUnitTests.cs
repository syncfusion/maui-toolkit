using Syncfusion.Maui.Toolkit.Cards;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class CardsAutomationScenariosUnitTests
	{
		#region Automation Scenarios

		[Fact]
		public void CardsManager_011()
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView() { BorderWidth = 20 };
			
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.VisibleIndex = 2;
			((SfCardView)cardLayout.Children[1]).BorderWidth = 40;

			Assert.Equal(40, ((SfCardView)cardLayout.Children[1]).BorderWidth);
		}

		[Fact]
		public void CardsManager_012()
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView() { BorderColor = Colors.Red };

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.VisibleIndex = 2;
			((SfCardView)cardLayout.Children[1]).BorderColor = Colors.Yellow;

			Assert.Equal(Colors.Yellow, ((SfCardView)cardLayout.Children[1]).BorderColor);
		}

		[Fact]
		public void CardsManager_013()
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView() { IndicatorThickness = 80 };

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.VisibleIndex = 2;
			((SfCardView)cardLayout.Children[1]).IndicatorThickness = 20;

			Assert.Equal(20, ((SfCardView)cardLayout.Children[1]).IndicatorThickness);
		}

		[Theory]
		[InlineData(CardIndicatorPosition.Left)]
		[InlineData(CardIndicatorPosition.Right)]
		[InlineData(CardIndicatorPosition.Bottom)]
		[InlineData(CardIndicatorPosition.Top)]
		public void CardsManager_014_17(CardIndicatorPosition expectedValue)
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView() { IndicatorThickness = 80 };

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.VisibleIndex = 2;
			((SfCardView)cardLayout.Children[1]).IndicatorThickness = 20;
			((SfCardView)cardLayout.Children[1]).IndicatorPosition =  expectedValue;

			Assert.Equal(20, ((SfCardView)cardLayout.Children[1]).IndicatorThickness);
			Assert.Equal(expectedValue, ((SfCardView)cardLayout.Children[1]).IndicatorPosition);
		}

		[Fact]
		public void CardsManager_018()
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView() { IndicatorThickness = 80 };

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.VisibleIndex = 2;
			((SfCardView)cardLayout.Children[1]).IndicatorThickness = 20;
			((SfCardView)cardLayout.Children[1]).IndicatorColor = Colors.Red;

			Assert.Equal(20, ((SfCardView)cardLayout.Children[1]).IndicatorThickness);
			Assert.Equal(Colors.Red, ((SfCardView)cardLayout.Children[1]).IndicatorColor);
		}

		[Fact]
		public void CardsManager_019()
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView() { IndicatorThickness = 80 };

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.VisibleIndex = 2;
			((SfCardView)cardLayout.Children[1]).IndicatorThickness = 20;
			((SfCardView)cardLayout.Children[1]).CornerRadius = new CornerRadius(20);

			Assert.Equal(new CornerRadius(20), ((SfCardView)cardLayout.Children[1]).CornerRadius);
		}

		[Fact]
		public void CardsManager_022()
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView();

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.VisibleIndex = 2;
			cardLayout.VerticalCardSpacing = 20;
			cardLayout.HorizontalCardSpacing = 20;

			Assert.Equal(20,cardLayout.HorizontalCardSpacing);
			Assert.Equal(20,cardLayout.VerticalCardSpacing);
		}

		[Theory]
		[InlineData(CardSwipeDirection.Right)]
		[InlineData(CardSwipeDirection.Left)]
		[InlineData(CardSwipeDirection.Top)]
		[InlineData(CardSwipeDirection.Bottom)]
		public void CardsManager_027_030(CardSwipeDirection setValue)
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView();

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.SwipeDirection = setValue;
			cardLayout.VerticalCardSpacing = 20;

			Assert.Equal(20, cardLayout.VerticalCardSpacing);
		}

		[Theory]
		[InlineData(CardSwipeDirection.Right)]
		[InlineData(CardSwipeDirection.Left)]
		[InlineData(CardSwipeDirection.Top)]
		[InlineData(CardSwipeDirection.Bottom)]
		public void CardsManager_031_034(CardSwipeDirection setValue)
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView();

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.SwipeDirection = setValue;
			cardLayout.HorizontalCardSpacing = 20;

			Assert.Equal(20, cardLayout.HorizontalCardSpacing);
		}

		[Theory]
		[InlineData(CardSwipeDirection.Right)]
		[InlineData(CardSwipeDirection.Left)]
		[InlineData(CardSwipeDirection.Top)]
		[InlineData(CardSwipeDirection.Bottom)]
		public void CardsManager_035_038(CardSwipeDirection setValue)
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView();

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.SwipeDirection = setValue;
			Assert.Throws<ArgumentException>(() => cardLayout.Backward());

			Assert.Equal(-1, cardLayout.VisibleIndex);
		}

		[Theory]
		[InlineData(CardSwipeDirection.Right)]
		[InlineData(CardSwipeDirection.Left)]
		[InlineData(CardSwipeDirection.Top)]
		[InlineData(CardSwipeDirection.Bottom)]
		public void CardsManager_039_042(CardSwipeDirection setValue)
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView();

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.SwipeDirection = setValue;
			Assert.Throws<ArgumentException>(() => cardLayout.Backward());
			cardLayout.Forward();

			Assert.Equal(-1, cardLayout.VisibleIndex);
		}

		[Theory]
		[InlineData(CardSwipeDirection.Right)]
		[InlineData(CardSwipeDirection.Left)]
		[InlineData(CardSwipeDirection.Top)]
		[InlineData(CardSwipeDirection.Bottom)]
		public void CardsManager_043_046(CardSwipeDirection setValue)
		{
			SfCardLayout cardLayout = new SfCardLayout();
			SfCardView cardView = new SfCardView();

			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.Children.Add(cardView);
			cardLayout.SwipeDirection = setValue;
			Assert.Throws<ArgumentException>(() => cardLayout.VisibleIndex = 1);

			Assert.Equal(1, cardLayout.VisibleIndex);
		}

		#endregion
	}
}
