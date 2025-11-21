using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using System;
using System.Collections.Generic;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents the sunburst segment and it will be the binding context to the tooltip.
    /// </summary>
    public class SunburstSegment
    {
        #region Fields

        Brush? _fill;
        float _currentSegmentStartAngle;
        float _currentSegmentEndAngle;
        bool _isCircularBar;
        float _opacity = 1;

        object? _item;
        Brush? _strokeColor;
        double _strokeWidth;
        int _currentLevel, _index;
        bool _hasChild, _hasParent;
		RectF _currentBounds;
		RectF _actualBounds;
		RectF _segmentBounds;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SunburstSegment"/> class.
		/// </summary>
		public SunburstSegment()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the data object for the associated segment, which contains the category and value.
        /// </summary>
        public object? Item
        {
            get => _item;
            internal set => _item = value;
        }

        /// <summary>
        /// Gets the index which represents the current level in the hierarchy. 
        /// </summary>
        public int CurrentLevel
        {
            get => _currentLevel;
            internal set => _currentLevel = value;
        }

        /// <summary>
        /// Gets segment corresponding slice index. [Based on root category]
        /// </summary>
        public int Index
        {
            get => _index;
            internal set => _index = value;
        }

        /// <summary>
        ///  Gets the stroke for segment.
        /// </summary>
        public Brush? Stroke
        {
            get => _strokeColor;
            internal set => _strokeColor = value;
        }

        /// <summary>
        ///  Gets the brush value to customize the segment background.
        /// </summary>
        public Brush? Fill
        {
            get => _fill;
            internal set => _fill = value;
        }

        /// <summary>
        /// Gets the width for the segment stroke.
        /// </summary>
        public double StrokeWidth
        {
            get => _strokeWidth;
            internal set => _strokeWidth = value;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets a value indicating whether this segment has child or not.
        /// </summary>
        internal bool HasChild
        {
            get => _hasChild;
            set => _hasChild = value;
        }

        internal float Opacity
        {
            get => _opacity;
            set => _opacity = value;
        }

        /// <summary>
        /// Gets a value indicating whether this segment has parent or not.
        /// </summary>
        internal bool HasParent
        {
            get => _hasParent;
            set => _hasParent = value;
        }

        /// <summary>
        /// Gets a value indicating whether the segment has a visible stroke.
        /// </summary>
        internal bool HasStroke
        {
            get { return StrokeWidth > 0 && !IsEmpty(SunburstChartUtils.ToColor(Stroke)); }
        }

        /// <summary>
        /// Gets or sets the current segment's child's.
        /// </summary>
        internal List<SunburstSegment>? Childs { get; set; }

        /// <summary>
        /// Gets or sets the current segment's parent.
        /// </summary>
        internal SunburstSegment? Parent { get; set; }

        /// <summary>
        /// Gets or sets the current segment's item.
        /// </summary>
        internal SunburstItem? SunburstItems { get; set; }

        /// <summary>
        /// Gets or sets the start angle for drawing the segment.
        /// </summary>
        internal double ArcStartAngle { get; set; }

        /// <summary>
        /// Gets or sets the end angle for drawing the segment.
        /// </summary>
        internal double ArcEndAngle { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SfSunburstChart"/>.
        /// </summary>
        internal SfSunburstChart? Chart { get; set; }

        /// <summary>
        /// Gets or sets the inner radius of this segment.
        /// </summary>
        internal double InnerRadius { get; set; }

        /// <summary>
        /// Gets or sets the outer radius of this segment.
        /// </summary>
        internal double OuterRadius { get; set; }

        /// <summary>
        /// Gets the x value for the tooltip position.
        /// </summary>
        internal float TooltipXPosition { get; set; }

        /// <summary>
        /// Gets the y value for the tooltip position.
        /// </summary>
        internal float TooltipYPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this segment is selected.
        /// </summary>
        internal bool IsSelected { get; set; }

        #endregion

        #region Internal methods

        /// <summary>
        /// Draws the segment using the provided canvas context.
        /// </summary>
        /// <param name="canvas"></param>
        internal void Draw(ICanvas canvas)
        {
            if (Chart == null) return;

			canvas.Alpha = (float)Opacity;

			PathF pathF = new PathF();

            var segmentStartAngle = _currentSegmentStartAngle;
            var segmentEndAngle = _currentSegmentEndAngle;

            if (Chart.CanAnimate())
            {
                float animationValue = Chart.AnimationValue;

                segmentStartAngle = (float)(Chart.StartAngle + ((_currentSegmentStartAngle - Chart.StartAngle) * animationValue));
                segmentEndAngle = _currentSegmentEndAngle * animationValue;
            }

            pathF.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom,
                -segmentStartAngle, -segmentEndAngle, true);
            pathF.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom,
                -segmentEndAngle, -segmentStartAngle, false);
            pathF.Close();
            canvas.SetFillPaint(Fill, pathF.Bounds);
            canvas.FillPath(pathF);
            _segmentBounds = pathF.Bounds;

            if (HasStroke)
            {
                if (_isCircularBar)
                {
                    var outerPath = new PathF();
                    outerPath.AddArc(_actualBounds.Left, _actualBounds.Top, _actualBounds.Right, _actualBounds.Bottom,
                   -segmentStartAngle, -segmentEndAngle, true);
                    var innerPath = new PathF();
                    innerPath.AddArc(_currentBounds.Left, _currentBounds.Top, _currentBounds.Right, _currentBounds.Bottom,
                        -segmentEndAngle, -segmentStartAngle, false);
                    canvas.StrokeColor = SunburstChartUtils.ToColor(Stroke);
                    canvas.StrokeSize = (float)StrokeWidth;
                    canvas.DrawPath(innerPath);
                    canvas.DrawPath(outerPath);
                }
                else
                {
                    canvas.StrokeColor = SunburstChartUtils.ToColor(Stroke);
                    canvas.StrokeSize = (float)StrokeWidth;
                    canvas.DrawPath(pathF);
                }
            }
        }

        /// <summary>
		/// Updates the segment's bounds and layout-related properties.
		/// </summary>
        internal void OnLayout()
        {
            UpdateBounds();
        }

        /// <summary>
		/// Determines whether the specified point is within the segment's bounds.
		/// </summary>
		/// <param name="x">The x-coordinate of the point.</param>
		/// <param name="y">The y-coordinate of the point.</param>
		/// <returns><c>true</c> if the point is inside the segment; otherwise, <c>false</c>.</returns>
        internal bool IsPointInSunburstSegment(double x, double y)
        {
            if (Chart == null) return false;

            // Calculate point relative to chart center
            double centerX = (_currentBounds.Left + _currentBounds.Right) / 2;
            double centerY = (_currentBounds.Top + _currentBounds.Bottom) / 2;
            double dx = x - centerX;
            double dy = y - centerY;

            // Quick distance check before doing angle calculations
            double distanceSquare = (dx * dx) + (dy * dy);
            double innerRadiusSq = InnerRadius * InnerRadius;
            double outerRadiusSq = OuterRadius * OuterRadius;

            // Early exit if point is not within the ring's radial bounds
            if (distanceSquare < innerRadiusSq || distanceSquare > outerRadiusSq)
                return false;

            // Calculate point angle and normalize to [0, 360]
            double pointAngle = SunburstChartUtils.RadianToDegreeConverter(Math.Atan2(dy, dx));
            pointAngle = (pointAngle % 360 + 360) % 360; // Simpler normalization

            // Normalize segment angles to [0, 360] for consistent comparison
            double startAngle = (_currentSegmentStartAngle % 360 + 360) % 360;
            double endAngle = (_currentSegmentEndAngle % 360 + 360) % 360;

            // Handle the main logic cases more efficiently
            bool isClockwise = Chart.StartAngle < Chart.EndAngle;

            if (isClockwise)
            {
                if (endAngle > startAngle)
                {
                    // Simple case: point must be between start and end
                    return pointAngle >= startAngle && pointAngle <= endAngle;
                }
                else
                {
                    // Segment wraps around the 0/360 point
                    return pointAngle >= startAngle || pointAngle <= endAngle;
                }
            }
            else
            {
                if (startAngle > endAngle)
                {
                    // Simple case: point must be between end and start (in counter-clockwise direction)
                    return pointAngle >= endAngle && pointAngle <= startAngle;
                }
                else
                {
                    // Segment wraps around the 0/360 point
                    return pointAngle >= endAngle || pointAngle <= startAngle;
                }
            }
        }

        /// <summary>
		/// Calculates the maximum horizontal width available for text within the segment.
		/// </summary>
		/// <param name="text">The label text to measure.</param>
		/// <param name="element">The text style properties.</param>
		/// <returns>The available horizontal width in pixels.</returns>
        internal double GetSegmentHorizantalWidth(string text, ITextElement element)
        {
            double segmentWidth = _segmentBounds.Width;

			if (Chart != null)
			{
				var textWidth = text.Measure(element).Width + (HasStroke ? _strokeWidth * 2 : 4);

				var r = (OuterRadius + InnerRadius) / 2;
				var center = Chart.Center;
				double radian = (ArcStartAngle + ArcEndAngle) / 2;

				var x = (float)(center.X + r * Math.Cos(radian));
				var y = (float)(center.Y + r * Math.Sin(radian));

				var h_left = x - (textWidth / 2);
				var h_right = x + (textWidth / 2);

				// Walk towards the slice until both points are inside, but prevent endless looping
				int maxIteration = (int)textWidth; 

				// --- Right side scan (move left) ---
				int rightIteration = 0;
				while (!IsPointInSunburstSegment(h_right, y) && rightIteration < maxIteration)
				{
					h_right -= 1;
					rightIteration++;
				}
				bool rightConverged = rightIteration < maxIteration;

				// --- Left side scan (move right) ---
				int leftIteration = 0;
				while (!IsPointInSunburstSegment(h_left, y) && leftIteration < maxIteration)
				{
					h_left += 1;
					leftIteration++;
				}
				bool leftConverged = leftIteration < maxIteration;

				// If either side failed to converge, fall back to chord length approximation
				if (!leftConverged || !rightConverged)
				{
					double chord = 2 * r * Math.Sin(Math.Abs(ArcEndAngle - ArcStartAngle) / 2);
					return Math.Max(0, chord - (HasStroke ? _strokeWidth * 2 : 4));
				}

				// Otherwise compute the usable width
				segmentWidth = Math.Max(0, (h_right - h_left) - (HasStroke ? _strokeWidth * 2 : 4));

			}
			
			return segmentWidth;
        }

        #endregion

        #region Private methods

        /// <summary>
		/// Recalculates the inner and outer bounds of the segment based on chart properties.
		/// </summary>
        void UpdateBounds()
        {
            if (Chart != null)
            {
                var center = Chart.Center;

                double size = Chart.RingSize;

				double outerRadius = Math.Abs(Chart.OuterRadius - (size * (Chart.LevelsCount - (CurrentLevel + 1))));
                var innerRadius = outerRadius - size;

                // Subtract the stroke width value from the radius to crop the surface area.
                if (HasStroke)
                {
                    outerRadius -= StrokeWidth;
                    innerRadius -= StrokeWidth;
                }

                _actualBounds = new RectF(
                    (float)(center.X - (float)outerRadius / 2),
                    (float)(center.Y - (float)outerRadius / 2),
                    (float)outerRadius, (float)outerRadius);
                _currentBounds = new RectF(
                    (float)(center.X - (float)innerRadius / 2),
                    (float)(center.Y - (float)innerRadius / 2), 
                    (float)innerRadius, (float)innerRadius);

                OuterRadius = _actualBounds.Width / 2;
                InnerRadius = _currentBounds.Width / 2;

                _currentSegmentStartAngle = (float)SunburstChartUtils.RadianToDegreeConverter(ArcStartAngle);
                _currentSegmentEndAngle = (float)SunburstChartUtils.RadianToDegreeConverter(ArcEndAngle);

                _isCircularBar = Math.Round(_currentSegmentEndAngle - _currentSegmentStartAngle, 1).Equals(360);
            }
        }

        /// <summary>
		/// Determines whether the specified color is null or transparent.
		/// </summary>
		/// <param name="color">The color to check.</param>
		/// <returns><c>true</c> if the color is considered empty; otherwise, <c>false</c>.</returns>
		bool IsEmpty(Color? color)
        {
            return color == null || color == Colors.Transparent;
        }

        #endregion
    }
}
