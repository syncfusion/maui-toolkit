using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Syncfusion.Maui.Toolkit.Accordion;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Internals;

namespace Syncfusion.Maui.Toolkit.Expander
{
    /// <summary>
    /// Class used to host the <see cref="SfExpander.Header"/>.
    /// </summary>
    internal partial class ExpanderHeader : SfView, ITapGestureListener, ITouchListener, IKeyboardListener
    {
		#region Fields

		/// <summary>
		/// Width of ExpandCollapseButton.
		/// </summary>
		internal readonly int _iconViewSize = 40;

		/// <summary>
		/// Holds the content and icon of header.
		/// </summary>
		View? _headerViewGrid;

		/// <summary>
		/// Indicates whether the mouse is currently hovering over the element.
		/// </summary>
		bool _isMouseHover = false;

		/// <summary>
		/// Represents the rectangle area that currently has focus.
		/// </summary>
		Rect _focusedRect;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpanderHeader" /> class.
        /// </summary>
        public ExpanderHeader()
        {
            SetUp();
        }

		#endregion

		#region Internal Properties       

		/// <summary>
		/// Gets or sets a new instance of the <see cref="ExpanderHeader" /> class.
		/// </summary>
		internal SfExpander? Expander
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a view that to be loaded as the indicator in the <see cref="ExpanderHeader"/> of <see cref="SfExpander"/>.
        /// </summary>
        internal ExpandCollapseButton? IconView
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a border need to draw for the item.
        /// </summary>
        internal bool IsMouseHover
        {
            get
            {
                return _isMouseHover;
            }

            set
            {
                if (value == _isMouseHover)
                {
                    return;
                }

                _isMouseHover = value;
                OnPropertyChanged(nameof(IsMouseHover));
            }
        }

		#endregion

		#region Private Properties

		/// <summary>
		/// Gets a value indicating whether the view can become the first responder to listen the keyboard actions.
		/// </summary>
		bool IKeyboardListener.CanBecomeFirstResponder
		{
			get { return true; }
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Adds the Icon view to the header.
		/// </summary>
		internal void UpdateIconView()
		{
			if (Expander == null || Expander.Header == null || (IconView == null && Expander.HeaderIconPosition == ExpanderIconPosition.None))
			{
				return;
			}

			if (IconView == null)
			{
				CreateIndicatorView();
				UpdateIconViewDirection(Expander.IsExpanded);
				if (IconView != null)
				{
					SemanticProperties.SetDescription(IconView.Content, Expander.SemanticDescription + " Expander");
				}
			}
		}

		/// <summary>
		/// Rotates the expander icon when expanded or collapsed..
		/// </summary>
		/// <param name="isExpand">Indicating whether the expander is expanded or not.</param>
		internal void UpdateIconViewDirection(bool isExpand)
		{
			if (IconView == null || Expander == null)
			{
				return;
			}

			IconView.RotateIcon(isExpand);
			var hint = Expander.IsExpanded ? "Double tap to collapse" : "Double tap to expand";
			SemanticProperties.SetHint(IconView.Content, hint);
		}

		/// <summary>
		/// Method to clear and add the content and icon views in Header.
		/// </summary>
		internal void UpdateChildViews()
		{
			if (_headerViewGrid is Grid headerview && Children.Count > 0)
			{
				headerview.ColumnDefinitions.Clear();
				headerview.Children.Clear();

				// MouseHover,Focused state is not applied on the header when the header is changed at runtime.
				Children.Remove(headerview);
			}

			UpdateIconView();
			AddChildViews();
		}

		/// <summary>
		/// Method to update the icon color.
		/// </summary>
		/// <param name="color">color value.</param>
		internal void UpdateIconColor(Color color)
        {
			if (IconView == null)
			{
				return;
			}

			IconView.UpdateIconColor(color);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Raises when the key is pressed.
		/// </summary>
		/// <param name="e">Event data.</param>
		void IKeyboardListener.OnKeyDown(KeyEventArgs e)
		{
			if (Expander == null)
			{
				return;
			}

			if (Expander is AccordionItemView accordionItemView && accordionItemView.Accordion is IKeyboardListener listener)
			{
				listener.OnKeyDown(e);
			}
			else if (e.Key == KeyboardKey.Enter)
			{
				if (Expander.IsExpanded)
				{
					Expander.RaiseCollapsingEvent();
				}
				else
				{
					Expander.RaiseExpandingEvent();
				}
			}
		}

		/// <summary>
		/// Raises when the key is released.
		/// </summary>
		/// <param name="e">Event data.</param>
		void IKeyboardListener.OnKeyUp(KeyEventArgs e)
		{
		}

		/// <summary>  
		/// Creates the indicator view, which visually represents the state or progress of the element.  
		/// </summary>  
		ExpandCollapseButton? CreateIndicatorView()
		{
			if (Expander == null)
			{
				return null;
			}

			IconView = new ExpandCollapseButton(Expander);
			return IconView;
		}

		/// <summary>  
		/// Sets up the necessary configurations or initializations for the control.  
		/// </summary>  
		void SetUp()
		{
			BackgroundColor = Colors.Transparent;
			this.AddGestureListener(this);
			this.AddTouchListener(this);
			_headerViewGrid = new Grid();
			DrawingOrder = DrawingOrder.BelowContent;
			this.AddKeyboardListener(this);
		}

		/// <summary>  
		/// Adds the child views to the parent element or container.  
		/// </summary>  
		void AddChildViews()
		{
			if (_headerViewGrid is Grid headerview && Expander != null)
			{
				// Configure column definitions based on header icon position
				if (Expander.HeaderIconPosition == ExpanderIconPosition.Start)
				{
					headerview.ColumnDefinitions.Add(new ColumnDefinition { Width = _iconViewSize });
					headerview.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
				}
				else
				{
					headerview.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
					if (Expander.HeaderIconPosition == ExpanderIconPosition.End)
					{
						headerview.ColumnDefinitions.Add(new ColumnDefinition { Width = _iconViewSize });
					}
				}

				// Add header and icon views to the grid
				if (Expander.Header != null)
				{
					headerview.Children.Add(Expander.Header);
					if (IconView != null && Expander.HeaderIconPosition != ExpanderIconPosition.None)
					{
						headerview.Children.Add(IconView);
						Grid.SetColumn(IconView, Expander.HeaderIconPosition == ExpanderIconPosition.Start ? 0 : 1);
						Grid.SetColumn(Expander.Header, Expander.HeaderIconPosition == ExpanderIconPosition.Start ? 1 : 0);
					}
					else
					{
						Grid.SetColumn(Expander.Header, 0);
					}
				}
			}

			if (_headerViewGrid != null)
			{
				Children.Add(_headerViewGrid);
			}
		}

		/// <summary>
		/// This method enables the necessary setting to the KeyListener.
		/// </summary>
		void SetUpKeyListenerRequirements()
		{
			if (Handler == null || Handler.PlatformView == null)
			{
				return;
			}

#if WINDOWS
			if (Handler.PlatformView is Microsoft.UI.Xaml.FrameworkElement handler)
			{
				handler.IsTabStop = true;
			}
#elif ANDROID
			if (Handler.PlatformView is Android.Views.View handler)
			{
				handler.FocusableInTouchMode = true;
			}
#endif
		}

		/// <summary>
		/// Handles the mouse hover state for the item.
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="dirtyRect"></param>
		/// <param name="view"></param>
		/// <param name="hasVisualStateGroups"></param>
		void HandleMouseHoverState(ICanvas canvas, RectF dirtyRect, View? view, bool hasVisualStateGroups)
		{
			if (Expander == null)
			{
				return;
			}

			if (hasVisualStateGroups)
			{
				VisualStateManager.GoToState(view, "PointerOver");
			}

			Paint headerPaint = hasVisualStateGroups ? Expander.HeaderBackground : Expander.HoverHeaderBackground;
			canvas.SetFillPaint(headerPaint, dirtyRect);
			canvas.FillRectangle(dirtyRect);
		}

		/// <summary>
		/// Manages the state changes when an accordion item is expanded or collapsed
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="dirtyRect"></param>
		/// <param name="view"></param>
		/// <param name="hasVisualStateGroups"></param>
		/// <param name="isSelectedOrExpanded"></param>
		void HandleAccordionState(ICanvas canvas, RectF dirtyRect, View? view, bool hasVisualStateGroups, bool isSelectedOrExpanded)
		{
			if (Expander == null)
			{
				return;
			}

			if (Expander.IsExpanded)
			{
				canvas.StrokeColor = Expander.HeaderStroke;
				canvas.StrokeSize = (float)Expander.HeaderStrokeThickness;
				canvas.DrawLine(dirtyRect.X, dirtyRect.Y, dirtyRect.Width, dirtyRect.Y);
			}

			_focusedRect = Expander.IsExpanded
				? new Rect(dirtyRect.X, Expander.HeaderStrokeThickness / 2, Width, Height - (Expander.HeaderStrokeThickness / 2))
				: new Rect(dirtyRect.X, dirtyRect.Y, Width, Height);
			if (isSelectedOrExpanded)
			{
				if (hasVisualStateGroups)
				{
					VisualStateManager.GoToState(view, "Focused");
					VisualStateManager.GoToState(view, Expander.IsExpanded ? "Expanded" : "Collapsed");
				}

				Paint headerPaint = hasVisualStateGroups ? Expander.HeaderBackground : Expander.FocusedHeaderBackground;
				canvas.SetFillPaint(headerPaint, _focusedRect);
				canvas.FillRectangle(_focusedRect);
			}
			else if (!IsMouseHover)
			{
				HandleNormalState(canvas, dirtyRect, view, hasVisualStateGroups);
			}

			if (Expander is AccordionItemView accordionItemView && accordionItemView.Accordion != null && accordionItemView.Accordion.CurrentItem != null && accordionItemView.Accordion.CurrentItem._accordionItemView == Expander)
			{
				canvas.StrokeColor = accordionItemView.FocusBorderColor;
				canvas.StrokeSize = 1.2f;
				canvas.DrawRectangle(1.2f / 2, 1.2f / 2, dirtyRect.Width - 1.2f, dirtyRect.Height - 1.2f);
			}
		}

		/// <summary>
		/// Handles transitioning to the normal state.
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="dirtyRect"></param>
		/// <param name="view"></param>
		/// <param name="hasVisualStateGroups"></param>
		void HandleNormalState(ICanvas canvas, RectF dirtyRect, View? view, bool hasVisualStateGroups)
		{
			if (Expander == null)
			{
				return;
			}

			if (hasVisualStateGroups)
			{
				VisualStateManager.GoToState(view, "Normal");
				VisualStateManager.GoToState(view, Expander.IsExpanded ? "Expanded" : "Collapsed");
			}

			Paint headerPaint = Expander.HeaderBackground;
			canvas.SetFillPaint(headerPaint, dirtyRect);
			canvas.FillRectangle(dirtyRect);
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Drawing methods of chip control.
		/// </summary>
		/// <param name="canvas">The Canvas.</param>
		/// <param name="dirtyRect">The rectangle.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{		
			View? view = Expander is AccordionItemView accordion && accordion != null ? accordion.AccordionItem : Expander;
			if (view == null || Expander == null)
			{
				return;
			}

			bool hasVisualStateGroups = view.HasVisualStateGroups();
			bool isAccordion = Expander is AccordionItemView;
			bool isSelectedOrExpanded = Expander.IsSelected || Expander.IsExpanded;
			if (IsMouseHover)
			{
				HandleMouseHoverState(canvas, dirtyRect, view, hasVisualStateGroups);
			}

			if (isAccordion)
			{
				HandleAccordionState(canvas, dirtyRect, view, hasVisualStateGroups, isSelectedOrExpanded);
			}
			else if (!IsMouseHover)
			{
				HandleNormalState(canvas, dirtyRect, view, hasVisualStateGroups);
			}

			base.OnDraw(canvas, dirtyRect);
		}

		/// <summary>
		/// Need to handle the run time changes of <see cref="PropertyChangedEventArgs"/> of <see cref="ExpanderHeader"/>.
		/// </summary>
		/// <param name="propertyName">Represents the property changed event arguments.</param>
		protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
		{
			if (Expander == null)
			{
				return;
			}

			if (propertyName == nameof(IsMouseHover))
			{
				// To disable mouse hover when the accordion item is already in focused state.
				if (Expander.IsSelected)
				{
					_isMouseHover = false;
				}

				InvalidateDrawable();
				if (IsMouseHover)
				{
					if (Expander.HasVisualStateGroups())
					{
						VisualStateManager.GoToState(Expander, "PointerOver");
						UpdateIconColor(Expander.HeaderIconColor);
					}
					else
					{
						UpdateIconColor(Expander.HoverIconColor);
					}
				}
				else
				{
					if (Expander.HasVisualStateGroups())
					{
						VisualStateManager.GoToState(Expander, "Normal");
						VisualStateManager.GoToState(this, Expander.IsExpanded ? "Expanded" : "Collapsed");
					}

					if (!Expander.IsSelected)
					{
						UpdateIconColor(Expander.HeaderIconColor);
					}
				}
			}

			base.OnPropertyChanged(propertyName);
		}

        /// <summary>
        /// Raised when handler gets changed.
        /// </summary>
        protected override void OnHandlerChanged()
        {
            SetUpKeyListenerRequirements();
            base.OnHandlerChanged();
        }

		#endregion

		#region Interface Implementation

		/// <summary>
		/// Invoked when tap on expander header.
		/// </summary>
		/// <param name="e">Event data.</param>
		public async void OnTap(TapEventArgs e)
		{
			if (Expander == null)
			{
				return;
			}

			if (Expander.IsExpanded)
			{
				Expander.RaiseCollapsingEvent();
			}
			else
			{
				Expander.RaiseExpandingEvent();
			}

			if (Expander is AccordionItemView accordionItemView && accordionItemView.Accordion is SfAccordion accordion)
			{
#if ANDROID
				if (!accordion.IsFocused)
				{
					accordion.Focus();
				}
#elif WINDOWS
				if (accordion.Handler != null && accordion.Handler.PlatformView is Microsoft.Maui.Platform.LayoutPanel layoutPanel)
				{
					layoutPanel.Focus(Microsoft.UI.Xaml.FocusState.Programmatic);
				}
#else
				Focus();
#endif

				var currentItem = accordionItemView.Accordion.CurrentItem;
				if (currentItem != null && currentItem._accordionItemView != null && currentItem._accordionItemView.HeaderView != null)
				{
					accordionItemView.Accordion.CurrentItem = null;
					currentItem._accordionItemView.HeaderView.InvalidateDrawable();				
				}

				if (Expander != null && Expander._effectsView != null)
				{
					await Task.Delay((int)Expander._effectsView.RippleAnimationDuration);
				}

				if (!accordionItemView._isRippleAnimationProgress)
				{
					accordionItemView.Accordion.UpdateSelection();
					accordionItemView.IsSelected = true;
				}
			}
			else
			{
#if WINDOWS
				if (Handler != null && Handler.PlatformView is Microsoft.Maui.Platform.LayoutPanel layoutPanel)
				{
					layoutPanel.Focus(Microsoft.UI.Xaml.FocusState.Programmatic);
				}
#else
				Focus();
#endif
			}
		}

		/// <summary>
		/// Raises when mouse hover on element.
		/// </summary>
		/// <param name="e">Represents the event data.</param>
		public void OnTouch(Internals.PointerEventArgs e)
		{
			if (Expander != null && Expander._effectsView != null)
			{
				if (e.Action == PointerActions.Pressed)
				{
					Expander._effectsView.RippleBackground = Expander.HeaderRippleBackground;
					Expander._effectsView.ApplyEffects(SfEffects.Ripple, RippleStartPosition.Default, new System.Drawing.Point((int)e.TouchPoint.X, (int)e.TouchPoint.Y));
					UpdateIconColor(Expander.PressedIconColor);
					Expander._isRippleAnimationProgress = true;
				}

				if (e.Action == PointerActions.Entered)
				{
					IsMouseHover = true;
				}

				if (e.Action == PointerActions.Exited)
				{
					IsMouseHover = false;
				}

				if (e.Action == PointerActions.Released)
				{
					Expander._effectsView.Reset();
					// Restore icon color based on current state.
					if (IsMouseHover)
					{
						if (Expander.HasVisualStateGroups())
						{
							UpdateIconColor(Expander.HeaderIconColor);
						}
						else
						{
							UpdateIconColor(Expander.HoverIconColor);
						}
					}
					else if (!Expander.IsSelected)
					{
						UpdateIconColor(Expander.HeaderIconColor);
					}
				}

				if (e.Action == PointerActions.Cancelled)
				{
					Expander._effectsView.Reset();
					IsMouseHover = false;
					// Restore icon color to normal state since mouse hover is reset
					if (!Expander.IsSelected)
					{
						UpdateIconColor(Expander.HeaderIconColor);
					}
				}
			}
		}

		#endregion
	}


	/// <summary>
	/// A class that represents the icon view of ExpanderHeader.
	/// </summary>
	internal partial class ExpandCollapseButton : SfContentView
    {
		#region Fields

		/// <summary>
		/// Instance of SfExpander.
		/// </summary>
		internal SfExpander _expander;

		/// <summary>
		/// Holds the value for animation duration for ExpandCollapse icon.
		/// </summary>
		readonly uint _iconAnimationDuration = 200;

		/// <summary>
		/// Rotation angle.
		/// </summary>
	    int _rotateAngle = 180;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpandCollapseButton"/> class.
		/// </summary>
		/// <param name="expander">Instance of SfExpander.</param>
		public ExpandCollapseButton(SfExpander expander)
        {
            _expander = expander;
            Content = new Label()
            {
                Text = "\ue705",
                TextColor = _expander.HeaderIconColor,
                FontSize = 16f,
				FontFamily = DeviceInfo.Platform == DevicePlatform.WinUI ? "MauiMaterialAssets.ttf#MauiMaterialAssets" : "MauiMaterialAssets",
				VerticalTextAlignment = Microsoft.Maui.TextAlignment.Center,
                HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center,
            };
        }

		#endregion

		#region Internal Methods

		/// <summary>  
		/// Updates the color of the icon to reflect the current state or configuration.  
		/// </summary>  
		/// <param name="color">Color value.</param>
		internal void UpdateIconColor(Color color)
        {
			if (Content is Label label)
			{
				label.TextColor = color;
			}
        }

		/// <summary>
		/// Rotate the content based on IsExpanded value.
		/// </summary>
		/// <param name="isExpanded">IsExpanded property value.</param>
		internal void RotateIcon(bool isExpanded)
		{
			// ExpandCollapse icon rotates anti clockwise on windows,so we need to rotate it in opposite direction.
			if (DeviceInfo.Platform == DevicePlatform.WinUI)
			{
				_rotateAngle = _expander._isRTL ? -_rotateAngle : _rotateAngle;
			}

			if (Content is Label label)
			{
				double rotationValue = label.Rotation;
				double targetRotation = isExpanded
					? (rotationValue < 0 ? -rotationValue - _rotateAngle : -_rotateAngle)
					: (rotationValue > -_rotateAngle ? -rotationValue : _rotateAngle);

				if (isExpanded || (!isExpanded && _expander.IsViewLoaded))
				{
#if NET10_0_OR_GREATER
                        label.RelRotateToAsync(targetRotation, _iconAnimationDuration);
#else
					    label.RelRotateTo(targetRotation, _iconAnimationDuration);
#endif
				}
			}
		}

		#endregion
	}
}
