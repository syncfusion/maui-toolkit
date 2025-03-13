namespace Syncfusion.Maui.Toolkit.Cards
{
    /// <summary>
    /// Represents a class that holds the helper methods for cards.
    /// </summary>
    internal static class CardsHelper
    {
        #region Internal Methods

        /// <summary>
        /// Gets the RTL value true or false.
        /// </summary>
        /// <param name="view">The type of view.</param>
        /// <returns>Return true or false</returns>
        internal static bool IsRTL(this View view)
        {
            //// Flow direction property only added in VisualElement class only, so that we return while the object type is not visual element.
            //// And Effective flow direction is flag type so that we add & RightToLeft.
            if (!(view is IVisualElementController))
            {
                return false;
            }

            return ((view as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;
        }

        /// <summary>
        /// Method to update the card opacity.
        /// </summary>
        /// <param name="opacity">Opacity value of the card.</param>
        /// <param name="view">View added to the card layout.</param>
        internal static void UpdateCardOpacity(float opacity, View view)
        {
            view.Opacity = opacity;
            //// Return if the view is not SfCardView.
            if (!(view is SfCardView))
            {
                return;
            }

            //// TODO: Could not map the opacity property in MAUI SfView. So we have handled the opacity property in the platform specific code.
#if WINDOWS
            if (view.Handler != null && view.Handler.PlatformView != null && view.Handler.PlatformView is Microsoft.Maui.Platform.LayoutPanel layoutPanel)
            {
                layoutPanel.Opacity = opacity;
            }
#elif ANDROID
            if (view.Handler != null && view.Handler.PlatformView != null && view.Handler.PlatformView is Microsoft.Maui.Platform.LayoutViewGroup layoutViewGroup)
            {
                layoutViewGroup.Alpha = opacity;
            }
#elif MACCATALYST || IOS
            if (view.Handler != null && view.Handler.PlatformView != null && view.Handler.PlatformView is Microsoft.Maui.Platform.LayoutView layoutView)
            {
                layoutView.Alpha = opacity;
            }
#endif
        }

        #endregion
    }
}