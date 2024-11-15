using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Provides the base functionality for selection behaviors in charts.
	/// </summary>
	/// <remarks>
	/// The <see cref="ChartSelectionBehavior"/> class serves as the base class for specialized selection behaviors, 
	/// such as <see cref="DataPointSelectionBehavior"/> and <see cref="SeriesSelectionBehavior"/>. 
	/// It provides the core methods and properties required to handle selection logic in chart elements.
	/// </remarks>
	public abstract class ChartSelectionBehavior : ChartBehavior, IParentThemeElement
    {
        #region Private Fields 

        int _previousSelectedIndex = -1;
        bool _shouldUpdateOnTap = false;

        #endregion

        #region Internal Properties

        internal List<int> ActualSelectedIndexes { get; set; }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when the user clicks on the series segment or sets the value for <see cref="SelectedIndex"/> or <see cref="SelectedIndexes"/> properties. This event is triggered before a segment or series is selected. 
        /// </summary>
        /// <remarks>Restrict a data point from being selected, by canceling this event, by setting <see cref="System.ComponentModel.CancelEventArgs.Cancel"/> property to true in the event argument.</remarks>
        public event EventHandler<ChartSelectionChangingEventArgs>? SelectionChanging;

        /// <summary>
        /// This event is triggered after a clicked series segment or sets the value for <see cref="SelectedIndex"/> or <see cref="SelectedIndexes"/> properties. It provides information about the selection change, including the currently selected index and the previously selected index.
        /// </summary>
        public event EventHandler<ChartSelectionChangedEventArgs>? SelectionChanged;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="Type"/> bindable property determines the type of 
        /// selection behavior for the chart.
        /// </remarks>
        public static readonly BindableProperty TypeProperty = BindableProperty.Create(
            nameof(Type), 
            typeof(ChartSelectionType), 
            typeof(ChartSelectionBehavior), 
            ChartSelectionType.Single, 
            BindingMode.Default, 
            null, 
            OnSelectionTypeChanged);

        /// <summary>
        /// Identifies the <see cref="SelectionBrush"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="SelectionBrush"/> bindable property determines the brush 
        /// used for the selected chart segments.
        /// </remarks>
        public static readonly BindableProperty SelectionBrushProperty = BindableProperty.Create(
            nameof(SelectionBrush), 
            typeof(Brush), 
            typeof(ChartSelectionBehavior), 
            null, 
            BindingMode.Default, 
            null, 
            OnSelectionColorChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="SelectedIndex"/> bindable property determines the index 
        /// of the selected chart segment.
        /// </remarks>
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
            nameof(SelectedIndex), 
            typeof(int), 
            typeof(ChartSelectionBehavior), 
            -1, 
            BindingMode.Default, 
            null, 
            OnSelectedIndexChanged);

        /// <summary>
        /// Identifies the <see cref="SelectedIndexes"/> bindable property.
        /// </summary>
        /// <remarks>
        /// The identifier for the <see cref="SelectedIndexes"/> bindable property determines the
        /// indexes of the selected chart segments.
        /// </remarks>
        public static readonly BindableProperty SelectedIndexesProperty = BindableProperty.Create(
            nameof(SelectedIndexes), 
            typeof(List<int>), 
            typeof(ChartSelectionBehavior), 
            null, 
            BindingMode.Default, 
            null, 
            OnSelectedIndexesChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the selection mode for selection behavior.
        /// </summary>
        /// <value>This property takes <see cref="ChartSelectionType"/> as value and its default value is <see cref="ChartSelectionType.Single"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <!--omitted for brevity-->
        /// 
        ///         <chart:PieSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue">
        ///             <chart:PieSeries.SelectionBehavior>
        ///                 <chart:DataPointSelectionBehavior Type="Multiple" SelectionBrush = "Red" />
        ///         </chart:PieSeries.SelectionBehavior>
        ///         </chart:PieSeries>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  ViewModel viewModel = new ViewModel();
        ///  
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///  
        ///  series.SelectionBehavior = new DataPointSelectionBehavior()
        ///  {
        ///      Type = ChartSelectionType.Multiple,
        ///      SelectionBrush = new SolidColorBrush(Colors.Red),
        ///  };
        ///  
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartSelectionType Type
        {
            get { return (ChartSelectionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selection brush color for selection behavior.
        /// </summary>
        /// <value>This property takes <see cref="Brush"/> value and its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        /// 
        ///         <!--omitted for brevity-->
        /// 
        ///         <chart:PieSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue">
        ///             <chart:PieSeries.SelectionBehavior>
        ///                 <chart:DataPointSelectionBehavior SelectionBrush = "Red" />
        ///         </chart:PieSeries.SelectionBehavior>
        ///         </chart:PieSeries>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  ViewModel viewModel = new ViewModel();
        ///  
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///  
        ///  series.SelectionBehavior = new DataPointSelectionBehavior()
        ///  {
        ///      SelectionBrush = new SolidColorBrush(Colors.Red),
        ///  };
        ///  
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of segment or series to be selected in selection behavior.
        /// </summary>
        /// <value>This property takes <see cref="int"/> value and its default value is -1.</value>
        /// <remarks>
        /// <para>This property value is used only when <see cref="ChartSelectionBehavior.Type"/> is set to <see cref="ChartSelectionType.Single"/> or <see cref="ChartSelectionType.SingleDeselect"/>.</para>
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///         <!--omitted for brevity-->
        ///
        ///         <chart:PieSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue">
        ///             <chart:PieSeries.SelectionBehavior>
        ///                 <chart:DataPointSelectionBehavior SelectedIndex="3" SelectionBrush = "Red" />
        ///         </chart:PieSeries.SelectionBehavior>
        ///         </chart:PieSeries>
        ///
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  ViewModel viewModel = new ViewModel();
        ///
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///
        ///  series.SelectionBehavior = new DataPointSelectionBehavior()
        ///  {
        ///      SelectedIndex = 3,
        ///      SelectionBrush = new SolidColorBrush(Colors.Red),
        ///  };
        ///
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the list of segments or series to be selected in selection behavior.
        /// </summary>
        /// <value>This property takes the list of <see cref="List{T}"/> where <see cref="int"/> represents the integer values and its default value is null.</value>
        /// <remarks>
        /// <para>This property value is used only when <see cref="ChartSelectionBehavior.Type"/> is set to <see cref="ChartSelectionType.Multiple"/>.</para>
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCircularChart>
        ///
        ///         <!--omitted for brevity-->
        ///
        ///         <chart:PieSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue">
        ///             <chart:PieSeries.SelectionBehavior>
        ///                 <chart:DataPointSelectionBehavior Type="Multiple" SelectedIndexes="{Binding indexes}" SelectionBrush = "Red" />
        ///         </chart:PieSeries.SelectionBehavior>
        ///         </chart:PieSeries>
        ///
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  ViewModel viewModel = new ViewModel();
        ///
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewModel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///  };
        ///
        ///  List<int> indexes = new List<int>() { 1, 3, 5 };
        ///  series.SelectionBehavior = new DataPointSelectionBehavior()
        ///  {
        ///      Type = ChartSelectionType.Multiple,
        ///      SelectedIndexes= indexes,
        ///      SelectionBrush = new SolidColorBrush(Colors.Red),
        ///  };
        ///
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public List<int> SelectedIndexes
        {
            get { return (List<int>)GetValue(SelectedIndexesProperty); }
            set { SetValue(SelectedIndexesProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSelectionBehavior"/> class.
        /// </summary>
        public ChartSelectionBehavior()
        {
            ThemeElement.InitializeThemeResources(this, "SfChartTheme");
            SelectedIndexes = ActualSelectedIndexes = new List<int>();
        }

        #endregion

        #region Methods

        #region Interface Implementation

        ResourceDictionary IParentThemeElement.GetThemeDictionary()
        {
            return new SfChartCommonStyle();
        }

        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Resets the current selection behavior by removing all values from SelectedIndex and SelectedIndexes.
        /// </summary>
        public void ClearSelection()
        {
            if (CanClearSelection())
            {
                ResetMultiSelection();
                SelectedIndex = -1;
                _previousSelectedIndex = -1;
                SelectedIndexes?.Clear();
            }
        }

        #endregion

        #region Internal methods

        internal void UpdateSelectionChanging(int index, bool isUpdateOnTap)
        {
            switch (Type)
            {
                case ChartSelectionType.Multiple:
                    if (!ActualSelectedIndexes.Contains(index))
                    {
                        ActualSelectedIndexes.Add(index);
                    }
                    else
                    {
                        ActualSelectedIndexes.Remove(index);
                    }

                    if (isUpdateOnTap)
                    {
                        _shouldUpdateOnTap = isUpdateOnTap;
                        SelectedIndexes = new List<int>(ActualSelectedIndexes);
                    }

                    UpdateSelectedItem(index);
                    break;

                default:

                    _previousSelectedIndex = SelectedIndex;

                    if (SelectedIndex == index && Type == ChartSelectionType.SingleDeselect)
                    {
                        SelectedIndex = -1;
                    }
                    else
                    {
                        SelectedIndex = index;
                    }

                    break;
            }

            _shouldUpdateOnTap = false;
        }

        internal Brush? GetSelectionBrush(int index)
        {
            if ((ActualSelectedIndexes.Contains(index) && Type == ChartSelectionType.Multiple) ||
               ((SelectedIndex == index) && (Type == ChartSelectionType.Single || Type == ChartSelectionType.SingleDeselect)))
            {
                return SelectionBrush;
            }

            return null;
        }

        internal void InvokeSelectionChangedEvent(object source, int index)
        {
            if (SelectionChanged == null)
                return;

            ChartSelectionChangedEventArgs args = new();
            if (Type == ChartSelectionType.Multiple)
            {
                if (ActualSelectedIndexes.Contains(index))
                {
                    args.NewIndexes.Add(index);
                }
                else
                {
                    args.OldIndexes.Add(index);
                }
            }
            else
            {
                if (SelectedIndex == -1)
                    args.OldIndexes.Add(index);
                else
                    args.NewIndexes.Add(SelectedIndex);
            }

            SelectionChanged.Invoke(source, args);
        }

        internal bool IsSelectionChangingInvoked(object source, int index)
        {
            if (SelectionChanging == null)
                return true;

            ChartSelectionChangingEventArgs args = new();

            if (Type == ChartSelectionType.Multiple)
            {
                if (ActualSelectedIndexes.Contains(index))
                {
                    args.OldIndexes.Add(index);
                }
                else
                {
                    args.NewIndexes.Add(index);
                }
            }
            else
            {
                if (SelectedIndex != index)
                    args.NewIndexes.Add(index);
                if (SelectedIndex != -1)
                    args.OldIndexes.Add(SelectedIndex);
            }

            SelectionChanging.Invoke(source, args);
            return !args.Cancel;
        }

        internal void Invalidate(ChartSeries series)
        {
            series.InvalidateSeries();

            if (series.ShowDataLabels)
                series.InvalidateDataLabel();
        }

        internal virtual void SelectionIndexesPropertyChanged(List<int> newValue)
        {
        }

        internal virtual void MapActualIndexes()
        {
            if (SelectedIndex != -1 && !ActualSelectedIndexes.Contains(SelectedIndex))
            {
                ActualSelectedIndexes.Add(SelectedIndex);
            }

            if (SelectedIndexes != null)
            {
                foreach (int index in SelectedIndexes)
                {
                    if (!ActualSelectedIndexes.Contains(index))
                    { ActualSelectedIndexes.Add(index); }
                }
            }
        }

        internal virtual bool CanClearSelection()
        {
            return false;
        }

        internal virtual void ChangeSelectionBrushColor(object newValue)
        {
        }

        internal virtual void UpdateSelectedItem(int index)
        {
        }

        internal virtual void ResetMultiSelection()
        {
        }

        internal virtual void SelectionIndexChanged(int oldValue, int newValue)
        {
        }

        #endregion

        #region Private Methods

        void SelectionTypeChanged(int oldValue, int newValue)
        {
            if ((oldValue == (int)ChartSelectionType.Multiple || newValue == (int)ChartSelectionType.Multiple))
            {
                //TODO: Confirm to reset color when type changed.
                ClearSelection();
            }
        }

        void InternalClearSelection()
        {
            ResetMultiSelection();
        }

        void InitializeDynamicResource(ChartSelectionBehavior behavior)
        {
            if (behavior is DataPointSelectionBehavior)
            {
                SetDynamicResource(SelectionBrushProperty, "SfChartDataPointSelectionBrush");
            }
            else
            {
                SetDynamicResource(SelectionBrushProperty, "SfChartSeriesSelectionBrush");
            }
        }

        static void OnSelectionTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartSelectionBehavior behavior)
            {
                behavior.SelectionTypeChanged((int)oldValue, (int)newValue);
            }
        }

        static void OnSelectionColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartSelectionBehavior behavior)
            {
                behavior.ChangeSelectionBrushColor(newValue);
            }
        }

        static void OnSelectedIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartSelectionBehavior behavior)
            {
                behavior.SelectionIndexChanged((int)oldValue, (int)newValue);
                behavior.InitializeDynamicResource(behavior);
            }
        }

        static void OnSelectedIndexesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartSelectionBehavior behavior && behavior.Type != ChartSelectionType.None && !behavior._shouldUpdateOnTap)
            {
                if (oldValue != null)
                {
                    behavior.InternalClearSelection();
                }

                if (newValue == null)
                {
                    behavior.ActualSelectedIndexes = new List<int>();
                }
                else
                {
                    behavior.SelectionIndexesPropertyChanged((List<int>)newValue);
                }
            }
        }

        #endregion

        #endregion
    }
}