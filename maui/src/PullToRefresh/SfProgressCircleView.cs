using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.PullToRefresh
{
	/// <summary>
	/// This is the refresh view of <see cref="SfPullToRefresh"/>.
	/// </summary>
	internal partial class SfProgressCircleView : SfContentView
    {
        #region Fields
		
		Rect _circleViewBounds;
        Rect _processedBounds;
        Rect _oval;
        Rect _fillRect;
        const float MinArcLength = 21.6f;
        const float MaxArcLength = 280.8f;
        const float ArcMovementAngle = 7f;
        const float ArcIncrement = 22f;
        int _angleMaintenanceCounter = 0;
        WeakReference<SfPullToRefresh>? _pullToRefresh;
        float _startAngle = 90f;
        float _endAngle;
        bool _isInShift = false;
        bool _isArcCollapsing;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfProgressCircleView"/> class using the specified values.
        /// </summary>
        /// <param name="pullToRefresh">The reference of the associated <see cref="SfPullToRefresh"/> object.</param>
        internal SfProgressCircleView(SfPullToRefresh pullToRefresh)
        {
            PullToRefresh = pullToRefresh;
            ZIndex = 1;
            DrawingOrder = DrawingOrder.BelowContent;
            ClipToBounds = true;
            _endAngle = _startAngle - MinArcLength;
            UpdateDrawProperties();
        }

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the circle view bounds.
		/// </summary>
		internal Rect CircleViewBounds
		{
			get { return _circleViewBounds; }
			set { _circleViewBounds = value; }
		}

		/// <summary>
		/// Gets or sets the bounds in which the circle bounds is previously processed.
		/// </summary>
		internal Rect ProcessedBounds
		{
			get { return _processedBounds; }
			set { _processedBounds = value; }
		}

		SfPullToRefresh? PullToRefresh
		{
			get => _pullToRefresh is not null && _pullToRefresh.TryGetTarget(out var v) ? v : null;
			set => _pullToRefresh = value is null ? null : new(value);
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// This method initializes the pulling template view.
		/// </summary>
		internal void InitializePullingViewTemplate()
		{
			if (PullToRefresh is not null && PullToRefresh.PullingViewTemplate is not null)
			{
				PullToRefresh.PullingTemplateView = SfProgressCircleView.CreateTemplateContent(PullToRefresh.PullingViewTemplate);
			}
		}

		/// <summary>
		/// This method initializes the refreshing template view.
		/// </summary>
		internal void InitializeRefreshViewViewTemplate()
		{
			if (PullToRefresh is not null && PullToRefresh.RefreshingViewTemplate is not null)
			{
				PullToRefresh.RefreshingTemplateView = SfProgressCircleView.CreateTemplateContent(PullToRefresh.RefreshingViewTemplate);
			}
		}

		/// <summary>
		/// Method checks for template and set its as content if template is assigned.
		/// </summary>
		internal void CheckIfAndSetTemplate()
		{
			if (PullToRefresh is not null)
			{
				var isPulling = PullToRefresh.IsPulling;
				var isRefreshing = PullToRefresh.ActualIsRefreshing;

				if (isPulling && PullToRefresh.PullingTemplateView is not null)
				{
					UpdateContent(PullToRefresh.PullingTemplateView);
				}
				else if (isRefreshing && PullToRefresh.RefreshingTemplateView is not null)
				{
					UpdateContent(PullToRefresh.RefreshingTemplateView);
				}
			}
           
		}

		/// <summary>
		/// Method updates the content of <see cref="SfProgressCircleView"/>.
		/// </summary>
		/// <param name="content">Indicates the content to be added, if null <see cref="SfProgressCircleView"/> Content will be set null.</param>
		internal void UpdateContent(View? content = null)
		{
			if (content is not null)
			{
				Content = content;
			}

			if (PullToRefresh is not null)
			{ 
				PullToRefresh.MeasureSfProgressCircleView(PullToRefresh.Bounds.Width, PullToRefresh.Bounds.Height);
				UpdateCircleViewBounds();
				if (PullToRefresh.IsPulling || PullToRefresh.ActualIsRefreshing)
				{
					PullToRefresh.ManualArrangeContent(PullToRefresh.TransitionMode == PullToRefreshTransitionType.SlideOnTop, PullToRefresh.GetBounds());
				}
			}
		}

		/// <summary>
		/// Method resets the arc angle.
		/// </summary>
		internal void ResetArcAngle()
		{
			_startAngle = 90f;
			_endAngle = _startAngle - MinArcLength;
			_isArcCollapsing = false;
			_isInShift = false;
			_angleMaintenanceCounter = 0;
		}

		/// <summary>
		/// This method computes the <see cref=" _startAngle"/> and <see cref="_endAngle"/> based on counter value, circle quadrant based on clockwise direction.
		/// </summary>
		internal void ComputeArcAngle()
		{
            if (_isInShift)
            {
                _angleMaintenanceCounter--;
            }

            if (_isArcCollapsing)
            {
                _endAngle = SfProgressCircleView.ConvertAngleBasedOnQuadrant(_endAngle - ArcMovementAngle);
                const float counterValue = 86.4f;
                float incrementAngle = GetArcLength() <= counterValue ? ArcMovementAngle + (ArcIncrement / 4f) : ArcIncrement;
                 _startAngle = SfProgressCircleView.ConvertAngleBasedOnQuadrant( _startAngle - incrementAngle);
            }
            else
            {
                 _startAngle = SfProgressCircleView.ConvertAngleBasedOnQuadrant( _startAngle - ArcMovementAngle);
                _endAngle = SfProgressCircleView.ConvertAngleBasedOnQuadrant(_endAngle - (_isInShift ? ArcMovementAngle : ArcIncrement));
            }

            if (GetArcLength() >= MaxArcLength)
            {
                if (!_isInShift)
                {
                    _isInShift = true;
                    _angleMaintenanceCounter = 30;
                }
            }
            else if (GetArcLength() <= MinArcLength)
            {
                _isArcCollapsing = false;
            }

            if (_angleMaintenanceCounter == 0 && _isInShift)
            {
                _isInShift = false;
                _isArcCollapsing = true;
            }
        }

        /// <summary>
        /// Method updates the circle view computed position except Y value.
        /// </summary>
        /// <remarks>Do not consider Y updated in this method.</remarks>
        internal void UpdateCircleViewBounds()
        {
            // if either a pulling template or refreshing template need to use their desired size values.
            if (Content is not null)
            {
                _circleViewBounds.Width = Content.DesiredSize.Width;
				_circleViewBounds.Height = Content.DesiredSize.Height;
            }
			else if (PullToRefresh is not null)
			{
				_circleViewBounds.Width = PullToRefresh.CircleViewWidth;
				_circleViewBounds.Height = PullToRefresh.CircleViewHeight;
			}

			if (PullToRefresh is not null)
			{
				if (PullToRefresh.Bounds.Width > 0)
				{
					_circleViewBounds.X = (PullToRefresh.Width / 2) - (CircleViewBounds.Width / 2);
				}
				else
				{
					_circleViewBounds.X = 0;
				}

				ProcessedBounds = PullToRefresh.Bounds;
			}
		}

		/// <summary>
		/// Updates fields used in OnDraw for drawing arc and fill ellipse.
		/// </summary>
		internal void UpdateDrawProperties()
		{
			if (PullToRefresh is not null)
			{
				// Calculate positions and sizes for drawing the background circle.
				float x = (float)((PullToRefresh.CircleViewWidth / 2) - (PullToRefresh.RefreshViewWidth / 2));
				float y = (float)PullToRefresh.GetShadowSpace(true);
				_fillRect = new Rect(x, y, (float)PullToRefresh.RefreshViewWidth, (float)PullToRefresh.RefreshViewHeight);

				// Based on UI spec  52.0833 is calculated, In UI spec circle width is 48 and arc width is 28 and the stroke thickness is 3.
				var circleWidth = 52.0833;
				// On drawing an arc with thickness 3 at 10 - the arc center will be 10 i.e arc thickness - left will be at 8.5 and thickness - right will be 11.5.
				// so arc width in spec is 28 and should be considered as 28 - 3 = 25 which is 52.0833 in 48(Circle width).
				var arcWidth = (PullToRefresh.RefreshViewWidth * circleWidth) / 100;
				var arcHeight = (PullToRefresh.RefreshViewHeight * circleWidth) / 100;
				x = (float)((PullToRefresh.CircleViewWidth / 2) - (arcWidth / 2));
				y = (float)((PullToRefresh.RefreshViewHeight / 2) - (arcHeight / 2));
				_oval = new Rect(x, y + PullToRefresh.GetShadowSpace(true), arcWidth, arcHeight);
			}
		}

		#endregion

        #region Private Methods

        bool ShouldSkipDrawing(RectF dirtyRect)
        {
			return (PullToRefresh is not null && PullToRefresh.IsPulling && PullToRefresh.PullingTemplateView is not null) ||
                   (PullToRefresh is not null && PullToRefresh.ActualIsRefreshing && PullToRefresh.RefreshingTemplateView is not null) ||
                   dirtyRect.Width <= 0 || dirtyRect.Height <= 0;
        }

        void DrawBackgroundCircle(ICanvas canvas)
        {
			if (PullToRefresh is null)
			{
				return;
			}

			Color shadowColor = Color.FromArgb("#59000000");
			Paint solidPaint = PullToRefresh.ProgressBackground;
			canvas.SetFillPaint(solidPaint, _fillRect);
#if ANDROID
			const int androidShadowSize = 4;
			if (PullToRefresh.ProgressBackground is SolidColorBrush brush && brush.Color.Alpha == 0)
			{
				shadowColor = Color.FromArgb("#00000000");
			}

			canvas.SetShadow(new SizeF(0, 1), androidShadowSize, shadowColor);
#else
			const int defaultShadowSize = 3;
			canvas.SetShadow(new SizeF(0, 1), defaultShadowSize, shadowColor);
#endif
			canvas.FillEllipse((float)_fillRect.X, (float)_fillRect.Y, (float)_fillRect.Width, (float)_fillRect.Height);
        }

        void DrawProgressArc(ICanvas canvas)
        {
			if (PullToRefresh is null)
			{
				return;
			}

			canvas.StrokeColor = PullToRefresh.ProgressColor;
			canvas.StrokeSize = (float)PullToRefresh.ProgressThickness;
			const float quarterCircle = 90f;
			if (PullToRefresh.IsPulling)
			{
				if (PullToRefresh.ProgressRate == 100)
				{
					canvas.DrawEllipse(_oval);
				}
				else
				{
					canvas.DrawArc(_oval, quarterCircle, SfProgressCircleView.ThresholdToAngle(PullToRefresh.ProgressRate), true, false);
				}
			}
			else if (PullToRefresh.ActualIsRefreshing)
			{
				canvas.DrawArc(_oval, _startAngle, _endAngle, true, false);
			}
		}

		/// <summary>
		/// This method used to calculate the progress value based on the progress rate.
		/// </summary>
		/// <param name="progressRate">Indicates the progress rate.</param>
		/// <returns>Progress value.</returns>
		static float ThresholdToAngle(double progressRate)
        {
            const float fullCircle = 360f;
            const float quarterCircle = 90f;
            float arcAngle = (float)(progressRate / 100) * fullCircle;
            if (arcAngle <= quarterCircle)
            {
                arcAngle = quarterCircle - arcAngle;
            }
            else
            {
                arcAngle = fullCircle - (arcAngle - quarterCircle);
            }

            return arcAngle;
        }

		/// <summary>
		/// Method converts angle relative to quadrant of circle.
		/// </summary>
		/// <param name="value">Calculated angle.</param>
		/// <returns>Returns the angle relative to quadrant.</returns>
		static float ConvertAngleBasedOnQuadrant(float value)
        {
            const float fullCircle = 360f;
            if (value < 0)
            {
                return fullCircle + value;
            }

            return value;
        }

        /// <summary>
        /// Method returns the length of arc between start and end angle.
        /// </summary>
        /// <returns>The length of arc between start and end angle.</returns>
        float GetArcLength()
        {
            return SfProgressCircleView.ConvertAngleBasedOnQuadrant( _startAngle - _endAngle);
        }

		/// <summary>
		/// Method cast the template content as view which loaded inside the <see cref="DataTemplate"/>.
		/// </summary>
		/// <param name="dataTemplate">DateTemplate instance.</param>
		/// <returns>Returns the <see cref="DataTemplate"/> content as view.</returns>
		static View CreateTemplateContent(DataTemplate dataTemplate)
        {
            var templateView = dataTemplate.CreateContent();
#if NET10_0
            return (View)templateView;
#else
			if (templateView is ViewCell viewCell)
			{
				return viewCell.View;
			}
			else
            {
                return (View)templateView;
            }
#endif
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Raises when <see cref="SfProgressCircleView"/>'s handler gets changed.
		/// <exclude/>
		/// </summary>
		protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
#if WINDOWS
			// In WinUI, we will do clipping while pulling, which triggers the WrapperView procedure at runtime.
			// which causes to removing and adding circle view to wrapper view. So, canvasControl will get loaded and unloaded at runtime
			// causes a blank issue for first time pulling in WInUI.
			if (Handler == null)
			{
			   return;
			}
			else
			{
				Handler.HasContainer = true;
			}
#endif
		}

		/// <summary>
		/// This method used the shape the view to circle and draw arc based on <see cref="SfPullToRefresh"/>.
		/// <exclude/>
		/// </summary>
		/// <param name="canvas">The canvas on which the background will be drawn.</param>
		/// <param name="dirtyRect">Bounds of the <see cref="SfProgressCircleView"/>.</param>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            if (ShouldSkipDrawing(dirtyRect))
            {
                return;
            }

            canvas.CanvasSaveState();
            DrawBackgroundCircle(canvas);
            canvas.CanvasRestoreState();
            DrawProgressArc(canvas);
        }

        #endregion
    }
}
