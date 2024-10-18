using Android.Content.Res;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;

namespace Syncfusion.Maui.Toolkit.Carousel
{
	/// <summary>
	/// Platform carousel item item class.
	/// </summary>
	/// <exclude/>
	public partial class PlatformCarouselItem : FrameLayout
    {
        #region Fields

        /// <summary>
        /// The index.
        /// </summary>
        private int _index;

        /// <summary>
        /// The is item selected property.
        /// </summary>
        private bool _isItemSelectedProperty;

        /// <summary>
        /// The content view.
        /// </summary>
        private View? _contentView;

        /// <summary>
        /// The name of the image.
        /// </summary>
        private string? _imageName;

        /// <summary>
        /// AutomationId field.
        /// </summary>
        private string _automationId = string.Empty;

        /// <summary>
        /// The parent item.
        /// </summary>
        private PlatformCarousel? _parentItem;

        /// <summary>
        /// The context.
        /// </summary>
        private Context? _contextCarouselItem;

        /// <summary>
        /// The child view.
        /// </summary>
        private View? _childView;

        /// <summary>
        /// The drawn view.
        /// </summary>
        private View? _drawnView;

        /// <summary>
        /// The temp adapter view.
        /// </summary>
        private View? _adapterView;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformCarouselItem"/> class.
        /// </summary>
        /// <param name="context">Context of carousel item.</param>
        public PlatformCarouselItem(Context context) : base(context)
        {
            _contextCarouselItem = context;
            Init();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the image.
        /// </summary>
        /// <value>The name of the image.</value>
        public string? ImageName
        {
            get
            {
                return _imageName;
            }

            set
            {
                _imageName = value;
            }
        }

        /// <summary>
        /// Gets or sets the content view.
        /// </summary>
        /// <value>The content view.</value>
        public View? ContentView
        {
            get
            {
                return _contentView!;
            }

            set
            {
                _contentView = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent item.
        /// </summary>
        /// <value>The parent item.</value>
        public PlatformCarousel? ParentItem
        {
            get
            {
                return _parentItem;
            }

            set
            {
                _parentItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        internal int Index
        {
            get
            {
                return _index;
            }

            set
            {
                _index = value;
            }
        }

        /// <summary>
        /// Gets or sets the AutomationId
        /// </summary>
        internal string AutomationId
        {
            get
            {
                return _automationId;
            }

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

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PlatformCarouselItem"/> is item selected.
        /// </summary>
        /// <value><c>true</c> if is item selected; otherwise, <c>false</c>.</value>
        internal bool IsItemSelected
        {
            get
            {
                return _isItemSelectedProperty;
            }

            set
            {
                _isItemSelectedProperty = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Decodes the sampled bitmap from resource.
        /// </summary>
        /// <returns>The sampled bitmap from resource.</returns>
        /// <param name="res">Resource value.</param>
        /// <param name="resId">Resource identifier.</param>
        /// <param name="reqWidth">Required width.</param>
        /// <param name="reqHeight">Required height.</param>
        public static Bitmap? DecodeSampledBitmapFromResource(Resources res, int resId, int reqWidth, int reqHeight)
        {
            using (BitmapFactory.Options options = new BitmapFactory.Options())
            {
                options.InJustDecodeBounds = true;
                options.InSampleSize = 8;
                options.InJustDecodeBounds = false;
                options.InScaled = false;
#pragma warning disable CA1422 
                options.InDither = false;
#pragma warning restore CA1422 
                options.InPreferredConfig = Bitmap.Config.Rgb565;
                BitmapFactory.DecodeResource(res, resId, options);
                options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);
                options.InJustDecodeBounds = false;
                return BitmapFactory.DecodeResource(res, resId, options);
            }
        }

        /// <summary>
        /// Calculates the size of the in sample.
        /// </summary>
        /// <returns>The in sample size.</returns>
        /// <param name="options">Options of bitmap.</param>
        /// <param name="reqWidth">Required width.</param>
        /// <param name="reqHeight">Required height.</param>
        public static int CalculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight)
        {
            int height = options.OutHeight;
            int width = options.OutWidth;
            int inSampleSize = 1;

            if (height > reqHeight || width > reqWidth)
            {
                int halfHeight = height / 2;
                int halfWidth = width / 2;

                while ((halfHeight / inSampleSize) >= reqHeight
                        && (halfWidth / inSampleSize) >= reqWidth)
                {
                    inSampleSize *= 2;
                }
            }

            return inSampleSize;
        }
		#endregion

		#region Override Methods

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <exclude/>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_contentView != null)
                {
                    _contentView = null;
                }

                if (_drawnView != null)
                {
                    _drawnView = null;
                }

                if (_parentItem != null)
                {
                    _parentItem = null;
                }

                if (_childView != null)
                {
                    _childView = null;
                }
            }

            base.Dispose(disposing);
        }

		/// <summary>
		/// On the size changed.
		/// </summary>
		/// <exclude/>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="oldWidth">Old width.</param>
		/// <param name="oldHeight">old height.</param>
		protected override void OnSizeChanged(int width, int height, int oldWidth, int oldHeight)
        {
            if (width <= 10)
            {
                return;
            }

            SetupView();

            GetImageFromView(_drawnView, ImageName);

            if (oldWidth != width || oldHeight != height)
            {
                Measure(MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified),
                        MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
            }

            base.OnSizeChanged(width, height, oldWidth, oldHeight);
        }
		#endregion

		#region Private Methods


		/// <summary>
		/// Gets the image from view.
		/// </summary>
		/// <param name="view">main view.</param>
		/// <param name="imageName">Image name.</param>
		internal void GetImageFromView(View? view, string? imageName)
		{
			using (ImageView imageView = new ImageView(_contextCarouselItem))
			{
				imageView.SetScaleType(ImageView.ScaleType.FitXy);

				this.RemoveExistingViews();

				if (view != null)
				{
					AddViewToParent(view);
				}
				else if (!string.IsNullOrEmpty(imageName))
				{
					AddImageViewToParent(imageView, imageName);
				}
			}
		}

		/// <summary>
		/// Removes existing views if any.
		/// </summary>
		void RemoveExistingViews()
		{
			if (ChildCount > 1)
			{
				RemoveViewAt(0);
			}
		}

		/// <summary>
		/// Adds the provided view to the parent.
		/// </summary>
		/// <param name="view">The view to be added.</param>
		void AddViewToParent(View view)
		{
			RemoveViewFromParent(view);
			if (ParentItem != null)
			{
				using (var viewLayout = new ViewGroup.LayoutParams(ParentItem.ItemWidth, ParentItem.ItemHeight))
				{
					using (var layParams = new LayoutParams(viewLayout))
					{
						view.LayoutParameters = layParams;
					}
				}
			}
			AddView(view);
		}

		/// <summary>
		/// Adds an ImageView with the specified image to the parent.
		/// </summary>
		/// <param name="imageView">The ImageView to be added.</param>
		/// <param name="imageName">The name of the image resource.</param>
		void AddImageViewToParent(ImageView imageView, string imageName)
		{
			if (ParentItem == null)
				return;

#pragma warning disable CS8602
			int resID = Resources.GetIdentifier(imageName, "drawable", _contextCarouselItem.PackageName);
#pragma warning restore CS8602

			RemoveViewFromParent(imageView);

			Bitmap? originalBitmap = DecodeSampledBitmapFromResource(Resources, resID, ParentItem.ItemWidth, ParentItem.ItemHeight);
			if (originalBitmap != null)
			{
				SetImageBitmap(imageView, originalBitmap);
				originalBitmap.Recycle();
				originalBitmap = null;
			}

			_childView = imageView;
			_childView.LayoutParameters = new LayoutParams(ParentItem.ItemWidth, ParentItem.ItemHeight);
			AddView(_childView);
		}

		/// <summary>
		/// Sets the bitmap for the ImageView.
		/// </summary>
		/// <param name="imageView">The ImageView to set the bitmap for.</param>
		/// <param name="originalBitmap">The original bitmap to be set.</param>
		void SetImageBitmap(ImageView imageView, Bitmap originalBitmap)
		{
			if (Bitmap.Config.Rgb565 != null)
			{
				using (Bitmap? img = originalBitmap.Copy(Bitmap.Config.Rgb565, true))
				{
					imageView.SetImageBitmap(img);
				}
			}
		}

		/// <summary>
		/// Sets up the view based on the current state of the ParentItem and Adapter.
		///</summary>summary>
		void SetupView()
        {
            if (ParentItem != null && ParentItem.Adapter != null)
            {
                _adapterView = ParentItem.Adapter.GetItemView(ParentItem, _index);

                if (_adapterView != null)
                {
                    SetupDrawnView();
                }
            }
            else if (_contentView != null)
            {
                _drawnView = ContentView;
                AttachTouchListener();
            }
        }

        /// <summary>
        /// Sets up the drawn view based on the current state of the ParentItem.
        /// </summary>
        void SetupDrawnView()
        {
            if (IsLastItemAndLoadMoreAllowed())
            {
                _drawnView = CreateOrGetLoadMoreView();
            }
            else
            {
                _drawnView = _adapterView;
                AttachTouchListener();
            }
        }

        /// <summary>
        /// Checks if the current item is the last one and if load more is allowed.
        /// </summary>
        /// <returns>True if it's the last item and load more is allowed; otherwise, false.</returns>
        bool IsLastItemAndLoadMoreAllowed()
        {
            return  ParentItem != null && ParentItem.TempDataSource != null &&
                   _index == ParentItem.TempDataSource.Count - 1 &&
                   ParentItem.TempItemsSource != null && ParentItem.ItemsSourceCollection != null &&
                   ParentItem.TempDataSource.Count > ParentItem.ItemsSourceCollection.Count &&
                   ParentItem.AllowLoadMore;
        }

        /// <summary>
        /// Creates or gets the load more view.
        /// </summary>
        /// <returns>The load more view.</returns>
        View? CreateOrGetLoadMoreView()
        {
            if(ParentItem == null)
                return null;
            if (ParentItem.LoadMoreView == null)
            {
                return CreateLoadMoreButton();
            }
            return ParentItem.LoadMoreView;
        }

        /// <summary>
        /// Creates a load more button.
        /// </summary>
        /// <returns>A TextView configured as a load more button.</returns>
        TextView CreateLoadMoreButton()
        {
            TextView button = new TextView(_contextCarouselItem)
            {
                Text = SfCarouselResources.LoadMoreText,
                TextSize = 18,
                Gravity = GravityFlags.Center,
                
            };
            if (ParentItem != null)
                button.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, ParentItem.ItemHeight, GravityFlags.Center);

            return button;
        }

        /// <summary>
        /// Attaches the touch listener to the drawn view if it's not null.
        /// </summary>
        void AttachTouchListener()
        {
            if (_drawnView != null)
            {
                _drawnView.Touch += DrawnView_Touch;
            }
        }

        void DrawnView_Touch(object? sender, TouchEventArgs e)
        {
            _parentItem?.OnClick(this);
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        void Init()
        {
            if (!IsItemSelected)
            {
                RequestLayout();
            }
        }

        /// <summary>
        /// Removes the view from parent.
        /// </summary>
        /// <param name="view">View of remove from parent.</param>
        void RemoveViewFromParent(View view)
        {
            if (view.Parent != null)
            {
                ViewGroup parent = (ViewGroup)view.Parent;
                parent.RemoveView(view);
            }
        }

        #endregion
    }
}
