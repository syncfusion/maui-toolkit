using Microsoft.Maui.Handlers;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// The ScrollViewHandler for <see cref="SfPickerView"/>.
    /// </summary>
    internal partial class SfPickerScrollViewHandler : ScrollViewHandler
    {
        #region Fields

        /// <summary>
        /// The proxy object for handling picker scroll view events.
        /// </summary>
        PickerScrollViewProxy? _proxy;

        #endregion

        #region Override Methods

        /// <summary>
        /// Connects the handler.
        /// </summary>
        /// <param name="platformView">Instance of platformView.</param>
        protected override void ConnectHandler(UIScrollView platformView)
        {
            if (_proxy == null && VirtualView is SfPickerView view)
            {
                _proxy = new PickerScrollViewProxy(view);
            }

            platformView.WillEndDragging += _proxy!.OnPlatformViewEndDragging;

            base.ConnectHandler(platformView);
        }

        /// <summary>
        /// Disconnects the handler.
        /// </summary>
        /// <param name="platformView">Instance of platform view.</param>
        protected override void DisconnectHandler(UIScrollView platformView)
        {
            if (_proxy != null)
            {
                platformView.WillEndDragging -= _proxy.OnPlatformViewEndDragging;
            }

            base.DisconnectHandler(platformView);
        }

        #endregion
    }

    /// <summary>
    /// Proxy class to handle scroll view events for the picker.
    /// </summary>
    internal class PickerScrollViewProxy
    {
        #region Fields

        /// <summary>
        /// A weak reference to the associated SfPickerView instance.
        /// </summary>
        readonly WeakReference<SfPickerView> _view;

        #endregion

        #region Internal Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerScrollViewProxy"/> class.
        /// </summary>
        /// <param name="view">The SfPickerView instance to associate with this proxy.</param>
        internal PickerScrollViewProxy(SfPickerView view) => _view = new(view);

        /// <summary>
        /// Handles the end of dragging event for the platform view.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event arguments containing dragging information.</param>
        internal void OnPlatformViewEndDragging(object? sender, WillEndDraggingEventArgs e)
        {
            _view.TryGetTarget(out var view);
            SfPickerView? scrollLayout = view;
            scrollLayout?.OnPickerViewScrollEnd((int)e.TargetContentOffset.Y);
        }

        #endregion
    }
}