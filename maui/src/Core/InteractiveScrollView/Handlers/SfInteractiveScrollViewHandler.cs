namespace Syncfusion.Maui.Toolkit.Internals
{
    using Microsoft.Maui;

    /// <summary>
    /// Provides the platform-specific handler implementation for the <see cref="SfInteractiveScrollView"/> control.
    /// </summary>
    internal partial class SfInteractiveScrollViewHandler
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfInteractiveScrollViewHandler"/> class.
        /// </summary>
        public SfInteractiveScrollViewHandler()
            : base(Mapper, CommandMapper)
        {
        }

        #endregion

        #region Mappers

        /// <summary>
        /// Defines the property mappings between <see cref="SfInteractiveScrollView"/> and its platform-specific handler implementation.
        /// </summary>
        internal static IPropertyMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler> Mapper =
            new PropertyMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler>(ViewMapper)
            {
                [nameof(SfInteractiveScrollView.PresentedContent)] = MapContent,
                [nameof(SfInteractiveScrollView.HorizontalScrollBarVisibility)] = MapHorizontalScrollBarVisibility,
                [nameof(SfInteractiveScrollView.VerticalScrollBarVisibility)] = MapVerticalScrollBarVisibility,
                [nameof(SfInteractiveScrollView.Orientation)] = MapScrollOrientation,

#if ANDROID
            [nameof(SfInteractiveScrollView.SuppressAutoScroll)] = MapSuppressAutoScroll,
#endif

#if IOS || MACCATALYST
            [nameof(SfInteractiveScrollView.ContentSize)] = MapContentSize,
            [nameof(SfInteractiveScrollView.IsEnabled)] = MapIsEnabled,
            [nameof(SfInteractiveScrollView.CanBecomeFirstResponder)] = MapCanBecomeFirstResponder,
#endif

#if WINDOWS
            [nameof(SfInteractiveScrollView.ContentSize)] = MapContentSize,
#endif
            };

        /// <summary>
        /// Defines the command mappings between <see cref="SfInteractiveScrollView"/> and its platform-specific handler implementation.
        /// </summary>
        internal static CommandMapper<SfInteractiveScrollView, SfInteractiveScrollViewHandler> CommandMapper =
            new(ViewCommandMapper)
            {
                [nameof(SfInteractiveScrollView.ScrollTo)] = MapScrollTo
            };

        #endregion
    }
}