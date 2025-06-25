using Syncfusion.Maui.Toolkit.Buttons;
using Syncfusion.Maui.Toolkit.Chips;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using System.Reflection;

namespace Syncfusion.Maui.Toolkit.UnitTest.Buttons
{
	public class SfButtonUnitTests : BaseUnitTest
	{
		#region Fields

		private readonly BrushTypeConverter _converter = new();

		#endregion

		#region Constructor

		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var button = new SfButton();
			Assert.Equal(Colors.White, button.TextColor);
			Assert.Equal(TextAlignment.Center, button.VerticalTextAlignment);
			if (button.Background is SolidColorBrush brush)
			{
				Assert.Equal(Color.FromArgb("#6750A4"), brush.Color);
			}
			Console.WriteLine(button.Background?.ToString());
			Assert.Equal(20, button.CornerRadius);
			Assert.Equal(FontAttributes.None, button.FontAttributes);
			Assert.Equal(14d, button.FontSize);
			Assert.Equal(TextAlignment.Center, button.HorizontalTextAlignment);
			Assert.Equal(Alignment.Start, button.ImageAlignment);
			Assert.Equal(18, button.ImageSize);
			Assert.Equal(0d, button.StrokeThickness);
			Assert.Equal(0, button.Padding);
			Assert.Equal(TextTransform.Default, button.TextTransform);
			Assert.Equal(LineBreakMode.NoWrap, button.LineBreakMode);
			Assert.Equal([0, 0], button.DashArray);
			Assert.True(button.EnableRippleEffect);
			Assert.False(button.FontAutoScalingEnabled);
			Assert.False(button.ShowIcon);
			Assert.False(button.IsCheckable);
			Assert.False(button.IsChecked);
			Assert.Null(button.BackgroundImageSource);
			Assert.Null(button.Command);
			Assert.Null(button.CommandParameter);
			Assert.Null(button.FontFamily);
			Assert.Null(button.ImageSource);
			Assert.Null(button.Content);
			Assert.Empty(button.Text);
		}
		#endregion

		#region Properties

		[Theory]
		[InlineData(0, 0, 0, 0)]
		[InlineData(5, 5, 5, 5)]
		[InlineData(10, 15, 20, 25)]
		public void CornerRadius_ShouldSetAndGetCorrectly(double topLeft, double topRight, double bottomLeft, double bottomRight)
		{
			var button = new SfButton();
			var expectedCornerRadius = new CornerRadius(topLeft, topRight, bottomLeft, bottomRight);
			button.CornerRadius = expectedCornerRadius;
			var actualCornerRadius = button.CornerRadius;
			Assert.Equal(expectedCornerRadius, actualCornerRadius);
		}

		[Theory]
		[InlineData(0.0)]
		[InlineData(1.5)]
		[InlineData(5)]
		[InlineData(10.0)]
		public void StrokeThickness_ShouldSetAndGetCorrectly(double expectedThickness)
		{
			var button = new SfButton();
			button.StrokeThickness = expectedThickness;
			var actualThickness = button.StrokeThickness;
			Assert.Equal(expectedThickness, actualThickness);
		}

		[Theory]
		[InlineData("#FF0000")]
		[InlineData("#00FF00")]
		[InlineData("#0000FF")]
		public void Stroke_ShouldSetAndGetCorrectly(string colorHex)
		{
			var button = new SfButton();
			var expectedBrush = GetSolidColorBrush(colorHex);
			button.Stroke = expectedBrush;
			var actualStroke = (Color?)GetNonPublicProperty(button, "BaseStrokeColor");
			Assert.Equal(expectedBrush.Color, actualStroke);
		}

		[Theory]
		[InlineData("#FFFFFF")]
		[InlineData("#FF5733")]
		[InlineData("#C70039")]
		public void Background_ShouldSetAndGetCorrectly(string colorHex)
		{
			var button = new SfButton();
			var expectedBrush = GetSolidColorBrush(colorHex);
			button.Background = expectedBrush;
			var actualBrush = button.Background;
			Assert.Equal(expectedBrush.Color, actualBrush);
		}

		[Theory]
		[InlineData("sample")]
		[InlineData("sAmPlE")]
		[InlineData("SAMPLE")]
		public void Text_ShouldSetAndGetCorrectly(string expectedText)
		{
			var button = new SfButton();
			button.Text = expectedText;
			var actualText = button.Text;
			var isTextChanged = (bool?)GetPrivateMember(button, "_isSemanticTextChanged");
			Assert.Equal(expectedText, actualText);
			Assert.True(isTextChanged);
		}

		[Theory]
		[InlineData(255, 0, 0)]
		[InlineData(0, 255, 0)]
		[InlineData(0, 0, 255)]
		public void TextColor_ShouldSetAndGetCorrectly(byte r, byte g, byte b)
		{
			var button = new SfButton();
			var expectedColor = Color.FromRgb(r, g, b);
			button.TextColor = expectedColor;
			var actualColor = button.TextColor;
			Assert.Equal(expectedColor, actualColor);
		}

		[Theory]
		[InlineData(12.0)]
		[InlineData(16.5)]
		[InlineData(24.0)]
		public void FontSize_ShouldSetAndGetCorrectly(double expectedFontSize)
		{
			var button = new SfButton();
			button.FontSize = expectedFontSize;
			var actualFontSize = button.FontSize;
			Assert.Equal(expectedFontSize, actualFontSize);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void FontAutoScalingEnabled_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var button = new SfButton();
			button.FontAutoScalingEnabled = expectedValue;
			var actualValue = button.FontAutoScalingEnabled;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Center)]
		[InlineData(TextAlignment.End)]
		public void HorizontalTextAlignment_ShouldSetAndGetCorrectly(TextAlignment expectedValue)
		{
			var button = new SfButton();
			button.HorizontalTextAlignment = expectedValue;
			var actualValue = button.HorizontalTextAlignment;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(TextAlignment.Start)]
		[InlineData(TextAlignment.Center)]
		[InlineData(TextAlignment.End)]
		public void VerticalTextAlignment_ShouldSetAndGetCorrectly(TextAlignment expectedValue)
		{
			var button = new SfButton();
			button.VerticalTextAlignment = expectedValue;
			var actualValue = button.VerticalTextAlignment;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("SampleImage1.png")]
		[InlineData("SampleImage2.png")]
		public void ImageSource_ShouldSetAndGetCorrectly(string imagePath)
		{
			var button = new SfButton();
			var expectedImageSource = ImageSource.FromFile(imagePath);
			button.ShowIcon = true;
			button.ImageSource = expectedImageSource;
			var actualImageSource = button.ImageSource;
			var isImageUpdated = (bool?)GetPrivateMember(button, "_isImageIconUpdated");
			Assert.Equal(expectedImageSource, actualImageSource);
			Assert.True(isImageUpdated);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void ShowIcon_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var button = new SfButton();
			button.ShowIcon = expectedValue;
			button.ImageSource = "SampleImage1.png";
			var actualValue = button.ShowIcon;
			var isImageUpdated = (bool?)GetPrivateMember(button, "_isImageIconUpdated");
			Assert.Equal(expectedValue, actualValue);
			Assert.Equal(expectedValue, isImageUpdated);
		}

		[Theory]
		[InlineData(10.0, 24)]
		[InlineData(20.5, 34.5)]
		[InlineData(0.0, 14)]
		public void ImageSize_ShouldSetAndGetCorrectly(double expectedValue, double expectedHeight)
		{
			var button = new SfButton();
			button.IsCreatedInternally = true;
			button.ShowIcon = true;
			button.ImageSource = "SampleImage1.png";
			button.ImageSize = expectedValue;
			var actualValue = button.ImageSize;
			var actualHeight = button.HeightRequest;
			Assert.Equal(expectedValue, actualValue);
			Assert.Equal(expectedHeight, actualHeight);
		}

		[Theory]
		[InlineData(Alignment.Top)]
		[InlineData(Alignment.Bottom)]
		[InlineData(Alignment.Left)]
		[InlineData(Alignment.Right)]
		public void ImageAlignment_ShouldSetAndGetCorrectly(Alignment expectedValue)
		{
			var button = new SfButton();
			button.ImageAlignment = expectedValue;
			var actualValue = button.ImageAlignment;
			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData("SampleImage1.png")]
		[InlineData("SampleImage2.png")]
		public void BackgroundImageSource_ShouldSetAndGetCorrectly(string imagePath)
		{
			var button = new SfButton();
			var expectedValue = ImageSource.FromFile(imagePath);
			button.BackgroundImageSource = expectedValue;
			var actualValue = button.BackgroundImageSource;
			var backgroundImageView = (Image?)GetPrivateMember(button, "_backgroundImageView");
			Assert.Equal(expectedValue, actualValue);
			Assert.Equal(expectedValue, backgroundImageView?.Source);
		}

		[Theory]
		[InlineData("TestParameter1")]
		[InlineData(123)]
		public void CommandParameter_ShouldSetAndGetCorrectly(object expectedParameter)
		{
			var button = new SfButton();
			button.CommandParameter = expectedParameter;
			var actualParameter = button.CommandParameter;
			Assert.Equal(expectedParameter, actualParameter);
		}

		[Theory]
		[InlineData(10, 10, 10, 10)]
		[InlineData(5, 10, 5, 10)]
		[InlineData(0, 0, 0, 0)]
		public void Padding_ShouldSetAndGetCorrectly(double left, double top, double right, double bottom)
		{
			var button = new SfButton();
			var expectedPadding = new Thickness(left, top, right, bottom);
			button.Padding = expectedPadding;
			var actualPadding = button.Padding;
			Assert.Equal(expectedPadding, actualPadding);
		}

		[Theory]
		[InlineData("Arial")]
		[InlineData("Times New Roman")]
		[InlineData("Courier New")]
		[InlineData("")]
		public void FontFamily_ShouldSetAndGetCorrectly(string expectedFontFamily)
		{
			var button = new SfButton();
			button.FontFamily = expectedFontFamily;
			var actualFontFamily = button.FontFamily;
			Assert.Equal(expectedFontFamily, actualFontFamily);
		}

		[Theory]
		[InlineData(FontAttributes.None)]
		[InlineData(FontAttributes.Bold)]
		[InlineData(FontAttributes.Italic)]
		public void FontAttributes_ShouldSetAndGetCorrectly(FontAttributes expectedAttributes)
		{
			var button = new SfButton();
			button.FontAttributes = expectedAttributes;
			var actualAttributes = button.FontAttributes;
			Assert.Equal(expectedAttributes, actualAttributes);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void EnableRippleEffect_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var button = new SfButton();
			button.EnableRippleEffect = expectedValue;
			var actualValue = button.EnableRippleEffect;
			Assert.Equal(expectedValue, actualValue);
		}

		[Fact]
		public void Content_ShouldSetAndGetCorrectly()
		{
			var button = new SfButton();
			var expectedContent = new DataTemplate(() => { return new Label { Text = "Custom Content" }; });
			button.Content = expectedContent;
			Assert.Equal(expectedContent, button.Content);
			expectedContent =  new DataTemplate(() => { return new Label { Text = "12345" }; });
			button.Content = expectedContent;
			Assert.Equal(expectedContent, button.Content);
			expectedContent =  new DataTemplate(() => { return new Label { Text = "@Button" }; });
			Assert.NotEqual(expectedContent, button.Content);
		}

		[Theory]
		[InlineData(TextTransform.Uppercase, "SYNCFUSION")]
		[InlineData(TextTransform.Lowercase, "syncfusion")]
		[InlineData(TextTransform.Default, "syncFusion")]
		public void TextTransform_ShouldSetAndGetCorrectly(TextTransform textTransform, string expected)
		{
			var button = new SfButton();
			button.Text = "syncFusion";
			button.TextTransform = textTransform;
			var result = InvokePrivateMethod(button, "ApplyTextTransform", button.Text);
			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData(LineBreakMode.NoWrap)]
		[InlineData(LineBreakMode.WordWrap)]
		[InlineData(LineBreakMode.CharacterWrap)]
		[InlineData(LineBreakMode.HeadTruncation)]
		[InlineData(LineBreakMode.TailTruncation)]
		[InlineData(LineBreakMode.MiddleTruncation)]
		public void LineBreakModeProperty_ShouldSetAndGetCorrectly(LineBreakMode lineBreakMode)
		{
			var button = new SfButton();

			button.LineBreakMode = lineBreakMode;

			Assert.Equal(lineBreakMode, button.LineBreakMode);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsCheckable_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var button = new SfButton();

			button.IsCheckable = expectedValue;

			Assert.Equal(expectedValue, button.IsCheckable);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsChecked_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var button = new SfButton();

			button.IsChecked = expectedValue;

			Assert.Equal(expectedValue, button.IsChecked);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void IsEnabled_ShouldSetAndGetCorrectly(bool expectedValue)
		{
			var button = new SfButton();

			button.IsEnabled = expectedValue;

			Assert.Equal(expectedValue, button.IsEnabled);
		}



		[Theory]
		[InlineData(Aspect.AspectFit)]
		[InlineData(Aspect.AspectFill)]
		[InlineData(Aspect.Fill)]
		public void BackgroundImageAspect_ShouldSetAndGetCorrectly(Aspect expectedAspect)
		{
			var button = new SfButton();

			button.BackgroundImageAspect = expectedAspect;
			var backgroundImageView = (Image?)GetPrivateMember(button, "_backgroundImageView");
			button.BackgroundImageSource = "SampleImage1.png";

			Assert.Equal(expectedAspect, button.BackgroundImageAspect);
			Assert.Equal(expectedAspect, backgroundImageView?.Aspect);

		}
		#endregion

		#region Private Method

		protected object? GetPrivateMember<T>(T obj, string memberName)
		{
			var type = obj?.GetType();
			while (type != null)
			{
				var field = type.GetField(memberName, BindingFlags.NonPublic | BindingFlags.Instance);
				if (field != null)
				{
					return field.GetValue(obj);
				}

				var property = type.GetProperty(memberName, BindingFlags.NonPublic | BindingFlags.Instance);
				if (property != null)
				{
					return property.GetValue(obj);
				}

				type = type.BaseType;
			}

			throw new InvalidOperationException($"Field or property '{memberName}' not found.");
		}

		private SolidColorBrush GetSolidColorBrush(string colorString)
		{
			var brush = _converter.ConvertFromString(colorString) as SolidColorBrush;
			if (brush == null)
			{
				throw new InvalidOperationException($"Failed to convert color: {colorString}");
			}
			return brush;
		}

		[Theory]
		[InlineData(30, -47, -15.5, Alignment.Right, FlowDirection.LeftToRight)]
		[InlineData(40, 16, -20.5, Alignment.Right, FlowDirection.RightToLeft)]
		[InlineData(50, -67, -25.5, Alignment.End, FlowDirection.LeftToRight)]
		[InlineData(60, -77, -30.5, Alignment.End, FlowDirection.RightToLeft)]
		public void ComputeIconRectForRightOrEndAlignment_ShouldReturn_IconRectF(double imageSize, float expectedX, float expectedY, Alignment imageAlignment, FlowDirection flowDirection)
		{
			var button = new SfButton();
			button.ImageSize = imageSize;
			button.ShowIcon = true;
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";
			button.ImageAlignment = imageAlignment;
			button.FlowDirection = flowDirection;

			InvokePrivateMethod(button, "ComputeIconRectForRightAlignment");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_iconRectF");

			Assert.Equal(expectedX, actualRectF!.Value.X);
			Assert.Equal(expectedY, actualRectF!.Value.Y);
		}

		[Theory]
		[InlineData(30, 16, -15.5, Alignment.Left, FlowDirection.LeftToRight)]
		[InlineData(40, -57, -20.5, Alignment.Left, FlowDirection.RightToLeft)]
		[InlineData(50, 16, -25.5, Alignment.Start, FlowDirection.LeftToRight)]
		[InlineData(60, 16, -30.5, Alignment.Start, FlowDirection.RightToLeft)]
		public void ComputeIconRectForLefOrStartAlignment_ShouldReturn_IconRectF(double imageSize, float expectedX, float expectedY, Alignment imageAlignment, FlowDirection flowDirection)
		{
			var button = new SfButton();
			button.ImageSize = imageSize;
			button.ShowIcon = true;
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";
			button.ImageAlignment = imageAlignment;
			button.FlowDirection = flowDirection;

			InvokePrivateMethod(button, "ComputeIconRectForLeftAlignment");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_iconRectF");

			Assert.Equal(expectedX, actualRectF!.Value.X);
			Assert.Equal(expectedY, actualRectF!.Value.Y);
		}

		[Theory]
		[InlineData(30, -15.5, 12)]
		[InlineData(40, -20.5, 12)]
		[InlineData(50, -25.5, 12)]
		[InlineData(60, -30.5, 12)]
		public void ComputeIconRectForTopAlignment_ShouldReturn_IconRectF(double imageSize, float expectedX, float expectedY)
		{
			var button = new SfButton();
			button.ImageSize = imageSize;
			button.ShowIcon = true;
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";

			InvokePrivateMethod(button, "ComputeIconRectForTopAlignment");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_iconRectF");

			Assert.Equal(expectedX, actualRectF!.Value.X);
			Assert.Equal(expectedY, actualRectF!.Value.Y);
		}

		[Theory]
		[InlineData(30, -15.5, -39)]
		[InlineData(40, -20.5, -49)]
		[InlineData(50, -25.5, -59)]
		[InlineData(60, -30.5, -69)]
		public void ComputeIconRectForBottomAlignment_ShouldReturn_IconRectF(double imageSize, float expectedX, float expectedY)
		{
			var button = new SfButton();
			button.ImageSize = imageSize;
			button.ShowIcon = true;
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";

			InvokePrivateMethod(button, "ComputeIconRectForBottomAlignment");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_iconRectF");

			Assert.Equal(expectedX, actualRectF!.Value.X);
			Assert.Equal(expectedY, actualRectF!.Value.Y);
		}

		[Theory]
		[InlineData(30, 50, 4, 55, 9, Alignment.Left, FlowDirection.LeftToRight)]
		[InlineData(40, 60, 4, 65, 9, Alignment.Left, FlowDirection.RightToLeft)]
		[InlineData(50, 70, 4, 75, 9, Alignment.Start, FlowDirection.LeftToRight)]
		[InlineData(60, 4, 4, 85, 9, Alignment.Start, FlowDirection.RightToLeft)]
		public void ComputeTextRectForLeftOrStartAlignment_ShouldReturn_TextRectF(double imageSize, float expectedX, float expectedY, float expectedWidth, float expectedHeight, Alignment alignment, FlowDirection flowDirection)
		{
			var expectedRectF = new RectF(expectedX, expectedY, expectedWidth, expectedHeight);

			var button = new SfButton();
			button.ImageSize = imageSize;
			button.ShowIcon = true;
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";
			button.ImageAlignment = alignment;
			button.FlowDirection = flowDirection;

			InvokePrivateMethod(button, "ComputeTextRectForLeftAlignment");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_textRectF");

			Assert.Equal(expectedRectF, actualRectF);
		}

		[Theory]
		[InlineData(30, 4, 4, 55, 9, Alignment.Right, FlowDirection.LeftToRight)]
		[InlineData(40, 4, 4, 65, 9, Alignment.Right, FlowDirection.RightToLeft)]
		[InlineData(50, 4, 4, 75, 9, Alignment.End, FlowDirection.LeftToRight)]
		[InlineData(60, 80, 4, 85, 9, Alignment.End, FlowDirection.RightToLeft)]
		public void ComputeTextRectForRightOrEndAlignment_ShouldReturn_TextRectF(double imageSize, float expectedX, float expectedY, float expectedWidth, float expectedHeight, Alignment alignment, FlowDirection flowDirection)
		{
			var expectedRectF = new RectF(expectedX, expectedY, expectedWidth, expectedHeight);

			var button = new SfButton();
			button.ImageSize = imageSize;
			button.ShowIcon = true;
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";
			button.ImageAlignment = alignment;
			button.FlowDirection = flowDirection;

			InvokePrivateMethod(button, "ComputeTextRectForRightAlignment");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_textRectF");

			Assert.Equal(expectedRectF, actualRectF);
		}

		[Theory]
		[InlineData(30, 4, 50, 9, 55)]
		[InlineData(40, 4, 60, 9, 65)]
		[InlineData(50, 4, 70, 9, 75)]
		[InlineData(60, 4, 80, 9, 85)]
		public void ComputeTextRectForTopAlignment_ShouldReturn_TextRectF(double imageSize, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
		{
			var expectedRectF = new RectF(expectedX, expectedY, expectedWidth, expectedHeight);

			var button = new SfButton();
			button.ImageSize = imageSize;
			button.ShowIcon = true;
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";

			InvokePrivateMethod(button, "ComputeTextRectForTopAlignment");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_textRectF");

			Assert.Equal(expectedRectF, actualRectF);
		}

		[Theory]
		[InlineData(30, 4, 4, 9, 47)]
		[InlineData(40, 4, 4, 9, 57)]
		[InlineData(50, 4, 4, 9, 67)]
		[InlineData(60, 4, 4, 9, 77)]
		public void ComputeTextRectForBottomAlignment_ShouldReturn_TextRectF(double imageSize, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
		{
			var expectedRectF = new RectF(expectedX, expectedY, expectedWidth, expectedHeight);

			var button = new SfButton();
			button.ImageSize = imageSize;
			button.ShowIcon = true;
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";

			InvokePrivateMethod(button, "ComputeTextRectForBottomAlignment");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_textRectF");

			Assert.Equal(expectedRectF, actualRectF);
		}

		[Theory]
		[InlineData(30, 34, 4, 39, 9)]
		[InlineData(40, 44, 4, 49, 9)]
		[InlineData(50, 54, 4, 59, 9)]
		[InlineData(60, 64, 4, 69, 9)]
		public void ComputeTextRectForDefaultAlignment_ShouldReturn_TextRectF(double imageSize, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
		{
			var expectedRectF = new RectF(expectedX, expectedY, expectedWidth, expectedHeight);

			var button = new SfButton();
			button.ImageSize = imageSize;
			button.ShowIcon = true;
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";

			InvokePrivateMethod(button, "ComputeTextRectForDefaultAlignment");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_textRectF");

			Assert.Equal(expectedRectF, actualRectF);
		}

		[Theory]
		[InlineData(3, 5.5, 5.5, 12, 12)]
		[InlineData(5, 6.5, 6.5, 14, 14)]
		[InlineData(7, 7.5, 7.5, 16, 16)]
		[InlineData(9, 8.5, 8.5, 18, 18)]
		public void ComputeTextRectWithoutIcon_ShouldReturn_TextRectF(double strokeThickness, float expectedX, float expectedY, float expectedWidth, float expectedHeight)
		{
			var expectedRectF = new RectF(expectedX, expectedY, expectedWidth, expectedHeight);

			var button = new SfButton();
			button.WidthRequest = 150;
			button.HeightRequest = 50;
			button.Text = "Button";
			button.StrokeThickness = strokeThickness;

			InvokePrivateMethod(button, "ComputeTextRectWithoutIcon");
			RectF? actualRectF = (RectF?)GetPrivateField(button, "_textRectF");

			Assert.Equal(expectedRectF, actualRectF);
		}

		[Theory]
		[InlineData(50, 150, 50)]
		[InlineData(150, 50, 150)]
		public void CalculateHeight_ShouldReturnHeight(double height, double width, double expectedValue)
		{
			var button = new SfButton();
			button.WidthRequest = width;
			button.HeightRequest = height;
			button.Text = "Button";

			var actualValue = InvokePrivateMethod(button, "CalculateHeight", height, width);

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(150, 150)]
		[InlineData(100, 100)]
		public void CalculateWidth_ShouldReturnWidth(double width, double expectedValue)
		{
			var button = new SfButton();
			button.WidthRequest = width;
			button.HeightRequest = 50;

			var actualValue = InvokePrivateMethod(button, "CalculateWidth", width);

			Assert.Equal(expectedValue, actualValue);
		}

		[Theory]
		[InlineData(true, false, false, false, "#000000")] 
		[InlineData(true, true, false, false, "#FF0000")] 
		[InlineData(false, false, false, false, "#808080")] 
		[InlineData(true, false, true, true, "#008000")] 
		public void ChangeVisualState_ShouldSetCorrectTextColorString(bool isEnabled, bool isPressed, bool isCheckable, bool isChecked, string expectedColor)
		{
			var button = new SfButton
			{
				IsEnabled = isEnabled,
				IsCheckable = isCheckable,
				IsChecked = isChecked
			};

			AttachVisualStates(button);

			SetPrivateField(button, "_isPressed", isPressed);

			InvokePrivateMethod(button, "ChangeVisualState");

			Assert.Equal(expectedColor, button.TextColor.ToHex());
		}

		private void AttachVisualStates(SfButton button)
		{
			VisualStateGroupList visualStateGroupList = new();
			var visualStateGroup = new VisualStateGroup
			{
				Name = "CommonStates"
			};

			var normalState = new VisualState { Name = "Normal" };
			normalState.Setters.Add(new Setter
			{
				Property = SfButton.TextColorProperty,
				Value = Colors.Black
			});
			visualStateGroup.States.Add(normalState);

			var pressedState = new VisualState { Name = "Pressed" };
			pressedState.Setters.Add(new Setter
			{
				Property = SfButton.TextColorProperty,
				Value = Colors.Red
			});
			visualStateGroup.States.Add(pressedState);

			var hoveredState = new VisualState { Name = "Hovered" };
			hoveredState.Setters.Add(new Setter
			{
				Property = SfButton.TextColorProperty,
				Value = Colors.Blue
			});
			visualStateGroup.States.Add(hoveredState);

			var disabledState = new VisualState { Name = "Disabled" };
			disabledState.Setters.Add(new Setter
			{
				Property = SfButton.TextColorProperty,
				Value = Colors.Gray
			});
			visualStateGroup.States.Add(disabledState);

			var checkedState = new VisualState { Name = "Checked" };
			checkedState.Setters.Add(new Setter
			{
				Property = SfButton.TextColorProperty,
				Value = Colors.Green
			});
			visualStateGroup.States.Add(checkedState);

			visualStateGroupList.Add(visualStateGroup);
			VisualStateManager.SetVisualStateGroups(button, visualStateGroupList);
		}

		#endregion

		#region HorizontalOptions Width Tests

		[Theory]
		[InlineData(LayoutAlignment.Start)]
		[InlineData(LayoutAlignment.Center)]
		[InlineData(LayoutAlignment.End)]
		public void CalculateWidth_WithNonFillHorizontalOptions_ShouldUseContentWidth(LayoutAlignment alignment)
		{
			var button = new SfButton();
			button.Text = "Sample Text";
			button.HorizontalOptions = new LayoutOptions(alignment, false);
			
			// Test with available width constraint larger than content
			double widthConstraint = 300;
			var actualWidth = (double)InvokePrivateMethod(button, "CalculateWidth", widthConstraint);
			
			// Should not use the full constraint width, but rather content-based width
			Assert.True(actualWidth < widthConstraint, 
				$"Button with HorizontalOptions.{alignment} should not fill constraint width {widthConstraint}, but got {actualWidth}");
		}

		[Fact]
		public void CalculateWidth_WithFillHorizontalOptions_ShouldUseConstraintWidth()
		{
			var button = new SfButton();
			button.Text = "Sample Text";
			button.HorizontalOptions = LayoutOptions.Fill;
			
			// Test with available width constraint
			double widthConstraint = 300;
			var actualWidth = (double)InvokePrivateMethod(button, "CalculateWidth", widthConstraint);
			
			// Should use the constraint width when HorizontalOptions is Fill
			Assert.Equal(widthConstraint, actualWidth);
		}

		[Fact]
		public void CalculateWidth_WithWidthRequest_ShouldAlwaysUseWidthRequest()
		{
			var button = new SfButton();
			button.Text = "Sample Text";
			button.WidthRequest = 150;
			button.HorizontalOptions = LayoutOptions.Fill;
			
			// Test with larger width constraint
			double widthConstraint = 300;
			var actualWidth = (double)InvokePrivateMethod(button, "CalculateWidth", widthConstraint);
			
			// Should use WidthRequest regardless of HorizontalOptions
			Assert.Equal(150, actualWidth);
		}

		[Fact]
		public void CalculateWidth_WithInfiniteConstraint_ShouldUseContentWidth()
		{
			var button = new SfButton();
			button.Text = "Sample Text";
			button.HorizontalOptions = LayoutOptions.Fill;
			
			// Test with infinite width constraint
			double widthConstraint = double.PositiveInfinity;
			var actualWidth = (double)InvokePrivateMethod(button, "CalculateWidth", widthConstraint);
			
			// Should fall back to content width even with Fill when constraint is infinite
			Assert.True(actualWidth > 0 && actualWidth != double.PositiveInfinity, 
				$"Button should calculate content width when constraint is infinite, but got {actualWidth}");
		}

		#endregion

		#region Text Wrapping Tests

		[Fact]
		public void TextWrapping_ShouldWrapWithoutWidthRequest()
		{
			var button = new SfButton();
			button.Text = "This is a very long text that should automatically wrap into multiple lines and resize the button height accordingly";
			button.LineBreakMode = LineBreakMode.WordWrap;
			button.HorizontalOptions = LayoutOptions.Start;
			button.VerticalOptions = LayoutOptions.Start;

			// Measure with width constraint but no WidthRequest
			var size = button.MeasureContent(200, double.PositiveInfinity);
			
			// Calculate expected single line height for comparison
			var singleLineButton = new SfButton();
			singleLineButton.Text = "Short text";
			singleLineButton.LineBreakMode = LineBreakMode.NoWrap;
			var singleLineSize = singleLineButton.MeasureContent(200, double.PositiveInfinity);

			// Height should be greater than single line due to text wrapping
			Assert.True(size.Height > singleLineSize.Height, 
				$"Button height {size.Height} should be greater than single line height {singleLineSize.Height} when text wraps");
			
			// Width should not exceed the constraint
			Assert.True(size.Width <= 200, 
				$"Button width {size.Width} should not exceed width constraint of 200");
		}

		[Fact]
		public void TextWrapping_ShouldRespectWidthRequest()
		{
			var button = new SfButton();
			button.Text = "This is a very long text that should automatically wrap into multiple lines and resize the button height accordingly";
			button.LineBreakMode = LineBreakMode.WordWrap;
			button.WidthRequest = 150;

			// Measure with larger width constraint, but WidthRequest should take precedence
			var size = button.MeasureContent(300, double.PositiveInfinity);
			
			// Width should be close to WidthRequest (accounting for padding)
			Assert.True(size.Width >= 150, 
				$"Button width {size.Width} should respect WidthRequest of 150");
		}

		[Fact]
		public void TextWrapping_WithIcon_ShouldAccountForIconSpace()
		{
			var button = new SfButton();
			button.Text = "This is a very long text that should automatically wrap into multiple lines";
			button.LineBreakMode = LineBreakMode.WordWrap;
			button.ShowIcon = true;
			button.ImageAlignment = Alignment.Start; // Icon on left side
			button.ImageSize = 20;

			// Measure with width constraint
			var sizeWithIcon = button.MeasureContent(200, double.PositiveInfinity);
			
			// Compare with button without icon
			var buttonNoIcon = new SfButton();
			buttonNoIcon.Text = button.Text;
			buttonNoIcon.LineBreakMode = LineBreakMode.WordWrap;
			var sizeNoIcon = buttonNoIcon.MeasureContent(200, double.PositiveInfinity);

			// Button with icon should potentially wrap more (higher height) due to less available text width
			Assert.True(sizeWithIcon.Height >= sizeNoIcon.Height, 
				$"Button with icon height {sizeWithIcon.Height} should be >= button without icon height {sizeNoIcon.Height}");
		}

		#endregion

		#region AutomationScenario

		[Theory]
		[InlineData("SampleImage1.png", "#FFFFFF")]
		[InlineData("SampleImage2.png", "#FF5733")]
		public void BackgroundImageSource_Background(string imagePath, string colorHex)
		{
			var button = new SfButton();
			var expectedValue = ImageSource.FromFile(imagePath);
			var expectedBrush = GetSolidColorBrush(colorHex);
			button.BackgroundImageSource = expectedValue;
			button.Background = expectedBrush;
			var actualValue = button.BackgroundImageSource;
			var actualBrush = button.Background;
			var backgroundImageView = (Image?)GetPrivateMember(button, "_backgroundImageView");
			Assert.Equal(expectedValue, actualValue);
			Assert.Equal(expectedValue, backgroundImageView?.Source);
			Assert.Equal(expectedBrush.Color, actualBrush);
		}

		#endregion
	}
}
