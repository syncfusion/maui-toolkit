using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Shimmer
{
	/// <summary>
	/// Represents a drawable component for rendering shimmer effects.
	/// </summary>
	internal partial class ShimmerDrawable : SfDrawableView
	{

		#region Fields

		/// <summary>
		/// Hold the shimmer control customization details.
		/// </summary>
		WeakReference<IShimmer>? _shimmer;

		/// <summary>
		/// The available size allotted in OnDraw method.
		/// </summary>
		Size _availableSize;

		/// <summary>
		/// The gradient brush for shimmer wave effect.
		/// </summary>
		readonly LinearGradientBrush _gradient;

		/// <summary>
		/// The path to draw built in views like persona, video, shopping etc.
		/// </summary>
		PathF? _path;

		/// <summary>
		/// The padding value for left and right side of the shimmer view.
		/// </summary>
		const float SidePadding = 10;

		/// <summary>
		/// When repeat count was set, this spacing will be used in between the repeated shimmer view.
		/// </summary>
		const float RepeatViewSpacing = 10;

		/// <summary>
		/// Corner radius for the shimmer.
		/// </summary>
		const int CornerRadius = 2;

		/// <summary>
		/// Column spacing for the shimmer.
		/// </summary>
		const float ColumnSpacing = 10;

		/// <summary>
		/// Persona size for the shimmer.
		/// </summary>
		const float PersonaSize = 0.2f;

		// Constants for design structure for profile path.
		const float ProfileCircleSizeRatio = 0.3f;
		const float ProfileTopRectangleHeightRatio = 0.1f;
		const float ProfileBottomRectangleHeightRatio = 0.08f;
		const float ProfileRectangleSecondarySpacingRatio = 0.05f;
		const float ProfileRectanglePrimarySpacingRatio = 0.12f;

		// Constants for design structure for video path.
		const float VideoCircleSizeRatio = 0.2f;
		const float VideoCircleHeightRatio = 0.3f;
		const float VideoRowSpacingRatio = 0.1f;

		// Constants for design structure for feed path.
		const float FeedCircleSizeRatio = 0.25f;
		const float FeedCircleHeightRatio = 0.2f;
		const float FeedRowSpacingRatio = 0.04f;
		const float FeedCircleRowSpacingRatio = 0.1f;
		const float FeedCircleRectangleHeightRatio = 0.3f;
		const float FeedRectangleHeightRatio = 0.06f;

		// Constants for design structure for shopping path.
		const float ShoppingVideoRectSizeRatio = 0.65f;
		const float ShoppingPaddingRatio = 0.05f;

		// Constants for design structure article path.
		const float ArticleRectangleHeightFactor = 0.15f;
		const float ArticleRowSpacingFactor = 0.05f;
		const float ArticleBoxSize = 0.25f;

		// Constants for design structure square and circle persona.
		const float PersonaRectangleHeightFactor = 0.33f;
		const float PersonaRowSpacingFactor = 0.1f;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ShimmerDrawable"/> class.
		/// </summary>
		/// <param name="shimmer">The instance for the <see cref="SfShimmer"/> class.</param>
		internal ShimmerDrawable(IShimmer shimmer)
		{
			Shimmer = shimmer;
			_gradient = new LinearGradientBrush();
		}

		#endregion

		#region Properties

		internal IShimmer? Shimmer
		{
			get => _shimmer != null && _shimmer.TryGetTarget(out var v) ? v : null;
			set => _shimmer = value == null ? null : new(value);
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Creates the wave animator for the shimmer effect.
		/// </summary>
		internal void CreateWaveAnimator()
		{
			if (AnimationExtensions.AnimationIsRunning(this, "ShimmerAnimation"))
			{
				this.AbortAnimation("ShimmerAnimation");
			}

			if (Shimmer?.AnimationDuration <= 0)
			{
				InvalidateDrawable();
				return;
			}

			CreateAnimator();
		}

		/// <summary>
		/// Updates the custom view when changed dynamically. 
		/// Setting path to null whenever the custom view was changed in order to update the control.
		/// </summary>
		internal void UpdateShimmerDrawable()
		{
			_path = null;

			// Need to invalidate the shimmer drawable when the custom view or type or repeat count was changed dynamically when duration is 0,
			// Since the animation duration is 0, the OnDraw method will not be called in shimmer drawable to invalidate the view.
			// OnDraw will be invalidated if the animation duration is >0 on OnAnimationUpdate method. 
			// So we need to invalidate the view manually.
			if (Shimmer?.AnimationDuration <= 0)
			{
				InvalidateDrawable();
			}
		}

		/// <summary>
		/// Creates the gradient brush for shimmer wave animation.
		/// </summary>
		internal void CreateWavePaint()
		{
			switch (Shimmer?.WaveDirection)
			{
				case ShimmerWaveDirection.LeftToRight:
					_gradient.StartPoint = new Point(0, 0);
					_gradient.EndPoint = new Point(1, 0);
					break;
				case ShimmerWaveDirection.TopToBottom:
					_gradient.StartPoint = new Point(0, 0);
					_gradient.EndPoint = new Point(0, 1);
					break;
				case ShimmerWaveDirection.RightToLeft:
					_gradient.StartPoint = new Point(1, 0);
					_gradient.EndPoint = new Point(0, 0);
					break;
				case ShimmerWaveDirection.BottomToTop:
					_gradient.StartPoint = new Point(0, 1);
					_gradient.EndPoint = new Point(0, 0);
					break;
				default:
					_gradient.StartPoint = new Point(0, 0);
					_gradient.EndPoint = new Point(1, 1);
					break;
			}
		}

		/// <summary>
		/// Disposes Element of Drawable View
		/// </summary>
		internal void Dispose()
		{
			if (_path != null)
			{
				_path.Dispose();
				_path = null;
			}

			if (Shimmer != null)
			{
				Shimmer = null;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Updates the shimmer view and shimmer wave elements.
		/// </summary>
		void UpdateShimmerView()
		{
			CreateShimmerViewPath();
			CreateWavePaint();
			CreateWaveAnimator();
		}

		/// <summary>
		/// Creates the path for the shimmer view.
		/// </summary>
		void CreateShimmerViewPath()
		{
			if (_availableSize.Width <= 0 || _availableSize.Height <= 0)
			{
				return;
			}

			_path = new PathF();
			if (Shimmer?.CustomView != null)
			{
				CreateCustomViewPath();
			}
			else
			{
				CreateDefaultPath();
			}
		}

		/// <summary>
		/// Creates the path and draw the custom view children.
		/// </summary>
		void CreateCustomViewPath()
		{
			if (Shimmer != null)
			{
				DrawCustomViewChildren(Shimmer.CustomView, new Point(Shimmer.CustomView.X, Shimmer.CustomView.Y));
			}
		}

		/// <summary>
		/// Draws the children of the custom view.
		/// </summary>
		/// <param name="view">The custom view.</param>
		/// <param name="position">The position to draw the children.</param>
		void DrawCustomViewChildren(View view, Point position)
		{
			//  <StackLayout>
			//      <BoxView HeightRequest="20" WidthRequest="100"/>
			//      <Grid>
			//          <BoxView HeightRequest="40" WidthRequest="50"/>
			//      </Grid>
			//      <Grid HeightRequest="20" WidthRequest="100"/>
			//  </StackLayout>
			//
			// This method enters with stack layout. Since stack layout is a type of Layout so it may or may not contain child view.
			// The above example stack layout has 3 children.
			// First child is BoxView, second child is Grid with box view as a children and third child is Grid with no children.
			// If the child is a type of view then it will be added to the path (Box view will be drawn) in else statement.
			// The second child (ie. grid) is a type of Layout so it will enter into the if statement and it has child and hence the child box view will be drawn by calling this recursive method.
			// The third child (ie. grid) is a type of Layout so it will enter into the if statement and it has no child and hence it will not be drawn.

			if (view == null || _path == null)
			{
				return;
			}

			if (view is Layout layout)
			{
				foreach (View item in layout.Children.Cast<View>())
				{
					DrawCustomViewChildren(item, new Point(item.X + position.X, item.Y + position.Y));
				}
			}
			else if (view is ContentView contentView && contentView.Content != null)
			{
				View item = contentView.Content;
				DrawCustomViewChildren(item, new Point(item.X + position.X, item.Y + position.Y));
			}
			else
			{
				CornerRadius radius = 0;
				Point viewPosition = new Point(position.X, position.Y);
				float width = (float)view.Bounds.Width;
				float height = (float)view.Bounds.Height;

				if (view is ShimmerView shimmerView)
				{
					if (shimmerView.ShapeType == ShimmerShapeType.Circle)
					{
						_path.AppendCircle((float)viewPosition.X + width / 2, (float)viewPosition.Y + height / 2, Math.Min(width / 2, height / 2));
						return;
					}
					else if (shimmerView.ShapeType == ShimmerShapeType.RoundedRectangle)
					{
						radius = 5;
					}
				}

				if (view is BoxView boxView)
				{
					radius = boxView.CornerRadius;
				}

				_path.AppendRoundedRectangle((float)viewPosition.X, (float)viewPosition.Y, width, height, (float)radius.TopLeft, (float)radius.TopRight, (float)radius.BottomLeft, (float)radius.BottomRight);
			}
		}

		/// <summary>
		/// Creates the path data for the build-in shimmer types.
		/// </summary>
		void CreateDefaultPath()
		{
			if (Shimmer == null)
			{
				return;
			}

			int repeatCount = Math.Max(Shimmer.RepeatCount, 1);

			// This will be the maximum height of the single shimmer view.
			float totalWidth = (float)_availableSize.Width - 2 * SidePadding;
			float shimmerHeight = (float)(_availableSize.Height - ((repeatCount + 1) * RepeatViewSpacing)) / repeatCount;

			// Define the rectangle for the shimmer effect.
			RectF shimmerRect = new RectF(SidePadding, RepeatViewSpacing, totalWidth, shimmerHeight);

			switch (Shimmer.Type)
			{
				case ShimmerType.CirclePersona:
					CreateCirclePersonaPath(shimmerRect);
					break;
				case ShimmerType.SquarePersona:
					CreateSquarePersonaPath(shimmerRect);
					break;
				case ShimmerType.Profile:
					CreateProfilePath(shimmerRect);
					break;
				case ShimmerType.Article:
					CreateArticlePath(shimmerRect);
					break;
				case ShimmerType.Video:
					CreateVideoPath(shimmerRect);
					break;
				case ShimmerType.Feed:
					CreateFeedPath(shimmerRect);
					break;
				case ShimmerType.Shopping:
					CreateShoppingPath(shimmerRect);
					break;
			}
		}

		/// <summary>
		/// Creates the profile path for the shimmer view.
		/// </summary>
		/// <param name="shimmerRect">The rectangle area for the shimmer effect.</param>
		void CreateProfilePath(RectF shimmerRect)
		{
			if (_path == null || Shimmer == null)
			{
				return;
			}

			// Design structure as follows:
			// primary circle have 30 % from the height.
			// The top rectangles have 0.1 % from the height.
			// The bottom rectangles have 0.08 % from the height.
			// The spacing between the two consecutive top/bottom rectangles is 0.05 % from the height.
			// The spacing between the top and bottom rectangle and the spacing between
			// circle and top rectangle is 0.12 % from the height.

			int repeatCount = Math.Max(Shimmer.RepeatCount, 1);

			// Radius for the profile circle.
			float radius = shimmerRect.Height * ProfileCircleSizeRatio / 2f;
			float diameter = radius * 2;

			// The initial or first y position for the profile view.
			float y = (float)(_availableSize.Height / 2) - ((repeatCount * shimmerRect.Height) + (repeatCount - 1) * shimmerRect.Y) / 2;
			y = Math.Max(y, shimmerRect.Y);

			// The height of the profile top rectangle.
			float topRectangleHeight = shimmerRect.Height * ProfileTopRectangleHeightRatio;

			// The height of the profile bottom rectangle.
			float bottomRectangleHeight = shimmerRect.Height * ProfileBottomRectangleHeightRatio;

			// The spacing between the two consecutive top/bottom rectangles.
			float rectangleSecondarySpacing = shimmerRect.Height * ProfileRectangleSecondarySpacingRatio;

			// The spacing between the top and bottom rectangle and the spacing between circle and top rectangle.
			float rectanglePrimarySpacing = shimmerRect.Height * ProfileRectanglePrimarySpacingRatio;

			for (int i = 0; i < repeatCount; i++)
			{
				float yPosition = y + radius;
				_path.AppendCircle(shimmerRect.X + shimmerRect.Width / 2, yPosition, radius);

				yPosition = y + diameter + rectanglePrimarySpacing;

				// First rectangle has 40% of width
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X + shimmerRect.Width * 0.3f, yPosition, shimmerRect.Width * 0.4f, topRectangleHeight), CornerRadius);

				yPosition += topRectangleHeight + rectangleSecondarySpacing;

				// Second rectangle has 70% of width
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X + shimmerRect.Width * 0.15f, yPosition, shimmerRect.Width * 0.7f, topRectangleHeight), CornerRadius);

				yPosition += topRectangleHeight + rectanglePrimarySpacing;
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, yPosition, shimmerRect.Width, bottomRectangleHeight), CornerRadius);

				yPosition += bottomRectangleHeight + rectangleSecondarySpacing;
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, yPosition, shimmerRect.Width, bottomRectangleHeight), CornerRadius);

				y += shimmerRect.Y + shimmerRect.Height;
			}
		}

		/// <summary>
		/// Creates the video path for the shimmer view.
		/// </summary>
		/// <param name="shimmerRect">The rectangle area for the shimmer effect.</param>
		void CreateVideoPath(RectF shimmerRect)
		{
			if (_path == null || Shimmer == null)
			{
				return;
			}

			// Design structure as follows:
			// Circle have min value of 30 % from the height and 20 % from the width.
			// Padding between the circle and the rectangle is 10 % from the height.
			// primary rectangle height is total height - circle height - padding.
			// The bottom rectangles have 30 % from the circle height.
			// The spacing between the two consecutive bottom rectangles is 20 % from the circle height.
			// The top and bottom padding for the top and bottom rectangle is 10 % from the circle height.
			// circle and top rectangle padding is 10 % from the height.

			int repeatCount = Math.Max(Shimmer.RepeatCount, 1);

			// Radius for the video circle.
			float radius = shimmerRect.Width * VideoCircleSizeRatio / 2f;
			radius = Math.Min(shimmerRect.Height * VideoCircleHeightRatio / 2, radius);
			float diameter = radius * 2;

			// The initial or first y position for the video view.
			float y = (float)(_availableSize.Height / 2) - ((repeatCount * shimmerRect.Height) + (repeatCount - 1) * shimmerRect.Y) / 2;
			y = Math.Max(y, shimmerRect.Y);

			// The top and bottom row spacing for the circle rectangle.
			float circleRowSpacing = diameter * VideoRowSpacingRatio;

			// The height of the circle rectangle height.
			float circleRectangleHeight = diameter * VideoCircleHeightRatio;

			// Row space between the circle and the rectangle.
			float rowSpacing = shimmerRect.Height * VideoRowSpacingRatio;

			// The height of the video rectangle.
			float videoRectangleHeight = shimmerRect.Height - diameter - rowSpacing;

			// The padding for the right end of the two consecutive rectangle.
			float rectangleRightEndPadding = shimmerRect.Width * VideoRowSpacingRatio;

			// The x-position for the persona rectangle.
			float xValue = shimmerRect.X + ColumnSpacing + diameter;

			for (int i = 0; i < repeatCount; i++)
			{
				float yPosition = y;

				// Append the main video rectangle
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, yPosition, shimmerRect.Width, videoRectangleHeight), CornerRadius);

				// Append the circle
				yPosition = y + videoRectangleHeight + rowSpacing + radius;
				_path.AppendCircle(shimmerRect.X + radius, yPosition, radius);

				// Append the first bottom rectangle
				yPosition = y + videoRectangleHeight + rowSpacing + circleRowSpacing;
				_path.AppendRoundedRectangle(new RectF(xValue, yPosition, shimmerRect.Width - ColumnSpacing - diameter - rectangleRightEndPadding, circleRectangleHeight), CornerRadius);

				// Append the second bottom rectangle
				yPosition += circleRectangleHeight + (circleRowSpacing * 2);
				_path.AppendRoundedRectangle(new RectF(xValue, yPosition, shimmerRect.Width - ColumnSpacing - diameter - rectangleRightEndPadding, circleRectangleHeight), CornerRadius);

				// Update y for the next repeat
				y += shimmerRect.Y + shimmerRect.Height;
			}
		}

		/// <summary>
		/// Creates the feed path for the shimmer view.
		/// </summary>
		/// <param name="shimmerRect">The rectangle area for the shimmer effect.</param>
		void CreateFeedPath(RectF shimmerRect)
		{
			if (_path == null || Shimmer == null)
			{
				return;
			}

			// Design structure as follows:
			// Circle have min value of 30 % from the height and 20 % from the width.
			// The bottom rectangles have 30 % from the circle height.
			// The spacing between the two consecutive bottom rectangles is 20 % from the circle height.
			// The top and bottom padding for the top and bottom rectangle is 20 % from the circle height.
			// Padding between the circle and the primary rectangle is 8 % from the height.
			// primary rectangle height is total height - circle height - padding 
			// - bottom 2 rectangle - padding between primary rectangle and bottom rectangle
			// - padding between the bottom 2 rectangle.
			// Padding between the primary rectangle and the secondary rectangle is secondary rectangle height.
			// Secondary rectangle height is 6 % from the height.
			// Padding between the consecutive secondary rectangle is 4 % from the height.

			int repeatCount = Math.Max(Shimmer.RepeatCount, 1);

			// Radius for the feed circle.
			float radius = shimmerRect.Height * FeedCircleSizeRatio / 2f;
			radius = Math.Min(shimmerRect.Width * FeedCircleHeightRatio / 2, radius);
			float diameter = radius * 2;

			// The initial or first y position for the feed view.
			float y = (float)(_availableSize.Height / 2) - ((repeatCount * shimmerRect.Height) + (repeatCount - 1) * shimmerRect.Y) / 2;
			y = Math.Max(y, shimmerRect.Y);

			// The row spacing for the circle rectangle.
			float circleRowSpacing = diameter * FeedCircleRowSpacingRatio;

			// The circle rectangle height.
			float circularItemHeight = diameter * FeedCircleRectangleHeightRatio;

			// The common row spacing for the feed.
			float rowSpacing = shimmerRect.Height * FeedRowSpacingRatio;

			// The height of the subject rectangle
			float rectangleHeight = shimmerRect.Height * FeedRectangleHeightRatio;

			// The hight of the feed box.
			// The height of the feed rectangle is follows
			// The height of the circle - space between the circle and feed rectangle(2 * row spacing)
			// - space between the feed rectangle and the bottom rectangle(rectangleHeight) - two bottom rectangles(2* rectangleHeight)
			// - space between the two bottom rectangles(row spacing).
			float feedRectangleHeight = shimmerRect.Height - diameter - (3 * rowSpacing) - (rectangleHeight * 3);
			float circleRectangleWidth = shimmerRect.Width - ColumnSpacing - diameter;
			float xValue = shimmerRect.X + diameter + ColumnSpacing;

			for (int i = 0; i < repeatCount; i++)
			{
				// Append the circle
				_path.AppendCircle(shimmerRect.X + radius, y + radius, radius);

				// Append the first rectangle
				_path.AppendRoundedRectangle(new RectF(xValue, y + circleRowSpacing, circleRectangleWidth, circularItemHeight), CornerRadius);

				// Append the second rectangle
				_path.AppendRoundedRectangle(new RectF(xValue, y + (circleRowSpacing * 3) + circularItemHeight, circleRectangleWidth * 0.5f, circularItemHeight), CornerRadius);

				// Append the main feed rectangle
				float yPosition = y + diameter + (2 * rowSpacing);
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, yPosition, shimmerRect.Width, feedRectangleHeight), CornerRadius);

				// Append the first bottom rectangle
				yPosition += feedRectangleHeight + rectangleHeight;
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, yPosition, shimmerRect.Width, rectangleHeight), CornerRadius);

				// Append the second bottom rectangle
				yPosition += rectangleHeight + rowSpacing;
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, yPosition, shimmerRect.Width, rectangleHeight), CornerRadius);

				// Update y for the next repeat
				y += shimmerRect.Height + shimmerRect.Y;
			}
		}

		/// <summary>
		/// Creates the shopping path for the shimmer view.
		/// </summary>
		/// <param name="shimmerRect">The rectangle area for the shimmer effect.</param>
		void CreateShoppingPath(RectF shimmerRect)
		{
			if (_path == null || Shimmer == null)
			{
				return;
			}

			// Design structure as follows:
			// primary rectangle height is 65 % of the height.
			// The bottom rectangles have 10 % from the height.
			// The spacing between the primary and bottom rectangles is 10 % from the height.
			// The padding for the 2 consecutive rectangles is 5 % from the height.

			int repeatCount = Math.Max(Shimmer.RepeatCount, 1);

			// The initial or first y position for the shopping view.
			float y = (float)(_availableSize.Height / 2) - ((repeatCount * shimmerRect.Height) + (repeatCount - 1) * shimmerRect.Y) / 2;
			y = Math.Max(y, shimmerRect.Y);

			// The calculated video rectangle size.
			float videoRectSize = shimmerRect.Height * ShoppingVideoRectSizeRatio;

			// The factor value for the rectangle path height.
			float rectangleHeightFactor = ShoppingPaddingRatio;

			// The height of the rectangle.
			float rectangleHeight = shimmerRect.Height * rectangleHeightFactor * 2;

			// The row space between the rectangle.
			float rowSpacing = shimmerRect.Height * rectangleHeightFactor;

			for (int i = 0; i < repeatCount; i++)
			{
				float yPosition = y;

				// Append the primary rectangle
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, yPosition, shimmerRect.Width, videoRectSize), CornerRadius);

				// Append the first bottom rectangle
				yPosition += videoRectSize + rectangleHeight;
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, yPosition, shimmerRect.Width, rectangleHeight), CornerRadius);

				// Append the second bottom rectangle
				yPosition += rectangleHeight + rowSpacing;
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, yPosition, shimmerRect.Width, rectangleHeight), CornerRadius);

				y += shimmerRect.Y + shimmerRect.Height;
			}
		}

		/// <summary>
		/// Creates the article path for the shimmer view.
		/// </summary>
		/// <param name="shimmerRect">The rectangle area for the shimmer effect.</param>
		void CreateArticlePath(RectF shimmerRect)
		{
			if (_path == null || Shimmer == null)
			{
				return;
			}

			// Design structure as follows:
			// primary rectangle have 25 % from the width and if the size greater than height
			// then the rectangle size assigned to height value.
			// The secondary rectangle have 75 % from the width with inbetween padding as 10.
			// The secondary rectangle total height as primary rectangle size and each rectangle
			// height value as 0.15 % of primary rectangle height with top and bottom padding
			// as 5 % of primary rectangle height.

			int repeatCount = Math.Max(Shimmer.RepeatCount, 1);

			// The article box size.

			// Calculating the article box width.
			float articleBoxWidth = shimmerRect.Width * ArticleBoxSize;
			articleBoxWidth = Math.Min(shimmerRect.Height, articleBoxWidth);

			// The initial or first y position for the article view.
			// Split the additional space between the each view based on the circle size.
			float y = (float)(_availableSize.Height / 2) - ((repeatCount * articleBoxWidth) + (repeatCount - 1) * shimmerRect.Y) / 2;
			y = Math.Max(y, shimmerRect.Y);

			// The row space between the paths.
			float rowSpacing = articleBoxWidth * ArticleRowSpacingFactor;

			// The height of the secondary rectangle.
			float rectangleHeight = articleBoxWidth * ArticleRectangleHeightFactor;

			// The x-position for the secondary rectangle.
			float xValue = shimmerRect.X + articleBoxWidth + ColumnSpacing;

			for (int i = 0; i < repeatCount; i++)
			{
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, y, articleBoxWidth, articleBoxWidth), CornerRadius);
				_path.AppendRoundedRectangle(new RectF(xValue, y + rowSpacing, shimmerRect.Width - ColumnSpacing - articleBoxWidth, rectangleHeight), CornerRadius);
				_path.AppendRoundedRectangle(new RectF(xValue, y + rowSpacing * 2 + rectangleHeight, shimmerRect.Width * 0.4f, rectangleHeight), CornerRadius);

				_path.AppendRoundedRectangle(new RectF(xValue, y + articleBoxWidth - rectangleHeight * 2 - rowSpacing * 2, shimmerRect.Width - ColumnSpacing - articleBoxWidth, rectangleHeight), CornerRadius);
				_path.AppendRoundedRectangle(new RectF(xValue, y + articleBoxWidth - rectangleHeight - rowSpacing, shimmerRect.Width * 0.4f, rectangleHeight), CornerRadius);
				y += shimmerRect.Y + articleBoxWidth;
			}
		}

		/// <summary>
		/// Creates the square persona path for the shimmer view.
		/// </summary>
		/// <param name="shimmerRect">The rectangle area for the shimmer effect.</param>
		void CreateSquarePersonaPath(RectF shimmerRect)
		{
			if (_path == null || Shimmer == null)
			{
				return;
			}

			// Design structure as follows:
			// primary rectangle have 20 % from the width and if the size greater than height
			// then the rectangle size assigned to height value.
			// The secondary rectangle have 80 % from the width with inbetween padding as 10.
			// The secondary rectangle total height as primary rectangle size and each rectangle
			// height value as 0.33 % of primary rectangle height with top and bottom padding
			// as 10% of primary rectangle height.

			int repeatCount = Math.Max(Shimmer.RepeatCount, 1);

			// Size of the persona primary rectangle.
			float primaryRectSize = shimmerRect.Width * PersonaSize;
			primaryRectSize = Math.Min(shimmerRect.Height, primaryRectSize);

			// The initial or first y position for the persona view.
			// Split the additional space between the each view based on the circle size.
			float y = (float)(_availableSize.Height / 2) - ((repeatCount * primaryRectSize) + (repeatCount - 1) * shimmerRect.Y) / 2;
			y = Math.Max(y, shimmerRect.Y);

			// The space between the top and bottom rectangle.
			float rowSpacing = primaryRectSize * PersonaRowSpacingFactor;

			// The height of the persona rectangle.
			float rectangleHeight = primaryRectSize * PersonaRectangleHeightFactor;

			// The x-position for the persona rectangle.
			float xValue = shimmerRect.X + primaryRectSize + ColumnSpacing;

			for (int i = 0; i < repeatCount; i++)
			{
				_path.AppendRoundedRectangle(new RectF(shimmerRect.X, y, primaryRectSize, primaryRectSize), CornerRadius);
				_path.AppendRoundedRectangle(new RectF(xValue, y + rowSpacing, shimmerRect.Width - ColumnSpacing - primaryRectSize, rectangleHeight), CornerRadius);
				_path.AppendRoundedRectangle(new RectF(xValue, y + primaryRectSize - rowSpacing - rectangleHeight, shimmerRect.Width * 0.4f, rectangleHeight), CornerRadius);
				y += shimmerRect.Y + primaryRectSize;
			}
		}

		/// <summary>
		/// Creates the circle persona path for the shimmer view.
		/// </summary>
		/// <param name="shimmerRect">The rectangle area for the shimmer effect.</param>
		void CreateCirclePersonaPath(RectF shimmerRect)
		{
			if (_path == null || Shimmer == null)
			{
				return;
			}

			// Design structure as follows:
			// primary rectangle have 20 % from the width and if the size greater than height
			// then the rectangle size assigned to height value.
			// The secondary rectangle have 80 % from the width with inbetween padding as 10.
			// The secondary rectangle total height as primary rectangle size and each rectangle
			// height value as 0.33 % of primary rectangle height with top and bottom padding
			// as 10% of primary rectangle height.

			int repeatCount = Math.Max(Shimmer.RepeatCount, 1);

			// Size of the persona primary rectangle.
			float primaryRectSize = shimmerRect.Width * PersonaSize;
			primaryRectSize = Math.Min(shimmerRect.Height, primaryRectSize);

			// Radius of the circle.
			float radius = primaryRectSize / 2;

			// The initial or first y position for the persona view.
			// Split the additional space between the each view based on the circle size.
			float y = (float)(_availableSize.Height / 2) - ((repeatCount * primaryRectSize) + (repeatCount - 1) * shimmerRect.Y) / 2;
			y = Math.Max(y, shimmerRect.Y);

			// The space between the top and bottom rectangle.
			float rowSpacing = primaryRectSize * PersonaRowSpacingFactor;

			// The height of the persona rectangle.
			float rectangleHeight = primaryRectSize * PersonaRectangleHeightFactor;

			// The x-position for the persona rectangle.
			float xValue = shimmerRect.X + primaryRectSize + ColumnSpacing;

			for (int i = 0; i < repeatCount; i++)
			{
				_path.AppendCircle(shimmerRect.X + radius, y + radius, radius);
				_path.AppendRoundedRectangle(new RectF(xValue, y + rowSpacing, shimmerRect.Width - ColumnSpacing - primaryRectSize, rectangleHeight), CornerRadius);
				_path.AppendRoundedRectangle(new RectF(xValue, y + primaryRectSize - rowSpacing - rectangleHeight, shimmerRect.Width * 0.4f, rectangleHeight), CornerRadius);
				y += shimmerRect.Y + primaryRectSize;
			}
		}

		/// <summary>
		/// Creates the animator for the shimmer view.
		/// </summary>
		void CreateAnimator()
		{
			// We are returning, because we don't need to create animator in not active state.
			if (Shimmer == null || !Shimmer.IsActive || _availableSize == Size.Zero)
			{
				return;
			}

			// Calculating the wave width in factor value.
			float waveWidthFactor = (float)(Shimmer.WaveWidth / _availableSize.Width);

			// Validating the factor, because the factor should lie between 0 and 1.
			waveWidthFactor = Math.Clamp(waveWidthFactor, 0, 1);

			Animation parentAnimation = new Animation(OnAnimationUpdate, 0,
							1 + waveWidthFactor, Easing.Linear, null);
			parentAnimation.Commit(this, "ShimmerAnimation", 16, (uint)Shimmer.AnimationDuration,
				Easing.Linear, null, () => true);
		}

		/// <summary>
		/// Handles the animation update for the shimmer effect.
		/// </summary>
		/// <param name="animationValue">The current value of the animation.</param>
		void OnAnimationUpdate(double animationValue)
		{
			IShimmer? shimmer = Shimmer;
			if (shimmer == null)
			{
				return;
			}

			float offset = (float)animationValue;

			// Calculating the wave width in factor value.
			float waveWidthFactor = (float)(shimmer.WaveWidth / _availableSize.Width);

			// Validating the factor, because the factor should lie between 0 and 1.
			waveWidthFactor = Math.Clamp(waveWidthFactor, 0, 1);
			float gradientOffset3 = Math.Clamp(offset, 0, 1);
			float gradientOffset2 = Math.Clamp(offset - (waveWidthFactor / 2), 0, 1);
			float gradientOffset1 = Math.Clamp(offset - waveWidthFactor, 0, 1);
			if (_gradient != null)
			{
				// If gradient brush was set to fill property. only the first gradient stop color will be used for shimmer fill.
				Color color = BrushToColorConverter(shimmer.Fill);
				_gradient.GradientStops =
				[
					new GradientStop(){Color = color,Offset = gradientOffset1},
					new GradientStop(){Color = shimmer.WaveColor,Offset = gradientOffset2},
					new GradientStop(){Color = color,Offset = gradientOffset3},
				];
			}

			// Invalidate the drawable to reflect the changes.
			InvalidateDrawable();
		}

		/// <summary>
		/// Converts a <see cref="Brush"/> to a <see cref="Color"/>.
		/// </summary>
		/// <param name="color">The <see cref="Brush"/> to convert.</param>
		/// <returns>
		/// Returns the <see cref="Color"/> representation of the <see cref="Brush"/>. 
		/// If the conversion fails, returns <see cref="Colors.Transparent"/>.
		/// </returns>
		static Color BrushToColorConverter(Brush color)
		{
			Paint paint = color;
			return paint.ToColor() ?? Colors.Transparent;
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// Draws the shimmer effect on the canvas.
		/// </summary>
		/// <exclude/>
		/// <param name="canvas">The canvas to draw on.</param>
		/// <param name="dirtyRect">The rectangle area that needs to be redrawn.</param>
		/// <exclude/>
		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			// It is not required to update the view when the size is not changed.
			// Used this logic to update the view only when the size gets changed or if the path is null(Path=null was set in dynamic property changes).
			bool sizeChanged = _availableSize.Height != dirtyRect.Height || _availableSize.Width != dirtyRect.Width;
			if (sizeChanged || _path == null)
			{
				_availableSize = dirtyRect.Size;
				UpdateShimmerView();
			}

			if (_path == null || Shimmer == null || !Shimmer.IsActive)
			{
				return;
			}

			// To save the current state of the canvas returned by framework.
			canvas.CanvasSaveState();

			// When animation duration is zero, animation won't be triggered hence the color won't be set for gradient brush.
			// So, we are setting the color directly to the canvas.
			// Gradient fill can also be set for shimmer fill when animation duration is 0.
			Paint fillPaint = Shimmer.AnimationDuration <= 0 ? Shimmer.Fill : _gradient;
			canvas.SetFillPaint(fillPaint, _path.Bounds);
			canvas.FillPath(_path);

			// We have used canvas for our drawing purpose and now returning it previous saved state as returned by framework.
			canvas.CanvasRestoreState();
		}

		#endregion
	}
}
