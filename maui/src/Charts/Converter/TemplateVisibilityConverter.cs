using System.Globalization;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal class TemplateVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (object item in values)
            {
                if (item != null && (double)item >= 0)
                    continue;
                else
                    return false;
            }

            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null!;
        }
    }
}