using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.NumericEntry.SfNumericEntry;
public partial class NumericEntryCultureAndFormattingMobile : SampleView
{
	public NumericEntryCultureAndFormattingMobile()
	{
		InitializeComponent();
        currencyCulture.Culture = new CultureInfo("en-US");
        decimalCulture.Culture = new CultureInfo("en-US");
        percentCulture.Culture = new CultureInfo("en-US");
        usBorder.SetAppThemeColor(Microsoft.Maui.Controls.Border.StrokeProperty, Color.FromArgb("#49454F"), Color.FromArgb("#CAC4D0"));
    }

    private void IndiaImageButton_Clicked(object sender, EventArgs e)
    {
        currencyCulture.Culture = new CultureInfo("en-IN");
        decimalCulture.Culture = new CultureInfo("en-IN");
        percentCulture.Culture = new CultureInfo("en-IN");
        ResetColor();
        ResetSize();
        indiaBorder.SetAppThemeColor(Microsoft.Maui.Controls.Border.StrokeProperty, Color.FromArgb("#49454F"), Color.FromArgb("#CAC4D0"));
        indiaStack.HeightRequest = 34;
        indiaStack.WidthRequest = 42;
#if IOS
        indiaImage.WidthRequest = 40;
        indiaImage.HeightRequest = 32;
#endif
    }

    private void USImageButton_Clicked(object sender, EventArgs e)
    {
        currencyCulture.Culture = new CultureInfo("en-US");
        decimalCulture.Culture = new CultureInfo("en-US");
        percentCulture.Culture = new CultureInfo("en-US");
        ResetColor();
        ResetSize();
        usBorder.SetAppThemeColor(Microsoft.Maui.Controls.Border.StrokeProperty, Color.FromArgb("#49454F"), Color.FromArgb("#CAC4D0"));
        usStack.HeightRequest = 34;
        usStack.WidthRequest = 42;
#if IOS
        usImage.WidthRequest = 40;
        usImage.HeightRequest = 32;
#endif
    }

    private void RussiaImageButton_Clicked(object sender, EventArgs e)
    {
        currencyCulture.Culture = new CultureInfo("ru-RU");
        decimalCulture.Culture = new CultureInfo("ru-RU");
        percentCulture.Culture = new CultureInfo("ru-RU");
        ResetColor();
        ResetSize();
        russiaBorder.SetAppThemeColor(Microsoft.Maui.Controls.Border.StrokeProperty, Color.FromArgb("#49454F"), Color.FromArgb("#CAC4D0"));
        russiaStack.HeightRequest = 34;
        russiaStack.WidthRequest = 42;
#if IOS
        russiaImage.WidthRequest = 40;
        russiaImage.HeightRequest = 32;
#endif
    }


    private void FranceImageButton_Clicked(object sender, EventArgs e)
    {
        currencyCulture.Culture = new CultureInfo("fr-FR");
        decimalCulture.Culture = new CultureInfo("fr-FR");
        percentCulture.Culture = new CultureInfo("fr-FR");
        ResetColor();
        ResetSize();
        franceBorder.SetAppThemeColor(Microsoft.Maui.Controls.Border.StrokeProperty, Color.FromArgb("#49454F"), Color.FromArgb("#CAC4D0"));
        franceStack.HeightRequest = 34;
        franceStack.WidthRequest = 42;
#if IOS
        franceImage.WidthRequest = 40;
        franceImage.HeightRequest = 32;
#endif
    }

    void ResetColor()
    {
        indiaBorder.Stroke = Colors.Transparent;
        franceBorder.Stroke = Colors.Transparent;
        usBorder.Stroke = Colors.Transparent;
        russiaBorder.Stroke = Colors.Transparent;
    }

    void ResetSize()
    {
        usStack.WidthRequest = 36;
        usStack.HeightRequest = 28;
        indiaStack.WidthRequest = 36;
        indiaStack.HeightRequest = 28;
        russiaStack.WidthRequest = 36;
        russiaStack.HeightRequest = 28;
        franceStack.WidthRequest = 36;
        franceStack.HeightRequest = 28;
#if IOS
        usImage.WidthRequest = 36;
        usImage.HeightRequest = 28;
        indiaImage.WidthRequest = 36;
        indiaImage.HeightRequest = 28;
        russiaImage.WidthRequest = 36;
        russiaImage.HeightRequest = 28;
        franceImage.WidthRequest = 36;
        franceImage.HeightRequest = 28;
#endif
    }
}