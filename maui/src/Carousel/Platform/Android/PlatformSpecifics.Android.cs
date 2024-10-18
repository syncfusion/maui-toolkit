using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Syncfusion.Maui.Toolkit.Carousel
{
	#region Carousel Adapter

	/// <summary>
	/// The Carousel adapter interface.
	/// </summary>
	/// <exclude/>
	public interface ICarouselAdapter
    {
        /// <summary>
        /// Gets the item view.
        /// </summary>
        /// <returns>The item view.</returns>
        /// <param name="carousel">Carousel control.</param>
        /// <param name="index">Index of carousel.</param>
        View? GetItemView(PlatformCarousel carousel, int index);
    }
    #endregion

    #region CustomImageView

    /// <summary>
    /// The <see cref="CustomImageView"/> class.
    /// </summary>
    internal class CustomImageView : FrameLayout
    {
        #region Fields

        /// <summary>
        /// The index.
        /// </summary>
        int _index;

        /// <summary>
        /// The height.
        /// </summary>
        float _height;

        /// <summary>
        /// The width.
        /// </summary>
        float _width;

        /// <summary>
        /// The context.
        /// </summary>
        Context? _context;

        /// <summary>
        /// The view.
        /// </summary>
        View? _view;

        /// <summary>
        /// The is dynamic.
        /// </summary>
        bool _isDynamic;

        /// <summary>
        /// The image view.
        /// </summary>
        ImageView? _imageView;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImageView"/> class.
        /// </summary>
        /// <param name="context">Context of custom image view.</param>
        /// <param name="view">View of custom image view.</param>
        /// <param name="widthArgs">Width of custom image view.</param>
        /// <param name="heightArgs">Height of custom image view.</param>
        public CustomImageView(Context context, View view, float widthArgs, float heightArgs) : base(context)
        {
            InitializeValues(context, heightArgs, widthArgs, view);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImageView"/> class.
        /// </summary>
        /// <param name="context">Context of the custom image view.</param>
        /// <param name="widthArgs">Width of custom image view.</param>
        /// <param name="heightArgs">Height of custom image view.</param>
        public CustomImageView(Context context, float widthArgs, float heightArgs) : base(context)
        {
            InitializeValues(context, heightArgs, widthArgs);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImageView"/> class.
        /// </summary>
        /// <param name="context">Context of the custom image view.</param>
        /// <param name="attribute">Attribute set value.</param>
        public CustomImageView(Context context, IAttributeSet attribute) : base(context, attribute)
        {
            InitializeValues(context);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImageView"/> class.
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        protected CustomImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImageView"/> class.
        /// </summary>
        /// <param name="context"></param>
        public CustomImageView(Context context) : base(context)
        {
            InitializeValues(context);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImageView"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        public CustomImageView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            InitializeValues(context);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomImageView"/> class.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attrs"></param>
        /// <param name="defStyleAttr"></param>
        /// <param name="defStyleRes"></param>
        public CustomImageView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            InitializeValues(context);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the _index.
        /// </summary>
        /// <value>The index.</value>
        public int Index
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
        /// Gets or sets the view.
        /// </summary>
        /// <value>The view.</value>
        public View? View
        {
            get
            {
                return _view;
            }

            set
            {
                _view = value;
                AddImageView();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Syncfusion.Maui.Toolkit.Carousel.CustomImageView"/> is dynamic.
        /// </summary>
        /// <value><c>true</c> if is dynamic; otherwise, <c>false</c>.</value>
        internal bool IsDynamic
        {
            get
            {
                return _isDynamic;
            }

            set
            {
                _isDynamic = value;
            }
        }

		#endregion

		#region Override Method
		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_imageView != null)
                {
                    _imageView = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Private Methods

        void InitializeValues(Context context, float height = 0, float width = 0, View? view = null)
        {
            _context = context;
            _height = height;
            _width = width;
            if (view != null)
            {
                View = view;
            }
        }

        /// <summary>
        /// Add the image view.
        /// </summary>
        void AddImageView()
        {
            if (_view != null)
            {
                if (!IsDynamic)
                {
                    _imageView = new ImageView(_context);
                    _imageView.LayoutParameters = new LayoutParams((int)_width, (int)_height, GravityFlags.Center);
                    var bitmap = this.GetBitmapFromView(_view);
                    if(bitmap != null)
                    {
                        Bitmap bitmap1 = Bitmap.CreateScaledBitmap(bitmap, (int)_width, (int)_height, true);
                        _imageView.SetImageBitmap(bitmap1);
                    }
                    var layoutparamter = new LayoutParams((int)_width, (int)_height, GravityFlags.Center);
                    AddView(_imageView, layoutparamter);
                }
                else
                {
                    var layoutparamter = new LayoutParams((int)_width, (int)_height, GravityFlags.Center);
                    if(_view.Parent != null)
                    {
                        ViewGroup parentItem = (ViewGroup)_view.Parent;
                        parentItem?.RemoveView(_view);
                    }
                    AddView(_view, layoutparamter);
                }
            }
        }

        /// <summary>
        /// Gets the bitmap from view.
        /// </summary>
        /// <returns>The bitmap from view.</returns>
        /// <param name="imageView">The imageView.</param>
        Bitmap? GetBitmapFromView(View imageView)
        {
            int viewHeight, viewWidth;
            imageView.LayoutParameters = new LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
            imageView.Measure(MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified), MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified));
            if (imageView.MeasuredHeight > 0)
            {
                viewHeight = imageView.MeasuredHeight;
                viewWidth = imageView.MeasuredWidth;
            }
            else
            {
                viewHeight = (int)_height;
                viewWidth = (int)_width;
            }

            imageView.Layout(0, 0, viewWidth, viewHeight);
            if(Bitmap.Config.Argb8888 != null)
            {
                Bitmap? bitmap = Bitmap.CreateBitmap(viewWidth, viewHeight, Bitmap.Config.Argb8888);
                Canvas canvas = new Canvas(bitmap);
                imageView.Draw(canvas);
                return bitmap;
            }
            return null;
        }
        #endregion
    }

    #endregion

    #region ItemAdaptar

    /// <summary>
    /// The <see cref="ItemAdapter"/> class.
    /// </summary>
    internal class ItemAdapter : RecyclerView.Adapter
    {
        #region Fields

        /// <summary>
        /// The temp source.
        /// </summary>
        private IList<PlatformCarouselItem>? _tempSource;

        /// <summary>
        /// The carousel.
        /// </summary>
        private PlatformCarousel? _carousel;

        /// <summary>
        /// The width of the view.
        /// </summary>
        private float _viewWidth;

        /// <summary>
        /// The height of the view.
        /// </summary>
        private float _viewHeight;

        /// <summary>
        /// The context.
        /// </summary>
        private Context _context;

        /// <summary>
        /// The passing view.
        /// </summary>
        private View? _passingView;

        /// <summary>
        /// The is item changed.
        /// </summary>
        private bool _isItemChanged = false;

        /// <summary>
        /// The index of the tap.
        /// </summary>
        private int _tapIndex = 0;

        /// <summary>
        /// The previous index carousel.
        /// </summary>
        private int _previousIndexCarousel = -1;

        /// <summary>
        /// The temp adapter.
        /// </summary>
        private View? _tempAdapter;

        /// <summary>
        /// The custom image view.
        /// </summary>
        private CustomImageView? _customImageView;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Syncfusion.Maui.Toolkit.Carousel.ItemAdapter"/> class.
        /// </summary>
        /// <param name="context">Context of item adapter.</param>
        /// <param name="carouselArgs">Carousel as parameter.</param>
        /// <param name="height">Height of adapter.</param>
        /// <param name="width">Width of adapter.</param>
        /// <param name="tempList">Temp list.</param>
        public ItemAdapter(Context context, PlatformCarousel carouselArgs, float height, float width, IList<PlatformCarouselItem> tempList)
        {
            _carousel = carouselArgs;
            _context = context;
            _tempSource = new List<PlatformCarouselItem>();
            _viewHeight = height;
            _viewWidth = width;
            _tempSource = tempList;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <value>The item count.</value>
        public override int ItemCount
        {
            get
            {
                if(_tempSource == null) {return 0;}
                return _tempSource.Count;
            }
        }

        /// <summary>
        /// On the bind view holder.
        /// </summary>
        /// <param name="holder">Holder of adapter.</param>
        /// <param name="position">Position value.</param>
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if (_carousel == null)
            {
                return;
            }

            if (position == (ItemCount - 1))
            {
                _carousel.IsLastIndex = true;
            }

            ViewHolder viewHolder = (ViewHolder)holder;
            _customImageView = (CustomImageView)viewHolder.ItemView;
            using (LinearLayout.LayoutParams imageParams = new LinearLayout.LayoutParams((int)_viewWidth, (int)_viewHeight))
            {
                PlatformCarouselItem? item = null;
                if (_tempSource != null)
                    item = _tempSource[position];
                _customImageView.LayoutParameters = imageParams;
                _customImageView.Index = position;
                if (_carousel.Adapter != null)
                {
                    _tempAdapter = _carousel.Adapter.GetItemView(_carousel, position);
                }

                if (_carousel.Adapter != null && _tempAdapter != null && _carousel.ViewMode == ViewMode.Linear)
                {
                   if (_carousel.TempDataSource != null && position == _carousel.TempDataSource.Count - 1 && _carousel.TempItemsSource != null && _carousel.ItemsSourceCollection != null && _carousel.TempDataSource.Count > _carousel.ItemsSourceCollection.Count && _carousel.AllowLoadMore)
                    {
                        if (_carousel.LoadMoreView == null)
                        {
                            _passingView = GetLoadMoreDefaultView();
                        }
                        else
                        {
                            _passingView = _carousel.LoadMoreView;
                        }

                        if (holder != null && holder.ItemView != null)
                        {
                            holder.ItemView.ContentDescription = _carousel.AutomationId + " Load More. Tap to load more items";
                        }
                    }
                    else if (_tempAdapter != null)
                    {
                        if (_carousel.ItemsSource != null && item != null)
                        {
                            holder.ItemView.ContentDescription = item.AutomationId + " PlatformCarouselItem " + (position + 1) + " of " + ((IList)_carousel.ItemsSource).Count;
                        }

                        _passingView = _tempAdapter;
                    }

                    _customImageView.IsDynamic = true;
                }
                else if(item != null)
                {
                    item.ParentItem = _carousel;
                    item.Index = position;
                    item.LayoutParameters = imageParams;
                    if (_carousel.ItemsSource != null)
                    {
                        holder.ItemView.ContentDescription = item.AutomationId + " PlatformCarouselItem " + (position + 1) + " of " + (_carousel.ItemsSource as IList)!.Count;
                        holder.ItemView.ContentDescription = item.AutomationId + " PlatformCarouselItem " + (position + 1) + " of " + ((IList)_carousel.ItemsSource).Count;
                    }

                    if (item.ImageName != null)
                    {
                        _customImageView.IsDynamic = false;
                    }
                    else
                    {
                        _customImageView.IsDynamic = true;
                    }

                    item.GetImageFromView(item.ContentView, item.ImageName);
                    _passingView = item;
                }
            }

            _customImageView.View = _passingView;
            _customImageView.Click += CustomImageView_Click;
        }

		/// <summary>
		/// On the create view holder.
		/// </summary>
		/// <exclude/>
		/// <returns>The create view holder.</returns>
		/// <param name="parent">Parent of adapter.</param>
		/// <param name="viewType">View type.</param>
		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            CustomImageView frame = new CustomImageView(_context, _viewWidth, _viewHeight);
            frame.LayoutParameters = new ViewGroup.LayoutParams((int)_viewWidth, (int)_viewHeight);
            frame.RequestLayout();
            return new ViewHolder(frame);
        }

		/// <summary>
		/// On the view recycled.
		/// </summary>
		/// <exclude/>
		/// <param name="holder">The Holder.</param>
		public override void OnViewRecycled(Java.Lang.Object holder)
        {
            var viewHolder = holder as ViewHolder;
            if (viewHolder != null && viewHolder.ItemView != null)
            {
                ((CustomImageView)viewHolder.ItemView).RemoveAllViewsInLayout();
                base.OnViewRecycled(viewHolder);
            }
        }

        /// <summary>
        /// Gets the item count.
        /// </summary>
        /// <param name="index">The Index.</param>
        internal void LoadMore(int index = 0)
        {
            _isItemChanged = false;
            if (_carousel != null && _carousel.LoadMoreItemsCount > 0 && (_carousel.TempDataSource != null && ((_carousel.AllowLoadMore && index == _carousel.TempDataSource.Count - 1) || (!_carousel.AllowLoadMore && index == _carousel.TempDataSource.Count))) && _carousel.AllowLoadMoreDataSource != null && _carousel.AllowLoadMoreDataSource.Count > 0)
            {
                _tapIndex = index;
                _previousIndexCarousel = index;
                if (_carousel.AllowLoadMore)
                {
                    _carousel.TempDataSource.RemoveAt(index);
                }

                if (_carousel.AllowLoadMoreDataSource != null)
                {
                    var count = _carousel.AllowLoadMoreDataSource.Count;
                    if (_carousel.ItemsSourceCollection != null)
                    {
                        if (_carousel.AllowLoadMoreDataSource.Count >= _carousel.LoadMoreItemsCount)
                        {
                            for (int i = 0; i < _carousel.LoadMoreItemsCount; i++)
                            {
                                _carousel.TempDataSource.Add(_carousel.AllowLoadMoreDataSource[0]);
                                _carousel.ItemsSourceCollection.Add(_carousel.AllowLoadMoreDataSource[0]);
                                _carousel.AllowLoadMoreDataSource.RemoveAt(0);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < count; i++)
                            {
                                _carousel.TempDataSource.Add(_carousel.AllowLoadMoreDataSource[0]);
                                _carousel.ItemsSourceCollection.Add(_carousel.AllowLoadMoreDataSource[0]);
                                _carousel.AllowLoadMoreDataSource.RemoveAt(0);
                            }
                        }
                    }

                    _isItemChanged = true;
                    if (_carousel.AllowLoadMoreDataSource.Count > 0 && _carousel.AllowLoadMore && _carousel.Context != null)
                    {
                        PlatformCarouselItem loadMoreItem = new PlatformCarouselItem(_carousel.Context);
                        if (_carousel.LoadMoreView == null)
                        {
                            TextView button = new TextView(_context);
                            button.Text = SfCarouselResources.LoadMoreText;
                            button.TextSize = 18;
                            button.Gravity = GravityFlags.Center;
                            loadMoreItem.ContentView = button;
                        }
                        else
                        {
                            loadMoreItem.ContentView = _carousel.LoadMoreView;
                        }

                        _carousel.TempDataSource.Add(loadMoreItem);
                    }
                    if(_carousel.ItemsSourceCollection != null)
                        _carousel.CountItem = _carousel.ItemsSourceCollection.Count;
                    int visibleItem = _carousel.Width / _carousel.ItemWidth;
                    _carousel.HorizontalGridView?.ScrollToPosition((int)_tapIndex - (visibleItem / 2));
                    NotifyItemChanged(_tapIndex);
                }
            }

            if (_carousel != null && !_carousel.AllowLoadMore)
            {
                _carousel.SelectedIndex = index;
            }
            else
            {
                if (_carousel != null && !_isItemChanged)
                {
                    if (_previousIndexCarousel != index)
                    {
                        _carousel.SelectedIndex = index;
                    }
                }
            }
        }

		/// <summary>
		/// Dispose the specified disposing.
		/// </summary>
		/// <exclude/>
		/// <param name="disposing">If set to <c>true</c> disposing.</param>
		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_tempSource != null)
                {
                    _tempSource = null;
                }

                if (_carousel != null)
                {
                    _carousel = null;
                }

                if (_passingView != null)
                {
                    _passingView = null;
                }

                if (_customImageView != null)
                {
                    _customImageView.Click -= CustomImageView_Click;
                    _customImageView = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Invoked when custom image is clicked.
        /// </summary>
        ///<param name="sender">Sender value.</param>
        /// <param name="e">E argument.</param>
        void CustomImageView_Click(object? sender, EventArgs e)
        {
            if (_carousel != null)
            {
                var imageView = sender as CustomImageView;
                _carousel.isImageClicked = true;

                if (imageView != null)
                {
                    LoadMore(imageView.Index);
                }
            }
        }

        /// <summary>
        /// Get the loadmore default view
        /// </summary>
        /// <returns></returns>
        View GetLoadMoreDefaultView()
        {
            TextView loadMoreDefaultView = new TextView(_context);
            loadMoreDefaultView.Text = SfCarouselResources.LoadMoreText;
            loadMoreDefaultView.TextSize = 18;
            loadMoreDefaultView.Gravity = GravityFlags.Center;

            return loadMoreDefaultView;
        }

        #endregion
    }

	#endregion

	#region ViewHolder

	/// <summary>
	/// The <see cref="ViewHolder"/> class.
	/// </summary>
	internal class ViewHolder : RecyclerView.ViewHolder
    {
		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Syncfusion.Maui.Toolkit.Carousel.ViewHolder"/> class.
		/// </summary>
		/// <param name="view">The View.</param>
		public ViewHolder(View view) : base(view)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Syncfusion.Maui.Toolkit.Carousel.ViewHolder"/> class
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        public ViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
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
            base.Dispose(disposing);
        }

		#endregion
	}

	#endregion
}
