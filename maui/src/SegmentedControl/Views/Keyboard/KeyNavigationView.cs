using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents a navigation view that draws a rounded rectangle border around the focused view.
	/// </summary>
	internal class KeyNavigationView : SfDrawableView
    {
        #region Fields

        /// <summary>
        /// The segment item info.
        /// </summary>
        ISegmentItemInfo? _itemInfo;

        /// <summary>
        /// The segment item associated with the focused view.
        /// </summary>
        SfSegmentItem? _segmentItem;

        /// <summary>
        /// The calculated bounds for the view to be focused on scrolling.
        /// </summary>
        Rect? _bounds;

        /// <summary>
        /// The default stroke thickness used for drawing the border.
        /// </summary>
        float _defaultStrokeThickness;

        /// <summary>
        /// Indicates whether the first view within the segment is currently focused.
        /// This is used to determine the scrolling behavior for navigation.
        /// </summary>
        bool _isFirstViewFocused;

        /// <summary>
        /// Indicates whether the last view within the segment is currently focused.
        /// This is used to determine the scrolling behavior for navigation.
        /// </summary>
        bool _isLastViewFocused;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyNavigationView"/> class.
        /// </summary>
        public KeyNavigationView(ISegmentItemInfo itemInfo)
        {
            _itemInfo = itemInfo;
            InputTransparent = true;
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Updates the navigation view with a new focused view and triggers a redraw.
        /// </summary>
        /// <param name="segmentItem">The focused segment item.</param>
        /// <param name="bounds">The calculated bounds for the view to be focused.</param>
        /// <param name="isFirstViewFocused">A flag indicating whether the new focused view is the first view within the segment.</param>
        /// <param name="isLastViewFocused">A flag indicating whether the new focused view is the last view within the segment.</param>
        internal void UpdateNavigationView(SfSegmentItem? segmentItem, Rect? bounds, bool isFirstViewFocused = false, bool isLastViewFocused = false)
        {
            _segmentItem = segmentItem;
            _bounds = bounds;
            _defaultStrokeThickness = 3;
            _isFirstViewFocused = isFirstViewFocused;
            _isLastViewFocused = isLastViewFocused;
            InvalidateDrawable();
        }

        /// <summary>
        /// Methods to clear the keyboard focused view.
        /// </summary>
        internal void ClearFocusedView()
        {
            _defaultStrokeThickness = 0;
            _segmentItem = null;
            InvalidateDrawable();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Calculates the keyboard outlined border with padding applied.
        /// </summary>
        /// <param name="dirtyRect">The dirty rect.</param>
        /// <returns>The draw rectangle with padding.</returns>
        RectF GetDrawRect(Rect dirtyRect)
        {
            if (_segmentItem == null || _bounds == null || _itemInfo == null)
            {
                return default;
            }

            int verticalPadding = 3;
            float strokeThickness = (float)_itemInfo.StrokeThickness;

            // Calculate the coordinates for the draw rectangle with the padding value.
            float left = (float)(_bounds.Value.Left + verticalPadding);

            // Consider segment control stroke thickness for drawing outlined focus border.
            float right = (float)_bounds.Value.Width + strokeThickness;
            float top = (float)dirtyRect.Top + verticalPadding;
            float bottom = (float)dirtyRect.Height - (2 * verticalPadding);

            // Adjust the left and right coordinates based on the focused view. This condition is used to avoid the stroke thickness for the first and last view.
            if (!_isFirstViewFocused && !_isLastViewFocused)
            {
                left = left - strokeThickness;
                right = right + strokeThickness;
            }

            // Adjust the left coordinate based on the focused view. This condition is used to avoid the stroke thickness for the last view.
            if (_isLastViewFocused)
            {
                left = left - strokeThickness;
            }

            return new RectF(left, top, right, bottom);
        }

        /// <summary>
        /// Adjusts the corner radii for a border based on segment item settings.
        /// </summary>
        /// <param name="cornerRadiusTopLeft">The top-left corner radius.</param>
        /// <param name="cornerRadiusTopRight">The top-right corner radius.</param>
        /// <param name="cornerRadiusBottomRight">The bottom-right corner radius.</param>
        /// <param name="cornerRadiusBottomLeft">The bottom-left corner radius.</param>
        void AdjustCornerRadiiForBorder(out float cornerRadiusTopLeft, out float cornerRadiusTopRight, out float cornerRadiusBottomRight, out float cornerRadiusBottomLeft)
        {
            cornerRadiusTopLeft = 0;
            cornerRadiusTopRight = 0;
            cornerRadiusBottomRight = 0;
            cornerRadiusBottomLeft = 0;

            // If the segment item or segment item info is null, there's nothing to adjust.
            if (_segmentItem == null || _itemInfo == null)
            {
                return;
            }

            ISegmentItemInfo itemInfo = _itemInfo;
            CornerRadius segmentCornerRadius = itemInfo.SegmentCornerRadius;
            CornerRadius cornerRadius = _segmentItem.CornerRadius;
            bool isDefaultValue = cornerRadius == default;

            // Determine the corner radii for each corner based on segment item settings.
            cornerRadiusTopLeft = (float)(isDefaultValue ? segmentCornerRadius.TopLeft : cornerRadius.TopLeft);
            cornerRadiusTopRight = (float)(isDefaultValue ? segmentCornerRadius.TopRight : cornerRadius.TopRight);
            cornerRadiusBottomLeft = (float)(isDefaultValue ? segmentCornerRadius.BottomLeft : cornerRadius.BottomLeft);
            cornerRadiusBottomRight = (float)(isDefaultValue ? segmentCornerRadius.BottomRight : cornerRadius.BottomRight);

            // When navigating the view using keyboard arrow keys within a scroll view, we must manually update the corner radius for the focused view among the visible first and last item views.
            cornerRadius = itemInfo.CornerRadius;
            cornerRadiusTopLeft = (float)(_isFirstViewFocused ? cornerRadius.TopLeft : cornerRadiusTopLeft);
            cornerRadiusTopRight = (float)(_isLastViewFocused ? cornerRadius.TopRight : cornerRadiusTopRight);
            cornerRadiusBottomLeft = (float)(_isFirstViewFocused ? cornerRadius.BottomLeft : cornerRadiusBottomLeft);
            cornerRadiusBottomRight = (float)(_isLastViewFocused ? cornerRadius.BottomRight : cornerRadiusBottomRight);
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Method to draw the rounded rectangle border around the focused view.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area to draw within.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.CanvasSaveState();
            canvas.Antialias = true;

            // Get the draw rectangle with padding.
            RectF clipRect = GetDrawRect(dirtyRect);

            // Set the stroke thickness for the border.
            canvas.StrokeSize = _defaultStrokeThickness;
            if (_itemInfo?.KeyboardFocusStroke != null)
            {
                canvas.StrokeColor = SegmentViewHelper.BrushToColorConverter(_itemInfo.KeyboardFocusStroke);
            }

            // Get the corner radius settings and dirty rect.
            float cornerRadiusTopLeft, cornerRadiusTopRight, cornerRadiusBottomRight, cornerRadiusBottomLeft;
            AdjustCornerRadiiForBorder(out cornerRadiusTopLeft, out cornerRadiusTopRight, out cornerRadiusBottomRight, out cornerRadiusBottomLeft);

            // Draw the rounded rectangle border around the dirty area.
            canvas.DrawRoundedRectangle(clipRect.Left, clipRect.Top, clipRect.Width, clipRect.Height, cornerRadiusTopLeft, cornerRadiusTopRight, cornerRadiusBottomLeft, cornerRadiusBottomRight);
            canvas.CanvasRestoreState();
        }

        #endregion
    }
}