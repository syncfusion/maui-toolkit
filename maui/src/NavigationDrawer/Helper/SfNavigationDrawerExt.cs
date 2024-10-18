namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
    /// <summary>
    /// Helper class to override native methods.
    /// </summary>
    /// <exclude/>
    public abstract class SfNavigationDrawerExt : SfView
    {
        #region Android

#if ANDROID
        /// <summary>
        /// Intercepts touch events for <see cref="SfNavigationDrawerExt"/>.
        /// </summary>
        /// <param name="ev">MotionEvent arguments.</param>
        /// <returns>Returns true if the <see cref="SfNavigationDrawerExt"/> will handle the touch.</returns>
        internal virtual bool OnInterceptTouchEvent(Android.Views.MotionEvent? ev)
        {
            return false;
        }
#endif

        #endregion
    }
}
