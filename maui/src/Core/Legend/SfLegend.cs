using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using Layout = Microsoft.Maui.Controls.Layout;
using ScrollBarVisibility = Microsoft.Maui.ScrollBarVisibility;

namespace Syncfusion.Maui.Toolkit
{
	/// <summary>
	/// Represents a legend component for displaying chart legends or other graphical legends.
	/// </summary>
	internal class SfLegend : SfView, ILegend
    {
        #region Fields

        const double _maxSize = 8388607.5;

        readonly ScrollView _legendView;

        Func<double> _getLegendSizeCoeff = GetLegendSizeCoeff;

        #endregion

        #region Bindable Properties

        #region Public Bindable Properties

        /// <summary>
        /// Gets or sets the items source for the legend. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(SfLegend),
            null,
            BindingMode.Default,
            null,
            OnItemsSourceChanged);

        /// <summary>
        /// The DependencyProperty for <see cref="ToggleVisibility"/> property.
        /// </summary>
        public static readonly BindableProperty ToggleVisibilityProperty = BindableProperty.Create(
            nameof(ToggleVisibility),
            typeof(bool),
            typeof(SfLegend),
            false,
            BindingMode.Default,
            null,
            OnToggleVisibilityChanged,
            null,
            null);

        #endregion

        #region Internal Bindable Properties
        /// <summary>
        /// Gets or sets a data template for all series legend item. This is a bindable property.
        /// </summary>
        internal static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(SfLegend),
            null,
            BindingMode.Default,
            null,
            OnItemTemplateChanged,
            null,
            null);

        /// <summary>
        ///  Gets or sets placement of the legend. This is a bindable property.
        /// </summary>
        internal static readonly BindableProperty PlacementProperty = BindableProperty.Create(
            nameof(Placement),
            typeof(LegendPlacement),
            typeof(SfLegend),
            LegendPlacement.Top,
            BindingMode.Default,
            null,
            OnPlacementChanged,
            null,
            null);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty ItemsLayoutProperty = BindableProperty.Create(
            nameof(ItemsLayout),
            typeof(Layout),
            typeof(SfLegend),
            null,
            BindingMode.Default,
            null,
            OnPlacementChanged,
            null,
            null);

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        ///  Gets or sets the ItemsSource for the legend.
        /// </summary>
        /// <remarks>The default will be of <see cref="LegendItem"/> type.</remarks>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to bind the series visibility with its corresponding legend item in the legend. This is bindable property.
        /// </summary>
        public bool ToggleVisibility
        {
            get { return (bool)GetValue(ToggleVisibilityProperty); }
            set { SetValue(ToggleVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets the itemlayout.
        /// </summary>
        public Layout ItemsLayout
        {
            get { return (Layout)GetValue(ItemsLayoutProperty); }
            set { SetValue(ItemsLayoutProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the data template for legend item.
        /// </summary>
        /// <value>This property takes the DataTemplate value.</value>
        internal DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets placement of the legend. This is a bindable property.
        /// </summary>
        internal LegendPlacement Placement
        {
            get { return (LegendPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        internal Layout ContentLayout { get; set; }

        LegendPlacement ILegend.Placement { get => Placement; set { } }

        DataTemplate ILegend.ItemTemplate { get => ItemTemplate; set { } }

        bool ILegend.IsVisible { get; set; }

        internal event EventHandler<LegendItemClickedEventArgs>? ItemClicked;

        Func<double> ILegend.ItemsMaximumHeightRequest
        {
            get => _getLegendSizeCoeff;
            set { _getLegendSizeCoeff = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfLegend"/> class.
        /// </summary>
        public SfLegend()
        {
            _legendView = new ScrollView()
            {
                Orientation = ScrollOrientation.Both,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never
            };

            _legendView.HorizontalOptions = LayoutOptions.Center;
            _legendView.VerticalOptions = LayoutOptions.Center;

            ContentLayout = ItemsLayout is Layout ? ItemsLayout : new HorizontalStackLayout();

            UpdateLegendTemplate();
            _legendView.Content = ContentLayout;
            Add(_legendView);
        }

		#endregion

		#region Methods

		#region Public Methods

		/// <summary>
		/// Calculates the rectangle size for the legend based on the available size and maximum size percentage.
		/// </summary>
		/// <param name="legend">The legend for which the rectangle size is being calculated.</param>
		/// <param name="availableSize">The available size within which the legend should fit.</param>
		/// <param name="maxSizePercentage">The maximum size percentage of the available size that the legend can occupy.</param>
		/// <returns>A <see cref="Rect"/> representing the size and position of the legend.</returns>
		public static Rect GetLegendRectangle(SfLegend legend, Rect availableSize, double maxSizePercentage)
        {
            if (legend != null)
            {
                Size size = Size.Zero;
                double maxSize = 8388607.5;
                double position = 0;
                double maxWidth = 0;

                switch (legend.Placement)
                {
                    case LegendPlacement.Top:
                        size = legend.Measure(availableSize.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
                        maxWidth = availableSize.Height * maxSizePercentage < size.Height ? availableSize.Height * maxSizePercentage : size.Height;
                        position = (availableSize.Height != maxSize) ? availableSize.Height - maxWidth : 0;
                        return new Rect(availableSize.X, availableSize.Y, availableSize.Width, maxWidth);

                    case LegendPlacement.Bottom:
                        size = legend.Measure(availableSize.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
                        maxWidth = availableSize.Height * maxSizePercentage < size.Height ? availableSize.Height * maxSizePercentage : size.Height;
                        position = (availableSize.Height != maxSize) ? availableSize.Height - maxWidth : 0;
                        return new Rect(availableSize.X, availableSize.Y + position, availableSize.Width, maxWidth);

                    case LegendPlacement.Left:
                        size = legend.Measure(double.PositiveInfinity, availableSize.Height, MeasureFlags.IncludeMargins);
                        maxWidth = availableSize.Width * maxSizePercentage < size.Width ? availableSize.Width * maxSizePercentage : size.Width;
                        return new Rect(availableSize.X, availableSize.Y, maxWidth, availableSize.Height);

                    case LegendPlacement.Right:
                        size = legend.Measure(double.PositiveInfinity, availableSize.Height, MeasureFlags.IncludeMargins);
                        maxWidth = availableSize.Width * maxSizePercentage < size.Width ? availableSize.Width * maxSizePercentage : size.Width;
                        position = (availableSize.Width != maxSize) ? availableSize.Width - maxWidth : 0;
                        return new Rect(availableSize.X + position, availableSize.Y, maxWidth, availableSize.Height);
                }
            }

            return Rect.Zero;
        }


        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates the <see cref="SfShapeView"/> type for the default legend icon.
        /// </summary>
        /// <returns>Returns the ShapeView.</returns>
        protected virtual SfShapeView CreateShapeView()
        {
            return new SfShapeView();
        }

        /// <summary>
        /// Creates the <see cref="Label"/> type for the default legend text.
        /// </summary>
        /// <returns>Returns the Label.</returns>
        /// <remarks>Method must return label type and should not return any null value.</remarks>
        protected virtual Label CreateLabelView()
        {
            return new Label();
        }

        #endregion

        #region Private Call backs

        static double GetLegendSizeCoeff()
        {
            return 0.25d;
        }

        static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLegend legend)
            {
                legend.OnItemsSourceChanged(oldValue, newValue);
            }
        }

        static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLegend legend)
            {
                legend.OnItemTemplateChanged(oldValue, newValue);
            }
        }

        static void OnPlacementChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLegend legend)
            {
                legend.UpdateLegendPlacement();
            }
        }

        static void OnToggleVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        static void OnLegendLayoutChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfLegend legend)
            {
                legend.OnLegendLayoutChanged();
            }
        }

        #endregion

        #region Private Methods

#if MACCATALYST || IOS

        //Todo: Remove this method the when the SfView was measured the correct size while dynamically changes the itemsSource.
        internal void UpdateRelayout()
        {
            InvalidateMeasure();
        }
#endif

        internal void UpdateLegendPlacement()
        {
            if (ItemsLayout is Layout customLayout)
            {
                ContentLayout = customLayout;
            }
            else
            {
                if (Placement == LegendPlacement.Left || Placement == LegendPlacement.Right)
                {
                    ContentLayout = new VerticalStackLayout();
                }
                else
                {
                    ContentLayout = new HorizontalStackLayout();
                }
            }

            OnLegendLayoutChanged();
            _legendView.Content = ContentLayout;
        }

        void OnLegendLayoutChanged()
        {
            var template = new DataTemplate(() =>
            {
                DataTemplate itemTemplate = ItemTemplate;
                LegendItemView views = new LegendItemView(LegendTappedAction);

                if (itemTemplate == null)
                {
                    views.ItemTemplate = GetDefaultLegendTemplate();
                }

                if (itemTemplate != null)
                {
                    views.ItemTemplate = itemTemplate;
                }

                return views;

            });

            BindableLayout.SetItemTemplate(ContentLayout, template);

            if (ItemsSource != null)
            {
                BindableLayout.SetItemsSource(ContentLayout, ItemsSource);
            }
        }

        public void LegendTappedAction(LegendItem legendItem)
        {
            if (ToggleVisibility && legendItem != null)
            {
                legendItem.IsToggled = !legendItem.IsToggled;

                if (ItemClicked != null)
                {
                    ItemClicked(this, new LegendItemClickedEventArgs() { LegendItem = legendItem });
                }
            }
        }

        /// <summary>
        /// Metod used to get the default legend template.
        /// </summary>
        /// <returns></returns>
        DataTemplate GetDefaultLegendTemplate()
        {
            var template = new DataTemplate(() =>
            {
                HorizontalStackLayout stack = new HorizontalStackLayout()
                {
                    Spacing = 6,
                    Padding = new Thickness(8, 10)
                };

                ToggleColorConverter toggleColorConverter = new ToggleColorConverter();
                Binding binding;
                Binding binding1;
                MultiBinding multiBinding;
                SfShapeView shapeView = CreateShapeView();

                if (shapeView != null)
                {
                    shapeView.HorizontalOptions = LayoutOptions.Start;
                    shapeView.VerticalOptions = LayoutOptions.Center;
                    binding = new Binding(nameof(LegendItem.IsToggled));
                    binding.Converter = toggleColorConverter;
                    binding.ConverterParameter = shapeView;
                    binding1 = new Binding(nameof(LegendItem.IconBrush));
                    multiBinding = new MultiBinding()
                    {
                        Bindings = new List<BindingBase>() { binding, binding1 },
                        Converter = new MultiBindingIconBrushConverter(),
                        ConverterParameter = shapeView
                    };

                    shapeView.SetBinding(SfShapeView.IconBrushProperty, multiBinding);
                    shapeView.SetBinding(SfShapeView.ShapeTypeProperty, nameof(LegendItem.IconType));
                    shapeView.SetBinding(SfShapeView.HeightRequestProperty, nameof(LegendItem.IconHeight));
                    shapeView.SetBinding(SfShapeView.WidthRequestProperty, nameof(LegendItem.IconWidth));
                    stack.Children.Add(shapeView);
                }

                Label label = CreateLabelView();

                if (label != null)
                {
                    label.VerticalTextAlignment = TextAlignment.Center;
                    label.SetBinding(Label.TextProperty, nameof(LegendItem.Text));
                    label.SetBinding(Label.TextColorProperty, nameof(LegendItem.ActualTextColor));
                    label.SetBinding(Label.MarginProperty, nameof(LegendItem.TextMargin));
                    label.SetBinding(Label.FontSizeProperty, nameof(LegendItem.FontSize));
                    label.SetBinding(Label.FontFamilyProperty, nameof(LegendItem.FontFamily));
                    label.SetBinding(Label.FontAttributesProperty, nameof(LegendItem.FontAttributes));
                    stack.Children.Add(label);
                }
                return stack;
            });

            return template;
        }

        void UpdateLegendTemplate()
        {
            if (ItemTemplate == null)
            {
                var itemTemplate = new DataTemplate(() =>
                {
                    LegendItemView views = new LegendItemView(LegendTappedAction);
                    views.ItemTemplate = GetDefaultLegendTemplate();
                    return views;
                });

                BindableLayout.SetItemTemplate(ContentLayout, itemTemplate);
            }
        }

        void OnItemsSourceChanged(object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (newValue != null && ContentLayout != null)
            {
                BindableLayout.SetItemsSource(ContentLayout, newValue as IEnumerable);
            }
        }

        void OnItemTemplateChanged(object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (newValue == null)
            {
                UpdateLegendTemplate();
            }

            if (_legendView != null)
            {
                if (newValue != null && newValue is DataTemplate dataTemplate)
                {
                    var itemtemplate = new DataTemplate(() =>
                    {
                        LegendItemView views = new LegendItemView(LegendTappedAction);
                        views.ItemTemplate = dataTemplate;
                        return views;
                    });

                    BindableLayout.SetItemTemplate(ContentLayout, itemtemplate);
                }
            }
        }
    }

	#endregion
	#endregion

	/// <summary>
	/// Represents a view for individual legend items, inheriting from <see cref="ContentView"/> and implementing the <see cref="ITapGestureListener"/> interface.
	/// </summary>
	internal class LegendItemView : ContentView, ITapGestureListener
    {
        readonly Action<LegendItem> _legendAction;

		/// <summary>
		/// Identifies the bindable property for the item template.
		/// </summary>
		internal static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
            nameof(ItemTemplate),
            typeof(DataTemplate),
            typeof(LegendItemView),
            default(DataTemplate),
            BindingMode.Default,
            null,
            OnItemTemplateChanged,
            null,
            null);

		/// <summary>
		/// Gets or sets the data template for the items.
		/// </summary>
		internal DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="LegendItemView"/> class with the specified action.
		/// </summary>
		/// <param name="action">The action to be performed on the legend item.</param>
		public LegendItemView(Action<LegendItem> action)
        {
            _legendAction = action;
            this.AddGestureListener(this);
        }

        static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LegendItemView template)
            {
                template.OnItemTemplateChanged(oldValue, newValue);
            }
        }

        void OnItemTemplateChanged(object oldValue, object newValue)
        {
            if (newValue != null && newValue is DataTemplate template)
            {
                base.Content = (View)template.CreateContent();
            }
        }

		/// <summary>
		/// Handles the tap event.
		/// </summary>
		/// <param name="e">The event data for the tap event.</param>
		public void OnTap(TapEventArgs e)
        {
            if (_legendAction != null && BindingContext is LegendItem legendItem)
            {
                _legendAction.Invoke(legendItem);
            }
        }
    }
}
