using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal abstract class SfTabViewExt : SfContentView
    {
        #region Android

#if ANDROID
        /// <summary>
        /// This method will helps to intercept touch of <see cref="SfTabViewExt"/>.
        /// </summary>
        /// <param name="ev">MotionEvent arguments.</param>
        /// <returns>Returns true, if <see cref="SfTabViewExt"/> will handle the touch.</returns>
        internal virtual bool OnInterceptTouchEvent(Android.Views.MotionEvent? ev)
        {
            return false;
        }
#endif

        #endregion
    }
}
