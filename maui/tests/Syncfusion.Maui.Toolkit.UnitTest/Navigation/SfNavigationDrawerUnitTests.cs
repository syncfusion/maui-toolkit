using Syncfusion.Maui.Toolkit.NavigationDrawer;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class SfNavigationDrawerUnitTests : BaseUnitTest
    {
        #region constructor

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();

            Assert.False(navigationDrawer.IsOpen);
            Assert.Null(navigationDrawer.ContentView);
            Assert.NotNull(navigationDrawer.DrawerSettings);
        }

        [Fact]
        public void DrawerSetting_Contructor_InitializesDefaultCorrectly()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;

            Assert.Equal(drawerSetting, navigationDrawer.DrawerSettings);
            Assert.Equal(200d, drawerSetting.DrawerWidth);
            Assert.Equal(500d, drawerSetting.DrawerHeight);
            Assert.Equal(50d, drawerSetting.DrawerHeaderHeight);
            Assert.Equal(50d, drawerSetting.DrawerFooterHeight);
            Assert.Null(drawerSetting.DrawerContentView);
            Assert.Null(drawerSetting.DrawerHeaderView);
            Assert.Null(drawerSetting.DrawerFooterView);
            Assert.Equal(Position.Left, drawerSetting.Position);
            Assert.Equal(400d, drawerSetting.Duration);
            Assert.True(drawerSetting.EnableSwipeGesture);
            Assert.Equal(120d, drawerSetting.TouchThreshold);
            Assert.Equal(Color.FromArgb("F7F2FB"), drawerSetting.ContentBackground);
            Assert.Equal(Transition.SlideOnTop, drawerSetting.Transition);
        }

        #endregion

        #region public properties

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void IsOpen_SetValue_ReturnsExpectedValue(bool isOpen)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.IsOpen = isOpen;

            Assert.Equal(isOpen, navigationDrawer.IsOpen);
        }

        [Fact]
        public void ContentView_SetValue_ReturnsExpectedValue()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var contentView = new ContentView
            {
                Content = new Label { Text = "Navigation Drawer" }
            };
            navigationDrawer.ContentView = contentView;
            Assert.Same(contentView, navigationDrawer.ContentView);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(1000d)]
        [InlineData(0d)]
        public void DrawerHeight_SetValue_ReturnsExpectedValue(double drawerHeight)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            drawerSetting.DrawerHeight = drawerHeight;

            Assert.Equal(drawerSetting.DrawerHeight, drawerHeight);
        }

        [Fact]
        public void ContentBackground_SetValue_ReturnsExpectedValue()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            var contentBackground = Color.FromArgb("#FFF0");
            drawerSetting.ContentBackground = contentBackground;

            Assert.Same(contentBackground, drawerSetting.ContentBackground);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(-1000d)]
        [InlineData(0d)]
        public void DrawerHeaderHeight_SetValue_ReturnsExpectedValue(double drawerHeaderHeight)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            drawerSetting.DrawerHeaderHeight = drawerHeaderHeight;

            Assert.Equal(drawerSetting.DrawerHeaderHeight, drawerHeaderHeight);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(-1000d)]
        [InlineData(0d)]
        public void DrawerFooterrHeight_SetValue_ReturnsExpectedValue(double drawerFooterHeight)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            drawerSetting.DrawerFooterHeight = drawerFooterHeight;

            Assert.Equal(drawerSetting.DrawerFooterHeight, drawerFooterHeight);
        }

        [Theory]
        [InlineData(1.9d)]
        [InlineData(500d)]
        [InlineData(1.0d)]
        public void Duration_SetValue_ReturnsExpectedValue(double duration)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            drawerSetting.Duration = duration;

            Assert.Equal(drawerSetting.Duration, duration);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(-1000d)]
        [InlineData(0d)]
        public void TouchThreshold_SetValue_ReturnsExpectedValue(double touchThreshold)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            drawerSetting.TouchThreshold = touchThreshold;

            Assert.Equal(drawerSetting.TouchThreshold, touchThreshold);
        }

        [Theory]
        [InlineData(Transition.Push)]
        [InlineData(Transition.Reveal)]
        [InlineData(Transition.SlideOnTop)]
        public void Transition_SetValue_ReturnsExpectedValue(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            drawerSetting.Transition = transition;

            Assert.Equal(drawerSetting.Transition, transition);
        }

        [Theory]
        [InlineData(Position.Top)]
        [InlineData(Position.Left)]
        [InlineData(Position.Right)]
        [InlineData(Position.Bottom)]
        public void Position_SetValue_ReturnsExpectedValue(Position position)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            drawerSetting.Position = position;

            Assert.Equal(drawerSetting.Position, position);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void EnableSwipeGetstuse_SetValue_ReturnsExpectedValue(bool enableSwipeGesture)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            drawerSetting.EnableSwipeGesture = enableSwipeGesture;

            Assert.Equal(enableSwipeGesture, drawerSetting.EnableSwipeGesture);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(1000d)]
        [InlineData(0d)]
        public void DrawerWidth_SetValue_ReturnsExpectedValue(double drawerWidth)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            drawerSetting.DrawerWidth = drawerWidth;

            Assert.Equal(drawerSetting.DrawerWidth, drawerWidth);
        }

        [Fact]
        public void DrawerContentView_SetValue_ReturnsExpectedValue()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            var drawerContentView = new Label { Text = " DrawerContentView" };
            drawerSetting.DrawerContentView = drawerContentView;

            Assert.Same(drawerContentView, drawerSetting.DrawerContentView);
        }

        [Fact]
        public void DrawerHeaderView_SetValue_ReturnsExpectedValue()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            var drawerHeaderView = new Label
            { 
                Text = " DrawerHeaderView" 
            };
            drawerSetting.DrawerHeaderView = drawerHeaderView;

            Assert.Same(drawerHeaderView, drawerSetting.DrawerHeaderView);
        }

        [Fact]
        public void DrawerFooterView_SetValue_ReturnsExpectedValue()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var drawerSetting = new DrawerSettings();
            navigationDrawer.DrawerSettings = drawerSetting;
            var drawerFooterView = new Label { Text = "DrawerFooterView" };
            drawerSetting.DrawerFooterView = drawerFooterView;

            Assert.Same(drawerFooterView, drawerSetting.DrawerFooterView);
        }

        [Fact]
        public void TestHeaderViewTransfer()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings() { DrawerHeaderView = new Grid() } };

            Assert.Equal(new Grid(), navigationDrawer.DrawerSettings.DrawerHeaderView);
            Assert.Null(navigationDrawer.DrawerSettings.DrawerFooterView);

            navigationDrawer.DrawerSettings.DrawerFooterView = navigationDrawer.DrawerSettings.DrawerHeaderView;

            Assert.NotNull(navigationDrawer.DrawerSettings.DrawerFooterView);
            Assert.Equal(new Grid(), navigationDrawer.DrawerSettings.DrawerFooterView);
        }

        [Fact]
        public void TestContentViewWithPositionAndIsOpen()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Left;
            navigationDrawer.DrawerSettings.Transition = Transition.Push;
            navigationDrawer.IsOpen = true;
            navigationDrawer.DrawerSettings.Position += 1;

            Assert.Equal(Position.Right, navigationDrawer.DrawerSettings.Position);
            Assert.True(navigationDrawer.IsOpen);
            Assert.Equal(new Grid(), navigationDrawer.ContentView);
        }

        [Fact]
        public void TestDrawerHeightWhenContentLoaded()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings() };
            var drawerHeaderView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                Children =
                {
                    new Label { Text = "Welcome", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerContentView = new StackLayout
            {
                Children =
                {
                    new Label { Text = "Item 1", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 2", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 3", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 4", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 5", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 6", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 7", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 8", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 9", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 10", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 11", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 12", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerFooterView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                Children =
                {
                    new Button { Text = "Submit" },
                    new Label { Text = "Footer", HorizontalOptions = LayoutOptions.Center }
                }
            };
            Assert.Equal(500d, navigationDrawer.DrawerSettings.DrawerHeight);

            navigationDrawer.DrawerSettings.DrawerContentView = drawerContentView;
            navigationDrawer.DrawerSettings.DrawerHeaderView = drawerHeaderView;
            navigationDrawer.DrawerSettings.DrawerFooterView = drawerFooterView;

            Assert.Equal(500d, navigationDrawer.DrawerSettings.DrawerHeight);
        }

        [Fact]
        public void TestDrawerWidthWhenContentLoaded()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings() };
            var drawerHeaderView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                Children =
                {
                    new Label { Text = "Welcome", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerContentView = new StackLayout
            {
                Children =
                {
                    new Label { Text = "Item 1", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 2", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 3", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 4", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 5", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 6", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 7", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 8", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 9", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 10", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 11", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 12", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerFooterView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                Children =
                {
                    new Button { Text = "Submit" },
                    new Label { Text = "Footer", HorizontalOptions = LayoutOptions.Center }
                }
            };
            Assert.Equal(200d, navigationDrawer.DrawerSettings.DrawerWidth);

            navigationDrawer.DrawerSettings.DrawerContentView = drawerContentView;
            navigationDrawer.DrawerSettings.DrawerHeaderView = drawerHeaderView;
            navigationDrawer.DrawerSettings.DrawerFooterView = drawerFooterView;

            Assert.Equal(200d, navigationDrawer.DrawerSettings.DrawerWidth);
        }

        [Theory]
        [InlineData(-5)]
        [InlineData(-1522)]
        [InlineData(-200)]
        [InlineData(-245)]
        public void TestDrawerWidthWhenNegative(double value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings() };
            navigationDrawer.DrawerSettings.DrawerWidth = value;

            Assert.Equal(200d, navigationDrawer.DrawerSettings.DrawerWidth);
        }

        [Theory]
        [InlineData(-57)]
        [InlineData(-450)]
        [InlineData(-225)]
        [InlineData(-300)]
        public void TestDrawerHeightWhenNegative(double value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings() };
            navigationDrawer.DrawerSettings.DrawerHeight = value;

            Assert.Equal(500d, navigationDrawer.DrawerSettings.DrawerHeight);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(235)]
        [InlineData(-3534)]
        [InlineData(4567)]
        [InlineData(-3250)]
        public void TestDrawerHeaderHeight(double value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings() };
            navigationDrawer.DrawerSettings.DrawerHeaderHeight = value;
            var drawerHeaderView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                Children =
                {
                    new Label { Text = "Welcome", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerContentView = new StackLayout
            {
                Children =
                {
                    new Label { Text = "Item 1", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 2", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 3", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 4", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 5", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 6", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 7", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 8", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 9", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 10", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 11", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 12", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerFooterView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                Children =
                {
                    new Button { Text = "Submit" },
                    new Label { Text = "Footer", HorizontalOptions = LayoutOptions.Center }
                }
            };
            navigationDrawer.DrawerSettings.DrawerContentView = drawerContentView;
            navigationDrawer.DrawerSettings.DrawerHeaderView = drawerHeaderView;

            Assert.Equal(value, navigationDrawer.DrawerSettings.DrawerHeaderHeight);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(235)]
        [InlineData(-3534)]
        [InlineData(4567)]
        [InlineData(-3250)]
        public void TestDrawerFooterHeight(double value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings() };
            navigationDrawer.DrawerSettings.DrawerFooterHeight = value;
            var drawerHeaderView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                Children =
                {
                    new Label { Text = "Welcome", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerContentView = new StackLayout
            {
                Children =
                {
                    new Label { Text = "Item 1", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 2", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 3", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 4", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 5", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 6", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 7", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 8", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 9", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 10", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 11", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 12", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerFooterView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                Children =
                {
                    new Button { Text = "Submit" },
                    new Label { Text = "Footer", HorizontalOptions = LayoutOptions.Center }
                }
            };
            navigationDrawer.DrawerSettings.DrawerContentView = drawerContentView;
            navigationDrawer.DrawerSettings.DrawerHeaderView = drawerHeaderView;
            navigationDrawer.DrawerSettings.DrawerFooterView = drawerFooterView;

            Assert.Equal(value, navigationDrawer.DrawerSettings.DrawerFooterHeight);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(25)]
        [InlineData(353)]
        [InlineData(2379)]
        [InlineData(98762)]
        public void TestDrawerHeightWithDifferentValues(double value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings() };
            navigationDrawer.DrawerSettings.DrawerHeight = value;
            var drawerHeaderView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                HeightRequest = value,
                Children =
                {
                    new Label { Text = "Welcome", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerContentView = new StackLayout
            {
                HeightRequest = value,
                Children =
                {
                    new Label { Text = "Item 1", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 2", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 3", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 4", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 5", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 6", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 7", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 8", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 9", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 10", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 11", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 12", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerFooterView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                HeightRequest = value,
                Children =
                {
                    new Button { Text = "Submit" },
                    new Label { Text = "Footer", HorizontalOptions = LayoutOptions.Center }
                }
            };
            navigationDrawer.DrawerSettings.DrawerContentView = drawerContentView;
            navigationDrawer.DrawerSettings.DrawerHeaderView = drawerHeaderView;
            navigationDrawer.DrawerSettings.DrawerFooterView = drawerFooterView;

            Assert.Equal(value, navigationDrawer.DrawerSettings.DrawerHeight);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(25)]
        [InlineData(353)]
        [InlineData(2379)]
        [InlineData(98762)]
        public void TestDrawerWidthWithDifferentValues(double value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings() };
            navigationDrawer.DrawerSettings.DrawerWidth = value;
            var drawerHeaderView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                WidthRequest = value,
                Children =
                {
                    new Label { Text = "Welcome", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerContentView = new StackLayout
            {
                WidthRequest = value,
                Children =
                {
                    new Label { Text = "Item 1", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 2", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 3", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 4", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 5", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 6", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 7", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 8", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 9", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 10", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 11", FontSize = 24, HorizontalOptions = LayoutOptions.Center },
                    new Label { Text = "Item 12", FontSize = 24, HorizontalOptions = LayoutOptions.Center }
                }
            };
            var drawerFooterView = new StackLayout
            {
                BackgroundColor = Color.FromArgb("F7F7F7"),
                Padding = 10,
                WidthRequest = value,
                Children =
                {
                    new Button { Text = "Submit" },
                    new Label { Text = "Footer", HorizontalOptions = LayoutOptions.Center }
                }
            };
            navigationDrawer.DrawerSettings.DrawerContentView = drawerContentView;
            navigationDrawer.DrawerSettings.DrawerHeaderView = drawerHeaderView;
            navigationDrawer.DrawerSettings.DrawerFooterView = drawerFooterView;

            Assert.Equal(value, navigationDrawer.DrawerSettings.DrawerWidth);
        }

        [Theory]
        [InlineData(Position.Left)]
        [InlineData(Position.Right)]
        [InlineData(Position.Top)]
        [InlineData(Position.Bottom)]
        public void TestDrawerContentViewAndHeaderFooterViewIsType(Position position)
        {
            var navigationDrawer = new SfNavigationDrawer
            {
                ContentView = new StackLayout(),
                DrawerSettings = new DrawerSettings
                {
                    DrawerHeaderView = new Label { Text = "Header" },
                    DrawerFooterView = new Label { Text = "Footer" },
                    Position = position
                }
            };
            Assert.IsType<StackLayout>(navigationDrawer.ContentView);
            Assert.IsType<Label>(navigationDrawer.DrawerSettings.DrawerHeaderView);
            Assert.IsType<Label>(navigationDrawer.DrawerSettings.DrawerFooterView);
            navigationDrawer.DrawerSettings.DrawerFooterView = new Button();
            navigationDrawer.DrawerSettings.DrawerHeaderView = new Button();
            Assert.IsType<Button>(navigationDrawer.DrawerSettings.DrawerFooterView);
            Assert.IsType<Button>(navigationDrawer.DrawerSettings.DrawerHeaderView);
        }

        [Theory]
        [InlineData(Position.Left)]
        [InlineData(Position.Right)]
        [InlineData(Position.Top)]
        [InlineData(Position.Bottom)]
        public void TestDrawerContentViewAndHeaderFooterViewTexts(Position position)
        {
            var navigationDrawer = new SfNavigationDrawer
            {
                ContentView = new StackLayout(),
                DrawerSettings = new DrawerSettings
                {
                    DrawerHeaderView = new Label { Text = "Header" },
                    DrawerFooterView = new Label { Text = "Footer" },
                    Position = position
                }
            };
            Assert.Equal("Header", ((Label)navigationDrawer.DrawerSettings.DrawerHeaderView).Text);
            Assert.Equal("Footer", ((Label)navigationDrawer.DrawerSettings.DrawerFooterView).Text);
            navigationDrawer.DrawerSettings.DrawerFooterView = new Button() { Text = "New Footer" };
            navigationDrawer.DrawerSettings.DrawerHeaderView = new Button() { Text = "New Header" };
            Assert.Equal("New Footer", ((Button)navigationDrawer.DrawerSettings.DrawerFooterView).Text);
            Assert.Equal("New Footer", ((Button)navigationDrawer.DrawerSettings.DrawerFooterView).Text);
        }

        #endregion

        #region internal properties

        [Fact]
        public void ContentBackgroundColor_SetValue_ReturnsExpectedValue()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.ContentBackgroundColor = Colors.Indigo;
            Color expectedValue = navigationDrawer.ContentBackgroundColor;

            Assert.Equal(Colors.Indigo, expectedValue);
        }

        [Fact]
        public void GreyOverLayColor_SetValue_ReturnsExpectedValue()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.GreyOverlayColor = Colors.DarkRed;
            Color expectedValue = navigationDrawer.GreyOverlayColor;

            Assert.Equal(Colors.DarkRed, expectedValue);
        }

        [Theory]
        [InlineData(-9.0d)]
        [InlineData(0d)]
        [InlineData(500)]
        public void ScreenWidth_SetValue_ReturnsExpectedValue(double screenWidth)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.ScreenWidth = screenWidth;

            Assert.Equal(screenWidth, navigationDrawer.ScreenWidth);
        }

        [Theory]
        [InlineData(-9.0d)]
        [InlineData(0d)]
        [InlineData(500)]
        public void ScreenHeight_SetValue_ReturnsExpectedValue(double screenHeight)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.ScreenHeight = screenHeight;

            Assert.Equal(screenHeight, navigationDrawer.ScreenHeight);
        }

        #endregion

        #region private methods

        [Fact]
        public void TestFirstMoveActionStarted()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            InvokePrivateMethod(navigationDrawer, "FirstMoveActionStarted");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isPressed"));
            Assert.True(expectedValue);
            expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_actionFirstMoveOpen"));
            Assert.True(expectedValue);
            expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_actionFirstMoveClose"));
            Assert.True(expectedValue);
        }

        [Fact]
        public void TestFirstMoveActionCompleted()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            InvokePrivateMethod(navigationDrawer, "FirstMoveActionCompleted");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_actionFirstMoveOpen"));
            Assert.False(expectedValue);
            expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_actionFirstMoveClose"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestValidateRemainDrawerWidth()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            SetPrivateField(navigationDrawer, "_remainDrawerWidth", 30);
            InvokePrivateMethod(navigationDrawer, "ValidateRemainDrawerWidth");
            double expectedValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_remainDrawerWidth"));
            Assert.Equal(0, expectedValue);
        }

        [Fact]
        public void TestValidateRemainDrawerHeight()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            SetPrivateField(navigationDrawer, "_remainDrawerHeight", 30);
            InvokePrivateMethod(navigationDrawer, "ValidateRemainDrawerHeight");
            double expectedValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_remainDrawerHeight"));
            Assert.Equal(0, expectedValue);
        }

        [Fact]
        public void TestValidateRemainDrawerWidthNegative()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            SetPrivateField(navigationDrawer, "_remainDrawerWidth", -30);
            navigationDrawer.DrawerSettings.DrawerWidth = 20;
            InvokePrivateMethod(navigationDrawer, "ValidateRemainDrawerWidth");
            double expectedValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_remainDrawerWidth"));
            Assert.Equal(-20, expectedValue);
        }

        [Fact]
        public void TestValidateRemainDrawerHeightNegative()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            SetPrivateField(navigationDrawer, "_remainDrawerHeight", -30);
            navigationDrawer.DrawerSettings.DrawerHeight = 20;
            InvokePrivateMethod(navigationDrawer, "ValidateRemainDrawerHeight");
            double expectedValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_remainDrawerHeight"));
            Assert.Equal(-20, expectedValue);
        }

        [Fact]
        public void TestValidateCurrentDuration()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            double expectedValue = 20;
            double actualValue = Convert.ToDouble(InvokePrivateStaticMethod(navigationDrawer, "ValidateCurrentDuration", expectedValue));
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void TestOnBindingContextChanged()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { BindingContext = 250 };
            var fired = false;
            navigationDrawer.BindingContextChanged += (sender, e) => fired = true;
            navigationDrawer.BindingContext = 100;
            Assert.True(fired);
        }

        [Fact]
        public void TestUpdateDrawerWidthMethod()
        {
            DrawerSettings drawerSettings = new DrawerSettings();
            drawerSettings.DrawerWidth = -200;
            InvokePrivateMethod(drawerSettings, "UpdateDrawerWidth");
            Assert.Equal(200, drawerSettings.DrawerWidth);
        }

        [Fact]
        public void TestUpdateDrawerHeightMethod()
        {
            DrawerSettings drawerSettings = new DrawerSettings();
            drawerSettings.DrawerHeight = -200;
            InvokePrivateMethod(drawerSettings, "UpdateDrawerHeight");
            Assert.Equal(500, drawerSettings.DrawerHeight);
        }

        [Fact]
        public void TestToggleEventArgs()
        {
            Syncfusion.Maui.Toolkit.NavigationDrawer.ToggledEventArgs eventArgs = new Syncfusion.Maui.Toolkit.NavigationDrawer.ToggledEventArgs();
            bool expectedValue = eventArgs.IsOpen;
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestOnSizeAllocated()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            InvokePrivateMethod(navigationDrawer, "OnSizeAllocated", 1200, 2042);
            double expectedValue = navigationDrawer.ScreenWidth;
            Assert.Equal(1200, expectedValue);
            expectedValue = navigationDrawer.ScreenHeight;
            Assert.Equal(2042, expectedValue);
        }

        [Fact]
        public void TestFlowDirectionMatchParentLTR()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.FlowDirection = FlowDirection.MatchParent;
            navigationDrawer.Parent = new Grid() { FlowDirection = FlowDirection.LeftToRight };
            InvokePrivateMethod(navigationDrawer, "OnPropertyChanged", "FlowDirection");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isRTL"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestFlowDirectionMatchParentRTL()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.FlowDirection = FlowDirection.MatchParent;
            navigationDrawer.Parent = new Grid() { FlowDirection = FlowDirection.RightToLeft };
            InvokePrivateMethod(navigationDrawer, "OnPropertyChanged", "FlowDirection");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isRTL"));
            Assert.True(expectedValue);
        }

        [Fact]
        public void TestFlowDirectionLTR()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.FlowDirection = FlowDirection.LeftToRight;
            InvokePrivateMethod(navigationDrawer, "OnPropertyChanged", "FlowDirection");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isRTL"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnRightvelocitiyPositive()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Right;
            SetPrivateField(navigationDrawer, "_velocityX", 550);
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnBottomvelocitiyPositive()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Bottom;
            SetPrivateField(navigationDrawer, "_velocityY", 550);
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnLeftvelocitiyNegative()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Left;
            SetPrivateField(navigationDrawer, "_velocityX", -550);
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnRightvelocitiyNegative()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Right;
            navigationDrawer.DrawerSettings.Transition = Transition.Push;
            SetPrivateField(navigationDrawer, "_velocityX", -550);
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnTopvelocitiyNegative()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Top;
            SetPrivateField(navigationDrawer, "_velocityY", -550);
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnBottomvelocitiyNegative()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Bottom;
            navigationDrawer.DrawerSettings.Transition = Transition.Push;
            SetPrivateField(navigationDrawer, "_velocityY", -550);
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnLeft()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Left;
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnRight()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Right;
            navigationDrawer.DrawerSettings.Transition = Transition.Reveal;
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnTop()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Top;
            navigationDrawer.DrawerSettings.Transition = Transition.Reveal;
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestCompleteSwipeOnBottom()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            navigationDrawer.DrawerSettings.Position = Position.Bottom;
            navigationDrawer.DrawerSettings.Transition = Transition.Reveal;
            InvokePrivateMethod(navigationDrawer, "CompletedDrawerSwipe");
            bool expectedValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(expectedValue);
        }

        [Fact]
        public void TestUpdateToggleOutEvent()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { IsOpen = true };
            InvokePrivateMethod(navigationDrawer, "UpdateToggleOutEvent");
            Assert.False(navigationDrawer.IsOpen);
        }

        [Theory]
        [InlineData(Transition.SlideOnTop)]
        [InlineData(Transition.Reveal)]
        [InlineData(Transition.Push)]
        public void TestLeftDrawerSwipe(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ContentView = new Grid() };
            navigationDrawer.DrawerSettings = new DrawerSettings();
            navigationDrawer.DrawerSettings.Transition = transition;
            SetPrivateField(navigationDrawer, "_isTransitionDifference", false);
            InvokePrivateMethod(navigationDrawer, "LeftDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(Transition.SlideOnTop)]
        [InlineData(Transition.Push)]
        public void TestRightDrawerSwipe(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ContentView = new Grid() };
            navigationDrawer.DrawerSettings = new DrawerSettings();
            navigationDrawer.DrawerSettings.Transition = transition;
            SetPrivateField(navigationDrawer, "_isTransitionDifference", false);
            InvokePrivateMethod(navigationDrawer, "RightDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Fact]
        public void TestRightDrawerSwipeReveal()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ContentView = new Grid() };
            navigationDrawer.DrawerSettings = new DrawerSettings();
            navigationDrawer.DrawerSettings.Transition = Transition.Reveal;
            SetPrivateField(navigationDrawer, "_isTransitionDifference", false);
            InvokePrivateMethod(navigationDrawer, "RightDrawerSwipe", 525);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(actualValue);
        }

        [Theory]
        [InlineData(Transition.SlideOnTop)]
        [InlineData(Transition.Push)]
        [InlineData(Transition.Reveal)]
        public void TestTopDrawerSwipe(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ContentView = new Grid() };
            navigationDrawer.DrawerSettings = new DrawerSettings();
            navigationDrawer.DrawerSettings.Transition = transition;
            SetPrivateField(navigationDrawer, "_isTransitionDifference", false);
            InvokePrivateMethod(navigationDrawer, "TopDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(Transition.SlideOnTop)]
        [InlineData(Transition.Push)]
        public void TestBottomDrawerSwipe(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ContentView = new Grid() };
            navigationDrawer.DrawerSettings = new DrawerSettings();
            navigationDrawer.DrawerSettings.Transition = transition;
            SetPrivateField(navigationDrawer, "_isTransitionDifference", false);
            InvokePrivateMethod(navigationDrawer, "BottomDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Fact]
        public void TestBottomDrawerSwipeReveal()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ContentView = new Grid() };
            navigationDrawer.DrawerSettings = new DrawerSettings();
            navigationDrawer.DrawerSettings.Transition = Transition.Reveal;
            SetPrivateField(navigationDrawer, "_isTransitionDifference", false);
            InvokePrivateMethod(navigationDrawer, "BottomDrawerSwipe", 525);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(actualValue);
        }

        [Fact]
        public void TestDrawerUnloaded()
        {
            StackLayout mainLayout = new StackLayout();
            SfNavigationDrawer navigationDrawerLTR = new SfNavigationDrawer() { FlowDirection = FlowDirection.LeftToRight };
            SfNavigationDrawer navigationDrawerRTL = new SfNavigationDrawer() { FlowDirection = FlowDirection.RightToLeft };
            mainLayout.Children.Add(navigationDrawerLTR);
            Assert.Same(navigationDrawerLTR, mainLayout.Children[0]);
            mainLayout.Children.Remove(navigationDrawerLTR);
            mainLayout.Children.Add(navigationDrawerRTL);
            Assert.Same(navigationDrawerRTL, mainLayout.Children[0]);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestHandleTopDrawerWhenClose(bool value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_isDrawerOpen", false);
            SetPrivateField(navigationDrawer, "_actionFirstMoveOpen", value);
            SetPrivateField(navigationDrawer, "_actionFirstMoveClose", value);
            InvokePrivateMethod(navigationDrawer, "HandleTopDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
            InvokePrivateMethod(navigationDrawer, "HandleTopDrawerSwipe", -25);
            actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestHandleTopDrawerWhenOpen(bool value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            SetPrivateField(navigationDrawer, "_actionFirstMoveOpen", value);
            SetPrivateField(navigationDrawer, "_actionFirstMoveClose", value);
            InvokePrivateMethod(navigationDrawer, "HandleTopDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
            InvokePrivateMethod(navigationDrawer, "HandleTopDrawerSwipe", -25);
            actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestHandleLeftDrawerWhenOpen(bool value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            SetPrivateField(navigationDrawer, "_actionFirstMoveOpen", value);
            SetPrivateField(navigationDrawer, "_actionFirstMoveClose", value);
            InvokePrivateMethod(navigationDrawer, "HandleLeftDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
            InvokePrivateMethod(navigationDrawer, "HandleLeftDrawerSwipe", -25);
            actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestHandleLeftDrawerWhenClose(bool value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_isDrawerOpen", false);
            SetPrivateField(navigationDrawer, "_actionFirstMoveOpen", value);
            SetPrivateField(navigationDrawer, "_actionFirstMoveClose", value);
            InvokePrivateMethod(navigationDrawer, "HandleLeftDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestHandleRightDrawerWhenOpen(bool value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            SetPrivateField(navigationDrawer, "_actionFirstMoveOpen", value);
            SetPrivateField(navigationDrawer, "_actionFirstMoveClose", value);
            InvokePrivateMethod(navigationDrawer, "HandleRightDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestHandleRightDrawerWhenClose(bool value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_isDrawerOpen", false);
            SetPrivateField(navigationDrawer, "_actionFirstMoveOpen", value);
            SetPrivateField(navigationDrawer, "_actionFirstMoveClose", value);
            InvokePrivateMethod(navigationDrawer, "HandleRightDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
            InvokePrivateMethod(navigationDrawer, "HandleRightDrawerSwipe", -25);
            actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestHandleBottomDrawerWhenOpen(bool value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            SetPrivateField(navigationDrawer, "_actionFirstMoveOpen", value);
            SetPrivateField(navigationDrawer, "_actionFirstMoveClose", value);
            InvokePrivateMethod(navigationDrawer, "HandleBottomDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
            InvokePrivateMethod(navigationDrawer, "HandleBottomDrawerSwipe", -25);
            actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestHandleBottomDrawerWhenClose(bool value)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_isDrawerOpen", false);
            SetPrivateField(navigationDrawer, "_actionFirstMoveOpen", value);
            SetPrivateField(navigationDrawer, "_actionFirstMoveClose", value);
            InvokePrivateMethod(navigationDrawer, "HandleBottomDrawerSwipe", 25);
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            InvokePrivateMethod(navigationDrawer, "HandleBottomDrawerSwipe", -25);
            actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Fact]
        public void TestFindVelocity()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_startPoint", new Point(20, 20));
            SetPrivateField(navigationDrawer, "_startTime", DateTime.Now.AddSeconds(-10));
            InvokePrivateMethod(navigationDrawer, "FindVelocity", new Point(120, 120));
            double actualValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_velocityX"));
            Assert.Equal(10d, Math.Round(actualValue));
            actualValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_velocityY"));
            Assert.Equal(10d, Math.Round(actualValue));

        }

        [Fact]
        public void TestToggleDrawerXPositionLeft()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Left;
            SetPrivateField(navigationDrawer, "_oldPoint", new Point(20, 20));
            SetPrivateField(navigationDrawer, "_newPoint", new Point(50, 20));
            InvokePrivateMethod(navigationDrawer, "TranslateDrawerXPosition");
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Fact]
        public void TestToggleDrawerYPositionTop()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Top;
            SetPrivateField(navigationDrawer, "_oldPoint", new Point(20, 20));
            SetPrivateField(navigationDrawer, "_newPoint", new Point(50, 50));
            InvokePrivateMethod(navigationDrawer, "TranslateDrawerYPosition");
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.True(actualValue);
        }

        [Fact]
        public void TestToggleDrawerXPositionRight()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Right;
            SetPrivateField(navigationDrawer, "_oldPoint", new Point(20, 20));
            SetPrivateField(navigationDrawer, "_newPoint", new Point(50, 20));
            InvokePrivateMethod(navigationDrawer, "TranslateDrawerXPosition");
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(actualValue);
        }

        [Fact]
        public void TestToggleDrawerYPositionBottom()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Bottom;
            SetPrivateField(navigationDrawer, "_oldPoint", new Point(20, 20));
            SetPrivateField(navigationDrawer, "_newPoint", new Point(50, 50));
            InvokePrivateMethod(navigationDrawer, "TranslateDrawerYPosition");
            bool actualValue = Convert.ToBoolean(GetPrivateField(navigationDrawer, "_isTransitionDifference"));
            Assert.False(actualValue);
        }

        [Fact]
        public void TestDurationPropertyUpdate()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            InvokePrivateMethod(navigationDrawer, "DurationPropertyUpdate", -35);
            Assert.Equal(1, navigationDrawer.DrawerSettings.Duration);
        }

        [Fact]
        public void TestUpdateTouchThresholdRight()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ScreenWidth = 150, DrawerSettings = new DrawerSettings() { TouchThreshold = 50, Position = Position.Right }, ContentView = new Grid() };
            InvokePrivateMethod(navigationDrawer, "UpdateTouchThreshold");
            double actualValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_touchRightThreshold"));
            Assert.Equal(100, actualValue);
        }

        [Fact]
        public void TestUpdateTouchThresholdBottom()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ScreenHeight = 150, DrawerSettings = new DrawerSettings() { TouchThreshold = 50, Position = Position.Bottom }, ContentView = new Grid() };
            InvokePrivateMethod(navigationDrawer, "UpdateTouchThreshold");
            double actualValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_touchBottomThreshold"));
            Assert.Equal(100, actualValue);
        }

        [Fact]
        public void TestUpdateDrawerFlowDirection()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            SetPrivateField(navigationDrawer, "_isRTL", true);
            InvokePrivateMethod(navigationDrawer, "UpdateDrawerFlowDirection");
            Assert.Equal(FlowDirection.RightToLeft, navigationDrawer.ContentView.FlowDirection);
        }

        [Fact]
        public void TestPositionUpdateOnDrawerCloseRight()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Right;
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            double actualValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_touchRightThreshold"));
            Assert.Equal(-120, actualValue);

        }

        [Theory]
        [InlineData(Transition.Reveal)]
        [InlineData(Transition.Push)]
        public void TestPositionUpdateOnDrawerCloseRightTransition(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Right;
            navigationDrawer.DrawerSettings.Transition = transition;
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(0, navigationDrawer.ContentView.TranslationX);

        }

        [Fact]
        public void TestPositionUpdateOnDrawerOpenRight()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Right;
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            double actualValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_touchRightThreshold"));
            Assert.Equal(-120, actualValue);
        }

        [Fact]
        public void TestPositionUpdateOnDrawerOpenRightWithPush()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Right;
            navigationDrawer.DrawerSettings.Transition = Transition.Push;
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            double actualValue = Convert.ToDouble(GetPrivateField(navigationDrawer, "_touchRightThreshold"));
            Assert.Equal(-120, actualValue);
        }

        [Fact]
        public void TestPositionUpdateOnDrawerOpenLeft()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Left;
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(0, navigationDrawer.ContentView.TranslationX);
        }

        [Theory]
        [InlineData(Transition.Push)]
        [InlineData(Transition.Reveal)]
        public void TestPositionUpdateOnDrawerOpenLeftWithTransition(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Left;
            navigationDrawer.DrawerSettings.Transition = transition;
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(navigationDrawer.DrawerSettings.DrawerWidth, navigationDrawer.ContentView.TranslationX);
        }

        [Fact]
        public void TestPositionUpdateOnDrawerCloseTopReveal()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Top;
            navigationDrawer.DrawerSettings.Transition = Transition.Reveal;
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(0, navigationDrawer.ContentView.TranslationY);
        }

        [Fact]
        public void TestPositionUpdateOnDrawerCloseTopPush()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Top;
            navigationDrawer.DrawerSettings.Transition = Transition.Push;
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(0, navigationDrawer.ContentView.TranslationY);
        }

        [Fact]
        public void TestPositionUpdateOnDrawerOpenTop()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Top;
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(0, navigationDrawer.ContentView.TranslationY);
        }

        [Theory]
        [InlineData(Transition.Reveal)]
        [InlineData(Transition.Push)]
        public void TestPositionUpdateOnDrawerOpenTopWithTransition(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Top;
            navigationDrawer.DrawerSettings.Transition = transition;
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(navigationDrawer.DrawerSettings.DrawerHeight, navigationDrawer.ContentView.TranslationY);
        }

        [Fact]
        public void TestPositionUpdateOnDrawerOpenBottom()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Bottom;
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(0, navigationDrawer.ContentView.TranslationY);
        }

        [Theory]
        [InlineData(Transition.Reveal)]
        [InlineData(Transition.Push)]
        public void TestPositionUpdateOnDrawerOpenBottomWithTransition(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Bottom;
            navigationDrawer.DrawerSettings.Transition = transition;
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(-navigationDrawer.DrawerSettings.DrawerHeight, navigationDrawer.ContentView.TranslationY);
        }

        [Theory]
        [InlineData(Transition.Push)]
        [InlineData(Transition.Reveal)]
        public void TestPositionUpdateOnDrawerCloseBottom(Transition transition)
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Bottom;
            navigationDrawer.DrawerSettings.Transition = transition;
            InvokePrivateMethod(navigationDrawer, "PositionUpdate");
            Assert.Equal(0, navigationDrawer.ContentView.TranslationY);
        }

        [Fact]
        public void TestIsTouchOutsideDrawerBoundsOnLeft()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Left;
            navigationDrawer.DrawerSettings.DrawerWidth = 50;
            SetPrivateField(navigationDrawer, "_initialTouchPoint", new Point(100, 0));
            bool actualValue = Convert.ToBoolean(InvokePrivateMethod(navigationDrawer, "IsTouchOutsideDrawerBounds"));
            Assert.True(actualValue);
        }

        [Fact]
        public void TestIsTouchOutsideDrawerBoundsOnRight()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Right;
            navigationDrawer.DrawerSettings.DrawerWidth = 50;
            navigationDrawer.ScreenWidth = 100;
            SetPrivateField(navigationDrawer, "_initialTouchPoint", new Point(20, 0));
            bool actualValue = Convert.ToBoolean(InvokePrivateMethod(navigationDrawer, "IsTouchOutsideDrawerBounds"));
            Assert.True(actualValue);
        }

        [Fact]
        public void TestIsTouchOutsideDrawerBoundsOnTop()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Top;
            navigationDrawer.DrawerSettings.DrawerHeight = 50;
            SetPrivateField(navigationDrawer, "_initialTouchPoint", new Point(0, 100));
            bool actualValue = Convert.ToBoolean(InvokePrivateMethod(navigationDrawer, "IsTouchOutsideDrawerBounds"));
            Assert.True(actualValue);
        }

        [Fact]
        public void TestIsTouchOutsideDrawerBoundsOnBottom()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { DrawerSettings = new DrawerSettings(), ContentView = new Grid() };
            navigationDrawer.DrawerSettings.Position = Position.Bottom;
            navigationDrawer.DrawerSettings.DrawerHeight = 50;
            navigationDrawer.ScreenHeight = 100;
            SetPrivateField(navigationDrawer, "_initialTouchPoint", new Point(0, 20));
            bool actualValue = Convert.ToBoolean(InvokePrivateMethod(navigationDrawer, "IsTouchOutsideDrawerBounds"));
            Assert.True(actualValue);
        }

        protected object? InvokePrivateStaticMethod<T>(T obj, string methodName, params object[] parameters)
        {
            var method = typeof(T).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            if (method == null)
                throw new InvalidOperationException($"Method '{methodName}' not found.");
            return method.Invoke(obj, parameters);
        }

        #endregion

        #region events

        [Fact]
        public void TestDrawerClosedInvoked()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var fired = false;
            Grid content = new Grid();
            content.Add(new Label());
            navigationDrawer.ContentView = content;
            navigationDrawer.DrawerClosed += (sender, e) => fired = true;
            SetPrivateField(navigationDrawer, "_isDrawerOpen", true);
            navigationDrawer.DrawerSettings.Transition = Transition.Reveal;
            InvokePrivateMethod(navigationDrawer, "OnDrawerClosedToggledEvent");
            Assert.True(fired);
        }

        [Fact]
        public void TestDrawerOpenedInvoked()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer() { ContentView = new Grid() };
            navigationDrawer.DrawerSettings = new DrawerSettings { Transition = Transition.Reveal };
            var fired = false;
            navigationDrawer.DrawerOpened += (sender, e) => fired = true;
            InvokePrivateMethod(navigationDrawer, "OnDrawerOpenedToggledEvent");
            Assert.True(fired);
        }

        [Fact]
        public void TestDrawerToggledInvoked()
        {
            SfNavigationDrawer navigationDrawer = new SfNavigationDrawer();
            var fired = false;
            navigationDrawer.DrawerToggled += (sender, e) => fired = true;
            InvokePrivateMethod(navigationDrawer, "OnDrawerOpenedToggledEvent");
            Assert.True(fired);
        }
        #endregion
    }
}