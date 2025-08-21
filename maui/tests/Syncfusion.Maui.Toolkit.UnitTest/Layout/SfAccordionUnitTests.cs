using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;
using Syncfusion.Maui.Toolkit.Accordion;
using Syncfusion.Maui.Toolkit.Expander;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfAccordionUnitTests : BaseUnitTest
	{
		#region Fields

		private readonly double _itemSpacing = 10.0;

		#endregion

		#region Constructor

		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem();
			Assert.NotNull(accordion);
			Assert.NotNull(accordionItem);
			Assert.Empty(accordion.Items);
			Assert.Equal(AccordionExpandMode.Single, accordion.ExpandMode);
			Assert.Equal(250d, accordion.AnimationDuration);
			Assert.Equal(0d, accordion.ItemSpacing);
			Assert.Equal(ExpanderAnimationEasing.Linear, accordion.AnimationEasing);
			Assert.Equal(AccordionAutoScrollPosition.MakeVisible, accordion.AutoScrollPosition);
			Assert.Equal(ExpanderIconPosition.End, accordion.HeaderIconPosition);
			Assert.Equal(Color.FromArgb("#49454F"), accordionItem.HeaderIconColor);
			Assert.False(accordionItem.IsExpanded);
			Assert.Null(accordionItem.Header);
			Assert.Null(accordionItem.Content);
		}

		#endregion

		#region Properties

		[Theory]
		[InlineData(AccordionExpandMode.Multiple)]
		[InlineData(AccordionExpandMode.Single)]
		[InlineData(AccordionExpandMode.MultipleOrNone)]
		[InlineData(AccordionExpandMode.SingleOrNone)]
		public void ExpandMode_SetAndGet_ReturnsExpectedValue(object expected)
		{
			var accordion = new SfAccordion
			{
				ExpandMode = (AccordionExpandMode)expected
			};

			Assert.Equal(expected, accordion.ExpandMode);
		}

		[Theory]
		[InlineData(AccordionExpandMode.Single)]
		[InlineData(AccordionExpandMode.Multiple)]
		[InlineData(AccordionExpandMode.MultipleOrNone)]
		[InlineData(AccordionExpandMode.SingleOrNone)]
		public void ExpandModePropertyWithBinding_SetAndGet_ReturnsExpectedValue(AccordionExpandMode expectedExpandMode)
		{
			var accordion = new SfAccordion();
			var bindingContext = new { Mode = expectedExpandMode };
			accordion.BindingContext = bindingContext;
			accordion.SetBinding(SfAccordion.ExpandModeProperty, new Binding("Mode"));
			Assert.Equal(expectedExpandMode, accordion.ExpandMode);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(300)]
		[InlineData(200)]
		[InlineData(150)]
		public void AnimationDuration_SetAndGet_ReturnsExpectedValue(double expectedValue)
		{
			var accordion = new SfAccordion { AnimationDuration = expectedValue };
			Assert.Equal(expectedValue, accordion.AnimationDuration);
		}

		[Theory]
		[InlineData(250)]
		[InlineData(300)]
		[InlineData(500)]
		public void AnimationDurationPropertyWithBinding_SetAndGet_ReturnsExpectedValue(double expectedDuration)
		{
			var accordion = new SfAccordion();
			var bindingContext = new { Duration = expectedDuration };
			accordion.BindingContext = bindingContext;
			accordion.SetBinding(SfAccordion.AnimationDurationProperty, new Binding("Duration"));
			Assert.Equal(expectedDuration, accordion.AnimationDuration);
		}


		[Theory]
		[InlineData(0)]
		[InlineData(10)]
		[InlineData(100)]
		public void ItemSpacing_SetAndGet_ReturnsExpectedValue(double expectedValue)
		{
			var accordion = new SfAccordion { ItemSpacing = expectedValue };
			Assert.Equal(expectedValue, accordion.ItemSpacing);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(10)]
		[InlineData(25.5)]
		public void ItemSpacingPropertyWithBinding_SetAndGet_ReturnsExpectedValue(double expectedSpacing)
		{
			var accordion = new SfAccordion();
			var bindingContext = new { Spacing = expectedSpacing };
			accordion.BindingContext = bindingContext;
			accordion.SetBinding(SfAccordion.ItemSpacingProperty, new Binding("Spacing"));
			Assert.Equal(expectedSpacing, accordion.ItemSpacing);
		}

		[Theory]
		[MemberData(nameof(ItemsPropertyTestData))]
		public void ItemsProperty_SetAndGet_ReturnsExpectedValue(ObservableCollection<AccordionItem> expectedValue)
		{
			var accordion = new SfAccordion
			{
				Items = expectedValue
			};

			Assert.Equal(expectedValue, accordion.Items);
		}

		[Theory]
		[InlineData(typeof(ObservableCollection<AccordionItem>))]
		public void ItemsPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type collectionType)
		{
			var accordion = new SfAccordion();
			var itemsCollection = (ObservableCollection<AccordionItem>?)Activator.CreateInstance(collectionType);
			itemsCollection?.Add(new AccordionItem
			{
				Header = new Label { Text = "Header 1" },
				Content = new Label { Text = "Content 1" }
			});

			var bindingContext = new { ItemsCollection = itemsCollection };
			accordion.BindingContext = bindingContext;
			accordion.SetBinding(SfAccordion.ItemsProperty, new Binding("ItemsCollection"));
			Assert.Equal(itemsCollection, accordion.Items);
		}

		public static IEnumerable<object?[]> ItemsPropertyTestData()
		{
			yield return new object?[]
			{
				null
			};

			yield return new object[]
			{
				new ObservableCollection<AccordionItem>()
			};

			yield return new object[]
			{
				new ObservableCollection<AccordionItem>
				{
					new() {
						Header = new Label { Text = "Header 1" },
						Content = new Label { Text = "Content 1" }
					},
					new() {
					Header = new Label { Text = "Header 2" },
					Content = new Label { Text = "Content 2" }
					}
				}
			};

			yield return new object[]
			{
				new ObservableCollection<AccordionItem>
				{
					new() {
						Header = new Button { Text = "Header Button" },
						Content = new StackLayout
						{
							Children =
							{
								new Label { Text = "Content in StackLayout" },
								new Button { Text = "Button in Content" }
							}
						}
					}
				}		
			};
		}

		[Theory]
		[InlineData(ExpanderAnimationEasing.Linear)]
		[InlineData(ExpanderAnimationEasing.SinIn)]
		[InlineData(ExpanderAnimationEasing.SinOut)]
		[InlineData(ExpanderAnimationEasing.SinInOut)]
		[InlineData(ExpanderAnimationEasing.None)]
		public void AnimationEasing_SetAndGet_ReturnsExpectedValue(ExpanderAnimationEasing expectedValue)
		{
			var accordion = new SfAccordion { AnimationEasing = expectedValue };
			Assert.Equal(expectedValue, accordion.AnimationEasing);
		}

		[Theory]
		[InlineData(ExpanderAnimationEasing.Linear)]
		[InlineData(ExpanderAnimationEasing.SinIn)]
		[InlineData(ExpanderAnimationEasing.SinOut)]
		[InlineData(ExpanderAnimationEasing.SinInOut)]
		[InlineData(ExpanderAnimationEasing.None)]
		public void AnimationEasingPropertyWithBinding_SetAndGet_ReturnsExpectedValue(ExpanderAnimationEasing expectedEasing)
		{
			var accordion = new SfAccordion();
			var bindingContext = new { Easing = expectedEasing };
			accordion.BindingContext = bindingContext;
			accordion.SetBinding(SfAccordion.AnimationEasingProperty, new Binding("Easing"));
			Assert.Equal(expectedEasing, accordion.AnimationEasing);
		}

		[Theory]
		[InlineData(AccordionAutoScrollPosition.None)]
		[InlineData(AccordionAutoScrollPosition.MakeVisible)]
		[InlineData(AccordionAutoScrollPosition.Top)]
		public void AutoScrollPosition_SetAndGet_ReturnsExpectedValue(AccordionAutoScrollPosition expectedValue)
		{
			var accordion = new SfAccordion { AutoScrollPosition = expectedValue };
			Assert.Equal(expectedValue, accordion.AutoScrollPosition);
		}

		[Theory]
		[InlineData(AccordionAutoScrollPosition.MakeVisible)]
		[InlineData(AccordionAutoScrollPosition.Top)]
		[InlineData(AccordionAutoScrollPosition.None)]
		public void AutoScrollPositionPropertyWithBinding_SetAndGet_ReturnsExpectedValue(AccordionAutoScrollPosition expectedPosition)
		{
			var accordion = new SfAccordion();
			var bindingContext = new { ScrollPosition = expectedPosition };
			accordion.BindingContext = bindingContext;
			accordion.SetBinding(SfAccordion.AutoScrollPositionProperty, new Binding("ScrollPosition"));
			Assert.Equal(expectedPosition, accordion.AutoScrollPosition);
		}

		[Theory]
		[InlineData(ExpanderIconPosition.End)]
		[InlineData(ExpanderIconPosition.Start)]
		[InlineData(ExpanderIconPosition.None)]
		public void HeaderIconPosition_SetAndGet_ReturnsExpectedValue(ExpanderIconPosition expectedValue)
		{
			var accordion = new SfAccordion { HeaderIconPosition = expectedValue };
			Assert.Equal(expectedValue, accordion.HeaderIconPosition);
		}

		[Theory]
		[InlineData(ExpanderIconPosition.Start)]
		[InlineData(ExpanderIconPosition.End)]
		[InlineData(ExpanderIconPosition.None)]
		public void HeaderIconPositionPropertyWithBinding_SetAndGet_ReturnsExpectedValue(ExpanderIconPosition expectedPosition)
		{
			var accordion = new SfAccordion();
			var bindingContext = new { IconPosition = expectedPosition };
			accordion.BindingContext = bindingContext;
			accordion.SetBinding(SfAccordion.HeaderIconPositionProperty, new Binding("IconPosition"));
			Assert.Equal(expectedPosition, accordion.HeaderIconPosition);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsViewLoaded_SetAndGet_ReturnsExpectedValue(bool expectedValue)
		{
			var accordion = new SfAccordion { IsViewLoaded = expectedValue };
			Assert.Equal(expectedValue, accordion.IsViewLoaded);
		}

		[Theory]
		[InlineData(null, null)]
		[InlineData("Header 1", "Content 1")]
		[InlineData("Header 2", "Content 2")]
		[InlineData("Header Button", "Content in StackLayout")]
		public void CurrentItem_SetAndGet_ReturnsExpectedValue(string? header, string? content)
		{
			AccordionItem? expectedItem = null;
			if (header != null && content != null)
			{
				expectedItem = new AccordionItem
				{
					Header = header == "Header Button" ?
								new Button { Text = header } :
								new Label { Text = header },
					Content = header == "Header Button" ?
								new StackLayout
								{
									Children =
									{
								new Label { Text = content },
								new Button { Text = "Button in Content" }
									}
								} :
								new Label { Text = content }
				};
			}
			var accordion = new SfAccordion { CurrentItem = expectedItem };
			Assert.Equal(expectedItem, accordion.CurrentItem);
		}

		[Theory]
		[InlineData(null)]
		[InlineData("Description")]
		public void SemanticDescription_SetAndGet_ReturnsExpectedValue(string? expectedValue)
		{
			var accordion = new SfAccordion();
			accordion.GetType().GetProperty("SemanticDescription", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
				   .SetValue(accordion, expectedValue);
			var actualValue = accordion.GetType().GetProperty("SemanticDescription", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
									   .GetValue(accordion);
			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public void AccordionItemProperty_SetAndGet_ReturnsExpectedValue()
		{
			var accordionItemView = new AccordionItemView();
			var accordionItem = new AccordionItem();
			var propertyInfo = typeof(AccordionItemView).GetProperty("AccordionItem", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException("Property 'AccordionItem' not found.");
			propertyInfo.SetValue(accordionItemView, accordionItem);
			var result = propertyInfo.GetValue(accordionItemView);
			Assert.Equal(accordionItem, result);
		}

		[Fact]
		public void Accordion_SetAndGet_ReturnsExpectedValue()
		{
			var accordionItemView = new AccordionItemView();
			var accordion = new SfAccordion();
			var propertyInfo = typeof(AccordionItemView).GetProperty("Accordion", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException("Property 'Accordion' not found.");
			propertyInfo.SetValue(accordionItemView, accordion);
			var result = propertyInfo.GetValue(accordionItemView);
			Assert.Equal(accordion, result);
		}

		[Theory]
		[InlineData(null)]
		[InlineData(typeof(ContentView))]
		public void HeaderProperty_SetAndGet_ReturnsExpectedValue(object? headerValue)
		{
			var accordionItemView = new AccordionItemView();
			View? headerView = headerValue as View;
			if (headerView != null)
			{
				accordionItemView.Header = headerView;
			}

			Assert.Equal(headerView, accordionItemView.Header);
		}

		[Theory]
		[InlineData(null)]
		[InlineData(typeof(ContentView))]
		public void ContentProperty_SetAndGet_ReturnsExpectedValue(object? contentValue)
		{
			var accordionItemView = new AccordionItemView();
			View? contentView = contentValue as View;
			if (contentView != null)
			{
				accordionItemView.Content = contentView;
			}

			Assert.Equal(contentView, accordionItemView.Content);
		}

		[Theory]
		[InlineData(typeof(ContentView))]
		[InlineData(typeof(Label))]
		public void HeaderPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type headerType)
		{
			var accordionItemView = new AccordionItemView();
			var headerView = Activator.CreateInstance(headerType) as View;
			var bindingContext = new { HeaderView = headerView };
			accordionItemView.BindingContext = bindingContext;
			accordionItemView.SetBinding(AccordionItemView.HeaderProperty, new Binding("HeaderView"));
			Assert.Equal(headerView, accordionItemView.Header);
		}

		[Theory]
		[InlineData(typeof(ContentView))]
		[InlineData(typeof(Label))]
		public void ContentPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type contentType)
		{
			var accordionItemView = new AccordionItemView();
			var contentView = (View?)Activator.CreateInstance(contentType);
			var bindingContext = new { ContentView = contentView };
			accordionItemView.BindingContext = bindingContext;
			accordionItemView.SetBinding(AccordionItemView.ContentProperty, new Binding("ContentView"));
			Assert.Equal(contentView, accordionItemView.Content);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsExpandedProperty_SetAndGet_ReturnsExpectedValue(bool isExpandedValue)
		{
			var accordionItemView = new AccordionItemView
			{
				IsExpanded = isExpandedValue
			};

			Assert.Equal(isExpandedValue, accordionItemView.IsExpanded);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsExpandedPropertyWithBinding_SetAndGet_ReturnsExpectedValue(bool isExpandedValue)
		{
			var accordionItemView = new AccordionItemView();
			var bindingContext = new { IsExpandedValue = isExpandedValue };
			accordionItemView.BindingContext = bindingContext;
			accordionItemView.SetBinding(AccordionItemView.IsExpandedProperty, new Binding("IsExpandedValue"));
			Assert.Equal(isExpandedValue, accordionItemView.IsExpanded);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		[InlineData(0, 0, 255)]
		public void HeaderBackgroundProperty_SetAndGetRgbColor_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var accordionItemView = new AccordionItemView();
			var expectedColor = Color.FromRgb(r, g, b);
			var expectedBrush = new SolidColorBrush(expectedColor);
			accordionItemView.HeaderBackground = expectedBrush;
			Assert.Equal(expectedBrush, accordionItemView.HeaderBackground);
		}

		[Theory]
		[InlineData(typeof(SolidColorBrush))]
		[InlineData(typeof(LinearGradientBrush))]
		public void HeaderBackgroundPropertyWithBinding_SetAndGet_ReturnsExpectedValue(Type brushType)
		{
			var accordionItemView = new AccordionItemView();
			var brush = (Brush?)Activator.CreateInstance(brushType);
			var bindingContext = new { HeaderBackgroundBrush = brush };
			accordionItemView.BindingContext = bindingContext;
			accordionItemView.SetBinding(AccordionItemView.HeaderBackgroundProperty, new Binding("HeaderBackgroundBrush"));
			Assert.Equal(brush, accordionItemView.HeaderBackground);
		}

		[Theory]
		[InlineData("#49454F")]
		[InlineData("#FF5733")]
		[InlineData("#33FF57")]
		[InlineData("#5733FF")]
		public void HeaderIconColorProperty_SetAndGet_ReturnsExpectedValue(string colorHex)
		{
			var accordionItem = new AccordionItem();
			var expectedColor = Color.FromArgb(colorHex);
			accordionItem.HeaderIconColor = expectedColor;
			Assert.Equal(expectedColor, accordionItem.HeaderIconColor);
		}

		[Theory]
		[InlineData(73, 69, 79)]
		[InlineData(255, 87, 51)]
		[InlineData(51, 255, 87)]
		[InlineData(87, 51, 255)]
		public void HeaderIconColorProperty_SetAndGetRGB_ReturnsExpectedValue(byte r, byte g, byte b)
		{
			var accordionItem = new AccordionItem();
			var expectedColor = Color.FromRgb(r, g, b);
			accordionItem.HeaderIconColor = expectedColor;
			Assert.Equal(r / 255.0, accordionItem.HeaderIconColor.Red, precision: 6);
			Assert.Equal(g / 255.0, accordionItem.HeaderIconColor.Green, precision: 6);
			Assert.Equal(b / 255.0, accordionItem.HeaderIconColor.Blue, precision: 6);
		}

		[Theory]
		[InlineData(0xFFFF0000)]
		[InlineData(0xFF00FF00)]
		[InlineData(0xFF0000FF)]
		[InlineData(0xFF000000)]
		[InlineData(0xFFFFFFFF)]
		public void HeaderIconColor_SetAndGetVariousColors(uint expectedColorValue)
		{
			var accordionItem = new AccordionItem();
			var expectedColor = Color.FromRgb(
				(byte)(expectedColorValue >> 16),
				(byte)(expectedColorValue >> 8),
				(byte)(expectedColorValue)
			);

			accordionItem.HeaderIconColor = expectedColor;
			Assert.Equal(expectedColor, accordionItem.HeaderIconColor);
		}

		[Theory]
		[InlineData("#49454F")]
		[InlineData("#FF5733")]
		[InlineData("#33FF57")]
		[InlineData("#5733FF")]
		public void HeaderIconColorPropertyWithBinding_SetAndGet_ReturnsExpectedValue(string expectedColorHex)
		{
			var accordionItem = new AccordionItem();
			var expectedColor = Color.FromArgb(expectedColorHex);
			var bindingContext = new { IconColor = expectedColor };
			accordionItem.BindingContext = bindingContext;
			accordionItem.SetBinding(AccordionItem.HeaderIconColorProperty, new Binding("IconColor"));
			Assert.Equal(expectedColor, accordionItem.HeaderIconColor);
		}

		[Fact]
		public void ItemsProperty_SetItems_TriggersOnItemsPropertyChanged()
		{
			var accordion = new SfAccordion();
			var accordionItem1 = new AccordionItem { _itemIndex = 0 };
			var accordionItem2 = new AccordionItem { _itemIndex = 1 };
			var items = new ObservableCollection<AccordionItem> { accordionItem1, accordionItem2 };
			accordion.SetValue(SfAccordion.ItemsProperty, items);
			var itemsFromProperty = accordion.GetValue(SfAccordion.ItemsProperty) as ObservableCollection<AccordionItem>;
			Assert.NotNull(itemsFromProperty);
			Assert.Equal(2, itemsFromProperty.Count);
		}

		[Fact]
		public void AccordionStyle_WhenApplied_SetsCorrectProperties()
		{
			// Arrange
			var animationDuration = 100;
			ExpanderAnimationEasing easing = new ExpanderAnimationEasing();
			easing = ExpanderAnimationEasing.SinInOut;
			ExpanderIconPosition expanderIconPosition = new ExpanderIconPosition();
			expanderIconPosition = ExpanderIconPosition.Start;
			var itemSpacing = 100;
			AccordionExpandMode expandMode = new AccordionExpandMode();
			expandMode = AccordionExpandMode.Multiple;
			AccordionAutoScrollPosition autoScrollPosition = new AccordionAutoScrollPosition();
			autoScrollPosition = AccordionAutoScrollPosition.MakeVisible;
			var style = new Style(typeof(SfAccordion));
			style.Setters.Add(new Setter
			{
				Property = SfAccordion.AnimationDurationProperty,
				Value = animationDuration
			});
			style.Setters.Add(new Setter
			{
				Property = SfAccordion.HeaderIconPositionProperty,
				Value = expanderIconPosition
			});
			style.Setters.Add(new Setter
			{
				Property = SfAccordion.AnimationEasingProperty,
				Value = easing
			});
			style.Setters.Add(new Setter
			{
				Property = SfAccordion.ExpandModeProperty,
				Value = expandMode
			});
			style.Setters.Add(new Setter
			{
				Property = SfAccordion.ItemSpacingProperty,
				Value = itemSpacing
			});
			style.Setters.Add(new Setter
			{
				Property = SfAccordion.AutoScrollPositionProperty,
				Value = autoScrollPosition
			});
			var resources = new ResourceDictionary();
			resources.Add("AccordionStyle", style);
			Application.Current = new Application();
			Application.Current.Resources = resources;
			var accordion = new SfAccordion();
			// Act
			accordion.Style = (Style)Application.Current.Resources["AccordionStyle"];
			// Assert
			Assert.Equal(animationDuration, accordion.AnimationDuration);
			Assert.Equal(expanderIconPosition, accordion.HeaderIconPosition);
			Assert.Equal(easing, accordion.AnimationEasing);
			Assert.Equal(expandMode, accordion.ExpandMode);
			Assert.Equal(animationDuration, accordion.AnimationDuration);
			Assert.Equal(itemSpacing, accordion.ItemSpacing);
			Assert.Equal(autoScrollPosition, accordion.AutoScrollPosition);
		}

		[Fact]
		public void AnimationDuration_SetValue_Runtime_ShouldUpdateProperty()
		{
			var accordion = new SfAccordion { AnimationDuration = 200 };
			Assert.Equal(200, accordion.AnimationDuration);
			accordion.AnimationDuration = 500;
			Assert.Equal(500, accordion.AnimationDuration);
		}

		[Fact]
		public void AnimationEasing_SetValue_Runtime_ShouldUpdateProperty()
		{
			var accordion = new SfAccordion { AnimationEasing = ExpanderAnimationEasing.SinIn };
			Assert.Equal(ExpanderAnimationEasing.SinIn, accordion.AnimationEasing);
			accordion.AnimationEasing = ExpanderAnimationEasing.Linear;
			Assert.Equal(ExpanderAnimationEasing.Linear, accordion.AnimationEasing);
		}

		[Fact]
		public void AutoScrollPosition_SetValue_Runtime_ShouldUpdateProperty()
		{
			var accordion = new SfAccordion { AutoScrollPosition = AccordionAutoScrollPosition.Top };
			Assert.Equal(AccordionAutoScrollPosition.Top, accordion.AutoScrollPosition);
			accordion.AutoScrollPosition = AccordionAutoScrollPosition.None;
			Assert.Equal(AccordionAutoScrollPosition.None, accordion.AutoScrollPosition);
		}

		[Fact]
		public void HeaderIconPosition_SetValue_Runtime_ShouldUpdateProperty()
		{
			var accordion = new SfAccordion { HeaderIconPosition = ExpanderIconPosition.Start };
			Assert.Equal(ExpanderIconPosition.Start, accordion.HeaderIconPosition);
			accordion.HeaderIconPosition = ExpanderIconPosition.End;
			Assert.Equal(ExpanderIconPosition.End, accordion.HeaderIconPosition);
		}

		[Fact]
		public void ExpandMode_SetValue_Runtime_ShouldUpdateProperty()
		{
			var accordion = new SfAccordion { ExpandMode = AccordionExpandMode.Multiple };
			Assert.Equal(AccordionExpandMode.Multiple, accordion.ExpandMode);
			accordion.ExpandMode = AccordionExpandMode.SingleOrNone;
			Assert.Equal(AccordionExpandMode.SingleOrNone, accordion.ExpandMode);
		}

		[Fact]
		public void ItemSpacing_SetValue_Runtime_ShouldUpdateProperty()
		{
			var accordion = new SfAccordion { ItemSpacing = 100 };
			Assert.Equal(100, accordion.ItemSpacing);
			accordion.ItemSpacing = 200;
			Assert.Equal(200, accordion.ItemSpacing);
		}

		[Fact]
		public void HeaderBackground_SetValue_Runtime_ShouldUpdateProperty()
		{
			var accordion = new AccordionItem { HeaderBackground = Colors.Green };
			Assert.Equal(Colors.Green, accordion.HeaderBackground);
			accordion.HeaderBackground = Colors.Blue;
			Assert.Equal(Colors.Blue, accordion.HeaderBackground);
		}

		[Fact]
		public void HeaderIconColor_SetValue_Runtime_ShouldUpdateProperty()
		{
			var accordion = new AccordionItem { HeaderIconColor = Colors.Green };
			Assert.Equal(Colors.Green, accordion.HeaderIconColor);
			accordion.HeaderIconColor = Colors.Blue;
			Assert.Equal(Colors.Blue, accordion.HeaderIconColor);
		}

		[Fact]
		public void IsExpanded_SetValue_Runtime_ShouldUpdateProperty()
		{
			var accordion = new AccordionItem { IsExpanded = true };
			Assert.True(accordion.IsExpanded);
			accordion.IsExpanded = false;
			Assert.False(accordion.IsExpanded);
		}

		[Fact]
		public void AccordionItemStyle_WhenApplied_SetsCorrectProperties()
		{
			Color color = new Color();
			color = Color.FromRgba("#233434");
			Color color2 = new Color();
			color2 = Color.FromRgba("#FF0000");
			var style = new Style(typeof(Syncfusion.Maui.Toolkit.Accordion.AccordionItem));
			style.Setters.Add(new Setter
			{
				Property = Syncfusion.Maui.Toolkit.Accordion.AccordionItem.HeaderBackgroundProperty,
				Value = color
			});
			style.Setters.Add(new Setter
			{
				Property = Syncfusion.Maui.Toolkit.Accordion.AccordionItem.HeaderIconColorProperty,
				Value = color2
			});
			style.Setters.Add(new Setter
			{
				Property = Syncfusion.Maui.Toolkit.Accordion.AccordionItem.IsExpandedProperty,
				Value = true
			});
			var resources = new ResourceDictionary();
			resources.Add("AccordionStyle", style);
			Application.Current = new Application();
			Application.Current.Resources = resources;
			var accordion = new Syncfusion.Maui.Toolkit.Accordion.AccordionItem();
			// Act
			accordion.Style = (Style)Application.Current.Resources["AccordionStyle"];
			Assert.Equal(color, accordion.HeaderBackground);
			Assert.Equal(color2, accordion.HeaderIconColor);
			Assert.True(accordion.IsExpanded);
		}

		[Fact]
		public void SfAccordion_ShouldBeAddedToStackLayout()
		{
			// Arrange
			var stackLayout = new StackLayout();
			var accordion = new SfAccordion
			{
				ExpandMode = AccordionExpandMode.Single
			};
			var label = new Label
			{
				Text = "Accordion Content",
			};
			var accordionItem = new AccordionItem
			{
				Header = new Label { Text = "Accordion Header", AutomationId = "accordionHeader" },
				Content = label
			};
			// Act
			accordion.Items.Add(accordionItem);
			stackLayout.Children.Add(accordion);
			// Assert
			Assert.Contains(accordion, stackLayout.Children);
		}

		[Fact]
		public void SfAccordion_ShouldBeAddedToAbsoluteLayout()
		{
			// Arrange
			var absoluteLayout = new StackLayout();
			var accordion = new SfAccordion
			{
				ExpandMode = AccordionExpandMode.Single
			};
			var label = new Label
			{
				Text = "Accordion Content",
			};
			var accordionItem = new AccordionItem
			{
				Header = new Label { Text = "Accordion Header", AutomationId = "accordionHeader" },
				Content = label
			};
			// Act
			accordion.Items.Add(accordionItem);
			absoluteLayout.Children.Add(accordion);
			// Assert
			Assert.Contains(accordion, absoluteLayout.Children);
		}
		[Fact]
		public void SfAccordion_ShouldBeAddedToHorizontalStackLayout()
		{
			// Arrange
			var stackLayout = new HorizontalStackLayout();
			var accordion = new SfAccordion
			{
				ExpandMode = AccordionExpandMode.Single
			};
			var label = new Label
			{
				Text = "Accordion Content",
			};
			var accordionItem = new AccordionItem
			{
				Header = new Label { Text = "Accordion Header", AutomationId = "accordionHeader" },
				Content = label
			};
			// Act
			accordion.Items.Add(accordionItem);
			stackLayout.Children.Add(accordion);
			// Assert
			Assert.Contains(accordion, stackLayout.Children);
		}
		[Fact]
		public void SfAccordion_ShouldBeAddedToFlexLayout()
		{
			// Arrange
			var flexLayout = new FlexLayout();
			var accordion = new SfAccordion
			{
				ExpandMode = AccordionExpandMode.Single
			};
			var label = new Label
			{
				Text = "Accordion Content",
			};
			var accordionItem = new AccordionItem
			{
				Header = new Label { Text = "Accordion Header", AutomationId = "accordionHeader" },
				Content = label
			};
			// Act
			accordion.Items.Add(accordionItem);
			flexLayout.Children.Add(accordion);
			// Assert
			Assert.Contains(accordion, flexLayout.Children);
		}
		[Fact]
		public void SfAccordion_ShouldBeAddedToGrid()
		{
			// Arrange
			var grid = new Grid();
			var accordion = new SfAccordion
			{
				ExpandMode = AccordionExpandMode.Single
			};
			var label = new Label
			{
				Text = "Accordion Content",
			};
			var accordionItem = new AccordionItem
			{
				Header = new Label { Text = "Accordion Header", AutomationId = "accordionHeader" },
				Content = label
			};
			// Act
			accordion.Items.Add(accordionItem);
			grid.Children.Add(accordion);
			// Assert
			Assert.Contains(accordion, grid.Children);
		}

		#endregion

		#region Methods

		[Theory]
		[InlineData(false, true)]
		public void UpdateAccordionItems_SingleExpandMode_ExpandsFirstItem(bool initialState, bool expectedState)
		{
			var accordion = new SfAccordion
			{
				Items =
				[
					new AccordionItem
					{
						_accordionItemView = new AccordionItemView { IsExpanded = initialState }
					}
				],
				ExpandMode = AccordionExpandMode.Single
			};

			accordion.IsViewLoaded = true;
			accordion.UpdateAccordionItemsBasedOnExpandModes(false);
			Assert.Equal(expectedState, accordion.Items[0]._accordionItemView?.IsExpanded);
		}

		[Theory]
		[InlineData(true, true, true, false)]
		public void UpdateAccordionItemsBasedOnExpandModes_MultipleItemsExpanded(
			bool firstItemInitialState, bool secondItemInitialState,
			bool firstItemExpectedState, bool secondItemExpectedState)
		{
			var accordion = new SfAccordion
			{
				Items =
					[
						new AccordionItem
						{
							_accordionItemView = new AccordionItemView { IsExpanded = firstItemInitialState }
						},
						new AccordionItem
						{
							_accordionItemView = new AccordionItemView { IsExpanded = secondItemInitialState }
						}
					],
				ExpandMode = AccordionExpandMode.Single
			};

			var accordionItem = new AccordionItem();
			accordion.IsViewLoaded = true;
			accordion.UpdateAccordionItemsBasedOnExpandModes(false);
			Assert.Equal(firstItemExpectedState, accordion.Items[0]._accordionItemView?.IsExpanded);
			Assert.Equal(secondItemExpectedState, accordion.Items[1]._accordionItemView?.IsExpanded);
		}

		[Theory]
		[InlineData(0, false)]
		[InlineData(1, true)]
		public void RaiseCollapsingEvent_ShouldReturnExpectedResult(int index, bool expectedCancel)
		{
			var accordion = new SfAccordion();
			accordion.Collapsing += (sender, args) =>
			{
				args.Cancel = expectedCancel;
			};

			var result = accordion.RaiseCollapsingEvent(index);
			Assert.Equal(!expectedCancel, result);
		}

		[Theory]
		[InlineData(0, false)]
		[InlineData(1, true)]
		public void RaiseExpandingEvent_ShouldReturnExpectedResult(int index, bool expectedCancel)
		{
			var accordion = new SfAccordion();
			accordion.Expanding += (sender, args) =>
			{
				args.Cancel = expectedCancel;
			};

			var result = accordion.RaiseExpandingEvent(index);
			Assert.Equal(!expectedCancel, result);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(5)]
		[InlineData(10)]
		public void RaiseCollapsedEvent_CollapsedEventTriggered_CorrectArgumentsPassed(int expectedIndex)
		{
			var accordion = new SfAccordion();
			var eventTriggered = false;
			var eventIndex = -1;
			accordion.Collapsed += (sender, args) =>
			{
				eventTriggered = true;
				eventIndex = args.Index;
			};

			accordion.RaiseCollapsedEvent(expectedIndex);
			Assert.True(eventTriggered, "The Collapsed event was not triggered.");
			Assert.Equal(expectedIndex, eventIndex);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(5)]
		[InlineData(10)]
		public void RaiseExpandedEvent_ExpandedEventTriggered_CorrectArgumentsPassed(int expectedIndex)
		{
			var accordion = new SfAccordion();
			var eventTriggered = false;
			var eventIndex = -1;
			accordion.Expanded += (sender, args) =>
			{
				eventTriggered = true;
				eventIndex = args.Index;
			};

			accordion.RaiseExpandedEvent(expectedIndex);
			Assert.True(eventTriggered, "The Expanded event was not triggered.");
			Assert.Equal(expectedIndex, eventIndex);
		}

		[Fact]
		public void ResetAccordionItemPadding_ShouldResetPaddingToZero()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem
			{
				_accordionItemView = new AccordionItemView
				{
					Padding = new Thickness(10, 20, 30, 40)
				}
			};

			InvokePrivateMethod(accordion, "ResetAccordionItemPadding", accordionItem);
			Assert.Equal(new Thickness(0, 0, 0, 0), accordionItem._accordionItemView?.Padding);
		}

		[Fact]
		public void ResetAccordionItemPadding_ShouldNotThrow_WhenAccordionItemViewIsNull()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem();
			InvokePrivateMethod(accordion, "ResetAccordionItemPadding", accordionItem);
			Assert.Null(accordionItem._accordionItemView);
		}

		[Fact]
		public void UpdateAccordionItemPadding_ShouldUpdatePaddingToNewBottom()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem
			{
				_accordionItemView = new AccordionItemView
				{
					Padding = new Thickness(5, 5, 5, 5)
				}
			};

			InvokePrivateMethod(accordion, "UpdateAccordionItemPadding", accordionItem);
			var expectedPadding = new Thickness(5, 5, 5, _itemSpacing);
			var actualPadding = accordionItem._accordionItemView?.Padding;
			Assert.Equal(expectedPadding.Top, actualPadding?.Top);
			Assert.Equal(expectedPadding.Left, actualPadding?.Left);
			Assert.Equal(expectedPadding.Right, actualPadding?.Right);
		}

		[Fact]
		public void UpdateAccordionItemPadding_WithItemSpacing()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem();
			accordion.ItemSpacing = 10;
			accordionItem._accordionItemView = new AccordionItemView
			{
				Padding = new Thickness(0, 0, 0, 0)
			};

			InvokePrivateMethod(accordion, "UpdateAccordionItemPadding", accordionItem);
			var updatedPadding = accordionItem._accordionItemView.Padding;
			Assert.Equal(10, updatedPadding.Bottom);
		}

		[Fact]
		public void UpdateAccordionProperties_ShouldUpdateAccordionItemViewProperties()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem
			{
				_itemIndex = 0,
				Header = new Label { Text = "Test Header" },
				Content = new Label { Text = "Test Content" },
				HeaderBackground = Colors.Gray,
				HeaderIconColor = Colors.White,
				IsExpanded = true,
				IsEnabled = true
			};

			var accordionItemView = new AccordionItemView();
			accordionItem._accordionItemView = accordionItemView;
			InvokePrivateMethod(accordion, "UpdateAccordionProperties", accordionItem);
			Assert.Equal(accordionItem.Header, accordionItemView.Header);
			Assert.Equal(accordionItem.HeaderIconColor, accordionItemView.HeaderIconColor);
			Assert.Equal(accordionItem.IsExpanded, accordionItemView.IsExpanded);
			Assert.Equal(accordion.AnimationDuration, accordionItemView.AnimationDuration);
			Assert.Equal(accordion.AnimationEasing, accordionItemView.AnimationEasing);
			Assert.Equal(accordion.HeaderIconPosition, accordionItemView.HeaderIconPosition);
			Assert.Equal(accordionItem.IsEnabled, accordionItemView.IsEnabled);
			Assert.Equal(accordionItem.Header.BindingContext, accordionItemView.Header.BindingContext);
			Assert.Equal(accordionItem.Content.BindingContext, accordionItemView.Content?.BindingContext);
		}

		[Fact]
		public void InitializeAccordion_ShouldInitializeFieldsCorrectly()
		{
			var accordion = new SfAccordion();
			InvokePrivateMethod(accordion, "InitializeAccordion");
			var itemContainer = GetPrivateField(accordion, "_itemContainer");
			Assert.NotNull(itemContainer);
			Assert.IsType<ItemContainer>(itemContainer);
			var scrollView = GetPrivateField(accordion, "_scrollView");
			Assert.NotNull(scrollView);
			Assert.IsType<AccordionScrollView>(scrollView);
		}

		[Fact]
		public void InitializeAccordion_Invoked_PropertiesInitializedCorrectly()
		{
			var accordion = new SfAccordion();
			var methodInfo = typeof(SfAccordion).GetMethod("InitializeAccordion", BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(methodInfo);
			methodInfo.Invoke(accordion, null);
			Assert.NotNull(accordion.Items);
			Assert.Empty(accordion.Items);
			Assert.Equal(Colors.Transparent, accordion.Background);
		}

		[Fact]
		public void OnAccordionLoaded_NotLoaded_AddsItemsAndSetsViewLoaded()
		{
			var accordion = new SfAccordion();
			SetNonPublicProperty(accordion, "IsViewLoaded", false);
			InvokePrivateMethod(accordion, "OnAccordionLoaded", [this, EventArgs.Empty]);
			var isViewLoaded = GetNonPublicProperty(accordion, "IsViewLoaded");
			Assert.True((bool?)isViewLoaded);
		}

		[Fact]
		public void OnAccordionItemsCollectionChanged_HandleAddAction_WhenNewItemIsAdded()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem();
			accordion.Items.Add(accordionItem);
			var newItems = new List<object> { accordionItem };
			var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, 0);
			InvokePrivateMethod(accordion, "OnAccordionItemsCollectionChanged", [this, e]);
			Assert.Contains(accordionItem, accordion.Items);
		}

		[Fact]
		public void OnAccordionItemsCollectionChanged_ShouldNotProcessWhenViewNotLoaded()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem();
			SetNonPublicProperty(accordion, "IsViewLoaded", false);
			var newItems = new List<object> { accordionItem };
			var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, 0);
			InvokePrivateMethod(accordion, "OnAccordionItemsCollectionChanged", [this, e]);
			Assert.Empty(accordion.Items);
		}

		[Fact]
		public void RaiseCollapsingEvent_ShouldInvokeAccordionEventHandler()
		{
			var accordion = new SfAccordion();
			var accordionItemView = new AccordionItemView
			{
				Accordion = accordion
			};

			bool eventRaised = false;
			var items = new ObservableCollection<AccordionItem>
			{
				new() { _accordionItemView = accordionItemView, _itemIndex = 0 },
				new() { _accordionItemView = accordionItemView, _itemIndex = 1 },
				new() { _accordionItemView = accordionItemView, _itemIndex = 2 },
			};

			accordion.Items = items;
			accordion.Collapsing += (sender, args) =>
			{
				eventRaised = true;
				Assert.Equal(0, args.Index);
			};

			accordionItemView.GetType().GetProperty("Accordion")?.SetValue(accordionItemView, accordion);
			typeof(AccordionItemView).GetProperty("AccordionItem")?.SetValue(accordionItemView, items[0]);
			accordionItemView.RaiseCollapsingEvent();
			Assert.True(eventRaised, "The collapsing event should have been raised.");
		}

		[Fact]
		public void RaiseCollapsedEvent_WhenAccordionIsNull_DoesNotThrowException()
		{
			var accordionItemView = new AccordionItemView();
			var exception = Record.Exception(() => accordionItemView.RaiseCollapsedEvent());
			Assert.Null(exception);
		}

		[Fact]
		public void RaiseCollapsedEvent_CallsRaiseCollapsedEventOnAccordion_WhenIndexIsValid()
		{
			var accordionItemView = new AccordionItemView();
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem
			{
				_accordionItemView = accordionItemView,
				_itemIndex = 1
			};

			accordion.Items.Add(accordionItem);
			accordionItemView.Accordion = accordion;
			MethodInfo? raiseCollapsedEventMethod = typeof(AccordionItemView)
				.GetMethod("RaiseCollapsedEvent", BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(raiseCollapsedEventMethod);
			MethodInfo? getCurrentAccordionItemIndexMethod = typeof(AccordionItemView)
				.GetMethod("GetCurrentAccordionItemIndex", BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(getCurrentAccordionItemIndexMethod);
			raiseCollapsedEventMethod.Invoke(accordionItemView, null);
			var index = (int?)getCurrentAccordionItemIndexMethod.Invoke(accordionItemView, null);
			Assert.Equal(1, index);
		}

		[Fact]
		public void OnBindingContextChanged_SetsHeaderBindingContext_WhenHeaderIsNotNull()
		{
			var accordionItem = new AccordionItem();
			var bindingContext = new object();
			var header = new ExpanderHeader();
			accordionItem.Header = header;
			accordionItem.BindingContext = bindingContext;
			InvokePrivateMethod(accordionItem, "OnBindingContextChanged");
			Assert.Equal(bindingContext, header.BindingContext);
		}

		[Fact]
		public void OnBindingContextChanged_SetsContentBindingContext_WhenHeaderIsNotNull()
		{
			var accordionItem = new AccordionItem();
			var bindingContext = new object();
			var expanderContent = new ExpanderContent();
			accordionItem.Content = expanderContent;
			accordionItem.BindingContext = bindingContext;
			InvokePrivateMethod(accordionItem, "OnBindingContextChanged");
			Assert.Equal(bindingContext, expanderContent.BindingContext);
		}

		[Fact]
		public void OnHeaderPropertyChanged_SetsAccordionItemViewHeader_WhenHeaderIsNotNull()
		{
			var accordionItem = new AccordionItem();
			var accordion = new SfAccordion();
			accordionItem._accordion = accordion;
			accordionItem._accordion.IsViewLoaded = true;
			var contentView = new ContentView();
			var accordionItemView = new AccordionItemView();
			accordionItem._accordionItemView = accordionItemView;
			InvokePrivateMethod(accordionItem, "OnHeaderPropertyChanged", accordionItem, null, contentView);
			Assert.Equal(contentView, accordionItemView.Header);
		}

		[Fact]
		public void OnHeaderPropertyChanged_DoesNotSetHeader_WhenAccordionItemViewIsNull()
		{
			var accordionItem = new AccordionItem();
			var contentView = new ContentView();
			accordionItem._accordionItemView = null;
			InvokePrivateMethod(accordionItem, "OnHeaderPropertyChanged", accordionItem, null, contentView);
			Assert.Null(accordionItem._accordionItemView);
		}

		[Fact]
		public void OnContentPropertyChanged_SetsContentVisibilityAndViewContent_WhenContentIsNotNull()
		{
			var accordionItem = new AccordionItem
			{
				IsExpanded = true
			};
			var accordion = new SfAccordion();
			accordionItem._accordion = accordion;
			accordionItem._accordion.IsViewLoaded = true;
			var contentView = new ContentView();
			var accordionItemView = new AccordionItemView();
			accordionItem._accordionItemView = accordionItemView;
			InvokePrivateMethod(accordionItem, "OnContentPropertyChanged", accordionItem, null, contentView);
			Assert.Equal(contentView, accordionItemView.Content);
		}

		[Theory]
		[InlineData(null)]
		[InlineData(typeof(ContentView))]
		public void OnContentPropertyChanged_SetsContentVisibilityAndViewContent_WhenContentIsNullOrNotNull(object? content)
		{
			var accordionItem = new AccordionItem
			{
				IsExpanded = true
			};

			var accordion = new SfAccordion();
			accordionItem._accordion = accordion;
			accordionItem._accordion.IsViewLoaded = true;
			// Create a new AccordionItemView and set it to _accordionItemView
			var accordionItemView = new AccordionItemView();
			accordionItem._accordionItemView = accordionItemView;

			// If content is null, pass null, otherwise, create an instance of ContentView
			ContentView? contentView = content is null ? null : new ContentView();

			// Invoke the private method for setting content
			InvokePrivateMethod(accordionItem, "OnContentPropertyChanged", accordionItem, null, contentView);

			// Assert if the content is properly set, including when it's null
			if (contentView != null)
			{
				Assert.Equal(contentView, accordionItemView.Content);
			}
			else
			{
				Assert.Null(accordionItemView.Content);
			}
		}


		[Theory]
		[InlineData("#FF0000")]
		[InlineData("#00FF00")]
		[InlineData("#00000000")]
		public void OnHeaderBackgroundPropertyChanged_UpdatesHeaderBackground(string brushColor)
		{
			var accordionItem = new AccordionItem();
			var accordion = new SfAccordion();
			accordionItem._accordion = accordion;
			accordionItem._accordion.IsViewLoaded = true;
			var accordionItemView = new AccordionItemView();
			accordionItem._accordionItemView = accordionItemView;
			var newBrush = new SolidColorBrush(Color.FromArgb(brushColor));
			InvokePrivateMethod(accordionItem, "OnHeaderBackgroundPropertyChanged", accordionItem, null, newBrush);
			Assert.Equal(newBrush, accordionItemView.HeaderBackground);
		}		
		
		[Theory]
		[InlineData("#FF0000")]
		[InlineData("#00FF00")]
		[InlineData("#0000FF")]
		public void OnHeaderIconColorPropertyChanged_UpdatesHeaderIconColor(string colorHex)
		{
			var accordionItem = new AccordionItem();
			var accordion = new SfAccordion();
			accordionItem._accordion = accordion;
			accordionItem._accordion.IsViewLoaded = true;
			var accordionItemView = new AccordionItemView();
			accordionItem._accordionItemView = accordionItemView;
			var newColor = Color.FromArgb(colorHex);
			InvokePrivateMethod(
				accordionItem,
				"OnHeaderIconColorPropertyChanged",
				accordionItem,
				null,
				newColor);
			Assert.Equal(newColor, accordionItemView.HeaderIconColor);
		}

		[Theory]
		[InlineData(false, true, false)]
		[InlineData(true, false, false)]
		[InlineData(true, true, true)]
		public void AccordionItemView_IsEnabled_ShouldUpdateCorrectly(bool accordionIsEnabled, bool itemIsEnabled, bool expectedIsEnabled)
		{
			var accordion = new SfAccordion { IsEnabled = accordionIsEnabled };
			var accordionItemView = new AccordionItemView { IsEnabled = true };
			var accordionItem = new AccordionItem
			{
				_accordionItemView = accordionItemView,
				IsEnabled = itemIsEnabled
			};

			if (!accordion.IsEnabled)
			{
				accordionItem._accordionItemView.IsEnabled = accordion.IsEnabled;
			}
			else if (!accordionItem.IsEnabled)
			{
				accordionItem._accordionItemView.IsEnabled = accordionItem.IsEnabled;
			}

			Assert.Equal(expectedIsEnabled, accordionItemView.IsEnabled);
		}

		[Fact]
		public void OnPropertyChanged_UpdatesAccordionItemViewsIsEnabled_WhenIsEnabledPropertyChanges()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem
			{
				IsEnabled = false,
				_accordionItemView = []
			};

			accordion.Items.Add(accordionItem);
			Assert.True(accordionItem._accordionItemView.IsEnabled);
			accordion.IsEnabled = true;
			Assert.True(accordionItem._accordionItemView.IsEnabled);
			accordion.IsEnabled = false;
			Assert.False(accordionItem._accordionItemView.IsEnabled);
			accordionItem.IsEnabled = true;
			accordion.IsEnabled = true;
			Assert.True(accordionItem._accordionItemView.IsEnabled);
		}

		[Fact]
		public void CreateLayoutManager_ReturnsAccordionLayoutManager_UsingReflection()
		{
			var accordion = new SfAccordion();
			var createLayoutManagerMethod = typeof(SfAccordion).GetMethod("CreateLayoutManager", BindingFlags.NonPublic | BindingFlags.Instance);
			var layoutManager = createLayoutManagerMethod?.Invoke(accordion, null);
			Assert.NotNull(layoutManager);
			Assert.IsType<AccordionLayoutManager>(layoutManager);
		}

		[Fact]
		public void OnHeaderIconPositionPropertyChanged_UpdatesIconPosition()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem();
			accordion.Items.Add(accordionItem);
			accordionItem._accordionItemView = [];
			accordion.IsViewLoaded = true;
			accordion.HeaderIconPosition = ExpanderIconPosition.Start;
			var oldValue = ExpanderIconPosition.End;
			var newValue = ExpanderIconPosition.Start;
			MethodInfo? methodInfo = typeof(SfAccordion).GetMethod(
				"OnHeaderIconPositionPropertyChanged",
				BindingFlags.NonPublic | BindingFlags.Static);
			if (methodInfo == null)
			{
				Assert.Fail("Method not found.");
			}

			methodInfo.Invoke(null, [accordion, oldValue, newValue]);
			Assert.Equal(newValue, accordionItem._accordionItemView.HeaderIconPosition);
		}

		[Theory]
		[InlineData(ExpanderAnimationEasing.Linear)]
		[InlineData(ExpanderAnimationEasing.SinOut)]
		[InlineData(ExpanderAnimationEasing.SinInOut)] 
		[InlineData(ExpanderAnimationEasing.SinIn)]
		public void OnAnimationEasingPropertyChanged_UpdatesAnimationEasing(ExpanderAnimationEasing newValue)
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem();
			accordion.Items.Add(accordionItem);
			accordionItem._accordionItemView = [];
			accordion.IsViewLoaded = true;
			accordion.AnimationEasing = newValue;
			MethodInfo? methodInfo = typeof(SfAccordion).GetMethod(
				"OnAnimationEasingPropertyChanged",
				BindingFlags.NonPublic | BindingFlags.Static);
			if (methodInfo == null)
			{
				Assert.Fail("Method not found.");
			}

			methodInfo.Invoke(null, [accordion, null, newValue]);
			Assert.Equal(newValue, accordionItem._accordionItemView.AnimationEasing);
		}

		[Theory]
		[InlineData(200)]
		[InlineData(500)]
		[InlineData(1000)]
		public void OnAnimationDurationPropertyChanged_UpdatesAnimationDuration(int newValue)
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem();
			accordion.Items.Add(accordionItem);
			accordionItem._accordionItemView = [];
			accordion.IsViewLoaded = true;
			accordion.AnimationDuration = newValue;
			MethodInfo? methodInfo = typeof(SfAccordion).GetMethod(
				"OnAnimationDurationPropertyChanged",
				BindingFlags.NonPublic | BindingFlags.Static);
			methodInfo?.Invoke(null, [accordion, null, newValue]);
			Assert.Equal(newValue, accordionItem._accordionItemView.AnimationDuration);
		}

		[Fact]
		public void OnAccordionChildRemoved_RemovesItemFromItems()
		{
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem();
			accordion.Items.Add(accordionItem);
			Assert.Contains(accordionItem, accordion.Items);
			var eventArgs = new ElementEventArgs(accordionItem);
			MethodInfo? methodInfo = typeof(SfAccordion).GetMethod(
				"OnAccordionChildRemoved",
				BindingFlags.NonPublic | BindingFlags.Instance);
			if (methodInfo == null)
			{
				Assert.Fail("OnAccordionChildRemoved method not found.");
			}

			methodInfo.Invoke(accordion, [accordion, eventArgs]);
			Assert.DoesNotContain(accordionItem, accordion.Items);
		}

		[Fact]
		public void RaiseExpandedEvent_AccordionIsNull_NoExceptionThrown()
		{
			var accordionItemView = new AccordionItemView() { Accordion = null };
			var method = typeof(AccordionItemView)
				.GetMethod("RaiseExpandedEvent", BindingFlags.Instance | BindingFlags.NonPublic);
			Assert.NotNull(method);
			method.Invoke(accordionItemView, null);
		}

		[Fact]
		public void RaiseExpandedEvent_CallsRaiseExpandedEventOnAccordion_WhenIndexIsValid()
		{
			var accordionItemView = new AccordionItemView();
			var accordion = new SfAccordion();
			var accordionItem = new AccordionItem
			{
				_accordionItemView = accordionItemView,
				_itemIndex = 1
			};

			accordion.Items.Add(accordionItem);
			accordionItemView.Accordion = accordion;
			MethodInfo? raiseExpandedEventMethod = typeof(AccordionItemView)
				.GetMethod("RaiseExpandedEvent", BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(raiseExpandedEventMethod);
			MethodInfo? getCurrentAccordionItemIndexMethod = typeof(AccordionItemView)
				.GetMethod("GetCurrentAccordionItemIndex", BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(getCurrentAccordionItemIndexMethod);
			raiseExpandedEventMethod.Invoke(accordionItemView, null);
			var index = (int?)getCurrentAccordionItemIndexMethod.Invoke(accordionItemView, null);
			Assert.Equal(1, index);
		}

		[Fact]
		public void UpdateSelection_DeselectsItem_WhenOnlyOneItemIsSelected()
		{
			var accordion = new SfAccordion();
			var accordionItemView1 = new AccordionItemView();
			var accordionItemView2 = new AccordionItemView();
			var accordionItem1 = new AccordionItem { _accordionItemView = accordionItemView1, _itemIndex = 0 };
			var accordionItem2 = new AccordionItem { _accordionItemView = accordionItemView2, _itemIndex = 1 };
			accordion.Items.Add(accordionItem1);
			accordion.Items.Add(accordionItem2);
			accordion.UpdateSelection();
			Assert.False(accordionItemView1.IsSelected);
		}

		[Fact]
		public void UpdateSelection_DeselectsSelectedItem_WhenExactlyOneItemIsSelected()
		{
			var accordionItem1 = new AccordionItem
			{
				_itemIndex = 0,
				_accordionItemView = []
			};

			var accordionItem2 = new AccordionItem
			{
				_itemIndex = 1,
				_accordionItemView = []
			};

			var accordion = new SfAccordion();
			accordion.Items.Add(accordionItem1);
			accordion.Items.Add(accordionItem2);
			accordion.UpdateSelection();
			Assert.False(accordionItem1._accordionItemView.IsSelected, "The first item should be deselected.");
			Assert.False(accordionItem2._accordionItemView.IsSelected, "The second item should remain unselected.");
		}

		[Theory]
		[InlineData(AccordionExpandMode.SingleOrNone, true)]
		[InlineData(AccordionExpandMode.SingleOrNone, false)]
		public void UpdateAccordionItemsBasedOnExpandModes_WhenSingleorNone(AccordionExpandMode mode, bool isExpandModeChanged)
		{
			var accordion = new SfAccordion
			{
				ExpandMode = mode
			};

			var accordionItem1 = new AccordionItem { _itemIndex = 0, _accordionItemView = new AccordionItemView { IsExpanded = false } };
			var accordionItem2 = new AccordionItem { _itemIndex = 1, _accordionItemView = new AccordionItemView { IsExpanded = false } };
			accordion.Items.Add(accordionItem1);
			accordion.Items.Add(accordionItem2);
			if (mode == AccordionExpandMode.Single || mode == AccordionExpandMode.SingleOrNone)
			{
				accordionItem1._accordionItemView.IsExpanded = true;
			}

			accordion.UpdateAccordionItemsBasedOnExpandModes(isExpandModeChanged);
			if (mode == AccordionExpandMode.Single)
			{
				Assert.True(accordionItem1._accordionItemView.IsExpanded);
				Assert.False(accordionItem2._accordionItemView.IsExpanded);
				if (isExpandModeChanged)
				{
					Assert.False(accordionItem1._accordionItemView.IsExpanded);
				}
			}
			else if (mode == AccordionExpandMode.SingleOrNone)
			{
				Assert.True(accordionItem1._accordionItemView.IsExpanded);
				Assert.False(accordionItem2._accordionItemView.IsExpanded);
			}
			else if (mode == AccordionExpandMode.Multiple)
			{
				Assert.True(accordionItem1._accordionItemView.IsExpanded);
				Assert.True(accordionItem2._accordionItemView.IsExpanded);
			}
		}

		[Theory]
		[InlineData(AccordionExpandMode.Multiple, true)]
		[InlineData(AccordionExpandMode.Multiple, false)]
		public void UpdateAccordionItemsBasedOnExpandModes_WhenMultiple(AccordionExpandMode mode, bool isExpandModeChanged)
		{
			var accordion = new SfAccordion
			{
				ExpandMode = mode
			};

			var accordionItem1 = new AccordionItem { _itemIndex = 0, _accordionItemView = new AccordionItemView { IsExpanded = true } };
			var accordionItem2 = new AccordionItem { _itemIndex = 1, _accordionItemView = new AccordionItemView { IsExpanded = true } };
			accordion.Items.Add(accordionItem1);
			accordion.Items.Add(accordionItem2);
			accordion.UpdateAccordionItemsBasedOnExpandModes(isExpandModeChanged);
			if (mode == AccordionExpandMode.Multiple)
			{
				Assert.True(accordionItem1._accordionItemView.IsExpanded);
				Assert.True(accordionItem2._accordionItemView.IsExpanded);
			}
			else if (mode == AccordionExpandMode.Single && isExpandModeChanged)
			{
				Assert.False(accordionItem1._accordionItemView.IsExpanded);
				Assert.False(accordionItem2._accordionItemView.IsExpanded);
			}
		}

		[Fact]
		public void UpdateAccordionItemsBasedOnExpandModes_MultipleMode_AllowsMultipleExpansion()
		{
			var accordion = new SfAccordion
			{
				ExpandMode = AccordionExpandMode.Multiple
			};

			var accordionItem1 = new AccordionItem { _itemIndex = 0, _accordionItemView = new AccordionItemView { IsExpanded = true } };
			var accordionItem2 = new AccordionItem { _itemIndex = 1, _accordionItemView = new AccordionItemView { IsExpanded = true } };
			accordion.Items.Add(accordionItem1);
			accordion.Items.Add(accordionItem2);
			accordion.UpdateAccordionItemsBasedOnExpandModes(false);
			Assert.True(accordionItem1._accordionItemView.IsExpanded);
			Assert.True(accordionItem2._accordionItemView.IsExpanded);
		}

		[Fact]
		public void UpdateSelection_ShouldDeselectItem_WhenSingleItemIsSelected()
		{
			var accordion = new SfAccordion();
			accordion.Items.Add(new AccordionItem { _accordionItemView = [] });
			InvokePrivateMethod(accordion, "UpdateSelection");
			Assert.False(accordion.Items[0]._accordionItemView?.IsSelected);
		}
		
		#endregion

	}
}
