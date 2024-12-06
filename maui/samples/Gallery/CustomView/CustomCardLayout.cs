using Microsoft.Maui.Controls.Shapes;

namespace Syncfusion.Maui.ControlsGallery.CustomView
{
	/// <summary>
	/// 
	/// </summary>
	public partial class CustomCardLayout : Grid
	{
		readonly VerticalStackLayout _verticalStackLayout;
		readonly Grid _titleGrid;
		readonly Label _titleLabel;
		readonly TapGestureRecognizer? _tapGestureRecognizer;
		readonly MainPageAndroid? _mobilePageAndroid;
		readonly MainPageiOS? _mobilePageiOS;
		bool _showExpandIcon = true;
		readonly SfEffectsViewAdv _effectsViewAdv;

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomCardLayout), null, propertyChanged: OnTitleChangedProperty);

		/// <summary>
		/// 
		/// </summary>
		public static readonly BindableProperty CardContentProperty =
			BindableProperty.Create(nameof(CardContent), typeof(View), typeof(CustomCardLayout), null, propertyChanged: OnContentPropertyChanged);

		/// <summary>
		/// 
		/// </summary>
		internal SampleModel? SampleModel { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public View CardContent
		{
			get => (View)GetValue(CardContentProperty);
			set => SetValue(CardContentProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		/// <summary>
		/// 
		/// </summary>
		public bool ShowExpandIcon
		{
			get
			{
				return _showExpandIcon;
			}
			set
			{
				_showExpandIcon = value;
				UpdateExpandIcon();
			}
		}

		private static void OnTitleChangedProperty(BindableObject bindable, object oldValue, object newValue)
		{
			var title = bindable as CustomCardLayout;
			title?.TitleChange((string)newValue);

		}

		private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var bind = bindable as CustomCardLayout;
			bind?.ContentChanged((View)newValue);
		}

		private void TitleChange(string newValue)
		{
			_titleLabel.Text = newValue;
		}

		private void ContentChanged(View newValue)
		{
			if (_verticalStackLayout.Children.Count > 1)
			{
				_verticalStackLayout.RemoveAt(1);
			}
			_verticalStackLayout.Add(newValue);
		}

		/// <summary>
		/// 
		/// </summary>
		public CustomCardLayout(MainPageAndroid? androidPage = null, MainPageiOS? iOSPage = null, string badgeText = "")
		{
			_mobilePageAndroid = androidPage;
			_mobilePageiOS = iOSPage;

			_titleGrid = [];
			_titleGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
			_titleGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 48 });

			_verticalStackLayout = [];
			_titleLabel = new Label()
			{
				FontFamily = "Roboto-Regular",
				FontSize = 14,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				VerticalTextAlignment = TextAlignment.Center,
			};

			if (string.IsNullOrEmpty(badgeText))
			{
				_titleGrid.Children.Add(_titleLabel);
				Grid.SetColumnSpan(_titleLabel, 2);
			}
			else
			{
				_titleLabel.Padding = new Thickness(0, 0, 20, 0);
			}
			Label label = new()
			{
				Text = "\ue7cd",
				FontFamily = "MauiSampleFontIcon",
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				FontSize = 24
			};

			_effectsViewAdv = new()
			{
				Content = label

			};
			Border expandiconBorder = new()
			{
				WidthRequest = 40,
				HeightRequest = 40,
				StrokeThickness = 0,
				BackgroundColor = Colors.Transparent,
				StrokeShape = new RoundRectangle
				{
					CornerRadius = new CornerRadius(30)
				},
				Content = _effectsViewAdv
			};

			_titleGrid.Children.Add(expandiconBorder);
			Grid.SetColumn(expandiconBorder, 1);

			_tapGestureRecognizer = new TapGestureRecognizer();
			_tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
			_titleGrid.GestureRecognizers.Add(_tapGestureRecognizer);
			_effectsViewAdv.GestureRecognizers.Add(_tapGestureRecognizer);
			_verticalStackLayout.Add(_titleGrid);
			_verticalStackLayout.IsClippedToBounds = true;

			Padding = 5;
			var layoutBorder = new Border()
			{
				StrokeThickness = 1,
				Background = Colors.Transparent
			};

			if (Application.Current != null)
			{
				AppTheme currentTheme = Application.Current.RequestedTheme;

				if (currentTheme == AppTheme.Dark)
				{
					layoutBorder.Stroke = Color.FromRgba("#49454F");
				}
				else
				{
					layoutBorder.Stroke = Color.FromRgba("#CAC4D0");
				}
			}

			layoutBorder.StrokeShape = new RoundRectangle()
			{
				CornerRadius = new CornerRadius(10),
			};
			layoutBorder.Content = _verticalStackLayout;
			Children.Add(layoutBorder);
		}

		private void UpdateExpandIcon()
		{
			if (_effectsViewAdv != null)
			{
				_effectsViewAdv.IsVisible = ShowExpandIcon;
			}
		}

		private void TapGestureRecognizer_Tapped(object? sender, EventArgs e)
		{
			if (ShowExpandIcon)
			{
#if ANDROID
				_mobilePageAndroid?.LoadSample(SampleModel, null, true);
#else
				_mobilePageiOS?.LoadSample(SampleModel, null, true);
#endif
			}
		}
	}
}
