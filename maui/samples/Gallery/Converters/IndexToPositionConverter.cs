using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class IndexToPositionConverter : IValueConverter
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

            foreach (var item in Syncfusion.Maui.ControlsGallery.PositionHelper.ControlTiles)
            {
                if (item.Name == value?.ToString())
                {
                    return item.Bounds;
                }
            }

            Rect updatedBounds = new()
            {
                Width = 300,
                Height = 50
            };

            updatedBounds.X = (Syncfusion.Maui.ControlsGallery.PositionHelper.Column * updatedBounds.Width);
            updatedBounds.Y = Syncfusion.Maui.ControlsGallery.PositionHelper.CurrentBounds.Y + updatedBounds.Height;

            Syncfusion.Maui.ControlsGallery.PositionHelper.ControlTiles.Add(new ControlTile() { Name = value?.ToString()!, Bounds = updatedBounds });
            Syncfusion.Maui.ControlsGallery.PositionHelper.CurrentBounds = updatedBounds;

            return updatedBounds;
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
