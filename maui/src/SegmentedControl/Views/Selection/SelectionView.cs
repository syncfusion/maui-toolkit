using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents a view is used to customize the appearance of a segment item based on different selection options, including border selection, top border selection, and bottom border selection.
	/// </summary>
	internal class SelectionView : SfDrawableView
    {
        #region Fields

        /// <summary>
        /// The segment item info.
        /// </summary>
        ISegmentItemInfo? _itemInfo;

        /// <summary>
        /// The segment item.
        /// </summary>
        SfSegmentItem? _segmentItem;

        /// <summary>
        /// Gets or sets a value indicating whether the segment item is currently selected.
        /// </summary>
        bool _isSelected;

        /// <summary>
        /// Gets or sets a value indicating whether the mouse pointer is currently over the segment item.
        /// </summary>
        bool _isMouseOver;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionView"/> class.
        /// </summary>
        /// <param name="itemInfo">The segment item info.</param>
        /// <param name="item">The segment item view to associate with this selection view.</param>
        internal SelectionView(SfSegmentItem item, ISegmentItemInfo itemInfo)
        {
            _segmentItem = item;
            _itemInfo = itemInfo;
            InputTransparent = true;
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Method to update the visual state of the selection view.
        /// </summary>
        /// <param name="isSelected">Whether the segment is selected.</param>
        /// <param name="isMouseOver">Whether the mouse is over the segment.</param>
        internal void UpdateVisualState(bool isSelected, bool isMouseOver = false)
        {
            _isSelected = isSelected;
            _isMouseOver = isMouseOver;
            InvalidateDrawable();
        }

        /// <summary>
        /// Method to dispose the selection view's instance.
        /// </summary>
        internal void Dispose()
        {
            if (_itemInfo != null)
            {
                _itemInfo = null;
            }

            if (_segmentItem != null)
            {
                _segmentItem = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method to draw a rounded rectangle with a border.
        /// </summary>
        /// <param name="canvas">The canvas used for drawing.</param>
        /// <param name="dirtyRect">The area that needs to be redrawn.</param>
        void DrawRoundedRectangle(ICanvas canvas, RectF dirtyRect)
        {
            if (_itemInfo == null)
            {
                return;
            }

            canvas.CanvasSaveState();
            canvas.Antialias = true;

            // Get the stroke thickness and background color.
            float selectionStrokeThickness = (float)_itemInfo.SelectionIndicatorSettings.StrokeThickness;
            canvas.StrokeSize = selectionStrokeThickness;
            canvas.StrokeColor = SegmentViewHelper.BrushToColorConverter(GetStrokeValue());
            CornerRadius selectionCornerRadius = _itemInfo.SegmentCornerRadius;
            CornerRadius segmentCornerRadius = _segmentItem?.CornerRadius ?? default;
            float strokeRadius = selectionStrokeThickness / 2f;
            bool isDefaultValue = segmentCornerRadius == default;
            float cornerRadiusTopLeft = (float)(isDefaultValue ? selectionCornerRadius.TopLeft : segmentCornerRadius.TopLeft) - strokeRadius;
            float cornerRadiusTopRight = (float)(isDefaultValue ? selectionCornerRadius.TopRight : segmentCornerRadius.TopRight) - strokeRadius;
            float cornerRadiusBottomRight = (float)(isDefaultValue ? selectionCornerRadius.BottomRight : segmentCornerRadius.BottomRight) - strokeRadius;
            float cornerRadiusBottomLeft = (float)(isDefaultValue ? selectionCornerRadius.BottomLeft : segmentCornerRadius.BottomLeft) - strokeRadius;

            // Calculate the adjusted rectangle dimensions.
            RectF rect = new RectF(dirtyRect.Left, dirtyRect.Top, dirtyRect.Right, dirtyRect.Bottom);
            rect.Left = GetLeftBorderPosition(dirtyRect, selectionStrokeThickness);
            rect.Top = GetTopBorderPosition(dirtyRect, selectionStrokeThickness);
            rect.Right = GetRightBorderPosition(dirtyRect, strokeRadius);
            rect.Bottom = GetBottomBorderPosition(dirtyRect, strokeRadius);

            // Draw the rounded rectangle border.
            canvas.DrawRoundedRectangle(rect.Left, rect.Top, rect.Width, rect.Height, cornerRadiusTopLeft, cornerRadiusTopRight, cornerRadiusBottomLeft, cornerRadiusBottomRight);
            canvas.CanvasRestoreState();
        }

        /// <summary>
        /// Method to draw the top border of the segment.
        /// </summary>
        /// <param name="canvas">The canvas used for drawing.</param>
        /// <param name="dirtyRect">The area that needs to be redrawn.</param>
        void DrawTopBorder(ICanvas canvas, RectF dirtyRect)
        {
            if (_itemInfo == null)
            {
                return;
            }

            canvas.CanvasSaveState();
            canvas.Antialias = true;

            // Get the stroke thickness and background color.
            float strokeThickness = (float)_itemInfo.StrokeThickness;
            float selectionStrokeThickness = (float)_itemInfo.SelectionIndicatorSettings.StrokeThickness;

            // Get the background color based on selection and mouse-over states.
            Brush mouseOverBackground = GetStrokeValue();

            // As the top border is drawn on the top of the segment, the stroke thickness should be added to the top of the dirty rect.
            canvas.StrokeSize = selectionStrokeThickness;
            canvas.StrokeColor = SegmentViewHelper.BrushToColorConverter(mouseOverBackground);

            // The position for drawing a top border on a canvas.
            float topBorderYPosition = dirtyRect.Top + (selectionStrokeThickness / 2);

            // Draw the top border.
            canvas.DrawLine(dirtyRect.Left, topBorderYPosition, dirtyRect.Width, topBorderYPosition);
            canvas.CanvasRestoreState();
        }

        /// <summary>
        /// Method to draw the bottom border of the segment.
        /// </summary>
        /// <param name="canvas">The canvas used for drawing.</param>
        /// <param name="dirtyRect">The area that needs to be redrawn.</param>
        void DrawBottomBorder(ICanvas canvas, RectF dirtyRect)
        {
            if (_itemInfo == null)
            {
                return;
            }

            canvas.CanvasSaveState();
            canvas.Antialias = true;

            // Get the stroke thickness and background color.
            float selectionStrokeThickness = (float)_itemInfo.SelectionIndicatorSettings.StrokeThickness;
            canvas.StrokeSize = selectionStrokeThickness;

            // Set the background color based on selection and mouse-over states.
            canvas.StrokeColor = SegmentViewHelper.BrushToColorConverter(GetStrokeValue());

            // The position for drawing a bottom border on a canvas.
            float bottomBorderYPosition = dirtyRect.Height - (selectionStrokeThickness / 2);

            // Draw the bottom border.
            canvas.DrawLine(dirtyRect.Left, bottomBorderYPosition, dirtyRect.Width, bottomBorderYPosition);
            canvas.CanvasRestoreState();
        }

        /// <summary>
        /// Gets the stroke brush value for the segment item based on its selection state and mouse hover state.
        /// </summary>
        /// <returns>The stroke brush for the segment item.</returns>
        Brush GetStrokeValue()
        {
            Brush background = Brush.Transparent;
            if (_itemInfo == null || _segmentItem == null)
            {
                return background;
            }

            if (_isSelected)
            {
                background = _isMouseOver ? SegmentViewHelper.GetSelectedSegmentHoveredStroke(_itemInfo, _segmentItem)
                    : SegmentViewHelper.GetSelectedSegmentStroke(_itemInfo, _segmentItem);
            }
            else if (_isMouseOver)
            {
                background = SegmentViewHelper.GetHoveredSegmentBackground(_itemInfo);
            }

            return background;
        }

        /// <summary>
        /// Calculates the position for drawing the top border on a canvas.
        /// </summary>
        /// <param name="dirtyRect">The dirty rect.</param>
        /// <param name="strokeThickness">The thickness of the border stroke.</param>
        /// <returns>The calculated top position.</returns>
        float GetTopBorderPosition(RectF dirtyRect, float strokeThickness)
        {
            return dirtyRect.Top + strokeThickness / 2;
        }

        /// <summary>
        /// Calculates the position for drawing the bottom border on a canvas.
        /// </summary>
        /// <param name="dirtyRect">The dirty rect.</param>
        /// <param name="strokeThickness">The thickness of the border stroke.</param>
        /// <returns>The calculated bottom position.</returns>
        float GetBottomBorderPosition(RectF dirtyRect, float strokeThickness)
        {
            // The position for drawing a bottom border on a canvas.
            return dirtyRect.Height - strokeThickness;
        }

        /// <summary>
        /// Calculates the position for drawing the left border on a canvas.
        /// </summary>
        /// <param name="dirtyRect">The dirty rect.</param>
        /// <param name="strokeThickness">The thickness of the border stroke.</param>
        /// <returns>The calculated left position.</returns>
        float GetLeftBorderPosition(RectF dirtyRect, float strokeThickness)
        {
            // The position for drawing a left border on a canvas.
            return dirtyRect.Left + strokeThickness / 2;
        }

        /// <summary>
        /// Calculates the position for drawing the right border on a canvas.
        /// </summary>
        /// <param name="dirtyRect">The dirty rect.</param>
        /// <param name="strokeThickness">The thickness of the border stroke.</param>
        /// <returns>The calculated right position.</returns>
        float GetRightBorderPosition(RectF dirtyRect, float strokeThickness)
        {
            // The position for drawing a right border on a canvas.
            return dirtyRect.Width - strokeThickness;
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Override OnDraw method to perform custom drawing based on segment selection options.
        /// </summary>
        /// <param name="canvas">The canvas used for drawing.</param>
        /// <param name="dirtyRect">The area that needs to be redrawn.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            if (_itemInfo == null)
            {
                return;
            }

            // Perform drawing based on the selection option.
            var segmentOption = _itemInfo.SelectionIndicatorSettings.SelectionIndicatorPlacement;
            switch (segmentOption)
            {
                case SelectionIndicatorPlacement.Border:
                    DrawRoundedRectangle(canvas, dirtyRect);
                    break;

                case SelectionIndicatorPlacement.TopBorder:
                    DrawTopBorder(canvas, dirtyRect);
                    break;

                case SelectionIndicatorPlacement.BottomBorder:
                    DrawBottomBorder(canvas, dirtyRect);
                    break;
            }

            base.OnDraw(canvas, dirtyRect);
        }

        #endregion
    }
}