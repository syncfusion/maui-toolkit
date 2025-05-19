using Android.Content;
using Android.OS;
using Android.Views;
using Java.Lang;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// The ScrollViewHandler for <see cref="SfPickerView"/>.
    /// </summary>
    internal partial class SfPickerScrollViewHandler : ScrollViewHandler
    {
        #region Override Method

        /// <summary>
        /// Creates native scroll view.
        /// </summary>
        /// <returns>Instance of native scroll view.</returns>
        protected override MauiScrollView CreatePlatformView()
        {
            NativeCustomScrolLayout nativeScrollView = new NativeCustomScrolLayout(Context, VirtualView);
            // Set to avoid overlapping control issue when scrolling.
            nativeScrollView.ClipToOutline = true;
            return nativeScrollView;
        }

        #endregion
    }

    /// <summary>
    /// Represents a class which contains the information of native scroll view.
    /// </summary>
    internal class NativeCustomScrolLayout : MauiScrollView
    {
        #region Fields

        /// <summary>
        /// A handler used to send and process message and runnable objects associated with a thread's message queue.
        /// </summary>
        Handler? _handler;

        /// <summary>
        /// The runnable instance.
        /// </summary>
        Runnable? _runnable;

        /// <summary>
        /// The pickerView instance.
        /// </summary>
        readonly SfPickerView? _pickerView;

        /// <summary>
        /// The previous scroll y position default value is 0. This value is used to check whether the current scroll position is end scroll position or not while the motion event status up or cancel.
        /// </summary>
        float _previousScrollY = 0;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NativeCustomScrolLayout" /> class.
        /// </summary>
        /// <param name="context">The <see cref="Context"/>.</param>
        /// <param name="scrollView">The parent scroll view.</param>
        internal NativeCustomScrolLayout(Context context, IScrollView scrollView) : base(context)
        {
            SfPickerView? scrollLayout = scrollView as SfPickerView;
            _pickerView = scrollLayout;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to call while runnable is running.
        /// </summary>
        void Run()
        {
            //// Check previous scroll position and the current scroll position is same or not. If it is same then it is end scroll position.
            if (_previousScrollY - ScrollY == 0)
            {
                _pickerView?.OnPickerViewScrollStart();
                _pickerView?.OnPickerViewScrollEnd(GetEndScrollPosition());
            }
            else
            {
                _previousScrollY = ScrollY;
                if (_runnable != null)
                {
                    _handler?.PostDelayed(_runnable, 150);
                }
            }
        }

        /// <summary>
        /// Method to get the end scroll position based on the pixels.
        /// </summary>
        /// <returns></returns>
        int GetEndScrollPosition()
        {
            Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
            double endScrollPosition = fromPixels(ScrollY);
            return (int)endScrollPosition;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to handle the on touch event.
        /// </summary>
        /// <param name="action">The motion event.</param>
        /// <returns></returns>
        public override bool OnTouchEvent(MotionEvent? action)
        {
            if (action == null)
            {
                return base.OnTouchEvent(action);
            }

            if (action.Action == MotionEventActions.Move)
            {
                _previousScrollY = ScrollY;
                _pickerView?.OnPickerViewScrollStart();
            }
            else if (action.Action == MotionEventActions.Up || action.Action == MotionEventActions.Cancel)
            {
                //// Check previous scroll position and the current scroll position is same or not. If it is same then it is end scroll position.
                if (_previousScrollY == ScrollY)
                {
                    _pickerView?.OnPickerViewScrollStart();
                    _pickerView?.OnPickerViewScrollEnd(GetEndScrollPosition());
                    if (_handler != null)
                    {
                        if (_runnable != null)
                        {
                            _handler.RemoveCallbacks(_runnable);
                        }

                        _handler.Dispose();
                        _handler = null;
                    }

                    if (_runnable != null)
                    {
                        _runnable.Dispose();
                        _runnable = null;
                    }

                    return base.OnTouchEvent(action);
                }

                _previousScrollY = ScrollY;
                _runnable = new Runnable(Run);
                _handler = new Handler(Looper.MainLooper!);
                _handler?.PostDelayed(_runnable, 200);
            }

            return base.OnTouchEvent(action);
        }

        /// <summary>
        /// Return false to skip the motion events(mouse wheel scroll) for scroll view.
        /// </summary>
        /// <param name="e">Motion event.</param>
        /// <returns>Return false to skip the mouse wheel scroll.</returns>
        public override bool OnGenericMotionEvent(MotionEvent? e)
        {
            return false;
        }

        /// <summary>
        /// Method to use to dispose the handler and runnable instance.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_handler != null)
                {
                    if (_runnable != null)
                    {
                        _handler.RemoveCallbacks(_runnable);
                    }

                    _handler.Dispose();
                    _handler = null;
                }

                if (_runnable != null)
                {
                    _runnable.Dispose();
                    _runnable = null;
                }                
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}