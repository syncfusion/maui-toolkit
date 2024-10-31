using Syncfusion.Maui.Toolkit.TabView;
using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.TabView.SfTabView
{
    /// <summary>
    /// Represents the Customization view for the TabView sample.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Customization : SampleView
    {
        /// <summary>
        /// Gets or sets the color of the selected item.
        /// </summary>
        public Color SelectedItemColor
        {
            get { return (Color)GetValue(SelectedItemColorProperty); }
            set { SetValue(SelectedItemColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItemColor.  This enables animation, styling, binding, etc...
        public static readonly BindableProperty SelectedItemColorProperty =
            BindableProperty.Create(nameof(SelectedItemColor), typeof(Color), typeof(Customization), Colors.RoyalBlue);

        public Customization()
        {
            InitializeComponent();
            this.BindingContext = this;

            this.TabView.IndicatorBackground = new SolidColorBrush(Colors.RoyalBlue);
            this.SelectedItemColor = ((SolidColorBrush)this.TabView.IndicatorBackground).Color;
            int r = Convert.ToInt32(this.SelectedItemColor.Red * 255);
            int g = Convert.ToInt32(this.SelectedItemColor.Green * 255);
            int b = Convert.ToInt32(this.SelectedItemColor.Blue * 255);

            this.TabView.TabBarBackground = new SolidColorBrush(Color.FromRgba(r, g, b, 25));
        }

        /// <summary>
        /// Handles the SelectionChanged event of the SfTabView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Syncfusion.Maui.Toolkit.TabView.TabSelectionChangedEventArgs"/> instance containing the event data.</param>
        private void SfTabView_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.TabView.TabSelectionChangedEventArgs e)
        {
            var view = (Syncfusion.Maui.Toolkit.TabView.SfTabView)sender;

            SfTabItem item = view.Items[e.NewIndex];

            var selectedFile = item.Header;

            if (item.Header == "Document")
            {
                view.IndicatorBackground = new SolidColorBrush(Colors.RoyalBlue);
            }
            else if (item.Header == "Excel")
            {
                view.IndicatorBackground = new SolidColorBrush(Colors.Green);
            }
            else if (item.Header == "PDF")
            {
                view.IndicatorBackground = new SolidColorBrush(Colors.DarkRed);
            }
            else if (item.Header == "PowerPoint")
            {
                view.IndicatorBackground = new SolidColorBrush(Colors.Red);
            }

            this.SelectedItemColor = ((SolidColorBrush)view.IndicatorBackground).Color;
            int r = Convert.ToInt32(this.SelectedItemColor.Red * 255);
            int g = Convert.ToInt32(this.SelectedItemColor.Green * 255);
            int b = Convert.ToInt32(this.SelectedItemColor.Blue * 255);

            view.TabBarBackground = new SolidColorBrush(Color.FromRgba(r, g, b, 25));
        }
    }

    /// <summary>
    /// Converts a file name and format to a formatted text description.
    /// </summary>
    public class TextToFormatTextConverter : IValueConverter
    {
        /// <summary>
        /// Converts a file name and format to a formatted text description.
        /// </summary>
        /// <param name="value">The file name.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The file format.</param>
        /// <param name="culture">The culture information.</param>
        /// <returns>A formatted text description of the file.</returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                string fileName = (string)value;

                string format = (string)parameter;

                if (format == ".docx")
                {
                    fileName += " Document File";
                }
                else if (format == ".xlsx")
                {
                    fileName += " Excel File";
                }
                else if (format == ".pdf")
                {
                    fileName += " PDF File";
                }
                else if (format == ".pptx")
                {
                    fileName += " PowerPoint File";
                }
                return fileName += format;
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // This converter does not support converting back.
            return null;
        }
    }
}