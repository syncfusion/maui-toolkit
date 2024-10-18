
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ContentProperty("Source")]
    public class ImageResourceExtension : IMarkupExtension<ImageSource>
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Source { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        /// <exception cref="XamlParseException"></exception>
        public ImageSource ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(Source))
            {
                IXmlLineInfo lineInfo = (serviceProvider.GetService(typeof(IXmlLineInfoProvider)) is IXmlLineInfoProvider lineInfoProvider) ? lineInfoProvider.XmlLineInfo : new XmlLineInfo();
                throw new XamlParseException("ImageResourceExtension requires Source property to be set", lineInfo);
            }

            string? assemblyName = typeof(ImageResourceExtension).GetTypeInfo().Assembly.GetName().Name; //GetType().GetTypeInfo().Assembly.GetName().Name;
            return ImageSource.FromResource(assemblyName + ".Resources.Images." + Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<ImageSource>).ProvideValue(serviceProvider);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ImageSourceConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string? source = value as string;
            string? assemblyName = typeof(ImageSourceConverter).GetTypeInfo().Assembly.GetName().Name; //GetType().GetTypeInfo().Assembly.GetName().Name;
            return ImageSource.FromResource(assemblyName + ".Resources.Images." + source, typeof(ImageSourceConverter).GetTypeInfo().Assembly);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
