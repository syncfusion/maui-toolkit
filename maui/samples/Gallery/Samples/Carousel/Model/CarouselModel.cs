using System.ComponentModel;
using System.Windows.Input;

namespace Syncfusion.Maui.ControlsGallery.Carousel.Carousel
{
    public class CarouselModel : INotifyPropertyChanged
    {
        #region private properties

        /// <summary>
        /// The image.
        /// </summary>
        private string image = string.Empty;

        /// <summary>
        /// The name.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// The description.
        /// </summary>
        private string description = string.Empty;

        /// <summary>
        /// The close command.
        /// </summary>
        private ICommand? closeCommand;

        /// <summary>
        /// The color of the item.
        /// </summary>
        private Color itemColor = Colors.White;

        /// <summary>
        /// 
        /// </summary>
        private string checkValue = "C";
        /// <summary>
        /// The grid visible.
        /// </summary>
        private bool gridVisible = false;

        /// <summary>
        /// The tap command.
        /// </summary>
        private ICommand? tapCommand;
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
                return name;
            }
            set
            {
                string text = value.ToString().Replace("-WF", "", StringComparison.Ordinal);

                if (text.Contains("-", StringComparison.Ordinal))
                {
                    name = text.Substring(0, text.IndexOf('-', StringComparison.Ordinal));
                    return;
                }
                name = text;
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Gets or sets the close command.
        /// </summary>
        /// <value>The close command.</value>
        public ICommand CloseCommand
        {
            set { closeCommand = value!; }
            get { return closeCommand!; }
        }

        /// <summary>
        /// Gets or sets the color of the item.
        /// </summary>
        /// <value>The color of the item.</value>
        public Color ItemColor
        {
            get
            {
                return itemColor;
            }
            set
            {
                itemColor = value;
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
                return checkValue;
            }
            set
            {
                checkValue = value;
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
                return image;
            }

            set
            {
                image = value;
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
                return gridVisible;
            }

            set
            {
                gridVisible = value;
                RaisePropertyChanged("GridVisible");
            }
        }

        /// <summary>
        /// Gets or sets the tap command.
        /// </summary>
        /// <value>The tap command.</value>
        public ICommand? TapCommand
        {
            set { tapCommand = value!; }
            get { return tapCommand; }
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
            this.description = description;
            this.Name = name;         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SampleBrowser.SfCarousel.CarouselModel"/> class.
        /// </summary>
        public CarouselModel()
        {
            TapCommand = new Command<Object>(TappedEvent);
            CloseCommand = new Command<Object>(CloseCommandAction);

        }


        #endregion

        #region raised event

        public void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region tap event action

        /// <summary>
        /// Closes the command action.
        /// </summary>
        /// <param name="s">S.</param>
        private void CloseCommandAction(Object s)
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
        private void TappedEvent(Object s)
        {
            ItemColor = (Color)s;
        }

        #endregion
    }


}
