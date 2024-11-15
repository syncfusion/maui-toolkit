using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// This class contains information about the tooltip.
	/// </summary>
	public class TooltipInfo : INotifyPropertyChanged
    {
        #region Fields

        int _index;
        object? _item = null;
        string _text = string.Empty;
        string _fontFamily = string.Empty;
        FontAttributes _fontAttributes = FontAttributes.None;
        Color _textColor = Colors.White;
        Brush? _background = Brush.Black;
        float _fontSize = 12;
        Thickness _margin = new Thickness(0);

        #endregion

        #region Internal properties

        internal TooltipPosition Position { get; set; }

        internal Rect TargetRect { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the x value for the tooltip position.
        /// </summary>
        /// <value>It accepts <c>float</c> values.</value>
        public float X { get; internal set; }

        /// <summary>
        /// Gets the y value for the tooltip position.
        /// </summary>
        /// <value>It accepts <c>float</c> values.</value>
        public float Y { get; internal set; }

        /// <summary>
        /// Gets the associated series or chart.
        /// </summary>
        public readonly object Source;

        /// <summary>
        /// Gets or sets a value that displays on the tooltip.
        /// </summary>
        /// <value>It accepts <c>string</c> values.</value>
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                {
                    return;
                }

                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        /// <summary>
        /// Gets or sets a value to specify the FontFamily for the tooltip label.
        /// </summary>
        /// <value>It accepts <c>string</c> values.</value>
        public string FontFamily
        {
            get { return _fontFamily; }
            set
            {
                if (_fontFamily == value)
                {
                    return;
                }

                _fontFamily = value;
                OnPropertyChanged(nameof(FontFamily));
            }
        }

        /// <summary>
        /// Gets or sets a value to specify the FontAttributes for the tooltip label.
        /// </summary>
        /// <value>It accepts <see cref="Microsoft.Maui.Controls.FontAttributes"/> values.</value>
        public FontAttributes FontAttributes
        {
            get { return _fontAttributes; }
            set
            {
                if (_fontAttributes == value)
                {
                    return;
                }

                _fontAttributes = value;
                OnPropertyChanged(nameof(FontAttributes));
            }
        }

        /// <summary>
        /// Gets or sets the brush value to customize the tooltip background.
        /// </summary>
        /// <value>It accepts the <see cref="Brush"/> values.</value>
        public Brush? Background
        {
            get { return _background; }

            set
            {
                if (_background == value)
                {
                    return;
                }

                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }

        /// <summary>
        /// Gets or sets the color value to customize the text color of the tooltip label.
        /// </summary>
        /// <value>It accepts the <see cref="Color"/> values.</value>
        public Color TextColor
        {
            get { return _textColor; }

            set
            {
                if (_textColor == value)
                {
                    return;
                }

                _textColor = value;
                OnPropertyChanged(nameof(TextColor));
            }
        }

        /// <summary>
        /// Gets or sets a value to change the label's text size of the tooltip.
        /// </summary>
        /// <value>It accepts the <c>float</c> values and the default value is 14.</value>
        public float FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize == value)
                {
                    return;
                }

                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        /// <summary>
        /// Gets or sets a thickness value to adjust the tooltip margin.
        /// </summary>
        /// <value>It accepts the <see cref="Thickness"/> values and the default value is 0.</value>
        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                if (_margin == value)
                {
                    return;
                }

                _margin = value;
                OnPropertyChanged(nameof(Margin));
            }
        }

        /// <summary>
        /// Gets the index for the corresponding segment.
        /// </summary>
        /// <value>It accepts <c>int</c> values.</value>
        public int Index
        {
            get { return _index; }

            internal set
            {
                if (_index == value)
                {
                    return;
                }

                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        /// <summary>
        /// Gets the data object for the associated segment 
        /// </summary>
        /// <value>It accepts <c>object</c> values.</value>
        public object? Item
        {
            get { return _item; }

            internal set
            {
                if (_item == value)
                {
                    return;
                }

                _item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

		#endregion

		#region Event

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		/// <exclude/>
		public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipInfo"/> class.
        /// </summary>
        public TooltipInfo(object source)
        {
            Source = source;
        }

        #endregion

        #region Methods

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}