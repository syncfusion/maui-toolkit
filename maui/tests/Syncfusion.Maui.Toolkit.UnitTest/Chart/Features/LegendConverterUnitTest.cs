using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Maui.Graphics.Color;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class LegendConverterUnitTest : BaseUnitTest
	{
		#region MultiBindingBrushCOnverter

		private readonly MultiBindingIconBrushConverter _converter = new MultiBindingIconBrushConverter();
		private static readonly object[] values = Array.Empty<object>();

		[Fact]
		public void Convert_NullValues_ReturnsTransparent()
		{
			object parameter  = new object();
			var result = _converter.Convert(values, typeof(Brush), parameter, CultureInfo.InvariantCulture);
			Assert.Equal(Colors.Transparent, ((Color)result));
		}

		[Fact]
		public void Convert_InvalidTargetType_ReturnsTransparent()
		{
			object parameter = new object();
			var result = _converter.Convert(values, typeof(string), parameter, CultureInfo.InvariantCulture);
			Assert.Equal(Colors.Transparent, ((Color)result));
		}

		[Fact]
		public void Convert_ValidShapeToggled_ReturnsDisableBrush()
		{
			var legendItem = new LegendItem { IsToggled = true, DisableBrush = new SolidColorBrush(Colors.Red) };
			var shapeView = new SfShapeView { BindingContext = legendItem };

			var result = _converter.Convert(values, typeof(Brush), shapeView, CultureInfo.InvariantCulture);
			Assert.Equal(((SolidColorBrush)legendItem.DisableBrush).Color, ((SolidColorBrush)result).Color);
		}

		[Fact]
		public void Convert_ValidShapeNotToggled_ReturnsIconBrush()
		{
			var legendItem = new LegendItem { IsToggled = false, IconBrush = new SolidColorBrush(Colors.Blue) };
			var shapeView = new SfShapeView { BindingContext = legendItem };

			var result = _converter.Convert(values, typeof(Brush), shapeView, CultureInfo.InvariantCulture);
			Assert.Equal(((SolidColorBrush)legendItem.IconBrush).Color, ((SolidColorBrush)result).Color);
		}

		[Fact]
		public void Convert_ValidLabelToggled_ReturnsDisableBrushColor()
		{
			var legendItem = new LegendItem { IsToggled = true, DisableBrush = new SolidColorBrush(Colors.Yellow) };
			var label = new Label { BindingContext = legendItem };

			var result = _converter.Convert(values, typeof(Color), label, CultureInfo.InvariantCulture);
			Assert.Equal(((SolidColorBrush)legendItem.DisableBrush).Color, result);
		}

		[Fact]
		public void Convert_ValidLabelNotToggled_ReturnsTextColor()
		{
			var legendItem = new LegendItem { IsToggled = false, TextColor = Colors.Green };
			var label = new Label { BindingContext = legendItem };

			var result = _converter.Convert(values, typeof(Color), label, CultureInfo.InvariantCulture);
			Assert.Equal(legendItem.TextColor, result);
		}
		#endregion

		#region ToggleColorConverter

		private readonly ToggleColorConverter _toggleConverter = new ToggleColorConverter();

		[Fact]
		public void Convert_NullValue_ReturnsTransparent()
		{
			var result = _toggleConverter.Convert(null, typeof(Brush), null, CultureInfo.InvariantCulture);

			Assert.NotNull(result);
			Assert.Equal(Colors.Transparent, (Color)result);
		}

		[Fact]
		public void Convert_NullParameter_ReturnsTransparent()
		{
			var result = _toggleConverter.Convert(true, typeof(Brush), null, CultureInfo.InvariantCulture);
			Assert.NotNull(result);
			Assert.Equal(Colors.Transparent, (Color)result);
		}

		[Fact]
		public void ConvertToggle_ValidShapeToggled_ReturnsDisableBrush()
		{
			var legendItem = new LegendItem { DisableBrush = new SolidColorBrush(Colors.Red) };
			var shapeView = new SfShapeView { BindingContext = legendItem };

			var result = _toggleConverter.Convert(true, typeof(Brush), shapeView, CultureInfo.InvariantCulture);
			Assert.NotNull(result);
			Assert.Equal(Colors.Red, ((SolidColorBrush)result).Color);
		}

		[Fact]
		public void ConvertToggle_ValidShapeNotToggled_ReturnsIconBrush()
		{
			var legendItem = new LegendItem { IconBrush = new SolidColorBrush(Colors.Blue) };
			var shapeView = new SfShapeView { BindingContext = legendItem };

			var result = _toggleConverter.Convert(false, typeof(Brush), shapeView, CultureInfo.InvariantCulture);
			Assert.NotNull(result);
			Assert.Equal(Colors.Blue, ((SolidColorBrush)result).Color);
		}

		[Fact]
		public void ConvertToggle_ValidLabelToggled_ReturnsDisableBrushColor()
		{
			var legendItem = new LegendItem { DisableBrush = new SolidColorBrush(Colors.Yellow) };
			var label = new Label { BindingContext = legendItem };

			var result = _toggleConverter.Convert(true, typeof(Color), label, CultureInfo.InvariantCulture);
			Assert.Equal(Colors.Yellow, result);
		}

		[Fact]
		public void ConvertToggle_ValidLabelNotToggled_ReturnsTextColor()
		{
			var legendItem = new LegendItem { TextColor = Colors.Green };
			var label = new Label { BindingContext = legendItem };

			var result = _toggleConverter.Convert(false, typeof(Color), label, CultureInfo.InvariantCulture);
			Assert.Equal(Colors.Green, result);
		}

		

		#endregion
	}
}
