#region Copyright Syncfusion Inc. 2001-2024.
// Copyright Syncfusion Inc. 2001-2024. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	public partial class DataLabelTemplate : SampleView
	{
		public DataLabelTemplate()
		{
			InitializeComponent();
		}

		public override void OnAppearing()
		{
			base.OnAppearing();

#if !WINDOWS && !MACCATALYST
			title.Text = "Tracking USA Box Office Revenue Since 2000";
#endif
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			chart.Handler?.DisconnectHandler();
		}
	}

	public class DoubleToFontIconConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (parameter is not string para)
				return null;

			double doubleValue = 0;

			if (value is ChartDataModel model)
			{
				if (para == "GrossLastYearDelta")
				{
					return model.GrossLastYearDelta;
				}
				doubleValue = (double)model.GrossLastYearDelta;
			}

			string text = "\ue704";
			Color color = Colors.Red;
			TextAlignment alignment = TextAlignment.End;
			if (doubleValue >= 0)
			{
				text = "\ue705";
				color = Colors.Green;
				alignment = TextAlignment.Start;
			}

			//FontImageSource source = new FontImageSource() { Glyph = text, Color = color, FontFamily = "MauiSampleFontIcon", Size = 30};

			return para == "icon" ? text : para == "position" ? alignment : color;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
