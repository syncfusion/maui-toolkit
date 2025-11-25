using Syncfusion.Maui.Toolkit.Helper;

namespace Syncfusion.Maui.Toolkit.TabView
{
    /// <summary>
    /// Represents the material design visual style for a <see cref="SfTabView"/> control.
    /// </summary>
    internal partial class TabViewMaterialVisualStyle : SfGrid
    {
        #region Fields

        SfImage? _image;
        SfLabel? _header;
        SfHorizontalStackLayout? _horizontalLayout;
        SfVerticalStackLayout? _verticalLayout;
		View? _headerContent;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="HeaderDisplayMode"/> bindable property.
        /// </summary>
        public static readonly BindableProperty HeaderDisplayModeProperty =
           BindableProperty.Create(
               nameof(HeaderDisplayMode),
               typeof(TabBarDisplayMode),
               typeof(TabViewMaterialVisualStyle),
               TabBarDisplayMode.Default,
               propertyChanged: OnHeaderDisplayModePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ImagePosition"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ImagePositionProperty =
           BindableProperty.Create(
               nameof(ImagePosition),
               typeof(TabImagePosition),
               typeof(TabViewMaterialVisualStyle),
               TabImagePosition.Top,
               propertyChanged: OnImagePositionPropertyChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value that defines the header display mode.
        /// </summary>
        public TabBarDisplayMode HeaderDisplayMode
        {
            get => (TabBarDisplayMode)GetValue(HeaderDisplayModeProperty);
            set => SetValue(HeaderDisplayModeProperty, value);
        }

        /// <summary>
        /// Gets or sets the value that defines the image position.
        /// </summary>
        public TabImagePosition ImagePosition
        {
            get => (TabImagePosition)GetValue(ImagePositionProperty);
            set => SetValue(ImagePositionProperty, value);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TabViewMaterialVisualStyle"/> class.
        /// </summary>
        public TabViewMaterialVisualStyle()
        {
            IsClippedToBounds = true;

            VerticalOptions = LayoutOptions.Fill;
            HorizontalOptions = LayoutOptions.Fill;

            Margin = new Thickness(0, 0, 0, 0);

            InitializeLayout();
            InitializeIcon();
            InitializeHeader();
            UpdateLayout();
            ResetStyle();
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Invoked whenever the parent of an element is set.
        /// </summary>
        protected override void OnParentSet()
        {
            if (Parent != null)
            {
                Parent.PropertyChanged -= OnParentPropertyChanged;
            }

            base.OnParentSet();
            SetBinding();
			SetHeaderContent();

            if (Parent != null)
            {
                Parent.PropertyChanged += OnParentPropertyChanged;
            }
        }

		#endregion

		#region Property Changed Implementation

		/// <summary>
		/// Handles property changes of the parent control, updating the current control accordingly.
		/// </summary>
		void OnParentPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is SfTabItem item && item != null)
            {
                if (e.PropertyName == nameof(SfTabItem.HeaderDisplayMode))
                {
                    HeaderDisplayMode = item.HeaderDisplayMode;
                }
                if (e.PropertyName == nameof(SfTabItem.TabWidthMode) ||
                    e.PropertyName == nameof(SfTabItem.HeaderDisplayMode) ||
                    e.PropertyName == nameof(SfTabItem.HeaderDisplayMode) ||
                    e.PropertyName == nameof(SfTabItem.ImagePosition) ||
                    e.PropertyName == nameof(SfTabItem.HeaderHorizontalTextAlignment))
                {
                    UpdateHorizontalOptions(item);
                    UpdateHeaderHorizontalOptions(item);
                }
				if (e.PropertyName == nameof(SfTabItem.HeaderContent))
				{
					UpdateHeaderContent(item);
				}
			}
        }

        static void OnHeaderDisplayModePropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabViewMaterialVisualStyle)?.UpdateHeaderDisplayMode();

        static void OnImagePositionPropertyChanged(BindableObject bindable, object oldValue, object newValue) => (bindable as TabViewMaterialVisualStyle)?.UpdateImagePosition();

        #endregion

        #region Private Methods

        /// <summary>
        /// Resets the styles of the control and its child elements to their default values.
        /// </summary>
        void ResetStyle()
        {
            Style = new Style(typeof(SfGrid));

            if (_horizontalLayout != null)
            {
                _horizontalLayout.Style = new Style(typeof(SfHorizontalStackLayout));
            }

            if (_verticalLayout != null)
            {
                _verticalLayout.Style = new Style(typeof(SfVerticalStackLayout));
            }

            if (_image != null)
            {
                _image.Style = new Style(typeof(SfImage));
            }

            if (_header != null)
            {
                _header.Style = new Style(typeof(SfLabel));
            }
        }

        /// <summary>
        /// Initializes the horizontal and vertical layouts for the control.
        /// </summary>
        void InitializeLayout()
        {
            _horizontalLayout = new SfHorizontalStackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill,
				Spacing = 0,
			};

            _verticalLayout = new SfVerticalStackLayout()
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
				Spacing = 0,
			};

            Children.Add(_verticalLayout);
        }

        /// <summary>
        /// Initializes the image icon for the tab and adds it to the vertical layout.
        /// </summary>
        void InitializeIcon()
        {
            _image = new SfImage()
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            _verticalLayout?.Children.Add(_image);
        }

        /// <summary>
        /// Initializes the header label for the tab and adds it to the vertical layout.
        /// </summary>
        void InitializeHeader()
        {
            _header = new SfLabel()
            {
                LineBreakMode = LineBreakMode.TailTruncation,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontAutoScalingEnabled = false,
            };

            _verticalLayout?.Children.Add(_header);
        }

        /// <summary>
        /// Updates the layout based on the current <see cref="HeaderDisplayMode"/>  and <see cref="ImagePosition"/>.
        /// </summary>
        void UpdateLayout()
        {
            ClearAllChildren();

			if (_headerContent != null)
			{
				Children.Add(_headerContent);
				return;
			}

			if (HeaderDisplayMode == TabBarDisplayMode.Text)
            {
                if (_header != null)
                {
                    Children.Add(_header);
                }
            }
            else if (HeaderDisplayMode == TabBarDisplayMode.Image)
            {
                if (_image != null)
                {
                    Children.Add(_image);
                }
            }
            else
            {
                if (ImagePosition == TabImagePosition.Top || ImagePosition == TabImagePosition.Bottom)
                {
                    UpdateVerticalLayout();
                }
                else
                {
                    UpdateHorizontalLayout();
                }
            }
        }

        /// <summary>
        /// Clears all child elements from the main control and its layouts.
        /// </summary>
        void ClearAllChildren()
        {
            Children?.Clear();
            _horizontalLayout?.Clear();
            _verticalLayout?.Clear();
        }

        /// <summary>
        /// Updates the vertical layout based on the current <see cref="ImagePosition"/>.
        /// </summary>
        void UpdateVerticalLayout()
        {
            if (_verticalLayout == null)
			{
				return;
			}

			if (ImagePosition == TabImagePosition.Top)
            {
                _verticalLayout.Add(_image);
                _verticalLayout.Add(_header);
            }
            else
            {
                _verticalLayout.Add(_header);
                _verticalLayout.Add(_image);
            }
            Children.Add(_verticalLayout);
        }

        /// <summary>
        /// Updates the horizontal layout based on the current <see cref="ImagePosition"/>.
        /// </summary>
        void UpdateHorizontalLayout()
        {
            if (_horizontalLayout == null)
			{
				return;
			}

			if (ImagePosition == TabImagePosition.Left)
            {
                _horizontalLayout.Add(_image);
                _horizontalLayout.Add(_header);
            }
            else
            {
                _horizontalLayout.Add(_header);
                _horizontalLayout.Add(_image);
            }
            Children.Add(_horizontalLayout);
        }

        /// <summary>
        /// Sets up data bindings for the control and its child elements.
        /// </summary>
        void SetBinding()
        {
            BindingContext = Parent;
            _image?.SetBinding(SfImage.SourceProperty, BindingHelper.CreateBinding(nameof(SfTabItem.ImageSource), getter: static (SfTabItem item) => item.ImageSource));

            SetHeaderBinding();

            this.SetBinding(TabViewMaterialVisualStyle.ImagePositionProperty, BindingHelper.CreateBinding(nameof(SfTabItem.ImagePosition), getter: static (SfTabItem item) => item.ImagePosition, mode: BindingMode.TwoWay));
            _horizontalLayout?.SetBinding(StackBase.SpacingProperty, BindingHelper.CreateBinding(nameof(SfTabItem.ImageTextSpacing), getter: static (SfTabItem item) => item.ImageTextSpacing));
            _verticalLayout?.SetBinding(StackBase.SpacingProperty, BindingHelper.CreateBinding(nameof(SfTabItem.ImageTextSpacing), getter: static (SfTabItem item) => item.ImageTextSpacing));
			_image?.SetBinding(SfImage.WidthRequestProperty, BindingHelper.CreateBinding(nameof(SfTabItem.ImageSize), getter: static (SfTabItem item) => item.ImageSize));
			_image?.SetBinding(SfImage.HeightRequestProperty, BindingHelper.CreateBinding(nameof(SfTabItem.ImageSize), getter: static (SfTabItem item) => item.ImageSize));
		}

        void SetHeaderBinding()
        {
            if (_header != null)
            {
                _header.SetBinding(SfLabel.TextProperty, BindingHelper.CreateBinding(nameof(SfTabItem.Header), getter: static (SfTabItem item) => item.Header, mode: BindingMode.OneWay));
                _header.SetBinding(SfLabel.TextColorProperty, BindingHelper.CreateBinding(nameof(SfTabItem.TextColor), getter: static (SfTabItem item) => item.TextColor));
                _header.SetBinding(SfLabel.FontSizeProperty, BindingHelper.CreateBinding(nameof(SfTabItem.FontSize), getter: static (SfTabItem item) => item.FontSize));
                _header.SetBinding(SfLabel.FontAttributesProperty, BindingHelper.CreateBinding(nameof(SfTabItem.FontAttributes), getter: static (SfTabItem item) => item.FontAttributes));
                _header.SetBinding(SfLabel.FontFamilyProperty, BindingHelper.CreateBinding(nameof(SfTabItem.FontFamily), getter: static (SfTabItem item) => item.FontFamily));
                _header.SetBinding(SfLabel.FontAutoScalingEnabledProperty, BindingHelper.CreateBinding(nameof(SfTabItem.FontAutoScalingEnabled), getter: static (SfTabItem item) => item.FontAutoScalingEnabled));
            }
        }

		/// <summary>
		/// Sets up for HeaderContent property.
		/// </summary>
		void SetHeaderContent()
		{
			if (Parent is SfTabItem tabItem)
			{
				UpdateHeaderContent(tabItem);
			}
		}

		/// <summary>
		/// Updates the HeaderContent display.
		/// </summary>
		void UpdateHeaderContent(SfTabItem tabItem)
		{
			if (tabItem is not null)
			{
				if (tabItem.HeaderContent is not null)
				{
					if (tabItem.HeaderContent.BindingContext is null)
					{
						tabItem.HeaderContent.BindingContext = tabItem.BindingContext;
					}
					_headerContent = tabItem.HeaderContent;
				}
				else
				{
					Children.Remove(_headerContent);
					_headerContent = null;
				}

				UpdateLayout();
			}
		}

		/// <summary>
		/// Updates the layout in response to a change in the image position.
		/// </summary>
		void UpdateImagePosition()
        {
            UpdateLayout();
        }

        /// <summary>
        /// Updates the layout in response to a change in the header display mode.
        /// </summary>
        void UpdateHeaderDisplayMode()
        {
            UpdateLayout();
        }

        /// <summary>
        /// Updates the horizontal options of the control based on the current <see cref="SfTabItem"/>.
        /// </summary>
        void UpdateHorizontalOptions(SfTabItem item)
        {
            if (_horizontalLayout != null && _image != null)
            {
                var options = TabViewMaterialVisualStyle.GetLayoutOptions(item.HeaderHorizontalTextAlignment);
                _horizontalLayout.HorizontalOptions = options;
                _image.HorizontalOptions = options;
            }
        }

		static LayoutOptions GetLayoutOptions(TextAlignment alignment)
        {
			return alignment switch
			{
				TextAlignment.Center => LayoutOptions.Center,
				TextAlignment.End => LayoutOptions.End,
				_ => LayoutOptions.Start,
			};
		}

        /// <summary>
        /// Updates the horizontal options of the header label based on the current <see cref="SfTabItem"/>.
        /// </summary>
        void UpdateHeaderHorizontalOptions(SfTabItem item)
        {
            if (_header != null)
            {
                if (item.HeaderHorizontalTextAlignment == TextAlignment.Start)
                {
                    _header.HorizontalOptions = LayoutOptions.Start;
                }
                else if (item.HeaderHorizontalTextAlignment == TextAlignment.Center)
                {
                    _header.HorizontalOptions = LayoutOptions.Center;
                }
                else if (item.HeaderHorizontalTextAlignment == TextAlignment.End)
                {
                    _header.HorizontalOptions = LayoutOptions.End;
                }
            }
        }

        #endregion
    }
}