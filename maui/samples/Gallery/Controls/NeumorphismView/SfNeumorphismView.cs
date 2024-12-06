namespace Syncfusion.Maui.ControlsGallery.CustomView
{
	/// <summary>
	/// 
	/// </summary>
	public partial class SfNeumorphismView : ContentView
	{
		readonly Grid _grid;

		readonly GraphicsView _graphicsView;
		/// <summary>
		/// 
		/// </summary>
		public SfNeumorphismView()
		{
			Drawable = new SfNeumorphismDrawer();

			_grid = [];
			_grid.Margin = new Thickness(0);

			_graphicsView = new GraphicsView
			{
				Margin = new Thickness(0),
				BackgroundColor = Colors.Transparent
			};
			_graphicsView.SetBinding(GraphicsView.DrawableProperty, new Binding() { Path = nameof(Drawable), Source = this });

			_grid.Children.Add(_graphicsView);
			base.Content = _grid;
		}

		/// <summary>
		/// 
		/// </summary>
		public SfNeumorphismDrawer Drawable
		{
			get { return (SfNeumorphismDrawer)GetValue(DrawableProperty); }
			set { SetValue(DrawableProperty, value); }
		}
		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty DrawableProperty =
			BindableProperty.Create(nameof(Drawable), typeof(SfNeumorphismDrawer), typeof(SfNeumorphismView), defaultValue: null, propertyChanged: OnDrawablePropertyChanged);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		protected static void OnDrawablePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{

		}
		/// <summary>
		/// 
		/// </summary>
		public new View Content
		{
			get { return (View)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}
		/// <summary>
		/// 
		/// </summary>
		public static readonly new BindableProperty ContentProperty =
			BindableProperty.Create(nameof(Content), typeof(View), typeof(SfNeumorphismView), defaultValue: null, propertyChanged: OnContentPropertyChanged);
		/// <summary>
		/// 
		/// </summary>
		public void Invalidate()
		{
			_graphicsView.Invalidate();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="bindable"></param>
		/// <param name="oldValue"></param>
		/// <param name="newValue"></param>
		protected static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			SfNeumorphismView view = (SfNeumorphismView)bindable;
			if (newValue is View newView)
			{
				if (!view._grid.Children.Contains(newView))
				{
					view._grid.Children.Add(newView);
				}
			}

			if (oldValue is View oldView)
			{
				if (view._grid.Children.Contains(oldView))
				{
					view._grid.Children.Remove(oldView);
				}
			}
		}
	}
}