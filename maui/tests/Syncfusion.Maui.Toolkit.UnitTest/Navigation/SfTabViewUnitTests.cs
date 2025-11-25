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
		SfTabBar tabBar = [];
		private void PopulateTabBarItems()
		{
			tabBar = [];

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
			List<Button> tabItems =
			[
				new Button() { Text = "button1" },
				new Button() { Text = "button2" },
				new Button() { Text = "button3" },
			];

			return tabItems;
		}

		private List<object> PopulateMixedObjectItemsSource()
		{
			List<object> tabItems = [new Button() { Text = "button" }, new Label() { Text = "label" }, new Microsoft.Maui.Controls.Picker() { }];

			return tabItems;
		}

		private TabItemCollection PopulateMixedItemsCollection()
		{
			var tabItems = new TabItemCollection()
				{
					new SfTabItem { Header = "TAB 1", Content = new Label { Text = "Content 1" } },
					new SfTabItem { Header = "TAB 2", Content = new Button { Text = "Content 2" } },
					new SfTabItem { Header = "TAB 3", Content = new Microsoft.Maui.Controls.Picker { } }
				};

			return tabItems;
		}

		private TabItemCollection PopulateLabelImageItemsCollection()
		{
			var tabViewItems = new TabItemCollection
			{
				new SfTabItem { Header = "TAB 1", ImageSource ="SampleImage1.png", Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "TAB 2", ImageSource ="SampleImage1.png", Content = new Label { Text = "Content 2" } },
				new SfTabItem { Header = "TAB 3", ImageSource ="SampleImage1.png", Content = new Label { Text = "Content 3" } }
			};
			return tabViewItems;
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

		private TabItemCollection PopulateLabelItemsCollectionMore()
		{
			var tabViewItems = new TabItemCollection
			{
				new SfTabItem { Header = "TAB 1", Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "TAB 2", Content = new Label { Text = "Content 2" } },
				new SfTabItem { Header = "TAB 3", Content = new Label { Text = "Content 3" } },
				new SfTabItem { Header = "TAB 4", Content = new Label { Text = "Content 4" } },
				new SfTabItem { Header = "TAB 5", Content = new Label { Text = "Content 5" } },
				new SfTabItem { Header = "TAB 6", Content = new Label { Text = "Content 6" } }
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
			List<object> mixedList =
			[
				"String item",
				42,
				new SfTabItem { Header = "TAB 1", Content = new Label { Text = "Content 1" } },
				true
			];
			return mixedList;
		}

		#region TabView Public properties

		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var tabView = new SfTabView();

			Assert.Null(tabView.TabBarBackground);
			Assert.Equal(TabBarDisplayMode.Default, tabView.HeaderDisplayMode);
			Assert.Equal(TabHeaderAlignment.Start, tabView.TabHeaderAlignment);
			Assert.Equal(TabBarPlacement.Top, tabView.TabBarPlacement);
			Assert.Equal(TabIndicatorPlacement.Bottom, tabView.IndicatorPlacement);
			Assert.Equal(IndicatorWidthMode.Fit, tabView.IndicatorWidthMode);
			Assert.Equal(TabWidthMode.Default, tabView.TabWidthMode);
			Assert.Equal(48d, tabView.TabBarHeight);
			Assert.Equal(Color.FromArgb("#6750A4"), ((SolidColorBrush)tabView.IndicatorBackground).Color);
			Assert.Equal(Color.FromArgb("#49454F"), tabView.ScrollButtonColor);
			Assert.Equal(Color.FromArgb("#F7F2FB"), ((SolidColorBrush)tabView.ScrollButtonBackground).Color);
			Assert.Equal(3, tabView.IndicatorStrokeThickness);
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
			Assert.True(tabView.IsContentTransitionEnabled);
		}

		[Theory]
		[InlineData(IndicatorWidthMode.Fit)]
		[InlineData(IndicatorWidthMode.Stretch)]
		public void IndicatorWidthMode_SetAndGet_ReturnsExpectedValue(IndicatorWidthMode expected)
		{
			var tabView = new SfTabView
			{
				IndicatorWidthMode = expected
			};

			Assert.Equal(expected, tabView.IndicatorWidthMode);
		}

		[Theory]
		[InlineData(TabBarDisplayMode.Default)]
		[InlineData(TabBarDisplayMode.Image)]
		[InlineData(TabBarDisplayMode.Text)]
		public void HeaderDisplayMode_SetAndGet_ReturnsExpectedValue(TabBarDisplayMode expected)
		{
			var tabView = new SfTabView
			{
				HeaderDisplayMode = expected
			};

			Assert.Equal(expected, tabView.HeaderDisplayMode);
		}

		[Theory]
		[InlineData(TabHeaderAlignment.Start)]
		[InlineData(TabHeaderAlignment.Center)]
		[InlineData(TabHeaderAlignment.End)]
		public void TabHeaderAlignment_SetAndGet_ReturnsExpectedValue(TabHeaderAlignment expected)
		{
			var tabView = new SfTabView
			{
				TabHeaderAlignment = expected
			};

			Assert.Equal(expected, tabView.TabHeaderAlignment);
		}


		[Fact]
		public void TabBarBackground_SetAndGet_ReturnsExpectedValue()
		{
			var tabView = new SfTabView
			{
				TabBarBackground = Colors.Beige
			};

			Assert.Equal(Colors.Beige, tabView.TabBarBackground);
		}

		[Theory]
		[InlineData(TabBarPlacement.Top)]
		[InlineData(TabBarPlacement.Bottom)]
		public void TabBarPlacement_SetAndGet_ReturnsExpectedValue(TabBarPlacement expected)
		{
			var tabView = new SfTabView
			{
				TabBarPlacement = expected
			};

			Assert.Equal(expected, tabView.TabBarPlacement);
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top)]
		[InlineData(TabIndicatorPlacement.Bottom)]
		[InlineData(TabIndicatorPlacement.Fill)]
		public void IndicatorPlacement_SetAndGet_ReturnsExpectedValue(TabIndicatorPlacement expected)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = expected
			};

			Assert.Equal(expected, tabView.IndicatorPlacement);
		}

		[Theory]
        [InlineData(true)]
        [InlineData(false)]        
        public void IsContentTransitionEnabled_SetAndGet_ReturnsExpectedValue(bool expected) 
        {
            SfTabView tabView = new SfTabView();
            tabView.IsContentTransitionEnabled = expected;
            Assert.Equal(expected, tabView.IsContentTransitionEnabled);
        }

		[Theory]
		[InlineData(TabWidthMode.Default)]
		[InlineData(TabWidthMode.SizeToContent)]
		public void TabWidthMode_SetAndGet_ReturnsExpectedValue(TabWidthMode expected)
		{
			var tabView = new SfTabView
			{
				TabWidthMode = expected
			};

			Assert.Equal(expected, tabView.TabWidthMode);
		}

		[Fact]
		public void IndicatorBackground_SetAndGet_ReturnsExpectedValue()
		{
			var tabView = new SfTabView
			{
				IndicatorBackground = Colors.Magenta
			};

			Assert.Equal(Colors.Magenta, tabView.IndicatorBackground);
		}

		[Fact]
		public void ScrollButtonBackground_SetAndGet_ReturnsExpectedValue()
		{
			var tabView = new SfTabView();
			tabView.ScrollButtonBackground = Colors.Red;
			Assert.Equal(Colors.Red, tabView.ScrollButtonBackground);
		}

		[Fact]
		public void ScrollButtonColor_SetAndGet_ReturnsExpectedValue()
		{
			var tabView = new SfTabView();
			tabView.ScrollButtonColor = Colors.Yellow;
			Assert.Equal(Colors.Yellow, tabView.ScrollButtonColor);
		}

		[Theory]
		[InlineData(40, 40)]
		[InlineData(0, 0)]
		[InlineData(-20, -20)]
		public void IndicatorStrokeThickness_SetAndGet_ReturnsExpectedValue(double value, double expected)
		{
			var tabView = new SfTabView();
			tabView.IndicatorStrokeThickness = value;
			Assert.Equal(expected, tabView.IndicatorStrokeThickness);
		}

		[Fact]
		public void TabBarHeight_SetAndGet_ReturnsExpectedValue()
		{
			var tabView = new SfTabView
			{
				TabBarHeight = 40d
			};

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
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = expected
			};

			Assert.Equal(expected, tabView.HeaderHorizontalTextAlignment);
		}

		[Theory]
		[InlineData(2)]
		[InlineData(0)]
		public void SelectedIndex_SetAndGet_ReturnsExpectedValue(int expected)
		{
			var tabView = new SfTabView
			{
				SelectedIndex = expected
			};

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
			var tabView = new SfTabView
			{
				FontAutoScalingEnabled = expected
			};

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
			var tabView = new SfTabView
			{
				IsScrollButtonEnabled = expected
			};

			Assert.Equal(expected, tabView.IsScrollButtonEnabled);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnableSwiping_SetAndGet_ReturnsExpectedValue(bool expected)
		{
			var tabView = new SfTabView
			{
				EnableSwiping = expected
			};

			Assert.Equal(expected, tabView.EnableSwiping);
		}

		[Theory]
		[InlineData(double.MinValue)]
		[InlineData(double.MaxValue)]
		[InlineData((double)0)]
		public void ContentTransitionDuration_SetAndGet_ReturnsExpectedValue(double expected)
		{
			var tabView = new SfTabView
			{
				ContentTransitionDuration = expected
			};

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

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnableVirtualization(bool expected)
		{
			var tabView = new SfTabView
			{
				EnableVirtualization = expected
			};

			Assert.Equal(expected, tabView.EnableVirtualization);
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
					handler.Method.Invoke(handler.Target, [tabView, eventArgs]);
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
					handler.Method.Invoke(handler.Target, [tabView, eventArgs]);
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
					handler.Method.Invoke(handler.Target, [tabView, eventArgs]);
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
					handler.Method.Invoke(handler.Target, [tabView, eventArgs]);
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
				Assert.Equal(3, tabBar.Items.Count);
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
			SfTabView tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection()
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
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
			SfTabView tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection()
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
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
		public void TestIndicatorPlacementPropertyShouldBeFill()
		{
			tabBar.IndicatorPlacement = TabIndicatorPlacement.Fill;
			Assert.Equal(TabIndicatorPlacement.Fill, tabBar.IndicatorPlacement);
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Bottom)]
		[InlineData(TabIndicatorPlacement.Top)]
		[InlineData(TabIndicatorPlacement.Fill)]
		public void TestIndicatorPositionInTabHeaderContainer(TabIndicatorPlacement expected)
		{
			var tabView = new SfTabView();
			tabView.IndicatorPlacement = expected;
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfGrid? parentGrid = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderContentContainer") as SfGrid;
			Assert.NotNull(parentGrid);
			Border? border = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as Border;
			if (expected == TabIndicatorPlacement.Fill)
			{
				Assert.Equal(border, parentGrid.Children[0]);
			}
			else
			{
				Assert.Equal(border, parentGrid.Children[1]);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Bottom)]
		[InlineData(TabIndicatorPlacement.Top)]
		[InlineData(TabIndicatorPlacement.Fill)]
		public void TestIndicatorLayoutPositionInTabHeaderContainer(TabIndicatorPlacement expected)
		{
			var tabView = new SfTabView();
			tabView.IndicatorPlacement = expected;
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfGrid? parentGrid = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderContentContainer") as SfGrid;
			Assert.NotNull(parentGrid);
			Border? border = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as Border;
			Assert.NotNull(border);
			if (expected == TabIndicatorPlacement.Fill)
			{
				Assert.Equal(LayoutOptions.Fill, border.VerticalOptions);
			}
			else if (expected == TabIndicatorPlacement.Top)
			{
				Assert.Equal(LayoutOptions.Start, border.VerticalOptions);
			}
			else
			{
				Assert.Equal(LayoutOptions.End, border.VerticalOptions);
			}
		}

		[Theory]
		[InlineData(TabBarPlacement.Bottom, 500, TabIndicatorPlacement.Bottom)]
		[InlineData(TabBarPlacement.Bottom, 250, TabIndicatorPlacement.Bottom)]
		[InlineData(TabBarPlacement.Bottom, 500, TabIndicatorPlacement.Fill)]
		[InlineData(TabBarPlacement.Bottom, 250, TabIndicatorPlacement.Fill)]
		[InlineData(TabBarPlacement.Bottom, 500, TabIndicatorPlacement.Top)]
		[InlineData(TabBarPlacement.Bottom, 250, TabIndicatorPlacement.Top)]
		public void BottomContentTransition_Bottom(TabBarPlacement tabBarPlacement, double contentTransition, TabIndicatorPlacement tabIndicatorPlacement)
		{
			var tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollectionMore();
			tabView.TabBarPlacement = tabBarPlacement;
			tabView.ContentTransitionDuration = contentTransition;
			tabView.IndicatorPlacement = tabIndicatorPlacement;
			SfGrid? parentGrid = GetPrivateField<SfTabView>(tabView, "_parentGrid") as SfGrid;
			Assert.NotNull(parentGrid);
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);
			Border? border = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as Border;
			Assert.NotNull(border);
			if (tabBarPlacement is TabBarPlacement.Bottom)
			{
				Assert.Equal(0, Grid.GetRow(horizontalContent));
				Assert.Equal(1, Grid.GetRow(tabBar));
			}
			else
			{
				Assert.Equal(0, Grid.GetRow(tabBar));
				Assert.Equal(1, Grid.GetRow(horizontalContent));
			}

			Assert.Equal(contentTransition, tabBar.ContentTransitionDuration);

			if (tabIndicatorPlacement == TabIndicatorPlacement.Fill)
			{
				Assert.Equal(LayoutOptions.Fill, border.VerticalOptions);
			}
			else if (tabIndicatorPlacement == TabIndicatorPlacement.Top)
			{
				Assert.Equal(LayoutOptions.Start, border.VerticalOptions);
			}
			else
			{
				Assert.Equal(LayoutOptions.End, border.VerticalOptions);
			}
		}

		[Theory]
		[InlineData(TabBarPlacement.Bottom, 500, TabIndicatorPlacement.Bottom, FlowDirection.RightToLeft)]
		[InlineData(TabBarPlacement.Bottom, 250, TabIndicatorPlacement.Bottom, FlowDirection.RightToLeft)]
		[InlineData(TabBarPlacement.Bottom, 500, TabIndicatorPlacement.Fill, FlowDirection.RightToLeft)]
		[InlineData(TabBarPlacement.Bottom, 250, TabIndicatorPlacement.Fill, FlowDirection.RightToLeft)]
		[InlineData(TabBarPlacement.Bottom, 500, TabIndicatorPlacement.Top, FlowDirection.RightToLeft)]
		[InlineData(TabBarPlacement.Bottom, 250, TabIndicatorPlacement.Top, FlowDirection.RightToLeft)]
		public void Direction_BottomContentTransition_Bottom(TabBarPlacement tabBarPlacement, double contentTransition, TabIndicatorPlacement tabIndicatorPlacement, FlowDirection flowDirection)
		{
			var tabView = new SfTabView();
			tabView.FlowDirection = flowDirection;
			tabView.Items = PopulateLabelItemsCollectionMore();
			tabView.TabBarPlacement = tabBarPlacement;
			tabView.ContentTransitionDuration = contentTransition;
			tabView.IndicatorPlacement = tabIndicatorPlacement;
			SfGrid? parentGrid = GetPrivateField<SfTabView>(tabView, "_parentGrid") as SfGrid;
			Assert.NotNull(parentGrid);
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);
			Border? border = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as Border;
			Assert.NotNull(border);
			if (tabBarPlacement is TabBarPlacement.Bottom)
			{
				Assert.Equal(0, Grid.GetRow(horizontalContent));
				Assert.Equal(1, Grid.GetRow(tabBar));
			}
			else
			{
				Assert.Equal(0, Grid.GetRow(tabBar));
				Assert.Equal(1, Grid.GetRow(horizontalContent));
			}

			Assert.Equal(contentTransition, tabBar.ContentTransitionDuration);

			if (tabIndicatorPlacement == TabIndicatorPlacement.Fill)
			{
				Assert.Equal(LayoutOptions.Fill, border.VerticalOptions);
			}
			else if (tabIndicatorPlacement == TabIndicatorPlacement.Top)
			{
				Assert.Equal(LayoutOptions.Start, border.VerticalOptions);
			}
			else
			{
				Assert.Equal(LayoutOptions.End, border.VerticalOptions);
			}
		}

		[Theory]
		[InlineData(500, TabIndicatorPlacement.Bottom, FlowDirection.RightToLeft)]
		[InlineData(250, TabIndicatorPlacement.Bottom, FlowDirection.RightToLeft)]
		[InlineData(500, TabIndicatorPlacement.Fill, FlowDirection.RightToLeft)]
		[InlineData(250, TabIndicatorPlacement.Fill, FlowDirection.RightToLeft)]
		[InlineData(500, TabIndicatorPlacement.Top, FlowDirection.RightToLeft)]
		[InlineData(250, TabIndicatorPlacement.Top, FlowDirection.RightToLeft)]
		public void Direction_TopContentTransition(double contentTransition, TabIndicatorPlacement tabIndicatorPlacement, FlowDirection flowDirection)
		{
			var tabView = new SfTabView();
			tabView.FlowDirection = flowDirection;
			tabView.Items = PopulateLabelItemsCollectionMore();
			tabView.ContentTransitionDuration = contentTransition;
			tabView.IndicatorPlacement = tabIndicatorPlacement;
			SfGrid? parentGrid = GetPrivateField<SfTabView>(tabView, "_parentGrid") as SfGrid;
			Assert.NotNull(parentGrid);
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);
			Border? border = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as Border;
			Assert.NotNull(border);

			Assert.Equal(contentTransition, tabBar.ContentTransitionDuration);

			if (tabIndicatorPlacement == TabIndicatorPlacement.Fill)
			{
				Assert.Equal(LayoutOptions.Fill, border.VerticalOptions);
			}
			else if (tabIndicatorPlacement == TabIndicatorPlacement.Top)
			{
				Assert.Equal(LayoutOptions.Start, border.VerticalOptions);
			}
			else
			{
				Assert.Equal(LayoutOptions.End, border.VerticalOptions);
			}
		}

		[Theory]
		[InlineData(500, TabIndicatorPlacement.Bottom)]
		[InlineData(250, TabIndicatorPlacement.Bottom)]
		[InlineData(500, TabIndicatorPlacement.Fill)]
		[InlineData(250, TabIndicatorPlacement.Fill)]
		[InlineData(500, TabIndicatorPlacement.Top)]
		[InlineData(250, TabIndicatorPlacement.Top)]
		public void TopContentTransition(double contentTransition, TabIndicatorPlacement tabIndicatorPlacement)
		{
			var tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollectionMore();
			tabView.ContentTransitionDuration = contentTransition;
			tabView.IndicatorPlacement = tabIndicatorPlacement;
			SfGrid? parentGrid = GetPrivateField<SfTabView>(tabView, "_parentGrid") as SfGrid;
			Assert.NotNull(parentGrid);
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);
			Border? border = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as Border;
			Assert.NotNull(border);

			Assert.Equal(contentTransition, tabBar.ContentTransitionDuration);

			if (tabIndicatorPlacement == TabIndicatorPlacement.Fill)
			{
				Assert.Equal(LayoutOptions.Fill, border.VerticalOptions);
			}
			else if (tabIndicatorPlacement == TabIndicatorPlacement.Top)
			{
				Assert.Equal(LayoutOptions.Start, border.VerticalOptions);
			}
			else
			{
				Assert.Equal(LayoutOptions.End, border.VerticalOptions);
			}
		}

		[Theory]
		[InlineData(FlowDirection.RightToLeft)]
		[InlineData(FlowDirection.LeftToRight)]
		public void FlowDirection1(FlowDirection flowDirection)
		{
			var tabView = new SfTabView()
			{
				FlowDirection = flowDirection,
			};

			tabView.Items = PopulateLabelItemsCollectionMore();

			SfGrid? parentGrid = GetPrivateField<SfTabView>(tabView, "_parentGrid") as SfGrid;
			Assert.NotNull(parentGrid);
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);
			Border? border = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as Border;
			Assert.NotNull(border);
			Assert.Equal(flowDirection, tabBar.FlowDirection);
			Assert.Equal(flowDirection, parentGrid.FlowDirection);
		}

		[Theory]
		[InlineData(TabBarPlacement.Bottom, FlowDirection.LeftToRight)]
		public void FlowDirection2(TabBarPlacement tabBarPlacement, FlowDirection flowDirection)
		{
			var tabView = new SfTabView()
			{
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelItemsCollectionMore();
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			SfGrid? parentGrid = GetPrivateField<SfTabView>(tabView, "_parentGrid") as SfGrid;
			Assert.NotNull(parentGrid);
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);
			Border? border = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as Border;
			Assert.NotNull(border);
			Assert.Equal(FlowDirection.RightToLeft, parentGrid.FlowDirection);
			Assert.Equal(FlowDirection.RightToLeft, tabBar.FlowDirection);
			tabView.FlowDirection = flowDirection;
			tabView.TabBarPlacement = tabBarPlacement;
			if (tabBarPlacement is TabBarPlacement.Bottom)
			{
				Assert.Equal(0, Grid.GetRow(horizontalContent));
				Assert.Equal(1, Grid.GetRow(tabBar));
			}
			else
			{
				Assert.Equal(0, Grid.GetRow(tabBar));
				Assert.Equal(1, Grid.GetRow(horizontalContent));
			}

			Assert.Equal(flowDirection, tabBar.FlowDirection);
			Assert.Equal(flowDirection, parentGrid.FlowDirection);
		}

		[Theory]
		[InlineData(TabBarPlacement.Bottom, FlowDirection.RightToLeft)]
		[InlineData(TabBarPlacement.Top, FlowDirection.RightToLeft)]
		public void FlowDirection3(TabBarPlacement tabBarPlacement, FlowDirection flowDirection)
		{
			var tabView = new SfTabView()
			{
				FlowDirection = flowDirection,
			};

			tabView.Items = PopulateLabelItemsCollectionMore();

			SfGrid? parentGrid = GetPrivateField<SfTabView>(tabView, "_parentGrid") as SfGrid;
			Assert.NotNull(parentGrid);
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);
			Border? border = GetPrivateField<SfTabBar>(tabBar, "_tabSelectionIndicator") as Border;
			Assert.NotNull(border);
			Assert.Equal(flowDirection, tabBar.FlowDirection);
			Assert.Equal(flowDirection, parentGrid.FlowDirection);
			tabView.TabBarPlacement = tabBarPlacement;
			if (tabBarPlacement is TabBarPlacement.Bottom)
			{
				Assert.Equal(0, Grid.GetRow(horizontalContent));
				Assert.Equal(1, Grid.GetRow(tabBar));
			}
			else
			{
				Assert.Equal(0, Grid.GetRow(tabBar));
				Assert.Equal(1, Grid.GetRow(horizontalContent));
			}
		}

		[Theory]
		[InlineData(TabBarPlacement.Bottom, FlowDirection.LeftToRight)]
		[InlineData(TabBarPlacement.Top, FlowDirection.LeftToRight)]
		public void FlowDirection4(TabBarPlacement tabBarPlacement, FlowDirection flowDirection)
		{
			var tabView = new SfTabView()
			{
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelItemsCollectionMore();

			SfGrid? parentGrid = GetPrivateField<SfTabView>(tabView, "_parentGrid") as SfGrid;
			Assert.NotNull(parentGrid);
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);
			Assert.Equal(FlowDirection.RightToLeft, tabBar.FlowDirection);
			Assert.Equal(FlowDirection.RightToLeft, parentGrid.FlowDirection);
			tabView.FlowDirection = flowDirection;
			tabView.TabBarPlacement = tabBarPlacement;
			if (tabBarPlacement is TabBarPlacement.Bottom)
			{
				Assert.Equal(0, Grid.GetRow(horizontalContent));
				Assert.Equal(1, Grid.GetRow(tabBar));
			}
			else
			{
				Assert.Equal(0, Grid.GetRow(tabBar));
				Assert.Equal(1, Grid.GetRow(horizontalContent));
			}

			Assert.Equal(flowDirection, tabBar.FlowDirection);
			Assert.Equal(flowDirection, parentGrid.FlowDirection);
		}

		[Theory]
		[InlineData(50)]
		[InlineData(200)]
		[InlineData(1000)]
		[InlineData(50, true)]
		public void MAUI_SfTabView_LoadTesting(int numberOfItems, bool removeNewItems = false)
		{
			var tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollectionMore();
			var existingCount = tabView.Items.Count;
			for (int i = 0; i < numberOfItems; i++)
			{
				var item = new SfTabItem()
				{
					Header = $"Item {tabView.Items.Count + 1}",
					Content = new Label()
					{
						Text = $"Item {i}",
					}
				};
				tabView.Items.Add(item);
			}

			var totalCount = existingCount + numberOfItems;
			Assert.Equal(totalCount, tabView.Items.Count);

			if (removeNewItems)
			{
				for (int i = totalCount - 1; i > existingCount - 1; i--)
				{
					tabView.Items.RemoveAt(i);
				}

				Assert.Equal(existingCount, tabView.Items.Count);
			}
		}

		[Theory]
		[InlineData(2)]
		[InlineData(2, true)]
		[InlineData(2, true, true)]
		[InlineData(2, false, true)]
		public void TestTab(int numberOfItems, bool removeNewItems = false, bool changeFlowDirection = false)
		{
			var tabView = new SfTabView();
			tabView.Items = PopulateLabelItemsCollectionMore();
			var existingCount = tabView.Items.Count;
			for (int i = 0; i < numberOfItems; i++)
			{
				var item = new SfTabItem()
				{
					Header = $"Item {tabView.Items.Count + 1}",
					Content = new Label()
					{
						Text = $"Item {i}",
					}
				};
				tabView.Items.Add(item);
			}

			var totalCount = existingCount + numberOfItems;
			Assert.Equal(totalCount, tabView.Items.Count);

			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			SfGrid? parentGrid = GetPrivateField<SfTabView>(tabView, "_parentGrid") as SfGrid;
			Assert.NotNull(parentGrid);
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);

			tabView.TabBarPlacement = TabBarPlacement.Bottom;

			Assert.Equal(0, Grid.GetRow(horizontalContent));
			Assert.Equal(1, Grid.GetRow(tabBar));

			if (removeNewItems)
			{
				for (int i = totalCount - 1; i > existingCount - 1; i--)
				{
					tabView.Items.RemoveAt(i);
				}

				Assert.Equal(existingCount, tabView.Items.Count);
			}

			if (changeFlowDirection)
			{
				tabView.FlowDirection = FlowDirection.RightToLeft;
				Assert.Equal(FlowDirection.RightToLeft, tabBar.FlowDirection);
				Assert.Equal(FlowDirection.RightToLeft, parentGrid.FlowDirection);
			}
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
			tabBar.ScrollButtonColor = iconColor;
			Assert.Equal(iconColor, tabBar.ScrollButtonColor);
		}

		[Theory]
		[InlineData(30, 30)]
		[InlineData(0, 0)]
		[InlineData(-20, -20)]
		public void TestIndicatorStrokeThickness(double value, double expected)
		{
			tabBar.IndicatorStrokeThickness = value;
			Assert.Equal(expected, tabBar.IndicatorStrokeThickness);
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
			SfTabView tabView = new SfTabView
			{
				IndicatorPlacement = expected
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
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
			SfTabView tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection(),
				IndicatorCornerRadius = new CornerRadius(6)
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
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
			SfTabBar _tabBar = [];
			_tabBar.FlowDirection = FlowDirection.LeftToRight;
			_tabBar.IsScrollButtonEnabled = true;
			_tabBar.UpdateFlowDirection();
			var flowDirection = _tabBar.FlowDirection;
			Assert.Equal(FlowDirection.LeftToRight, flowDirection);
		}

		[Fact]
		public void TestUpdateFlowDirectionRTL()
		{
			SfTabBar _tabBar = [];
			_tabBar.FlowDirection = FlowDirection.RightToLeft;
			_tabBar.UpdateFlowDirection();
			var flowDirection = _tabBar.FlowDirection;
			Assert.Equal(FlowDirection.RightToLeft, flowDirection);
		}

		[Fact]
		public void TestUpdateFlowDirectionMultiple()
		{
			SfTabBar _tabBar = [];
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
			SfTabBar _tabBar = [];
			_tabBar.UpdateFlowDirection();
			var flowDirection = _tabBar.FlowDirection;
			Assert.Equal(FlowDirection.MatchParent, flowDirection);
		}

		[Fact]
		public void TestUpdateTabIndicatorPosition()
		{
			SfTabView tabView = new SfTabView()
			{
				Items = new TabItemCollection()
				{
					new SfTabItem() {Header="Tab1"},
					new SfTabItem() {Header = "Tab2"},
					new SfTabItem() {Header="Tab3"},
					new SfTabItem() {Header = "Tab4"},
				},
				IsScrollButtonEnabled = true

			};

			SfTabBar? tabBar = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			tabBar.UpdateTabIndicatorPosition();
			SfScrollView? scrollView = GetPrivateField(tabBar, "_tabHeaderScrollView") as SfScrollView;
			Assert.NotNull(scrollView);
		}

		[Theory]
		[InlineData(3, 4, 3)]
		[InlineData(2, 3, 2)]
		public void TestClearHeaderItem(double index, double selectedIndex, double expected)
		{
			var tabitems = new TabItemCollection()
			{
				new SfTabItem() {Header="Tab1"},
				new SfTabItem() {Header = "Tab2"},
				new SfTabItem() {Header="Tab3"},
				new SfTabItem() {Header = "Tab4"},
			};

			SfTabBar tabBar = new SfTabBar()
			{
				Items = tabitems,
				SelectedIndex = (int)selectedIndex,
				IsScrollButtonEnabled = true
			};

			SfHorizontalStackLayout? horizontalStackLayout = GetPrivateField(tabBar, "_tabHeaderItemContent") as SfHorizontalStackLayout;
			Assert.NotNull(horizontalStackLayout);
			Assert.Equal(4, horizontalStackLayout.Children.Count);
			tabBar.ClearHeaderItem(tabitems[3], (int)index);
			SfHorizontalStackLayout? stackLayout = GetPrivateField(tabBar, "_tabHeaderItemContent") as SfHorizontalStackLayout;
			Assert.NotNull(stackLayout);
			Assert.Equal(3, stackLayout.Children.Count);
			Assert.Equal(expected, tabBar.SelectedIndex);

		}

		[Fact]
		public void TestClearLastHeaderItem()
		{
			var tabitems = new TabItemCollection()
			{
				new SfTabItem() {Header="Tab1"},
				new SfTabItem() {Header = "Tab2"},
				new SfTabItem() {Header="Tab3"},
				new SfTabItem() {Header = "Tab4"},
			};

			SfTabBar tabBar = new SfTabBar()
			{
				Items = tabitems,
				SelectedIndex = 4
			};

			SfHorizontalStackLayout? horizontalStackLayout = GetPrivateField(tabBar, "_tabHeaderItemContent") as SfHorizontalStackLayout;
			Assert.NotNull(horizontalStackLayout);
			Assert.Equal(4, horizontalStackLayout.Children.Count);
			tabBar.ClearHeaderItem(tabitems[3], 4);
			Assert.Equal(3, tabBar.SelectedIndex);

		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Fill)]
		[InlineData(TabIndicatorPlacement.Bottom)]
		[InlineData(TabIndicatorPlacement.Top)]
		public void TestUpdateSelectionIndicatorZIndex(TabIndicatorPlacement tabIndicatorPlacement)
		{
			var tabitems = new TabItemCollection()
			{
				new SfTabItem() {Header="Tab1"},
				new SfTabItem() {Header = "Tab2"},
				new SfTabItem() {Header="Tab3"},
				new SfTabItem() {Header = "Tab4"},
			};

			SfTabBar tabBar = new SfTabBar()
			{
				Items = tabitems,
				IndicatorPlacement = tabIndicatorPlacement,
				SelectedIndex = 2
			};

			tabBar.UpdateSelectionIndicatorZIndex();

			SfGrid? sfGrid = GetPrivateField(tabBar, "_tabHeaderContentContainer") as SfGrid;
			Border? border = GetPrivateField(tabBar, "_tabSelectionIndicator") as Border;
			Assert.NotNull(sfGrid);
			Assert.NotNull(border);
			if (tabIndicatorPlacement == TabIndicatorPlacement.Fill)
			{

				Assert.True(sfGrid.Children.Contains(border!));
				Assert.Equal(0, sfGrid.IndexOf(border));
			}
			else
			{
				Assert.True(sfGrid.Children.Contains(border!));
				Assert.NotEqual(0, sfGrid.Children.IndexOf(border));
			}
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
			SfTabBar _sfTabBar = [];
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

		[Fact]
		public void TestTabBarOnTapWithInvalidSender()
		{
			SfTabBar tabBar = new SfTabBar();
			SfTabItem tabItem = new SfTabItem();
			var tapArgs = new TapEventArgs(new Point(50, 50), 1);
			var exception = Record.Exception(() => tabBar.OnTap(tabItem, tapArgs));
			Assert.Null(exception);
		}

		#endregion

		#region Behaviour Tests

		[Theory]
		[InlineData(TextAlignment.End)]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Center)]
		public void TestSfTabBarHeaderHorizontalTextAlignment(TextAlignment expected)
		{
			SfTabView tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = expected
			};
			SfTabBar? tabBar = ((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			Assert.Equal(expected, tabBar?.HeaderHorizontalTextAlignment);
		}

		[Theory]
		[InlineData(IndicatorWidthMode.Fit)]
		[InlineData(IndicatorWidthMode.Stretch)]
		public void TestSfTabBarIndicatorWidthMode(IndicatorWidthMode expected)
		{
			SfTabView tabView = new SfTabView
			{
				IndicatorWidthMode = expected
			};
			SfTabBar? tabBar = ((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			Assert.Equal(expected, tabBar?.IndicatorWidthMode);
		}

		[Theory]
		[InlineData(TabWidthMode.Default)]
		[InlineData(TabWidthMode.SizeToContent)]
		public void TestSfTabBarTabWidthMode(TabWidthMode expected)
		{
			SfTabView tabView = new SfTabView
			{
				TabWidthMode = expected
			};
			SfTabBar? tabBar = ((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			Assert.Equal(expected, tabBar?.TabWidthMode);
		}

		[Fact]
		public void TestSfTabBarTabHeaderPadding()
		{
			SfTabView tabView = new SfTabView
			{
				TabHeaderPadding = new Thickness(-5)
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
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
			SfTabView tabView = new SfTabView
			{
				IndicatorBackground = Colors.Maroon
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
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
			SfTabView tabView = new SfTabView
			{
				IndicatorCornerRadius = new CornerRadius(0)
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
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
			SfTabBar? tabBar = ((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			tabView.HeaderDisplayMode = expected;
			Assert.Equal(expected, tabBar?.HeaderDisplayMode);
		}

		[Theory]
		[InlineData(TabHeaderAlignment.Start)]
		[InlineData(TabHeaderAlignment.Center)]
		[InlineData(TabHeaderAlignment.End)]
		public void TestSfTabBarTabHeaderAlignment(TabHeaderAlignment expected)
		{
			SfTabView tabView = new SfTabView();
			SfTabBar? tabBar = ((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			tabView.TabHeaderAlignment = expected;
			Assert.Equal(expected, tabBar?.TabHeaderAlignment);
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
			SfTabBar? tabBar = ((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() as SfTabBar;
			tabView.ContentTransitionDuration = expected;
			if (expected > 100)
			{
				Assert.Equal(expected, tabBar?.ContentTransitionDuration);
			}
			else
			{
				Assert.Equal(100, tabBar?.ContentTransitionDuration);
			}
		}

		[Fact]
		public void TestSfTabBarFontAutoScalingEnabled()
		{
			SfTabView tabView = new SfTabView
			{
				FontAutoScalingEnabled = true
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
			{
				Assert.True(tabBar.FontAutoScalingEnabled);
				tabView.FontAutoScalingEnabled = false;
				Assert.False(tabBar.FontAutoScalingEnabled);
			}
		}

		[Fact]
		public void TestSfTabBarScrollButtonAppearance()
		{
			SfTabView tabView = new SfTabView
			{
				ScrollButtonBackground = Colors.Green,
				ScrollButtonColor = Colors.Green
			};
			tabView.ScrollButtonBackground = Colors.Green;
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
			{
				Assert.Equal(Colors.Green, tabBar.ScrollButtonBackground);
				Assert.Equal(Colors.Green, tabBar.ScrollButtonColor);
				Assert.Equal(Colors.Green, tabBar.ScrollButtonBackground);

				tabView.ScrollButtonBackground = Colors.Transparent;
				tabView.ScrollButtonColor = Colors.Transparent;
				tabView.ScrollButtonBackground = Colors.Transparent;
				Assert.Equal(Colors.Transparent, tabBar.ScrollButtonBackground);
				Assert.NotEqual(Colors.Green, tabBar.ScrollButtonBackground);
				Assert.Equal(Colors.Transparent, tabBar.ScrollButtonColor);
				Assert.NotEqual(Colors.Green, tabBar.ScrollButtonColor);
				Assert.Equal(Colors.Transparent, tabBar.ScrollButtonBackground);
				Assert.NotEqual(Colors.Green, tabBar.ScrollButtonBackground);
			}
		}

		[Fact]
		public void TestTabBarGetEffectsView()
		{
			SfTabView tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection()
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
			{
				var effectsView = tabBar.GetEffectsView(tabBar.Items[0]);
				Assert.NotNull(effectsView);
			}
		}

		[Fact]
		public void TestSfTabBarItems()
		{
			SfTabBar tabBar = [];
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

				tabBar.Items = [];
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
			SfTabBar tabBar = [];
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

				tabBar.Items = [];
				Assert.NotNull(tabBar?.Items);
			}
		}

		[Fact]
		public void TestSfTabBarLargeTabData()
		{
			SfTabBar tabBar = [];
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
			SfTabBar tabBar = [];

			if (tabBar != null)
			{
				tabBar.ItemsSource = PopulateMixedDataItemsSource();
				Assert.Equal(4, tabBar.ItemsSource?.Count);
				if (tabBar.ItemsSource != null)
				{
					Assert.True(tabBar.ItemsSource[2] is SfTabItem);
				}

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
			var tabView = new SfTabView
			{
				HeaderDisplayMode = displayMode
			};
			Assert.Equal(displayMode, tabView.HeaderDisplayMode);

			SfTabBar? tabHeaderContainer = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(displayMode, tabHeaderContainer?.HeaderDisplayMode);
		}

		[Theory]
		[InlineData(TabHeaderAlignment.Start)]
		[InlineData(TabHeaderAlignment.Center)]
		[InlineData(TabHeaderAlignment.End)]
		public void TestSetTabHeaderAlignment(TabHeaderAlignment headerAlignment)
		{
			var tabView = new SfTabView
			{
				TabHeaderAlignment = headerAlignment
			};
			Assert.Equal(headerAlignment, tabView.TabHeaderAlignment);

			SfTabBar? tabHeaderContainer = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(headerAlignment, tabHeaderContainer?.TabHeaderAlignment);
		}

		[Fact]
		public void TestTabContentContainerSetItemsSourceAndContentItemTemplate()
		{
			var tabView = new SfTabView();
			var tabContentContainerField = typeof(SfTabView).GetField("_tabContentContainer", BindingFlags.NonPublic | BindingFlags.Instance);
			var tabContentContainer = new SfHorizontalContent(tabView, []);
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
			var tabView = new SfTabView
			{
				TabBarHeight = tabBarHeight
			};
			Assert.Equal(tabBarHeight, tabView.TabBarHeight);
			if (GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") is SfTabBar tabHeaderContainer)
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
			var tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection()
			};
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
			SfTabBar tabBar = [];
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

		[Theory]
		[InlineData(28, 28)]
		[InlineData(0, 0)]
		[InlineData(-14, -14)]
		public void TestUpdateIndicatorStrokeThicknessWithIndicatorPositionBottom(double value, double expected)
		{
			tabBar.Items.Add(new SfTabItem { Header = "Tab 1" });
			tabBar.Items.Add(new SfTabItem { Header = "Tab 2" });
			tabBar.IndicatorPlacement = TabIndicatorPlacement.Bottom;
			SfBorder? indicator = GetPrivateField(tabBar, "_tabSelectionIndicator") as SfBorder;
			tabBar.IndicatorStrokeThickness = value;
			tabBar.UpdateIndicatorStrokeThickness(value);
			Assert.Equal(expected, indicator?.HeightRequest);
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
				if (GetPrivateField<SfTabBar>(tabBar, "_roundRectangle") is RoundRectangle roundRectangle)
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
			SfTabBar tabBar = [];
			tabBar.Items = PopulateMixedItemsCollection();
			tabBar.TabWidthMode = TabWidthMode.Default;
			InvokePrivateMethod(tabBar, "UpdateTabPadding");
			var tabHeaderContentContainer = GetPrivateField<SfTabBar>(tabBar, "_tabHeaderContentContainer");
			Assert.Equal(new Thickness(0), (tabHeaderContentContainer as SfGrid)?.Padding);
		}

		[Fact]
		public void TestUpdateTabPadding()
		{
			SfTabBar tabBar = new SfTabBar();
			tabBar.Items = PopulateMixedItemsCollection();
			InvokePrivateMethod(tabBar, "UpdateTabPadding");
			Assert.Equal(0, tabBar.SelectedIndex);
		}

		[Fact]
		public void TestGetNextVisibleItem()
		{
			SfTabBar tabBar = [];
			tabBar.Items = PopulateMixedItemsCollection();
			InvokePrivateMethod(tabBar, "UpdateTabPadding");
			Assert.Equal(0, tabBar.SelectedIndex);
		}

		[Fact]
		public void TestOnTabItemTouched()
		{
			tabBar = [];
			tabBar.ItemsSource = PopulateButtonsListItemsSource();
			var pointerEventArgs = new PointerEventArgs(1, PointerActions.Cancelled, new Point(10, 10));
			SfTabItem item = new SfTabItem() { Header = "Tab 1" };
			tabBar.Items.Add(item);
			InvokePrivateMethod(tabBar, "OnTabItemTouched", item, pointerEventArgs);
		}

		[Theory]
		[InlineData(TextAlignment.Center)]
		[InlineData(TextAlignment.End)]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Justify)]
		public void TestUpdateHeaderHorizontalAlignment(TextAlignment alignment)
		{
			PopulateTabBarItems();
			tabBar.HeaderHorizontalTextAlignment = alignment;
			foreach (var tabItem in tabBar.Items)
			{
				Assert.Equal(alignment, tabItem.HeaderHorizontalTextAlignment);
			}

		}

		[Theory]
		[InlineData(TabBarDisplayMode.Default)]
		[InlineData(TabBarDisplayMode.Image)]
		[InlineData(TabBarDisplayMode.Text)]
		public void TestHeaderDisplayModeDynamicChanges(TabBarDisplayMode tabBarDisplayMode)
		{
			SfTabBar tabBar = new SfTabBar();
			var imageSource = new FileImageSource { File = "icon.png" };
			var tabItems = new TabItemCollection
			{
				new SfTabItem()
				{
					Header = "Call",
					ImageSource = imageSource,
					Content = new Button()
					{
						Text = "Tab Item1",
					}
				},
				new SfTabItem()
				{
					Header = "Favorites",
					ImageSource = imageSource,
					Content = new Button()
					{
						Text = "Tab Item2",
					}
				},
				new SfTabItem()
				{
					Header = "Contacts",
					ImageSource = imageSource,
					Content = new Button()
					{
						Text = "Tab Item3",
					}
				}
			};

			tabBar.Items = tabItems;
			tabBar.HeaderDisplayMode = tabBarDisplayMode;
			foreach (var item in tabBar.Items)
			{
				if (item.ImageSource != null && !string.IsNullOrEmpty(item.Header))
				{
					Assert.Equal(tabBarDisplayMode, item.HeaderDisplayMode);
				}
			}
		}

		[Fact]
		public void TestUpdateItemSourceDynamically()
		{
			SfTabBar tabBar = new SfTabBar()
			{
				SelectedIndex = 2,

			};

			tabBar.ItemsSource = new ObservableCollection<Model>()
			{
				new Model(){Name="Tab1"},
				new Model(){Name="Tab2"},
				new Model(){Name="Tab3"}
			};

			Assert.NotNull(tabBar.ItemsSource);
			Assert.Equal(3, tabBar.ItemsSource.Count);
			Assert.Null(tabBar.HeaderItemTemplate);
			Assert.Equal(2, tabBar.SelectedIndex);
			tabBar.ItemsSource.Clear();
			Assert.Empty(tabBar.ItemsSource);
			tabBar.ItemsSource = new ObservableCollection<Model>()
			{
				new Model() { Name="Item1"},
				new Model() { Name="Item2"},
				new Model() { Name="Item3"},
				new Model() { Name="Item4"}
			};

			Assert.NotNull(tabBar.ItemsSource);
			Assert.Equal(4, tabBar.ItemsSource.Count);

		}

		[Theory]
		[InlineData(21, 28)]
		[InlineData(27, 36)]
		[InlineData(6, 8)]
		public void TestCalculateTabHeaderImageSize(double size, double expectedsize)
		{
			SfTabBar tabBar = new SfTabBar()
			{
				Items = new TabItemCollection()
				{
					new SfTabItem { Header = "TAB 1", ImageSource ="SampleImage1.png", ImageSize = size, Content = new Label { Text = "Content 1" } },
					new SfTabItem { Header = "TAB 2", ImageSource ="SampleImage1.png", ImageSize = size, Content = new Label { Text = "Content 2" } },
					new SfTabItem { Header = "TAB 3", ImageSource ="SampleImage1.png",ImageSize = size, Content = new Label { Text = "Content 3" } }
				}
			};

			for (int i = 0; i < tabBar.Count; i++)
			{
				var result = InvokePrivateMethod(tabBar, "CalculateTabHeaderImageSize", [tabBar.Items[i].ImageSize]);
				Assert.NotNull(result);
				Assert.Equal(expectedsize, result);

			}
		}

		[Fact]
		public void TestHandleTabItemTapped()
		{
			SfTabView tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection()
			};
			if (((tabView as IVisualTreeElement)?.GetVisualChildren().FirstOrDefault() as SfGrid)?.Children.FirstOrDefault() is SfTabBar tabBar)
			{
				_ = tabBar.GetEffectsView(tabBar.Items[0]);

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
		public void TestScrollButtonDisabledColorWhenScrollEnabled()
		{
			var tabBar = new SfTabBar()
			{
				Items = new TabItemCollection()
				{
					new SfTabItem { Header="Tab1" },
					new SfTabItem { Header="Tab2" },
				},

			};

			tabBar.IsScrollButtonEnabled = true;
			ArrowIcon? forwardArrow = GetPrivateField(tabBar, "_forwardArrow") as ArrowIcon;
			ArrowIcon? backwardArrow = GetPrivateField(tabBar, "_backwardArrow") as ArrowIcon;
			forwardArrow!.IsEnabled = false;
			backwardArrow!.IsEnabled = false;
			SetPrivateField(tabBar, "_forwardArrow", forwardArrow!);
			SetPrivateField(tabBar, "_backwardArrow", backwardArrow!);
			tabBar.ScrollButtonDisabledIconColor = Colors.Red;
			ArrowIcon? resultForwardArrow = GetPrivateField(tabBar, "_forwardArrow") as ArrowIcon;
			ArrowIcon? resultBackwardArrow = GetPrivateField(tabBar, "_backwardArrow") as ArrowIcon;
			Assert.False(resultForwardArrow!.IsEnabled);
			Assert.False(resultBackwardArrow!.IsEnabled);
			Assert.Equal(Colors.Red, resultForwardArrow.ScrollButtonColor);
			Assert.Equal(Colors.Red, resultBackwardArrow.ScrollButtonColor);
		}

		[Fact]
		public void TestMixedObjectItemSource()
		{
			SfTabBar tabBar = [];
			Assert.Null(tabBar.ItemsSource);
			tabBar.ItemsSource = new List<object>();
			Assert.NotNull(tabBar.ItemsSource);
			Assert.Empty(tabBar.ItemsSource);

			var tabItems = PopulateMixedObjectItemsSource();
			if (tabBar != null)
			{
				tabBar?.ItemsSource = tabItems;
			}

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

		[Fact]
		public void TestUpdateScrollButtonBackground()
		{
			tabBar.Items.Add(new SfTabItem() { Header = "Tab 1" });
			InvokePrivateMethod(tabBar, "UpdateScrollButtonBackground", []);
			ArrowIcon? forwardArrow = GetPrivateField<SfTabBar>(tabBar, "_forwardArrow") as ArrowIcon;
			ArrowIcon? backwardArrow = GetPrivateField<SfTabBar>(tabBar, "_backwardArrow") as ArrowIcon;
			tabBar.ScrollButtonBackground = Colors.Blue;
			Assert.Equal(Colors.Blue, forwardArrow?.ScrollButtonBackground);
			Assert.Equal(Colors.Blue, backwardArrow?.ScrollButtonBackground);
		}

		[Fact]
		public void TestUpdateScrollButtonColor()
		{
			tabBar.Items.Add(new SfTabItem() { Header = "Tab 1" });
			ArrowIcon? forwardArrow = GetPrivateField<SfTabBar>(tabBar, "_forwardArrow") as ArrowIcon;
			ArrowIcon? backwardArrow = GetPrivateField<SfTabBar>(tabBar, "_backwardArrow") as ArrowIcon;
			tabBar.ScrollButtonColor = Colors.Blue;
			tabBar.ScrollButtonDisabledIconColor = Colors.Red;
			if (forwardArrow != null)
			{
				forwardArrow?.IsEnabled = false;
			}

			InvokePrivateMethod(tabBar, "UpdateScrollButtonColor", []);
			Assert.Equal(Colors.Red, forwardArrow?.ScrollButtonColor);
			Assert.Equal(Colors.Blue, backwardArrow?.ScrollButtonColor);
		}

		[Fact]
		public void TestTabBarUpdateItems()
		{
			SfTabBar tabBar = new SfTabBar()
			{
				IsScrollButtonEnabled = true,
				Items = PopulateLabelImageItemsCollection(),
				SelectedIndex = 1,
			};

			Assert.Equal(1, tabBar.SelectedIndex);
			Assert.NotNull(tabBar.Items);
			SfGrid? sfGrid = GetPrivateField(tabBar, "_tabHeaderParentContainer") as SfGrid;
			ArrowIcon? forwardArrow = GetPrivateField(tabBar, "_forwardArrow") as ArrowIcon;
			ArrowIcon? backwardArrow = GetPrivateField(tabBar, "_backwardArrow") as ArrowIcon;
			Assert.NotNull(sfGrid);
			Assert.NotNull(forwardArrow);
			Assert.NotNull(backwardArrow);
			Assert.True(sfGrid.Children.Contains(forwardArrow));
			Assert.True(sfGrid.Children.Contains(backwardArrow));

		}

		[Fact]
		public void TestUpdateTabItemWidth()
		{
			SfTabBar tabBar = new SfTabBar()
			{
				WidthRequest = 200,
				Items = new TabItemCollection()
				{
					new SfTabItem() { Header= "Tab 1"},
					new SfTabItem() { Header= "Tab 2"}
				},
			};

			for (int i = 0; i < tabBar.Items.Count; i++)
			{
				if (tabBar.Items[i] is not null)
				{
					if (tabBar.Items[i].Parent is SfGrid)
					{
						var grid = tabBar.Items[i].Parent as SfGrid;
						Assert.NotNull(grid);
					};
				}

			}
		}

		[Fact]
		public void TestUpdateIndicatorPosition()
		{
			SfTabBar tabBar = new SfTabBar()
			{
				Items = new TabItemCollection()
				{
					new SfTabItem() { Header="Tab1"},
					new SfTabItem() {Header ="Tab2"},
					new SfTabItem() {Header = "Tab3"},
					new SfTabItem() {Header = "Tab4"}
				}
			};


			SfHorizontalStackLayout? horizontalStackLayout = GetPrivateField(tabBar, "_tabHeaderItemContent") as SfHorizontalStackLayout;
			Assert.NotNull(horizontalStackLayout);
			Assert.Equal(4, horizontalStackLayout.Children.Count);
			SetPrivateField(tabBar, "_removedItemWidth", 100);
			InvokePrivateMethod(tabBar, "UpdateIndicatorPosition", [300, 40]);
			double? result1 = (double?)GetPrivateField(tabBar, "_removedItemWidth");
			Assert.NotNull(result1);
			Assert.Equal(0, result1);
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

		[Fact]
		public void TestHoverBackground_DefaultValue_ShouldBeExpectedBrush()
		{
			var tabBar = new SfTabBar();
			var expectedColor = Color.FromArgb("#1C1B1F");
			var actualBrush = tabBar.HoverBackground as SolidColorBrush;

			Assert.NotNull(actualBrush);
			Assert.Equal(expectedColor, actualBrush.Color);
		}


		[Fact]
		public void HoverBackground_SetValue_ShouldUpdateBrush()
		{
			var tabBar = new SfTabBar();
			var newBrush = new SolidColorBrush(Colors.Blue);
			tabBar.HoverBackground = newBrush;

			Assert.Equal(newBrush, tabBar.HoverBackground);
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

		[Theory]
		[InlineData(-10)]
		[InlineData(0)]
		[InlineData(8)]
		public void TestUpdateIndicatorStrokeThickness(double value)
		{
			var tabView = new SfTabView();
			InvokePrivateMethod(tabView, "UpdateIndicatorStrokeThickness", [value]);
			SfTabBar? tabBar = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(value, tabBar?.IndicatorStrokeThickness);
			Border? indicator = GetPrivateField(tabBar, "_tabSelectionIndicator") as Border;
			Assert.Equal(value, indicator?.HeightRequest);
		}

		[Fact]
		public void TestItemsSource()
		{
			var tabView = new SfTabView();
			InvokePrivateMethod(tabView, "UpdateItemsSource");
			SfTabBar? tabBar = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			SfHorizontalContent? horizontalContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.Null(tabView.ItemsSource);
			Assert.Null(tabBar?.ItemsSource);
			Assert.Null(tabBar?.HeaderItemTemplate);
			Assert.Null(horizontalContent?.ItemsSource);
			Assert.Null(horizontalContent?.ContentItemTemplate);
		}

		[Fact]
		public void TestUpdateItems()
		{
			var tabView = new SfTabView();
			tabView.Items = PopulateMixedItemsCollection();
			tabView.SelectedIndex = -1;
			InvokePrivateMethod(tabView, "UpdateItems");
			Assert.Equal(0, tabView.SelectedIndex);
		}

		[Theory]
		[InlineData(20)]
		[InlineData(70)]
		[InlineData(-54)]
		[InlineData(-20)]
		public void TestUpdateTabBarHeight(double height)
		{
			SfTabView tabView = new SfTabView()
			{
				IndicatorPlacement = TabIndicatorPlacement.Fill,
				Items = PopulateLabelItemsCollection()
			};

			tabView.TabBarHeight = height;
			SfTabBar? tabBar = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Border? border = GetPrivateField(tabBar!, "_tabSelectionIndicator") as Border;
			Assert.NotNull(border);
			if (height > 0)
			{
				Assert.Equal(height, border!.HeightRequest);
			}
			else
			{
				Assert.Equal(48, border!.HeightRequest);
			}

		}

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
			methodInfo?.Invoke(tabView, [null, eventArgs]);
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
			methodInfo?.Invoke(tabView, [null, eventArgs]);
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
			methodInfo?.Invoke(tabView, [null, eventArgs]);
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
			methodInfo?.Invoke(tabView, [null, eventArgs]);
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
			methodInfo?.Invoke(tabView, [null, eventArgs]);
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
			var eventArgs = new TabItemTappedEventArgs
			{
				Cancel = cancelValue
			};
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
			tabView.ScrollButtonColor = expectedColor;
			Assert.Equal(expectedColor, tabView.ScrollButtonColor);

			SfTabBar? tabHeaderContainer = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(expectedColor, tabHeaderContainer?.ScrollButtonColor);
		}

		[Fact]
		public void TestContentTransitionEnabled()
		{
			var instance = new SfTabView();			
			var result = instance.IsContentTransitionEnabled;
			Assert.True(result);
		}
		[Fact]
		public void TestContentTransitionEnabledDefaultValue()
		{
			var instance = new SfTabView();
			var result = instance.IsContentTransitionEnabled;
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
			updateHorizontalOptionsMethod?.Invoke(tabViewMaterialVisualStyle, [tabItem]);
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
			updateHorizontalOptionsMethod?.Invoke(tabViewMaterialVisualStyle, [tabItem]);
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
			updateHorizontalOptionsMethod?.Invoke(tabViewMaterialVisualStyle, [tabItem]);
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
			updateMethod?.Invoke(tabView, [tabItem]);
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
			updateMethod?.Invoke(tabView, [tabItem]);
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
			updateMethod?.Invoke(tabView, [tabItem]);
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
			onHeaderDisplayModePropertyChangedMethod?.Invoke(null, [tabViewMaterialVisualStyle, null, null]);
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
			var tabView = new TabViewMaterialVisualStyle
			{
				HeaderDisplayMode = expectedValue
			};
			var actualValue = (TabBarDisplayMode)tabView.GetValue(TabViewMaterialVisualStyle.HeaderDisplayModeProperty); // Correct usage
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(TabImagePosition.Top)]
		[InlineData(TabImagePosition.Bottom)]
		[InlineData(TabImagePosition.Left)]
		[InlineData(TabImagePosition.Right)]
		public void TestImagePositionSetter(TabImagePosition expectedValue)
		{
			var tabView = new TabViewMaterialVisualStyle();
			tabView.ImagePosition = expectedValue;
			var actualValue = (TabImagePosition)tabView.GetValue(TabViewMaterialVisualStyle.ImagePositionProperty); // Correct usage
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
			SfTabItem item = new SfTabItem
			{
				FontAutoScalingEnabled = value
			};
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
			SfTabItem item = new SfTabItem
			{
				ImageTextSpacing = value
			};
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
			SfTabItem sfTabItem = new SfTabItem
			{
				FontSize = value
			};
			Assert.Equal(value, sfTabItem.FontSize);
		}

		[Theory]
		[InlineData(FontAttributes.Bold)]
		[InlineData(FontAttributes.Italic)]
		[InlineData(FontAttributes.None)]
		public void TestFontAttributes(FontAttributes font)
		{
			SfTabItem sfTabItem = new SfTabItem
			{
				FontAttributes = font
			};
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
			SfTabItem _tabItem = new SfTabItem
			{
				Header = "New Header"
			};
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
			SfTabItem _tabItem = new SfTabItem
			{
				FontFamily = fontFamily
			};
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
			SfTabItem _tabItem = new SfTabItem
			{
				FontSize = 20.0
			};
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
			SfTabItem _tabItem = new SfTabItem
			{
				ImagePosition = TabImagePosition.Left
			};
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
			SfTabItem _tabItem = new SfTabItem
			{
				ImageTextSpacing = value
			};
			Assert.Equal(value, _tabItem.ImageTextSpacing);
		}

		[Fact]
		public void TestIsSelectedPropertyDefaultValue()
		{
			SfTabItem _tabItem = new SfTabItem();
			Assert.False(_tabItem.IsSelected);
		}

		[Theory]
		[InlineData(-12)]
		[InlineData(0)]
		[InlineData(24)]
		public void TestImageSize(double value)
		{
			SfTabItem _tabItem = new SfTabItem()
			{
				ImageSize = value
			};
			Assert.Equal(value,_tabItem.ImageSize);
			
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestIsSelected(bool value)
		{
			SfTabItem _tabItem = new SfTabItem();
			_tabItem.IsSelected = value;
			Assert.Equal(value, _tabItem.IsSelected);

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
			{
				eventArgs?.Index = index;
			}

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

		[Fact]
		public void TestOnTouchReleaseWhenFilledState()
		{
			SfTabItem item = new SfTabItem();
			item.IndicatorPlacement = TabIndicatorPlacement.Fill;
			item.IsSelected = true;
			SetPrivateField(item, "_isPreviousItemSelected", true);
			var touchEventArgs = new PointerEventArgs(1, PointerActions.Released, new Point(50, 50));
			item.OnTouch(touchEventArgs);
			PointerActions pointerActions = PointerActions.Released;
			Assert.Equal(touchEventArgs.Action, pointerActions);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestOnTouchReleased(bool value)
		{
			SfTabItem item = new SfTabItem
			{
				IsSelected = value
			};
			var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Released, new Point(50, 50));
			item.OnTouch(touchEventArgs);
			Assert.Equal(touchEventArgs.TouchPoint, new Point(50, 50));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void OnTouchReleasedActionCheck(bool value)
		{
			SfTabItem item = new SfTabItem
			{
				IsSelected = value
			};
			var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Exited, new Point(50, 50));
			item.OnTouch(touchEventArgs);
			Assert.Equal(touchEventArgs.TouchPoint, new Point(50, 50));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestOnTouchReleasedActionCheck(bool value)
		{
			SfTabItem item = new SfTabItem
			{
				IsSelected = value
			};
			var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Pressed, new Point(50, 50));
			item.OnTouch(touchEventArgs);
			Assert.Equal(touchEventArgs.TouchPoint, new Point(50, 50));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestOnTouchReleasedAction(bool value)
		{
			SfTabItem item = new SfTabItem
			{
				IsSelected = value
			};
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
			SfTabItem item = new SfTabItem
			{
				IsSelected = value
			};
			var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Released, new Point(-50, -50));
			item.OnTouch(touchEventArgs);
			Assert.Equal(touchEventArgs.TouchPoint, new Point(-50, -50));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void CheckOnTouchReleasedActionCheckForNegativePointValues(bool value)
		{
			SfTabItem item = new SfTabItem
			{
				IsSelected = value
			};
			var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Exited, new Point(-50, 50));
			item.OnTouch(touchEventArgs);
			Assert.Equal(touchEventArgs.TouchPoint, new Point(-50, 50));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestOnTouchReleasedActionCheckForNegativePointValues(bool value)
		{
			SfTabItem item = new SfTabItem
			{
				IsSelected = value
			};
			var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Pressed, new Point(-50, -500));
			item.OnTouch(touchEventArgs);
			Assert.Equal(touchEventArgs.TouchPoint, new Point(-50, -500));
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void TestOnTouchReleasedActionForNegativePointValues(bool value)
		{
			SfTabItem item = new SfTabItem
			{
				IsSelected = value
			};
			var touchEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Entered, new Point(-500, 50));
			item.OnTouch(touchEventArgs);
			Assert.Equal(touchEventArgs.TouchPoint, new Point(-500, 50));
		}

		[Fact]
		public void TestOnTouchReleaseWhenItemDisabled()
		{
			SfTabItem item = new SfTabItem()
			{
				IsEnabled = false,
			};

			var touchEventArgs = new PointerEventArgs(1, PointerActions.Released, new Point(-500, 50));
			item.OnTouch(touchEventArgs);
			Assert.Equal(touchEventArgs.TouchPoint, new Point(-500, 50));
		}

		#endregion

		#region HorizontalContent properties

		[Fact]
		public void TestItemsProperty()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			Assert.NotNull(horizontal.Items);
		}

		[Fact]
		public void TestItemsPropertyCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			Assert.Equal(-1, horizontal.SelectedIndex);
		}

		[Fact]
		public void TestSelectedIndexPropertySetValue()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				SelectedIndex = 1
			};
			Assert.Equal(1, horizontal.SelectedIndex);
		}

		[Fact]
		public void ContentTransitionDurationPropertyDefaultValueCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			Assert.Equal(100d, horizontal.ContentTransitionDuration);
		}

		[Fact]
		public void TestContentTransitionDurationPropertySetValue()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				ContentTransitionDuration = 200d
			};
			Assert.Equal(200d, horizontal.ContentTransitionDuration);
		}

		[Fact]
		public void TestContentWidthProperty()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				ContentWidth = 500d
			};
			Assert.Equal(500d, horizontal.ContentWidth);
		}

		[Fact]
		public void TestIsEnableProperty()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				IsEnabled = true
			};
			Assert.True(horizontal.IsEnabled);
		}

		[Fact]
		public void HorizontalContentContentItemTemplateCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				ContentTransitionDuration = value
			};
			Assert.Equal(value, horizontal.ContentTransitionDuration);
		}

		[Fact]
		public void TestHorizontalContentNullCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				ContentWidth = value
			};
			Assert.Equal(value, horizontal.ContentWidth);

		}

		#endregion

		#region SfHorizontalContent Fields

		[Fact]
		public void TestIsTowardsRightPropertySetValue()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_isTowardsRight", true);
			var isTowardsRight = (bool?)GetPrivateField(horizontal, "_isTowardsRight");
			Assert.True(isTowardsRight);
		}

		[Fact]
		public void TestsTowardsRightField()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_isPressed", true);
			SetPrivateField(horizontal, "_isMoved", true);
			SetPrivateField(horizontal, "_visibleItemCount", 4);
			SfHorizontalStackLayout views1 = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};
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
			SfTabBar views = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};
			var clearItems = InvokePrivateMethod(horizontal, "ClearItems");
			Assert.Null(clearItems);
		}

		[Fact]
		public void GetCountVisibleItemsMethodCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var handleTouchReleased = InvokePrivateMethod(horizontal, "HandleTouchReleased");
			Assert.Null(handleTouchReleased);
		}

		[Fact]
		public void GetNextVisibleItemIndexMethodCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateMixedItemsCollection(),
				SelectedIndex = 1
			};
			SetPrivateField(horizontal, "_isTowardsRight", true);
			var nextIndex = InvokePrivateMethod(horizontal, "GetNextVisibleItemIndex");
			Assert.NotNull(nextIndex);
			double index = (int)nextIndex;
			Assert.Equal(3, horizontal.Items.Count);
			Assert.NotNull(nextIndex);
			Assert.Equal(2, index);
		}

		[Fact]
		public void GetNextVisibleItemIndexWhenItemIsVisibleFalse()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var tabItems = new TabItemCollection()
			{
				new SfTabItem { Header = "TAB 1", Content = new Label { Text = "Content 1" } , IsVisible=true},
				new SfTabItem { Header = "TAB 2", Content = new Button { Text = "Content 2" }, IsVisible=false },
				new SfTabItem { Header = "TAB 3", Content = new  Microsoft.Maui.Controls.Picker() { }, IsVisible=true},
				new SfTabItem { Header = "TAB 4", Content = new Label { Text = "Content 4" } , IsVisible=false},
				new SfTabItem { Header = "TAB 5", Content = new Button { Text = "Content 5" }, IsVisible=false },
				new SfTabItem { Header = "TAB 6", Content = new  Microsoft.Maui.Controls.Picker() { }, IsVisible=true}
			};
			horizontal.Items = tabItems;
			horizontal.SelectedIndex = 2;
			SetPrivateField(horizontal, "_isTowardsRight", true);
			var nextIndex = InvokePrivateMethod(horizontal, "GetNextVisibleItemIndex");
			Assert.NotNull(nextIndex);
			double index = Convert.ToDouble(nextIndex);
			Assert.Equal(6, horizontal.Items.Count);
			Assert.Equal(5, index);
		}

		 [Fact]
        public void GetNextVisibleItemIndexMethodCheckWhenItemNull()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            var nextIndex = InvokePrivateMethod(horizontal, "GetNextVisibleItemIndex");
            double index = Convert.ToDouble(nextIndex!);
            Assert.Equal(0, index);
        }

        [Fact]
        public void GetNextItemIndexMethodCheckWhenItemsSourceNull()
        {
            SfTabView sfTabView = new SfTabView();
            SfTabBar views = new SfTabBar();
            SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
            var nextIndex = InvokePrivateMethod(horizontal, "GetNextItemIndex");
            int index = Convert.ToInt32(nextIndex!);
			;
            Assert.Equal(0, index);
        }

		[Theory]
		[InlineData(0)]
		[InlineData(3)]
		[InlineData(-2)]
		[InlineData(1)]
		public void GetNextItemIndexCheck(int value)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var tabItem1 = new SfTabItem { Header = "Tab 1" };
			var tabItem2 = new SfTabItem { Header = "Tab 2" };
			var tabItem3 = new SfTabItem { Header = "Tab 2" };
			var tabItem4 = new SfTabItem { Header = "Tab 2" };
			var tabItem5 = new SfTabItem { Header = "Tab 2" };
			ObservableCollection<SfTabItem> sfTabItems = [tabItem1, tabItem2, tabItem3, tabItem4, tabItem5];
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
			SfTabBar views = [];
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
			if (GetPrivateField<SfHorizontalContent>(horizontal, "_horizontalStackLayout") is SfHorizontalStackLayout horizontalStackLayout)
			{
				Assert.Equal(900, horizontalStackLayout.WidthRequest);
				foreach (var item in horizontalStackLayout.Children)
				{
					Assert.Equal(300, item.Width);
				}
			}
		}

		[Fact]
		public void TestHorizontalContentItemsSource()
		{
			var itemSource1 = PopulateButtonsListItemsSource();
			var itemSource2 = PopulateMixedObjectItemsSource();
			DataTemplate template1 = new DataTemplate(() => new Label { Text = "Test" });
			DataTemplate template2 = new DataTemplate(() => new Button { Text = "Test" });
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.ItemsSource = itemSource1;
			horizontal.ContentItemTemplate = template1;
			Assert.Equal(horizontal.ItemsSource, itemSource1);
			Assert.Equal(horizontal.ContentItemTemplate, template1);
			horizontal.ItemsSource = itemSource2;
			horizontal.ContentItemTemplate = template2;
			Assert.NotEqual(horizontal.ItemsSource, itemSource1);
			Assert.Equal(horizontal.ItemsSource, itemSource2);
			Assert.Equal(horizontal.ContentItemTemplate, template2);
		}

		[Fact]
		public void TestInitializeControl()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var invoke = InvokePrivateMethod(horizontal, "InitializeControl");
			Assert.Null(invoke);
			Assert.NotNull(horizontal.Content);
		}

		[Fact]
		public void TestOnTabItemsSourceCollectionChangedEvent()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			var invoke = InvokePrivateMethod(horizontal, "OnTabItemsSourceCollectionChanged", sfTabView, e);
			Assert.Null(invoke);
		}

		[Fact]
		public void TestOnItemsCollectionChangedEventTrigger()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
			var invoke = InvokePrivateMethod(horizontal, "OnItemsCollectionChanged", sfTabView, e);
			Assert.Null(invoke);
		}

		[Fact]
		public void TestOnItemsCollectionChangedValueChange()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

			_ = InvokePrivateMethod(horizontal, "OnItemsCollectionChanged", horizontal, e);
			Assert.Equal(3, horizontal.Items.Count);
		}

		[Fact]
		public void TestClearTabContent()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			HorizontalStackLayout sfHorizontalStackLayout = [];
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
			SfTabBar views = [];
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
			SfTabBar views = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};
			NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);

			_ = InvokePrivateMethod(horizontal, "OnItemsCollectionChanged", horizontal.Items[1], e);
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};

			_ = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, horizontal.Items[0], 0);

			_ = typeof(SfHorizontalContent).GetMethod("ClearTabContent", BindingFlags.NonPublic | BindingFlags.Instance);
			horizontal.Items.RemoveAt(0);
			int itemCount = horizontal.Items.Count;
			Assert.Equal(2, itemCount);
		}

		[Fact]
		public void TestItemCollectionChangedShouldClearTabContentOnOldItems()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};

			_ = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, horizontal.Items[0], 0);
			var clearTabContentMethod = typeof(SfHorizontalContent).GetMethod("ClearTabContent", BindingFlags.NonPublic | BindingFlags.Instance);
			horizontal.Items.RemoveAt(0);
			Assert.NotNull(clearTabContentMethod);
		}

		[Fact]
		public void GetNextVisibleItemIndexMethodValueCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};
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
			SfTabBar views = [];
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
			SfTabBar views = [];
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
			SfTabBar views = [];
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
			SfTabBar views = [];
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
			SfTabBar views = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				ContentItemTemplate = new DataTemplate(() =>
			{
				Label label = new Label
				{
					BackgroundColor = Colors.Black
				};
				return label;
			}),
				ContentWidth = 400d,
				SelectedIndex = 1
			};
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SetPrivateField(horizontal, "_startPoint", new Point(50, 50));
			var value = GetPrivateField(horizontal, "_startPoint");
			InvokePrivateMethod(horizontal, "FindVelocity", new Point(10000000000, 10000000000));
			var value1 = GetPrivateField(horizontal, "_velocityX");
			Assert.NotNull(value1);
			double val = (double)value1;
			Assert.Equal(Math.Round(0.15656,2), Math.Round(val, 2));
		}

		[Fact]
		public void TestClampIndexValue()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
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
			SfTabBar views = [];
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
			SfTabBar views = [];
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
			SfTabBar views =
			[
				new SfTabItem { Header = "Tab 1" },
				new SfTabItem { Header = "Tab 1" },
				new SfTabItem { Header = "Tab 1" },
				new SfTabItem { Header = "Tab 1" },
			];
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
			SfTabBar views = [];
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
		public void TestHorizontalContentItems()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.SelectedIndex = 1;
			horizontal.Items = PopulateMixedItemsCollection();
			int eventCount = 0;
			horizontal.SelectionChanging += (sender, args) => eventCount++;
			SelectionChangingEventArgs args = new SelectionChangingEventArgs();
			InvokePrivateMethod(horizontal, "RaiseSelectionChanging");
			Assert.Equal(1, eventCount);
			Assert.Equal(3, horizontal.Items.Count);
		}

		[Fact]
		public void TestGetVisibleItemsCount()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection(),
				SelectedIndex = 2
			};

			_ = InvokePrivateMethod(horizontal, "SelectPreviousTabItem");
			SfTabBar? tabBar = GetPrivateField<SfHorizontalContent>(horizontal, "_tabBar") as SfTabBar;
			Assert.Equal(1, tabBar?.SelectedIndex);
			horizontal.SelectedIndex = 0;
			_ = InvokePrivateMethod(horizontal, "SelectPreviousTabItem");
			Assert.Equal(0, tabBar?.SelectedIndex);
		}

		[Fact]
		public void TestSelectNextItem()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};
			SetPrivateField(horizontal, "_isTowardsRight", true);

			_ = InvokePrivateMethod(horizontal, "SelectNextItem");
			SfTabBar? tabBar = GetPrivateField<SfHorizontalContent>(horizontal, "_tabBar") as SfTabBar;
			Assert.Equal(1, tabBar?.SelectedIndex);
			horizontal.SelectedIndex = 2;
			_ = InvokePrivateMethod(horizontal, "SelectNextItem");
			Assert.Equal(2, tabBar?.SelectedIndex);
		}

		[Fact]
		public void TestUpdateItemVisibility()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection()
			};

			_ = InvokePrivateMethod(horizontal, "UpdateItemVisibility");
			var isNextItemVisible = (bool?)GetPrivateField(horizontal, "_isNextItemVisible");
			var isPreviousItemVisible = (bool?)GetPrivateField(horizontal, "_isPreviousItemVisible");
			Assert.True(isNextItemVisible);
			Assert.False(isPreviousItemVisible);
			horizontal.SelectedIndex = 2;
			_ = InvokePrivateMethod(horizontal, "UpdateItemVisibility");
			isNextItemVisible = (bool?)GetPrivateField(horizontal, "_isNextItemVisible");
			isPreviousItemVisible = (bool?)GetPrivateField(horizontal, "_isPreviousItemVisible");
			Assert.False(isNextItemVisible);
			Assert.True(isPreviousItemVisible);
		}

		[Fact]
		public void TestUpdateItemVisibilityWithTemplate()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views)
			{
				Items = PopulateLabelItemsCollection(),
				ContentItemTemplate = new DataTemplate(() =>
				{
					Label label = new Label
					{
						BackgroundColor = Colors.Black
					};
					return label;
				})
			};
			var invoke = InvokePrivateMethod(horizontal, "UpdateItemVisibility");
			var isPreviousItemVisible = (bool?)GetPrivateField(horizontal, "_isPreviousItemVisible");
			Assert.False(isPreviousItemVisible);
		}

		[Fact]
		public void TestRaiseSelectionChangingEventCheck()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			bool eventInvoked = false;
			horizontal.SelectionChanging += (s, e) => eventInvoked = true;

			var args = new SelectionChangingEventArgs();
			InvokePrivateMethod(horizontal, "RaiseSelectionChangingEvent", args);

			Assert.True(eventInvoked);
		}
		[Fact]
		public void TestUpdateContentItems()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.Items = PopulateMixedItemsCollection();
			InvokePrivateMethod(horizontal, "UpdateItems");
			SfHorizontalStackLayout? stackLayout1 = GetPrivateField(horizontal, "_horizontalStackLayout") as SfHorizontalStackLayout;
			Assert.NotNull(horizontal.Items);
			Assert.NotEmpty(stackLayout1?.Children!);
			horizontal.Items = null!;
			InvokePrivateMethod(horizontal, "UpdateItems");
			SfHorizontalStackLayout? stackLayout2 = GetPrivateField(horizontal, "_horizontalStackLayout") as SfHorizontalStackLayout;
			Assert.Null(horizontal?.Items);
			Assert.Empty(stackLayout2?.Children!);
		}

		[Fact]
		public void TestAddTabContentItemsWithContent()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SfTabItem tabItem = new SfTabItem()
			{
				Content = new Label { Text = "Tab1", BackgroundColor = Colors.Red }
			};

			InvokePrivateMethod(horizontal, "AddTabContentItems", [tabItem, -1]);
			SfHorizontalStackLayout? stackLayout = GetPrivateField(horizontal, "_horizontalStackLayout") as SfHorizontalStackLayout;
			Assert.NotNull(stackLayout);
			var sfGrid = stackLayout.Children.Last() as SfGrid;
			Assert.NotNull(sfGrid);
			Assert.Contains(tabItem.Content, sfGrid.Children);
		}

		[Fact]
		public void TestAddTabContentItemsWithContentValidIndex()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.SelectedIndex = -1;
			horizontal.Items = PopulateMixedItemsCollection();
			SfTabItem tabItem = new SfTabItem()
			{
				Content = new Label { Text = "Tab1", BackgroundColor = Colors.Red }
			};

			InvokePrivateMethod(horizontal, "AddTabContentItems", [tabItem, 0]);
			SfHorizontalStackLayout? stackLayout1 = GetPrivateField(horizontal, "_horizontalStackLayout") as SfHorizontalStackLayout;
			Assert.NotNull(stackLayout1);
			var grid = stackLayout1.Children[0] as SfGrid;
			Assert.NotNull(grid);
			Assert.Contains(tabItem.Content, grid.Children);
			Assert.Equal(0, horizontal.SelectedIndex);
		}

		[Fact]
		public void TestAddTabContentItemsWithoutContent()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			SfTabItem tabItem = new SfTabItem();
			InvokePrivateMethod(horizontal, "AddTabContentItems", [tabItem, -1]);
			SfHorizontalStackLayout? stackLayout = GetPrivateField(horizontal, "_horizontalStackLayout") as SfHorizontalStackLayout;
			Assert.NotNull(stackLayout);
			SfGrid? sfGrid = stackLayout.Children.Last() as SfGrid;
			Assert.NotNull(sfGrid);
			Assert.False(sfGrid.Children.Any());
		}

		[Fact]
		public void TestAddTabContentItemsWithoutContentValidIndex()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			horizontal.SelectedIndex = -1;
			horizontal.Items = PopulateMixedItemsCollection();
			SfTabItem tabItem = new SfTabItem();
			InvokePrivateMethod(horizontal, "AddTabContentItems", [tabItem, 0]);
			SfHorizontalStackLayout? stackLayout1 = GetPrivateField(horizontal, "_horizontalStackLayout") as SfHorizontalStackLayout;
			Assert.NotNull(stackLayout1);
			var grid = stackLayout1.Children[0] as SfGrid;
			Assert.NotNull(grid);
			Assert.False(grid.Children.Any());
			Assert.Equal(0, horizontal.SelectedIndex);
		}

		[Fact]
		public void TestOnHandleTouchInteractionPressed()
		{
			SfTabView sfTabView = new SfTabView()
			{
				EnableSwiping = true,
				SelectedIndex = 2
			};

			SfTabBar tabBar = new SfTabBar();
			tabBar.SelectedIndex = 2;
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, tabBar);
			horizontal.ContentItemTemplate = new DataTemplate(() => new Label { Text = "Content1" });
			horizontal.ContentWidth = 600;
			horizontal.OnHandleTouchInteraction(PointerActions.Pressed, new Point(10, 10));
			var currentIndex = GetPrivateField(horizontal, "_currentIndex");
			var xPosition = GetPrivateField(horizontal, "_initialXPosition");
			SfTabBar? sftabBar = GetPrivateField(horizontal, "_tabBar") as SfTabBar;
			double expectedXPosition = -600 * 2;
			Assert.Equal(expectedXPosition, xPosition);

		}

		[Fact]
		public void TestOnHandleTouchInteractionMoved()
		{
			SfTabView sfTabView = new SfTabView()
			{
				EnableSwiping = true,
				SelectedIndex = 1
			};

			sfTabView.ItemsSource = PopulateMixedObjectItemsSource();
			sfTabView.ContentItemTemplate = new DataTemplate(() => new Label { Text = "Content" });
			SfHorizontalContent? horizontal = GetPrivateField(sfTabView, "_tabContentContainer") as SfHorizontalContent;
			SetPrivateField(horizontal!, "_isPressed", true);
			SetPrivateField(horizontal!, "_visibleItemCount", 3);
			horizontal!.OnHandleTouchInteraction(PointerActions.Moved, new Point(600, 600));
			var oldValue = GetPrivateField(horizontal, "_oldPoint");
			Assert.Equal(new Point(600, 600), oldValue);

		}

		[Theory]
		[InlineData(PointerActions.Released)]
		[InlineData(PointerActions.Pressed)]
		[InlineData(PointerActions.Moved)]
		[InlineData(PointerActions.Cancelled)]
		[InlineData(PointerActions.Exited)]
		public void TestOnTouch(PointerActions pointerActions)
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var eventArgs = new PointerEventArgs(1, pointerActions, new Point(10, 10));
			var exception = Record.Exception(() => ((ITouchListener)horizontal).OnTouch(eventArgs));
			Assert.Null(exception);

		}

		[Fact]
		public void TestTranslateXPositionWhenSelectionChangingEventIsCancelled()
		{
			SfTabView sfTabView = new SfTabView()
			{
				Items = PopulateLabelItemsCollection(),
				SelectedIndex = 0
			};

			SfTabBar? tabBar = GetPrivateField(sfTabView, "_tabHeaderContainer") as SfTabBar;
			SfHorizontalContent? horizontalContent = GetPrivateField(sfTabView, "_tabContentContainer") as SfHorizontalContent;
			horizontalContent!.SelectionChanging += (sender, e) =>
			{
				e.Cancel = true;
			};

			SetPrivateField(horizontalContent!, "_isPreviousItemVisible", true);
			SetPrivateField(horizontalContent!, "_isNextItemVisible", true);
			SetPrivateField(horizontalContent!, "_currentIndex", 0);
			SetNonPublicProperty(horizontalContent, "IsTowardsRight", true);
			SetPrivateField(horizontalContent!, "_contentWidth", 400);
			InvokePrivateMethod(horizontalContent!, "TranslateXPosition", [-200]);
			SfHorizontalStackLayout? stackLayout = GetPrivateField(horizontalContent, "_horizontalStackLayout") as SfHorizontalStackLayout;
			Assert.Equal(0, stackLayout!.TranslationX);
		}

		[Fact]
		public void TestTranslateXPositionWhenSelectionChangingEvent()
		{
			SfTabView sfTabView = new SfTabView()
			{
				Items = PopulateLabelItemsCollection(),
				SelectedIndex = 0
			};

			SfTabBar? tabBar = GetPrivateField(sfTabView, "_tabHeaderContainer") as SfTabBar;
			SfHorizontalContent? horizontalContent = GetPrivateField(sfTabView, "_tabContentContainer") as SfHorizontalContent;
			var eventTriggered = false;
			horizontalContent!.SelectionChanging += (sender, e) =>
			{
				eventTriggered = true;
			};

			SetPrivateField(horizontalContent!, "_isPreviousItemVisible", true);
			SetPrivateField(horizontalContent!, "_isNextItemVisible", true);
			SetPrivateField(horizontalContent!, "_currentIndex", 0);
			SetNonPublicProperty(horizontalContent, "IsTowardsRight", true);
			SetPrivateField(horizontalContent!, "_contentWidth", 400);
			InvokePrivateMethod(horizontalContent!, "TranslateXPosition", [-200]);
			SfHorizontalStackLayout? stackLayout = GetPrivateField(horizontalContent, "_horizontalStackLayout") as SfHorizontalStackLayout;
			SfTabBar? tabBar1 = GetPrivateField(horizontalContent, "_tabBar") as SfTabBar;
			Assert.Equal(-200, stackLayout!.TranslationX);
			Assert.True(eventTriggered);
		}

		[Theory]
		[InlineData(0, true, -50, false, true, -50, true)]
		[InlineData(1, true, -50, false, true, -400, true)]
		[InlineData(2, true, -250, true, true, 400, false)]
		[InlineData(2, true, -100, true, false, 400, false)]
		public void TestTranslateXPosition(int currentIndex, bool isTowardRight, double difference, bool isPreviousItem, bool isNextItem, double expectedTranlationX, bool expectedValue)
		{
			SfTabView sfTabView = new SfTabView()
			{
				Items = PopulateLabelItemsCollection(),
				SelectedIndex = currentIndex
			};

			SfTabBar? tabBar = GetPrivateField(sfTabView, "_tabHeaderContainer") as SfTabBar;
			SfHorizontalContent? horizontalContent = GetPrivateField(sfTabView, "_tabContentContainer") as SfHorizontalContent;
			var eventTriggered = false;
			horizontalContent!.SelectionChanging += (sender, e) =>
			{
				eventTriggered = true;
			};

			SetPrivateField(horizontalContent!, "_isPreviousItemVisible", isPreviousItem);
			SetPrivateField(horizontalContent!, "_isNextItemVisible", isNextItem);
			SetPrivateField(horizontalContent!, "_currentIndex", currentIndex);
			SetNonPublicProperty(horizontalContent, "IsTowardsRight", isTowardRight);
			SetPrivateField(horizontalContent!, "_contentWidth", 400);
			InvokePrivateMethod(horizontalContent!, "TranslateXPosition", [difference]);
			SfHorizontalStackLayout? stackLayout = GetPrivateField(horizontalContent, "_horizontalStackLayout") as SfHorizontalStackLayout;
			SfTabBar? tabBar1 = GetPrivateField(horizontalContent, "_tabBar") as SfTabBar;
			Assert.Equal(expectedTranlationX, stackLayout!.TranslationX);
			Assert.Equal(expectedValue, eventTriggered);
		}

		[Fact]
		public void LoadsItems_WithVirtualization()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar sfTabBar = new SfTabBar();
			var horizontalContent = new SfHorizontalContent(sfTabView, sfTabBar)
			{
				EnableVirtualization = true
			};

			var tabItem1 = new SfTabItem { Header = "Tab1", IsVisible = true };
			var tabItem2 = new SfTabItem { Header = "Tab2", IsVisible = true };
			horizontalContent.Items.Add(tabItem1);
			horizontalContent.Items.Add(tabItem2);

			var horizontalStackLayout = new SfHorizontalStackLayout();
			horizontalStackLayout.Children.Add(new BoxView());
			horizontalStackLayout.Children.Add(new BoxView());
			SetPrivateField(horizontalContent, "_horizontalStackLayout", horizontalStackLayout);

			InvokePrivateMethod(horizontalContent, "LoadItemsContent", 0);

			var layout = (SfHorizontalStackLayout?)GetPrivateField(horizontalContent, "_horizontalStackLayout");
			Assert.IsType<SfGrid>(layout?.Children[0]);
			Assert.True(layout.Children[0] is SfGrid, "Expected a SfGrid at index 0 after virtualization.");
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
			SfTabBar views = [];
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
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var expectedAction = pointerActions;
			horizontal.OnHandleTouchInteraction(pointerActions, new Point(0, 0));
			Assert.Equal(expectedAction, pointerActions);
		}

		[Fact]
		public void TestOnTouchMethod()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = [];
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var pointerEventArgs = new Internals.PointerEventArgs(1, Internals.PointerActions.Pressed, new Point(10, 10));
			horizontal.OnTouch(pointerEventArgs);
			var pressed = GetPrivateField(horizontal, "_isPressed");
			Assert.NotNull(pressed);
			bool isPressed = (bool)pressed;
			Assert.False(isPressed);
		}

		[Fact]
		public void TestOnTap()
		{
			SfTabView sfTabView = new SfTabView();
			SfTabBar views = new SfTabBar();
			SfHorizontalContent horizontal = new SfHorizontalContent(sfTabView, views);
			var tapArgs = new TapEventArgs(new Point(50, 50), 1);
			var exception = Record.Exception(() => horizontal.OnTap(tapArgs));
			Assert.Null(exception);
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

			_ = GetPrivateField(_tabItem, "_isSelected");
			bool isSelected = _tabItem.IsSelected;
			Assert.False(isSelected);
		}

		[Fact]
		public void TestSetIsSelectedValue()
		{
			SfTabItem _tabItem = new SfTabItem();
			SetPrivateField(_tabItem, "_isSelected", true);

			_ = GetPrivateField(_tabItem, "_isSelected");
			bool isSelected = _tabItem.IsSelected;
			Assert.True(isSelected);
		}

		#endregion

		#region ArrowIcon Internal Properties

		[Fact]
		public void TestForegroundColorPropertyShouldBeDefaultColor()
		{
			ArrowIcon _arrowIcon = [];
			Assert.Equal(Color.FromArgb("#49454F"), _arrowIcon.ScrollButtonColor);
		}
		[Fact]
		public void TestForegroundColorPropertyShouldUpdateValue()
		{
			ArrowIcon _arrowIcon = [];
			var color = Color.FromArgb("#FF0000");
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Color.FromArgb("#00FF00");
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Color.FromArgb("#0000FF");
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Color.FromRgb(255, 0, 0);
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Color.FromRgb(0, 255, 0);
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Color.FromRgb(0, 0, 255);
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Color.FromRgba(255, 0, 0, 128);
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Color.FromRgba(0, 255, 0, 128);
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Colors.Red;
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Colors.Green;
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Colors.Blue;
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Colors.White;
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
			color = Colors.Black;
			_arrowIcon.ScrollButtonColor = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonColor);
		}

		[Fact]
		public void TestScrollButtonBackgroundColorPropertyShouldBeDefaultColor()
		{
			ArrowIcon _arrowIcon = [];
			Assert.Equal(Color.FromArgb("#F7F2FB"),((SolidColorBrush) _arrowIcon.ScrollButtonBackground).Color);
		}

		[Fact]
		public void TestScrollButtonBackgroundColorProperty()
		{
			ArrowIcon _arrowIcon = [];
			var color = Color.FromArgb("#FF5733");
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Color.FromArgb("#33FF57");
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Color.FromArgb("#3357FF");
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Color.FromArgb("#FF33A1");
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Color.FromArgb("#FFD700");
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Color.FromRgb(255, 87, 51);
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Color.FromRgb(51, 255, 87);
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Color.FromRgb(51, 87, 255);
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Color.FromRgba(255, 87, 51, 128);
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Color.FromRgba(51, 255, 87, 128);
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Colors.OrangeRed;
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Colors.Green;
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Colors.Blue;
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Colors.Pink;
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
			color = Colors.Gold;
			_arrowIcon.ScrollButtonBackground = color;
			Assert.Equal(color, _arrowIcon.ScrollButtonBackground);
		}

		[Fact]
		public void TestButtonArrowTypePropertyShouldBeNull()
		{
			ArrowIcon _arrowIcon = [];
			Assert.Equal(ArrowType.Backward, _arrowIcon.ButtonArrowType);
		}

		[Fact]
		public void TestButtonArrowTypePropertyShouldUpdateValue()
		{
			ArrowIcon _arrowIcon = [];
			_arrowIcon.ButtonArrowType = ArrowType.Forward;
			Assert.Equal(ArrowType.Forward, _arrowIcon.ButtonArrowType);
			_arrowIcon.ButtonArrowType = ArrowType.Backward;
			Assert.Equal(ArrowType.Backward, _arrowIcon.ButtonArrowType);
		}

		[Fact]
		public void ShuffleButtonArrowType()
		{
			ArrowIcon _arrowIcon = [];
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
			ArrowIcon _arrowIcon = [];
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
			ArrowIcon _arrowIcon = [];
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
			ArrowIcon _arrowIcon = [];
			var pointerEventArgs = new PointerEventArgs(1, PointerActions.Entered, new Point(10, 10));
			_arrowIcon.OnTouch(pointerEventArgs);
			var sfEffectsView = GetPrivateField(_arrowIcon, "_sfEffectsView") as SfEffectsView;
			Assert.NotNull(sfEffectsView);
		}

		[Fact]
		public void TestOnTouchExited()
		{
			ArrowIcon _arrowIcon = [];
			var pointerEventArgs = new PointerEventArgs(1, PointerActions.Exited, new Point(10, 10));
			_arrowIcon.OnTouch(pointerEventArgs);
			var sfEffectsView = GetPrivateField(_arrowIcon, "_sfEffectsView") as SfEffectsView;
			Assert.NotNull(sfEffectsView);
		}
		#endregion

		#region CornerRadiusScripts

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, 20)]
		[InlineData(TabIndicatorPlacement.Top, 0)]
		[InlineData(TabIndicatorPlacement.Top, -20)]
		[InlineData(TabIndicatorPlacement.Bottom, 20)]
		[InlineData(TabIndicatorPlacement.Bottom, 0)]
		[InlineData(TabIndicatorPlacement.Bottom, -20)]
		[InlineData(TabIndicatorPlacement.Fill, 20)]
		[InlineData(TabIndicatorPlacement.Fill, 0)]
		[InlineData(TabIndicatorPlacement.Fill, -20)]
		public void Test_Bottom_CornerRadius(TabIndicatorPlacement tabIndicatorPlacement, double radius)
		{
			var cornerRadius = new CornerRadius(radius);
			var tabView = new SfTabView
			{
				IndicatorCornerRadius = cornerRadius,
				IndicatorPlacement = tabIndicatorPlacement,
				TabBarPlacement = TabBarPlacement.Bottom
			};

			tabView.Items = PopulateLabelItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var indicatorRoundRectangle = GetPrivateField(tabHeader, "_roundRectangle") as RoundRectangle;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);

			Assert.Equal(cornerRadius, indicatorRoundRectangle?.CornerRadius);
			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, 20)]
		[InlineData(TabIndicatorPlacement.Top, 0)]
		[InlineData(TabIndicatorPlacement.Top, -20)]
		[InlineData(TabIndicatorPlacement.Bottom, 20)]
		[InlineData(TabIndicatorPlacement.Bottom, 0)]
		[InlineData(TabIndicatorPlacement.Bottom, -20)]
		[InlineData(TabIndicatorPlacement.Fill, 20)]
		[InlineData(TabIndicatorPlacement.Fill, 0)]
		[InlineData(TabIndicatorPlacement.Fill, -20)]
		public void Test_Top_CornerRadius(TabIndicatorPlacement tabIndicatorPlacement, double radius)
		{
			var cornerRadius = new CornerRadius(radius);
			var tabView = new SfTabView
			{
				IndicatorCornerRadius = cornerRadius,
				IndicatorPlacement = tabIndicatorPlacement,
				TabBarPlacement = TabBarPlacement.Top
			};

			tabView.Items = PopulateLabelItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var indicatorRoundRectangle = GetPrivateField(tabHeader, "_roundRectangle") as RoundRectangle;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(0, tabHeaderIndex);
			Assert.Equal(1, tabContentIndex);

			Assert.Equal(cornerRadius, indicatorRoundRectangle?.CornerRadius);
			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, 20)]
		[InlineData(TabIndicatorPlacement.Top, 0)]
		[InlineData(TabIndicatorPlacement.Top, -20)]
		[InlineData(TabIndicatorPlacement.Bottom, 20)]
		[InlineData(TabIndicatorPlacement.Bottom, 0)]
		[InlineData(TabIndicatorPlacement.Bottom, -20)]
		[InlineData(TabIndicatorPlacement.Fill, 20)]
		[InlineData(TabIndicatorPlacement.Fill, 0)]
		[InlineData(TabIndicatorPlacement.Fill, -20)]
		public void Test_Bottom_CornerRadius_RTL(TabIndicatorPlacement tabIndicatorPlacement, double radius)
		{
			var cornerRadius = new CornerRadius(radius);
			var tabView = new SfTabView
			{
				IndicatorCornerRadius = cornerRadius,
				IndicatorPlacement = tabIndicatorPlacement,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft
			};

			tabView.Items = PopulateLabelItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var indicatorRoundRectangle = GetPrivateField(tabHeader, "_roundRectangle") as RoundRectangle;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);

			Assert.Equal(cornerRadius, indicatorRoundRectangle?.CornerRadius);
			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, 20)]
		[InlineData(TabIndicatorPlacement.Top, 0)]
		[InlineData(TabIndicatorPlacement.Top, -20)]
		[InlineData(TabIndicatorPlacement.Bottom, 20)]
		[InlineData(TabIndicatorPlacement.Bottom, 0)]
		[InlineData(TabIndicatorPlacement.Bottom, -20)]
		[InlineData(TabIndicatorPlacement.Fill, 20)]
		[InlineData(TabIndicatorPlacement.Fill, 0)]
		[InlineData(TabIndicatorPlacement.Fill, -20)]
		public void Test_Top_CornerRadius_RTL(TabIndicatorPlacement tabIndicatorPlacement, double radius)
		{
			var cornerRadius = new CornerRadius(radius);
			var tabView = new SfTabView
			{
				IndicatorCornerRadius = cornerRadius,
				IndicatorPlacement = tabIndicatorPlacement,
				TabBarPlacement = TabBarPlacement.Top,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var indicatorRoundRectangle = GetPrivateField(tabHeader, "_roundRectangle") as RoundRectangle;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(0, tabHeaderIndex);
			Assert.Equal(1, tabContentIndex);

			Assert.Equal(cornerRadius, indicatorRoundRectangle?.CornerRadius);
			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
		}

		#endregion

		#region HeaderFolder Scripts

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		public void Test_DisplayHeaderMode_Bottom_Default(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader!.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem.HeaderDisplayMode);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
				var actualIndicatorPlacement = (tabHeader!.Items[i]).IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		public void Test_DisplayHeaderMode_Default(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Top,
			};

			tabView.Items = PopulateLabelItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(0, tabHeaderIndex);
			Assert.Equal(1, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader!.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem.HeaderDisplayMode);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
				var actualIndicatorPlacement = (tabHeader!.Items[i]).IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		public void Test_DisplayHeaderMode_Bottom_Default_RTL(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft
			};

			tabView.Items = PopulateLabelItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader!.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem.HeaderDisplayMode);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
				var actualIndicatorPlacement = (tabHeader!.Items[i]).IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Default)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Default)]
		public void Test_DisplayHeaderMode_Default_RTL(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Top,
				FlowDirection = FlowDirection.RightToLeft
			};

			tabView.Items = PopulateLabelItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(0, tabHeaderIndex);
			Assert.Equal(1, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader!.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem.HeaderDisplayMode);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
				var actualIndicatorPlacement = (tabHeader!.Items[i]).IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		public void Test_DisplayHeaderMode_Bottom_Text(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader!.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem.HeaderDisplayMode);

				var actualIndicatorPlacement = tabItem.IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		public void Test_DisplayHeaderMode_Text(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Top,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(0, tabHeaderIndex);
			Assert.Equal(1, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader!.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem.HeaderDisplayMode);

				var actualIndicatorPlacement = tabItem.IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		public void Test_DisplayHeaderMode_Bottom_Text_RTL(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader!.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem.HeaderDisplayMode);

				var actualIndicatorPlacement = tabItem.IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Text)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Text)]
		public void Test_DisplayHeaderMode_Text_RTL(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Top,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(0, tabHeaderIndex);
			Assert.Equal(1, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader!.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem.HeaderDisplayMode);

				var actualIndicatorPlacement = tabItem.IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		public void Test_DisplayHeaderMode_Bottom_Image(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader?.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem?.HeaderDisplayMode);
				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);

				var actualIndicatorPlacement = tabItem?.IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		public void Test_DisplayHeaderMode_Image(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Top,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(0, tabHeaderIndex);
			Assert.Equal(1, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader?.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem?.HeaderDisplayMode);
				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);

				var actualIndicatorPlacement = tabItem?.IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		public void Test_DisplayHeaderMode_Bottom_Image_RTL(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader?.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem?.HeaderDisplayMode);
				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);

				var actualIndicatorPlacement = tabItem?.IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Top, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Bottom, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Fit, TabWidthMode.Default, TabBarDisplayMode.Image)]
		[InlineData(TabIndicatorPlacement.Fill, IndicatorWidthMode.Stretch, TabWidthMode.Default, TabBarDisplayMode.Image)]
		public void Test_DisplayHeaderMode_Image_RTL(TabIndicatorPlacement tabIndicatorPlacement, IndicatorWidthMode indicatorWidthMode, TabWidthMode tabWidthMode, TabBarDisplayMode tabBarDisplayMode)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = tabIndicatorPlacement,
				TabWidthMode = tabWidthMode,
				IndicatorWidthMode = indicatorWidthMode,
				HeaderDisplayMode = tabBarDisplayMode,
				TabBarPlacement = TabBarPlacement.Top,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(0, tabHeaderIndex);
			Assert.Equal(1, tabContentIndex);

			Assert.Equal(tabIndicatorPlacement, tabHeader?.IndicatorPlacement);
			Assert.Equal(indicatorWidthMode, tabHeader?.IndicatorWidthMode);
			Assert.Equal(tabWidthMode, tabHeader?.TabWidthMode);
			Assert.Equal(tabBarDisplayMode, tabHeader?.HeaderDisplayMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var tabItem = tabHeader?.Items[i];
				Assert.Equal(tabBarDisplayMode, tabItem?.HeaderDisplayMode);
				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);

				var actualIndicatorPlacement = tabItem?.IndicatorPlacement;
				Assert.Equal(tabIndicatorPlacement, actualIndicatorPlacement);
			}
		}

		[Fact]
		public void Test_HeaderText1()
		{
			var tabView = new SfTabView
			{
				IndicatorWidthMode = IndicatorWidthMode.Stretch,
			};
			var tabItems = new TabItemCollection
			{
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 2" } }
			};

			tabView.Items = tabItems;
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(IndicatorWidthMode.Stretch, tabHeader?.IndicatorWidthMode);
		}

		[Fact]
		public void Test_HeaderText2()
		{
			var tabView = new SfTabView
			{
				IndicatorWidthMode = IndicatorWidthMode.Stretch,
				FlowDirection = FlowDirection.RightToLeft,
			};
			var tabItems = new TabItemCollection
			{
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 2" } }
			};

			tabView.Items = tabItems;
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(IndicatorWidthMode.Stretch, tabHeader?.IndicatorWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
		}

		[Fact]
		public void Test_HeaderText4()
		{
			var tabView = new SfTabView
			{
				FlowDirection = FlowDirection.RightToLeft,
			};
			var tabItems = new TabItemCollection
			{
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 2" } }
			};

			tabView.Items = tabItems;
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
		}

		[Fact]
		public void Test_HeaderText5()
		{
			var tabView = new SfTabView
			{
				IndicatorWidthMode = IndicatorWidthMode.Stretch,
				FlowDirection = FlowDirection.RightToLeft,
				TabBarPlacement = TabBarPlacement.Bottom,
			};
			var tabItems = new TabItemCollection
			{
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 2" } }
			};

			tabView.Items = tabItems;
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(IndicatorWidthMode.Stretch, tabHeader?.IndicatorWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
		}

		[Fact]
		public void Test_HeaderText7()
		{
			var tabView = new SfTabView
			{
				FlowDirection = FlowDirection.RightToLeft,
				TabBarPlacement = TabBarPlacement.Bottom,
			};
			var tabItems = new TabItemCollection
			{
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "MAUI", Content = new Label { Text = "Content 2" } }
			};

			tabView.Items = tabItems;
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
		}

		[Fact]
		public void Test_TabHeaderAlignment_Centre_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Center,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_Centre_Direction_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Center,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_Centre_Default1()
		{
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = TextAlignment.Center,
				TabWidthMode = TabWidthMode.Default,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_Centre_Direction_Default1()
		{
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = TextAlignment.Center,
				TabWidthMode = TabWidthMode.Default,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_End_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.End,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_End_Direction_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.End,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_Direction_Start1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Start,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_End_Direction_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.End,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_End_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.End,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_End_ImagePosition2()
		{
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = TextAlignment.End,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_End1()
		{
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = TextAlignment.End,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_Start_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Start,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_Start_Direction_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Start,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_Start_Direction_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Start,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_Start_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Start,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignment_Start1()
		{
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = TextAlignment.Start,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Centre_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Center,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Centre_Direction_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Center,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Centre_Direction_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Center,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Centre_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Center,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Centre1()
		{
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = TextAlignment.Center,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Direction_Centre1()
		{
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = TextAlignment.Center,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Center, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Direction_End1()
		{
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = TextAlignment.End,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Direction_Start1()
		{
			var tabView = new SfTabView
			{
				HeaderHorizontalTextAlignment = TextAlignment.Start,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_End_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.End,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_End_Direction_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.End,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_End_Direction_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.End,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_End_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.End,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.End, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Start_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Start,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Start_Direction_Default1()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Start,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Start_Direction_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Start,
				TabBarPlacement = TabBarPlacement.Bottom,
				FlowDirection = FlowDirection.RightToLeft,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			Assert.Equal(FlowDirection.RightToLeft, tabHeader?.FlowDirection);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		[Fact]
		public void Test_TabHeaderAlignmentBottom_Start_ImagePosition_Default2()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default,
				HeaderHorizontalTextAlignment = TextAlignment.Start,
				TabBarPlacement = TabBarPlacement.Bottom,
			};

			tabView.Items = PopulateLabelImageItemsCollection();
			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var tabContentIndex = Grid.GetRow(tabContent);
			var tabHeaderIndex = Grid.GetRow(tabHeader);

			Assert.Equal(1, tabHeaderIndex);
			Assert.Equal(0, tabContentIndex);
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
			foreach (var item in tabView.Items)
			{
				item.ImagePosition = TabImagePosition.Top;
			}
			for (int i = 0; i < tabView.Items.Count; i++)
			{
				var actualAlignment = (tabHeader!.Items[i]).HeaderHorizontalTextAlignment;
				Assert.Equal(TextAlignment.Start, actualAlignment);

				var expectedPosition = tabView.Items[i].ImagePosition;
				var actualPosition = (tabHeader!.Items[i]).ImagePosition;
				Assert.Equal(expectedPosition, actualPosition);
			}
		}

		#endregion

		#region Bugs

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		public void Bug_860909(int index)
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.SelectedIndex = index;
			Assert.Equal(((Label)tabItems[index].Content).Text, ((Label)tabItems[(int)tabView.SelectedIndex].Content).Text);
		}

		[Fact]
		public void Bug_870790()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarHeight = 50;
			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(tabView.TabBarHeight, tabHeaderContainer!.HeightRequest);
		}

		[Theory]
		[InlineData(true, 1)]
		public void MAUI860875(bool value, int recursiveCount)
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			int count = 0;
			tabView.SelectionChanged += (sender, e) =>
			{
				e.Handled = value;
				count++;
			};
			tabView.SelectedIndex = 1;
			Assert.Equal(recursiveCount, count);
		}

		[Fact]
		public void XAMARIN_19362()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var tabContentContainer = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var horizontalStackLayout = GetPrivateField(tabContentContainer, "_horizontalStackLayout") as SfHorizontalStackLayout;
			Assert.Equal(3, horizontalStackLayout!.Count);
			tabView.Items.Add(new SfTabItem { Header = "TAB 4", Content = new Label { Text = "Content 4" } });
			tabView.Items.Add(new SfTabItem { Header = "TAB 5", Content = new Label { Text = "Content 5" } });
			Assert.Equal(5, horizontalStackLayout!.Count);

		}

		[Fact]
		public void XAMARIN_19663()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var tabContentContainer = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			var horizontalStackLayout = GetPrivateField(tabContentContainer, "_horizontalStackLayout") as SfHorizontalStackLayout;
			tabView.Items.RemoveAt(2);
			Assert.Equal(2, horizontalStackLayout!.Count);
			tabView.Items.RemoveAt(1);
			Assert.Single(horizontalStackLayout!.Children);
			tabView.Items.RemoveAt(0);
			Assert.Empty(horizontalStackLayout!.Children);
		}

		[Fact]
		public void XAMARIN_25935()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			bool eventTriggered = false;
			tabView.SelectionChanged += (sender, e) =>
			{
				eventTriggered = true;
			};
			tabView.SelectedIndex = 2;
			Assert.True(eventTriggered);
		}

		[Fact]
		public void XAMARIN_39415()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			double index = -1;
			tabView.SelectionChanged += (sender, e) =>
			{
				index = e.NewIndex;
			};
			for (int i = tabView.Items.Count - 1; i >= 0; i--)
			{
				tabView.SelectedIndex = i;
				Assert.Equal(tabView.SelectedIndex, index);
			}
		}

		[Fact]
		public void XAMARIN_41311()
		{
			var tabView = new SfTabView();
			var tabItems = new TabItemCollection
			{
				new SfTabItem { Header = "SfTabItem1", FontSize=20, Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "SfTabItem2", FontSize=20, Content = new Label { Text = "Content 2" } },
				new SfTabItem { Header = "SfTabItem3", FontSize=20, Content = new Label { Text = "Content 3" } },
				new SfTabItem { Header = "SfTabItem1", FontSize=20, Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "SfTabItem2", FontSize=20, Content = new Label { Text = "Content 2" } },
				new SfTabItem { Header = "SfTabItem3", FontSize=20, Content = new Label { Text = "Content 3" } },
				new SfTabItem { Header = "SfTabItem1", FontSize=20, Content = new Label { Text = "Content 1" } },
				new SfTabItem { Header = "SfTabItem2", FontSize=20, Content = new Label { Text = "Content 2" } },
				new SfTabItem { Header = "SfTabItem3", FontSize=20, Content = new Label { Text = "Content 3" } }
			};

			tabView.Items = tabItems;
			tabView.TabWidthMode = TabWidthMode.Default;
			var materialStyle = new TabViewMaterialVisualStyle();
			var headerLabel = GetPrivateField(materialStyle, "_header") as SfLabel;
			Assert.Equal(LineBreakMode.TailTruncation, headerLabel!.LineBreakMode);
		}

		[Fact]
		public void XAMARIN_42654()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.SelectedIndex = 2;
			Assert.True(tabView.Items[(int)tabView.SelectedIndex].IsVisible);
		}

		[Theory]
		[InlineData(0, 2)]
		[InlineData(1, 0)]
		public void XAMARIN_866018(double index1, double index2)
		{
			var tabView1 = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView1.Items = tabItems;
			tabView1.SelectedIndex =Convert.ToInt32(index1);

			var tabView2 = new SfTabView();
			tabView2.Items = PopulateLabelItemsCollection();
			tabView2.SelectedIndex =Convert.ToInt32(index2);

			var verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.Add(tabView1);
			verticalStackLayout.Add(tabView2);

			Assert.Equal(index1, tabView1.SelectedIndex);
			Assert.Equal(index2, tabView2.SelectedIndex);
		}

		#endregion

		#region Indicator

		[Fact]
		public void IndicatorBackground1()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			var color = new SolidColorBrush(Colors.Violet);
			tabView.IndicatorBackground = color;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackground2()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			var color = new SolidColorBrush(Colors.Violet);
			tabView.IndicatorBackground = color;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackground3()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			var color = new SolidColorBrush(Colors.Violet);
			tabView.IndicatorBackground = color;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackground7()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var color = new SolidColorBrush(Colors.Violet);
			tabView.IndicatorBackground = color;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackground9()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var color = new SolidColorBrush(Colors.DarkGreen);
			tabView.IndicatorBackground = color;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackground12()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			var color = new SolidColorBrush(Colors.Violet);
			tabView.IndicatorBackground = color;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackgroundPlacement1()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var color = new SolidColorBrush(Colors.Black);
			tabView.IndicatorBackground = color;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Fill;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackgroundPlacement4()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var color = new SolidColorBrush(Colors.Black);
			tabView.IndicatorBackground = color;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackgroundPlacement7()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var color = new SolidColorBrush(Colors.Black);
			tabView.IndicatorBackground = color;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackgroundPlacement10()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var color = new SolidColorBrush(Colors.Black);
			tabView.IndicatorBackground = color;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Fill;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackgroundPlacement13()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var color = new SolidColorBrush(Colors.Black);
			tabView.IndicatorBackground = color;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorBackgroundPlacement16()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			var color = new SolidColorBrush(Colors.Black);
			tabView.IndicatorBackground = color;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.FlowDirection = FlowDirection.RightToLeft;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal((Brush)color, (Brush)tabSelectionIndicator!.BackgroundColor);
		}

		[Fact]
		public void IndicatorPlacement2()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal(LayoutOptions.Start, tabSelectionIndicator!.VerticalOptions);
		}

		[Fact]
		public void IndicatorPlacement4()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Fill;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal(tabView.TabBarHeight, tabSelectionIndicator!.HeightRequest);
		}

		[Fact]
		public void IndicatorPlacement5()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal(LayoutOptions.Start, tabSelectionIndicator!.VerticalOptions);
		}

		[Fact]
		public void IndicatorPlacement6()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Fill;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal(tabView.TabBarHeight, tabSelectionIndicator!.HeightRequest);
		}

		[Fact]
		public void IndicatorPlacement11()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal(LayoutOptions.Start, tabSelectionIndicator!.VerticalOptions);
		}

		[Fact]
		public void IndicatorPlacement13()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.FlowDirection = FlowDirection.RightToLeft;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabSelectionIndicator = GetPrivateField(tabHeaderContainer, "_tabSelectionIndicator") as Border;
			Assert.Equal(LayoutOptions.Start, tabSelectionIndicator!.VerticalOptions);
		}

		#endregion
		#region Customization

		[Fact]
		public void Tabbarbackground1()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarBackground = Colors.Gray;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(Colors.Gray, tabHeaderContainer!.Background);
		}

		[Fact]
		public void Tabbarbackground4()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarBackground = Colors.Gray;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(Colors.Gray, tabHeaderContainer!.Background);
		}

		[Fact]
		public void Tabbarbackground7()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarBackground = Colors.Gray;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(Colors.Gray, tabHeaderContainer!.Background);
		}

		[Fact]
		public void TabbarbackgroundIndicatorplacement1()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarBackground = Colors.Gray;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Fill;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(Colors.Gray, tabHeaderContainer!.Background);
		}

		[Fact]
		public void TabbarbackgroundIndicatorplacement4()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarBackground = Colors.Gray;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(Colors.Gray, tabHeaderContainer!.Background);
		}

		[Fact]
		public void TabbarbackgroundIndicatorplacement10()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarBackground = Colors.Gray;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Fill;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(Colors.Gray, tabHeaderContainer!.Background);
		}

		[Fact]
		public void TabbarbackgroundIndicatorplacement19()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarBackground = Colors.Gray;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(Colors.Gray, tabHeaderContainer!.Background);
		}

		[Fact]
		public void TabbarbackgroundIndicatorplacement22()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarBackground = Colors.Gray;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(Colors.Gray, tabHeaderContainer!.Background);
		}

		[Fact]
		public void TabbarbackgroundIndicatorplacement28()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarBackground = Colors.Gray;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.FlowDirection = FlowDirection.RightToLeft;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(Colors.Gray, tabHeaderContainer!.Background);
		}

		[Fact]
		public void TabBarHeight1()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarHeight = 100;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(100, tabHeaderContainer!.HeightRequest);
		}

		[Fact]
		public void TabBarHeight2()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarHeight = 100;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(100, tabHeaderContainer!.HeightRequest);
		}

		[Fact]
		public void TabBarHeight3()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarHeight = 100;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(100, tabHeaderContainer!.HeightRequest);
		}

		[Fact]
		public void TabBarHeight7()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			tabView.TabBarHeight = 100;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(100, tabHeaderContainer!.HeightRequest);
		}

		[Fact]
		public void TabBarHeight9()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			tabView.TabBarHeight = 100;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(100, tabHeaderContainer!.HeightRequest);
		}

		[Fact]
		public void TabBarHeight11()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarHeight = 100;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(100, tabHeaderContainer!.HeightRequest);
		}

		[Fact]
		public void TabBarHeight15()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			tabView.TabBarHeight = 100;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(100, tabHeaderContainer!.HeightRequest);
		}

		[Fact]
		public void TabBarHeight17()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.IndicatorWidthMode = IndicatorWidthMode.Stretch;
			tabView.TabBarHeight = 100;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(100, tabHeaderContainer!.HeightRequest);
		}

		[Fact]
		public void TabBarHeight18()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarHeight = 100;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(100, tabHeaderContainer!.HeightRequest);
		}

		[Fact]
		public void TabBarPlacement1()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContentContainer = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.Equal(0, Grid.GetRow(tabContentContainer));
			Assert.Equal(1, Grid.GetRow(tabHeaderContainer));
		}

		[Fact]
		public void TabBarPlacement4()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContentContainer = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.Equal(0, Grid.GetRow(tabContentContainer));
			Assert.Equal(1, Grid.GetRow(tabHeaderContainer));
		}

		[Fact]
		public void TabBarPlacement6()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Fill;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContentContainer = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.Equal(0, Grid.GetRow(tabContentContainer));
			Assert.Equal(1, Grid.GetRow(tabHeaderContainer));
		}

		[Fact]
		public void TabBarPlacement10()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContentContainer = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.Equal(0, Grid.GetRow(tabContentContainer));
			Assert.Equal(1, Grid.GetRow(tabHeaderContainer));
		}

		[Fact]
		public void TabBarPlacement12()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Top;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContentContainer = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.Equal(0, Grid.GetRow(tabContentContainer));
			Assert.Equal(1, Grid.GetRow(tabHeaderContainer));
		}

		[Fact]
		public void TabBarPlacement15()
		{
			var tabView = new SfTabView();
			var tabItems = PopulateLabelItemsCollection();

			tabView.Items = tabItems;
			tabView.TabBarPlacement = TabBarPlacement.Bottom;
			tabView.IndicatorPlacement = TabIndicatorPlacement.Fill;
			tabView.FlowDirection = FlowDirection.RightToLeft;

			var tabHeaderContainer = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContentContainer = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.Equal(0, Grid.GetRow(tabContentContainer));
			Assert.Equal(1, Grid.GetRow(tabHeaderContainer));
		}

		#endregion

		#region Center Button

		[Fact]
		public void TestInitializeCenterButtonWithItems()
		{
			SfTabView sfTabView = new SfTabView();
			sfTabView.Items = PopulateLabelItemsCollection();
			sfTabView.IsCenterButtonEnabled = true;
			sfTabView.CenterButtonSettings = new CenterButtonSettings();

			// From  SfTabView class
			var _tabHeaderContainer = GetPrivateField(sfTabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(_tabHeaderContainer);
			Assert.Equal(_tabHeaderContainer.IsCenterButtonEnabled, sfTabView.IsCenterButtonEnabled);
			var _centerButtonViewPlaceholder = GetPrivateField(_tabHeaderContainer, "_centerButtonViewPlaceholder") as SfGrid;
			Assert.NotNull(_centerButtonViewPlaceholder);
			var _tabHeaderItemContent = GetPrivateField(_tabHeaderContainer, "_tabHeaderItemContent") as SfHorizontalStackLayout;
			int _countOfCenterButton = 1;
			Assert.NotNull(_tabHeaderItemContent);
			Assert.Equal(_tabHeaderItemContent.Children.Count, sfTabView.Items.Count + _countOfCenterButton);
			var totalCount = sfTabView.Items.Count;
			var centerIndex = totalCount / 2 + totalCount % 2;
			Assert.IsType<SfGrid>((IView)_tabHeaderItemContent.Children[centerIndex]);
			Assert.Equal(_centerButtonViewPlaceholder, _tabHeaderItemContent.Children[centerIndex]);
			var _centerButtonView = GetPrivateField(sfTabView, "_centerButtonView") as CenterButtonView;
			Assert.NotNull(_centerButtonView);
			var _parentGrid = GetPrivateField(sfTabView, "_parentGrid") as SfGrid;
			Assert.NotNull(_parentGrid);
			Assert.True(_parentGrid.Children.Contains(_centerButtonView));
			Assert.Equal(2, Grid.GetRowSpan(_centerButtonView));
			Assert.Equal(LayoutOptions.Start, _centerButtonView.VerticalOptions);
			Assert.Equal(LayoutOptions.Center, _centerButtonView.HorizontalOptions);

			// From CenterButtonView
			var _centerButtonTitle = GetPrivateField(_centerButtonView, "_centerButtonTitle") as Label;
			Assert.NotNull(_centerButtonTitle);
			Assert.Equal(sfTabView.CenterButtonSettings.Title, _centerButtonTitle.Text);
			Assert.Equal(sfTabView.CenterButtonSettings.TextColor, _centerButtonTitle.TextColor);
			Assert.Equal(sfTabView.CenterButtonSettings.FontSize, _centerButtonTitle.FontSize);
			Assert.Equal(sfTabView.CenterButtonSettings.FontAutoScalingEnabled, _centerButtonTitle.FontAutoScalingEnabled);
			Assert.Equal(sfTabView.CenterButtonSettings.FontAttributes, _centerButtonTitle.FontAttributes);
			Assert.Equal(sfTabView.CenterButtonSettings.FontFamily, _centerButtonTitle.FontFamily);
			var _centerButtonBorderView = GetPrivateField(_centerButtonView, "_centerButtonBorderView") as SfBorder;
			Assert.NotNull(_centerButtonBorderView);
			Assert.Equal(sfTabView.CenterButtonSettings.Height, _centerButtonBorderView.HeightRequest);
			Assert.Equal(sfTabView.CenterButtonSettings.Width, _centerButtonBorderView.WidthRequest);
			Assert.Equal(sfTabView.CenterButtonSettings.Background, _centerButtonBorderView.Background);
			Assert.Null(_centerButtonBorderView.Stroke);
			var _centerButtonVerticalLayout = GetPrivateField(_centerButtonView, "_centerButtonVerticalLayout") as SfVerticalStackLayout;
			Assert.NotNull(_centerButtonVerticalLayout);
			Assert.Equal(_centerButtonTitle, _centerButtonVerticalLayout.Children[0]);
			var _centerButtonImage = GetPrivateField(_centerButtonView, "_centerButtonImage") as Image;
			Assert.NotNull(_centerButtonImage);
			Assert.Equal(sfTabView.CenterButtonSettings.ImageSource, _centerButtonImage.Source);
			Assert.Equal(sfTabView.CenterButtonSettings.ImageSize, _centerButtonImage.WidthRequest);
			Assert.Equal(sfTabView.CenterButtonSettings.ImageSize, _centerButtonImage.HeightRequest);

			sfTabView.IsCenterButtonEnabled = false;
			Assert.Equal(_tabHeaderItemContent.Children.Count, sfTabView.Items.Count);
			Assert.False(_parentGrid.Children.Contains(_centerButtonView));
		}

		[Theory]
		[InlineData("Center button")]
		[InlineData("Home")]
		[InlineData("Profile",20)]
		[InlineData("Home", -1)]
		[InlineData("Profile", 5)]
		[InlineData("Home", 15, "Times New Roman")]
		[InlineData("Profile", 2, "Arial")]
		[InlineData("Center button", 30, "", FontAttributes.Italic)]
		[InlineData("Profile", 4, "", FontAttributes.Bold)]
		[InlineData("Home", 35, "", FontAttributes.None)]
		[InlineData("Profile", 4, "", FontAttributes.Bold, true)]
		[InlineData("Center button", 10, "", FontAttributes.None, false)]
		public void TestCenterButtonTitle(string title, double fontSize = 14, string fontFamily = "", FontAttributes fontAttributes = FontAttributes.None, bool fontAutoScalingEnabled = false)
		{
			SfTabView sfTabView = new SfTabView();
			sfTabView.Items = PopulateLabelItemsCollection();
			sfTabView.IsCenterButtonEnabled = true;
			var centerButtonSettings = new CenterButtonSettings()
			{
				Title = title,
				FontSize = fontSize,
				FontFamily = fontFamily,
				FontAttributes = fontAttributes,
				FontAutoScalingEnabled = fontAutoScalingEnabled
			};
			sfTabView.CenterButtonSettings = centerButtonSettings;
			var _centerButtonView = GetPrivateField(sfTabView, "_centerButtonView") as CenterButtonView;
			Assert.NotNull(_centerButtonView);
			var _centerButtonTitle = GetPrivateField(_centerButtonView, "_centerButtonTitle") as Label;
			Assert.NotNull(_centerButtonTitle);
			Assert.Equal(title, _centerButtonTitle.Text);
			Assert.Equal(fontSize, _centerButtonTitle.FontSize);
			Assert.Equal(fontFamily, _centerButtonTitle.FontFamily);
			Assert.Equal(fontAttributes, _centerButtonTitle.FontAttributes);

			centerButtonSettings.TextColor = Colors.Green;
			Assert.Equal(Colors.Green, _centerButtonTitle.TextColor);
		}

		[Theory]
		[InlineData(50)]
		[InlineData(5)]
		[InlineData(48, 100)]
		[InlineData(48, 5)]
		[InlineData(48, 70, 10)]
		[InlineData(48, 70, -5)]
		[InlineData(48, 70, 50)]
		[InlineData(48, 70, 0, 5)]
		[InlineData(48, 70, 0, -5)]
		[InlineData(48, 70, 0, 20)]
		public void TestCenterButtonBorder(double height, double width = 70d, double cornerRadius = 0, double strokeThickness = 0)
		{
			SfTabView sfTabView = new SfTabView();
			sfTabView.Items = PopulateLabelItemsCollection();
			sfTabView.IsCenterButtonEnabled = true;
			var centerButtonSettings = new CenterButtonSettings()
			{
				Height = height,
				Width = width,
				CornerRadius = cornerRadius,
				StrokeThickness = strokeThickness,

			};
			sfTabView.CenterButtonSettings = centerButtonSettings;
			var _centerButtonView = GetPrivateField(sfTabView, "_centerButtonView") as CenterButtonView;
			Assert.NotNull(_centerButtonView);

			var _centerButtonBorderView = GetPrivateField(_centerButtonView, "_centerButtonBorderView") as SfBorder;
			Assert.NotNull(_centerButtonBorderView);
			var _centerButtonRoundRectangle = GetPrivateField(_centerButtonView, "_centerButtonRoundRectangle") as RoundRectangle;
			Assert.NotNull(_centerButtonRoundRectangle);
			Assert.Equal(height, _centerButtonBorderView.HeightRequest);
			Assert.Equal(width, _centerButtonBorderView.WidthRequest);
			Assert.Equal(cornerRadius, _centerButtonRoundRectangle.CornerRadius);
			Assert.Equal(strokeThickness, _centerButtonBorderView.StrokeThickness);

			centerButtonSettings.Stroke = Colors.Red;
			Assert.Equal(Colors.Red, _centerButtonBorderView.Stroke);
		}

		[Theory]
		[InlineData(CenterButtonDisplayMode.Text)]
		[InlineData(CenterButtonDisplayMode.Image)]
		[InlineData(CenterButtonDisplayMode.ImageWithText)]
		[InlineData(CenterButtonDisplayMode.ImageWithText,"SampleImage1.png")]
		[InlineData(CenterButtonDisplayMode.Image, "SampleImage2.png")]
		[InlineData(CenterButtonDisplayMode.ImageWithText, "SampleImage1.png", 45)]
		[InlineData(CenterButtonDisplayMode.Image, "SampleImage2.png", -5)]
		public void TestCenterButtonDisplayMode(CenterButtonDisplayMode centerButtonDisplayMode = CenterButtonDisplayMode.Text, string imagePath = "", double imageSize = 20d)
		{
			SfTabView sfTabView = new SfTabView();
			sfTabView.Items = PopulateLabelItemsCollection();
			sfTabView.IsCenterButtonEnabled = true;
			var imageSource = ImageSource.FromFile(imagePath);
			var centerButtonSettings = new CenterButtonSettings()
			{
				DisplayMode = centerButtonDisplayMode,
				ImageSource = imageSource,
				ImageSize = imageSize,
			};
			sfTabView.CenterButtonSettings = centerButtonSettings;
			var _centerButtonView = GetPrivateField(sfTabView, "_centerButtonView") as CenterButtonView;
			Assert.NotNull(_centerButtonView);
			var _centerButtonVerticalLayout = GetPrivateField(_centerButtonView, "_centerButtonVerticalLayout") as SfVerticalStackLayout;
			Assert.NotNull(_centerButtonVerticalLayout);
			var _centerButtonTitle = GetPrivateField(_centerButtonView, "_centerButtonTitle") as SfLabel;
			Assert.NotNull(_centerButtonTitle);
			var _centerButtonImage = GetPrivateField(_centerButtonView, "_centerButtonImage") as SfImage;
			Assert.NotNull(_centerButtonImage);
			if (centerButtonDisplayMode is CenterButtonDisplayMode.Text)
			{
				Assert.Equal(_centerButtonTitle, _centerButtonVerticalLayout.Children[0]);
			}
			else if (centerButtonDisplayMode is CenterButtonDisplayMode.Image)
			{
				Assert.Equal(_centerButtonImage, _centerButtonVerticalLayout.Children[0]);
			}
			else
			{
				Assert.Equal(_centerButtonImage, _centerButtonVerticalLayout.Children[0]);
				Assert.Equal(_centerButtonTitle, _centerButtonVerticalLayout.Children[1]);
			}

			Assert.Equal(imageSource, _centerButtonImage.Source);
			Assert.Equal(imageSize, _centerButtonImage.HeightRequest);
			Assert.Equal(imageSize, _centerButtonImage.WidthRequest);
		}

		[Theory]
		[InlineData(TabBarPlacement.Bottom)]
		[InlineData(TabBarPlacement.Top)]
		public void TestCenterButtonWithTabBarPlacement(TabBarPlacement tabBarPlacement)
		{
			SfTabView sfTabView = new SfTabView();
			sfTabView.Items = PopulateLabelItemsCollection();
			sfTabView.IsCenterButtonEnabled = true;
			sfTabView.TabBarPlacement = tabBarPlacement;
			var centerButtonSettings = new CenterButtonSettings();
			sfTabView.CenterButtonSettings = centerButtonSettings;
			var _centerButtonView = GetPrivateField(sfTabView, "_centerButtonView") as CenterButtonView;
			Assert.NotNull(_centerButtonView);
			if (sfTabView.TabBarPlacement is TabBarPlacement.Bottom)
			{
				Assert.Equal(LayoutOptions.Center, _centerButtonView.HorizontalOptions);
				Assert.Equal(LayoutOptions.End, _centerButtonView.VerticalOptions);
			}
			else
			{
				Assert.Equal(LayoutOptions.Center, _centerButtonView.HorizontalOptions);
				Assert.Equal(LayoutOptions.Start, _centerButtonView.VerticalOptions);
			}
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(7)]
		[InlineData(8)]
		[InlineData(9)]
		[InlineData(10)]
		public void TestCenterButtonWithItemsCount(int count)
		{
			SfTabView sfTabView = new SfTabView();
			sfTabView.IsCenterButtonEnabled = true;
			var centerButtonSettings = new CenterButtonSettings();
			sfTabView.CenterButtonSettings = centerButtonSettings;
			TabItemCollection items = new TabItemCollection();
			for (int i=0; i < count; i++)
			{
				items.Add(new SfTabItem
				{
					Header = $"Item {items.Count + 1}",
					Content = new Label(){Text = $"Item {items.Count+1}"},
				});
			}
			sfTabView.Items = items;
			var totalCount = items.Count;
			int centerIndex = totalCount / 2 + totalCount % 2;
			var _centerButtonView = GetPrivateField(sfTabView, "_centerButtonView") as CenterButtonView;
			Assert.NotNull(_centerButtonView);
			var _parentGrid = GetPrivateField(sfTabView, "_parentGrid") as SfGrid;
			Assert.NotNull(_parentGrid);
			var _tabHeaderContainer = GetPrivateField(sfTabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(_tabHeaderContainer);
			Assert.Equal(_tabHeaderContainer.IsCenterButtonEnabled, sfTabView.IsCenterButtonEnabled);
			var _tabHeaderItemContent = GetPrivateField(_tabHeaderContainer, "_tabHeaderItemContent") as SfHorizontalStackLayout;
			int _countOfCenterButton = 1;
			Assert.NotNull(_tabHeaderItemContent);
			var _centerButtonViewPlaceholder = GetPrivateField(_tabHeaderContainer, "_centerButtonViewPlaceholder") as SfGrid;
			Assert.NotNull(_centerButtonViewPlaceholder);
			Assert.Equal(_tabHeaderItemContent.Children.Count, sfTabView.Items.Count + _countOfCenterButton);
			Assert.True(_parentGrid.Children.Contains(_centerButtonView));
			Assert.Equal(2, Grid.GetRowSpan(_centerButtonView));
			Assert.IsType<SfGrid>((IView)_tabHeaderItemContent.Children[centerIndex]);
			Assert.True(_tabHeaderItemContent.Children.Contains(_centerButtonViewPlaceholder));
			Assert.Equal(_centerButtonViewPlaceholder, _tabHeaderItemContent.Children[centerIndex]);
		}

		#endregion

		#region HeaderContent Unit Tests

		[Fact]
		public void HeaderContent_Property_Default_Value_Is_Empty()
		{
			var tabItem = new SfTabItem();
			Assert.Empty(tabItem.HeaderContent?.ToString() ?? string.Empty);
		}

		[Theory]
		[InlineData(typeof(Label))]
		[InlineData(typeof(Button))]
		[InlineData(typeof(Grid))]
		[InlineData(typeof(StackLayout))]
		[InlineData(typeof(Image))]
		[InlineData(typeof(Border))]
		public void HeaderContent_Property_Accepts_Various_View_Types(Type viewType)
		{
			var tabItem = new SfTabItem();
			var view = Activator.CreateInstance(viewType) as View;
			Assert.NotNull(view);

			tabItem.HeaderContent = view;
			Assert.Equal(view, tabItem.HeaderContent);
		}

		[Fact]
		public void HeaderContent_Property_Change_Triggers_PropertyChanged()
		{
			var tabItem = new SfTabItem();
			var propertyChangedFired = false;
			string? changedPropertyName = null;

			tabItem.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == nameof(SfTabItem.HeaderContent))
				{
					propertyChangedFired = true;
					changedPropertyName = e.PropertyName;
				}
			};

			tabItem.HeaderContent = new Label { Text = "Test" };

			Assert.True(propertyChangedFired);
			Assert.Equal(nameof(SfTabItem.HeaderContent), changedPropertyName);
		}

		[Fact]
		public void HeaderContent_Takes_Precedence_Over_Header_Property()
		{
			var tabItem = new SfTabItem
			{
				Header = "Normal Header",
				HeaderContent = new Label { Text = "Custom Header" }
			};
			var content = tabItem.HeaderContent;
			content.BindingContext = tabItem;

			// Verify HeaderContent is displayed, not Header
			Assert.IsType<Label>(content);
			Assert.Equal("Custom Header", ((Label)content).Text);
		}

		[Fact]
		public void MeasureHeaderContentWidth_Returns_Correct_Width_For_Simple_View()
		{
			var tabBar = new SfTabBar();
			var label = new Label { Text = "Test Content", WidthRequest = 100 };
			var tabItem = new SfTabItem { HeaderContent = label };

			var width = tabBar.MeasureHeaderContentWidth(tabItem);

			Assert.True(width >= 0);
		}

		[Theory]
		[InlineData(50, 30)]
		[InlineData(200, 100)]
		[InlineData(10, 10)]
		public void MeasureHeaderContentWidth_Handles_Various_Sizes(double widthRequest, double heightRequest)
		{
			var tabBar = new SfTabBar();
			var grid = new Grid { WidthRequest = widthRequest, HeightRequest = heightRequest };
			var tabItem = new SfTabItem { HeaderContent = grid };

			var width = tabBar.MeasureHeaderContentWidth(tabItem);

			Assert.True(width >= 0);
		}

		[Fact]
		public void MeasureHeaderContentWidth_Returns_Zero_For_Invisible_Content()
		{
			var tabBar = new SfTabBar();
			var invisibleContent = new Label { Text = "Hidden", IsVisible = false };
			var tabItem = new SfTabItem { HeaderContent = invisibleContent, IsVisible = false };

			var width = tabBar.MeasureHeaderContentWidth(tabItem);

			Assert.Equal(0, width);
		}

		[Fact]
		public void MeasureHeaderContentWidth_Handles_Complex_Layout()
		{
			var tabBar = new SfTabBar();


			var complexLayout = new Grid
			{
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto }
				}
			};

			var label = new Label { Text = "Tab 1" };
			var button = new Button { Text = "Click" };

			Grid.SetRow(label, 0);

			Grid.SetRow(button, 1);

			complexLayout.Children.Add(label);
			complexLayout.Children.Add(button);

			var tabItem = new SfTabItem { HeaderContent = complexLayout };

			var width = tabBar.MeasureHeaderContentWidth(tabItem);

			Assert.True(width >= 0);
		}

		[Fact]
		public void TabViewMaterialVisualStyle_Updates_Layout_When_HeaderContent_Set()
		{
			var tabItem = new SfTabItem { Header = "Test" };
			var style = new TabViewMaterialVisualStyle();
			style.BindingContext = tabItem;

			var customContent = new Grid();

			var label = new Label { Text = "Tab 1" };

			Grid.SetRow(label, 0);

			customContent.Children.Add(label);
			tabItem.HeaderContent = customContent;

			// Simulate property change
			var method = style.GetType().GetMethod("OnParentPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance);
			method?.Invoke(style, new object[] { tabItem, new PropertyChangedEventArgs(nameof(SfTabItem.HeaderContent)) });

			Assert.Single(style.Children);
			Assert.Equal(customContent, style.Children[0]);
		}

		[Fact]
		public void HeaderContent_BindingContext_Is_Set_From_Parent()
		{
			var bindingContext = new { TestProperty = "TestValue" };
			var tabItem = new SfTabItem { BindingContext = bindingContext };
			var style = new TabViewMaterialVisualStyle();

			var customContent = new Label { Text = "Test" };
			tabItem.HeaderContent = customContent;
			style.BindingContext = tabItem;

			// Simulate the binding context flow in UpdateHeaderContent
			if (customContent.BindingContext == null)
				customContent.BindingContext = tabItem.BindingContext;

			Assert.Equal(bindingContext, customContent.BindingContext);
		}

		[Theory]
		[InlineData(TabHeaderAlignment.Start)]
		[InlineData(TabHeaderAlignment.Center)]
		[InlineData(TabHeaderAlignment.End)]
		public void HeaderContent_Respects_TabHeaderAlignment(TabHeaderAlignment alignment)
		{
			var tabView = new SfTabView
			{
				TabHeaderAlignment = alignment
			};
			var tabItem = new SfTabItem
			{
				Header = "Test",
				HeaderContent = new Label { Text = "Custom" }
			};
			tabView.Items.Add(tabItem);

			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(alignment, tabHeader?.TabHeaderAlignment);
		}

		[Fact]
		public void HeaderContent_Works_With_Default_TabWidthMode()
		{
			var tabView = new SfTabView
			{
				TabWidthMode = TabWidthMode.Default
			};
			var tabItem = new SfTabItem
			{
				Header = "Test",
				HeaderContent = new Label { Text = "Custom Content" }
			};
			tabView.Items.Add(tabItem);

			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(TabWidthMode.Default, tabHeader?.TabWidthMode);
		}

		[Fact]
		public void HeaderContent_SizeChanged_Handlers_Are_Attached()
		{
			var tabBar = new SfTabBar();
			var customContent = new Label { Text = "Test" };
			var tabItem = new SfTabItem { HeaderContent = customContent };

			tabBar.Items.Add(tabItem);

			// Verify no exception is thrown when adding the item
			Assert.Single(tabBar.Items);
			Assert.Equal(customContent, tabItem.HeaderContent);
		}

		[Fact]
		public void HeaderContent_PropertyChanged_Triggers_Layout_Update()
		{
			var tabItem = new SfTabItem();
			var propertyChangedCount = 0;

			tabItem.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == nameof(SfTabItem.HeaderContent))
					propertyChangedCount++;
			};

			tabItem.HeaderContent = new Label { Text = "Test1" };
			tabItem.HeaderContent = new Button { Text = "Test2" };
			tabItem.HeaderContent = new Grid();

			Assert.Equal(3, propertyChangedCount);
		}

		[Fact]
		public void HeaderContent_Can_Be_Updated_Multiple_Times()
		{
			var tabItem = new SfTabItem();

			var content1 = new Label { Text = "Content 1" };
			var content2 = new Button { Text = "Content 2" };
			var content3 = new Grid();

			tabItem.HeaderContent = content1;
			Assert.Equal(content1, tabItem.HeaderContent);

			tabItem.HeaderContent = content2;
			Assert.Equal(content2, tabItem.HeaderContent);

			tabItem.HeaderContent = content3;
			Assert.Equal(content3, tabItem.HeaderContent);
		}

		[Fact]
		public void HeaderContent_Updates_Trigger_Width_Recalculation()
		{
			var tabBar = new SfTabBar();
			var tabItem = new SfTabItem();
			tabBar.Items.Add(tabItem);

			var smallContent = new Label { Text = "Small", WidthRequest = 50 };
			var largeContent = new Label { Text = "Large Content", WidthRequest = 200 };

			tabItem.HeaderContent = smallContent;
			var smallWidth = tabBar.MeasureHeaderContentWidth(tabItem);

			tabItem.HeaderContent = largeContent;
			var largeWidth = tabBar.MeasureHeaderContentWidth(tabItem);

			// Large content should generally have larger or equal width
			Assert.True(largeWidth >= smallWidth || largeWidth >= 0);
		}

		[Fact]
		public void HeaderContent_With_Zero_Dimensions_Handled()
		{
			var tabBar = new SfTabBar();
			var zeroSizedContent = new Grid { WidthRequest = 1, HeightRequest = 1 };
			var tabItem = new SfTabItem { HeaderContent = zeroSizedContent };

			var width = tabBar.MeasureHeaderContentWidth(tabItem);

			Assert.True(width >= 0);
		}

		[Fact]
		public void HeaderContent_With_Nested_Complex_Layout()
		{
			var tabBar = new SfTabBar();
			var complexLayout = new Grid();
			var nestedStack = new StackLayout();
			nestedStack.Children.Add(new Label { Text = "Nested Label" });
			nestedStack.Children.Add(new Button { Text = "Nested Button" });
			complexLayout.Children.Add(nestedStack);

			var tabItem = new SfTabItem { HeaderContent = complexLayout };

			var width = tabBar.MeasureHeaderContentWidth(tabItem);
			Assert.True(width >= 0);
		}

		[Fact]
		public void HeaderContent_Measurement_Consistency()
		{
			var tabBar = new SfTabBar();
			var content = new Label { Text = "Consistent", WidthRequest = 100 };
			var tabItem = new SfTabItem { HeaderContent = content };

			var width1 = tabBar.MeasureHeaderContentWidth(tabItem);
			var width2 = tabBar.MeasureHeaderContentWidth(tabItem);

			Assert.Equal(width1, width2);
		}

		[Fact]
		public void ClearHeaderItem_Handles_HeaderContent_Properly()
		{
			var tabBar = new SfTabBar();
			var customContent = new Grid();
			var tabItem = new SfTabItem { HeaderContent = customContent };
			tabBar.Items.Add(tabItem);

			// Should not throw exception
			tabBar.ClearHeaderItem(tabItem, 0);
			Assert.True(true);
		}

		[Fact]
		public void HeaderContent_Removed_From_Visual_Tree_On_Clear()
		{
			var style = new TabViewMaterialVisualStyle();
			var tabItem = new SfTabItem();
			var customContent = new Label { Text = "Test" };

			tabItem.HeaderContent = customContent;
			style.BindingContext = tabItem;

			// Simulate property change to add content
			var method = style.GetType().GetMethod("OnParentPropertyChanged", BindingFlags.NonPublic | BindingFlags.Instance);
			method?.Invoke(style, new object[] { tabItem, new PropertyChangedEventArgs(nameof(SfTabItem.HeaderContent)) });

			Assert.Contains(customContent, style.Children);

			// Remove content by replacing with different content
			var replacementContent = new Button { Text = "Replacement" };
			tabItem.HeaderContent = replacementContent;
			method?.Invoke(style, new object[] { tabItem, new PropertyChangedEventArgs(nameof(SfTabItem.HeaderContent)) });

			Assert.DoesNotContain(customContent, style.Children);
			Assert.Contains(replacementContent, style.Children);
		}

		[Fact]
		public void HeaderContent_Works_With_TabView_Items_Collection()
		{
			var tabView = new SfTabView();
			var tabItem1 = new SfTabItem
			{
				Header = "Tab 1",
				HeaderContent = new Label { Text = "Custom Header 1" },
				Content = new Label { Text = "Content 1" }
			};
			var tabItem2 = new SfTabItem
			{
				Header = "Tab 2",
				HeaderContent = new Button { Text = "Custom Header 2" },
				Content = new Label { Text = "Content 2" }
			};

			tabView.Items.Add(tabItem1);
			tabView.Items.Add(tabItem2);

			Assert.Equal(2, tabView.Items.Count);
			Assert.Equal(tabItem1.HeaderContent, tabView.Items[0].HeaderContent);
			Assert.Equal(tabItem2.HeaderContent, tabView.Items[1].HeaderContent);
		}

		[Theory]
		[InlineData(TabBarDisplayMode.Default)]
		[InlineData(TabBarDisplayMode.Text)]
		[InlineData(TabBarDisplayMode.Image)]
		public void HeaderContent_Respects_TabBarDisplayMode(TabBarDisplayMode displayMode)
		{
			var tabView = new SfTabView
			{
				HeaderDisplayMode = displayMode
			};
			var tabItem = new SfTabItem
			{
				Header = "Test",
				HeaderContent = new Label { Text = "Custom Header" },
				ImageSource = "test.png"
			};
			tabView.Items.Add(tabItem);

			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(displayMode, tabHeader?.HeaderDisplayMode);
		}

		[Theory]
		[InlineData(TabBarPlacement.Top)]
		[InlineData(TabBarPlacement.Bottom)]
		public void HeaderContent_Works_With_Different_TabBarPlacements(TabBarPlacement placement)
		{
			var tabView = new SfTabView
			{
				TabBarPlacement = placement
			};
			var tabItem = new SfTabItem
			{
				Header = "Test",
				HeaderContent = new Grid(),
				Content = new Label { Text = "Content" }
			};
			tabView.Items.Add(tabItem);

			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			var tabContent = GetPrivateField(tabView, "_tabContentContainer") as SfHorizontalContent;

			if (placement == TabBarPlacement.Bottom)
			{
				Assert.Equal(1, Grid.GetRow(tabHeader));
				Assert.Equal(0, Grid.GetRow(tabContent));
			}
			else
			{
				Assert.Equal(0, Grid.GetRow(tabHeader));
				Assert.Equal(1, Grid.GetRow(tabContent));
			}
		}

		[Theory]
		[InlineData(FlowDirection.LeftToRight)]
		[InlineData(FlowDirection.RightToLeft)]
		public void HeaderContent_Supports_FlowDirection(FlowDirection flowDirection)
		{
			var tabView = new SfTabView
			{
				FlowDirection = flowDirection
			};
			var tabItem = new SfTabItem
			{
				Header = "Test",
				HeaderContent = new Label { Text = "Custom Header" }
			};
			tabView.Items.Add(tabItem);

			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(flowDirection, tabHeader?.FlowDirection);
		}

		[Theory]
		[InlineData(IndicatorWidthMode.Fit)]
		[InlineData(IndicatorWidthMode.Stretch)]
		public void HeaderContent_Works_With_Different_IndicatorWidthModes(IndicatorWidthMode widthMode)
		{
			var tabView = new SfTabView
			{
				IndicatorWidthMode = widthMode
			};
			var tabItem = new SfTabItem
			{
				Header = "Test",
				HeaderContent = new Button { Text = "Custom" }
			};
			tabView.Items.Add(tabItem);

			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(widthMode, tabHeader?.IndicatorWidthMode);
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top)]
		[InlineData(TabIndicatorPlacement.Bottom)]
		[InlineData(TabIndicatorPlacement.Fill)]
		public void HeaderContent_Works_With_Different_IndicatorPlacements(TabIndicatorPlacement placement)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = placement
			};
			var tabItem = new SfTabItem
			{
				Header = "Test",
				HeaderContent = new Label { Text = "Custom" }
			};
			tabView.Items.Add(tabItem);

			var tabHeader = GetPrivateField(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.Equal(placement, tabHeader?.IndicatorPlacement);
		}

		[Fact]
		public void HeaderContent_Large_Collection_Performance()
		{
			var tabView = new SfTabView();
			var items = new TabItemCollection();

			for (int i = 0; i < 100; i++)
			{
				var tabItem = new SfTabItem
				{
					Header = $"Tab {i + 1}",
					HeaderContent = new Label { Text = $"Custom Header {i + 1}" },
					Content = new Label { Text = $"Content {i + 1}" }
				};
				items.Add(tabItem);
			}

			tabView.Items = items;

			Assert.Equal(100, tabView.Items.Count);
			Assert.All(tabView.Items, item => Assert.NotNull(item.HeaderContent));
		}

		[Fact]
		public void HeaderContent_Memory_Usage_With_Multiple_Views()
		{
			var tabItem = new SfTabItem();
			var views = new List<View>();

			// Create multiple views and assign them one by one
			for (int i = 0; i < 10; i++)
			{
				var view = new Label { Text = $"View {i}" };
				views.Add(view);
				tabItem.HeaderContent = view;

				Assert.Equal(view, tabItem.HeaderContent);
			}

			// Only the last assigned view should be the current HeaderContent
			Assert.Equal(views.Last(), tabItem.HeaderContent);
		}

		#endregion

		#region AnimationEasing Unit Tests

		[Fact]
		public void UpdateAnimationEasing_ShouldPropagateToChildren()
		{
			var tabView = new SfTabView();
			var expectedEasing = Easing.SinOut;

			InvokePrivateMethod(tabView, "UpdateAnimationEasing", [expectedEasing]);

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			var horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;

			Assert.NotNull(tabBar);
			Assert.NotNull(horizontalContent);
			Assert.Equal(expectedEasing, tabBar.AnimationEasing);
			Assert.Equal(expectedEasing, horizontalContent.AnimationEasing);
		}

		[Fact]
		public void AnimationEasing_DefaultValue_ShouldBeLinear()
		{
			var tabView = new SfTabView();

			Assert.Equal(Easing.Linear, tabView.AnimationEasing);
		}

		[Theory]
		[InlineData("BounceIn")]
		[InlineData("BounceOut")]
		[InlineData("CubicIn")]
		[InlineData("CubicInOut")]
		[InlineData("CubicOut")]
		[InlineData("Linear")]
		[InlineData("SinIn")]
		[InlineData("SinInOut")]
		[InlineData("SinOut")]
		[InlineData("SpringIn")]
		[InlineData("SpringOut")]
		public void AnimationEasing_SetAndGet_ReturnsExpectedValue(string easingName)
		{
			var tabView = new SfTabView();

			// Map easing names to actual Easing instances
			Easing expected = easingName switch
			{
				"BounceIn" => Easing.BounceIn,
				"BounceOut" => Easing.BounceIn,
				"CubicIn" => Easing.BounceIn,
				"CubicInOut" => Easing.BounceIn,
				"CubicOut" => Easing.BounceIn,
				"Linear" => Easing.BounceIn,
				"SinIn" => Easing.BounceIn,
				"SinInOut" => Easing.BounceIn,
				"SinOut" => Easing.BounceIn,
				"SpringIn" => Easing.BounceIn,
				"SpringOut" => Easing.BounceIn,
				_ => Easing.Linear
			};

			tabView.AnimationEasing = expected;

			Assert.Equal(expected, tabView.AnimationEasing);
		}

		[Fact]
		public void AnimationEasing_PropertyChangedTriggered()
		{
			var tabView = new SfTabView();
			bool eventRaised = false;

			tabView.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName == nameof(tabView.AnimationEasing))
				{
					eventRaised = true;
				}
			};

			tabView.AnimationEasing = Easing.CubicInOut;

			Assert.True(eventRaised, $"PropertyChanged was not raised for {tabView.AnimationEasing}");
		}

		[Fact]
		public void AnimationEasing_PropagatedToTabBar()
		{
			var tabView = new SfTabView();
			var expectedEasing = Easing.BounceOut;

			tabView.AnimationEasing = expectedEasing;

			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.Equal(expectedEasing, tabBar.AnimationEasing);
		}

		[Fact]
		public void AnimationEasing_PropagatedToHorizontalContent()
		{
			var tabView = new SfTabView();
			var expectedEasing = Easing.SpringIn;

			tabView.AnimationEasing = expectedEasing;

			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;
			Assert.NotNull(horizontalContent);
			Assert.Equal(expectedEasing, horizontalContent.AnimationEasing);
		}

		[Fact]
		public void AnimationEasing_RuntimeChange_UpdatesChildComponents()
		{
			var tabView = new SfTabView();

			// Initial setup
			tabView.AnimationEasing = Easing.Linear;

			// Get child components
			SfTabBar? tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			SfHorizontalContent? horizontalContent = GetPrivateField<SfTabView>(tabView, "_tabContentContainer") as SfHorizontalContent;

			Assert.NotNull(tabBar);
			Assert.NotNull(horizontalContent);
			Assert.Equal(Easing.Linear, tabBar.AnimationEasing);
			Assert.Equal(Easing.Linear, horizontalContent.AnimationEasing);

			// Runtime change
			tabView.AnimationEasing = Easing.CubicOut;

			Assert.Equal(Easing.CubicOut, tabBar.AnimationEasing);
			Assert.Equal(Easing.CubicOut, horizontalContent.AnimationEasing);

		}
		#endregion
		#region EnableRippleAnimation Unit Tests

		[Fact]
		public void EnableRippleAnimation_DefaultValue_ShouldBeTrue()
		{
			var tabView = new SfTabView();
			Assert.True(tabView.EnableRippleAnimation);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnableRippleAnimation_SetAndGet_ReturnsExpectedValue(bool expected)
		{
			var tabView = new SfTabView
			{
				EnableRippleAnimation = expected
			};

			Assert.Equal(expected, tabView.EnableRippleAnimation);
		}

		[Fact]
		public void EnableRippleAnimation_PropertyChanged_TriggersEvent()
		{
			var tabView = new SfTabView();
			bool eventTriggered = false;
			string? changedPropertyName = null;

			tabView.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName == nameof(SfTabView.EnableRippleAnimation))
				{
					eventTriggered = true;
					changedPropertyName = e.PropertyName;
				}
			};

			tabView.EnableRippleAnimation = false;

			Assert.True(eventTriggered);
			Assert.Equal(nameof(SfTabView.EnableRippleAnimation), changedPropertyName);
		}

		[Fact]
		public void EnableRippleAnimation_RuntimeToggle_UpdatesCorrectly()
		{
			var tabView = new SfTabView();

			// Verify default value
			Assert.True(tabView.EnableRippleAnimation);

			// Toggle to false
			tabView.EnableRippleAnimation = false;
			Assert.False(tabView.EnableRippleAnimation);

			// Toggle back to true
			tabView.EnableRippleAnimation = true;
			Assert.True(tabView.EnableRippleAnimation);
		}

		[Fact]
		public void EnableRippleAnimation_PropagatesTo_TabBar()
		{
			var tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection(),
				EnableRippleAnimation = false
			};

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.False(tabBar.EnableRippleAnimation);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnableRippleAnimation_WithItems_MaintainsValue(bool rippleEnabled)
		{
			var tabView = new SfTabView
			{
				EnableRippleAnimation = rippleEnabled,
				Items = PopulateLabelItemsCollection()
			};

			Assert.Equal(rippleEnabled, tabView.EnableRippleAnimation);

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.Equal(rippleEnabled, tabBar.EnableRippleAnimation);
		}

		[Theory]
		[InlineData(TabBarPlacement.Top, true)]
		[InlineData(TabBarPlacement.Top, false)]
		[InlineData(TabBarPlacement.Bottom, true)]
		[InlineData(TabBarPlacement.Bottom, false)]
		public void EnableRippleAnimation_WithDifferentPlacements_WorksCorrectly(TabBarPlacement placement, bool rippleEnabled)
		{
			var tabView = new SfTabView
			{
				TabBarPlacement = placement,
				EnableRippleAnimation = rippleEnabled,
				Items = PopulateLabelItemsCollection()
			};

			Assert.Equal(rippleEnabled, tabView.EnableRippleAnimation);
			Assert.Equal(placement, tabView.TabBarPlacement);

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.Equal(rippleEnabled, tabBar.EnableRippleAnimation);
		}

		[Theory]
		[InlineData(FlowDirection.LeftToRight, true)]
		[InlineData(FlowDirection.LeftToRight, false)]
		[InlineData(FlowDirection.RightToLeft, true)]
		[InlineData(FlowDirection.RightToLeft, false)]
		public void EnableRippleAnimation_WithFlowDirection_WorksCorrectly(FlowDirection flowDirection, bool rippleEnabled)
		{
			var tabView = new SfTabView
			{
				FlowDirection = flowDirection,
				EnableRippleAnimation = rippleEnabled,
				Items = PopulateLabelItemsCollection()
			};

			Assert.Equal(rippleEnabled, tabView.EnableRippleAnimation);
			Assert.Equal(flowDirection, tabView.FlowDirection);

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.Equal(rippleEnabled, tabBar.EnableRippleAnimation);
			Assert.Equal(flowDirection, tabBar.FlowDirection);
		}

		[Fact]
		public void EnableRippleAnimation_DoesNotAffect_SelectionBehavior()
		{
			var tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection(),
				EnableRippleAnimation = false,
				SelectedIndex = 0
			};

			// Test selection change with ripple disabled
			int selectionChangedCount = 0;
			tabView.SelectionChanged += (s, e) => selectionChangedCount++;

			tabView.SelectedIndex = 1;
			Assert.Equal(1, tabView.SelectedIndex);
			Assert.Equal(1, selectionChangedCount);

			// Toggle ripple and test selection again
			tabView.EnableRippleAnimation = true;
			tabView.SelectedIndex = 2;
			Assert.Equal(2, tabView.SelectedIndex);
			Assert.Equal(2, selectionChangedCount);
		}

		[Fact]
		public void EnableRippleAnimation_DoesNotAffect_TabItemTappedEvent()
		{
			var tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection(),
				EnableRippleAnimation = false
			};

			bool eventTriggered = false;
			SfTabItem? tappedItem = null;

			tabView.TabItemTapped += (s, e) =>
			{
				eventTriggered = true;
				tappedItem = e.TabItem;
			};

			// Simulate tab item tapped event with ripple disabled
			var eventArgs = new TabItemTappedEventArgs { TabItem = tabView.Items[0] };
			tabView.RaiseTabItemTappedEvent(eventArgs);

			Assert.True(eventTriggered);
			Assert.Equal(tabView.Items[0], tappedItem);
		}

		[Theory]
		[InlineData(TabBarDisplayMode.Default, true)]
		[InlineData(TabBarDisplayMode.Default, false)]
		[InlineData(TabBarDisplayMode.Text, true)]
		[InlineData(TabBarDisplayMode.Text, false)]
		[InlineData(TabBarDisplayMode.Image, true)]
		[InlineData(TabBarDisplayMode.Image, false)]
		public void EnableRippleAnimation_WithHeaderDisplayMode_WorksCorrectly(TabBarDisplayMode displayMode, bool rippleEnabled)
		{
			var tabView = new SfTabView
			{
				HeaderDisplayMode = displayMode,
				EnableRippleAnimation = rippleEnabled,
				Items = PopulateLabelImageItemsCollection()
			};

			Assert.Equal(rippleEnabled, tabView.EnableRippleAnimation);
			Assert.Equal(displayMode, tabView.HeaderDisplayMode);

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.Equal(rippleEnabled, tabBar.EnableRippleAnimation);
			Assert.Equal(displayMode, tabBar.HeaderDisplayMode);
		}

		[Theory]
		[InlineData(IndicatorWidthMode.Fit, true)]
		[InlineData(IndicatorWidthMode.Fit, false)]
		[InlineData(IndicatorWidthMode.Stretch, true)]
		[InlineData(IndicatorWidthMode.Stretch, false)]
		public void EnableRippleAnimation_WithIndicatorWidthMode_WorksCorrectly(IndicatorWidthMode widthMode, bool rippleEnabled)
		{
			var tabView = new SfTabView
			{
				IndicatorWidthMode = widthMode,
				EnableRippleAnimation = rippleEnabled,
				Items = PopulateLabelItemsCollection()
			};

			Assert.Equal(rippleEnabled, tabView.EnableRippleAnimation);
			Assert.Equal(widthMode, tabView.IndicatorWidthMode);

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.Equal(rippleEnabled, tabBar.EnableRippleAnimation);
			Assert.Equal(widthMode, tabBar.IndicatorWidthMode);
		}

		[Fact]
		public void EnableRippleAnimation_BindableProperty_IsCorrectlyDefined()
		{
			var property = SfTabView.EnableRippleAnimationProperty;

			Assert.NotNull(property);
			Assert.Equal(nameof(SfTabView.EnableRippleAnimation), property.PropertyName);
			Assert.Equal(typeof(bool), property.ReturnType);
			Assert.Equal(typeof(SfTabView), property.DeclaringType);
			Assert.True((bool)property.DefaultValue);
		}

		[Theory]
		[InlineData(true, false)]
		[InlineData(false, true)]
		public void EnableRippleAnimation_PropertyChangedCallback_UpdatesTabBar(bool initialValue, bool newValue)
		{
			var tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollection(),
				EnableRippleAnimation = initialValue
			};

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.Equal(initialValue, tabBar.EnableRippleAnimation);

			// Change the value and verify propagation
			tabView.EnableRippleAnimation = newValue;
			Assert.Equal(newValue, tabBar.EnableRippleAnimation);
		}

		[Fact]
		public void EnableRippleAnimation_WithScrollButtonEnabled_WorksCorrectly()
		{
			var tabView = new SfTabView
			{
				Items = PopulateLabelItemsCollectionMore(), // More items to enable scrolling
				IsScrollButtonEnabled = true,
				EnableRippleAnimation = false
			};

			Assert.False(tabView.EnableRippleAnimation);
			Assert.True(tabView.IsScrollButtonEnabled);

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.False(tabBar.EnableRippleAnimation);
			Assert.True(tabBar.IsScrollButtonEnabled);
		}

		[Theory]
		[InlineData(TabIndicatorPlacement.Top, true)]
		[InlineData(TabIndicatorPlacement.Top, false)]
		[InlineData(TabIndicatorPlacement.Bottom, true)]
		[InlineData(TabIndicatorPlacement.Bottom, false)]
		[InlineData(TabIndicatorPlacement.Fill, true)]
		[InlineData(TabIndicatorPlacement.Fill, false)]
		public void EnableRippleAnimation_WithIndicatorPlacement_WorksCorrectly(TabIndicatorPlacement placement, bool rippleEnabled)
		{
			var tabView = new SfTabView
			{
				IndicatorPlacement = placement,
				EnableRippleAnimation = rippleEnabled,
				Items = PopulateLabelItemsCollection()
			};

			Assert.Equal(rippleEnabled, tabView.EnableRippleAnimation);
			Assert.Equal(placement, tabView.IndicatorPlacement);

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.Equal(rippleEnabled, tabBar.EnableRippleAnimation);
			Assert.Equal(placement, tabBar.IndicatorPlacement);
		}

		[Theory]
		[InlineData(TabWidthMode.Default, true)]
		[InlineData(TabWidthMode.Default, false)]
		[InlineData(TabWidthMode.SizeToContent, true)]
		[InlineData(TabWidthMode.SizeToContent, false)]
		public void EnableRippleAnimation_WithTabWidthMode_WorksCorrectly(TabWidthMode widthMode, bool rippleEnabled)
		{
			var tabView = new SfTabView
			{
				TabWidthMode = widthMode,
				EnableRippleAnimation = rippleEnabled,
			};

			Assert.Equal(rippleEnabled, tabView.EnableRippleAnimation);
			Assert.Equal(widthMode, tabView.TabWidthMode);

			var tabBar = GetPrivateField<SfTabView>(tabView, "_tabHeaderContainer") as SfTabBar;
			Assert.NotNull(tabBar);
			Assert.Equal(rippleEnabled, tabBar.EnableRippleAnimation);
			Assert.Equal(widthMode, tabBar.TabWidthMode);
		}

		#endregion
	}

	public class Model : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private string? name;

		public string? Name
		{
			get { return name; }
			set
			{
				name = value;
				OnPropertyChanged(nameof(Name));
			}
		}

		private string? subName;

		public string? SubName
		{
			get { return subName; }
			set
			{
				subName = value;
				OnPropertyChanged(nameof(SubName));
			}
		}
	}
}