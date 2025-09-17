using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// This represents a class that contains information about the picker view.
    /// </summary>
    internal class PickerView : SfView, ITapGestureListener, IKeyboardListener
    {
        #region Fields

        /// <summary>
        /// The picker view info.
        /// </summary>
        readonly IPickerLayout _pickerLayoutInfo;

        /// <summary>
        /// The picker info.
        /// </summary>
        readonly IPicker _pickerViewInfo;

        /// <summary>
        /// The picker view port height.
        /// Here -2 values is used to restrict the initial viewportSize assigning.
        /// </summary>
        double _pickerViewPortHeight = -2;

        /// <summary>
        /// The selected index holds the current selected index based on the current scroll position.
        /// It is used to restricted the unwanted drawing of the item source element while scrolling.
        /// </summary>
        int _selectedIndex = 0;

        /// <summary>
        /// This initial node top position is used to store the starting position of first picker item of y axis to generate the picker item semantic node.
        /// </summary>
        double _initialNodeTopPosition = 0;

        /// <summary>
        /// The items source.
        /// </summary>
        ObservableCollection<string> _itemsSource;

        /// <summary>
        /// Holds the picker item drawing view based on picker view height.
        /// </summary>
        PickerDrawableView? _pickerDrawableView;

        /// <summary>
        /// Gets or sets the virtual picker items semantic nodes.
        /// </summary>
        List<SemanticsNode>? _semanticsNodes;

        /// <summary>
        /// Gets or sets the size of the semantic.
        /// </summary>
        Size _semanticsSize = Size.Zero;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerView"/> class.
        /// </summary>
        /// <param name="pickerLayoutInfo">The picker layout info.</param>
        /// <param name="itemsSource">The items source.</param>
        /// <param name="pickerViewInfo">The picker info.</param>
        internal PickerView(IPickerLayout pickerLayoutInfo, IPicker pickerViewInfo, ObservableCollection<string> itemsSource)
        {
            this.AddGestureListener(this);
            this.AddKeyboardListener(this);
            _pickerLayoutInfo = pickerLayoutInfo;
            _pickerViewInfo = pickerViewInfo;
            _itemsSource = itemsSource;
#if __IOS__ || __MACCATALYST__
            DrawingOrder = DrawingOrder.BelowContent;
#else
            DrawingOrder = DrawingOrder.AboveContentWithTouch;
#endif
#if WINDOWS
            //// If the SfView drawing canvas size exceeds MaximumBitmapSizeInPixels when adding more items,
            //// an OS limitation with CanvasImageSource size (refer: https://github.com/dotnet/maui/issues/3785)
            //// requires restricting the draw function when semantics or accessibility are used on SfView. This prevents OS limitation issues on the Windows platform.
            //// For accessibility, SfView should be enabled with AboveContentWithTouch to add a native user control and override the AutomationPeer.
            //// Virtualization isn't possible because automation peers must be added initially to access scrollable content.
            //// Since accessibility highlights are managed by native framework automation peers, SfView canvas drawing is unnecessary.
            IsCanvasNeeded = false;
#endif
            if (_pickerLayoutInfo.PickerInfo.ItemTemplate != null)
            {
                GeneratePickerItemsTemplate();
            }
            else
            {
                _pickerDrawableView = new PickerDrawableView(pickerLayoutInfo, this, itemsSource, GetStartingIndex);
                Add(_pickerDrawableView);
            }

            Focus();
#if IOS
            IgnoreSafeArea = true;
#endif
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to update the view port height while scroll view height is changed.
        /// </summary>
        /// <param name="viewPortHeight">The view port height.</param>
        /// <returns>Return true while the view port size changed.</returns>
        internal bool UpdatePickerViewPortHeight(double viewPortHeight)
        {
            bool isSizeUpdated = _pickerViewPortHeight != -2;
            if (_pickerViewPortHeight == viewPortHeight)
            {
                return false;
            }

            _pickerViewPortHeight = viewPortHeight;
            return isSizeUpdated;
        }

        /// <summary>
        /// Method to update the selected index.
        /// </summary>
        /// <param name="index">The index.</param>
        internal void UpdateSelectedIndexValue(int index)
        {
            if (index == _selectedIndex)
            {
                return;
            }

            _selectedIndex = index;

            if (_pickerLayoutInfo.PickerInfo.ItemTemplate != null)
            {
                return;
            }

            int maximumViewPortCount = GetMaximumViewPort();
            double maximumViewPortHeight = maximumViewPortCount * _pickerLayoutInfo.PickerInfo.ItemHeight;
            double viewPortItemCount = Math.Round(GetViewPortHeight() / _pickerLayoutInfo.PickerInfo.ItemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && _itemsSource.Count > viewPortItemCount;
            if (!enableLooping && maximumViewPortHeight + GetViewPortHeight() - _pickerLayoutInfo.PickerInfo.ItemHeight < Height)
            {
                ArrangeContent(new Rect(0, 0, Width, Height));
            }
            else if (enableLooping)
            {
                ArrangeContent(new Rect(0, 0, Width, Height));
            }

            _pickerDrawableView?.UpdateSelectedIndexValue(index);
        }

        /// <summary>
        /// Method to update the selected index on animation end.
        /// </summary>
        /// <param name="index">The index.</param>
        internal void UpdateSelectedIndexOnAnimationEnd(int index)
        {
            _selectedIndex = index;
            if (_pickerLayoutInfo.PickerInfo.ItemTemplate != null)
            {
                return;
            }

            int maximumViewPortCount = GetMaximumViewPort();
            double maximumViewPortHeight = maximumViewPortCount * _pickerLayoutInfo.PickerInfo.ItemHeight;
            double viewPortItemCount = Math.Round(GetViewPortHeight() / _pickerLayoutInfo.PickerInfo.ItemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && _itemsSource.Count > viewPortItemCount;
            if (!enableLooping && maximumViewPortHeight + GetViewPortHeight() - _pickerLayoutInfo.PickerInfo.ItemHeight < Height)
            {
                ArrangeContent(new Rect(0, 0, Width, Height));
            }
            else if (enableLooping)
            {
                ArrangeContent(new Rect(0, 0, Width, Height));
            }

            _pickerDrawableView?.UpdateSelectedIndexOnAnimationEnd(index);
        }

        /// <summary>
        /// Method to update the items source.
        /// </summary>
        /// <param name="itemsSource">The items source.</param>
        internal void UpdateItemsSource(ObservableCollection<string> itemsSource)
        {
            _itemsSource = itemsSource;
            if (_pickerLayoutInfo.PickerInfo.ItemTemplate == null)
            {
                _pickerDrawableView?.UpdateItemsSource(itemsSource);
                InvalidateMeasure();
            }
            else
            {
                DataTemplate template = _pickerLayoutInfo.PickerInfo.ItemTemplate;
                if (template is DataTemplateSelector)
                {
                    Children.Clear();
                    GeneratePickerItemsTemplate();
                    InvalidateMeasure();
                }
                else
                {
                    //// Here we are using the childCount variable to update binding context for only the needed views.
                    int childCount = 0;
                    if (_itemsSource.Count > Children.Count)
                    {
                        //// Here we are storing the count of the child before adding the new views.
                        //// And using this to update the binding context for the old views.
                        childCount = Children.Count;
                        for (int i = Children.Count; i < _itemsSource.Count; i++)
                        {
                            PickerItemDetails itemDetails = GetColumnDetails(i);
                            View view = PickerHelper.CreateTemplateView(template, itemDetails);
                            Add(view);
                        }
                    }
                    else if (_itemsSource.Count < Children.Count)
                    {
                        for (int i = Children.Count - 1; i >= _itemsSource.Count; i--)
                        {
                            Children.RemoveAt(i);
                        }

                        //// Here we are storing the count of the child after removing the views and using it to update the binding context for the old views.
                        childCount = Children.Count;
                    }

                    for (int i = 0; i < childCount; i++)
                    {
                        View view = (View)Children[i];
                        PickerItemDetails itemDetails = GetColumnDetails(i);
                        view.BindingContext = itemDetails;
                    }

                    InvalidateMeasure();
                }
            }
        }

        /// <summary>
        /// Method to update the picker item template changes.
        /// </summary>
        internal void UpdateItemTemplate()
        {
            if (_pickerLayoutInfo.PickerInfo.ItemTemplate != null)
            {
                if (_pickerDrawableView != null)
                {
                    Remove(_pickerDrawableView);
                }

                Children.Clear();
                GeneratePickerItemsTemplate();
                InvalidateMeasure();
            }
            else
            {
                Children.Clear();
                _pickerDrawableView = new PickerDrawableView(_pickerLayoutInfo, this, _itemsSource, GetStartingIndex);
                Add(_pickerDrawableView);
                InvalidateMeasure();
            }
        }

        /// <summary>
        /// Method to redraw the scroll view.
        /// </summary>
        internal void InvalidatePickerViewDraw()
        {
            _pickerDrawableView?.UpdatePickerViewDraw();
        }

        /// <summary>
        /// Method to update the picker item height.
        /// </summary>
        internal void UpdateItemHeight()
        {
            _pickerDrawableView?.UpdatePickerViewDraw();
            InvalidateMeasure();
        }

        /// <summary>
        /// Method to update the measure the picker view.
        /// </summary>
        internal void TriggerPickerViewMeasure()
        {
            InvalidateMeasure();
        }

        /// <summary>
        /// Method to update the Focus for the keyboard interaction.
        /// </summary>
        internal void UpdatePickerFocus()
        {
            Focus();
        }

        /// <summary>
        /// Method to get the picker item view port count.
        /// </summary>
        /// <returns>Returns the picker item count.</returns>
        internal double GetPickerItemViewPortCount()
        {
            var itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            return Math.Round(GetViewPortHeight() / itemHeight);
        }

        /// <summary>
        /// Method to get the valid view port height.
        /// </summary>
        /// <returns>Returns view port height.</returns>
        internal double GetViewPortHeight()
        {
            if (_pickerViewPortHeight <= 0)
            {
                return 0;
            }

            return _pickerViewPortHeight;
        }

        #endregion

        #region Private Methods

#if __IOS__ || __MACCATALYST__

        /// <summary>
        /// Method to set the focus for layout changed to other view. Focus removes while new view generation or inner view gets focus.
        /// Hence used delay to set focus.
        /// </summary>
        /// <param name="delay">The focus delay in milliseconds.</param>
        async void SetFocus(int delay)
        {
            await Task.Delay(delay);
            Focus();
        }

#endif

        /// <summary>
        /// Get the starting index.
        /// </summary>
        /// <returns>Return the starting index.</returns>
        int GetStartingIndex()
        {
            //// The item height.
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            //// The view port item count is calculated based on the view port height and item height.
            //// Assume the view port height is 109 and the item height is 20, While using Math.Round then the view port item count is 109 / 20 = 5.4 => 5(view port item count).
            //// Assume the view port height is 110 and the item height is 20, While using Math.Round then the view port item count is 110 / 20 = 5.5 => 6(view port item count).
            double viewPortItemCount = Math.Round(GetViewPortHeight() / itemHeight);
            //// The maximum view port count is 3 times the view port item count because larger view port leads to crash with larger canvas size.
            int maximumViewPortCount = (int)(viewPortItemCount * 3);
            //// Calculate the last index based on item source count and maximum view port count.
            double lastIndex = _itemsSource.Count - maximumViewPortCount;
            lastIndex = lastIndex < 0 ? 0 : lastIndex;
            //// Get the current scrolled item based on scroll offset.
            int selectedIndex = (int)(_pickerLayoutInfo.ScrollOffset / itemHeight);
            //// Calculate the virtualized drawable view start index based on view port count and selected index.
            double startingIndex = selectedIndex - viewPortItemCount;
            startingIndex = startingIndex < 0 ? 0 : startingIndex;
            startingIndex = startingIndex > lastIndex ? lastIndex : startingIndex;
            return (int)startingIndex;
        }

        /// <summary>
        /// Get the maximum view port count.
        /// </summary>
        /// <returns>Return the maximum view port count.</returns>
        int GetMaximumViewPort()
        {
            //// The item height.
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            //// The view port item count is calculated based on the view port height and item height.
            //// Assume the view port height is 109 and the item height is 20, While using Math.Round then the view port item count is 109 / 20 = 5.4 => 5(view port item count).
            //// Assume the view port height is 110 and the item height is 20, While using Math.Round then the view port item count is 110 / 20 = 5.5 => 6(view port item count).
            double viewPortItemCount = Math.Round(GetViewPortHeight() / itemHeight);
            //// The maximum view port count is 3 times the view port item count because larger view port leads to crash with larger canvas size.
            int maximumViewPortCount = (int)(viewPortItemCount * 3);
            return maximumViewPortCount;
        }

        /// <summary>
        /// Method to generate the picker items template.
        /// </summary>
        void GeneratePickerItemsTemplate()
        {
            DataTemplate template = _pickerLayoutInfo.PickerInfo.ItemTemplate;
            double currentViewPortCount = GetPickerItemViewPortCount();
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && _itemsSource.Count > currentViewPortCount;
            if (enableLooping)
            {
                int index = 0;
                if (currentViewPortCount % 2 == 0)
                {
                    index = _selectedIndex - (int)Math.Floor(3 * currentViewPortCount / 2) + 1;
                }
                else
                {
                    index = _selectedIndex - (int)Math.Floor(3 * currentViewPortCount / 2);
                }

                if (index < 0)
                {
                    index += _itemsSource.Count;
                    if (index < 0)
                    {
                        index += _itemsSource.Count;
                    }
                }

                double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
                int totalItemsToDraw = (int)((currentViewPortCount * 3) * itemHeight / itemHeight);
                int itemsDrawn = 0;

                while (itemsDrawn < totalItemsToDraw)
                {
                    if (template is DataTemplateSelector itemTemplateSelector)
                    {
                        PickerItemDetails itemDetails = GetColumnDetails(index);
                        DataTemplate templateSelector = itemTemplateSelector.SelectTemplate(itemDetails, this);
                        CreatePickerTemplateView(templateSelector, itemDetails);
                    }
                    else
                    {
                        PickerItemDetails itemDetails = GetColumnDetails(index);
                        CreatePickerTemplateView(template, itemDetails);
                    }

                    index++;
                    if (index >= _itemsSource.Count)
                    {
                        index = 0;
                    }

                    itemsDrawn++;
                }
            }
            else
            {
                if (template is DataTemplateSelector itemTemplateSelector)
                {
                    for (int index = 0; index < _itemsSource.Count; index++)
                    {
                        PickerItemDetails itemDetails = GetColumnDetails(index);
                        DataTemplate templateSelector = itemTemplateSelector.SelectTemplate(itemDetails, this);
                        CreatePickerTemplateView(templateSelector, itemDetails);
                    }
                }
                else
                {
                    for (int index = 0; index < _itemsSource.Count; index++)
                    {
                        PickerItemDetails itemDetails = GetColumnDetails(index);
                        CreatePickerTemplateView(template, itemDetails);
                    }
                }
            }
        }

        /// <summary>
        /// Method to create a template view for the picker items.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="itemDetails">The item details.</param>
        void CreatePickerTemplateView(DataTemplate template, PickerItemDetails itemDetails)
        {
            var itemTemplateView = PickerHelper.CreateTemplateView(template, itemDetails);
            Add(itemTemplateView);
        }

        /// <summary>
        /// Method to get the item details.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The item details based on the index.</returns>
        PickerItemDetails GetColumnDetails(int index)
        {
            PickerItemDetails columnDetails = new PickerItemDetails(_itemsSource[index]);
            return columnDetails;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to measures child elements size in a container, item source item is measured with given width and height constraints.
        /// </summary>
        /// <param name="widthConstraint">The maximum width request of the layout.</param>
        /// <param name="heightConstraint">The maximum height request of the layout.</param>
        /// <returns>Returns the maximum size of the scroll view size.</returns>
        protected override Size MeasureContent(double widthConstraint, double heightConstraint)
        {
            //// The item height.
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            if (_pickerLayoutInfo.PickerInfo.ItemTemplate != null)
            {
                foreach (var child in Children)
                {
                    child.Measure(widthConstraint, itemHeight);
                }
            }

            //// The total picker height is calculated based on the item source count and item height.
            //// Assume the item source count is 5 and item height is 20, then the total picker height is 5 * 20 = 100.
            double totalPickerHeight = _itemsSource.Count * itemHeight;
            double viewPortHeight = GetViewPortHeight();
            //// Total height of the layout based on top and bottom empty space.
            double totalHeight = totalPickerHeight + viewPortHeight - itemHeight;

            //// The view port item count is calculated based on the view port height and item height.
            double viewPortItemCount = Math.Round(viewPortHeight / itemHeight);
            //// The maximum view port count is 3 times the view port item count because larger view port leads to crash with larger canvas size.
            int maximumViewPortCount = (int)(viewPortItemCount * 3);
            //// The maximum items height based on the maximum view port count.
            double pickerHeight = maximumViewPortCount * itemHeight;
            //// Ensure the valid looping or not.
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && viewPortItemCount < _itemsSource.Count;
            if (enableLooping && DesiredSize.Width != widthConstraint && _pickerLayoutInfo.PickerInfo.ItemTemplate != null)
            {
                UpdateItemTemplate();
            }

            foreach (var child in Children)
            {
                if (!(child is PickerDrawableView))
                {
                    continue;
                }

                if (totalHeight > pickerHeight)
                {
                    child.Measure(widthConstraint, pickerHeight);
                }
                else
                {
                    child.Measure(widthConstraint, totalPickerHeight);
                }
            }

            //// Need to add the empty space to the picker view. So by adding the picker view port height and it subtracted by single item height then we get total picker scroll view height.
            //// Need to show space before 0th item and after last item for showing the selected item at middle position.
            //// So that the total picker scroll view height is calculated by adding the picker view port height and it subtracted by single item height.
            DesiredSize = new Size(widthConstraint, enableLooping ? totalHeight + (viewPortHeight * 2) : totalHeight);
            return new Size(widthConstraint, enableLooping ? totalHeight + (viewPortHeight * 2) : totalHeight);
        }

        /// <summary>
        /// Method used to arrange the children with in the bounds.
        /// </summary>
        /// <param name="bounds">The size of the layout.</param>
        /// <returns>The layout size.</returns>
        protected override Size ArrangeContent(Rect bounds)
        {
            //// The item height.
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            //// The view port item count is calculated based on the view port height and item height.
            //// Assume the view port height is 109 and the item height is 20, While using Math.Round then the view port item count is 109 / 20 = 5.4 => 5(view port item count).
            //// Assume the view port height is 110 and the item height is 20, While using Math.Round then the view port item count is 110 / 20 = 5.5 => 6(view port item count).
            double viewPortItemCount = Math.Round(GetViewPortHeight() / itemHeight);
            //// The maximum view port count is 3 times the view port item count because larger view port leads to crash with larger canvas size.
            int maximumViewPortCount = (int)(viewPortItemCount * 3);
            //// The maximum items height based on the maximum view port count.
            double pickerHeight = maximumViewPortCount * itemHeight;
            //// Ensure the valid looping or not.
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && viewPortItemCount < _itemsSource.Count;
            //// The drawingStartIndex is calculated based on the view port item count.
            //// From above example the view port item count is 5, then the drawingStartIndex is 2. While using the Math.Ceiling the drawingStartIndex is 5/2 = 2.5 => 3.
            int drawingStartIndex = (int)Math.Ceiling(viewPortItemCount / 2) - 1;
            //// The start position is calculated based on drawingStartIndex value.
            //// From above example the drawingStartIndex is 2 and item height is 20, then the start y position is 40.
            double yPosition;

#if ANDROID || WINDOWS
            yPosition = _pickerLayoutInfo.Column.SelectedIndex <= -1
            ? (drawingStartIndex + 1) * itemHeight
            : drawingStartIndex * itemHeight;
#else
            yPosition = drawingStartIndex * itemHeight;
#endif
            //// Calculate the last index based on item source count and maximum view port count.
            double lastIndex = _itemsSource.Count - maximumViewPortCount;
            lastIndex = lastIndex < 0 ? 0 : lastIndex;
            double maxPosition = (lastIndex * itemHeight) + yPosition;
            if (enableLooping)
            {
                yPosition = enableLooping ? _pickerLayoutInfo.ScrollOffset - (Math.Floor(2 * viewPortItemCount / 2) * itemHeight) : yPosition;
            }

            foreach (var child in Children)
            {
                if (child is PickerDrawableView)
                {
                    if (enableLooping)
                    {
                        double scrollPosition = _selectedIndex * itemHeight;
                        if (scrollPosition < (viewPortItemCount * itemHeight) + itemHeight && scrollPosition >= 0)
                        {
                            //// Adjust the scroll end position by adding the total height of all items to the current scroll end position.
                            scrollPosition = (_itemsSource.Count * itemHeight) + scrollPosition;
                        }

                        //// Adjust the scroll position based on the current scroll offset.
                        double difference = Math.Abs(scrollPosition - _pickerLayoutInfo.ScrollOffset);
                        if (difference > itemHeight)
                        {
                            scrollPosition = _selectedIndex * itemHeight;
                        }

                        _initialNodeTopPosition = scrollPosition;
                        child.Arrange(new Rect(0, scrollPosition, bounds.Width, lastIndex != 0 ? pickerHeight : _itemsSource.Count * itemHeight));
                    }
                    else
                    {
                        if (lastIndex != 0)
                        {
                            //// Get the current scrolled item based on scroll offset.
                            int selectedIndex = (int)(_pickerLayoutInfo.ScrollOffset / itemHeight);
                            //// Calculate the virtualized drawable view start index based on view port count and selected index.
                            double startingIndex = selectedIndex - viewPortItemCount;
                            startingIndex = startingIndex < 0 ? 0 : startingIndex;
                            //// Does not need to move after the last index value.
                            startingIndex = startingIndex > lastIndex ? lastIndex : startingIndex;
                            double startPosition = startingIndex * itemHeight;
                            double topPosition = startPosition + yPosition;
                            if (topPosition > maxPosition)
                            {
                                topPosition = maxPosition;
                            }

                            _initialNodeTopPosition = topPosition;
                            child.Arrange(new Rect(0, topPosition, bounds.Width, pickerHeight));
                        }
                        else
                        {
                            //// Arrange the drawable view only for item source items without top and bottom empty spacing.
                            child.Arrange(new Rect(0, yPosition, bounds.Width, _itemsSource.Count * itemHeight));
                            _initialNodeTopPosition = yPosition;
                        }
                    }
                }
                else
                {
                    child.Arrange(new Rect(0, yPosition, bounds.Width, itemHeight));
                    yPosition += itemHeight;
                }
            }

            return bounds.Size;
        }

        /// <summary>
        /// Method to create the semantics node for each items in the picker items.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>Returns semantic virtual view.</returns>
        protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
        {
            Size newSize = new Size(width, height);

            _semanticsNodes = new List<SemanticsNode>();
            _semanticsSize = newSize;
            //// The item height.
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            double yPosition = _initialNodeTopPosition;
            int startIndex = GetStartingIndex();
            double viewPortItemCount = Math.Round(GetViewPortHeight() / itemHeight);
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && _itemsSource.Count > viewPortItemCount;
            if (enableLooping)
            {
                if (viewPortItemCount % 2 == 0)
                {
                    startIndex = _selectedIndex - (int)Math.Floor(viewPortItemCount / 2) + 1;
                }
                else
                {
                    startIndex = _selectedIndex - (int)Math.Floor(viewPortItemCount / 2);
                }

                //// For looping mode, we create nodes based on the current scroll position.
                double currentYPosition = _pickerLayoutInfo.ScrollOffset;
                for (int i = 0; i <= (_itemsSource.Count * 3); i++)
                {
                    int adjustedIndex = (startIndex + i) % _itemsSource.Count;
                    if (adjustedIndex < 0)
                    {
                        adjustedIndex += _itemsSource.Count;
                    }

                    double totalContentHeight = _itemsSource.Count * itemHeight;
                    //// When scrolling upward, update the scroll position based on the total content height.
                    if (currentYPosition < (viewPortItemCount * itemHeight))
                    {
                        currentYPosition = totalContentHeight;
                    }
                    //// When scrolling downward, update the scroll position based on the total content height.
                    else if (currentYPosition >= totalContentHeight + (viewPortItemCount * itemHeight))
                    {
                        currentYPosition = currentYPosition - totalContentHeight;
                    }

                    //// Create rectangle based on the current yPosition.
                    Rect rectangle = new Rect(0, currentYPosition, width, itemHeight);

                    SemanticsNode node = new SemanticsNode()
                    {
                        Text = _itemsSource[adjustedIndex],
                        Bounds = rectangle,
                        Id = adjustedIndex,
                    };
                    _semanticsNodes.Add(node);
                    //// The yPosition is adjusted based on the item height.
                    currentYPosition += itemHeight;
                }
            }
            else
            {
                //// Draw item source item based on the item source collection.
                for (int i = startIndex; i < _itemsSource.Count; i++)
                {
                    Rect rectangle = new Rect(0, yPosition, width, itemHeight);
                    //// The yPosition is adjusted based on the item height.
                    //// Assume the item height is 20, then the yPosition is 40. So that the next item is drawn from 60.
                    yPosition += itemHeight;
                    if (yPosition >= height)
                    {
                        break;
                    }

                    SemanticsNode node = new SemanticsNode()
                    {
                        Text = _itemsSource[i],
                        Bounds = rectangle,
                        Id = i,
                    };
                    _semanticsNodes.Add(node);
                }
            }

            return _semanticsNodes;
        }

        /// <summary>
        /// Method handle the scroll position of the picker while accessibility enabled.
        /// </summary>
        /// <param name="node">The semantic node.</param>
        protected override void ScrollToCore(SemanticsNode node)
        {
            if (Parent != null && Parent != null && Parent is PickerScrollView)
            {
                PickerScrollView scrollView = (PickerScrollView)Parent;
                double scrollHeight = scrollView.Height;
                double nodeHeight = node.Bounds.Height;
                double nodeTopPosition = node.Bounds.Top;
                double scrollYPosition = scrollView.ScrollY;
                double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
                double viewPortItemCount = Math.Round(GetViewPortHeight() / itemHeight);
                bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && _itemsSource.Count > viewPortItemCount;
                double maxScrollPosition = scrollView.ContentSize.Height - scrollView.Height;
                if (!enableLooping)
                {
                    if ((nodeTopPosition > scrollYPosition && nodeTopPosition < scrollYPosition + scrollHeight) ||
                        (nodeTopPosition + nodeHeight > scrollYPosition && nodeTopPosition + nodeHeight < scrollYPosition + scrollHeight))
                    {
                        return;
                    }

                    if (maxScrollPosition > node.Bounds.Top)
                    {
                        scrollView.ScrollToAsync(0, node.Bounds.Top, false);
                    }
                    else
                    {
                        scrollView.ScrollToAsync(0, maxScrollPosition, false);
                    }
                }
                else
                {
                    if (nodeTopPosition >= scrollYPosition && nodeTopPosition <= scrollYPosition + scrollHeight - nodeHeight)
                    {
                        return;
                    }

                    scrollView.ScrollToAsync(0, nodeTopPosition, false);
                }
            }
        }

        #endregion

        #region Interface Implementation

#if __IOS__ || __MACCATALYST__

        /// <summary>
        /// Gets a value indicating whether the view can become the first responder to listen the keyboard actions.
        /// </summary>
        /// <remarks>This property will be considered only in maccatalyst and iOS Platform.</remarks>
        bool IKeyboardListener.CanBecomeFirstResponder
        {
            get
            {
                return true;
            }
        }

#endif

        /// <summary>
        /// Method to handle the tap event.
        /// </summary>
        /// <param name="e">The tap event arguments.</param>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
#if __IOS__ || __MACCATALYST__
            SetFocus(10);
#endif
            //// The item height.
            double itemHeight = _pickerLayoutInfo.PickerInfo.ItemHeight;
            //// The view port item count is calculated based on the view port height and item height.
            //// Assume the view port height is 109 and the item height is 20, While using Math.Round then the view port item count is 109 / 20 = 5.4 => 5(view port item count).
            //// Assume the view port height is 110 and the item height is 20, While using Math.Round then the view port item count is 110 / 20 = 5.5 => 6(view port item count).
            double viewPortItemCount = Math.Round(GetViewPortHeight() / itemHeight);
            int itemCount = _itemsSource.Count;
            bool enableLooping = _pickerLayoutInfo.PickerInfo.EnableLooping && itemCount > viewPortItemCount;
            //// The drawingStartIndex is calculated based on the view port item count.
            //// From above example the view port item count is 5, then the drawingStartIndex is 2. While using the Math.Ceiling the drawingStartIndex is 5/2 = 2.5 => 3.
            int drawingStartIndex = (int)Math.Ceiling(viewPortItemCount / 2) - 1;
            //// The firstItemYPosition is calculated based on drawingStartIndex value.
            //// From above example the drawingStartIndex is 2 and item height is 20, then the yPosition is 40.
            double firstItemYPosition = drawingStartIndex * itemHeight;
            //// The selected index.
            int selectedIndex;
            //// The selected index is calculated based on the tap point.
            //// If the tap point is less than firstItemYPosition then need to move the scroll view to first item.
            //// It means tapped point is before the first item then need to render first item as selected item.
            if (e.TapPoint.Y < firstItemYPosition)
            {
                int offset = (int)((firstItemYPosition - e.TapPoint.Y) / itemHeight) + 1;
                selectedIndex = enableLooping ? (itemCount - offset) % itemCount : 0;
            }
            //// The tapped point is after the last item then need to render last item as selected item.
            else if (!enableLooping && e.TapPoint.Y > Height - firstItemYPosition - itemHeight)
            {
                selectedIndex = _itemsSource.Count;
            }
            else
            {
                //// Assume tap y position is 100 and first item y position is 50 and item height is 10.
                //// Then the selected index is 5 => (100 - 50) / 10.
                selectedIndex = (int)((e.TapPoint.Y - firstItemYPosition) / itemHeight);
            }

            //// While looping, check the selected index value.
            if (enableLooping)
            {
                selectedIndex = selectedIndex % itemCount;
                if (selectedIndex < 0)
                {
                    selectedIndex = 0;
                }
            }
            else
            {
                selectedIndex = _pickerLayoutInfo.Column.SelectedIndex < 0 ? selectedIndex - 1 : selectedIndex;
            }

            _pickerLayoutInfo.UpdateSelectedIndexValue(selectedIndex, true);
        }

        /// <summary>
        /// Method to handle the keyboard interaction.
        /// </summary>
        /// <param name="args">The keyboard event args.</param>
        void IKeyboardListener.OnKeyDown(KeyEventArgs args)
        {
            if (args.Key == KeyboardKey.Tab)
            {
                Focus();
            }
            else if (args.Key == KeyboardKey.Up)
            {
                if (_selectedIndex > 0)
                {
                    _pickerLayoutInfo.UpdateSelectedIndexValue(_selectedIndex - 1, false);
                }
            }
            else if (args.Key == KeyboardKey.Down)
            {
                _pickerLayoutInfo.UpdateSelectedIndexValue(_selectedIndex + 1, false);
            }
            else if (args.Key == KeyboardKey.Right || args.Key == KeyboardKey.Left)
            {
                Unfocus();
                PickerContainer? pickerContainer = Parent.Parent.Parent as PickerContainer;
                pickerContainer?.PickerRightLeftKeys(args, _pickerLayoutInfo.Column._columnIndex);
            }
            else if (args.Key == KeyboardKey.Enter)
            {
                if (_pickerViewInfo.IsOpen == true)
                {
                    _pickerViewInfo.IsOpen = false;
                }
            }
            else if (args.Key == KeyboardKey.Escape)
            {
                _pickerViewInfo.IsOpen = false;
            }
        }

        /// <summary>
        /// The keyboard interaction override method.
        /// </summary>
        /// <param name="args">The keyboard event args.</param>
        void IKeyboardListener.OnKeyUp(KeyEventArgs args)
        {
        }

        #endregion
    }
}