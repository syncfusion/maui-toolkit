using Microsoft.Maui.Animations;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Animation = Microsoft.Maui.Animations.Animation;
using ControlAnimation = Microsoft.Maui.Controls.Animation;

namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// This class represents a content view to show tooltip in absolute layout.
    /// </summary>
    internal class SfTooltip : ContentView
    {
        #region Fields

        readonly Grid _parentView;
        readonly TooltipDrawableView _drawableView;
        readonly ContentView _contentView;
        readonly TooltipHelper _tooltipHelper;
        bool _isDisappeared = false;
        bool _isTooltipActivate = false;
        const string _durationAnimation = "DurationAnimation";

        #endregion

        #region Properties

        View? _content;

        /// <summary>
        /// Gets or sets the content for tooltip.
        /// </summary>
        public new View? Content
        {
            get
            {
                return _content;
            }

            set
            {
                if (_content != value)
                {
                    _content = value;
                    OnContentChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the position of tooltip.
        /// </summary>
        public TooltipPosition Position { get; set; } = TooltipPosition.Auto;

        /// <summary>
        /// Gets or sets the duration of the tooltip in seconds.
        /// </summary>
        public double Duration { get; set; } = 2;

        /// <summary>
        /// Gets or sets the background for tooltip. This is bindable property.
        /// </summary>
        public static readonly new BindableProperty BackgroundProperty = BindableProperty.Create(
            nameof(Background),
            typeof(Brush),
            typeof(SfTooltip),
            new SolidColorBrush(Colors.Black),
            propertyChanged: OnBackgroundPropertyChanged);

        /// <summary>
        /// Gets or sets the background for tooltip.
        /// </summary>
        public new Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        #endregion

        #region Events

        /// <summary>
        /// It represents the tooltip closed event handler. This tooltip closed event is hooked when tooltip is disappear from the visibility.
        /// </summary>
        public event EventHandler<TooltipClosedEventArgs>? TooltipClosed;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of the <see cref="SfTooltip"/> class.
        /// </summary>
        public SfTooltip()
        {
            _parentView = new Grid();
            _drawableView = new TooltipDrawableView(this);
            _contentView = new ContentView();
            _tooltipHelper = new TooltipHelper(_drawableView.InvalidateDrawable);
            _parentView.Add(_drawableView);
            _parentView.Add(_contentView);
            base.Content = _parentView;
        }

        #endregion

        #region Internal properties

        /// <summary>
        /// Gets or sets the stroke color for tooltip.
        /// </summary>
        internal Brush Stroke { get; set; } = Brush.Transparent;

        /// <summary>
        /// Gets or sets the stroke width for the tool tip.
        /// </summary>
        internal float StrokeWidth { get; set; } = 0f;

        /// <summary>
        /// Get the tooltip Helper
        /// </summary>
        internal TooltipHelper Helper
        {
            get
            {
                return _tooltipHelper;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shows the tooltip based on target and container rectangle.
        /// </summary>
        /// <param name="containerRect"></param>
        /// <param name="targetRect"></param>
        /// <param name="animated"></param>
        public void Show(Rect containerRect, Rect targetRect, bool animated)
        {
            if (containerRect.IsEmpty || targetRect.IsEmpty || Content == null) return;

            var x = containerRect.X;
            var y = containerRect.Y;
            var width = containerRect.Width;
            var height = containerRect.Height;

            if (targetRect.X > x + width || targetRect.Y > y + height) return;

            _tooltipHelper.Position = Position;
            _tooltipHelper.Duration = Duration;
            _tooltipHelper.Background = Background;
            _tooltipHelper.Stroke = Stroke;
            _tooltipHelper.StrokeWidth = StrokeWidth;

            if (_isTooltipActivate)
            {
                _isDisappeared = false;
            }

            if (Opacity == 0f)
                Opacity = 1f;

#if WINDOWS
            Content.VerticalOptions = LayoutOptions.Fill;
            Content.HorizontalOptions = LayoutOptions.Fill;
#else
            Content.VerticalOptions = LayoutOptions.Start;
            Content.HorizontalOptions = LayoutOptions.Start;
#endif

            _tooltipHelper.ContentSize = Content.Measure(double.PositiveInfinity, double.PositiveInfinity);
            _tooltipHelper.Show(containerRect, targetRect, false);
            SetContentMargin(_tooltipHelper.ContentViewMargin);
            AbsoluteLayout.SetLayoutBounds(this, _tooltipHelper.TooltipRect);
            _drawableView.InvalidateDrawable();

            _isTooltipActivate = true;

            if (animated)
            {
                ShowAnimation();
            }
            else
            {
                AutoHide();
            }
        }

        IAnimationManager? animationManager = null;

        /// <summary>
        /// Hides the tooltip.
        /// </summary>
        /// <param name="animated"></param>
        public void Hide(bool animated)
        {
            this.AbortAnimation(_durationAnimation);
            Opacity = 0f;
            AbsoluteLayout.SetLayoutBounds(this, new Rect(0, 0, 1, 1));
            _isTooltipActivate = false;
            TooltipClosed?.Invoke(this, new TooltipClosedEventArgs() { IsCompleted = _isDisappeared });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        internal void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Draw(canvas);
        }

        void Draw(ICanvas canvas)
        {
            if (_tooltipHelper.RoundedRect == Rect.Zero) return;
            _tooltipHelper.Draw(canvas);
        }

        void ShowAnimation()
        {
            SetAnimationManager();
            if (animationManager != null)
            {
                var animation = new Animation(UpdateToolTipAnimation, start: 0, 0.25, Easing.SpringOut, AutoHide);
                animation.Commit(animationManager);
            }
        }

        void SetAnimationManager()
        {
            if (Application.Current != null && animationManager == null)
            {
                var handler = Application.Current.Handler;
                if (handler != null && handler.MauiContext != null)
                    animationManager = handler.MauiContext.Services.GetRequiredService<IAnimationManager>();
            }
        }

        void UpdateToolTipAnimation(double value)
        {
            Scale = value;
        }

        void AutoHide()
        {
            this.AbortAnimation(_durationAnimation);
            var duration = _tooltipHelper.Duration;

            if (double.IsFinite(duration) && duration > 0)
            {
                ControlAnimation animation = new ControlAnimation();
                animation.Commit(this, _durationAnimation, 16, (uint)(_tooltipHelper.Duration * 1000), Easing.Linear, Hide, () => false);
            }
        }

        void Hide(double value, bool isCompleted)
        {
            _isDisappeared = !isCompleted;

            if (!isCompleted)
                Hide(false);
        }

        #region ContentView Methods

        /// <summary>
        /// Invoked when binding context changed.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Content != null)
            {
                SetInheritedBindingContext(Content, BindingContext);
            }
        }


        static void OnBackgroundPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        /// <summary>
        /// Updated margin for <see cref="ContentView"/>.
        /// </summary>
        /// <param name="thickness"></param>
        void SetContentMargin(Thickness thickness)
        {
            if (_contentView != null)
            {
                _contentView.Margin = thickness;
            }
        }

        void OnContentChanged()
        {
            if (Content != null)
            {
                SetInheritedBindingContext(Content, BindingContext);
            }

            _contentView.Content = Content;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// This class represents a drawable view used draw the tooltip using native drawing options. 
    /// </summary>
    internal class TooltipDrawableView : SfDrawableView
    {
        SfTooltip tooltip;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            tooltip.Draw(canvas, dirtyRect);
        }

        internal TooltipDrawableView(SfTooltip sfTooltip)
        {
            tooltip = sfTooltip;
        }
    }
}
