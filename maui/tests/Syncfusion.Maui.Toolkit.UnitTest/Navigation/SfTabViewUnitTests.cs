using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Helper;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.TabView;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;


namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class SfTabViewUnitTests : BaseUnitTest
    {
        SfTabBar tabBar= new SfTabBar();
        private void PopulateTabBarItems()
        {
            tabBar = new SfTabBar();

            var tabItems = new TabItemCollection
            {
                new SfTabItem()
                {
                    Header = "Call",
                    Content = new Button()
                    {
                        Text = "Tab Item1",
                    }
                },
                new SfTabItem()
                {
                    Header = "Favorites",
                    Content = new Button()
                    {
                        Text = "Tab Item2",
                    }
                },
                new SfTabItem()
                {
                    Header = "Contacts",
                    Content = new Button()
                    {
                        Text = "Tab Item3",
                    }
                }
            };

            tabBar.Items = tabItems;
        }

        private List<Button> PopulateButtonsListItemsSource()
        {
            List<Button> tabItems = new List<Button>();
            tabItems.Add(new Button() { Text = "button1" });
            tabItems.Add(new Button() { Text = "button2" });
            tabItems.Add(new Button() { Text = "button3" });

            return tabItems;
        }

        private List<object> PopulateMixedObjectItemsSource()
        {
            List<object> tabItems = new List<object>();
            tabItems.Add(new Button() { Text = "button" });
            tabItems.Add(new Label() { Text = "label" });
            tabItems.Add(new Picker() { });

            return tabItems;
        }

        private TabItemCollection PopulateMixedItemsCollection()
        {
            var tabItems = new TabItemCollection()
                {
                    new SfTabItem { Header = "TAB 1", Content = new Label { Text = "Content 1" } },
                    new SfTabItem { Header = "TAB 2", Content = new Button { Text = "Content 2" } },
                    new SfTabItem { Header = "TAB 3", Content = new Picker { } }
                };

            return tabItems;
        }

        private TabItemCollection PopulateLabelItemsCollection()
        {
            var tabViewItems = new TabItemCollection
            {
                new SfTabItem { Header = "TAB 1", Content = new Label { Text = "Content 1" } },
                new SfTabItem { Header = "TAB 2", Content = new Label { Text = "Content 2" } },
                new SfTabItem { Header = "TAB 3", Content = new Label { Text = "Content 3" } }
            };
            return tabViewItems;
        }

        private List<string> PopulateLargeData()
        {
            List<string> largeDataSet = Enumerable.Range(1, 1000)
                                      .Select(i => $"Item {i}")
                                      .ToList();
            return largeDataSet;
        }

        private List<object> PopulateMixedDataItemsSource()
        {
            List<object> mixedList = new List<object>()
            {
                "String item",
                42,
                new SfTabItem { Header = "TAB 1", Content = new Label { Text = "Content 1" } },
                true
            };
            return mixedList;
        }

		#region TabView Public properties

		[Fact]
        public void Constructor_InitializesDefaultsCorrectly()
        {
            var tabView = new SfTabView();

            Assert.Null(tabView.TabBarBackground);
            Assert.Equal(TabBarDisplayMode.Default, tabView.HeaderDisplayMode);
            Assert.Equal(TabBarPlacement.Top, tabView.TabBarPlacement);
            Assert.Equal(TabIndicatorPlacement.Bottom, tabView.IndicatorPlacement);
            Assert.Equal(IndicatorWidthMode.Fit, tabView.IndicatorWidthMode);
            Assert.Equal(TabWidthMode.Default, tabView.TabWidthMode);
            Assert.Equal(48d, tabView.TabBarHeight);
            Assert.Equal(Color.FromArgb("#6750A4"), ((SolidColorBrush)tabView.IndicatorBackground).Color);
            Assert.Empty(tabView.Items);
            Assert.Equal(0d, tabView.SelectedIndex);
            Assert.Equal(TextAlignment.Center, tabView.HeaderHorizontalTextAlignment);
            Assert.Null(tabView.ItemsSource);
            Assert.Null(tabView.HeaderItemTemplate);
            Assert.Null(tabView.ContentItemTemplate);
            Assert.Equal(new Thickness(52, 0, 52, 0), tabView.TabHeaderPadding);
            Assert.False(tabView.FontAutoScalingEnabled);
            Assert.Equal(new CornerRadius(-1), tabView.IndicatorCornerRadius);
            Assert.False(tabView.IsScrollButtonEnabled);
            Assert.False(tabView.EnableSwiping);
            Assert.Equal(100d, tabView.ContentTransitionDuration);
        }

        [Theory]
        [InlineData(IndicatorWidthMode.Fit)]
        [InlineData(IndicatorWidthMode.Stretch)]
        public void IndicatorWidthMode_SetAndGet_ReturnsExpectedValue(IndicatorWidthMode expected)
        {
            var tabView = new SfTabView();
            tabView.IndicatorWidthMode = expected;

            Assert.Equal(expected, tabView.IndicatorWidthMode);
        }

        [Theory]
        [InlineData(TabBarDisplayMode.Default)]
        [InlineData(TabBarDisplayMode.Image)]
        [InlineData(TabBarDisplayMode.Text)]
        public void HeaderDisplayMode_SetAndGet_ReturnsExpectedValue(TabBarDisplayMode expected)
        {
            var tabView = new SfTabView();
            tabView.HeaderDisplayMode = expected;

            Assert.Equal(expected, tabView.HeaderDisplayMode);
        }

        [Fact]
        public void TabBarBackground_SetAndGet_ReturnsExpectedValue()
        {
            var tabView = new SfTabView();
            tabView.TabBarBackground = Colors.Beige;

            Assert.Equal(Colors.Beige, tabView.TabBarBackground);
        }

        [Theory]
        [InlineData(TabBarPlacement.Top)]
        [InlineData(TabBarPlacement.Bottom)]
        public void TabBarPlacement_SetAndGet_ReturnsExpectedValue(TabBarPlacement expected)
        {
            var tabView = new SfTabView();
            tabView.TabBarPlacement = expected;

            Assert.Equal(expected, tabView.TabBarPlacement);
        }

        [Theory]
        [InlineData(TabIndicatorPlacement.Top)]
        [InlineData(TabIndicatorPlacement.Bottom)]
        [InlineData(TabIndicatorPlacement.Fill)]
        public void IndicatorPlacement_SetAndGet_ReturnsExpectedValue(TabIndicatorPlacement expected)
        {
            var tabView = new SfTabView();
            tabView.IndicatorPlacement = expected;

            Assert.Equal(expected, tabView.IndicatorPlacement);
        }

        [Theory]
        [InlineData(TabWidthMode.Default)]
        [InlineData(TabWidthMode.SizeToContent)]
        public void TabWidthMode_SetAndGet_ReturnsExpectedValue(TabWidthMode expected)
        {
            var tabView = new SfTabView();
            tabView.TabWidthMode = expected;

            Assert.Equal(expected, tabView.TabWidthMode);
        }

        [Fact]
        public void IndicatorBackground_SetAndGet_ReturnsExpectedValue()
        {
            var tabView = new SfTabView();
            tabView.IndicatorBackground = Colors.Magenta;

            Assert.Equal(Colors.Magenta, tabView.IndicatorBackground);
        }

        [Fact]
        public void TabBarHeight_SetAndGet_ReturnsExpectedValue()
        {
            var tabView = new SfTabView();
            tabView.TabBarHeight = 40d;

            Assert.Equal(40d, tabView.TabBarHeight);
        }

        [Fact]
        public void Items_SetAndGet_ReturnsExpectedValue()
        {
            var tabView = new SfTabView();
            var tabItems = new TabItemCollection
            {
                new SfTabItem { Header = "TAB 1", Content = new Label { Text = "Content 1" } },
                new SfTabItem { Header = "TAB 2", Content = new Label { Text = "Content 2" } }
            };

            tabView.Items = tabItems;
            var actualItems = tabView.Items;

            Assert.NotNull(actualItems);
            Assert.Equal(tabItems.Count, actualItems.Count);
            for (int i = 0; i < tabItems.Count; i++)
            {
                Assert.Equal(tabItems[i].Header, actualItems[i].Header);
                Assert.Equal(((Label)tabItems[i].Content).Text, ((Label)actualItems[i].Content).Text);
            }
        }

        [Theory]
        [InlineData(TextAlignment.Start)]
        [InlineData(TextAlignment.Center)]
        [InlineData(TextAlignment.End)]
        public void HeaderHorizontalTextAlignment_SetAndGet_ReturnsExpectedValue(TextAlignment expected)
        {
            var tabView = new SfTabView();
            tabView.HeaderHorizontalTextAlignment = expected;

            Assert.Equal(expected, tabView.HeaderHorizontalTextAlignment);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(0)]
        public void SelectedIndex_SetAndGet_ReturnsExpectedValue(int expected)
        {
            var tabView = new SfTabView();
            tabView.SelectedIndex = expected;

            Assert.Equal(expected, tabView.SelectedIndex);
        }

        [Fact]
        public void ItemsSource_SetAndGet_ReturnsExpectedValue()
        {
            var tabView = new SfTabView();
            var expectedItemsSource = new List<string> { "Tab1", "Tab2", "Tab3" };

            tabView.ItemsSource = expectedItemsSource;
            var actualItemsSource = tabView.ItemsSource;

            Assert.NotNull(actualItemsSource);
            Assert.Equal(expectedItemsSource.Count, actualItemsSource.Count);

        }

        [Fact]
        public void HeaderItemTemplate_SetAndGet_ReturnsExpectedValue()
        {
            var tabView = new SfTabView();
            var expectedTemplate = new DataTemplate(() => new Label { Text = "Header" });

            tabView.HeaderItemTemplate = expectedTemplate;

            Assert.Equal(expectedTemplate, tabView.HeaderItemTemplate);
        }

        [Fact]
        public void ContentItemTemplate_SetAndGet_ReturnsExpectedValue()
        {
            var tabView = new SfTabView();
            var expectedTemplate = new DataTemplate(() => new Label { Text = "Test" });

            tabView.ContentItemTemplate = expectedTemplate;

            Assert.Equal(expectedTemplate, tabView.ContentItemTemplate);
        }

        [Fact]
        public void TabHeaderPadding_SetAndGet_ReturnsExpectedValue()
        {
            var tabView = new SfTabView();
            var expectedPadding = new Thickness(5, 10, 5, 10);

            tabView.TabHeaderPadding = expectedPadding;

            Assert.Equal(expectedPadding, tabView.TabHeaderPadding);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void FontAutoScalingEnabled_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var tabView = new SfTabView();
            tabView.FontAutoScalingEnabled = expected;

            Assert.Equal(expected, tabView.FontAutoScalingEnabled);
        }

        [Fact]
        public void IndicatorCornerRadius_SetAndGet_ReturnsExpectedValue()
        {
            var tabView = new SfTabView();
            var expectedRadius = new CornerRadius(5);

            tabView.IndicatorCornerRadius = expectedRadius;

            Assert.Equal(expectedRadius, tabView.IndicatorCornerRadius);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsScrollButtonEnabled_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var tabView = new SfTabView();
            tabView.IsScrollButtonEnabled = expected;

            Assert.Equal(expected, tabView.IsScrollButtonEnabled);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void EnableSwiping_SetAndGet_ReturnsExpectedValue(bool expected)
        {
            var tabView = new SfTabView();
            tabView.EnableSwiping = expected;

            Assert.Equal(expected, tabView.EnableSwiping);
        }

        [Theory]
        [InlineData(double.MinValue)]
        [InlineData(double.MaxValue)]
        [InlineData((double)0)]
        public void ContentTransitionDuration_SetAndGet_ReturnsExpectedValue(double expected)
        {
            var tabView = new SfTabView();
            tabView.ContentTransitionDuration = expected;

            Assert.Equal(expected, tabView.ContentTransitionDuration);
        }

		[Fact]
		public void TestTabView()
		{
			var tabView = new SfTabView();
			var tabItems = new TabItemCollection();
			tabView.Items = tabItems;
			Assert.Empty(tabView.Items);
		}

		#endregion

		#region TabView Public Events

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestTabItemTappedEventCancelProperty(bool shouldCancel)
		{
			var tabView = new SfTabView();
			var expectedTabItem = new SfTabItem { Header = "Test Tab" };
			var eventTriggered = false;
			tabView.TabItemTapped += (sender, e) =>
			{
				eventTriggered = true;
				e.Cancel = shouldCancel;
			};
			var eventInfo = typeof(SfTabView).GetField("TabItemTapped", BindingFlags.Instance | BindingFlags.NonPublic);
			var eventIn = eventInfo?.GetValue(tabView);
			Assert.NotNull(eventIn);
			MulticastDelegate eventDelegate = (MulticastDelegate)eventIn;

			if (eventDelegate != null)
			{
				var eventArgs = new TabItemTappedEventArgs { TabItem = expectedTabItem };
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[] { tabView, eventArgs });
				}
				Assert.True(eventTriggered, "TabItemTapped event was not triggered.");
				Assert.Equal(shouldCancel, eventArgs.Cancel);
			}
		}

		[Fact]
		public void TestTabItemTappedEventTriggered()
		{
			var tabView = new SfTabView();
			var wasEventTriggered = false;
			var expectedTabItem = new SfTabItem { Header = "Test Tab" };
			tabView.TabItemTapped += (sender, e) =>
			{
				wasEventTriggered = true;
				Assert.Equal(expectedTabItem, e.TabItem);
			};
			var eventInfo = typeof(SfTabView).GetField("TabItemTapped", BindingFlags.Instance | BindingFlags.NonPublic);
			var eventIn = eventInfo?.GetValue(tabView);
			Assert.NotNull(eventIn);
			MulticastDelegate eventDelegate = (MulticastDelegate)eventIn;
			if (eventDelegate != null)
			{
				var eventArgs = new TabItemTappedEventArgs { TabItem = expectedTabItem };
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[] { tabView, eventArgs });
				}
			}
			Assert.True(wasEventTriggered, "TabItemTapped event was not triggered.");
		}

		[Fact]
		public void TestTabViewSelectionChangedTriggers()
		{
			var tabView = new SfTabView { SelectedIndex = 0 };
			TabSelectionChangedEventArgs? receivedArgs = null;
			tabView.SelectionChanged += (sender, args) =>
			{
				receivedArgs = args;
			};
			tabView.SelectedIndex = 1;
			Assert.NotNull(receivedArgs);
			Assert.Equal(0, receivedArgs.OldIndex);
			Assert.Equal(1, receivedArgs.NewIndex);
			Assert.False(receivedArgs.Handled);
		}

		[Fact]
		public void TestTabViewSelectionChanged()
		{
			var tabView = new SfTabView { SelectedIndex = 0 };
			var eventTriggerCount = 0;
			TabSelectionChangedEventArgs? receivedArgs = null;
			tabView.SelectionChanged += (sender, args) =>
			{
				eventTriggerCount++;
				receivedArgs = args;
			};
			tabView.SelectedIndex = 1;
			tabView.SelectedIndex = 2;
			Assert.Equal(2, eventTriggerCount);
			Assert.NotNull(receivedArgs);
			Assert.Equal(1, receivedArgs.OldIndex);
			Assert.Equal(2, receivedArgs.NewIndex);
		}

		[Fact]
		public void TestSelectionChangingEventTriggered()
		{
			var tabView = new SfTabView();
			var wasEventTriggered = false;
			var expectedIndex = 1;
			tabView.SelectionChanging += (sender, e) =>
			{
				wasEventTriggered = true;
				Assert.Equal(expectedIndex, e.Index);
			};
			var eventArgs = new SelectionChangingEventArgs { Index = expectedIndex };
			var eventInfo = typeof(SfTabView).GetField("SelectionChanging", BindingFlags.Instance | BindingFlags.NonPublic);
			var eventIn = eventInfo?.GetValue(tabView);
			Assert.NotNull(eventIn);
			MulticastDelegate eventDelegate = (MulticastDelegate)eventIn;
			if (eventDelegate != null)
			{
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[] { tabView, eventArgs });
				}
			}
			Assert.True(wasEventTriggered, "SelectionChanging event was not triggered.");
		}

		[Fact]
		public void TestSelectionChangingEvent()
		{
			var tabView = new SfTabView();
			var wasEventCanceled = false;

			tabView.SelectionChanging += (sender, e) =>
			{
				e.Cancel = true;
				wasEventCanceled = e.Cancel;
			};
			var eventArgs = new SelectionChangingEventArgs { Index = 1 };
			var eventInfo = typeof(SfTabView).GetField("SelectionChanging", BindingFlags.Instance | BindingFlags.NonPublic);
			var eventIn = eventInfo?.GetValue(tabView);
			Assert.NotNull(eventIn);
			MulticastDelegate eventDelegate = (MulticastDelegate)eventIn;

			if (eventDelegate != null)
			{
				foreach (var handler in eventDelegate.GetInvocationList())
				{
					handler.Method.Invoke(handler.Target, new object[] { tabView, eventArgs });
				}
			}
			Assert.True(wasEventCanceled, "SelectionChanging event should have been canceled.");
		}

		#endregion

		#region TabBar Properties

		[Fact]
		public void TestScrollButtonDisabledIconColorSetValueShouldUpdateValue()
		{
			var disabledIconColor = Colors.Gray;
			tabBar.ScrollButtonDisabledIconColor = disabledIconColor;
			Assert.Equal(disabledIconColor, tabBar.ScrollButtonDisabledIconColor);
		}

		[Fact]
        public void TestTabBarItemsIndexesUsingHeader()
        {
            PopulateTabBarItems();
            if (tabBar is not null)
            {
                var matchingTabItem = tabBar.Items.FirstOrDefault(item => item.Header == "Favorites");
                if (matchingTabItem != null)
                {
                    Assert.Equal(1, tabBar.Items.IndexOf(matchingTabItem));
                    Assert.NotEqual(0, tabBar.Items.IndexOf(matchingTabItem));
                    Assert.NotEqual(2, tabBar.Items.IndexOf(matchingTabItem));
                }

                matchingTabItem = tabBar.Items.FirstOrDefault(item => item.Header == "Call");
                if (matchingTabItem != null)
                {
                    Assert.Equal(0, tabBar.Items.IndexOf(matchingTabItem));
                    Assert.NotEqual(1, tabBar.Items.IndexOf(matchingTabItem));
                    Assert.NotEqual(2, tabBar.Items.IndexOf(matchingTabItem));
                }

                matchingTabItem = tabBar.Items.FirstOrDefault(item => item.Header == "Contacts");

                if (matchingTabItem != null)
                {
                    Assert.Equal(2, tabBar.Items.IndexOf(matchingTabItem));
                    Assert.NotEqual(0, tabBar.Items.IndexOf(matchingTabItem));
                    Assert.NotEqual(1, tabBar.Items.IndexOf(matchingTabItem));
                }

                tabBar.Items = PopulateMixedItemsCollection();
                matchingTabItem = tabBar.Items.FirstOrDefault(item => item.Header == "TAB 3");

                if (matchingTabItem != null)
                {
                    Assert.Equal(2, tabBar.Items.IndexOf(matchingTabItem));
                    Assert.NotEqual(0, tabBar.Items.IndexOf(matchingTabItem));
                    Assert.NotEqual(1, tabBar.Items.IndexOf(matchingTabItem));
                }
            }
        }

        [Fact]
        public void TestTabBarItemsIndexes()
        {
            PopulateTabBarItems();
            if (tabBar != null)
            {
                Assert.Equal(3, tabBar.Items.Count());
                Assert.True(tabBar.Items[0].IsSelected);
                Assert.False(tabBar.Items[1].IsSelected);
                Assert.False(tabBar.Items[2].IsSelected);

                tabBar.ItemsSource = PopulateButtonsListItemsSource();
                Assert.Equal(3, tabBar.ItemsSource.Count);
                Assert.Equal(0, tabBar.SelectedIndex);
                Assert.NotEqual(1, tabBar.SelectedIndex);
                Assert.NotEqual(2, tabBar.SelectedIndex);
			}
		}

		[Fact]
		public void TestTabBarItemsCount()
		{
			SfTabView tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollection();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			if (tabBar != null)
			{
				Assert.Equal(3, tabBar.Items.Count);
				Assert.NotEmpty(tabBar.Items);
				Assert.NotEqual(1, tabBar.Items.Count);
				Assert.NotEqual(-1, tabBar.Items.Count);

				tabBar.ItemsSource = PopulateMixedObjectItemsSource();

				Assert.Equal(3, tabBar.ItemsSource.Count);
				Assert.NotEmpty(tabBar.ItemsSource);
				Assert.NotEqual(1, tabBar.ItemsSource.Count);
				Assert.NotEqual(-1, tabBar.ItemsSource.Count);
			}
		}

		[Fact]
		public void TestTabBarItemsHeaderText()
		{
			SfTabView tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollection();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			if (tabBar != null)
			{
				Assert.Equal("TAB 1", tabBar.Items[0].Header);
				Assert.NotEqual("TAB 2", tabBar.Items[0].Header);
				Assert.NotEqual("TAB 3", tabBar.Items[0].Header);
				Assert.Equal("TAB 2", tabBar.Items[1].Header);
				Assert.NotEqual("TAB 1", tabBar.Items[1].Header);
				Assert.NotEqual("TAB 3", tabBar.Items[1].Header);
				Assert.Equal("TAB 3", tabBar.Items[2].Header);
				Assert.NotEqual("TAB 1", tabBar.Items[2].Header);
				Assert.NotEqual("TAB 2", tabBar.Items[2].Header);
			}
		}

		[Fact]
		public void TestTabBarSingleItem()
		{
			var tabBar = new SfTabBar();
			var item = new SfTabItem();
			tabBar.Items.Add(item);
			Assert.Single(tabBar.Items);
			Assert.Contains(item, tabBar.Items);
		}

		[Fact]
		public void TestTabBarRemoveExistingItem()
		{
			var tabBar = new SfTabBar();
			var item = new SfTabItem();
			tabBar.Items.Add(item);
			tabBar.Items.Remove(item);
			Assert.Empty(tabBar.Items);
		}

		[Fact]
		public void TestIndicatorPlacementPropertyShouldBeBottom()
		{
			Assert.Equal(TabIndicatorPlacement.Bottom, tabBar.IndicatorPlacement);
		}

		[Fact]
		public void TestIndicatorPlacementPropertyShouldUpdateValue()
		{
			tabBar.IndicatorPlacement = TabIndicatorPlacement.Top;
			Assert.Equal(TabIndicatorPlacement.Top, tabBar.IndicatorPlacement);
		}

		[Fact]
		public void TestTabWidthModePropertyShouldBeDefault()
		{
			Assert.Equal(TabWidthMode.Default, tabBar.TabWidthMode);
		}

		[Fact]
		public void TestTabWidthModePropertyShouldUpdateValue()
		{
			tabBar.TabWidthMode = TabWidthMode.Default;
			Assert.Equal(TabWidthMode.Default, tabBar.TabWidthMode);
		}

		[Fact]
		public void CheckSelectedIndexPropertyShouldUpdateValue()
		{
			tabBar.SelectedIndex = 2;
			Assert.Equal(2, tabBar.SelectedIndex);
		}

		[Fact]
		public void TestIsScrollButtonEnabledPropertyShouldBeFalse()
		{
			Assert.False(tabBar.IsScrollButtonEnabled);
		}

		[Fact]

		public void CheckGetEffectsViewNull()
		{
			SfTabItem item = new SfTabItem();
			SfEffectsView? view = tabBar.GetEffectsView(item);
			Assert.Null(view);
		}

		[Fact]
		public void TestIsScrollButtonEnabledPropertyShouldUpdateValue()
		{
			tabBar.IsScrollButtonEnabled = true;
			Assert.True(tabBar.IsScrollButtonEnabled);
		}

		[Fact]
		public void TestGetPrivateFieldShouldReturnExpectedValue()
		{
			// Assuming there's a private field named '_defaultTextPadding'
			var value = GetPrivateField(tabBar, "_defaultTextPadding");
			Assert.Equal(36, value);
		}

		[Fact]
		public void SetPrivateFieldShouldUpdateValue()
		{
			// Assuming there's a private field named '_defaultTextPadding'
			SetPrivateField(tabBar, "_defaultTextPadding", 50);
			var value = GetPrivateField(tabBar, "_defaultTextPadding");
			Assert.Equal(50, value);
		}

		[Fact]
		public void TestItemsPropertyShouldUpdateValue()
		{
			var items = new TabItemCollection();
			tabBar.Items = items;
			Assert.Equal(items, tabBar.Items);
		}

		[Fact]
		public void CheckIndicatorBackgroundPropertyShouldUpdateValue()
		{
			var brush = new SolidColorBrush(Color.FromArgb("#FF0000"));
			tabBar.IndicatorBackground = brush;
			Assert.Equal(brush, tabBar.IndicatorBackground);
		}

		[Fact]
		public void CheckTabHeaderPaddingPropertyShouldUpdateValue()
		{
			var padding = new Thickness(10, 20, 30, 40);
			tabBar.TabHeaderPadding = padding;
			Assert.Equal(padding, tabBar.TabHeaderPadding);
		}

		[Fact]
		public void CheckHeaderHorizontalTextAlignmentShouldBeCenter()
		{
			Assert.Equal(TextAlignment.Center, tabBar.HeaderHorizontalTextAlignment);
		}

		[Fact]
		public void CheckHeaderHorizontalTextAlignment()
		{
			tabBar.HeaderHorizontalTextAlignment = TextAlignment.Start;
			Assert.Equal(TextAlignment.Start, tabBar.HeaderHorizontalTextAlignment);
		}

		[Fact]
		public void TestIndicatorWidthModeShouldBeFit()
		{
			Assert.Equal(IndicatorWidthMode.Fit, tabBar.IndicatorWidthMode);
		}

		[Fact]
		public void TestIndicatorWidthModeShouldUpdateValue()
		{
			tabBar.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			Assert.Equal(IndicatorWidthMode.Stretch, tabBar.IndicatorWidthMode);
		}

		[Fact]
		public void TestIndicatorCornerRadiusShouldBeNegativeOne()
		{
			Assert.Equal(new CornerRadius(-1), tabBar.IndicatorCornerRadius);
		}

		[Fact]
		public void TestIndicatorCornerRadius()
		{
			var cornerRadius = new CornerRadius(5);
			tabBar.IndicatorCornerRadius = cornerRadius;
			Assert.Equal(cornerRadius, tabBar.IndicatorCornerRadius);
		}

		[Fact]
		public void TestFontAutoScalingEnabled()
		{
			Assert.False(tabBar.FontAutoScalingEnabled);
		}

		[Fact]
		public void TestFontAutoScalingEnabledSetValueShouldUpdateValue()
		{
			tabBar.FontAutoScalingEnabled = true;
			Assert.True(tabBar.FontAutoScalingEnabled);
		}

		[Fact]
		public void TestScrollButtonBackgroundSetValueShouldUpdateValue()
		{
			var backgroundColor = Colors.Red;
			tabBar.ScrollButtonBackground = backgroundColor;
			Assert.Equal(backgroundColor, tabBar.ScrollButtonBackground);
		}

		[Fact]
		public void TestScrollButtonIconColorSetValueShouldUpdateValue()
		{
			var iconColor = Colors.Green;
			tabBar.ScrollButtonIconColor = iconColor;
			Assert.Equal(iconColor, tabBar.ScrollButtonIconColor);
		}

		[Fact]
		public void SelectedIndexCount()
		{
			tabBar.Items.Add(new SfTabItem { Header = "Tab 1" });
			tabBar.Items.Add(new SfTabItem { Header = "Tab 2" });
			Assert.Equal(0, tabBar.SelectedIndex);
			Assert.True(tabBar.Items.Count > 0);
		}

		#endregion

		#region TabBar Internal Method

		[Fact]
		public void TestUpdateSelectedTabItemIsSelected()
		{
			PopulateTabBarItems();
			if (tabBar != null)
			{
				tabBar.UpdateSelectedTabItemIsSelected(1, 0);
				Assert.True(tabBar.Items[1].IsSelected);
				Assert.True(!tabBar.Items[0].IsSelected);
				Assert.True(!tabBar.Items[2].IsSelected);

				tabBar.UpdateSelectedTabItemIsSelected(2, 1);
				Assert.True(tabBar.Items[2].IsSelected);
				Assert.True(!tabBar.Items[1].IsSelected);
				Assert.True(!tabBar.Items[0].IsSelected);

				tabBar.UpdateSelectedTabItemIsSelected(0, -1);
				Assert.True(tabBar.Items[0].IsSelected);
				Assert.True(!tabBar.Items[1].IsSelected);

				tabBar.UpdateSelectedTabItemIsSelected(-1, 0);
				Assert.True(tabBar.Items[2].IsSelected);
				Assert.True(!tabBar.Items[0].IsSelected);
				Assert.True(!tabBar.Items[1].IsSelected);

				tabBar.Items.Clear();
				tabBar.UpdateSelectedTabItemIsSelected(0, 0);
				Assert.Empty(tabBar.Items);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top)]
		[InlineData(TabIndicatorPlacement.Bottom)]
		[InlineData(TabIndicatorPlacement.Fill)]
		public void TestSfTabBarIndicatorPlacement(TabIndicatorPlacement expected)
		{
			SfTabView tabView = new SfTabView();
			tabView.IndicatorPlacement = expected;
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			if (tabBar != null)
			{
				Assert.Equal(expected, tabBar.IndicatorPlacement);
				tabBar.HeightRequest = 60;
				tabBar.UpdateHeaderItemHeight();
				SfBorder? tabSelectionIndicator = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as SfBorder;
				SfGrid? tabHeaderContentContainer = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderContentContainer") as SfGrid;
				SfScrollView? tabHeaderScrollView = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderScrollView") as SfScrollView;

				if (expected == TabIndicatorPlacement.Fill)
				{
					Assert.Equal(60, tabSelectionIndicator?.HeightRequest);
					Assert.Equal(60, tabHeaderScrollView?.HeightRequest);
				}
				else
				{
					Assert.Equal(3, tabSelectionIndicator?.HeightRequest);
				}

				Assert.Equal(60, tabHeaderContentContainer?.HeightRequest);
				Assert.Equal(60, tabHeaderScrollView?.HeightRequest);
			}
		}

		[Fact]
		public void TestSfTabBarCornerRadiusUpdated()
		{
			SfTabView tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollection();
			tabView.IndicatorCornerRadius = new CornerRadius(6);
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			if (tabBar != null)
			{
				tabBar.UpdateCornerRadius();
				var roundRectangle = GetPrivateField<SfTabBar>(tabBar, "_roundRectangle");
				Assert.Equal(new CornerRadius(6), (roundRectangle as RoundRectangle)?.CornerRadius);

				tabView.IndicatorCornerRadius = new CornerRadius(0);
				tabBar.UpdateCornerRadius();
				Assert.Equal(new CornerRadius(0), (roundRectangle as RoundRectangle)?.CornerRadius);

				tabView.IndicatorCornerRadius = new CornerRadius(-1);
				tabBar.UpdateCornerRadius();
				Assert.Equal(new CornerRadius(-1), (roundRectangle as RoundRectangle)?.CornerRadius);
			}
		}

		[Fact]
		public void TestUpdatemethods()
		{
			tabBar.UpdateFlowDirection();
			tabBar.UpdateHeaderItemHeight();
			tabBar.IsScrollButtonEnabled = true;
			Assert.True(tabBar.IsScrollButtonEnabled);
		}

		[Fact]
		public void TestUpdateFlowDirectionLTR()
		{
			SfTabBar _tabBar = new SfTabBar();
			_tabBar.FlowDirection = FlowDirection.LeftToRight;
			_tabBar.IsScrollButtonEnabled = true;
			_tabBar.UpdateFlowDirection();
			var flowDirection = _tabBar.FlowDirection;
			Assert.Equal(FlowDirection.LeftToRight, flowDirection);
		}

		[Fact]
		public void TestUpdateFlowDirectionRTL()
		{
			SfTabBar _tabBar = new SfTabBar();
			_tabBar.FlowDirection = FlowDirection.RightToLeft;
			_tabBar.UpdateFlowDirection();
			var flowDirection = _tabBar.FlowDirection;
			Assert.Equal(FlowDirection.RightToLeft, flowDirection);
		}

		[Fact]
		public void TestUpdateFlowDirectionMultiple()
		{
			SfTabBar _tabBar = new SfTabBar();
			_tabBar.FlowDirection = FlowDirection.RightToLeft;
			_tabBar.UpdateFlowDirection();
			var flowDirection = _tabBar.FlowDirection;
			Assert.Equal(FlowDirection.RightToLeft, flowDirection);
			_tabBar.FlowDirection = FlowDirection.MatchParent;
			flowDirection = _tabBar.FlowDirection;
			Assert.Equal(FlowDirection.MatchParent, flowDirection);
		}

		[Fact]
		public void TestUpdateFlowDirection()
		{
			SfTabBar _tabBar = new SfTabBar();
			_tabBar.UpdateFlowDirection();
			var flowDirection = _tabBar.FlowDirection;
			Assert.Equal(FlowDirection.MatchParent, flowDirection);
		}

		#endregion

		#region TabBar Public Method

		[Fact]
		public void TestOnTapInvalidIndexOutOfRange()
		{
			var items = tabBar.Items;
			var tabItem = new SfTabItem();
			items.Add(tabItem);
			TapEventArgs tapEvent = new TapEventArgs(new Point(10, 2), 1);
			var selectedIndex = tabBar.SelectedIndex;
			tabBar.OnTap(tabItem, tapEvent);
			Assert.Equal(0, selectedIndex);
		}

		[Fact]
		public void TestOnTapValidIndexMultipleItems()
		{
			SfTabBar _sfTabBar = new SfTabBar();
			var items = _sfTabBar.Items;
			var tabItem1 = new SfTabItem();
			var tabItem2 = new SfTabItem();
			items.Add(tabItem1);
			items.Add(tabItem2);

			TapEventArgs tapEvent = new TapEventArgs(new Point(10, 2), 1);
			var selectedIndex = tabBar.SelectedIndex;
			tabBar.OnTap(tabItem1, tapEvent);
			tabBar.OnTap(tabItem2, tapEvent);
			Assert.Equal(_sfTabBar.SelectedIndex, selectedIndex);
		}

		#endregion

		#region Behaviour Tests

		[Theory]
        [InlineData(TextAlignment.End)]
        [InlineData(TextAlignment.Start)]
        [InlineData(TextAlignment.Center)]
        public void TestSfTabBarHeaderHorizontalTextAlignment(TextAlignment expected)
        {
            SfTabView tabView = new SfTabView();
            tabView.HeaderHorizontalTextAlignment = expected;
            SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
            Assert.Equal(expected, tabBar?.HeaderHorizontalTextAlignment);
        }

        [Theory]
        [InlineData(IndicatorWidthMode.Fit)]
        [InlineData(IndicatorWidthMode.Stretch)]
        public void TestSfTabBarIndicatorWidthMode(IndicatorWidthMode expected)
        {
            SfTabView tabView = new SfTabView();
            tabView.IndicatorWidthMode = expected;
            SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
            Assert.Equal(expected, tabBar?.IndicatorWidthMode);
        }

        [Theory]
        [InlineData(TabWidthMode.Default)]
        [InlineData(TabWidthMode.SizeToContent)]
        public void TestSfTabBarTabWidthMode(TabWidthMode expected)
        {
            SfTabView tabView = new SfTabView();
            tabView.TabWidthMode = expected;
            SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
            Assert.Equal(expected, tabBar?.TabWidthMode);
        }

        [Fact]
        public void TestSfTabBarTabHeaderPadding()
        {
            SfTabView tabView = new SfTabView();
            SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
            tabView.TabHeaderPadding = new Thickness(-5);
            if (tabBar != null)
            {
                Assert.Equal(new Thickness(-5), tabBar.TabHeaderPadding);
                Assert.NotEqual(new Thickness(0), tabBar.TabHeaderPadding);
                tabView.TabHeaderPadding = new Thickness(0);
                Assert.Equal(new Thickness(0), tabBar.TabHeaderPadding);
                Assert.NotEqual(new Thickness(-5), tabBar.TabHeaderPadding);
                tabView.TabHeaderPadding = new Thickness(0, -5, 5, 10);
                Assert.Equal(new Thickness(0, -5, 5, 10), tabBar.TabHeaderPadding);
                tabView.TabHeaderPadding = new Thickness(5, 10, 5, 10);
                Assert.Equal(new Thickness(5, 10, 5, 10), tabBar.TabHeaderPadding);
            }
        }

        [Fact]
        public void TestSfTabBarIndicatorBackground()
        {
            SfTabView tabView = new SfTabView();
            SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
            tabView.IndicatorBackground = Colors.Maroon;
            if (tabBar != null)
            {
                Assert.Equal(Colors.Maroon, tabBar.IndicatorBackground);
                tabView.IndicatorBackground = Colors.Red;
                Assert.Equal(Colors.Red, tabBar.IndicatorBackground);
                Assert.NotEqual(Colors.Maroon, tabBar.IndicatorBackground);
                tabView.IndicatorBackground = Colors.Transparent;
                Assert.Equal(Colors.Transparent, tabBar.IndicatorBackground);
                Assert.NotEqual(Colors.Red, tabBar.IndicatorBackground);
            }
        }

		[Fact]
		public void TestSfTabBarIndicatorCornerRadius()
		{
			SfTabView tabView = new SfTabView();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			tabView.IndicatorCornerRadius = new CornerRadius(0);
			if (tabBar != null)
			{
				Assert.Equal(new CornerRadius(0), tabBar.IndicatorCornerRadius);
				tabView.IndicatorCornerRadius = new CornerRadius(-5);
				Assert.Equal(new CornerRadius(-5), tabBar.IndicatorCornerRadius);
				Assert.NotEqual(new CornerRadius(0), tabBar.IndicatorCornerRadius);
				Assert.NotEqual(new CornerRadius(5), tabBar.IndicatorCornerRadius);
				tabView.IndicatorCornerRadius = new CornerRadius(5);
				Assert.Equal(new CornerRadius(5), tabBar.IndicatorCornerRadius);
				Assert.NotEqual(new CornerRadius(0), tabBar.IndicatorCornerRadius);
				tabView.IndicatorCornerRadius = new CornerRadius(5, 10, 5, 10);
				Assert.Equal(new CornerRadius(5, 10, 5, 10), tabBar.IndicatorCornerRadius);
			}
		}

		[Theory]
		[InlineData(TabBarDisplayMode.Default)]
		[InlineData(TabBarDisplayMode.Image)]
		[InlineData(TabBarDisplayMode.Text)]
		public void TestSfTabBarHeaderDisplayMode(TabBarDisplayMode expected)
		{
			SfTabView tabView = new SfTabView();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			tabView.HeaderDisplayMode = expected;
			Assert.Equal(expected, tabBar?.HeaderDisplayMode);
		}

		[Theory]
		[InlineData(-200)]
		[InlineData(200)]
		[InlineData(0)]
		[InlineData(3000)]
		[InlineData(5000)]
		public void TestSfTabBarContentTransitionDuration(double expected)
		{
			SfTabView tabView = new SfTabView();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			tabView.ContentTransitionDuration = expected;
			if (expected > 100)
				Assert.Equal(expected, tabBar?.ContentTransitionDuration);
			else
				Assert.Equal(100, tabBar?.ContentTransitionDuration);
		}

		[Fact]
		public void TestSfTabBarFontAutoScalingEnabled()
		{
			SfTabView tabView = new SfTabView();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			tabView.FontAutoScalingEnabled = true;
			if (tabBar != null)
			{
				Assert.True(tabBar.FontAutoScalingEnabled);
				tabView.FontAutoScalingEnabled = false;
				Assert.False(tabBar.FontAutoScalingEnabled);
			}
		}

		[Fact]
		public void TestSfTabBarScrollButtonAppearance()
		{
			SfTabView tabView = new SfTabView();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			tabView.ScrollButtonBackground = Colors.Green;
			tabView.ScrollButtonIconColor = Colors.Green;
			tabView.ScrollButtonBackground = Colors.Green;
			if (tabBar != null)
			{
				Assert.Equal(Colors.Green, tabBar.ScrollButtonBackground);
				Assert.Equal(Colors.Green, tabBar.ScrollButtonIconColor);
				Assert.Equal(Colors.Green, tabBar.ScrollButtonBackground);

				tabView.ScrollButtonBackground = Colors.Transparent;
				tabView.ScrollButtonIconColor = Colors.Transparent;
				tabView.ScrollButtonBackground = Colors.Transparent;
				Assert.Equal(Colors.Transparent, tabBar.ScrollButtonBackground);
				Assert.NotEqual(Colors.Green, tabBar.ScrollButtonBackground);
				Assert.Equal(Colors.Transparent, tabBar.ScrollButtonIconColor);
				Assert.NotEqual(Colors.Green, tabBar.ScrollButtonIconColor);
				Assert.Equal(Colors.Transparent, tabBar.ScrollButtonBackground);
				Assert.NotEqual(Colors.Green, tabBar.ScrollButtonBackground);
			}
		}

		[Fact]
		public void TestTabBarGetEffectsView()
		{
			SfTabView tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollection();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			if (tabBar != null)
			{
				var effectsView = tabBar.GetEffectsView(tabBar.Items[0]);
				Assert.NotNull(effectsView);
			}
		}

		[Fact]
		public void TestSfTabBarItems()
		{
			SfTabBar tabBar = new SfTabBar();
			SfHorizontalStackLayout? tabHeaderItemContent = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderItemContent") as SfHorizontalStackLayout;
			Assert.NotNull(tabHeaderItemContent);
			Assert.Equal(0, tabHeaderItemContent?.Children.Count);
			Assert.NotNull(tabBar?.Items);
			Assert.Equal(0, tabBar?.Items.Count);

			var tabItems = PopulateMixedItemsCollection();
			if (tabBar != null)
			{
				tabBar.Items = tabItems;
				Assert.NotNull(tabBar.Items);
				Assert.Equal(3, tabBar.Items.Count);

				Assert.NotNull(tabHeaderItemContent);
				Assert.Equal(3, tabHeaderItemContent.Children.Count);
				SfBorder? tabSelectionIndicator = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as SfBorder;
				Assert.NotEqual(0.01d, tabSelectionIndicator?.WidthRequest);

				tabBar.Items.Clear();
				Assert.NotNull(tabBar.Items);
				Assert.Empty(tabBar.Items);
				Assert.Equal(0, tabHeaderItemContent?.Children.Count);

				Assert.Equal(0.01d, tabSelectionIndicator?.WidthRequest);

				tabBar.Items = new TabItemCollection();
				Assert.NotNull(tabBar.Items);
				Assert.Empty(tabBar.Items);
				Assert.Equal(0, tabHeaderItemContent?.Children.Count);
				Assert.Equal(0.01d, tabSelectionIndicator?.WidthRequest);
			}
		}

		[Fact]
		public void TestSfTabBarNestedTabViewItems()
		{
			SfTabView tabView = new SfTabView();
			var tabViewItems = PopulateLabelItemsCollection();
			tabView.Items = tabViewItems;
			SfTabBar tabBar = new SfTabBar();
			Assert.NotNull(tabBar?.Items);
			Assert.Equal(0, tabBar?.Items.Count);

			var tabBarItems = new TabItemCollection()
			{
				new SfTabItem { Header = "TAB bar 1", Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "TAB bar 2", Content = tabView },
			};

			if (tabBar != null)
			{
				tabBar.Items = tabBarItems;

				SfScrollView? tabHeaderScrollView = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderScrollView") as SfScrollView;
				SfGrid? tabHeaderContentContainer = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderContentContainer") as SfGrid;
				SfHorizontalStackLayout? tabHeaderItemContent = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderItemContent") as SfHorizontalStackLayout;
				SfBorder? tabSelectionIndicator = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as SfBorder;

				Assert.NotNull(tabHeaderScrollView);
				Assert.NotNull(tabHeaderScrollView.Style);

				Assert.NotNull(tabHeaderContentContainer);
				Assert.NotNull(tabHeaderContentContainer.Style);

				Assert.NotNull(tabHeaderItemContent);
				Assert.NotNull(tabHeaderItemContent.Style);

				Assert.NotNull(tabSelectionIndicator);
				Assert.NotNull(tabSelectionIndicator.Style);

				Assert.NotNull(tabBar.Items);
				Assert.Equal(2, tabBar.Items.Count);

				Assert.True(tabBar.Items[1].Content is SfTabView);

				if (tabBar.Items[1].Content is SfTabView nestedTabView)
				{
					if (nestedTabView != null)
					{
						Assert.NotNull(nestedTabView.Items);
						Assert.Equal(3, nestedTabView?.Items.Count);
					}
				}

				Assert.Equal(2, tabHeaderItemContent?.Children.Count);
				tabBar.Items.RemoveAt(0);
				Assert.Equal(1, tabHeaderItemContent?.Children.Count);

				tabBar.Items?.Clear();
				Assert.NotNull(tabBar.Items);
				Assert.Empty(tabBar.Items);
				Assert.Equal(0, tabHeaderItemContent?.Children.Count);

				tabBar.Items = new TabItemCollection();
				Assert.NotNull(tabBar?.Items);
			}
		}

		[Fact]
		public void TestSfTabBarLargeTabData()
		{
			SfTabBar tabBar = new SfTabBar();
			var largeDataSet = PopulateLargeData();
			tabBar.ItemsSource = largeDataSet;
			Assert.Equal(1000, tabBar?.ItemsSource.Count);
			Assert.Equal("Item 11", tabBar?.ItemsSource[10]);

			if (tabBar != null)
			{
				SfHorizontalStackLayout? tabHeaderItemContent = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderItemContent") as SfHorizontalStackLayout;
				Assert.Equal(1000, tabHeaderItemContent?.Children.Count);
			}
		}

		[Fact]
		public void TestSfTabBarMixedData()
		{
			SfTabBar tabBar = new SfTabBar();

			if (tabBar != null)
			{
				tabBar.ItemsSource = PopulateMixedDataItemsSource();
				Assert.Equal(4, tabBar.ItemsSource?.Count);
				if (tabBar.ItemsSource != null)
					Assert.True(tabBar.ItemsSource[2] is SfTabItem);
				SfHorizontalStackLayout? tabHeaderItemContent = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderItemContent") as SfHorizontalStackLayout;
				Assert.Equal(4, tabHeaderItemContent?.Children.Count);
				Assert.True(tabHeaderItemContent?.Children[2] is Label);
				var item = tabHeaderItemContent.Children[0];
				if (item != null && item is Label label && label != null)
				{
					Assert.Equal("String item", label.Text);
				}
			}
		}

		[Theory]
		[InlineData(0, 0, 0, 0)]
		[InlineData(1, 0, 0, 1)]
		[InlineData(0, 1, 0, 1)]
		[InlineData(0.5f, 0.5f, 0.5f, 1)]
		public void TestScrollButtonDisabledIconColor(float r, float g, float b, float a)
		{
			var tabView = new SfTabView();
			var expectedColor = new Color(r, g, b, a);
			tabView.ScrollButtonDisabledIconColor = expectedColor;
			Assert.Equal(expectedColor, tabView.ScrollButtonDisabledIconColor);
			SfTabBar? tabHeaderItemContent = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(expectedColor, tabHeaderItemContent?.ScrollButtonDisabledIconColor);
		}

		[Theory]
		[InlineData(TabImagePosition.Top)]
		[InlineData(TabImagePosition.Bottom)]
		[InlineData(TabImagePosition.Right)]
		[InlineData(TabImagePosition.Left)]
		public void TestTabViewImagePosition(TabImagePosition imagePosition)
		{
			var tabView = new SfTabView();
			var tabItems = new TabItemCollection()
			{
				new SfTabItem
				{
					ImagePosition = imagePosition
				}
			};

			tabView.Items = tabItems;
			Assert.NotNull(tabView);
			Assert.Single(tabView.Items);
			Assert.Equal(imagePosition, tabView.Items[0].ImagePosition);
		}

		[Theory]
		[InlineData(TabBarDisplayMode.Text)]
		[InlineData(TabBarDisplayMode.Image)]
		[InlineData(TabBarDisplayMode.Default)]
		public void TestSetHeaderDisplayMode(TabBarDisplayMode displayMode)
		{
			var tabView = new SfTabView();
			tabView.HeaderDisplayMode = displayMode;
			Assert.Equal(displayMode, tabView.HeaderDisplayMode);

			SfTabBar? tabHeaderContainer = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(displayMode, tabHeaderContainer?.HeaderDisplayMode);
		}

		[Fact]
		public void TestTabContentContainerSetItemsSourceAndContentItemTemplate()
		{
			var tabView = new SfTabView();
			var tabContentContainerField = typeof(SfTabView).GetField("_tabContentContainer", BindingFlags.NonPublic | BindingFlags.Instance);
			var tabContentContainer = new SfHorizontalContent(tabView, new SfTabBar());
			tabContentContainerField?.SetValue(tabView, tabContentContainer);
			var expectedItemsSource = new object[] { "Content1", "Content2" };
			var expectedContentItemTemplate = new DataTemplate(typeof(Label));
			typeof(SfTabView)?.GetProperty("ItemsSource", BindingFlags.Public | BindingFlags.Instance)?.SetValue(tabView, expectedItemsSource);
			typeof(SfTabView)?.GetProperty("ContentItemTemplate", BindingFlags.Public | BindingFlags.Instance)?.SetValue(tabView, expectedContentItemTemplate);
			Assert.Equal(expectedItemsSource, tabContentContainer.ItemsSource);
			Assert.Equal(expectedContentItemTemplate, tabContentContainer.ContentItemTemplate);
		}

		[Theory]
		[InlineData(-10d)]
		[InlineData(0d)]
		[InlineData(10d)]
		public void TestTabBarHeight(double tabBarHeight)
		{
			var tabView = new SfTabView();
			tabView.TabBarHeight = tabBarHeight;
			Assert.Equal(tabBarHeight, tabView.TabBarHeight);
			SfTabBar? tabHeaderContainer = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			if (tabHeaderContainer != null)
			{
				if (tabBarHeight > 0)
				{
					Assert.True(tabHeaderContainer.IsVisible);
					Assert.Equal(tabBarHeight, tabHeaderContainer.HeightRequest);
				}
				else
				{
					Assert.False(tabHeaderContainer.IsVisible);
					Assert.Equal(48, tabHeaderContainer.HeightRequest);
				}
			}
		}

		[Theory]
		[InlineData(TabBarDisplayMode.Image)]
		[InlineData(TabBarDisplayMode.Text)]
		public void TestSetHeaderDisplayModeTriggersPropertyChanged(TabBarDisplayMode displayMode)
		{
			var tabView = new SfTabView();
			bool eventRaised = false;
			tabView.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(tabView.HeaderDisplayMode))
				{
					eventRaised = true;
				}
			};
			tabView.HeaderDisplayMode = displayMode;
			Assert.True(eventRaised, $"PropertyChanged was not raised for {displayMode}");
		}

		[Theory]
		[InlineData(FlowDirection.RightToLeft)]
		[InlineData(FlowDirection.LeftToRight)]
		[InlineData(FlowDirection.MatchParent)]
		public void TestTabHeaderContainerFlowDirectionChanges(FlowDirection flowDirection)
		{
			var tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollection();
			var tabHeaderContainerField = typeof(SfTabView).GetField("_tabHeaderContainer", BindingFlags.NonPublic | BindingFlags.Instance);
			var tabHeaderContainer = new SfTabBar();
			tabHeaderContainerField?.SetValue(tabView, tabHeaderContainer);
			tabView.FlowDirection = flowDirection;
			Assert.Equal(flowDirection, tabHeaderContainer.FlowDirection);
		}

		[Fact]
		public void TestUpdateFontAutoScalingEnabled()
		{
			bool isEnable = false;
			SfTabItem tabItem = new SfTabItem() { Header = "Tab 1" };
			SfTabItem tabItem1 = new SfTabItem() { Header = "Tab 2" };
			SfTabBar tabBar = new SfTabBar();
			tabBar.Items.Add(tabItem);
			tabBar.Items.Add(tabItem1);
			tabBar.UpdateFontAutoScalingEnabled(isEnable);
			Assert.Equal(tabItem.FontAutoScalingEnabled, tabItem1.FontAutoScalingEnabled);
		}

		[Fact]
		public void TestClearHeaderItemWithValidIndex()
		{
			tabBar.Items.Add(new SfTabItem { Header = "Tab 1" });
			tabBar.Items.Add(new SfTabItem { Header = "Tab 2" });
			Assert.Equal(2, tabBar.Items.Count);
			tabBar.SelectedIndex = 3;
			tabBar.ClearHeaderItem(tabBar.Items[1], 1);
			Assert.Equal(1, tabBar.SelectedIndex);
		}

		[Fact]
		public void TestClearHeaderItemWithInvalidIndex()
		{
			tabBar.Items.Add(new SfTabItem { Header = "Tab 1" });
			Assert.Single(tabBar.Items);
			var exception = Record.Exception(() => tabBar.ClearHeaderItem(tabBar.Items[5], 5));
			Assert.NotNull(exception);
			Assert.IsType<ArgumentOutOfRangeException>(exception);
		}

		[Fact]
		public void TestClearHeaderItemWithNegativeIndex()
		{
			tabBar.Items.Add(new SfTabItem { Header = "Tab 1" });
			Assert.Single(tabBar.Items);
			var exception = Record.Exception(() => tabBar.ClearHeaderItem(tabBar.Items[-1], -1));
			Assert.NotNull(exception);
			Assert.IsType<ArgumentOutOfRangeException>(exception);
		}

		[Fact]
		public void TestClearHeaderItemWithEmptyItems()
		{
			Assert.Empty(tabBar.Items);
			var exception = Record.Exception(() => tabBar.ClearHeaderItem(tabBar.Items[0], 0));
			Assert.NotNull(exception);
			Assert.IsType<ArgumentOutOfRangeException>(exception);
		}

		[Fact]
		public void TestClearHeaderItemWithValidIndexMultipleTimes()
		{
			tabBar.Items.Add(new SfTabItem { Header = "Tab 1" });
			tabBar.Items.Add(new SfTabItem { Header = "Tab 2" });
			Assert.Equal(2, tabBar.Items.Count);
			tabBar.ClearHeaderItem(tabBar.Items[0], 0);
			tabBar.ClearHeaderItem(tabBar.Items[1], 1);
			Assert.Equal("Tab 1", tabBar.Items[0].Header);
			Assert.Equal("Tab 2", tabBar.Items[1].Header);
		}

		[Fact]
		public void MeasureHeaderContentWidth()
		{
			tabBar.Items.Add(new SfTabItem { Header = "Tab 1" });
			tabBar.Items.Add(new SfTabItem { Header = "Tab 2" });
			Assert.True(tabBar.Items[0].ImageSource == null);
			Assert.True(tabBar.Items != null && tabBar.Items.Count > 0);
		}

		#endregion

		#region TabBar Private Methods

		[Fact]
		public void ClearIndicatorWidth()
		{
			tabBar.ItemsSource = new ObservableCollection<Model>();
			InvokePrivateMethod(tabBar, "ClearIndicatorWidth");
			SfBorder? indicator = GetPrivateField(tabBar, "_tabSelectionIndicator") as SfBorder;
			Assert.Equal(indicator?.WidthRequest, 0.01d);
		}

		[Fact]
		public void TestTabSelectionIndicatorForNonFill()
		{
			PopulateTabBarItems();
			if (tabBar != null)
			{
				var roundRectangle = GetPrivateField<SfTabBar>(tabBar, "_roundRectangle");
				tabBar.IndicatorCornerRadius = new CornerRadius(-1);
				InvokePrivateMethod(tabBar, "UpdateTabSelectionIndicatorForNonFill", TabIndicatorPlacement.Bottom);
				Assert.Equal(new CornerRadius(4, 4, 0, 0), (roundRectangle as RoundRectangle)?.CornerRadius);

				tabBar.IndicatorCornerRadius = new CornerRadius(-1);
				InvokePrivateMethod(tabBar, "UpdateTabSelectionIndicatorForNonFill", TabIndicatorPlacement.Top);
				Assert.Equal(new CornerRadius(0, 0, 4, 4), (roundRectangle as RoundRectangle)?.CornerRadius);
			}
		}

		[Fact]
		public void TestUpdateTabSelectionIndicatorForFill()
		{
			PopulateTabBarItems();
			if (tabBar != null)
			{
				var roundRectangle = GetPrivateField<SfTabBar>(tabBar, "_roundRectangle") as RoundRectangle;
				if (roundRectangle != null)
				{
					tabBar.IndicatorCornerRadius = new CornerRadius(-1);
					InvokePrivateMethod(tabBar, "UpdateTabSelectionIndicator", TabIndicatorPlacement.Fill);
					Assert.Equal(new CornerRadius(0), roundRectangle.CornerRadius);

					tabBar.IndicatorCornerRadius = new CornerRadius(-5, -5, 5, 5);
					InvokePrivateMethod(tabBar, "UpdateTabSelectionIndicator", TabIndicatorPlacement.Fill);
					Assert.Equal(new CornerRadius(-5, -5, 5, 5), roundRectangle.CornerRadius);

					tabBar.IndicatorCornerRadius = new CornerRadius(-5, -5, -5, -5);
					InvokePrivateMethod(tabBar, "UpdateTabSelectionIndicator", TabIndicatorPlacement.Fill);
					Assert.Equal(new CornerRadius(-5, -5, -5, -5), roundRectangle.CornerRadius);

					tabBar.IndicatorCornerRadius = new CornerRadius(5, 5, 5, 5);
					InvokePrivateMethod(tabBar, "UpdateTabSelectionIndicator", TabIndicatorPlacement.Fill);
					Assert.Equal(new CornerRadius(5, 5, 5, 5), roundRectangle.CornerRadius);

					tabBar.IndicatorCornerRadius = new CornerRadius(0);
					InvokePrivateMethod(tabBar, "UpdateTabSelectionIndicator", TabIndicatorPlacement.Fill);
					Assert.Equal(new CornerRadius(0), roundRectangle.CornerRadius);
				}
			}
		}

		[Fact]
		public void TestSfTabBarUpdateTabPadding()
		{
			SfTabBar tabBar = new SfTabBar();
			tabBar.Items = PopulateMixedItemsCollection();
			tabBar.TabWidthMode = TabWidthMode.Default;
			InvokePrivateMethod(tabBar, "UpdateTabPadding");
			var tabHeaderContentContainer = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderContentContainer");
			Assert.Equal(new Thickness(0), (tabHeaderContentContainer as SfGrid)?.Padding);
		}

		[Fact]
		public void TestGetNextVisibleItem()
		{
			SfTabBar tabBar = new SfTabBar();
			tabBar.Items = PopulateMixedItemsCollection();
			InvokePrivateMethod(tabBar, "UpdateTabPadding");
			Assert.Equal(0, tabBar.SelectedIndex);
		}

		[Fact]
		public void TestResetEffectsView()
		{
			SfTabView tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollection();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;

			if (tabBar != null)
			{
				var effectsView = tabBar.GetEffectsView(tabBar.Items[0]);

				InvokePrivateMethod(tabBar, "ResetEffectsView", effectsView);
				Assert.True(effectsView?.ForceReset);

				effectsView = tabBar.GetEffectsView(tabBar.Items[1]);

				InvokePrivateMethod(tabBar, "ResetEffectsView", effectsView);
				Assert.True(effectsView?.ForceReset);

				effectsView = tabBar.GetEffectsView(tabBar.Items[2]);

				InvokePrivateMethod(tabBar, "ResetEffectsView", effectsView);
				Assert.True(effectsView?.ForceReset);
			}
		}

		[Fact]
		public void TestOnTabItemTouched()
		{
			tabBar = new SfTabBar();
			tabBar.ItemsSource = PopulateButtonsListItemsSource();
			var pointerEventArgs = new PointerEventArgs(1, PointerActions.Cancelled, new Point(10, 10));
			SfTabItem item = new SfTabItem() { Header = "Tab 1" };
			tabBar.Items.Add(item);
			InvokePrivateMethod(tabBar, "OnTabItemTouched", item, pointerEventArgs);
		}

		[Fact]
		public void TestHandleTabItemTapped()
		{
			SfTabView tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollection();
			SfTabBar? tabBar = (tabView.Children.FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			if (tabBar != null)
			{
				var effectsView = tabBar.GetEffectsView(tabBar.Items[0]);

				InvokePrivateMethod(tabBar, "HandleTabItemTapped", tabBar.Items[0]);
				var tabItemTappedEventArgs = GetPrivateField<SfTabBar>(tabBar, "_tabItemTappedEventArgs");
				Assert.Equal(tabBar.Items[0], (tabItemTappedEventArgs as TabItemTappedEventArgs)?.TabItem);

				InvokePrivateMethod(tabBar, "HandleTabItemTapped", tabBar.Items[1]);
				tabItemTappedEventArgs = GetPrivateField<SfTabBar>(tabBar, "_tabItemTappedEventArgs");
				Assert.Equal(tabBar.Items[1], (tabItemTappedEventArgs as TabItemTappedEventArgs)?.TabItem);

				InvokePrivateMethod(tabBar, "HandleTabItemTapped", tabBar.Items[2]);
				tabItemTappedEventArgs = GetPrivateField<SfTabBar>(tabBar, "_tabItemTappedEventArgs");
				Assert.Equal(tabBar.Items[2], (tabItemTappedEventArgs as TabItemTappedEventArgs)?.TabItem);
			}
		}


		[Fact]
		public void TestMixedObjectItemSource()
		{
			SfTabBar tabBar = new SfTabBar();
			Assert.Null(tabBar.ItemsSource);
			tabBar.ItemsSource = new List<object>();
			Assert.NotNull(tabBar.ItemsSource);
			Assert.Empty(tabBar.ItemsSource);

			var tabItems = PopulateMixedObjectItemsSource();
			if (tabBar != null)
				tabBar.ItemsSource = tabItems;

			InvokePrivateMethod(tabBar, "UpdateItemsSource");
			if (tabBar != null)
			{
				SfHorizontalStackLayout? tabHeaderItemContent = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderItemContent") as SfHorizontalStackLayout;

				Assert.Equal(3, tabHeaderItemContent?.Count);
				SfBorder? tabSelectionIndicator = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as SfBorder;

				Assert.NotNull(tabBar.ItemsSource);
				Assert.Equal(3, tabBar.ItemsSource.Count);

				tabBar.ItemsSource.Clear();
				Assert.NotNull(tabBar.ItemsSource);
				Assert.Empty(tabBar.ItemsSource);

				Assert.Equal(0.01d, tabSelectionIndicator?.WidthRequest);

				tabBar.ItemsSource = null;
				Assert.Null(tabBar.ItemsSource);
			}
		}

		[Fact]
		public void TestUpdateTabIndicatorWidthOnWidthRequest()
		{
			SfTabItem view = new SfTabItem() { Header = "Tab 1", HeaderHorizontalTextAlignment = TextAlignment.Start };
			tabBar.Items.Add(view);
			InvokePrivateMethod(tabBar, "UpdateCurrentIndicatorWidth", view);
			object? tabX = GetPrivateField(tabBar, "_selectedTabX");
			double actualTabX = tabX is double width ? width : 0.0;
			Assert.Equal(-0.5, actualTabX);
		}

		[Fact]
		public void TestUpdateTabIndicatorWidthOnWidthRequestWithLargeValue()
		{
			tabBar.Items.Add(new SfTabItem() { Header = "Tab 1" });
			InvokePrivateMethod(tabBar, "UpdateCurrentIndicatorWidth", tabBar.Items[0]);
			object? tabX = GetPrivateField(tabBar, "_selectedTabX");
			double actualTabX = tabX is double width ? width : 0.0;
			Assert.Equal(-0.5, actualTabX);
		}

		#endregion

		#region TabView Internal Methods

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestSelectionChangingEventInvoked(bool shouldRaise)
		{
			var tabView = new SfTabView();
			bool eventRaised = false;
			tabView.SelectionChanging += (sender, args) =>
			{
				eventRaised = true;
			};
			var args = new SelectionChangingEventArgs();
			if (shouldRaise)
			{
				tabView.RaiseSelectionChangingEvent(args);
			}
			Assert.Equal(shouldRaise, eventRaised);
		}

		[Theory]
		[InlineData(false, true)]
		[InlineData(true, false)]
		public void TestSelectionChangedEventTriggeredCorrectly(bool handled, bool expectedEventCalled)
		{
			var tabView = new SfTabView();
			bool eventCalled = false;
			tabView.SelectionChanged += (sender, args) =>
			{
				eventCalled = true;
			};
			tabView.RaiseSelectionChangedEvent(new TabSelectionChangedEventArgs { Handled = handled });
			Assert.Equal(expectedEventCalled, eventCalled);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestTabItemTappedEvent(bool shouldRaise)
		{
			var tabView = new SfTabView();
			bool eventRaised = false;

			tabView.TabItemTapped += (sender, args) =>
			{
				eventRaised = true;
			};
			var args = new TabItemTappedEventArgs();
			if (shouldRaise)
			{
				tabView.RaiseTabItemTappedEvent(args);
			}
			Assert.Equal(shouldRaise, eventRaised);
		}

		#endregion

		#region TabView Private Methods

		[Fact]
		public void TestHeaderContainerRaisesSelectionChangingEvent()
		{
			var tabView = new SfTabView();
			var wasEventRaised = false;
			tabView.SelectionChanging += (sender, e) =>
			{
				wasEventRaised = true;
			};
			var eventArgs = new SelectionChangingEventArgs();
			var methodInfo = typeof(SfTabView).GetMethod("HeaderContainer_SelectionChanging", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(tabView, new object?[] { null, eventArgs });
			Assert.True(wasEventRaised, "SelectionChanging event was not raised.");
		}

		[Fact]
		public void TestSelectionChangingEventRaises()
		{
			var tabView = new SfTabView();
			var wasEventRaised = false;
			tabView.SelectionChanging += (sender, e) =>
			{
				wasEventRaised = true;
			};
			var eventArgs = new SelectionChangingEventArgs();
			var methodInfo = typeof(SfTabView).GetMethod("TabContentContainer_SelectionChanging", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(tabView, new object?[] { null, eventArgs });
			Assert.True(wasEventRaised, "SelectionChanging event was not raised.");
		}

		[Fact]
		public void TestTabItemTappedEventRaises()
		{
			var tabView = new SfTabView();
			var wasEventRaised = false;
			tabView.TabItemTapped += (sender, e) =>
			{
				wasEventRaised = true;
			};
			var eventArgs = new TabItemTappedEventArgs();
			var methodInfo = typeof(SfTabView).GetMethod("TabHeaderContainer_TabItemTapped", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(tabView, new object?[] { null, eventArgs });
			Assert.True(wasEventRaised, "TabItemTapped event was not raised.");
		}

		[Theory]
		[InlineData(0, 1)]
		[InlineData(1, 2)]
		[InlineData(2, 0)]
		public void TestHeaderContainerSelectionChangedIndexIsDifferent(int oldIndex, int newIndex)
		{
			var tabView = new SfTabView
			{
				SelectedIndex = oldIndex
			};
			TabSelectionChangedEventArgs? eventArgsReceived = null;
			tabView.SelectionChanged += (sender, e) => eventArgsReceived = e;
			var eventArgs = new TabSelectionChangedEventArgs
			{
				OldIndex = oldIndex,
				NewIndex = newIndex
			};
			var methodInfo = typeof(SfTabView).GetMethod("HeaderContainer_SelectionChanged", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(tabView, new object?[] { null, eventArgs });
			Assert.NotNull(eventArgsReceived);
			Assert.Equal(newIndex, eventArgsReceived.NewIndex);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(3)]
		public void TestHeaderContainerSelectionChangedIndexIsSame(int index)
		{
			var tabView = new SfTabView
			{
				SelectedIndex = index
			};
			bool eventRaised = false;
			tabView.SelectionChanged += (sender, e) => eventRaised = true;
			var eventArgs = new TabSelectionChangedEventArgs
			{
				OldIndex = index,
				NewIndex = index
			};
			var methodInfo = typeof(SfTabView).GetMethod("HeaderContainer_SelectionChanged", BindingFlags.NonPublic | BindingFlags.Instance);
			methodInfo?.Invoke(tabView, new object?[] { null, eventArgs });
			Assert.False(eventRaised, "SelectionChanged event should not have been raised when the new index is the same as the old index.");
		}

		[Fact]
		public void TestOnContentTransitionDurationChanged()
		{
			var tabView = new SfTabView();
			var methodInvoked = false;
			var methodInfo = typeof(SfTabView).GetMethod("UpdateContentTransitionDuration", BindingFlags.NonPublic | BindingFlags.Instance);
			if (methodInfo != null)
			{
				tabView.PropertyChanged += (sender, args) =>
				{
					if (args.PropertyName == nameof(tabView.ContentTransitionDuration))
					{
						methodInfo.Invoke(tabView, null);
						methodInvoked = true;
					}
				};
				tabView.ContentTransitionDuration = 500;
				Assert.True(methodInvoked, "UpdateContentTransitionDuration was not invoked when the content transition duration changed.");
			}
			else
			{
				Assert.True(false);
			}
		}

		#endregion

		#region TabItemTappedEventArgs Property

		[Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestTabItemTappedCancelProperty(bool cancelValue)
        {
            var eventArgs = new TabItemTappedEventArgs();
            eventArgs.Cancel = cancelValue;
            Assert.Equal(cancelValue, eventArgs.Cancel);
        }

		[Theory]
		[InlineData("Tab1")]
		[InlineData("Tab2")]
		public void TestTabItem(string tabItemName)
		{
			var tabItemTappedEventArgs = new TabItemTappedEventArgs();
			var expectedTabItem = new SfTabItem { Header = tabItemName };
			tabItemTappedEventArgs.TabItem = expectedTabItem;
			Assert.NotNull(tabItemTappedEventArgs.TabItem);
			Assert.Equal(expectedTabItem, tabItemTappedEventArgs.TabItem);
		}

		#endregion

		#region TabView Internal Properties

		[Theory]
        [InlineData(0, 0, 1, 1)]
        [InlineData(0.5, 0, 0.5, 0.5)]
        [InlineData(0.5, 0, 0.5, 0)]
        public void TestSetScrollButtonBackground(float r, float g, float b, float a)
        {
            var tabView = new SfTabView();
            var expectedColor = new Color(a, r, g, b);
            tabView.ScrollButtonBackground = expectedColor;
            Assert.Equal(expectedColor, tabView.ScrollButtonBackground);
        }

        [Theory]
        [InlineData(0, 0.5, 1, 1)]
        [InlineData(1, 1, 0, 0)]
        [InlineData(0.5, 1, 0, 0.5)]
        public void TestSetScrollButtonIconColor(float r, float g, float b, float a)
        {
            var tabView = new SfTabView();
            var expectedColor = new Color(a, r, g, b);
            tabView.ScrollButtonIconColor = expectedColor;
            Assert.Equal(expectedColor, tabView.ScrollButtonIconColor);

            SfTabBar? tabHeaderContainer = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
            Assert.Equal(expectedColor, tabHeaderContainer?.ScrollButtonIconColor);
        }

		[Fact]
		public void TestContentTransitionEnabled()
		{
			var instance = new SfTabView();
			var propertyInfo = instance.GetType().GetProperty("IsContentTransitionEnabled",
				BindingFlags.NonPublic | BindingFlags.Instance);
			propertyInfo?.SetValue(instance, true);
			Assert.NotNull(propertyInfo);
			var res = propertyInfo.GetValue(instance);
			Assert.NotNull(res);
			bool result = (bool)res;
			Assert.True(result);
		}

		[Fact]
		public void TestContentTransitionEnabledDefaultValue()
		{
			var instance = new SfTabView();
			var propertyInfo = instance.GetType().GetProperty("IsContentTransitionEnabled",
				BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(propertyInfo);
			var res = propertyInfo.GetValue(instance);
			Assert.NotNull(res);
			bool result = (bool)res;
			Assert.True(result);
		}

		[Fact]
		public void TestContentLoopingEnabledDefaultValue()
		{
			var instance = new SfTabView();
			var propertyInfo = instance.GetType().GetProperty("IsContentLoopingEnabled",
				BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(propertyInfo);
			var res = propertyInfo.GetValue(instance);
			Assert.NotNull(res);
			bool result = (bool)res;
			Assert.False(result);
		}

		[Fact]
		public void TestContentLoopingEnabled()
		{
			var instance = new SfTabView();
			var propertyInfo = instance.GetType().GetProperty("IsContentLoopingEnabled",
				BindingFlags.NonPublic | BindingFlags.Instance);
			propertyInfo?.SetValue(instance, true);
			Assert.NotNull(propertyInfo);
			var res = propertyInfo.GetValue(instance);
			Assert.NotNull(res);
			bool result = (bool)res;
			Assert.True(result);
		}

		#endregion

		#region TabViewMaterialVisualStyle Private Methods

		[Fact]
        public void TestUpdateHorizontalOptionsSetsLayoutOptionsToStart()
        {
            var tabViewMaterialVisualStyle = new TabViewMaterialVisualStyle();
            var tabItem = new SfTabItem { HeaderHorizontalTextAlignment = TextAlignment.Start };
            var horizontalLayoutField = tabViewMaterialVisualStyle.GetType().GetField("_horizontalLayout", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(horizontalLayoutField);
            var horizontalLayoutType = horizontalLayoutField.FieldType;
            var horizontalLayoutInstance = Activator.CreateInstance(horizontalLayoutType);
            horizontalLayoutField.SetValue(tabViewMaterialVisualStyle, horizontalLayoutInstance);
            var updateHorizontalOptionsMethod = tabViewMaterialVisualStyle.GetType().GetMethod("UpdateHorizontalOptions", BindingFlags.NonPublic | BindingFlags.Instance);
            updateHorizontalOptionsMethod?.Invoke(tabViewMaterialVisualStyle, new object[] { tabItem });
            var horizontalOptionsProperty = horizontalLayoutType.GetProperty("HorizontalOptions");
            Assert.Equal(LayoutOptions.Start, horizontalOptionsProperty?.GetValue(horizontalLayoutInstance));
        }

        [Fact]
        public void TestUpdateHorizontalOptionsSetsLayoutOptionsToEnd()
        {
            var tabViewMaterialVisualStyle = new TabViewMaterialVisualStyle();
            var tabItem = new SfTabItem { HeaderHorizontalTextAlignment = TextAlignment.End };
            var horizontalLayoutField = tabViewMaterialVisualStyle.GetType().GetField("_horizontalLayout", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(horizontalLayoutField);
            var horizontalLayoutType = horizontalLayoutField.FieldType;
            var horizontalLayoutInstance = Activator.CreateInstance(horizontalLayoutType);
            horizontalLayoutField.SetValue(tabViewMaterialVisualStyle, horizontalLayoutInstance);
            var updateHorizontalOptionsMethod = tabViewMaterialVisualStyle.GetType().GetMethod("UpdateHorizontalOptions", BindingFlags.NonPublic | BindingFlags.Instance);
            updateHorizontalOptionsMethod?.Invoke(tabViewMaterialVisualStyle, new object[] { tabItem });
            var horizontalOptionsProperty = horizontalLayoutType.GetProperty("HorizontalOptions");
            Assert.Equal(LayoutOptions.End, horizontalOptionsProperty?.GetValue(horizontalLayoutInstance));
        }

        [Fact]
        public void TestUpdateHorizontalOptionsSetsLayoutOptionsToCenter()
        {
            var tabViewMaterialVisualStyle = new TabViewMaterialVisualStyle();
            var tabItem = new SfTabItem { HeaderHorizontalTextAlignment = TextAlignment.Center };
            var horizontalLayoutField = tabViewMaterialVisualStyle.GetType().GetField("_horizontalLayout", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(horizontalLayoutField);
            var horizontalLayoutType = horizontalLayoutField.FieldType;
            var horizontalLayoutInstance = Activator.CreateInstance(horizontalLayoutType);
            horizontalLayoutField.SetValue(tabViewMaterialVisualStyle, horizontalLayoutInstance);
            var updateHorizontalOptionsMethod = tabViewMaterialVisualStyle.GetType().GetMethod("UpdateHorizontalOptions", BindingFlags.NonPublic | BindingFlags.Instance);
            updateHorizontalOptionsMethod?.Invoke(tabViewMaterialVisualStyle, new object[] { tabItem });
            var horizontalOptionsProperty = horizontalLayoutType.GetProperty("HorizontalOptions");
            Assert.Equal(LayoutOptions.Center, horizontalOptionsProperty?.GetValue(horizontalLayoutInstance));
        }

		[Fact]
		public void TestUpdateHeaderHorizontalOptionsAlignmentIsStart()
		{
			var tabView = new TabViewMaterialVisualStyle();
			var tabItem = new SfTabItem { HeaderHorizontalTextAlignment = TextAlignment.Start };
			var headerField = tabView.GetType().GetField("_header", BindingFlags.NonPublic | BindingFlags.Instance);
			var sfLabelType = tabView.GetType().Assembly.GetType("Syncfusion.Maui.Toolkit.Helper.SfLabel");
			Assert.NotNull(sfLabelType);
			var headerInstance = Activator.CreateInstance(sfLabelType);
			headerField?.SetValue(tabView, headerInstance);
			var updateMethod = tabView.GetType().GetMethod("UpdateHeaderHorizontalOptions", BindingFlags.NonPublic | BindingFlags.Instance);
			updateMethod?.Invoke(tabView, new object[] { tabItem });
			var horizontalOptionsProperty = sfLabelType.GetProperty("HorizontalOptions");
			Assert.NotNull(horizontalOptionsProperty);
			var actualHorizontalOptions = horizontalOptionsProperty.GetValue(headerInstance);
			Assert.Equal(LayoutOptions.Start, actualHorizontalOptions);
		}

		[Fact]
		public void TestUpdateHeaderHorizontalOptionsAlignmentIsCenter()
		{
			var tabView = new TabViewMaterialVisualStyle();
			var tabItem = new SfTabItem { HeaderHorizontalTextAlignment = TextAlignment.Center };
			var headerField = tabView.GetType().GetField("_header", BindingFlags.NonPublic | BindingFlags.Instance);
			var sfLabelType = tabView.GetType().Assembly.GetType("Syncfusion.Maui.Toolkit.Helper.SfLabel");
			Assert.NotNull(sfLabelType);
			var headerInstance = Activator.CreateInstance(sfLabelType);
			headerField?.SetValue(tabView, headerInstance);
			var updateMethod = tabView.GetType().GetMethod("UpdateHeaderHorizontalOptions", BindingFlags.NonPublic | BindingFlags.Instance);
			updateMethod?.Invoke(tabView, new object[] { tabItem });
			var horizontalOptionsProperty = sfLabelType.GetProperty("HorizontalOptions");
			Assert.NotNull(horizontalOptionsProperty);
			var actualHorizontalOptions = horizontalOptionsProperty.GetValue(headerInstance);
			Assert.Equal(LayoutOptions.Center, actualHorizontalOptions);
		}

		[Fact]
		public void TestUpdateHeaderHorizontalOptionsAlignmentIsEnd()
		{
			var tabView = new TabViewMaterialVisualStyle();
			var tabItem = new SfTabItem { HeaderHorizontalTextAlignment = TextAlignment.End };
			var headerField = tabView.GetType().GetField("_header", BindingFlags.NonPublic | BindingFlags.Instance);
			var sfLabelType = tabView.GetType().Assembly.GetType("Syncfusion.Maui.Toolkit.Helper.SfLabel");
			Assert.NotNull(sfLabelType);
			var headerInstance = Activator.CreateInstance(sfLabelType);
			headerField?.SetValue(tabView, headerInstance);
			var updateMethod = tabView.GetType().GetMethod("UpdateHeaderHorizontalOptions", BindingFlags.NonPublic | BindingFlags.Instance);
			updateMethod?.Invoke(tabView, new object[] { tabItem });
			var horizontalOptionsProperty = sfLabelType.GetProperty("HorizontalOptions");
			Assert.NotNull(horizontalOptionsProperty);
			var actualHorizontalOptions = horizontalOptionsProperty.GetValue(headerInstance);
			Assert.Equal(LayoutOptions.End, actualHorizontalOptions);
		}

		[Fact]
		public void TestHeaderDisplayModePropertyChanged()
		{
			var tabViewMaterialVisualStyle = new TabViewMaterialVisualStyle();
			Assert.NotNull(tabViewMaterialVisualStyle);
			var onHeaderDisplayModePropertyChangedMethod = typeof(TabViewMaterialVisualStyle)
				.GetMethod("OnHeaderDisplayModePropertyChanged", BindingFlags.NonPublic | BindingFlags.Static);

			var updateHeaderDisplayModeMethod = typeof(TabViewMaterialVisualStyle)
				.GetMethod("UpdateHeaderDisplayMode", BindingFlags.NonPublic | BindingFlags.Instance);
			onHeaderDisplayModePropertyChangedMethod?.Invoke(null, new object?[] { tabViewMaterialVisualStyle, null, null });
			Assert.NotNull(updateHeaderDisplayModeMethod);
			var exception = Record.Exception(() => updateHeaderDisplayModeMethod.Invoke(tabViewMaterialVisualStyle, null));
			Assert.Null(exception);
		}

		#endregion

		#region TabViewMaterialVisualStyle Property

		[Theory]
        [InlineData(TabBarDisplayMode.Default)]
        [InlineData(TabBarDisplayMode.Image)]
        [InlineData(TabBarDisplayMode.Text)]
        public void TestHeaderDisplayModeSetter(TabBarDisplayMode expectedValue)
        {
            var tabView = new TabViewMaterialVisualStyle();
            tabView.HeaderDisplayMode = expectedValue;
            var actualValue = (TabBarDisplayMode)tabView.GetValue(TabViewMaterialVisualStyle.HeaderDisplayModeProperty); // Correct usage
            Assert.Equal(expectedValue, actualValue);
        }

		#endregion

		#region TabItem Properties

		[Fact]
        public void IsEnablePropertyCheck()
        {
            SfTabItem item = new SfTabItem();
            bool expectedValue = false;
            item.IsEnabled = false;
            Assert.Equal(expectedValue, item.IsEnabled);
        }

        [Fact]
        public void FontGetValueCheck()
        {
            SfTabItem sfTabItem = new SfTabItem();
            var defaultFont = sfTabItem.Font;
            Assert.Equal(Microsoft.Maui.Font.Default, defaultFont);
        }

        [Fact]
        public void TestHeaderHorizontalTextAlignment()
        {
            SfTabItem item = new SfTabItem();
            var newAlignment = TextAlignment.Center;
            item.HeaderHorizontalTextAlignment = newAlignment;
            var actualAlignment = item.HeaderHorizontalTextAlignment;
            Assert.Equal(newAlignment, actualAlignment);
        }

		[Theory]
		[InlineData(false)]
		[InlineData(true)]
		public void TestIsFontAutoScalingEnabledPropertyCheck(bool value)
		{
			SfTabItem item = new SfTabItem();
			item.FontAutoScalingEnabled = value;
			Assert.Equal(value, item.FontAutoScalingEnabled);
		}

		[Fact]
        public void TestTabWidthMode()
        {
            SfTabItem item = new SfTabItem();
            var newMode = TabWidthMode.SizeToContent;
            item.TabWidthMode = newMode;
            Assert.Equal(newMode, item.TabWidthMode);
        }

        [Fact]
        public void TestHeaderDisplayModeCheck()
        {
            SfTabItem item = new SfTabItem();
            var newMode = TabBarDisplayMode.Text;
            item.HeaderDisplayMode = newMode;
            Assert.Equal(newMode, item.HeaderDisplayMode);
        }

        [Theory]
        [InlineData(10.5)]
        [InlineData(double.NaN)]
        [InlineData(0)]
        [InlineData(-10.5)]
        public void ImageTextSpacingSetValueUpdatedValue(double value)
        {
            SfTabItem item = new SfTabItem();
            item.ImageTextSpacing = value;
            Assert.Equal(value, item.ImageTextSpacing);
        }

        [Fact]
        public void TestImagePosition()
        {
            SfTabItem item = new SfTabItem();
            var newPosition = TabImagePosition.Right;
            item.ImagePosition = newPosition;
            var actualPosition = item.ImagePosition;
            Assert.Equal(newPosition, actualPosition);
        }

        [Fact]
        public void TestTextColor()
        {
            SfTabItem item = new SfTabItem();
            var newColor = Colors.Red;
            item.TextColor = newColor;
            var actualColor = item.TextColor;
            Assert.Equal(newColor, actualColor);
        }

        [Theory]
        [InlineData(10.5)]
        [InlineData(double.NaN)]
        [InlineData(0)]
        [InlineData(-10.5)]
        public void TestFontSize(double value)
        {
            SfTabItem sfTabItem = new SfTabItem();
            sfTabItem.FontSize = value;
            Assert.Equal(value, sfTabItem.FontSize);
        }

        [Theory]
        [InlineData(FontAttributes.Bold)]
        [InlineData(FontAttributes.Italic)]
        [InlineData(FontAttributes.None)]
        public void TestFontAttributes(FontAttributes font)
        {
            SfTabItem sfTabItem = new SfTabItem();
            sfTabItem.FontAttributes = font;
            Assert.Equal(font, sfTabItem.FontAttributes);
        }

		[Fact]
		public void TestFontFamily()
		{
			SfTabItem item = new SfTabItem();
			var expectedFontFamily = item.FontFamily;
			Assert.Equal(expectedFontFamily, item.FontFamily);
		}

		[Fact]
		public void TestHeaderProperty()
		{
			SfTabItem _tabItem = new SfTabItem();
			_tabItem.Header = "New Header";
			Assert.Equal("New Header", _tabItem.Header);
		}

		[Fact]
		public void TestFontFamilyProperty()
		{
			SfTabItem _tabItem = new SfTabItem();
			Assert.Null(_tabItem.FontFamily);
		}

		[Theory]
		[InlineData("OpenSansRegular")]
		[InlineData("Arial")]
		[InlineData("OpenSansSemibold")]
		[InlineData("Serif")]
		public void TestFontFamilyPropertyValueChange(string fontFamily)
		{
			SfTabItem _tabItem = new SfTabItem();
			_tabItem.FontFamily = fontFamily;
			Assert.Equal(fontFamily, _tabItem.FontFamily);
		}

		[Fact]
		public void TestFontSizePropertyDefault()
		{
			SfTabItem _tabItem = new SfTabItem();
			Assert.Equal(14.0, _tabItem.FontSize);
		}

		[Fact]
		public void TestFontSizePropertySetValue()
		{
			SfTabItem _tabItem = new SfTabItem();
			_tabItem.FontSize = 20.0;
			Assert.Equal(20.0, _tabItem.FontSize);
		}

		[Fact]
		public void TextColorPropertyDefaultValue()
		{
			SfTabItem _tabItem = new SfTabItem();
			Assert.Equal(Color.FromArgb("#49454F"), _tabItem.TextColor);
		}

		[Fact]
		public void TestTextColorProperty()
		{
			SfTabItem _tabItem = new SfTabItem();
			var color = Color.FromArgb("#FF0000");
			_tabItem.TextColor = color;
			Assert.Equal(color, _tabItem.TextColor);
		}

		[Fact]
		public void TestImageSourcePropertyDefaultValue()
		{
			SfTabItem _tabItem = new SfTabItem();
			Assert.Null(_tabItem.ImageSource);
		}

		[Fact]
		public void TestImageSourceProperty()
		{
			SfTabItem _tabItem = new SfTabItem();
			var imageSource = new FileImageSource { File = "icon.png" };
			_tabItem.ImageSource = imageSource;
			Assert.Equal(imageSource, _tabItem.ImageSource);
		}

		[Fact]
		public void TestImagePositionProperty()
		{
			SfTabItem _tabItem = new SfTabItem();
			Assert.Equal(TabImagePosition.Top, _tabItem.ImagePosition);
		}

		[Fact]
		public void TestImagePositionPropertySetValue()
		{
			SfTabItem _tabItem = new SfTabItem();
			_tabItem.ImagePosition = TabImagePosition.Left;
			Assert.Equal(TabImagePosition.Left, _tabItem.ImagePosition);
		}

		[Fact]
		public void TestImageTextSpacingPropertyDefaultValue()
		{
			SfTabItem _tabItem = new SfTabItem();
			Assert.Equal(10.0, _tabItem.ImageTextSpacing);
		}

		[Theory]
		[InlineData(50.0)]
		[InlineData(-10)]
		[InlineData(-20)]
		[InlineData(100)]
		[InlineData(-1001)]
		[InlineData(0)]
		public void TestImageTextSpacingPropertySetValue(double value)
		{
			SfTabItem _tabItem = new SfTabItem();
			_tabItem.ImageTextSpacing = value;
			Assert.Equal(value, _tabItem.ImageTextSpacing);
		}

		[Fact]
		public void TestIsSelectedPropertyDefaultValue()
		{
			SfTabItem _tabItem = new SfTabItem();
			Assert.False(_tabItem.IsSelected);
		}

		#endregion

		#region SelectionChangingEventArgs property

		[Theory]
        [InlineData(3)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-6)]
        public void TestSelectionChangingEventCheck(int index)
        {
            var eventArgs = new SelectionChangingEventArgs();
            if (eventArgs != null)
                eventArgs.Index = index;
            Assert.NotNull(eventArgs);
            Assert.Equal(index, eventArgs.Index);
        }

		[Fact]
		public void TestSelectionChangingEventPropertyCheck()
		{
			SelectionChangingEventArgs _selectionChangingEventArgs = new SelectionChangingEventArgs();
			Assert.Equal(0, _selectionChangingEventArgs.Index);
			Assert.False(_selectionChangingEventArgs.Cancel);
		}

		#endregion

		#region TabItem Public Methods

		[Fact]
        public void TestOnTouchEnteredAction()
        {
            SfTabItem item = new SfTabItem();
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Entered, new Point(50, 50));
            item.OnTouch(touchEventArgs);
            PointerActions pointerActions = PointerActions.Entered;
            Assert.Equal(touchEventArgs.Action, pointerActions);
        }

        [Fact]
        public void TestOnTouchPressedAction()
        {
            SfTabItem item = new SfTabItem();
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Pressed, new Point(50, 50));
            item.OnTouch(touchEventArgs);
            PointerActions pointerActions = PointerActions.Pressed;
            Assert.Equal(touchEventArgs.Action, pointerActions);
        }

        [Fact]
        public void TestOnTouchExitedAction()
        {
            SfTabItem item = new SfTabItem();
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Exited, new Point(50, 50));
            item.OnTouch(touchEventArgs);
            PointerActions pointerActions = PointerActions.Exited;
            Assert.Equal(touchEventArgs.Action, pointerActions);
        }

        [Fact]
        public void CheckTestOnTouchReleased()
        {
            SfTabItem item = new SfTabItem();
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Released, new Point(50, 50));
            item.OnTouch(touchEventArgs);
            PointerActions pointerActions = PointerActions.Released;
            Assert.Equal(touchEventArgs.Action, pointerActions);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestOnTouchReleased(bool value)
        {
            SfTabItem item = new SfTabItem();
            item.IsSelected = value;
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Released, new Point(50, 50));
            item.OnTouch(touchEventArgs);
            Assert.Equal(touchEventArgs.TouchPoint, new Point(50, 50));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void OnTouchReleasedActionCheck(bool value)
        {
            SfTabItem item = new SfTabItem();
            item.IsSelected = value;
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Exited, new Point(50, 50));
            item.OnTouch(touchEventArgs);
            Assert.Equal(touchEventArgs.TouchPoint, new Point(50, 50));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestOnTouchReleasedActionCheck(bool value)
        {
            SfTabItem item = new SfTabItem();
            item.IsSelected = value;
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Pressed, new Point(50, 50));
            item.OnTouch(touchEventArgs);
            Assert.Equal(touchEventArgs.TouchPoint, new Point(50, 50));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestOnTouchReleasedAction(bool value)
        {
            SfTabItem item = new SfTabItem();
            item.IsSelected = value;
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Entered, new Point(50, 50));
            item.OnTouch(touchEventArgs);
            Assert.Equal(touchEventArgs.TouchPoint, new Point(50, 50));
        }

        [Fact]
        public void TestOnTouchEnteredActionForNegativePointValues()
        {
            SfTabItem item = new SfTabItem();
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Entered, new Point(-50, 50));
            item.OnTouch(touchEventArgs);
            PointerActions pointerActions = PointerActions.Entered;
            Assert.Equal(touchEventArgs.Action, pointerActions);
        }

        [Fact]
        public void TestOnTouchPressedActionForNegativePointValues()
        {
            SfTabItem item = new SfTabItem();
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Pressed, new Point(-50, -50));
            item.OnTouch(touchEventArgs);
            PointerActions pointerActions = PointerActions.Pressed;
            Assert.Equal(touchEventArgs.Action, pointerActions);
        }

        [Fact]
        public void TestOnTouchExitedActionForNegativePointValues()
        {
            SfTabItem item = new SfTabItem();
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Exited, new Point(-50, -50));
            item.OnTouch(touchEventArgs);
            PointerActions pointerActions = PointerActions.Exited;
            Assert.Equal(touchEventArgs.Action, pointerActions);
        }

        [Fact]
        public void CheckTestOnTouchReleasedForNegativePointValues()
        {
            SfTabItem item = new SfTabItem();
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Released, new Point(-50, 50));
            item.OnTouch(touchEventArgs);
            PointerActions pointerActions = PointerActions.Released;
            Assert.Equal(touchEventArgs.Action, pointerActions);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestOnTouchReleasedForNegativePointValues(bool value)
        {
            SfTabItem item = new SfTabItem();
            item.IsSelected = value;
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Released, new Point(-50, -50));
            item.OnTouch(touchEventArgs);
            Assert.Equal(touchEventArgs.TouchPoint, new Point(-50, -50));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckOnTouchReleasedActionCheckForNegativePointValues(bool value)
        {
            SfTabItem item = new SfTabItem();
            item.IsSelected = value;
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Exited, new Point(-50, 50));
            item.OnTouch(touchEventArgs);
            Assert.Equal(touchEventArgs.TouchPoint, new Point(-50, 50));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestOnTouchReleasedActionCheckForNegativePointValues(bool value)
        {
            SfTabItem item = new SfTabItem();
            item.IsSelected = value;
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Pressed, new Point(-50, -500));
            item.OnTouch(touchEventArgs);
            Assert.Equal(touchEventArgs.TouchPoint, new Point(-50, -500));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void TestOnTouchReleasedActionForNegativePointValues(bool value)
        {
            SfTabItem item = new SfTabItem();
            item.IsSelected = value;
            var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Entered, new Point(-500, 50));
            item.OnTouch(touchEventArgs);
            Assert.Equal(touchEventArgs.TouchPoint, new Point(-500, 50));
        }

		#endregion

		#region HorizontalContent properties

		[Fact]
        public void TestItemsProperty()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            Assert.NotNull(horizontal.Items);
        }

        [Fact]
        public void TestItemsPropertyCheck()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            var items = new TabItemCollection()
            {
                new SfTabItem { Header = "Item 1", Content = new Grid { BackgroundColor = Colors.Red } },
                new SfTabItem { Header = "Item 2", Content = new Grid { BackgroundColor = Colors.Green } }
            };

            horizontal.Items = items;
            Assert.Equal(items, horizontal.Items);
        }

        [Fact]
        public void TestSelectedIndexProperty()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            Assert.Equal(-1, horizontal.SelectedIndex);
        }

        [Fact]
        public void TestSelectedIndexPropertySetValue()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            horizontal.SelectedIndex = 1;
            Assert.Equal(1, horizontal.SelectedIndex);
        }

        [Fact]
        public void ContentTransitionDurationPropertyDefaultValueCheck()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            Assert.Equal(100d, horizontal.ContentTransitionDuration);
        }

        [Fact]
        public void TestContentTransitionDurationPropertySetValue()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            horizontal.ContentTransitionDuration = 200d;
            Assert.Equal(200d, horizontal.ContentTransitionDuration);
        }

        [Fact]
        public void TestContentWidthProperty()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            horizontal.ContentWidth = 500d;
            Assert.Equal(500d, horizontal.ContentWidth);
        }

		[Fact]
		public void TestIsEnableProperty()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.IsEnabled = true;
			Assert.True(horizontal.IsEnabled);
		}

		[Fact]
		public void HorizontalContentContentItemTemplateCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var newTemplate = new DataTemplate(() => new Label { Text = "Test" });
			horizontal.ContentItemTemplate = newTemplate;
			var actualTemplate = horizontal.ContentItemTemplate;
			Assert.Equal(newTemplate, actualTemplate);
		}

		[Theory]
		[InlineData(100.0)]
		[InlineData(-100)]
		[InlineData(-1000)]
		[InlineData(-228.0)]
		[InlineData(233)]
		[InlineData(0)]
		public void TestHorizontalContentTransition(double value)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.ContentTransitionDuration = value;
			Assert.Equal(value, horizontal.ContentTransitionDuration);
		}

		[Fact]
		public void TestHorizontalContentNullCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			Assert.NotNull(horizontal);
		}

		[Theory]
		[InlineData(100.0)]
		[InlineData(-100)]
		[InlineData(-1000)]
		[InlineData(-228.0)]
		[InlineData(233)]
		[InlineData(0)]
		public void TestHorizontalContentContentTransitionDuration(double value)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.ContentWidth = value;
			Assert.Equal(value, horizontal.ContentWidth);

		}

		#endregion

		#region SfHorizontalContent Fields

		[Fact]
        public void TestIsTowardsRightPropertySetValue()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            SetPrivateField(horizontal, "_isTowardsRight", true);
            var isTowardsRight = (bool?)GetPrivateField(horizontal, "_isTowardsRight");
            Assert.True(isTowardsRight);
        }

		[Fact]
		public void TestsTowardsRightField()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_isTowardsRight", true);
			var value = GetPrivateField(horizontal, "_isTowardsRight");
			Assert.NotNull(value);
			bool isTowardRight = (bool)value;
			Assert.True(isTowardRight);
		}

		[Fact]
		public void TestHandlerTouchReleasedMethod()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_isPressed", true);
			SetPrivateField(horizontal, "_isMoved", true);
			SetPrivateField(horizontal, "_visibleItemCount", 4);
			SfHorizontalStackLayout views1 = new SfHorizontalStackLayout();
			views1.Children.Add(new SfTabItem { Header = "Tab 1" });
			views1.Children.Add(new SfTabItem { Header = "Tab 2" });
			views1.Children.Add(new SfTabItem { Header = "Tab 3" });
			views1.Children.Add(new SfTabItem { Header = "Tab 4" });
			SetPrivateField(horizontal, "_horizontalStackLayout", views1);
			var stack = GetPrivateField(horizontal, "_horizontalStackLayout");
			Assert.NotNull(stack);
			SfHorizontalStackLayout layout = (SfHorizontalStackLayout)stack;
			var visible = GetPrivateField(horizontal, "_visibleItemCount");
			Assert.NotNull(visible);
			int isVisible = (int)visible;
			var press = GetPrivateField(horizontal, "_isPressed");
			Assert.NotNull(press);
			bool isPress = (bool)press;
			var moved = GetPrivateField(horizontal, "_isMoved");
			Assert.NotNull(moved);
			bool isMoved = (bool)moved;
			Assert.True(layout.Children.Count > 1);
			Assert.Equal(4, layout.Children.Count);
			Assert.True(isPress);
			Assert.True(isMoved);
			Assert.Equal(4, isVisible);
		}

		[Fact]
		public void TestUpdateOnTabItemContentChanged()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.Items = PopulateLabelItemsCollection();
			Assert.Equal(0, horizontal.SelectedIndex);
			Assert.True(horizontal.Items[1].Content is Label);

			horizontal.Items[1].Content = new Button() { Text = "Modified content" };
			SfHorizontalStackLayout? horizontalStackLayout = GetPrivateField<SfHorizontalContent>(horizontal, "_horizontalStackLayout") as SfHorizontalStackLayout;

			if (horizontalStackLayout?.Children[1] is SfGrid parentGrid && parentGrid != null)
			{
				Assert.Contains(horizontal.Items[1].Content, parentGrid.Children);
			}
		}

		#endregion

		#region HorizontalContent Private Methods

		[Fact]
		public void GetVisibleItemsCountMethodCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var visibleItemsCount = InvokePrivateMethod(horizontal, "GetVisibleItemsCount");
			Assert.NotNull(visibleItemsCount);
			Assert.Equal(0, visibleItemsCount);

			horizontal.ItemsSource = PopulateMixedDataItemsSource();
			visibleItemsCount = InvokePrivateMethod(horizontal, "GetVisibleItemsCount");

			Assert.Equal(0, visibleItemsCount);
		}

		[Fact]
		public void ClearItemsMethodCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.Items = PopulateLabelItemsCollection();
			var clearItems = InvokePrivateMethod(horizontal, "ClearItems");
			Assert.Null(clearItems);
		}

		[Fact]
		public void GetCountVisibleItemsMethodCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var visibleItemsCount = InvokePrivateMethod(horizontal, "GetCountVisibleItems");
			Assert.NotNull(visibleItemsCount);

			horizontal.Items = PopulateLabelItemsCollection();
			visibleItemsCount = InvokePrivateMethod(horizontal, "GetCountVisibleItems");
			Assert.Equal(3, visibleItemsCount);
		}

		[Fact]
		public void HandlerTouchReleasedCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var handleTouchReleased = InvokePrivateMethod(horizontal, "HandleTouchReleased");
			Assert.Null(handleTouchReleased);
		}

		[Fact]
        public void GetNextVisibleItemIndexMethodCheck()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            horizontal.Items = PopulateMixedItemsCollection();
            horizontal.SelectedIndex = 1;
            SetPrivateField(horizontal, "_isTowardsRight", true);
            var nextIndex = InvokePrivateMethod(horizontal, "GetNextVisibleItemIndex");
            Assert.NotNull(nextIndex);
            double index = (int)nextIndex;
            Assert.Equal(3, horizontal.Items.Count);
            Assert.NotNull(nextIndex);
            Assert.Equal(2, index);
        }

		[Theory]
		[InlineData(0)]
		[InlineData(3)]
		[InlineData(-2)]
		[InlineData(1)]
		public void GetNextItemIndexCheck(int value)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var tabItem1 = new SfTabItem { Header = "Tab 1" };
			var tabItem2 = new SfTabItem { Header = "Tab 2" };
			var tabItem3 = new SfTabItem { Header = "Tab 2" };
			var tabItem4 = new SfTabItem { Header = "Tab 2" };
			var tabItem5 = new SfTabItem { Header = "Tab 2" };
			ObservableCollection<SfTabItem> sfTabItems = new ObservableCollection<SfTabItem> { tabItem1, tabItem2, tabItem3, tabItem4, tabItem5 };
			horizontal.ItemsSource = sfTabItems;
			horizontal.SelectedIndex = value;
			var nextIndexValue = InvokePrivateMethod(horizontal, "GetNextItemIndex");
			Assert.NotNull(nextIndexValue);
			int nextIndex = (int)nextIndexValue;
			Assert.Equal(0, nextIndex);
		}

		[Fact]
		public void HorizontalContentItemSourceCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var newItemsSource = new List<string> { "Item1", "Item2", "Item3" };
			horizontal.ItemsSource = newItemsSource;
			var actualItemsSource = horizontal.ItemsSource;
			Assert.Equal(newItemsSource, actualItemsSource);
			SetPrivateField(horizontal, "_contentWidth", 300d);
			horizontal.ContentWidth = 300;
			DataTemplate newTemplate = new DataTemplate(() => new Label { Text = "Test" });
			horizontal.ContentItemTemplate = newTemplate;
			InvokePrivateMethod<SfHorizontalContent>(horizontal, "UpdateItemsSource");
			SfHorizontalStackLayout? horizontalStackLayout = GetPrivateField<SfHorizontalContent>(horizontal, "_horizontalStackLayout") as SfHorizontalStackLayout;
			if (horizontalStackLayout != null)
			{
				Assert.Equal(900, horizontalStackLayout.WidthRequest);
				foreach (var item in horizontalStackLayout.Children)
				{
					Assert.Equal(300, item.Width);
				}
			}
		}

		[Fact]
        public void TestInitializeControl()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            var invoke = InvokePrivateMethod(horizontal, "InitializeControl");
            Assert.Null(invoke);
            Assert.NotNull(horizontal.Content);
        }

        [Fact]
        public void TestOnTabItemsSourceCollectionChangedEvent()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            var invoke = InvokePrivateMethod(horizontal, "OnTabItemsSourceCollectionChanged", sfTabView, e);
            Assert.Null(invoke);
        }

        [Fact]
        public void TestOnItemsCollectionChangedEventTrigger()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            var invoke = InvokePrivateMethod(horizontal, "OnItemsCollectionChanged", sfTabView, e);
            Assert.Null(invoke);
        }

        [Fact]
        public void TestOnItemsCollectionChangedValueChange()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            horizontal.Items = PopulateLabelItemsCollection();
            NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            var invoke = InvokePrivateMethod(horizontal, "OnItemsCollectionChanged", horizontal, e);
            Assert.Equal(3, horizontal.Items.Count);
        }

        [Fact]
        public void TestClearTabContent()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            HorizontalStackLayout sfHorizontalStackLayout = new HorizontalStackLayout();
            var tabItem1 = new SfTabItem { Header = "Tab 1" };
            var tabItem2 = new SfTabItem { Header = "Tab 2" };
            horizontal.Add(tabItem1);
            horizontal.Add(tabItem2);
            horizontal.Children.Add(sfHorizontalStackLayout);
            horizontal.Content = sfHorizontalStackLayout;
            horizontal.Remove(sfHorizontalStackLayout);
            horizontal.SelectedIndex = 0;
            InvokePrivateMethod(horizontal, "ClearTabContent", tabItem1, 0);
            int itemCount = horizontal.Items.Count;
            Assert.Equal(0, itemCount);
        }

        [Fact]
        public void OnTabItemPropertyChangedCheck()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            var tabItem1 = new SfTabItem { Header = "Tab 1" };
            var tabItem2 = new SfTabItem { Header = "Tab 2" };
            sfTabView.Items.Add(tabItem1);
            sfTabView.Items.Add(tabItem2);
            var propertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SfTabItem.Content));
            var invoke = InvokePrivateMethod(horizontal, "OnTabItemPropertyChanged", sfTabView, propertyChangedEventArgs);
            Assert.Null(invoke);
        }

        [Fact]
        public void TestOnTabItemPropertyChanged()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            var tabItem1 = new SfTabItem { Header = "Tab 1" };
            var tabItem2 = new SfTabItem { Header = "Tab 2" };
            sfTabView.Items.Add(tabItem1);
            sfTabView.Items.Add(tabItem2);
            sfTabView.IsVisible = true;
            var propertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SfTabItem.Content));
            var invoke = InvokePrivateMethod(horizontal, "OnTabItemPropertyChanged", sfTabView.IsVisible, propertyChangedEventArgs);
            Assert.Null(invoke);
            if (propertyChangedEventArgs.PropertyName != null)
            {
                if (propertyChangedEventArgs.PropertyName.Equals(sfTabView.IsVisible))
                {
                    var invoked = InvokePrivateMethod(horizontal, "UpdateTabItemContentSize");
                    Assert.Null(invoked);
                    var invokes = InvokePrivateMethod(horizontal, "UpdateTabItemContentPosition");
                    Assert.Null(invokes);
                }
            }
        }

        [Fact]
        public void TestOnItemsCollectionChangedValueChanged()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            horizontal.Items = PopulateLabelItemsCollection();
            NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            var invoke = InvokePrivateMethod(horizontal, "OnItemsCollectionChanged", horizontal.Items[1], e);
            int values = 0;
            int value = 0;
            if (e.OldItems != null)
            {
                values = e.OldItems.Count;
            }
            if (e.NewItems != null && horizontal.Items != null)
            {
                foreach (SfTabItem tabItem in e.NewItems)
                {
                    var index = horizontal.Items.IndexOf(tabItem);
                    var invoked = InvokePrivateMethod(horizontal, "AddTabContentItems", tabItem, index);
                    value = e.NewItems.Count;
                }
            }
            Assert.Equal(0, values);
            Assert.Equal(0, value);
        }

        [Fact]
        public void ClearTabItemMethodCheck()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            horizontal.Items = PopulateLabelItemsCollection();
            var oldItems = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, horizontal.Items[0], 0);
            var clearTabContentMethod = typeof(SfHorizontalContent).GetMethod("ClearTabContent", BindingFlags.NonPublic | BindingFlags.Instance);
            horizontal.Items.RemoveAt(0);
            int itemCount = horizontal.Items.Count;
            Assert.Equal(2, itemCount);
        }

        [Fact]
        public void TestItemCollectionChangedShouldClearTabContentOnOldItems()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            horizontal.Items = PopulateLabelItemsCollection();
            var oldItems = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, horizontal.Items[0], 0);
            var clearTabContentMethod = typeof(SfHorizontalContent).GetMethod("ClearTabContent", BindingFlags.NonPublic | BindingFlags.Instance);
            horizontal.Items.RemoveAt(0);
            Assert.NotNull(clearTabContentMethod);
        }

		[Fact]
		public void GetNextVisibleItemIndexMethodValueCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.Items = PopulateLabelItemsCollection();
			sfTabView.SelectedIndex = 0;
			var invoke = InvokePrivateMethod(horizontal, "GetNextVisibleItemIndex");
			Assert.NotNull(invoke);
			double value = (int)invoke;
			Assert.Equal(0, value);
		}

		[Theory]
		[InlineData(100)]
		[InlineData(10000)]
		[InlineData(-10000)]
		[InlineData(0)]
		[InlineData(-50)]
		public void TestTranslateXPositionPositiveValue(double data)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			InvokePrivateMethod(horizontal, "TranslateXPosition", data);
			var _horizontalStackLayout = GetPrivateField(horizontal, "_horizontalStackLayout");
			if (_horizontalStackLayout != null && _horizontalStackLayout is SfHorizontalContent views1)
			{
				Assert.Equal(data, views1.TranslationX);
			}
		}

		[Theory]
		[InlineData(100)]
		[InlineData(10000)]
		[InlineData(-10000)]
		[InlineData(0)]
		[InlineData(-50)]
		public void TestAdjustForFirstIndex(double data)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			InvokePrivateMethod(horizontal, "AdjustForFirstIndex", data);
			var _horizontalStackLayout = GetPrivateField(horizontal, "_horizontalStackLayout");
			if (_horizontalStackLayout != null && _horizontalStackLayout is SfHorizontalContent views1)
			{
				Assert.Equal(data, views1.TranslationX);

			}
		}

		[Theory]
		[InlineData(100)]
		[InlineData(10000)]
		[InlineData(-10000)]
		[InlineData(0)]
		[InlineData(-50)]
		public void TestAdjustForFirstIndexMiddleIndexes(double data)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			InvokePrivateMethod(horizontal, "AdjustForMiddleIndices", data);
			var _horizontalStackLayout = GetPrivateField(horizontal, "_horizontalStackLayout");
			if (_horizontalStackLayout != null && _horizontalStackLayout is SfHorizontalContent views1)
			{
				Assert.Equal(data, views1.TranslationX);
			}
		}

		[Theory]
		[InlineData(100)]
		[InlineData(10000)]
		[InlineData(-10000)]
		[InlineData(0)]
		[InlineData(-50)]
		public void TestAdjustForFirstIndexLastIndex(double data)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			InvokePrivateMethod(horizontal, "AdjustForLastIndex", data);
			var _horizontalStackLayout = GetPrivateField(horizontal, "_horizontalStackLayout");
			if (_horizontalStackLayout != null && _horizontalStackLayout is SfHorizontalContent views1)
			{
				Assert.Equal(data, views1.TranslationX);
			}
		}

		[Fact]
		public void TestUpdateTabItemContentSize()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_contentWidth", 300d);
			InvokePrivateMethod(horizontal, "UpdateTabItemContentSize");
			var width = GetPrivateField(horizontal, "_contentWidth");
			double height = 200d;
			horizontal.HeightRequest = height;
			Assert.NotNull(width);
			double contentWidth = (double)width;
			Assert.Equal(300d, contentWidth);
			Assert.Equal(200d, horizontal.HeightRequest);
		}

		[Fact]
		public void TestUpdateTabItemContentPosition()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.ContentItemTemplate = new DataTemplate(() =>
			{
				Label label = new Label();
				label.BackgroundColor = Colors.Black;
				return label;
			});
			horizontal.ContentWidth = 400d;
			horizontal.SelectedIndex = 1;
			InvokePrivateMethod(horizontal, "UpdateTabItemContentPosition");
			if (horizontal.ContentItemTemplate != null)
			{
				var invoke = InvokePrivateMethod(horizontal, "UpdateTabItemContentPositionWithTemplate");
				Assert.Null(invoke);
			}
			Assert.NotNull(horizontal.ContentItemTemplate);
		}

		[Fact]
		public void TestVelocityValueCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_startPoint", new Point(50, 50));
			var value = GetPrivateField(horizontal, "_startPoint");
			InvokePrivateMethod(horizontal, "FindVelocity", new Point(10000000000, 10000000000));
			var value1 = GetPrivateField(horizontal, "_velocityX");
			Assert.NotNull(value1);
			double val = (double)value1;
			Assert.Equal(0.15658, Math.Round(val, 5));
		}

		[Fact]
		public void TestClampIndexValue()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var value = InvokePrivateMethod(horizontal, "ClampIndex", 1, 0, 5);
			Assert.NotNull(value);
			int value1 = (int)value;
			Assert.Equal(1, value1);
		}

		[Theory]
		[InlineData(100)]
		[InlineData(10000)]
		[InlineData(-10000)]
		[InlineData(0)]
		[InlineData(-50)]
		public void TestAdjustForFirstIndexMiddleIndex(double data)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_isPreviousItemVisible", true);
			SetPrivateField(horizontal, "_isNextItemVisible", true);
			IVisualElementController visualElementController = horizontal;
			InvokePrivateMethod(horizontal, "AdjustForMiddleIndices", data);
			var _horizontalStackLayout = GetPrivateField(horizontal, "_horizontalStackLayout");
			if (_horizontalStackLayout != null && _horizontalStackLayout is SfHorizontalContent views1)
			{
				Assert.Equal(data, views1.TranslationX);
			}
			if (visualElementController != null)
			{
				Assert.Equal(0, (int)visualElementController.EffectiveFlowDirection);
			}
		}

		[Fact]
		public void TestIsTowardsRightSetValue()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			bool eventTriggered = false;
			horizontal.SelectionChanging += (s, e) => eventTriggered = true;

			var args = new SelectionChangingEventArgs();
			InvokePrivateMethod(horizontal, "RaiseSelectionChangingEvent", args);
			SetNonPublicProperty(horizontal, "IsTowardsRight", true);
			var value = GetNonPublicProperty(horizontal, "IsTowardsRight");
			Assert.True(eventTriggered);
		}

		[Theory]
		[InlineData(100, 0)]
		[InlineData(10000, 2)]
		[InlineData(-10000, 1)]
		[InlineData(0, 0)]
		[InlineData(-50, 1)]
		public void TestTranslateXPositionValue(double data, int index)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			views.Add(new SfTabItem { Header = "Tab 1" });
			views.Add(new SfTabItem { Header = "Tab 1" });
			views.Add(new SfTabItem { Header = "Tab 1" });
			views.Add(new SfTabItem { Header = "Tab 1" });
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_tabBar", views);
			var tab = GetPrivateField(horizontal, "_tabBar");
			Assert.NotNull(tab);
			SfTabBar views2 = (SfTabBar)tab;
			views2.SelectedIndex = index;
			InvokePrivateMethod(horizontal, "TranslateXPosition", data);
			var _horizontalStackLayout = GetPrivateField(horizontal, "_horizontalStackLayout");
			if (_horizontalStackLayout != null && _horizontalStackLayout is SfHorizontalContent views1)
			{
				Assert.Equal(data, views1.TranslationX);
				Assert.Equal(index, views2.SelectedIndex);
			}

		}

		[Fact]
		public void TestItemSource()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_tabBar", views);
			var view = GetPrivateField(horizontal, "_tabBar");
			Assert.NotNull(view);
			SfTabBar views1 = (SfTabBar)view;
			views1.SelectedIndex = 1;
			horizontal.ItemsSource = new List<string> { "Tab1", "Tab2", "Tab3", "Tab4" };
			int eventCount = 0;
			horizontal.SelectionChanging += (sender, args) => eventCount++;
			SelectionChangingEventArgs args = new SelectionChangingEventArgs();
			InvokePrivateMethod(horizontal, "RaiseSelectionChanging");
			Assert.Equal(1, eventCount);
			Assert.Equal(4, horizontal.ItemsSource.Count);
			Assert.Equal(1, eventCount);
		}

		[Fact]
		public void TestGetVisibleItemsCount()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.Items = PopulateLabelItemsCollection();
			var invoke = InvokePrivateMethod(horizontal, "GetVisibleItemsCount");
			Assert.Equal(0, invoke);
			horizontal.SelectedIndex = 2;
			invoke = InvokePrivateMethod(horizontal, "GetVisibleItemsCount");
			Assert.Equal(2, invoke);
		}

		[Fact]
		public void TestMoveToPreviousTabItem()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.Items = PopulateLabelItemsCollection();
			horizontal.SelectedIndex = 2;
			var invoke = InvokePrivateMethod(horizontal, "SelectPreviousTabItem");
			SfTabBar? tabBar = GetPrivateField<SfHorizontalContent>(horizontal, "_tabBar") as SfTabBar;
			Assert.Equal(1, tabBar?.SelectedIndex);
			horizontal.SelectedIndex = 0;
			invoke = InvokePrivateMethod(horizontal, "SelectPreviousTabItem");
			Assert.Equal(0, tabBar?.SelectedIndex);
		}

		[Fact]
		public void TestSelectNextItem()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.Items = PopulateLabelItemsCollection();
			SetPrivateField(horizontal, "_isTowardsRight", true);
			var invoke = InvokePrivateMethod(horizontal, "SelectNextItem");
			SfTabBar? tabBar = GetPrivateField<SfHorizontalContent>(horizontal, "_tabBar") as SfTabBar;
			Assert.Equal(1, tabBar?.SelectedIndex);
			horizontal.SelectedIndex = 2;
			invoke = InvokePrivateMethod(horizontal, "SelectNextItem");
			Assert.Equal(2, tabBar?.SelectedIndex);
		}

		[Fact]
		public void TestUpdateItemVisibility()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.Items = PopulateLabelItemsCollection();
			var invoke = InvokePrivateMethod(horizontal, "UpdateItemVisibility");
			var isNextItemVisible = (bool?)GetPrivateField(horizontal, "_isNextItemVisible");
			var isPreviousItemVisible = (bool?)GetPrivateField(horizontal, "_isPreviousItemVisible");
			Assert.True(isNextItemVisible);
			Assert.False(isPreviousItemVisible);
			horizontal.SelectedIndex = 2;
			invoke = InvokePrivateMethod(horizontal, "UpdateItemVisibility");
			isNextItemVisible = (bool?)GetPrivateField(horizontal, "_isNextItemVisible");
			isPreviousItemVisible = (bool?)GetPrivateField(horizontal, "_isPreviousItemVisible");
			Assert.False(isNextItemVisible);
			Assert.True(isPreviousItemVisible);
		}

		[Fact]
		public void TestUpdateItemVisibilityWithTemplate()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.Items = PopulateLabelItemsCollection();
			horizontal.ContentItemTemplate = new DataTemplate(() =>
			{
				Label label = new Label();
				label.BackgroundColor = Colors.Black;
				return label;
			});
			var invoke = InvokePrivateMethod(horizontal, "UpdateItemVisibility");
			var isPreviousItemVisible = (bool?)GetPrivateField(horizontal, "_isPreviousItemVisible");
			Assert.False(isPreviousItemVisible);
		}

		[Fact]
		public void TestRaiseSelectionChangingEventCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			bool eventInvoked = false;
			horizontal.SelectionChanging += (s, e) => eventInvoked = true;

			var args = new SelectionChangingEventArgs();
			InvokePrivateMethod(horizontal, "RaiseSelectionChangingEvent", args);

			Assert.True(eventInvoked);
		}

		#endregion

		#region HorizontalContent Public Methods

		[Theory]
        [InlineData(PointerActions.Pressed)]
        [InlineData(PointerActions.Moved)]
        [InlineData(PointerActions.Released)]
        public void OnHandlerTouchInteractionMethodCheck(PointerActions pointerActions)
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            horizontal.OnHandleTouchInteraction(pointerActions, new Point(0, 0));
            if (pointerActions == PointerActions.Pressed)
            {
                var invokes = InvokePrivateMethod(horizontal, "InitializeTouchData", new Point(0, 0));
                Assert.Null(invokes);
            }
            if (pointerActions == PointerActions.Moved)
            {
                var invokes = InvokePrivateMethod(horizontal, "HandleTouchMovement", new Point(0, 0));
                Assert.Null(invokes);
            }
            if (pointerActions == PointerActions.Released)
            {
                var invokes = InvokePrivateMethod(horizontal, "HandleTouchReleased");
                Assert.Null(invokes);
            }
        }

		[Theory]
		[InlineData(PointerActions.Released)]
		[InlineData(PointerActions.Pressed)]
		[InlineData(PointerActions.Moved)]
		[InlineData(PointerActions.Cancelled)]
		[InlineData(PointerActions.Exited)]
		public void PointerActionPressedCheck(PointerActions pointerActions)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var expectedAction = pointerActions;
			horizontal.OnHandleTouchInteraction(pointerActions, new Point(0, 0));
			Assert.Equal(expectedAction, pointerActions);
		}

		[Fact]
		public void TestOnTouchMethod()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var pointerEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Pressed, new Point(10, 10));
			horizontal.OnTouch(pointerEventArgs);
			var pressed = GetPrivateField(horizontal, "_isPressed");
			Assert.NotNull(pressed);
			bool isPressed = (bool)pressed;
			Assert.False(isPressed);
		}

		#endregion

		#region General Tests

		[Fact]
        public void TestTabBarPlacementValue()
        {
            Assert.Equal(0, (int)TabBarPlacement.Bottom);
            Assert.Equal(1, (int)TabBarPlacement.Top);
        }

        [Fact]
        public void TestTabWidthModeValue()
        {
            Assert.Equal(0, (int)TabWidthMode.Default);
            Assert.Equal(1, (int)TabWidthMode.SizeToContent);
        }

        [Fact]
        public void TestTabIndicatorPlacementValue()
        {
            Assert.Equal(0, (int)TabIndicatorPlacement.Bottom);
            Assert.Equal(1, (int)TabIndicatorPlacement.Fill);
            Assert.Equal(2, (int)TabIndicatorPlacement.Top);
        }

        [Fact]
        public void TestTabImagePositionValues()
        {
            Assert.Equal(0, (int)TabImagePosition.Bottom);
            Assert.Equal(1, (int)TabImagePosition.Left);
            Assert.Equal(2, (int)TabImagePosition.Right);
            Assert.Equal(3, (int)TabImagePosition.Top);
        }

        [Fact]
        public void TestTabBarDisplayModeValues()
        {
            Assert.Equal(0, (int)TabBarDisplayMode.Image);
            Assert.Equal(1, (int)TabBarDisplayMode.Text);
            Assert.Equal(2, (int)TabBarDisplayMode.Default);
            Assert.NotEqual(1, (int)TabBarDisplayMode.Default);
        }

        [Fact]
        public void ValueIndicatorWidthModeValues()
        {
            Assert.Equal(0, (int)IndicatorWidthMode.Fit);
            Assert.Equal(1, (int)IndicatorWidthMode.Stretch);
        }

        [Fact]
        public void TestArrowTypeValues()
        {
            Assert.Equal(0, (int)ArrowType.Backward);
            Assert.Equal(1, (int)ArrowType.Forward);
        }

        [Fact]
        public void EnumUsageTest()
        {
            TabBarPlacement placement = TabBarPlacement.Top;
            string result = placement switch
            {
                TabBarPlacement.Bottom => "Bottom",
                TabBarPlacement.Top => "Top",
                _ => "Unknown"
            };
            Assert.Equal("Top", result);
        }

		#endregion

		#region TabItem Fields

		[Fact]
        public void TestIsSelectedPropertySetValue()
        {
            SfTabItem _tabItem = new SfTabItem();
            SetPrivateField(_tabItem, "_isSelected", true);
            Assert.True(_tabItem.IsSelected);
        }

        [Fact]
        public void TestIsSelectedFieldValueChange()
        {
            SfTabItem _tabItem = new SfTabItem();
            var value = GetPrivateField(_tabItem, "_isSelected");
            bool isSelected = _tabItem.IsSelected;
            Assert.False(isSelected);
        }

        [Fact]
        public void TestSetIsSelectedValue()
        {
            SfTabItem _tabItem = new SfTabItem();
            SetPrivateField(_tabItem, "_isSelected", true);
            var value = GetPrivateField(_tabItem, "_isSelected");
            bool isSelected = _tabItem.IsSelected;
            Assert.True(isSelected);
        }

		#endregion

		#region ArrowIcon Internal Properties

		[Fact]
        public void TestForegroundColorPropertyShouldBeDefaultColor()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            Assert.Equal(Color.FromArgb("#49454F"), _arrowIcon.ForegroundColor);
        }
        [Fact]
        public void TestForegroundColorPropertyShouldUpdateValue()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            var color = Color.FromArgb("#FF0000"); 
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Color.FromArgb("#00FF00");
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Color.FromArgb("#0000FF"); 
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Color.FromRgb(255, 0, 0); 
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Color.FromRgb(0, 255, 0); 
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Color.FromRgb(0, 0, 255); 
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Color.FromRgba(255, 0, 0, 128); 
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Color.FromRgba(0, 255, 0, 128); 
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Colors.Red;
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Colors.Green;
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Colors.Blue;
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Colors.White;
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
            color = Colors.Black;
            _arrowIcon.ForegroundColor = color;
            Assert.Equal(color, _arrowIcon.ForegroundColor);
        }

        [Fact]
        public void TestScrollButtonBackgroundColorPropertyShouldBeDefaultColor()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            Assert.Equal(Color.FromArgb("#F7F2FB"), _arrowIcon.ScrollButtonBackgroundColor);
        }

        [Fact]
        public void TestScrollButtonBackgroundColorProperty()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            var color = Color.FromArgb("#FF5733");
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Color.FromArgb("#33FF57");
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Color.FromArgb("#3357FF"); 
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Color.FromArgb("#FF33A1"); 
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Color.FromArgb("#FFD700"); 
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Color.FromRgb(255, 87, 51);
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Color.FromRgb(51, 255, 87);
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Color.FromRgb(51, 87, 255);
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Color.FromRgba(255, 87, 51, 128); 
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Color.FromRgba(51, 255, 87, 128); 
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Colors.OrangeRed;
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Colors.Green;
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Colors.Blue;
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Colors.Pink;
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
            color = Colors.Gold;
            _arrowIcon.ScrollButtonBackgroundColor = color;
            Assert.Equal(color, _arrowIcon.ScrollButtonBackgroundColor);
        }

        [Fact]
        public void TestButtonArrowTypePropertyShouldBeNull()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            Assert.Equal(ArrowType.Backward, _arrowIcon.ButtonArrowType);
        }

        [Fact]
        public void TestButtonArrowTypePropertyShouldUpdateValue()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            _arrowIcon.ButtonArrowType = ArrowType.Forward;
            Assert.Equal(ArrowType.Forward, _arrowIcon.ButtonArrowType);
            _arrowIcon.ButtonArrowType = ArrowType.Backward;
            Assert.Equal(ArrowType.Backward, _arrowIcon.ButtonArrowType);
        }

        [Fact]
        public void ShuffleButtonArrowType()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            _arrowIcon.ButtonArrowType = ArrowType.Forward;
            _arrowIcon.ButtonArrowType = ArrowType.Backward;
            Assert.Equal(ArrowType.Backward, _arrowIcon.ButtonArrowType);
            _arrowIcon.ButtonArrowType = ArrowType.Backward;
            _arrowIcon.ButtonArrowType = ArrowType.Forward;
            Assert.Equal(ArrowType.Forward, _arrowIcon.ButtonArrowType);
        }

		#endregion

		#region ArrowIcon Events

		[Fact]
        public void TestScrollButtonClickedEventShouldBeInvoked()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            bool eventInvoked = false;
            _arrowIcon.ScrollButtonClicked += (s, e) => eventInvoked = true;
            InvokePrivateMethod(_arrowIcon, "RaiseScrollButtonClickedEvent", new EventArgs());
            Assert.True(eventInvoked);
        }

		#endregion

		#region ArrowIcon Public Methods
		[Fact]
        public void TestOnTouchShouldSetIsPressedToFalse()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            SetPrivateField(_arrowIcon, "_isPressed", true);
            var pointerEventArgs = new PointerEventArgs(1, PointerActions.Released, new Point(10, 10));
            _arrowIcon.OnTouch(pointerEventArgs);
            object? privateField = GetPrivateField(_arrowIcon, "_isPressed");
            bool isPressed = privateField is bool pressed && pressed;
            Assert.False(isPressed);
        }

        [Fact]
        public void TestOnTouchShouldApplyHighlightEffect()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            var pointerEventArgs = new PointerEventArgs(1, PointerActions.Entered, new Point(10, 10));
            _arrowIcon.OnTouch(pointerEventArgs);
            var sfEffectsView = GetPrivateField(_arrowIcon, "_sfEffectsView") as SfEffectsView;
            Assert.NotNull(sfEffectsView);
        }

        [Fact]
        public void TestOnTouchExited()
        {
            ArrowIcon _arrowIcon = new ArrowIcon();
            var pointerEventArgs = new PointerEventArgs(1, PointerActions.Exited, new Point(10, 10));
            _arrowIcon.OnTouch(pointerEventArgs);
            var sfEffectsView = GetPrivateField(_arrowIcon, "_sfEffectsView") as SfEffectsView;
            Assert.NotNull(sfEffectsView);
        }
    }
	#endregion

	public class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private string? name;

        public string? Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        private string? subName;

        public string? SubName
        {
            get { return subName; }
            set
            {
                subName = value;
                OnPropertyChanged("SubName");
            }
        }
    }
}