#if ANDROID
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using AndroidX.Core.Widget;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Platform;
using View = Android.Views.View;
using ScrollView = Android.Widget.ScrollView;

namespace Syncfusion.Maui.Toolkit.BottomSheet
{
	/// <summary>
	/// Android container that lets inner views scroll normally and, at their edges, intercepts to hand off vertical gestures to the bottom sheet for smooth dragging.
	/// </summary>
	internal class BottomSheetBorderPlatformView : ContentViewGroup
	{
		#region Fields

		/// <summary>
		/// Weak reference to the owning <see cref="BottomSheetBorder"/> used to forward pointer actions to the bottom sheet.
		/// </summary>
		readonly WeakReference<BottomSheetBorder> _borderRef;

		/// <summary>
		/// The scrollable child view currently under the pointer, if any, used to decide when to hand off at edges.
		/// </summary>
		View? _scrollableUnderFinger;

		/// <summary>
		/// Indicates whether the gesture started inside a scrollable child.
		/// </summary>
		bool _insideScrollable;

		/// <summary>
		/// Last observed Y position (in pixels) to compute vertical deltas during MOVE.
		/// </summary>
		float _lastY;

		/// <summary>
		/// Last observed X position (in pixels) to compute horizontal deltas and filter horizontal swipes.
		/// </summary>
		float _lastX;

		/// <summary>
		/// Timestamp of the ACTION_DOWN for the current gesture (in uptime millis), used for gesture bookkeeping.
		/// </summary>
		long _downTime;

		/// <summary>
		/// System-defined movement threshold used to ignore tiny jitters.
		/// </summary>
		readonly int _touchSlop;

		#endregion

		#region Constructor
		public BottomSheetBorderPlatformView(Context context, BottomSheetBorder border) : base(context)
		{
			_borderRef = new(border);
			SetClipChildren(true);
			if(ViewConfiguration.Get(context) is ViewConfiguration vc)
			{
				_touchSlop = vc.ScaledTouchSlop;
			}

			Clickable = true; // needed to get touch events
			Focusable = true;
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Determines whether to intercept touch events for the bottom sheet.
		/// </summary>
		/// <param name="ev">The motion event.</param>
		/// <returns><c>true</c> if the touch event should be intercepted; otherwise, <c>false</c>.</returns>
		public override bool OnInterceptTouchEvent(MotionEvent? ev)
		{
			if (ev is not null)
			{
				switch (ev.ActionMasked)
				{
					case MotionEventActions.Down:
						_downTime = ev.DownTime;
						_lastX = ev.GetX();
						_lastY = ev.GetY();
						_scrollableUnderFinger = FindScrollableUnder(this, (int)ev.GetX(), (int)ev.GetY());
						_insideScrollable = _scrollableUnderFinger is not null;
						// Let the child try first.
						DisallowParentIntercept(true);
						return false;

					case MotionEventActions.Move:
						if (_insideScrollable && _scrollableUnderFinger is not null)
						{
							float curX = ev.GetX();
							float curY = ev.GetY();
							float dx = curX - _lastX;
							float dy = curY - _lastY;

							// Ignore tiny jitters
							if (Math.Abs(dx) < _touchSlop && Math.Abs(dy) < _touchSlop)
							{
								return false;
							}

							// Ignore mostly-horizontal swipes
							if (Math.Abs(dx) > Math.Abs(dy) * 1.2f)
							{
								_lastX = curX;
								_lastY = curY;
								return false;
							}

							// Map finger movement to content direction for CanScrollVertically:
							// dy > 0 (finger down)  -> child must be able to scroll UP -> dir = -1
							// dy < 0 (finger up)    -> child must be able to scroll DOWN -> dir = +1
							int dir = dy > 0 ? -1 : 1;

							if (CanChildScrollVertically(_scrollableUnderFinger, dir))
							{
								// Child can still scroll in this direction -> let it handle
								DisallowParentIntercept(true);
								_lastX = curX;
								_lastY = curY;
								return false;
							}

							// === EDGE REACHED ===
							// We take over; keep ancestors from intercepting
							DisallowParentIntercept(true);

							// Stop nested scrolling and any fling on the child
							ViewCompat.StopNestedScroll(_scrollableUnderFinger);
							if (_scrollableUnderFinger is RecyclerView rv)
							{
								rv.StopScroll();
							}

							if (_scrollableUnderFinger is NestedScrollView nsv)
							{
								nsv.StopNestedScroll();
							}

							// Immediately deliver Pressed + this Move to the bottom sheet
							if (_borderRef.TryGetTarget(out var bottomSheet))
							{
								float density = Resources?.DisplayMetrics?.Density ?? 1f;
								var point = new Microsoft.Maui.Graphics.Point(ev.GetX() / density, ev.GetY() / density);

								// Synthesize a Pressed so the sheet has a valid gesture stream
								bottomSheet.ForwardToSheet(Syncfusion.Maui.Toolkit.Internals.PointerActions.Pressed, point);
								// And forward the current Move
								bottomSheet.ForwardToSheet(Syncfusion.Maui.Toolkit.Internals.PointerActions.Moved, point);
							}

							_lastX = curX;
							_lastY = curY;
							return true; // BottomSheet now takes over the touch
						}
						break;

					case MotionEventActions.Up:
					case MotionEventActions.Cancel:
						ResetGestureState();
						break;
				}
			}

			return base.OnInterceptTouchEvent(ev);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Requests that ancestor views do or do not intercept this touch event sequence.
		/// </summary>
		/// <param name="disallow">True to prevent ancestors from intercepting; false to allow.</param>
		void DisallowParentIntercept(bool disallow)
		{
			var p = Parent;
			while (p is not null)
			{
				p.RequestDisallowInterceptTouchEvent(disallow);
				p = p.Parent;
			}
		}

		/// <summary>
		/// Finds the deepest scrollable child under the given point within the specified root.
		/// </summary>
		/// <param name="root">The root view group to search within.</param>
		/// <param name="x">X coordinate in the root's coordinate space.</param>
		/// <param name="y">Y coordinate in the root's coordinate space.</param>
		/// <returns>The scrollable view under the point if found; otherwise, null.</returns>
		View? FindScrollableUnder(ViewGroup root, int x, int y)
		{
			// x,y are in the coordinate space of 'root'
			for (int i = root.ChildCount - 1; i >= 0; i--)
			{
				var child = root.GetChildAt(i);
				if (child is null)
				{
					continue;
				}

				int left = child.Left;
				int top = child.Top;
				int right = child.Right;
				int bottom = child.Bottom;

				if (x < left || x > right || y < top || y > bottom)
				{
					continue;
				}

				// If the child itself is scrollable, prefer it
				if (IsScrollable(child))
				{
					return child;
				}

				// Otherwise, search deeper with coordinates translated into the child's space
				if (child is ViewGroup vg)
				{
					var t = FindScrollableUnder(vg, x - left, y - top);
					if (t is not null)
					{
						return t;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Determines whether the provided view can scroll further vertically in the specified direction.
		/// </summary>
		/// <param name="v">The child view.</param>
		/// <param name="dir">The scroll direction.</param>
		/// <returns>True if the view can scroll further in that direction; otherwise, false.</returns>
		bool CanChildScrollVertically(View v, int dir)
		{
			// Positive dir => check if can scroll further DOWN
			// Negative dir => check if can scroll further UP

			if (v is ScrollView sv)
			{
				var child = sv.ChildCount > 0 ? sv.GetChildAt(0) : null;
				if (child is null)
				{
					return false;
				}

				int viewport = sv.Height - sv.PaddingTop - sv.PaddingBottom;
				int range = Math.Max(0, child.MeasuredHeight - viewport);

				return dir > 0 ? sv.ScrollY < range : sv.ScrollY > 0;
			}

			if (v is AndroidX.Core.Widget.NestedScrollView nsv)
			{
				var child = nsv.ChildCount > 0 ? nsv.GetChildAt(0) : null;
				if (child is null)
				{
					return false;
				}

				int viewport = nsv.Height - nsv.PaddingTop - nsv.PaddingBottom;
				int range = Math.Max(0, child.MeasuredHeight - viewport);

				return dir > 0 ? nsv.ScrollY < range : nsv.ScrollY > 0;
			}

			if (v is RecyclerView rv)
			{
				int offset = rv.ComputeVerticalScrollOffset();
				int extent = rv.ComputeVerticalScrollExtent();
				int range = rv.ComputeVerticalScrollRange();
				int maxOffset = Math.Max(0, range - extent);

				return dir > 0 ? offset < maxOffset : offset > 0;
			}

			// Fallback for other views
			return v.CanScrollVertically(dir);
		}

		/// <summary>
		/// Returns whether the provided view is a known scrollable or currently scrollable vertically.
		/// </summary>
		/// <param name="v">The view to evaluate.</param>
		/// <returns>True if the view is scrollable; otherwise, false.</returns>
		bool IsScrollable(View v)
		{
			return v is RecyclerView
				|| v is ScrollView
				|| v is AbsListView
				|| v is NestedScrollView
				|| v.CanScrollVertically(1)
				|| v.CanScrollVertically(-1);
		}

		/// <summary>
		/// Resets per-gesture state after the touch sequence ends or is cancelled.
		/// </summary>
		void ResetGestureState()
		{
			_insideScrollable = false;
			_scrollableUnderFinger = null;
		}

		#endregion
	}
}
#endif
