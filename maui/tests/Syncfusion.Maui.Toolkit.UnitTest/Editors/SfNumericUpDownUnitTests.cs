using System.Globalization;
using Syncfusion.Maui.Toolkit.NumericUpDown;
using Syncfusion.Maui.Toolkit.TextInputLayout;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class SfNumericUpDownUnitTests : BaseUnitTest
	{

		#region Constructor

		[Fact]
		public void Constructor_InitializesDefaultsCorrectly()
		{
			var numericUpDown = new SfNumericUpDown();
			Assert.False(numericUpDown.AutoReverse);
			Assert.Equal(NumericUpDownPlacementMode.Inline, numericUpDown.UpDownPlacementMode);
			Assert.Equal(Colors.Black, numericUpDown.UpDownButtonColor);
			Assert.Null(numericUpDown.DownButtonTemplate);
			Assert.Null(numericUpDown.UpButtonTemplate);
			Assert.Equal(1.0, numericUpDown.SmallChange);
			Assert.Equal(10.0, numericUpDown.LargeChange);
			Assert.Equal(UpDownButtonAlignment.Right, numericUpDown.UpDownButtonAlignment);
			Assert.Null(numericUpDown.DownButtonTemplate);
			Assert.Null(numericUpDown.UpButtonTemplate);
		}

		#endregion

		#region Property


		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void AutoReverseProperty_ShouldSetAndGetCorrectly(bool value)
		{
			var numericUpDown = new SfNumericUpDown();

			numericUpDown.AutoReverse = value;

			Assert.Equal(value, numericUpDown.AutoReverse);
		}


		[Theory]
		[InlineData(NumericUpDownPlacementMode.Inline)]
		[InlineData(NumericUpDownPlacementMode.InlineVertical)]
		public void UpDownPlacementModeProperty_ShouldSetAndGetCorrectly(NumericUpDownPlacementMode value)
		{
			var numericUpDown = new SfNumericUpDown();

			numericUpDown.UpDownPlacementMode = value;
			InvokePrivateMethod(numericUpDown, "UpdateElementsBounds", new Microsoft.Maui.Graphics.RectF(0, 0, 0, 0), false);

			Assert.Equal(value, numericUpDown.UpDownPlacementMode);
		}

		[Theory]
		[InlineData(UpDownButtonAlignment.Right)]
		[InlineData(UpDownButtonAlignment.Left)]
		[InlineData(UpDownButtonAlignment.Both)]
		public void UpDownButtonAlignmentProperty_ShouldSetAndGetCorrectly(UpDownButtonAlignment expectedValue)
		{
			var numericUpDown = new SfNumericUpDown();
			numericUpDown.UpDownButtonAlignment = expectedValue;
			InvokePrivateMethod(numericUpDown, "ConfigureInlineButtonPositions", new RectF(10,10,10,10));
			Assert.Equal(expectedValue, numericUpDown.UpDownButtonAlignment);
		}

		[Theory]
		[InlineData("#FF0000")] 
		[InlineData("#0000FF")] 
		[InlineData("#00FF00")] 
		public void UpDownButtonColorProperty_ShouldSetAndGetCorrectly(string colorHex)
		{
			var numericUpDown = new SfNumericUpDown();
			var expectedColor = Color.FromArgb(colorHex); 

			numericUpDown.UpDownButtonColor = expectedColor;

			Assert.Equal(expectedColor, numericUpDown.UpDownButtonColor);
		}

		[Theory]
		[InlineData("#FF0000", 0, 0)] 
		[InlineData("#0000FF", 12345, 0)] 
		[InlineData("#00FF00", 0, 12345)] 
		public void UpDownButtonDisableColor_ShouldSetAndGetCorrectly(string colorHex, double minimum, double maximum)
		{
			var numericUpDown = new SfNumericUpDown();
			var expectedColor = Color.FromArgb(colorHex);
			numericUpDown.IsEnabled = false;
			numericUpDown.Value = 12345;
			numericUpDown.Minimum = minimum;
			numericUpDown.Maximum = maximum;
			SetNonPublicProperty(numericUpDown, "UpDownButtonDisableColor", expectedColor);
			var actualColor = GetNonPublicProperty(numericUpDown, "UpDownButtonDisableColor");
			Assert.Equal(expectedColor, actualColor);
		}

		[Theory]
		[InlineData("Template1")]
		[InlineData("Template2")]
		public void DownButtonTemplateProperty_ShouldSetAndGetCorrectly(string templateName)
		{
			var numericUpDown = new SfNumericUpDown();
			var template = new DataTemplate(() => new Label { Text = templateName });

			numericUpDown.DownButtonTemplate = template;

			Assert.Equal(template, numericUpDown.DownButtonTemplate);
		}

		[Theory]
		[InlineData("TemplateA")]
		[InlineData("TemplateB")]
		public void UpButtonTemplateProperty_ShouldSetAndGetCorrectly(string templateName)
		{
			var numericUpDown = new SfNumericUpDown();
			var template = new DataTemplate(() => new Label { Text = templateName });

			numericUpDown.UpButtonTemplate = template;

			Assert.Equal(template, numericUpDown.UpButtonTemplate);
		}

		[Theory]
		[InlineData(2.0)]
		[InlineData(5.0)]
		public void SmallChangeProperty_ShouldSetAndGetCorrectly(double value)
		{
			var numericUpDown = new SfNumericUpDown();

			numericUpDown.SmallChange = value;

			Assert.Equal(value, numericUpDown.SmallChange);
		}


		[Theory]
		[InlineData(20.0)]
		[InlineData(50.0)]
		public void LargeChangeProperty_ShouldSetAndGetCorrectly(double value)
		{
			var numericUpDown = new SfNumericUpDown();

			numericUpDown.LargeChange = value;

			Assert.Equal(value, numericUpDown.LargeChange);
		}

		[Fact]
		public void UpDownButtonTemplateWithTextInputLayout_ShouldSetAndGetCorrectly()
		{
			var numericUpDown = new SfNumericUpDown();
			var upTemplate = new DataTemplate(() => new Label { Text = "Up Button" });
			var downTemplate = new DataTemplate(() => new Label { Text = "Down Button" });
			
			var textInputLayout = new SfTextInputLayout { Content = numericUpDown };

			numericUpDown.UpButtonTemplate = upTemplate;
			numericUpDown.DownButtonTemplate = downTemplate;

			Assert.Equal(upTemplate, numericUpDown.UpButtonTemplate);
			Assert.Equal(downTemplate, numericUpDown.DownButtonTemplate);

		}

		#endregion

		#region Private Methods

		[Fact]
		public void UpdateTextInputLayoutUI_ShouldUpdate_LayoutCorrectly()
		{
			var numericUpDown = new SfNumericUpDown();
			var textInputLayout = new SfTextInputLayout { Content = numericUpDown };

			var expectedValue = (SfTextInputLayout?)GetPrivateField(numericUpDown, "_textInputLayout");
			Assert.NotNull(expectedValue);
		}


		[Theory]
		[InlineData(UpDownButtonAlignment.Left, 60, 30)]
		[InlineData(UpDownButtonAlignment.Right, 0, 90)]
		[InlineData(UpDownButtonAlignment.Both, 30, 60)]
		public void UpdateTextBoxMargin_ShouldUpdate_MarginCorrectly(UpDownButtonAlignment alignment, double expectedLeftMargin, double expectedRightMargin)
		{
			var numericUpDown = new SfNumericUpDown();
			numericUpDown.UpDownButtonAlignment = alignment;
			numericUpDown.UpDownPlacementMode = NumericUpDownPlacementMode.Inline;
			SetPrivateField(numericUpDown, "_isClearButtonVisible", true);
			InvokePrivateMethod(numericUpDown, "ConfigureClearButton", new RectF(10,10,10,10));
			
			var actualLeftMargin = GetPrivateField(numericUpDown, "_leftMargin");
			var actualRightMargin = GetPrivateField(numericUpDown, "_rightMargin");
			Assert.Equal(expectedLeftMargin, actualLeftMargin);
			Assert.Equal(expectedRightMargin, actualRightMargin);
		}

		[Theory]
		[InlineData(PointerActions.Released)]
		[InlineData(PointerActions.Pressed)]
		public void OnTouch_ShouldReturn_TouchPoint(PointerActions pointerActions)
		{
			var numericUpDown = new SfNumericUpDown();
			Point expectedValue = new Point(10, 10);
			var pointerEventArgs = new Internals.PointerEventArgs(1, pointerActions, expectedValue);
			numericUpDown.OnTouch(pointerEventArgs);
			var touchPoint = (Point?)GetPrivateField(numericUpDown, "_touchPoint");
			
			Assert.Equal(expectedValue, touchPoint);
		}

		[Theory]
		[InlineData(100, 100, 60, 0)]
		[InlineData(double.PositiveInfinity, double.PositiveInfinity, 60, 0)]
		[InlineData(-51, 100, -51, 0)]
		[InlineData(100, -89, 60, -89)]
		public void MeasureContent_ShouldReturn_SizeCorrectly(double width, double height, double expectedWidth, double expectedHeight)
		{
			var numericUpDown = new SfNumericUpDown();
			var size = (Size?)InvokePrivateMethod(numericUpDown, "MeasureContent", width, height);
			Assert.NotNull(size);
			Assert.Equal(expectedHeight, size.Value.Height);
			Assert.Equal(expectedWidth, size.Value.Width);
		}

		#endregion
	}
}
