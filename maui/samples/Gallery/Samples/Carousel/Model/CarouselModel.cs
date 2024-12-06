using System.ComponentModel;
using System.Windows.Input;

namespace Syncfusion.Maui.ControlsGallery.Carousel.Carousel
{
	public partial class CarouselModel : INotifyPropertyChanged
	{
		#region private properties

		/// <summary>
		/// The image.
		/// </summary>
		private string _image = string.Empty;

		/// <summary>
		/// The name.
		/// </summary>
		private string _name = string.Empty;

		/// <summary>
		/// The description.
		/// </summary>
		private string _description = string.Empty;

		/// <summary>
		/// The close command.
		/// </summary>
		private ICommand? _closeCommand;

		/// <summary>
		/// The color of the item.
		/// </summary>
		private Color _itemColor = Colors.White;

		/// <summary>
		/// 
		/// </summary>
		private string _checkValue = "C";
		/// <summary>
		/// The grid visible.
		/// </summary>
		private bool _gridVisible;

		/// <summary>
		/// The tap command.
		/// </summary>
		private ICommand? _tapCommand;



		#endregion
		#region Public properties
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				string text = value.ToString().Replace("-WF", "", StringComparison.Ordinal);

				if (text.Contains("-", StringComparison.Ordinal))
				{
					_name = text.Substring(0, text.IndexOf('-', StringComparison.Ordinal));
					return;
				}
				_name = text;
			}
		}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		/// <summary>
		/// Gets or sets the close command.
		/// </summary>
		/// <value>The close command.</value>
		public ICommand CloseCommand
		{
			set { _closeCommand = value!; }
			get { return _closeCommand!; }
		}

		/// <summary>
		/// Gets or sets the color of the item.
		/// </summary>
		/// <value>The color of the item.</value>
		public Color ItemColor
		{
			get
			{
				return _itemColor;
			}
			set
			{
				_itemColor = value;
				RaisePropertyChanged("ItemColor");
			}
		}

		/// <summary>
		/// Gets or sets the color of the item.
		/// </summary>
		/// <value>The color of the item.</value>
		public string CheckValue
		{
			get
			{
				return _checkValue;
			}
			set
			{
				_checkValue = value;
				RaisePropertyChanged("CheckValue");
			}
		}

		/// <summary>
		/// Gets or sets the tag.
		/// </summary>
		/// <value>The tag.</value>
		public string? Tag { get; set; }

		/// <summary>
		/// Gets or sets the unicode.
		/// </summary>
		/// <value>The unicode.</value>
		public string? Unicode { get; set; }

		/// <summary>
		/// Gets or sets the name of the watch.
		/// </summary>
		/// <value>The name of the watch.</value>
		public string ImageName
		{
			get
			{
				return _image;
			}

			set
			{
				_image = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:SampleBrowser.SfCarousel.CarouselModel"/> grid visible.
		/// </summary>
		/// <value><c>true</c> if grid visible; otherwise, <c>false</c>.</value>
		public bool GridVisible
		{
			get
			{
				return _gridVisible;
			}

			set
			{
				_gridVisible = value;
				RaisePropertyChanged("GridVisible");
			}
		}

		/// <summary>
		/// Gets or sets the tap command.
		/// </summary>
		/// <value>The tap command.</value>
		public ICommand? TapCommand
		{
			set { _tapCommand = value!; }
			get { return _tapCommand; }
		}

		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;


		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SampleBrowser.SfCarousel.CarouselModel"/> class.
		/// </summary>
		/// <param name="imagestr">Imagestr.</param>
		public CarouselModel(string imagestr, string name, string description)
		{
			ImageName = imagestr;
			_description = description;
			Name = name;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:SampleBrowser.SfCarousel.CarouselModel"/> class.
		/// </summary>
		public CarouselModel()
		{
			TapCommand = new Command<object>(TappedEvent);
			CloseCommand = new Command<object>(CloseCommandAction);

		}


		#endregion

		#region raised event

		public void RaisePropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion

		#region tap event action

		/// <summary>
		/// Closes the command action.
		/// </summary>
		/// <param name="s">S.</param>
		private void CloseCommandAction(object s)
		{
			if (s is Grid grids)
			{
				grids.IsVisible = false;
			}

		}
		/// <summary>
		/// Tappeds the event.
		/// </summary>
		/// <param name="s">S.</param>
		private void TappedEvent(object s)
		{
			ItemColor = (Color)s;
		}

		#endregion
	}


}
