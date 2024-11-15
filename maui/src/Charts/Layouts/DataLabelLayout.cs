using Microsoft.Maui.Layouts;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal class DataLabelLayout : Layout
    {
        protected override ILayoutManager CreateLayoutManager()
        {
            return new TemplatedViewLayoutManager(this);
        }

        internal IDataTemplateDependent source;

        internal bool IsTemplateItemsChanged()
        {
            return source.IsTemplateItemsChanged();
        }

        public DataLabelLayout(IDataTemplateDependent source)
        {
            IsClippedToBounds = true;
            this.source = source;
            BindableLayout.SetItemTemplate(this, GetItemTemplate());
            SetBinding(IsVisibleProperty, new Binding(nameof(IDataTemplateDependent.IsVisible)) { Source = this.source });
        }

        DataTemplate GetItemTemplate()
        {
            return new DataTemplate(() =>
            {
                var item = new DataLabelItemView();
                var binding = new MultiBinding
                {
                    Bindings = new Collection<BindingBase>
                    {
                        new Binding(nameof(ChartDataLabel.XPosition)),
                        new Binding(nameof(ChartDataLabel.YPosition)),
                    },
                    Converter = new TemplateVisibilityConverter(),
                };

                item.SetBinding(DataLabelItemView.IsVisibleProperty, binding);
                item.SetBinding(DataLabelItemView.XPositionProperty, new Binding(nameof(ChartDataLabel.XPosition)));
                item.SetBinding(DataLabelItemView.YPositionProperty, new Binding(nameof(ChartDataLabel.YPosition)));
                item.SetBinding(SfTemplatedView.ItemTemplateProperty, new Binding(nameof(IDataTemplateDependent.LabelTemplate)) { Source = this.source });
                return item;
            });
        }
    }

    internal class TemplatedViewLayoutManager : LayoutManager
    {
        public TemplatedViewLayoutManager(Microsoft.Maui.ILayout layout) : base(layout)
        {

        }

        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            var padding = Layout.Padding;

            double measuredHeight = 0;
            double measuredWidth = 0;

            if (Layout.Count > 0 && Layout is DataLabelLayout dataLabelLayout && dataLabelLayout.IsTemplateItemsChanged())
            {
                for (int n = 0; n < Layout.Count; n++)
                {
                    var child = Layout[n];

                    if (child.Visibility == Visibility.Collapsed)
                    {
                        continue;
                    }

                    var measure = child.Measure(widthConstraint, heightConstraint);

                    measuredHeight = Math.Max(measuredHeight, measure.Height);
                    measuredWidth = Math.Max(measuredWidth, measure.Width);
                }
            }

            var finalHeight = ResolveConstraints(heightConstraint, Layout.Height, 0, Layout.MinimumHeight, Layout.MaximumHeight);
            var finalWidth = ResolveConstraints(widthConstraint, Layout.Width, 0, Layout.MinimumWidth, Layout.MaximumWidth);

            return new Size(finalWidth, finalHeight);
        }

        public override Size ArrangeChildren(Rect bounds)
        {
            var padding = Layout.Padding;

            double top = padding.Top + bounds.Top;
            double left = padding.Left + bounds.Left;
            double availableWidth = bounds.Width - padding.HorizontalThickness;
            double availableHeight = bounds.Height - padding.VerticalThickness;

            for (int n = 0; n < Layout.Count; n++)
            {
                var child = Layout[n];

                if (child.Visibility == Visibility.Collapsed)
                {
                    continue;
                }

                if (child is ICustomAbsoluteView customView)
                {
                    var size = child.DesiredSize;

                    if (child.DesiredSize.IsZero)
                    {
                        size = child.Measure(bounds.Width, bounds.Height);
                    }

                    var destination = new Rect(customView.XPosition, customView.YPosition, child.DesiredSize.Width, child.DesiredSize.Height);

                    destination.X += left - (size.Width / 2);
                    destination.Y += top - (size.Height / 2);

                    customView.IsRequiredLayoutChange = false;
                    child.Arrange(destination);
                }
            }

            return new Size(availableWidth, availableHeight);
        }
    }
}
