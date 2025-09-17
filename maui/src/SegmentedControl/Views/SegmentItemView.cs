using Microsoft.Maui.Controls.Shapes;
using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Toolkit.SegmentedControl
{
	/// <summary>
	/// Represents a view used to display an individual segment item within a segmented control.
	/// </summary>
	internal partial class SegmentItemView : SfContentView, ITouchListener, ITapGestureListener, IKeyboardListener
	{
		#region Fields

		/// <summary>
		/// The border and fill used for item selection view.
		/// </summary>
		SelectionView? _selectionView;

		/// <summary>
		/// The SfEffectsView associated with this SegmentItemView.
		/// </summary>
		SfEffectsView? _effectsView;

		/// <summary>
		/// The image view associated with this SegmentItemView.
		/// </summary>
		Image? _imageView;

		/// <summary>
		/// The segment template view.
		/// </summary>
		View? _itemTemplateView;

		/// <summary>
		/// Gets or sets a value indicating whether the mouse pointer is currently over the control.
		/// </summary>
		bool _isMouseOver;

		/// <summary>
		/// Gets or sets a value indicating whether the selection view can be updated.
		/// </summary>       
		bool _canUpdateSelection;

#if IOS
		/// <summary>
		/// Gets or sets a value indicating whether the touch is moved or not.
		/// </summary>       
		bool _isMoved;
#endif

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SegmentItemView"/> class.
		/// </summary>
		/// <param name="itemInfo">The segment item info.</param>
		/// <param name="item">The segment item.</param>
		internal SegmentItemView(ISegmentItemInfo itemInfo, SfSegmentItem item)
		{
#if __IOS__
			this.IgnoreSafeArea = true;
#endif
			this.itemInfo = itemInfo;
			_segmentItem = item;
			DrawingOrder = DrawingOrder.BelowContent;
			UpdateItemViewEnabledState();
			InitializeSelectionView();
			InitializeImageView();
			this.AddTouchListener(this);
			this.AddGestureListener(this);
#if WINDOWS
			this.AddKeyboardListener(this);
#endif
			if (itemInfo?.SegmentTemplate != null)
			{
				CreateSegmentItemTemplateView();
			}

			InitializeEffectsView();
			UpdateSemantics();
		}

		#endregion

		#region properties

		/// <summary>
		/// The internal information about the segment item.
		/// </summary>
		internal ISegmentItemInfo? itemInfo { get; set; }

		/// <summary>
		/// Gets or sets the SfSegmentItem associated with this SegmentItemView.
		/// </summary>
		internal SfSegmentItem _segmentItem;

		/// <summary>
		/// Gets or sets a value indicating whether this SegmentItemView is currently selected.
		/// </summary>
		internal bool _isSelected;

		#endregion

		#region Internal methods

		/// <summary>
		/// Method to updates the selection.
		/// </summary>
		internal void UpdateSelection()
		{
			_isSelected = true;
			_segmentItem.IsSelected = true;
			_selectionView?.UpdateVisualState(true);
			InvalidateDrawable();
			UpdateSemantics();
		}

		/// <summary>
		/// Clears the selection from the view.
		/// </summary>
		internal void ClearSelection()
		{
			if (_selectionView == null)
			{
				return;
			}

			RemoveEffects();
			_isSelected = false;
			_segmentItem.IsSelected = false;
			_selectionView.UpdateVisualState(_isSelected, false);
			InvalidateDrawable();
			UpdateSemantics();
		}

		/// <summary>
		/// Method to invalidate the segment items layout.
		/// </summary>
		internal void InvalidateLayout()
		{
			InvalidateMeasure();
		}

		/// <summary>
		/// Updates the enabled state of the item view at the specified index.
		/// </summary>
		/// <param name="isEnabled">Determines whether the item view should be enabled (true) or disabled (false).</param>
		internal void UpdateItemViewEnabledState(bool isEnabled)
		{
			// Update the enabled state of the item view.
			IsEnabled = isEnabled;
			InitializeSelectionView();
			if (itemInfo?.SegmentTemplate == null)
			{
				InvalidateDrawable();
			}
			else if (_itemTemplateView != null)
			{
				_itemTemplateView.IsEnabled = isEnabled;
				InvalidateMeasure();
			}
		}

		/// <summary>
		/// Method to update the image view on item selection.
		/// </summary>
		internal void UpdateImageView()
		{
			if (_segmentItem == null || itemInfo?.SegmentTemplate != null)
			{
				return;
			}

			if (_imageView == null)
			{
				InitializeImageView();
			}

			ImageSource imageSource = _segmentItem.ImageSource;
			if (imageSource == null)
			{
				RemoveImageViewHandler();
				InvalidateMeasure();
				return;
			}

			if (_imageView != null)
			{
				_imageView.Source = imageSource;
			}

			InvalidateMeasure();
		}

		/// <summary>
		/// Method to invalidate the drawable views.
		/// </summary>
		internal void InvalidateDrawableView()
		{
			InvalidateDrawable();
			_selectionView?.InvalidateDrawable();
			InvalidateSemantics();
		}

		/// <summary>
		/// Method to dispose the segment item view's instance.
		/// </summary>
		internal void Dispose()
		{
			RemoveSelectionViewHandler();
			RemoveEffectsViewHandler();
			RemoveImageViewHandler();
			this.RemoveTouchListener(this);
			RemoveSegmentItemTemplateView();
			GC.SuppressFinalize(this);
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Invokes on semantics node click.
		/// </summary>
		/// <param name="node">Semantics node.</param>
		void OnSemanticsNodeClick(SemanticsNode node)
		{
			UpdateSelectedIndex();
		}

		/// <summary>
		/// Method to draw the rounded rectangle background for the segment item.
		/// </summary>
		/// <param name="canvas">The canvas used for drawing.</param>
		/// <param name="dirtyRect">The area that needs to be redrawn.</param>
		void DrawRoundedRectangle(ICanvas canvas, RectF dirtyRect)
		{
			if (itemInfo?.SegmentTemplate != null || itemInfo == null)
			{
				return;
			}

			canvas.CanvasSaveState();
			canvas.Antialias = true;
			float strokeRadius = (float)itemInfo.StrokeThickness / 2f;
			CornerRadius cornerRadius = itemInfo.SegmentCornerRadius;
			// Calculate corner radius values, subtracting stroke radius when there's stroke thickness
			float cornerRadiusTopLeft = itemInfo.StrokeThickness > 0 ? (float)cornerRadius.TopLeft - strokeRadius : (float)cornerRadius.TopLeft;
			float cornerRadiusTopRight = itemInfo.StrokeThickness > 0 ? (float)cornerRadius.TopRight - strokeRadius : (float)cornerRadius.TopRight;
			float cornerRadiusBottomRight = itemInfo.StrokeThickness > 0 ? (float)cornerRadius.BottomRight - strokeRadius : (float)cornerRadius.BottomRight;
			float cornerRadiusBottomLeft = itemInfo.StrokeThickness > 0 ? (float)cornerRadius.BottomLeft - strokeRadius : (float)cornerRadius.BottomLeft;

			// Always draw the background
			bool isEnabled = SegmentViewHelper.GetItemEnabled(itemInfo, _segmentItem);
			Brush background = isEnabled ? SegmentViewHelper.GetSegmentBackground(itemInfo, _segmentItem) : itemInfo.DisabledSegmentBackground;
			canvas.FillColor = SegmentViewHelper.BrushToColorConverter(background);
			canvas.FillRoundedRectangle(dirtyRect.Left, dirtyRect.Top, dirtyRect.Width, dirtyRect.Height, cornerRadiusTopLeft, cornerRadiusTopRight, cornerRadiusBottomRight, cornerRadiusBottomLeft);
			canvas.CanvasRestoreState();
		}

		/// <summary>
		/// Method to draw a filled rounded rectangle.
		/// </summary>
		/// <remarks>
		/// Handling the selection mode as Fill.
		/// </remarks>
		/// <param name="canvas">The canvas used for drawing.</param>
		/// <param name="dirtyRect">The area that needs to be redrawn.</param>
		void DrawSelectionFillRectangle(ICanvas canvas, RectF dirtyRect)
		{
			if (itemInfo == null || itemInfo.SelectionIndicatorSettings.SelectionIndicatorPlacement != SelectionIndicatorPlacement.Fill)
			{
				return;
			}

			canvas.CanvasSaveState();
			canvas.Antialias = true;

			// Get the corner radius settings.
			CornerRadius cornerRadius = itemInfo.SegmentCornerRadius;
			float cornerRadiusTopLeft = (float)cornerRadius.TopLeft;
			float cornerRadiusTopRight = (float)cornerRadius.TopRight;
			float cornerRadiusBottomRight = (float)cornerRadius.BottomRight;
			float cornerRadiusBottomLeft = (float)cornerRadius.BottomLeft;

			// Determine the background color based on selection and mouse-over states.
			Brush mouseOverBackground = GetBackgroundValue();
			canvas.FillColor = SegmentViewHelper.BrushToColorConverter(mouseOverBackground);

			// Draw the filled rounded rectangle.
			canvas.FillRoundedRectangle(dirtyRect.Left, dirtyRect.Top, dirtyRect.Width, dirtyRect.Height, cornerRadiusTopLeft, cornerRadiusTopRight, cornerRadiusBottomLeft, cornerRadiusBottomRight);
			canvas.CanvasRestoreState();
		}

		/// <summary>
		/// Method to draw the text and image for the segment item.
		/// </summary>
		/// <param name="canvas">The canvas used for drawing.</param>
		/// <param name="dirtyRect">The area that needs to be redrawn.</param>
		void DrawTextWithImage(ICanvas canvas, RectF dirtyRect)
		{
			if (itemInfo?.SegmentTemplate != null || itemInfo == null)
			{
				return;
			}

			canvas.CanvasSaveState();
			canvas.Antialias = true;
			canvas.StrokeSize = (float)itemInfo.StrokeThickness;

			Rect textRect = dirtyRect;
			// Calculate the center position for the image.
			string text = _segmentItem.Text;
			ImageSource image = _segmentItem.ImageSource;
			SegmentTextStyle textStyle = SegmentViewHelper.GetClonedSegmentTextStyle(itemInfo, _segmentItem);
			bool isEnabled = SegmentViewHelper.GetItemEnabled(itemInfo, _segmentItem);
			if (isEnabled && _isSelected)
			{
				textStyle.TextColor = SegmentViewHelper.GetSelectedSegmentForeground(itemInfo, _segmentItem);
			}
			else
			{
				textStyle.TextColor = isEnabled ? textStyle.TextColor : itemInfo.DisabledSegmentTextColor;
			}

			HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;

			// Draw the image.
			if (image != null)
			{
				RectF imageRect = GetImageRect(dirtyRect);
				// The text and image are centrally aligned with equal padding.
				double padding = 8;
				horizontalAlignment = string.IsNullOrEmpty(text) ? HorizontalAlignment.Center : HorizontalAlignment.Left;
				textRect = SegmentItemView.GetTextRect(dirtyRect, imageRect, padding);
			}

			// Trim the text based on the text rect.
			string trimmedText = SegmentItemViewHelper.TrimText(text, textRect.Width, textStyle);
			// Draw the text.
			SegmentItemViewHelper.DrawText(canvas, trimmedText, textStyle, textRect, horizontalAlignment, VerticalAlignment.Center);
			canvas.CanvasRestoreState();
		}

		/// <summary>
		/// Calculates the rectangle for the text based on the specified dirty rectangle, image rectangle, and padding.
		/// </summary>
		/// <param name="dirtyRect">The dirty rectangle within which the text is to be placed.</param>
		/// <param name="imageRect">The rectangle representing the image within the segment item.</param>
		/// <param name="padding">The padding value to be used for positioning the text.</param>
		/// <returns>The calculated rectangle for the text.</returns>
		static Rect GetTextRect(RectF dirtyRect, RectF imageRect, double padding)
		{
			return new Rect(imageRect.Right + padding, dirtyRect.Top, dirtyRect.Width - (imageRect.Right + padding), dirtyRect.Height);
		}

		/// <summary>
		/// Method to get the rectangle for the image in the segment item.
		/// </summary>
		/// <param name="dirtyRect">The area that needs to be redrawn.</param>
		/// <returns>The rectangle representing the image position.</returns>
		RectF GetImageRect(RectF dirtyRect)
		{
			double imageSize = _segmentItem.ImageSize;
			SegmentTextStyle textStyle = new SegmentTextStyle();
			if (itemInfo != null)
			{
				textStyle = SegmentViewHelper.GetSegmentTextStyle(itemInfo, _segmentItem);
			}

			Size textSize = SegmentItemViewHelper.GetTextSize(_segmentItem.Text, textStyle);

			// The text and image are centrally aligned with equal padding.
			double padding = 4;
			double totalWidth = imageSize + textSize.Width;

			// Calculate the horizontal center of the dirtyRect.
			double centerX = dirtyRect.Width / 2;

			// Calculate the vertical center of the dirtyRect.
			double centerY = dirtyRect.Height / 2;

			double availedSize = centerX - (totalWidth / 2);
			double imageYPos = centerY - (imageSize / 2);
			double imageXPos = string.IsNullOrEmpty(_segmentItem.Text) ? availedSize : availedSize - padding;

			if (imageXPos < dirtyRect.Left)
			{
				// Adjust xPos if it goes beyond the left boundary.
				imageXPos = dirtyRect.Left;
			}
			else if (imageXPos + imageSize > dirtyRect.Width)
			{
				// Adjust xPos if it goes beyond the right boundary.
				imageXPos = dirtyRect.Width - imageSize;
			}
			else if (imageXPos + imageSize + textSize.Width > dirtyRect.Width)
			{
				// Adjust xPos if it exceeds the available width after considering the text size.
				imageXPos = dirtyRect.Width - (imageSize + textSize.Width);
			}

			return new Rect(imageXPos, imageYPos, imageSize, imageSize);
		}

		/// <summary>
		/// Initializes the image view for the segment item's icon.
		/// </summary>
		void InitializeImageView()
		{
			if (_segmentItem == null || _segmentItem.ImageSource == null ||
				itemInfo?.SegmentTemplate != null)
			{
				return;
			}

			_imageView = new Image
			{
				Source = _segmentItem.ImageSource,
#if !ANDROID
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
#endif
#if MACCATALYST || IOS
				WidthRequest = _segmentItem.ImageSize,
				HeightRequest = _segmentItem.ImageSize,
#endif
			};

#if WINDOWS
			AutomationProperties.SetIsInAccessibleTree(_imageView, false);
#endif
			Children.Add(_imageView);
		}

		/// <summary>
		/// Initializes the selection view for the segment item's selection state.
		/// </summary>
		void InitializeSelectionView()
		{
			UpdateMouseOver(false);
			if (_segmentItem == null || itemInfo == null || !IsEnabled)
			{
				return;
			}

			_selectionView = new SelectionView(_segmentItem, itemInfo);
#if WINDOWS
			AutomationProperties.SetIsInAccessibleTree(_selectionView, false);
#endif
			Children.Add(_selectionView);
		}

		/// <summary>
		/// Initializes the effective view for the segment item's selection animation.
		/// </summary>
		/// <remarks>
		/// This method sets up the view to be animated when a segment item is selected.
		/// </remarks>

		void InitializeEffectsView()
		{
			_effectsView = [];

			// Setting true, the EffectsView will ignore touches, allowing the SegmentItemView to handle segment selection.
			_effectsView.ShouldIgnoreTouches = true;
			_effectsView.AnimationCompleted += OnEffectsViewAnimationCompleted;
			_effectsView.ClipToBounds = true;
#if WINDOWS
			AutomationProperties.SetIsInAccessibleTree(_effectsView, false);
#endif
			Children.Add(_effectsView);
		}

		/// <summary>
		/// Updates the layout of the selection and effects views based on the segment item's properties.
		/// </summary>
		/// <param name="bounds">The bounds of the segment item.</param>
		void SetClipBounds(Rect bounds)
		{
			if (_selectionView == null || _effectsView == null || itemInfo == null)
			{
				return;
			}

			// Get the rectangle for clipping based on the current bounds.
			RectF clipRect = new RectF(0, 0, (float)bounds.Width, (float)bounds.Height);
			CornerRadius segmentCornerRadius = itemInfo.SegmentCornerRadius;
			_selectionView.Clip = new RoundRectangleGeometry()
			{
				Rect = clipRect,
				CornerRadius = segmentCornerRadius,
			};

			_effectsView.Clip = new RoundRectangleGeometry()
			{
				Rect = clipRect,
				CornerRadius = segmentCornerRadius,
			};
		}

		/// <summary>
		/// Method to update the enabled property value to item view.
		/// </summary>
		void UpdateItemViewEnabledState()
		{
			if (itemInfo == null || _segmentItem == null)
			{
				return;
			}

			IsEnabled = SegmentViewHelper.GetItemEnabled(itemInfo, _segmentItem);
		}

		/// <summary>
		/// Method used to update the selection index.
		/// </summary>
		void UpdateSelectedIndex()
		{
			if (_segmentItem == null || itemInfo == null || itemInfo.Items == null)
			{
				return;
			}

			int index = itemInfo.Items.IndexOf(_segmentItem);
			if (index == -1)
			{
				index = 0;
			}

			// Clear the focused state of the previously focused item.
			itemInfo.ClearFocusedView();
			if (index != itemInfo.SelectedIndex)
			{
				itemInfo.UpdateSelectedIndex(index);
			}
			else if (this.itemInfo.SelectionMode == SegmentSelectionMode.SingleDeselect)
			{
				this.itemInfo.UpdateSelectedIndex(-1);
			}
		}

		/// <summary>
		/// Updates the view based on mouse hover state.
		/// </summary>
		/// <param name="isMouseOver">Indicates whether the mouse pointer is over the view.</param>
		void UpdateMouseOver(bool isMouseOver)
		{
			_isMouseOver = isMouseOver;
			_selectionView?.UpdateVisualState(_isSelected, isMouseOver);
			InvalidateDrawable();
			if (isMouseOver)
			{
				_effectsView?.ApplyEffects(SfEffects.Highlight);
			}
			else
			{
				RemoveEffects();
			}
		}

		/// <summary>
		/// Gets the background brush value for the segment item based on its selection state and mouse hover state.
		/// </summary>
		/// <returns>The background brush for the segment item.</returns>
		Brush GetBackgroundValue()
		{
			Brush background = Brush.Transparent;
			if (itemInfo == null || _segmentItem == null)
			{
				return background;
			}

			if (_isSelected)
			{
				background = _isMouseOver ? SegmentViewHelper.GetSelectedSegmentHoveredBackground(itemInfo, _segmentItem)
					: SegmentViewHelper.GetSelectedSegmentBackground(itemInfo, _segmentItem);
			}
			else if (_isMouseOver)
			{
				background = SegmentViewHelper.GetHoveredSegmentBackground(itemInfo);
			}

			return background;
		}

		/// <summary>
		/// Occurs when the ripple animation is completed.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The event arguments.</param>
		void OnEffectsViewAnimationCompleted(object? sender, EventArgs e)
		{
			// Updates the selection view for the selected item in the effects view animation changed event for the tap.
			if (_canUpdateSelection)
			{
				UpdateSelectedIndex();
#if ANDROID || IOS
				// Remove the effects when touch is released on Android and iOS platforms, while on other platforms, it handles mouse hovering effects on touch enter and exit.
				RemoveEffects();
#endif
			}
			else
			{
				bool isSelected = this.itemInfo?.SelectedIndex == this.itemInfo?.Items?.IndexOf(this._segmentItem);
				if (isSelected && this.itemInfo?.SelectionMode == SegmentSelectionMode.SingleDeselect)
				{
					this.itemInfo.UpdateSelectedIndex(-1);
				}
			}

			_canUpdateSelection = false;
		}

		/// <summary>
		/// Clears the visual effects applied to the segment item view.
		/// </summary>
		void RemoveEffects()
		{
			if (_effectsView == null)
			{
				return;
			}

			_effectsView.Reset();
		}

		/// <summary>
		/// Method to update the semantic properties.
		/// </summary>
		void UpdateSemantics()
		{
			if (_segmentItem != null)
			{
				if (!string.IsNullOrEmpty(SemanticProperties.GetDescription(_segmentItem)))
				{
					SemanticProperties.SetDescription(this, SemanticProperties.GetDescription(_segmentItem));
				}
				else if (!_segmentItem.IsEnabled || (itemInfo != null && !itemInfo.IsEnabled))
				{
					SemanticProperties.SetDescription(this, _segmentItem.Text + SfSegmentedResources.GetLocalizedString("Disabled"));
				}
				else
				{
					SemanticProperties.SetDescription(this, _isSelected ? _segmentItem.Text + SfSegmentedResources.GetLocalizedString("Selected") : _segmentItem.Text);
				}
			}
		}

		/// <summary>
		/// Method to remove the selection view handlers.
		/// </summary>
		void RemoveSelectionViewHandler()
		{
			if (_selectionView == null)
			{
				return;
			}

			Remove(_selectionView);
			if (_selectionView.Handler != null && _selectionView.Handler.PlatformView != null)
			{
				_selectionView.Handler.DisconnectHandler();
			}

			_selectionView.Dispose();
			_selectionView = null;
		}

		/// <summary>
		/// Method to remove the image view handlers.
		/// </summary>
		void RemoveImageViewHandler()
		{
			if (_imageView == null)
			{
				return;
			}

			Remove(_imageView);
			if (_imageView.Handler != null && _imageView.Handler.PlatformView != null)
			{
				_imageView.Handler.DisconnectHandler();
			}

			_imageView = null;
		}

		/// <summary>
		/// Method to remove the effects view handlers.
		/// </summary>
		void RemoveEffectsViewHandler()
		{
			if (_effectsView == null)
			{
				return;
			}

			Remove(_effectsView);
			if (_effectsView.Handler != null && _effectsView.Handler.PlatformView != null)
			{
				_effectsView.Handler.DisconnectHandler();
			}

			_effectsView = null;
		}

		/// <summary>
		/// Method to remove the item template view handlers.
		/// </summary>
		void RemoveSegmentItemTemplateView()
		{
			if (_itemTemplateView == null || !this.Contains(_itemTemplateView))
			{
				return;
			}

			Remove(_itemTemplateView);
			if (_itemTemplateView.Handler != null && _itemTemplateView.Handler.PlatformView != null)
			{
				_itemTemplateView.Handler.DisconnectHandler();
			}

			_itemTemplateView = null;
		}

		/// <summary>
		/// Method to create the segment item template views.
		/// </summary>
		void CreateSegmentItemTemplateView()
		{
			if (itemInfo == null || _segmentItem == null)
			{
				return;
			}

			DataTemplateSelector? templateSelector = null;
			DataTemplate segmentTemplate = itemInfo.SegmentTemplate;
			bool isRTL = itemInfo.IsRTL;
			if (segmentTemplate is DataTemplateSelector selector)
			{
				templateSelector = selector;
			}

			if (templateSelector != null)
			{
				DataTemplate template = templateSelector.SelectTemplate(_segmentItem, this);
				_itemTemplateView = SegmentItemViewHelper.CreateTemplateView(template, _segmentItem, isRTL);
			}
			else
			{
				_itemTemplateView = SegmentItemViewHelper.CreateTemplateView(segmentTemplate, _segmentItem, isRTL);
			}

			if (_itemTemplateView != null)
			{
				_itemTemplateView.IsEnabled = SegmentViewHelper.GetItemEnabled(itemInfo, _segmentItem);
				Children.Add(_itemTemplateView);
			}
		}

		/// <summary>
		/// Method to trigger the tapped event for the segment item.
		/// </summary>
		private void TriggerTappedEvent()
		{
			if (this.itemInfo == null || this._segmentItem == null)
			{
				return;
			}

			SegmentTappedEventArgs eventArgs = new SegmentTappedEventArgs() { SegmentItem = this._segmentItem };
			this.itemInfo.TriggerTappedEvent(eventArgs);
		}

		#endregion

		#region Override methods

		/// <summary>
		/// Called when the view needs to be redrawn.
		/// </summary>
		/// <param name="canvas">The canvas used for drawing.</param>
		/// <param name="dirtyRect">The area that needs to be redrawn.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			SetClipBounds(dirtyRect);
			DrawRoundedRectangle(canvas, dirtyRect);
			DrawSelectionFillRectangle(canvas, dirtyRect);
			DrawTextWithImage(canvas, dirtyRect);
		}

		/// <summary>
		/// Method to get semantics nodes.
		/// </summary>
		/// <param name="width">The available width.</param>
		/// <param name="height">The available height.</param>
		/// <returns>The semantics nodes.</returns>
		protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
		{
			SemanticsNode semanticsNode = new SemanticsNode()
			{
				Id = 0,
				Bounds = new Rect(0, 0, width, height),
				IsTouchEnabled = true,
			};

			if (_segmentItem.IsEnabled)
			{
				semanticsNode.OnClick = OnSemanticsNodeClick;
			}
			string text = _segmentItem.Text;
			if (!string.IsNullOrEmpty(SemanticProperties.GetDescription(_segmentItem)))
			{
				semanticsNode.Text = SemanticProperties.GetDescription(_segmentItem);
			}
			else if (!_segmentItem.IsEnabled || (itemInfo != null && !itemInfo.IsEnabled))
			{
				semanticsNode.Text = text + SfSegmentedResources.GetLocalizedString("Disabled");
			}
			else
			{
				semanticsNode.Text = _isSelected ? text + SfSegmentedResources.GetLocalizedString("Selected") : text;
			}

			return [semanticsNode];
		}

#if WINDOWS
		/// <summary>
		/// Raises when <see cref="SegmentItemView"/>'s handler gets changed.
		/// <exclude/>
		/// </summary>
		protected override void OnHandlerChanged()
		{
			base.OnHandlerChanged();
			if (this.Handler != null && this.Handler.PlatformView != null && this.Handler.PlatformView is Microsoft.UI.Xaml.UIElement nativeView)
			{
				nativeView.IsTabStop = true;
			}
		}
#endif

		/// <summary>
		/// Measures the size of the view's content based on the specified constraints.
		/// </summary>
		/// <param name="widthConstraint">The width constraint for the view.</param>
		/// <param name="heightConstraint">The height constraint for the view.</param>
		/// <returns>The size of the view's content based on the specified constraints.</returns>
		protected override Size MeasureContent(double widthConstraint, double heightConstraint)
		{
			foreach (var child in Children)
			{
				if (child is Image)
				{
					double iconSize = _segmentItem.ImageSize;
					child.Measure(iconSize, iconSize);
				}
				else
				{
					child.Measure(widthConstraint, heightConstraint);
				}
			}

			return new Size(widthConstraint, heightConstraint);
		}

		/// <summary>
		/// Arranges the child elements within the view's bounds.
		/// </summary>
		/// <param name="bounds">The view's bounds.</param>
		/// <returns>The final size of the view after arranging its child elements.</returns>
		protected override Size ArrangeContent(Rect bounds)
		{
			foreach (var child in Children)
			{
				if (child is Image)
				{
					// Set the layout bounds for the image view.
					child.Arrange(GetImageRect(bounds));
				}
				else if (child is View)
				{
					// Set the layout bounds for the selection, effects and item template view.
					child.Arrange(bounds);
				}
			}

			return bounds.Size;
		}

		#endregion

		#region Interface Implementations

		/// <summary>
		/// Event handler for touch events on the <see cref="SegmentItemView"/>.
		/// </summary>
		/// <param name="e">The <see cref="PointerEventArgs"/> representing the touch event.</param>
		void ITouchListener.OnTouch(PointerEventArgs e)
		{
			if (e.Action == PointerActions.Pressed)
			{
				if (itemInfo != null && itemInfo.EnableRippleEffect)
				{
					_effectsView?.ApplyEffects();
				}
			}
#if __MACCATALYST__ || WINDOWS
			else if (e.Action == PointerActions.Entered && e.PointerDeviceType == PointerDeviceType.Mouse)
			{
				UpdateMouseOver(true);
			}
			else if (e.Action == PointerActions.Exited)
			{
				UpdateMouseOver(false);
			}
#endif
#if IOS
			else if (e.Action == PointerActions.Moved)
			{
				_isMoved = true;
			}
			else if (e.Action == PointerActions.Released)
			{
				if (_isMoved)
				{
					RemoveEffects();
					_isMoved = false;
				}
			}
#endif
#if ANDROID || IOS
			else if (e.Action == PointerActions.Cancelled || e.Action == PointerActions.Released)
			{
				RemoveEffects();
			}
#endif
		}

		/// <summary>
		/// Event handler for touch events on the <see cref="SegmentItemView"/>.
		/// </summary>
		/// <param name="e">The tap event args.</param>
		void ITapGestureListener.OnTap(Syncfusion.Maui.Toolkit.Internals.TapEventArgs e)
		{
			this.TriggerTappedEvent();
			if (_effectsView.AnimationIsRunning("RippleAnimator"))
			{
				_canUpdateSelection = true;
				return;
			}

			// Disables the selection view update during the animation change event triggered by a long press.
			_canUpdateSelection = false;

			// Update of the selection view for the currently selected item for the long press.
			UpdateSelectedIndex();
#if ANDROID || IOS
			// Remove the effects when touch is released on Android and iOS platforms, while on other platforms, it handles mouse hovering effects on touch enter and exit.
			RemoveEffects();
#endif
		}

		/// <summary>
		/// Gets a value indicating whether the view can become the first responder to listen the keyboard actions.
		/// </summary>
		/// <remarks>This property will be considered only in iOS Platform.</remarks>
		bool IKeyboardListener.CanBecomeFirstResponder
		{
			get { return true; }
		}

		/// <inheritdoc/>
		void IKeyboardListener.OnKeyDown(KeyEventArgs e)
		{
			if (e.Key == KeyboardKey.Enter)
			{
				UpdateSelectedIndex();
			}
		}

		/// <inheritdoc/>
		void IKeyboardListener.OnKeyUp(KeyEventArgs args)
		{

		}

		#endregion
	}
}