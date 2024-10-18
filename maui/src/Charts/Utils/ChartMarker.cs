using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ChartMarker
    {
        /// <summary>
        /// Identifies the <see cref="ShowMarkersProperty"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="ShowMarkersProperty"/> bindable property determines whether 
        /// data markers are shown on the chart.
        /// </remarks>
        public static readonly BindableProperty ShowMarkersProperty = BindableProperty.Create(
            nameof(IMarkerDependent.ShowMarkers),
            typeof(bool),
            typeof(IMarkerDependent),
            false,
            propertyChanged: OnShowMarkersChanged);

        /// <summary>
        /// Identifies the <see cref="MarkerSettingsProperty"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="MarkerSettingsProperty"/> bindable property determines the
        /// customization options for the data markers on the chart.
        /// </remarks>
        public static readonly BindableProperty MarkerSettingsProperty = BindableProperty.Create(
            nameof(IMarkerDependent.MarkerSettings),
            typeof(ChartMarkerSettings),
            typeof(IMarkerDependent),
            propertyChanged: OnMarkerSettingsChanged);

        static void OnShowMarkersChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IMarkerDependent)bindable).OnShowMarkersChanged((bool)oldValue, (bool)newValue);
        }

        static void OnMarkerSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue is ChartMarkerSettings newSetting)
            {
                newSetting.HookStylePropertyChanged(bindable, newSetting);
            }

            if (oldValue is ChartMarkerSettings oldSetting)
            {
                oldSetting.UnHookStylePropertyChanged(bindable, oldSetting);
            }

            ((IMarkerDependent)bindable).InvalidateDrawable();
        }
    }
}
