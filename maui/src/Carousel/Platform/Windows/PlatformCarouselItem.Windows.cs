using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections;
using Windows.Foundation;

namespace Syncfusion.Maui.Toolkit.Carousel
{

	/// <summary>
	/// Implements a selectable inside a <see cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarousel"/>
	/// </summary>
	/// <exclude/>
	/// <remarks>
	/// The CarouselItem is a <see cref="N:Windows.UI.Xaml.Controls.ContentControl"/>.
	/// </remarks>
	/// <seealso cref="T:Syncfusion.Maui.Toolkit.Carousel.SfCarouselPanel"/>  
	public partial class PlatformCarouselItem : ContentControl, IDisposable
    {
        #region Bindable properties

        /// <summary>
        /// Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        /// </summary>        
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(PlatformCarouselItem), new PropertyMetadata(false, new PropertyChangedCallback(OnIsSelectedChanged)));

        /// <summary>
        /// Using a DependencyProperty as the backing store for SelectedTemplate.  This enables animation, styling, binding, etc...
        /// </summary>
        internal static readonly DependencyProperty SelectedTemplateProperty =
            DependencyProperty.Register("SelectedTemplate", typeof(DataTemplate), typeof(PlatformCarouselItem), new PropertyMetadata(null, OnSelectedTemplateChanged));

        #endregion

        #region Fields

        /// <summary>
        ///  Initializes a new instance of the Carousel
        /// </summary>
        private PlatformCarousel? _parentItemsControl;

        /// <summary>
        /// Initializes a new instance of the PlaneProjection
        /// </summary>
        private PlaneProjection? _planeProjection;

        /// <summary>
        /// Initializes pointer pressed.
        /// </summary>
        private bool _pointerPressed;

        /// <summary>
        /// Gets or sets X Rotation 
        /// </summary>
        private double _xRotation;

        /// <summary>
        /// Initializes a new instance of the Storyboard
        /// </summary>
        private Storyboard? _animation;

        /// <summary>
        /// Initializes a new instance of the Storyboard
        /// </summary>
        private Storyboard? _rotationStoryboard;

        /// <summary>
        /// Initializes a new instance of the Storyboard
        /// </summary>
        private Storyboard? _scaleStoryboard;

        /// <summary>
        /// Initializes a new instance of the Grid
        /// </summary>
        private Grid? _layoutGrid;

        /// <summary>
        /// Initializes a new instance of the Grid
        /// </summary>
        private Grid? _layoutGrid1;

        /// <summary>
        /// Initializes a new instance of the ScaleTransform
        /// </summary>
        private ScaleTransform? _scaleTransform;

        /// <summary>
        /// Initializes a new instance of the ContentPresenter
        /// </summary>
        private ContentPresenter? _contentPresenter;

        /// <summary>
        /// Initializes a new instance of the EasingDoubleKeyFrame
        /// </summary>
        private EasingDoubleKeyFrame? _rotationKeyFrame, _offsetZKeyFrame, _scaleXKeyFrame, _scaleYKeyFrame;

        /// <summary>
        /// Initializes a new instance of the X Animation
        /// </summary>
        private DoubleAnimation? _xAnimation;

        /// <summary>
        /// Initializes a new instance of the scaleXAnimation
        /// </summary>
        private DoubleAnimation? _scaleXAnimation;

        /// <summary>
        /// Initializes a new instance of the scaleYAnimation
        /// </summary>
        private DoubleAnimation? _scaleYAnimation;

        /// <summary>
        /// Initializes a new instance of the RotationAnimation
        /// </summary>
        private DoubleAnimation? _rotationAnimation;

        /// <summary>
        ///  Initializes a new instance of the CarouselItem
        /// </summary>
        private PlatformCarouselItem? _previousItem;

        /// <summary>
        ///  Initializes z Offset in double data type.
        /// </summary>
        private double _zOffset;

        /// <summary>
        /// Gets or sets Scale
        /// </summary>
        private double _scale;

        /// <summary>
        ///  Initializes a new instance of the Storyboard
        /// </summary>
        private Storyboard? _storyboard;

        /// <summary>
        /// Gets or sets Y Rotation
        /// </summary>
        private double _yRotation;

        /// <summary>
        /// Initializes IsAnimating 
        /// </summary>
        private bool _isAnimating;

        /// <summary>
        ///  Initializes a new instance of the  DataTemplate.
        /// </summary>
        private DataTemplate? _normalContentTemplate;

        /// <summary>
        ///  Initializes Pointer Over is false.
        /// </summary>
        private bool _isPointerOver;

        /// <summary>
        /// AutomationId field.
        /// </summary>
        private string _automationId = string.Empty;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see
		/// cref="T:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem"/> class.
		/// </summary>
		public PlatformCarouselItem()
        {
            DefaultStyleKey = typeof(PlatformCarouselItem);
            Loaded += SfCarouselItem_Loaded;
            Unloaded += SfCarouselItem_Unloaded;
        }

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the selected item has changed.
		/// </summary>
		/// <seealso cref="P:Syncfusion.Maui.Toolkit.Carousel.PlatformCarouselItem.IsSelected"/>
		public event RoutedEventHandler? Selected;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether <see
        /// cref="T:Syncfusion.UI.Xaml.Controls.Layout.PlatformCarouselItem"/> is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Gets or internal set the Storyboard.
        /// </summary>
        /// <value>The storyboard.</value>
        public Storyboard? Storyboard
        {
            get { return _storyboard; }
            internal set { _storyboard = value; }
        }

        /// <summary>
        /// Gets or sets the Parent Items Control
        /// </summary>
        internal PlatformCarousel? ParentItemsControl
        {
            get { return _parentItemsControl; }
            set { _parentItemsControl = value; }
        }

        /// <summary>
        /// Gets or sets the Normal content template
        /// </summary>
        internal DataTemplate? NormalContentTemplate
        {
            get { return _normalContentTemplate; }
            set { _normalContentTemplate = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is enabled.
        /// </summary>
        internal bool IsAnimating
        {
            get { return _isAnimating; }
            set { _isAnimating = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is enabled.
        /// </summary>
        internal bool IsPointerOver
        {
            get { return _isPointerOver; }
            set { _isPointerOver = value; }
        }

        /// <summary>
        /// Gets or sets Selected Template
        /// </summary>
        internal DataTemplate SelectedTemplate
        {
            get { return (DataTemplate)GetValue(SelectedTemplateProperty); }
            set { SetValue(SelectedTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets Z Offset
        /// </summary>
        internal double ZOffset
        {
            get
            {
                return _zOffset;
            }

            set
            {
                _zOffset = value;
                if (_planeProjection != null && Parent != null && Parent is PlatformCarousel parent && parent.ViewMode == ViewMode.Default)
                {
                    _planeProjection.LocalOffsetZ = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets Y Rotation
        /// </summary>
        internal double YRotation
        {
            get
            {
                return _yRotation;
            }

            set
            {
                _yRotation = value;
                if (_planeProjection != null && Parent != null && Parent is PlatformCarousel parent && parent.ViewMode == ViewMode.Default)
                {
                    _planeProjection.RotationY = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets X Rotation
        /// </summary>
        internal double XRotation
        {
            get
            {
                return _xRotation;
            }

            set
            {
                _xRotation = value;
                if (_planeProjection != null && Parent != null && Parent is PlatformCarousel parent && parent.ViewMode == ViewMode.Default)
                {
                    _planeProjection.RotationX = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets Scale
        /// </summary>
        internal new double Scale
        {
            get
            {
                return _scale;
            }

            set
            {
                _scale = value;
                if (_scaleTransform != null && Parent != null && Parent is PlatformCarousel parent && parent.ViewMode == ViewMode.Default)
                {
                    _scaleTransform.ScaleX = _scale;
                    _scaleTransform.ScaleY = _scale;
                }
            }
        }

        /// <summary>
        /// Gets or sets Left
        /// </summary>
        internal double Left
        {
            get
            {
                return Canvas.GetLeft(this);
            }

            set
            {
                Canvas.SetLeft(this, value);
            }
        }

        /// <summary>
        /// Gets or sets Top
        /// </summary>
        internal double Top
        {
            get
            {
                return Canvas.GetTop(this);
            }

            set
            {
                Canvas.SetTop(this, value);
            }
        }

        /// <summary>
        /// Gets or sets the AutomationId
        /// </summary>
        internal string AutomationId
        {
            get { return _automationId; }
            set
            {
                if (_automationId != value)
                {
                    _automationId = value;
                    if (_automationId == null)
                    {
                        _automationId = string.Empty;
                    }

                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// To clear unused objects
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method for Arrange
        /// </summary>
        /// <param name="left">The left</param>
        /// <param name="zIndex">The z Index</param>
        /// <param name="rotation">The rotation</param>
        /// <param name="zOffset">The z offset</param>
        /// <param name="zScale">The z Scale</param>
        /// <param name="duration">The duration</param>
        /// <param name="ease">The ease</param>
        /// <param name="useAnimation">The use Animation</param>
        /// <param name="isDisableAnimation">is disable animation</param>
        internal void Arrange(double left, int zIndex, double rotation, double zOffset, double zScale, TimeSpan duration, EasingFunctionBase ease, bool useAnimation, bool isDisableAnimation)
        {
            if (useAnimation)
            {
                AdjustPosition(left);

                if (ParentItemsControl != null)
                {
                    if (ParentItemsControl.ViewMode == ViewMode.Default)
                    {
                        SetTransform(left, rotation, zOffset, zScale);
                    }
                    else
                    {
                        SetTransform(left, rotation, zOffset, zScale);
                    }
                }

                duration = isDisableAnimation ? SetDuration(new TimeSpan(0)) : SetDuration(duration);

                SetEasing(ease);

                IsAnimating = true;

                BeginStoryboard();

                Canvas.SetZIndex(this, zIndex);
            }

            AdjustContentPresenterMargin();
        }

        /// <summary>
        /// Adjusts the position of the item.
        /// </summary>
        /// <param name="left"></param>
        void AdjustPosition(double left)
        {
            if (ParentItemsControl != null)
            {
                if (ParentItemsControl.Orientation == Orientation.Vertical)
                {
                    if (!IsAnimating && Canvas.GetTop(this) != left)
                    {
                        Canvas.SetTop(this, Top);
                    }
                }
                else
                {
                    if (!IsAnimating && Canvas.GetLeft(this) != left)
                    {
                        Canvas.SetLeft(this, Left);
                    }
                }
            }
        }

        /// <summary>
        /// Begins the animation storyboards based on the orientation of the parent items control.
        /// </summary>
        void BeginStoryboard()
        {
            if (ParentItemsControl != null)
            {
                if (ParentItemsControl.Orientation == Orientation.Horizontal)
                {
                    _animation?.Begin();
                    _rotationStoryboard?.Begin();
                    _scaleStoryboard?.Begin();
                }
            }
        }

        /// <summary>
        /// Adjusts the margin of the content presenter based on the orientation of the parent items control.
        /// </summary>
        void AdjustContentPresenterMargin()
        {
            if (ParentItemsControl != null && _contentPresenter != null)
            {
                _contentPresenter.Margin = ParentItemsControl.Orientation == Orientation.Vertical
                    ? new Thickness(0, 50, 0, 50)
                    : new Thickness(0);
            }
        }

        /// <summary>
        /// method for update items
        /// </summary>
        internal void UpdateItems()
        {
            if (ParentItemsControl != null)
            {
                if (ParentItemsControl.Queue != null && ParentItemsControl.Queue.Count > 0)
                {
                    ParentItemsControl.UpdateSource();
                }
            }
        }

        /// <summary>
        /// Method for Content Template.
        /// </summary>
        internal void ApplyContentTemplate()
        {
            if (ParentItemsControl != null && ParentItemsControl.SelectedItemTemplate != null && SelectedTemplate != null)
            {
                if (IsSelected)
                {
                    HandleSelectedState();
                }
                else
                {
                    ContentTemplate = NormalContentTemplate;
                }
            }
        }

        /// <summary>
        /// Handles the selected state of the item
        /// </summary>
        void HandleSelectedState()
        {
            if (ParentItemsControl != null && ParentItemsControl.ItemsSource != null && DataContext as object != ParentItemsControl.SelectedItem)
            {
                IsSelected = false;
                ContentTemplate = NormalContentTemplate;
            }
            else
            {
                ContentTemplate = SelectedTemplate;
                if (ParentItemsControl != null && ParentItemsControl.SelectedItem == null)
                {
                    ParentItemsControl.SelectedItem = this;
                }
            }
        }

		#endregion

		#region Override Methods
		/// <summary>
		/// Applies the template for<see cref="T:Syncfusion.Maui.Toolkit.Carousel.SfCarousel"/> control.
		/// </summary>
		/// <exclude/>
		protected override void OnApplyTemplate()
        {
            InitializeTemplateParts();
            InitializeAnimations();
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Initializes the template parts
        /// </summary>
        void InitializeTemplateParts()
        {
            _planeProjection = (PlaneProjection)GetTemplateChild("Rotator");
            _contentPresenter = (ContentPresenter)GetTemplateChild("ContentPresenter");
            _layoutGrid = (Grid)GetTemplateChild("LayoutRootGrid");
            _layoutGrid1 = (Grid)GetTemplateChild("LayoutRootGrid1");
            _scaleTransform = (ScaleTransform)GetTemplateChild("ScaleTransform");

            if (_planeProjection != null)
            {
                _planeProjection.RotationY = _yRotation;
                _planeProjection.RotationX = _xRotation;
                _planeProjection.LocalOffsetZ = _zOffset;

                Projection = _planeProjection;
            }

            if (_scaleTransform != null)
            {
                RenderTransform = _scaleTransform;
                RenderTransformOrigin = new Point(0.5, 0.5);
            }
        }

        /// <summary>
        /// Initializes the animation parts
        /// </summary>
        void InitializeAnimations()
        {
            _animation = new Storyboard();
            _rotationKeyFrame = CreateDoubleAnimation("Rotator", "RotationY", 0.9, 0);
            _offsetZKeyFrame = CreateDoubleAnimation("Rotator", "LocalOffsetZ", 0.9, 0);
            _scaleXKeyFrame = CreateDoubleAnimation("ScaleTransform", "ScaleX", 0.9, 1);
            _scaleYKeyFrame = CreateDoubleAnimation("ScaleTransform", "ScaleY", 0.9, 1);

            _layoutGrid?.Resources.Add("Animation", _animation);

            InitializeRotationAnimation();

            if (_scaleTransform != null)
            {
                _scaleXAnimation = new DoubleAnimation();
                _scaleYAnimation = new DoubleAnimation();

                Storyboard.SetTarget(_scaleXAnimation, _scaleTransform);
                Storyboard.SetTargetProperty(_scaleXAnimation, "ScaleX");
                Storyboard.SetTarget(_scaleYAnimation, _scaleTransform);
                Storyboard.SetTargetProperty(_scaleYAnimation, "ScaleY");

                _scaleStoryboard = new Storyboard();
                _scaleStoryboard.Children.Add(_scaleXAnimation);
                _scaleStoryboard.Children.Add(_scaleYAnimation);
            }

            if (_animation != null)
            {
                _animation.Completed += Animation_Completed;
                _xAnimation = new DoubleAnimation();
                _animation.Children.Add(_xAnimation);
                Storyboard.SetTarget(_xAnimation, this);
                Storyboard.SetTargetProperty(_xAnimation, "(Canvas.Left)");
            }

            if (ContentTemplate != null && ContentTemplate != SelectedTemplate)
            {
                NormalContentTemplate = ContentTemplate;
            }
        }

        /// <summary>
        /// Initialize the rotation animation.
        /// </summary>
        void InitializeRotationAnimation()
        {
            if (_planeProjection != null)
            {
                _rotationAnimation = new DoubleAnimation();
                Storyboard.SetTarget(_rotationAnimation, Projection);
                Storyboard.SetTargetProperty(_rotationAnimation, "RotationY");

                _rotationStoryboard = new Storyboard();
                _rotationStoryboard.Children.Add(_rotationAnimation);
            }
        }

        /// <summary>
        /// Create double animation.
        /// </summary>
        /// <param name="targetName"></param>
        /// <param name="targetProperty"></param>
        /// <param name="keyTimeSeconds"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static EasingDoubleKeyFrame CreateDoubleAnimation(string targetName, string targetProperty, double keyTimeSeconds, double value)
        {
            EasingDoubleKeyFrame keyFrame = new EasingDoubleKeyFrame
            {
                KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(keyTimeSeconds)),
                Value = value,
                EasingFunction = new CubicEase()
            };

            return keyFrame;
        }

		/// <summary>
		///  Sets the parentItemsControl when pointer is pressed.
		/// </summary>
		/// <exclude/>
		/// <param name="e">Pointer RoutedEvent Argument as e</param>
		protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            _pointerPressed = true;
            if (ParentItemsControl != null)
            {
                ParentItemsControl.CanSelect = true;
            }

            base.OnPointerPressed(e);
        }

		/// <summary>
		/// method is called when PointerExited.
		/// </summary>
		/// <exclude/>
		/// <param name="e">Pointer RoutedEvent Argument as e.</param>
		protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            if (_pointerPressed)
            {
                ExecutePointerReleased();
            }

            base.OnPointerExited(e);
        }

		/// <summary>
		/// sets the focus when pointer is moved. 
		/// </summary>
		/// <exclude/>
		/// <param name="e">Pointer RoutedEvent Argument as e.</param>
		protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if (e.Pointer.IsInContact)
            {
                IsPointerOver = true;
            }

            base.OnPointerMoved(e);
        }

		/// <summary>
		/// Sets the parent Items Control when pointer is released.
		/// </summary>
		/// <exclude/>
		/// <param name="e">Pointer RoutedEvent Argument as e.</param>
		protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            IsPointerOver = false;
            ApplyContentTemplate();
            if (ParentItemsControl != null)
            {
                if (ParentItemsControl.AllowLoadMore)
                {
                    if (Tag != null)
                    {
                        UpdateItems();
                    }
                }

                if (ParentItemsControl.CanSelect || (ParentItemsControl.AllowLoadMore && Tag == null))
                {
                    HandleSelection(e);
                }
            }

            base.OnPointerReleased(e);
        }

        /// <summary>
        /// Handles the selection of a carousel item when a pointer event occurs.
        /// </summary>
        /// <param name="e"></param>
        void HandleSelection(PointerRoutedEventArgs e)
        {
            if(ParentItemsControl != null)
            {
                PlatformCarouselItem? selectItem = ParentItemsControl.SelectedItem != null ? ParentItemsControl.ContainerFromItem(ParentItemsControl.SelectedItem) as PlatformCarouselItem : null;
                if (_previousItem != null && selectItem != null && selectItem != _previousItem)
                {
                    _previousItem.IsSelected = false;
                    _previousItem.ContentTemplate = NormalContentTemplate;
                }

                //// Added to restrict the selection change while swiping 
                if (Selected != null)
                {
                    if (!ParentItemsControl.IsManipulatedData)
                    {
                        Selected(this, e);
                    }
                    else
                    {
                        ParentItemsControl.IsManipulatedData = false;
                    }
                }
                _previousItem = this;
                ParentItemsControl.CanSelect = false;
            }
        }

        /// <summary>
        /// To remove all the instance which is used in Accordion
        /// </summary>
        /// <param name="disposing">The disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Loaded -= SfCarouselItem_Loaded;
                Unloaded -= SfCarouselItem_Unloaded;
                SizeChanged -= SfCarouselItem_SizeChanged;

                _planeProjection = null;
                _scaleTransform = null;
                _contentPresenter = null;

                if (_animation != null)
                {
                    _animation.Stop();
                    _animation.Completed -= Animation_Completed;
                    _animation.Children.Clear();
                    _animation = null;
                }

                if (_layoutGrid != null)
                {
                    _layoutGrid.Children?.Clear();
                    _layoutGrid = null;
                }

                if (_layoutGrid1 != null)
                {
                    _layoutGrid1.Children?.Clear();
                    _layoutGrid1 = null;
                }

                if (_storyboard != null)
                {
                    _storyboard.Stop();
                    _storyboard.Children.Clear();
                    _storyboard = null;
                }

                if (Storyboard != null)
                {
                    Storyboard.Stop();
                    Storyboard.Children.Clear();
                    Storyboard = null;
                }

                if (_previousItem != null)
                {
                    _previousItem = null;
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method called when Selected Template Changed.
        /// </summary>
        /// <param name="d">Dependency Object as d</param>
        /// <param name="e">Dependency Property Changed Event Argument as e</param>
        private static void OnSelectedTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlatformCarouselItem? carouselItem = d as PlatformCarouselItem;
            carouselItem?.ApplyContentTemplate();
        }

        /// <summary>
        /// method is called when selection changed.
        /// </summary>
        /// <param name="sender">Dependency Object as sender</param>
        /// <param name="args">Dependency Property Event Argument as args</param>
        private static void OnIsSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PlatformCarouselItem? item = sender as PlatformCarouselItem;
            if (item != null && item.ParentItemsControl != null && (bool)args.NewValue)
            {
                int index = -1;
                if (!item.ParentItemsControl.EnableVirtualization)
                {
                    index = item.ParentItemsControl.IndexFromContainer(item);
                }
                else
                {
                    if(item.ParentItemsControl.TempCollection != null)
                    	index = ((IList)item.ParentItemsControl.VirtualItemList).IndexOf(item);
                }

                if (item.ParentItemsControl.SelectedIndex != index)
                {
                    item.ParentItemsControl.SelectedIndex = index;
                }
            }
        }

        /// <summary>
        /// sets the Easing
        /// </summary>
        /// <param name="ease">Easing Function Base as ease.</param>
        void SetEasing(EasingFunctionBase ease)
        {
            if (ParentItemsControl != null)
            {
                if (ParentItemsControl.Orientation == Orientation.Horizontal)
                {
                    if (_offsetZKeyFrame != null && _scaleXKeyFrame != null && _scaleYKeyFrame != null && _xAnimation != null)
                    {
                        _offsetZKeyFrame.EasingFunction = ease;
                        _scaleYKeyFrame.EasingFunction = ease;
                        _scaleXKeyFrame.EasingFunction = ease;
                        if (_rotationKeyFrame != null)
                        {
                            _rotationKeyFrame.EasingFunction = ease;
                        }

                        _xAnimation.EasingFunction = ease;
                    }
                }
            }
        }

        /// <summary>
        /// sets the Duration
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>duration has returned.</returns>
        TimeSpan SetDuration(TimeSpan duration)
        {
            if (ParentItemsControl != null)
            {
                if (ParentItemsControl.Orientation == Orientation.Horizontal)
                {
                    if (_offsetZKeyFrame != null && _scaleXKeyFrame != null && _scaleYKeyFrame != null && _xAnimation != null)
                    {
                        _offsetZKeyFrame.KeyTime = KeyTime.FromTimeSpan(duration);
                        _scaleYKeyFrame.KeyTime = KeyTime.FromTimeSpan(duration);
                        _scaleXKeyFrame.KeyTime = KeyTime.FromTimeSpan(duration);
                        if (_rotationKeyFrame != null)
                        {
                            _rotationKeyFrame.KeyTime = KeyTime.FromTimeSpan(duration);
                        }

                        _xAnimation.Duration = duration;
                    }
                }
            }

            return duration;
        }

        /// <summary>
        /// Method for Set Transform.
        /// </summary>
        /// <param name="left">The Left</param>
        /// <param name="rotation">The Rotation</param>
        /// <param name="zOffset">The z offset</param>
        /// <param name="scaleValue">The scale value</param>
        void SetTransform(double left, double rotation, double zOffset, double scaleValue)
        {
            if (ParentItemsControl != null)
            {
                if (_xAnimation != null && ParentItemsControl.Orientation == Orientation.Horizontal)
                {
                    if (_offsetZKeyFrame != null && _scaleXAnimation != null && _scaleYAnimation != null && _xAnimation != null)
                    {
                        if (_rotationAnimation != null)
                        {
                            _rotationAnimation.To = rotation;
                            _rotationAnimation.Duration = ParentItemsControl.Duration;
                        }

                        _scaleXAnimation.To = scaleValue;
                        _scaleYAnimation.To = scaleValue;
                        _scaleXAnimation.Duration = ParentItemsControl.Duration;
                        _scaleYAnimation.Duration = ParentItemsControl.Duration;
                        _offsetZKeyFrame.Value = zOffset;
                        _xAnimation.To = left;
                    }
                }
            }
        }

        /// <summary>
        /// Method for Animation Completion.
        /// </summary>
        /// <param name="sender">object as sender.</param>
        /// <param name="e">object as e.</param>
        private void Animation_Completed(object? sender, object e)
        {
            if (!IsPointerOver && ParentItemsControl != null && ParentItemsControl.ViewMode == ViewMode.Default)
            {
                ApplyContentTemplate();
            }

            IsAnimating = false;
        }

        /// <summary>
        /// method has called when Pointer Released.
        /// </summary>
        void ExecutePointerReleased()
        {
            if (_pointerPressed)
            {
                if (_storyboard != null)
                {
                    DoubleAnimationUsingKeyFrames? xAnimation = _storyboard.Children[0] as DoubleAnimationUsingKeyFrames;

                    if (xAnimation != null)
                    {
                        EasingDoubleKeyFrame? frame1 = xAnimation.KeyFrames[0] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame? frame2 = xAnimation.KeyFrames[1] as EasingDoubleKeyFrame;

                        if (frame1 != null && frame2 != null)
                        {
                            frame1.Value = frame2.Value;
                            frame2.Value = 1;
                        }
                    }

                    _storyboard.Begin();
                }
            }

            _pointerPressed = false;
        }

        /// <summary>
        /// Carousel Item Unloaded method.
        /// </summary>
        /// <param name="sender">object as sender.</param>
        /// <param name="e">RoutedEvent Argument as e</param>
        private void SfCarouselItem_Unloaded(object sender, RoutedEventArgs e)
        {
            SizeChanged -= SfCarouselItem_SizeChanged;
        }

        /// <summary>
        /// Carousel Item Loaded Method.
        /// </summary>
        /// <param name="sender">object as sender.</param>
        /// <param name="e">RoutedEvent Argument as e.</param>
        private void SfCarouselItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (ParentItemsControl != null)
            {
                ParentItemsControl.Refresh(this);
                if (!ParentItemsControl.EnableVirtualization && ParentItemsControl.SelectedIndex >= 0 && ParentItemsControl.SelectedIndex < ParentItemsControl.Items.Count && ParentItemsControl.SelectedItem == null)
                {
                    PlatformCarouselItem? item = ParentItemsControl.ContainerFromIndex(ParentItemsControl.SelectedIndex) as PlatformCarouselItem;
                    ParentItemsControl.SelectedItem = ParentItemsControl.Items[ParentItemsControl.SelectedIndex];
                    if (item == this)
                    {
                        ParentItemsControl.UpdateSelection();
                    }

                    ApplyContentTemplate();
                }

                if (ParentItemsControl.ViewMode == ViewMode.Linear)
                {
                    ParentItemsControl.InvalidateMeasure();
                }
            }

            SizeChanged += SfCarouselItem_SizeChanged;
        }

        /// <summary>
        /// Method for Item Size Changed. 
        /// </summary>
        /// <param name="sender">object as sender.</param>
        /// <param name="e">Size Changed Event Argument as e.</param>
        private void SfCarouselItem_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ParentItemsControl?.ArrangeCarouselItem(sender);
        }

        #endregion
    }
}
