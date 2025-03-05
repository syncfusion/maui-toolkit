using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.EffectsView
{
	/// <summary>
	/// Represents the RippleEffectLayer class.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the <see cref="RippleEffectLayer"/> class.
	/// </remarks>
	/// <param name="rippleColor">The ripple color</param>
	/// <param name="rippleDuration">The ripple duration</param>
	/// <param name="drawable">The drawable</param>
	/// <param name="animate">The animate</param>
	internal class RippleEffectLayer(Brush rippleColor, double rippleDuration, IDrawable drawable, IAnimatable animate)
	{
		#region Fields
#if ANDROID
		const float RippleTransparencyFactor = 0.4f;
#else
		const float RippleTransparencyFactor = 0.12f;
#endif
		float _rippleDiameter;
		readonly string _rippleAnimatorName = "RippleAnimator";
		readonly string _fadeOutName = "RippleFadeOut";
		Point _touchPoint;
		double _animationAreaLength;
		float _alphaValue = RippleTransparencyFactor;
		bool _fadeOutRipple;
		Brush _rippleColor = rippleColor;
		double _rippleAnimationDuration = rippleDuration;
		readonly float _minAnimationDuration = 1f;
		readonly IDrawable _drawable = drawable;
		readonly IAnimatable _animation = animate;
		bool _isEffectsRenderer;
		EffectsRenderer? _effectsRenderer;
		double _effectsRendererWidth;
		double _effectsRendererHeight;
		readonly RadialGradientBrush _radialGradientBrush = new();
		readonly GradientStop _firstGradientStop = new();
		readonly GradientStop _secondGradientStop = new(Colors.Transparent, 1f);
		readonly GradientStopCollection _gradientStopCollection = [];

		#endregion
		#region Constructor

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the ripple effect layer width.
		/// </summary>
		internal double Width { get; set; }

		/// <summary>
		/// Gets or sets the ripple effect layer height.
		/// </summary>
		internal double Height { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to clear the animation.
		/// </summary>
		internal bool CanRemoveRippleAnimation { get; set; }

		/// <summary>
		/// Gets the ripple fade in and fade out animation duration in milliseconds.
		/// </summary>
		float RippleFadeInOutAnimationDuration
		{
			get
			{
				return (float)((_rippleAnimationDuration < _minAnimationDuration ? _minAnimationDuration : _rippleAnimationDuration) / 4);
			}
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Method to draw ripple.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		/// <param name="dirtyRect">The rectangle.</param>
		internal void DrawRipple(ICanvas canvas, RectF dirtyRect)
		{
			if (_rippleColor != null)
			{
				canvas.Alpha = _alphaValue;
				DrawRipple(canvas, dirtyRect, _rippleColor, false);
			}
		}

		/// <summary>
		/// Method to draw ripple.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		/// <param name="dirtyRect">The rectangle.</param>
		/// <param name="color">The color.</param>
		/// <param name="clipBounds">The clip bounds value.</param>
#pragma warning disable IDE0060 // Remove unused parameter
		internal void DrawRipple(ICanvas canvas, RectF dirtyRect, Brush color, bool clipBounds = false)
#pragma warning restore IDE0060 // Remove unused parameter
		{
			if (_rippleColor != null)
			{
				color = UpdateToGradient(color);
				canvas.SetFillPaint(color, dirtyRect);
				ExpandRippleEllipse(canvas);
			}
		}

		/// <summary>
		/// Start ripple animation method.
		/// </summary>
		/// <param name="point">The touch point.</param>
		/// <param name="rippleColor">The ripple color.</param>
		/// <param name="rippleAnimationDuration">The ripple aniamtion duration.</param>
		/// <param name="initialRippleFactor">The initial ripple factor value.</param>
		/// <param name="fadeoutRipple">The fadeout ripple property.</param>
		/// <param name="canRepeat">The can repeat value.</param>
		internal void StartRippleAnimation(Point point, Brush rippleColor, double rippleAnimationDuration, float initialRippleFactor, bool fadeoutRipple, bool canRepeat = false)
		{
			if (DeviceInfo.Platform == DevicePlatform.WinUI && ((_drawable as IVisualElementController)?.EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft)
			{
				_touchPoint = new Point(Width - point.X, point.Y);
			}
			else
			{
				_touchPoint = point;
			}

			_rippleColor = rippleColor;
			_rippleAnimationDuration = rippleAnimationDuration;
			_fadeOutRipple = fadeoutRipple;
			_alphaValue = RippleTransparencyFactor;
			double initialRippleRadius = GetRippleRadiusFromFactor(initialRippleFactor);
			_animationAreaLength = GetFinalRadius(point);

			var rippleRadiusAnimation = new Animation(OnRippleAnimationUpdate, initialRippleRadius, _animationAreaLength);
			rippleRadiusAnimation.Commit(
				_animation,
				_rippleAnimatorName,
				length: (uint)rippleAnimationDuration,
				easing: Easing.Linear,
				finished: OnRippleFinished,
				repeat: () => canRepeat);

			if (fadeoutRipple)
			{
				var fadeOutAnimation = new Animation(OnFadeAnimationUpdate, 0, _alphaValue);
				fadeOutAnimation.Commit(
					_animation,
					_fadeOutName,
					length: (uint)RippleFadeInOutAnimationDuration,
					easing: Easing.Linear,
					finished: null,
					repeat: () => canRepeat);
				rippleRadiusAnimation.WithConcurrent(fadeOutAnimation);
			}
		}

		/// <summary>
		/// Start ripple animation for renderer.
		/// </summary>
		/// <param name="point">The touch point.</param>
		/// <param name="rippleColor">The ripple color.</param>
		/// <param name="rippleAnimationDuration">The ripple animation duration.</param>
		/// <param name="initialRippleFactor">The initial ripple factor value.</param>
		/// <param name="fadeoutRipple">The fadeout ripple property.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="effectsRenderer">EffectsRenderer.</param>
		internal void StartRippleAnimation(Point point, Brush rippleColor, double rippleAnimationDuration, float initialRippleFactor, bool fadeoutRipple, double width, double height, EffectsRenderer effectsRenderer)
		{
			_isEffectsRenderer = true;
			if (effectsRenderer != null)
			{
				_effectsRenderer = effectsRenderer;
			}

			_effectsRendererWidth = width;
			_effectsRendererHeight = height;
			StartRippleAnimation(point, rippleColor, rippleAnimationDuration, initialRippleFactor, fadeoutRipple);
		}

		/// <summary>
		/// Fadeanimation update method.
		/// </summary>
		/// <param name="value">The animation update value.</param>
		internal void OnFadeAnimationUpdate(double value)
		{
			_alphaValue = (float)value;
			InvalidateDrawable();
		}

		/// <summary>
		/// Ripple animation update method.
		/// </summary>
		/// <param name="value">Animation update value.</param>
		internal void OnRippleAnimationUpdate(double value)
		{
			_rippleDiameter = (float)value;
			_radialGradientBrush.Radius = ((_rippleDiameter * 1.1) / _animationAreaLength) + 0.1;
			InvalidateDrawable();
		}

		/// <summary>
		/// Ripple animation finished method.
		/// </summary>
		internal void OnRippleAnimationFinished()
		{
			AnimationExtensions.AbortAnimation(_animation, _rippleAnimatorName);
			_rippleDiameter = 0;
			InvalidateDrawable();
		}

		#endregion

		#region Private methods

		/// <summary>
		/// This method is used to convert the brush color to radial gradient color.
		/// </summary>
		/// <param name="colorValue">The color value.</param>
		/// <returns></returns>
		Microsoft.Maui.Controls.RadialGradientBrush UpdateToGradient(Brush colorValue)
		{
			_gradientStopCollection.Clear();
			_firstGradientStop.Color = ((SolidColorBrush)colorValue).Color;
			_firstGradientStop.Offset = 0.1f;
			_gradientStopCollection.Add(_firstGradientStop);
			_gradientStopCollection.Add(_secondGradientStop);
			_radialGradientBrush.GradientStops = _gradientStopCollection;
			if (Width > 0 && Height > 0)
			{
				_radialGradientBrush.Center = new Point((_touchPoint.X / Width), (_touchPoint.Y / Height));

			}
			else if (GetParent() is View parent && parent.Width > 0 && parent.Height > 0)
			{
				_radialGradientBrush.Center = new Point((_touchPoint.X / parent.Width), (_touchPoint.Y / parent.Height));
			}

			return _radialGradientBrush;
		}

		/// <summary>
		/// Method to invalidate drawable.
		/// </summary>
		void InvalidateDrawable()
		{
			if (_drawable is IDrawableLayout drawableLayout)
			{
				drawableLayout.InvalidateDrawable();
			}
			else if (_drawable is IDrawableView drawableView)
			{
				drawableView.InvalidateDrawable();
			}
		}

		/// <summary>
		/// Method to get parent.
		/// </summary>
		/// <returns>Returns the parent layout or view.</returns>
		Microsoft.Maui.IElement? GetParent()
		{
			if (_drawable is IDrawableLayout drawableLayout)
			{
				return drawableLayout;
			}
			else if (_drawable is IDrawableView drawableView)
			{
				return drawableView;
			}

			return null;
		}

		/// <summary>
		/// Method used to initial radius for ripple.
		/// </summary>
		/// <param name="initialRippleFactor">The initial ripple factor.</param>
		/// <returns>Returns the converted radius value.</returns>
		float GetRippleRadiusFromFactor(float initialRippleFactor)
		{
			if (Width > 0 && Height > 0)
			{
				return (float)(Math.Min(Width, Height) / 2 * initialRippleFactor);
			}
			else if (GetParent() is View parent && parent.Width > 0 && parent.Height > 0)
			{
				return Math.Min((float)parent.Width, (float)parent.Height) / 2 * initialRippleFactor;
			}

			return 0;
		}

		/// <summary>
		/// Get the maximum radius based on the pythagoras theorem in the view.
		/// </summary>
		/// <param name="pivot">The touch point.</param>
		/// <returns>Final radius.</returns>
		float GetFinalRadius(Point pivot)
		{
			if (!_isEffectsRenderer)
			{
				if (Width > 0 && Height > 0)
				{
					float width = (float)(pivot.X > Width / 2 ? pivot.X : Width - pivot.X);
					float height = (float)(pivot.Y > Height / 2 ? pivot.Y : Height - pivot.Y);
					return (float)Math.Sqrt((width * width) + (height * height));
				}
				else if (GetParent() is View parent && parent.Width > 0 && parent.Height > 0)
				{
					float parentWidth = (float)parent.Width;
					float parentHeight = (float)parent.Height;
					float width = (float)(pivot.X > parentWidth / 2 ? pivot.X : parentWidth - pivot.X);
					float height = (float)(pivot.Y > parentHeight / 2 ? pivot.Y : parentHeight - pivot.Y);
					return (float)Math.Sqrt((width * width) + (height * height));
				}
				else
				{
					return (float)Math.Sqrt((pivot.X * pivot.X) + (pivot.Y * pivot.Y));
				}
			}
			else
			{
				return (float)Math.Sqrt((_effectsRendererWidth * _effectsRendererWidth) + (_effectsRendererHeight * _effectsRendererHeight));
			}

		}

		/// <summary>
		/// Expand ripple ellipse method.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		void ExpandRippleEllipse(ICanvas canvas)
		{
			canvas.FillCircle((float)_touchPoint.X, (float)_touchPoint.Y, _rippleDiameter);
		}

		/// <summary>
		/// Ripple animation finished method.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="isCompleted">The completed property.</param>
		void OnRippleFinished(double value, bool isCompleted)
		{
			if (CanRemoveRippleAnimation)
			{
				AnimationExtensions.AbortAnimation(_animation, _rippleAnimatorName);
				_rippleDiameter = 0;
				InvalidateDrawable();
			}

			if (CanRemoveRippleAnimation || !_animation.AnimationIsRunning(_rippleAnimatorName))
			{
				if (GetParent() != null && ((_drawable as View) as SfEffectsView) != null)
				{
					if ((GetParent() as View) is SfEffectsView effectView && ((effectView.TouchUpEffects == SfEffects.None || effectView.AutoResetEffects.GetAllAutoResetEffectsItems().Contains(AutoResetEffects.Ripple) || effectView.TouchUpEffects == SfEffects.Ripple || effectView.TouchUpEffects.GetAllItems().Contains(SfEffects.Ripple) || effectView.TouchUpEffects.GetAllItems().Contains(SfEffects.None)) &&
						(effectView.LongPressEffects.GetAllItems().Contains(SfEffects.None) || !effectView.LongPressHandled || effectView.LongPressEffects.GetAllItems().Contains(SfEffects.Ripple))))
					{
						effectView?.InvokeAnimationCompletedEvent();
					}
				}
			}

			if (_isEffectsRenderer && _effectsRenderer != null)
			{
				_isEffectsRenderer = false;
				_effectsRenderer?.RaiseAnimationCompletedEvent(EventArgs.Empty);
			}
		}

		#endregion
	}
}
