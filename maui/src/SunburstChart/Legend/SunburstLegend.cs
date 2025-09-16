using Syncfusion.Maui.Toolkit.Themes;
using System.Collections;

namespace Syncfusion.Maui.Toolkit.SunburstChart
{
    /// <summary>
    /// Represents the legend for the SfSunburstChart class.
    /// </summary>
    public class SunburstLegend : BindableObject, ILegend
    {
        internal SfLegend? SfLegend { get; set; }

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Placement"/> bindable property. 
        /// </summary>        
        public static readonly BindableProperty PlacementProperty = 
			BindableProperty.Create(
				nameof(Placement), 
				typeof(LegendPlacement), 
				typeof(SunburstLegend), 
				LegendPlacement.Top, 
				BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ToggleSeriesVisibility"/> bindable property. 
        /// </summary>        
        internal static readonly BindableProperty ToggleSeriesVisibilityProperty = 
			BindableProperty.Create(
				nameof(ToggleSeriesVisibility), 
				typeof(bool), 
				typeof(SunburstLegend), 
				false, 
				BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ItemsLayout"/> bindable property. 
        /// </summary>        
        internal static readonly BindableProperty ItemsLayoutProperty = 
			BindableProperty.Create(
				nameof(ItemsLayout), 
				typeof(Layout), 
				typeof(SunburstLegend), 
				null, 
				BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ItemMargin"/> bindable property. 
        /// </summary>        
        internal static readonly BindableProperty ItemMarginProperty = 
			BindableProperty.Create(
				nameof(ItemMargin), 
				typeof(Thickness), 
				typeof(SunburstLegend), 
				new Thickness(double.NaN), 
				BindingMode.Default);

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> bindable property. 
        /// </summary>
        internal static readonly BindableProperty ItemTemplateProperty = 
			BindableProperty.Create(
				nameof(ItemTemplate), 
				typeof(DataTemplate), 
				typeof(SunburstLegend), 
				default(DataTemplate));

        /// <summary>
        /// Identifies the <see cref="IsVisible"/> bindable property. 
        /// </summary>        
        public static readonly BindableProperty IsVisibleProperty = 
			BindableProperty.Create(
				nameof(IsVisible), 
				typeof(bool), 
				typeof(SunburstLegend), 
				true, 
				BindingMode.Default);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the legend is visible in the sunburst chart.
        /// </summary>
        public bool IsVisible
        {
            get => (bool)GetValue(IsVisibleProperty);
            set => SetValue(IsVisibleProperty, value);
        }

        /// <summary>
        /// Gets or sets the placement for the legend in the sunburst chart.
        /// </summary>
        public LegendPlacement Placement
        {
            get => (LegendPlacement)GetValue(PlacementProperty);
            set => SetValue(PlacementProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        internal Layout ItemsLayout
        {
            get => (Layout)GetValue(ItemsLayoutProperty);
            set => SetValue(ItemsLayoutProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool ToggleSeriesVisibility
        {
            get => (bool)GetValue(ToggleSeriesVisibilityProperty);
            set => SetValue(ToggleSeriesVisibilityProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        internal Thickness ItemMargin
        {
            get => (Thickness)GetValue(ItemMarginProperty);
            set => SetValue(ItemMarginProperty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        internal DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        IEnumerable? itemsSource;
#pragma warning disable CS8603 // Possible null reference return.
        IEnumerable ILegend.ItemsSource { get => itemsSource; set => itemsSource = value; }
#pragma warning restore CS8603 // Possible null reference return.
        Layout ILegend.ItemsLayout { get => ItemsLayout; set => ItemsLayout = value; }
        bool ILegend.ToggleVisibility { get => ToggleSeriesVisibility; set => ToggleSeriesVisibility = value; }
        DataTemplate ILegend.ItemTemplate { get => ItemTemplate; set => ItemTemplate = value; }
        Func<double> getLegendSizeCoeff = GetMaximumSizeCoefficient;

        Func<double> ILegend.ItemsMaximumHeightRequest
        {
            get => GetMaximumSizeCoefficient;
            set => getLegendSizeCoeff = value;
        }
        
        #endregion

        #region Methods

        private static double GetMaximumSizeCoefficient()
        {
            return 0.25d;
        }

        SfLegend ILegend.CreateLegendView()
        {
            SfLegend = new SfLegendExt();
            return SfLegend;
        }

        #endregion
    }

    internal class SfLegendExt : SfLegend
    {
        protected override SfShapeView CreateShapeView()
        {
            return new SfShapeView() { StrokeWidth = 0 };
        }
    }

    internal class ChartLegendLabelStyle : Element, IThemeElement
    {
        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty TextColorProperty = 
			BindableProperty.Create(
				nameof(TextColor), 
				typeof(Color), 
				typeof(ChartLegendLabelStyle), 
				Colors.Black, 
				BindingMode.Default, 
				null, 
				OnTextColorChanged);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty FontSizeProperty = 
			BindableProperty.Create(
				nameof(FontSize), 
				typeof(double), 
				typeof(ChartLegendLabelStyle), 
				12d, 
				BindingMode.Default, 
				null, 
				OnTextColorChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        /// <summary>
        /// Gets or sets a value that indicates the label's size.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values.</value>
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartLegendLabelStyle()
        {
            ThemeElement.InitializeThemeResources(this, "SfSunburstChartTheme");
        }

        #endregion

        #region Interface Implementation

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartLegendLabelStyle legendStyle)
            {
                legendStyle.UpdateItems();
            }
        }

        #endregion

        #region Methods

        internal void UpdateItems()
        {
            if (Parent is SfSunburstChart chart)
            {
                chart.UpdateLegendItems();
            }
        }

        #endregion
    }
}
