using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace Syncfusion.Maui.Toolkit.Internals
{
    internal class WindowOverlayStack : FrameLayout
    {
		// To check whether the popup has Overlay Mode blur.
		internal bool HasBlurMode { get; set; }

        public WindowOverlayStack(Context context)
            : base(context)
        {
        }

        public WindowOverlayStack(Context context, IAttributeSet? attrs)
            : base(context, attrs)
        {
        }

        public WindowOverlayStack(Context context, IAttributeSet? attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public WindowOverlayStack(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected WindowOverlayStack(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }

}
