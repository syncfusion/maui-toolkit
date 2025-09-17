using System.Collections;
using System.ComponentModel;
using System.Globalization;
using Syncfusion.Maui.Toolkit.Popup;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents the Picker base class.
    /// </summary>
    public abstract partial class PickerBase : SfView, IPicker
    {
        #region Fields

        /// <summary>
        /// Holds the previouslySelectedDate.
        /// </summary>
        internal DateTime _previousSelectedDateTime;

        /// <summary>
        /// The minimum height for the picker UI
        /// </summary>
        readonly double _minHeight = 300;

        /// <summary>
        /// The minimum width for the picker UI.
        /// </summary>
        readonly double _minWidth = 300;

        /// <summary>
        /// Holds the value of previous UI culture.
        /// </summary>
        CultureInfo _previousUICulture = CultureInfo.CurrentUICulture;

        /// <summary>
        /// The header layout contains the text of the header.
        /// </summary>
        HeaderLayout? _headerLayout;

        /// <summary>
        /// The footer layout contains ok and cancel buttons.
        /// </summary>
        FooterLayout? _footerLayout;

        /// <summary>
        /// The column header layout contains the text of the column header.
        /// </summary>
        ColumnHeaderLayout? _columnHeaderLayout;

        /// <summary>
        /// The picker container contains picker view layouts.
        /// </summary>
        PickerContainer? _pickerContainer;

        /// <summary>
        /// Holds the picker measured size.
        /// </summary>
        Size _availableSize = Size.Zero;

        /// <summary>
        /// The picker stack layout contains header, footer and picker container.
        /// </summary>
        PickerStackLayout? _pickerStackLayout;

        /// <summary>
        /// The SfPopup view.
        /// </summary>
        SfPopup? _popup;

        /// <summary>
        /// Boolean to get the previous open state of the picker in the dialog mode.
        /// This value is used to maintain the previous state of the picker while changing the visibility of the picker in the dialog mode.
        /// While changing the IsOpen property of the picker dynamically in the Visibility change the property change will not trigger sometimes.
        /// So that we are updated the previous state of the picker when the IsOpen property changed.
        /// </summary>
        bool _isPickerPreviouslyOpened = false;

        /// <summary>
        /// Flag indicating whether the text style is not internally.
        /// </summary>
        bool _isExternalStyle = false;

        /// <summary>
        /// Holds the value of internal property change or not.
        /// </summary>
        bool _isInternalPropertyChange = false;

#if MACCATALYST || IOS

        /// <summary>
        /// Holds the value of picker view loaded or not.
        /// </summary>
        bool _isPickerViewLoaded = false;

#endif

        #endregion

        #region Constructor
#if IOS
        /// <summary>
        /// Initializes a new instance of the <see cref="PickerBase"/> class.
        /// </summary>
        protected PickerBase()
        {
            IgnoreSafeArea = true;
        }
#endif
        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets a value indicating whether the direction of the layout is RTL.
        /// </summary>
        bool IPickerCommon.IsRTLLayout => IsRTL(this);

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the main page of the application.
        /// </summary>
        /// <returns>Returns page.</returns>
        internal static Page? GetMainWindowPage()
        {
            var application = IPlatformApplication.Current?.Application as Application;
            if (application != null && application.Windows.Count > 0)
            {
                return application.Windows[0].Page;
            }

            return new Page();
        }

        /// <summary>
        /// Method to reset the header interaction highlight.
        /// </summary>
        internal void ResetHeaderHighlight()
        {
            _headerLayout?.ResetHeaderHighlight();
        }

        /// <summary>
        /// Method to get the picker container value.
        /// </summary>
        /// <returns>The picker container.</returns>
        internal PickerContainer? GetPickerContainerValue()
        {
            return _pickerContainer;
        }

        /// <summary>
        /// Method to invoke the closed event.
        /// </summary>
        /// <param name="sender">The picker instance.</param>
        /// <param name="eventArgs">The event arguments.</param>
        internal void InvokeClosedEvent(object sender, EventArgs eventArgs)
        {
            Closed?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Method to invoke the closing event.
        /// </summary>
        /// <param name="sender">The picker instance.</param>
        /// <param name="eventArgs">The event arguments.</param>
        internal void InvokeClosingEvent(object sender, CancelEventArgs eventArgs)
        {
            Closing?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Method to invoke the opened event.
        /// </summary>
        /// <param name="sender">The picker instance.</param>
        /// <param name="eventArgs">The event arguments.</param>
        internal void InvokeOpenedEvent(object sender, EventArgs eventArgs)
        {
            Opened?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Method to invoke ok button clicked event.
        /// </summary>
        /// <param name="sender">The picker instance.</param>
        /// <param name="eventArgs">The event arguments.</param>
        internal void InvokeOkButtonClickedEvent(object sender, EventArgs eventArgs)
        {
            OkButtonClicked?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// Method to invoke ok button clicked event.
        /// </summary>
        /// <param name="sender">The picker instance.</param>
        /// <param name="eventArgs">The event arguments.</param>
        internal void InvokeCancelButtonClickedEvent(object sender, EventArgs eventArgs)
        {
            CancelButtonClicked?.Invoke(sender, eventArgs);
        }

        /// <summary>
        /// To check the internal selection need
        /// </summary>
        /// <returns>Returns the internal selection or not</returns>
        internal bool IsScrollSelectionAllowed()
        {
            if (Mode != PickerMode.Default && FooterView.Height != 0 && FooterView.ShowOkButton)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the RTL value true or false.
        /// </summary>
        /// <param name="view">The type of view.</param>
        /// <returns>Returns true or false.</returns>
        static bool IsRTL(View view)
        {
            //// Flow direction property only added in VisualElement class only, so that we return while the object type is not visual element.
            //// And Effective flow direction is flag type so that we add & RightToLeft.
            if (!(view is IVisualElementController))
            {
                return false;
            }

            return ((view as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;
        }

        /// <summary>
        /// Method to update format based on current culture.
        /// </summary>
        void UpdateFormatBasedOnCulture()
        {
            if (!_previousUICulture.Name.Equals(CultureInfo.CurrentUICulture.Name, StringComparison.Ordinal))
            {
                _previousUICulture = CultureInfo.CurrentUICulture;

                // Updated Formats of SfDatePicker,SfDateTimePicker and SfTimePicker to Reinitialize the component
                if (Children is SfDatePicker datePicker)
                {
                    datePicker.UpdateFormat();
                }

                if (Children is SfDateTimePicker dateTimePicker)
                {
                    dateTimePicker.ResetDateColumns();
                    dateTimePicker.BaseHeaderView.DateText = dateTimePicker.GetDateHeaderText();
                    dateTimePicker.BaseHeaderView.TimeText = dateTimePicker.GetTimeHeaderText();
                }

                if (Children is SfTimePicker timePicker)
                {
                    timePicker.UpdateFormat();
                }
            }
        }

        /// <summary>
        /// Method to add the children of the picker control.
        /// </summary>
        void AddChildren()
        {
            _pickerStackLayout = new PickerStackLayout(this);
            AddOrRemoveHeaderLayout();
            _pickerContainer = new PickerContainer(this);
            _pickerStackLayout.Children.Add(_pickerContainer);
            AddOrRemoveFooterLayout();
            Children.Add(_pickerStackLayout);
        }

        /// <summary>
        /// Method to add or remove header layout.
        /// </summary>
        void AddOrRemoveHeaderLayout()
        {
            if (BaseHeaderView.Height > 0 && _headerLayout == null)
            {
                _headerLayout = new HeaderLayout(this);
                _pickerStackLayout?.Children.Add(_headerLayout);
            }
            else if (_headerLayout != null && (BaseHeaderView.Height <= 0))
            {
                _pickerStackLayout?.Children.Remove(_headerLayout);
                _headerLayout = null;
            }
        }

        /// <summary>
        /// Method to add or remove column header layout.
        /// </summary>
        void AddorRemoveColumnHeaderLayout()
        {
            if (BaseColumnHeaderView.Height > 0 && _columnHeaderLayout == null && ColumnHeaderTemplate != null)
            {
                _columnHeaderLayout = new ColumnHeaderLayout(this, string.Empty);
                _pickerStackLayout?.Children.Insert(1, _columnHeaderLayout);
            }
            else if (_columnHeaderLayout != null && BaseColumnHeaderView.Height <= 0 && ColumnHeaderTemplate != null)
            {
                _pickerStackLayout?.Children.Remove(_columnHeaderLayout);
                _columnHeaderLayout = null;
            }
        }

        /// <summary>
        /// Method to add or remove footer layout.
        /// </summary>
        void AddOrRemoveFooterLayout()
        {
            if (FooterView.Height > 0 && _footerLayout == null)
            {
                _footerLayout = new FooterLayout(this);
                _pickerStackLayout?.Children.Add(_footerLayout);
            }
            else if (_footerLayout != null && FooterView.Height <= 0)
            {
                _pickerStackLayout?.Children.Remove(_footerLayout);
                _footerLayout.RemoveFooterButtons();
                _footerLayout = null;
            }
        }

        /// <summary>
        /// Method to update the picker view.
        /// </summary>
        void InvalidatePickerView()
        {
            InvalidateMeasure();
            _pickerStackLayout?.InvalidateView();
#if WINDOWS || MACCATALYST
            if (EnableLooping)
            {
                _pickerContainer?.UpdateItemHeight();
            }
#endif
        }

        /// <summary>
        /// Method to update the selected index changed.
        /// </summary>
        /// <param name="columnIndex">The updated column index.</param>
        void UpdateSelectedIndexValue(int columnIndex)
        {
            if (_pickerContainer == null || _availableSize == Size.Zero)
            {
                return;
            }

            _pickerContainer.UpdateSelectedIndexValue(columnIndex);
        }

        /// <summary>
        /// Method to update the picker selection view.
        /// </summary>
        void UpdatePickerSelectionView()
        {
            if (_pickerContainer == null || _availableSize == Size.Zero)
            {
                return;
            }

            _pickerContainer.UpdatePickerSelectionView();
        }

        /// <summary>
        /// Method to show the picker popup based on the mode.
        /// </summary>
        void ShowPopup()
        {
            if (_popup == null)
            {
                return;
            }

            if (Mode == PickerMode.RelativeDialog)
            {
                OnPickerLoading();
                if (RelativeView != null)
                {
                    ShowRelativeToView(RelativeView, RelativePosition);
                }
                else
                {
                    ShowRelativeToView(this, RelativePosition);
                }
            }
            else
            {
                OnPickerLoading();

                var application = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;
                var windowPage = GetMainWindowPage();
                if (application != null && windowPage is Shell shellPage && !shellPage.IsLoaded)
                {
                    shellPage.Loaded -= ShellPage_Loaded;
                    shellPage.Loaded += ShellPage_Loaded;
#if MACCATALYST || IOS
                    if (windowPage.Navigation != null && windowPage.Navigation.ModalStack.Count > 0)
                    {
                        _popup.IsOpen = true;
                    }
#endif
                }
                else
                {
                    _popup.IsOpen = true;
                }
            }
        }

        /// <summary>
        /// Method for shell page loaded event.
        /// </summary>
        /// <param name="sender">Loaded event instance</param>
        /// <param name="e">Shell page loaded event args</param>
        void ShellPage_Loaded(object? sender, EventArgs e)
        {
            var shellCurrentPage = (sender as Shell)?.CurrentPage;
            if (shellCurrentPage != null)
            {
                shellCurrentPage.Loaded -= OnMainPageLoaded;
                shellCurrentPage.Loaded += OnMainPageLoaded;
            }
        }

        /// <summary>
        /// Method for main page loaded event.
        /// </summary>
        /// <param name="sender">Loaded event instance</param>
        /// <param name="e">Main page loaded event args</param>
        void OnMainPageLoaded(object? sender, EventArgs e)
        {
            if (_popup != null)
            {
                _popup.IsOpen = true;
            }
        }

        /// <summary>
        /// Method to dismiss the picker popup view.
        /// </summary>
        void ClosePickerPopup()
        {
            if (_popup == null)
            {
                return;
            }

            _popup.IsOpen = false;
            _popup.ContentTemplate = new DataTemplate(() =>
            {
                return null;
            });

            ResetPopup();
        }

        /// <summary>
        /// Method to reset the popup and it was triggered while the mode changed to dialog to default.
        /// </summary>
        void ResetPopup()
        {
            if (_popup == null)
            {
                return;
            }

            _popup.IsOpen = false;
            _popup.Opened -= OnPopupOpened;
            _popup.Closed -= OnPopupClosed;
            _popup.Closing -= OnPopupClosing;
            Remove(_popup);
            _popup = null;
        }

        /// <summary>
        /// Method to show the picker popup based on the relative view and relative position.
        /// </summary>
        /// <param name="relativeView">The relative view.</param>
        /// <param name="relativePosition">The relative position.</param>
        void ShowRelativeToView(View relativeView, PickerRelativePosition relativePosition)
        {
            if (_popup == null)
            {
                return;
            }

            switch (relativePosition)
            {
                case PickerRelativePosition.AlignTop:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignTop);
                    break;

                case PickerRelativePosition.AlignBottom:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignBottom);
                    break;

                case PickerRelativePosition.AlignTopLeft:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignTopLeft);
                    break;

                case PickerRelativePosition.AlignTopRight:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignTopRight);
                    break;

                case PickerRelativePosition.AlignBottomLeft:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignBottomLeft);
                    break;

                case PickerRelativePosition.AlignBottomRight:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignBottomRight);
                    break;

                case PickerRelativePosition.AlignToLeftOf:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignToLeftOf);
                    break;

                case PickerRelativePosition.AlignToRightOf:
                    _popup.ShowRelativeToView(relativeView, PopupRelativePosition.AlignToRightOf);
                    break;
            }
        }

        /// <summary>
        /// Method to add the picker to popup.
        /// </summary>
        void AddPickerToPopup()
        {
            if (_popup == null)
            {
                _popup = new SfPopup();
                _popup.ShowHeader = false;
                _popup.ShowFooter = false;
                _popup.PopupStyle.CornerRadius = 5;
                _popup.Opened += OnPopupOpened;
                _popup.Closed += OnPopupClosed;
                _popup.Closing += OnPopupClosing;
                Add(_popup);
            }

            //// Here we have added the picker stack layout to the popup content if it is not null.
            if (_pickerStackLayout == null)
            {
                return;
            }

            _pickerStackLayout.Background = Background;
            _pickerStackLayout.BackgroundColor = BackgroundColor;
            _popup.ContentTemplate = new DataTemplate(() =>
            {
                return _pickerStackLayout;
            });
            UpdatePopupSize();
#if ANDROID
            //// While adding the picker to the popup, the picker selected item not updated properly in android. So, we have updated that.
            Dispatcher.Dispatch(() =>
            {
                _pickerContainer?.UpdateItemHeight();
            });
#endif
        }

        /// <summary>
        /// Method to update the popup size based on the column values.
        /// </summary>
        void UpdatePopupSize()
        {
            if (_popup == null)
            {
                return;
            }

            _popup.WidthRequest = PopupWidth <= 0 ? GetDefaultPopupWidth(this) : PopupWidth;
            _popup.HeightRequest = PopupHeight <= 0 ? GetDefaultPopupHeight(this) : PopupHeight;
        }

        /// <summary>
        /// Method triggered while the popup closing.
        /// </summary>
        /// <param name="sender">The popup instance.</param>
        /// <param name="e">Closing event argument.</param>
        void OnPopupClosing(object? sender, CancelEventArgs e)
        {
            e.Cancel = RaisePopupClosingEvent();
        }

        /// <summary>
        /// Method raises while the popup event closing.
        /// </summary>
        /// <returns>Returns whether to cancel closing of the popup.</returns>
        bool RaisePopupClosingEvent()
        {
            if (Closing != null)
            {
                CancelEventArgs popupClosingEventArgs = new CancelEventArgs();
                OnPopupClosing(popupClosingEventArgs);
                return popupClosingEventArgs.Cancel;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Method triggered while the popup closed.
        /// </summary>
        /// <param name="sender">The popup instance.</param>
        /// <param name="e">Closed event argument.</param>
        void OnPopupClosed(object? sender, EventArgs e)
        {
            IsOpen = false;
            OnPopupClosed(e);
        }

        /// <summary>
        /// Method triggered while the popup opened.
        /// </summary>
        /// <param name="sender">The popup instance.</param>
        /// <param name="e">Opened event argument.</param>
        void OnPopupOpened(object? sender, EventArgs e)
        {
            OnPopupOpened(e);
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>Returns layout size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            foreach (var child in Children)
            {
                child.Arrange(new Rect(bounds.Left, bounds.Top, bounds.Width, bounds.Height));
            }

            return bounds.Size;
        }

        /// <summary>
        /// Method used to measure the children based on width and height value.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>Returns maximum size of the layout.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            UpdateFormatBasedOnCulture();
            double width = double.IsFinite(widthConstraint) ? widthConstraint : _minWidth;
            double height = double.IsFinite(heightConstraint) ? heightConstraint : _minHeight;

#if MACCATALYST || IOS
            if (!_isPickerViewLoaded)
            {
                width = DesiredSize.Width <= 0 ? width : DesiredSize.Width;
                height = DesiredSize.Height <= 0 ? height : DesiredSize.Height;
                _isPickerViewLoaded = true;
            }
#endif

#if WINDOWS || ANDROID
            if (double.IsInfinity(heightConstraint) && HeightRequest == -1 && Mode == PickerMode.Default)
            {
                HeightRequest = height;
            }
#endif

#if IOS
            if (DesiredSize.Width == width && DesiredSize.Height == height && EnableLooping)
            {
                return new Size(width, height);
            }
#endif

            foreach (var child in Children)
            {
                child.Measure(width, height);
            }

            Size measuredSize = new Size(width, height);
            if (_availableSize != measuredSize)
            {
                _availableSize = measuredSize;
            }

            if (Mode != PickerMode.Default)
            {
                return Size.Zero;
            }

            return measuredSize;
        }

        /// <summary>
        /// Method triggers when the time picker property changed
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected override void OnPropertyChanged(string? propertyName = null)
        {
            if (propertyName == nameof(IsVisible) && Mode != PickerMode.Default)
            {
                //// If the picker is not visible, we have to close the picker popup only when it is opened previously.
                if (!IsVisible)
                {
                    _isPickerPreviouslyOpened = IsOpen;
                    if (_isPickerPreviouslyOpened)
                    {
                        ClosePickerPopup();
                    }
                }
                //// If the picker is visible, we have to open the picker popup only when it is opened initally.
                else if (IsOpen && IsVisible)
                {
                    AddPickerToPopup();
                    ShowPopup();
                }
                //// If the picker is visible, we have to open the picker popup only when it is opened previously.
                else if (IsVisible && _isPickerPreviouslyOpened)
                {
                    IsOpen = true;
                }
                else
                {
                    ClosePickerPopup();
                }
            }

            base.OnPropertyChanged(propertyName);
        }

        #endregion

        #region Virtual Methods

        /// <summary>
        /// Method to wire the events.
        /// </summary>
        protected virtual void Initialize()
        {
            // Wire events for header view properties.
            if (BaseHeaderView != null)
            {
                SetInheritedBindingContext(BaseHeaderView, BindingContext);
                BaseHeaderView.PickerPropertyChanged += OnHeaderSettingsPropertyChanged;
                if (BaseHeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(BaseHeaderView.TextStyle, BindingContext);
                    BaseHeaderView.TextStyle.PropertyChanged += OnHeaderTextStylePropertyChanged;
                }

                if (BaseHeaderView.SelectionTextStyle != null)
                {
                    SetInheritedBindingContext(BaseHeaderView.SelectionTextStyle, BindingContext);
                    BaseHeaderView.SelectionTextStyle.PropertyChanged += OnHeaderSelectionTextStylePropertyChanged;
                }

                PickerHelper.SetHeaderDynamicResource(this.BaseHeaderView, this);
            }

            // Wire events for footer view properties.
            if (FooterView != null)
            {
                SetInheritedBindingContext(FooterView, BindingContext);
                FooterView.PickerPropertyChanged += OnFooterSettingsPropertyChanged;
                if (FooterView.TextStyle != null)
                {
                    SetInheritedBindingContext(FooterView.TextStyle, BindingContext);
                    FooterView.TextStyle.PropertyChanged += OnFooterTextStylePropertyChanged;
                }

                PickerHelper.SetFooterDynamicResource(this.FooterView, this);
            }

            if (SelectedTextStyle != null)
            {
                SetInheritedBindingContext(SelectedTextStyle, BindingContext);
                SelectedTextStyle.PropertyChanged += OnSelectedTextStylePropertyChanged;
            }

            if (TextStyle != null)
            {
                SetInheritedBindingContext(TextStyle, BindingContext);
                TextStyle.PropertyChanged += OnUnSelectedTextStylePropertyChanged;
            }

            if (DisabledTextStyle != null)
            {
                SetInheritedBindingContext(DisabledTextStyle, BindingContext);
                DisabledTextStyle.PropertyChanged += OnDisabledTextStylePropertyChanged;
            }

            if (BaseColumns != null)
            {
                BaseColumns.CollectionChanged += OnColumnsCollectionChanged;
            }

            if (SelectionView != null)
            {
                PickerHelper.SetSelectionViewDynamicResource(this.SelectionView, this);
                SelectionView.PropertyChanged += OnSelectionViewPropertyChanged;
            }

            if (BaseColumnHeaderView != null)
            {
                SetInheritedBindingContext(BaseColumnHeaderView, BindingContext);
                BaseColumnHeaderView.PickerPropertyChanged += OnColumnHeaderViewPropertyChanged;
                if (BaseColumnHeaderView.TextStyle != null)
                {
                    SetInheritedBindingContext(BaseColumnHeaderView.TextStyle, BindingContext);
                    BaseColumnHeaderView.TextStyle.PropertyChanged += OnColumnHeaderTextStylePropertyChanged;
                }
            }

            PropertyChanged += OnPickerPropertyChanged;
            AddChildren();
        }

        /// <summary>
        /// Triggers while the popup opening or switched from popup to default.
        /// </summary>
        protected virtual void OnPickerLoading()
        {
        }

        /// <summary>
        /// Triggers while the header button clicked.
        /// </summary>
        /// <param name="index">Index of the header button.</param>
        protected virtual void OnHeaderButtonClicked(int index)
        {
        }

        /// <summary>
        /// Triggers when the picker popup closed.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnPopupClosed(EventArgs e)
        {
        }

        /// <summary>
        /// Triggers when the picker popup closing.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnPopupClosing(CancelEventArgs e)
        {
        }

        /// <summary>
        /// Triggers when the picker popup opened.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnPopupOpened(EventArgs e)
        {
        }

        /// <summary>
        /// Triggers when the ok button clicked.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnOkButtonClicked(EventArgs e)
        {
        }

        /// <summary>
        /// Triggers when the cancel button clicked.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected virtual void OnCancelButtonClicked(EventArgs e)
        {
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// Method to update the picker tapped item index and scroll view rendering based on the tapped item index.
        /// </summary>
        /// <param name="tappedIndex">The tapped index.</param>
        /// <param name="childIndex">The column child index.</param>
        /// <param name="isTapped">Is tap gesture used</param>
        /// <param name="isInitialLoading">Check whether is initial loading or not.</param>
        void IPicker.UpdateSelectedIndexValue(int tappedIndex, int childIndex, bool isTapped, bool isInitialLoading)
        {
            if (BaseColumns.Count <= childIndex)
            {
                return;
            }

            PickerColumn pickerColumn = BaseColumns[childIndex];

            //// Handles the scenario where the selected index of a picker column matches the tapped index,
            //// the selected item is null, it's not the initial loading phase, and the parent is an instance of SfDatePicker, SfTimePicker, or SfDateTimePicker.
            //// In such cases, it resets the selected date/time to the previous selected value
            //// to avoid triggering UI updates when setting the selected date/time as null during the initial loading phase.
            if (pickerColumn.SelectedIndex == tappedIndex && pickerColumn.SelectedItem == null && !isInitialLoading && pickerColumn.Parent is SfDatePicker or SfTimePicker or SfDateTimePicker)
            {
                if (pickerColumn.Parent is SfDatePicker datePicker)
                {
                    if (Mode == PickerMode.Default)
                    {
                        datePicker.SelectedDate = _previousSelectedDateTime.Date;
                    }
                }
                else if (pickerColumn.Parent is SfTimePicker timePicker)
                {
                    if (Mode == PickerMode.Default)
                    {
                        timePicker.SelectedTime = _previousSelectedDateTime.TimeOfDay;
                    }
                }
                else if (pickerColumn.Parent is SfDateTimePicker dateTimePicker)
                {
                    if (Mode == PickerMode.Default)
                    {
                        dateTimePicker.SelectedDate = _previousSelectedDateTime;
                    }
                }

                return;
            }

            if (pickerColumn.SelectedIndex == tappedIndex)
            {
                if ((IsScrollSelectionAllowed() && BaseColumns.Count == 1) && (pickerColumn.Parent is null || pickerColumn.Parent is SfPicker))
                {
                    if (pickerColumn._internalSelectedIndex != -1)
                    {
                        pickerColumn._internalSelectedIndex = -1;
                    }
                }

                return;
            }

            if ((IsScrollSelectionAllowed() && BaseColumns.Count == 1) && (pickerColumn.Parent == null || pickerColumn.Parent is SfPicker))
            {
                if (pickerColumn.SelectedIndex == -1)
                {
                    pickerColumn.SelectedIndex = tappedIndex;
                }
                else
                {
                    pickerColumn._internalSelectedIndex = tappedIndex;
                    if (isTapped)
                    {
                        _pickerContainer?.ScrollToSelectedIndex(pickerColumn._columnIndex, tappedIndex);
                    }
                }
            }
            else
            {
                pickerColumn.SelectedIndex = tappedIndex;
            }

            //// Call the template view for when selected value changed based on scroll the selected value.
            if (_headerLayout != null && BaseHeaderView.Height > 0 && HeaderTemplate != null)
            {
                _headerLayout?.InitializeTemplateView();
            }

            if (_columnHeaderLayout != null && BaseColumnHeaderView.Height > 0 && ColumnHeaderTemplate != null)
            {
                _columnHeaderLayout?.InitializeTemplateView();
            }

            if (_footerLayout != null && FooterView.Height > 0 && FooterTemplate != null)
            {
                _footerLayout?.InitializeTemplateView();
            }
        }

        /// <summary>
        /// Method to invoke the after confirm button clicked and it invokes the confirm button clicked event.
        /// </summary>
        void IFooterView.OnConfirmButtonClicked()
        {
            OnOkButtonClicked(new EventArgs { });
        }

        /// <summary>
        /// Method to invoke the after cancel button clicked and it invokes the cancel button clicked event.
        /// </summary>
        void IFooterView.OnCancelButtonClicked()
        {
            OnCancelButtonClicked(new EventArgs { });
        }

        /// <summary>
        /// Method to update the after date button clicked.
        /// </summary>
        void IHeaderView.OnDateButtonClicked()
        {
            OnHeaderButtonClicked(0);
        }

        /// <summary>
        /// Method to update the after time button clicked.
        /// </summary>
        void IHeaderView.OnTimeButtonClicked()
        {
            OnHeaderButtonClicked(1);
        }

        #endregion
    }
}