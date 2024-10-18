using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit
{
    internal class DrawableLayoutManager : AbsoluteLayoutManager
    {
        internal new IAbsoluteLayout Layout;

        const double AutoSize = -1;

        MethodInfo? methodInfos;

        object[]? parameters;

        public DrawableLayoutManager(IAbsoluteLayout layout) : base(layout)
        {
            Layout = layout;
            methodInfos = typeof(LayoutExtensions).GetMethod("ShouldArrangeLeftToRight", BindingFlags.Static | BindingFlags.Public);
            parameters = new object[] { Layout };
        }

        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            var padding = Layout.Padding;

            var availableWidth = widthConstraint - padding.HorizontalThickness;
            var availableHeight = heightConstraint - padding.VerticalThickness;

            double measuredHeight = 0;
            double measuredWidth = 0;

            if (Layout.Count > 0)
            {
                for (int n = 0; n < Layout.Count; n++)
                {
                    var child = Layout[n];

                    if (child.Visibility == Visibility.Collapsed)
                    {
                        continue;
                    }

                    var bounds = Layout.GetLayoutBounds(child);
                    var flags = Layout.GetLayoutFlags(child);
                    bool isWidthProportional = HasFlag(flags, AbsoluteLayoutFlags.WidthProportional);
                    bool isHeightProportional = HasFlag(flags, AbsoluteLayoutFlags.HeightProportional);

                    if (bounds.Width > 0 && bounds.Height > 0)
                    {
                        var measureWidth = ResolveChildMeasureConstraint(bounds.Width, isWidthProportional, widthConstraint);
                        var measureHeight = ResolveChildMeasureConstraint(bounds.Height, isHeightProportional, heightConstraint);

                        var measure = child.Measure(measureWidth, measureHeight);

                        var width = ResolveDimension(isWidthProportional, bounds.Width, availableWidth, measure.Width);
                        var height = ResolveDimension(isHeightProportional, bounds.Height, availableHeight, measure.Height);

                        measuredHeight = Math.Max(measuredHeight, bounds.Top + height);
                        measuredWidth = Math.Max(measuredWidth, bounds.Left + width);
                    }
                    else
                    {
                        var measure = child.Measure(availableWidth, availableHeight);
                        measuredHeight = measure.Height;
                        measuredWidth = measure.Width;
                    }
                }
            }
            else
            {
                if (heightConstraint > 0 && heightConstraint != double.PositiveInfinity && heightConstraint != double.NegativeInfinity && heightConstraint != double.NaN)
                {
                    measuredHeight = heightConstraint;
                }
                if (widthConstraint > 0 && widthConstraint != double.PositiveInfinity && widthConstraint != double.NegativeInfinity && widthConstraint != double.NaN)
                {
                    measuredWidth = widthConstraint;
                }
            }

            var finalHeight = ResolveConstraints(heightConstraint, Layout.Height, measuredHeight, Layout.MinimumHeight, Layout.MaximumHeight);
            var finalWidth = ResolveConstraints(widthConstraint, Layout.Width, measuredWidth, Layout.MinimumWidth, Layout.MaximumWidth);

            return new Size(finalWidth, finalHeight);
        }

        public override Size ArrangeChildren(Rect bounds)
        {
            var padding = Layout.Padding;

            double top = padding.Top + bounds.Top;
            double left = padding.Left + bounds.Left;
            double right = bounds.Right - padding.Right;
            double availableWidth = bounds.Width - padding.HorizontalThickness;
            double availableHeight = bounds.Height - padding.VerticalThickness;

            bool isLeftToRight = true;
            if (methodInfos != null)
            {
                var returnValue = methodInfos.Invoke(this, parameters);
                if (returnValue is bool leftToRight)
                {
                    isLeftToRight = leftToRight;
                }
            }
            for (int n = 0; n < Layout.Count; n++)
            {
                var child = Layout[n];

                if (child.Visibility == Visibility.Collapsed)
                {
                    continue;
                }

                var destination = Layout.GetLayoutBounds(child);
                if (destination.Width > 0 && destination.Height > 0)
                {
                    var flags = Layout.GetLayoutFlags(child);

                    bool isWidthProportional = HasFlag(flags, AbsoluteLayoutFlags.WidthProportional);
                    bool isHeightProportional = HasFlag(flags, AbsoluteLayoutFlags.HeightProportional);


                    destination.Width = ResolveDimension(isWidthProportional, destination.Width, availableWidth, child.DesiredSize.Width);
                    destination.Height = ResolveDimension(isHeightProportional, destination.Height, availableHeight, child.DesiredSize.Height);

                    if (HasFlag(flags, AbsoluteLayoutFlags.XProportional))
                    {
                        destination.X = (availableWidth - destination.Width) * destination.X;
                    }

                    if (HasFlag(flags, AbsoluteLayoutFlags.YProportional))
                    {
                        destination.Y = (availableHeight - destination.Height) * destination.Y;
                    }
                }
                else
                {
                    destination = new Rect(0, 0, availableWidth, availableHeight);
                }

                if (isLeftToRight)
                    destination.X += left;
                else
                    destination.X = right - destination.X - destination.Width;

                destination.Y += top;
                child.Arrange(destination);
            }

            return new Size(availableWidth, availableHeight);
        }

        static bool HasFlag(AbsoluteLayoutFlags a, AbsoluteLayoutFlags b)
        {
            // Avoiding Enum.HasFlag here for performance reasons; we don't need the type check
            return (a & b) == b;
        }

        static double ResolveDimension(bool isProportional, double fromBounds, double available, double measured)
        {
            // By default, we use the absolute value from LayoutBounds
            var value = fromBounds;

            if (isProportional && !double.IsInfinity(available))
            {
                // If this dimension is marked proportional, then the value is a percentage of the available space
                // Multiple it by the available space to figure out the final value
                value *= available;
            }
            else if (value == AutoSize)
            {
                // No absolute or proportional value specified, so we use the measured value
                value = measured;
            }

            return value;
        }

        static double ResolveChildMeasureConstraint(double boundsValue, bool proportional, double constraint)
        {
            if (boundsValue < 0)
            {
                // If the child view doesn't have bounds set by the AbsoluteLayout, then we'll let it auto-size
                return double.PositiveInfinity;
            }

            if (proportional)
            {
                return boundsValue * constraint;
            }

            return boundsValue;
        }

    }
}
