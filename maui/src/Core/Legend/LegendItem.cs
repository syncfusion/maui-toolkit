using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit
{
    /// <summary>
    /// Represents a legend item that can notify property changes and implements the <see cref="ILegendItem"/> interface.
    /// </summary>
    public class LegendItem : INotifyPropertyChanged, ILegendItem
    {
        #region Fields

        int _index;
        object? _item = null;
        object? _source = null;
        string _text = string.Empty;
        string _fontFamily = string.Empty;
        FontAttributes _fontAttributes = FontAttributes.None;
        Brush _iconBrush = new SolidColorBrush(Colors.Transparent);
        Color _textColor = Colors.Black;
        Color _actualColor = Colors.Black;
        ShapeType _iconType = ShapeType.Rectangle;
        double _iconHeight = 12;
        double _iconWidth = 12;
        float _fontSize = 12;
        Thickness _textMargin = new Thickness(0);
        bool _isToggled = false;
        bool _isIconVisible = true;
        Brush _disableBrush = new SolidColorBrush(Color.FromRgba(0, 0, 0, 0.38));

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the corresponding icon color for legend item.
        /// </summary>
        public Brush IconBrush
        {
            get { return _iconBrush; }

            set
            {
                if (_iconBrush == value)
                {
                    return;
                }

                _iconBrush = value;
                OnPropertyChanged(nameof(IconBrush));
            }
        }

        /// <summary>
        /// Gets or sets the height of the legend icon.
        /// </summary>
        /// <value>This property takes double value.</value>
        public double IconHeight
        {
            get { return _iconHeight; }
            set
            {
                if (_iconHeight == value)
                {
                    return;
                }

                _iconHeight = value;
                OnPropertyChanged(nameof(IconHeight));
            }
        }

        /// <summary>
        /// Gets or sets the width of the legend icon.
        /// </summary>
        /// <value>This property takes double value.</value>
        public double IconWidth
        {
            get { return _iconWidth; }
            set
            {
                if (_iconWidth == value)
                {
                    return;
                }

                _iconWidth = value;
                OnPropertyChanged(nameof(IconWidth));
            }
        }

        /// <summary>
        /// Gets the corresponding index for legend item.
        /// </summary>
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
        /// Gets or sets a value indicating whether to display the legend icon.
        /// </summary>
        public bool IsIconVisible
        {
            get { return _isIconVisible; }

            set
            {
                if (_isIconVisible == value)
                {
                    return;
                }

                _isIconVisible = value;
                OnPropertyChanged(nameof(IsIconVisible));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the legend item is toggled or not.
        /// </summary>
        public bool IsToggled
        {
            get { return _isToggled; }

            set
            {
                if (_isToggled == value)
                {
                    return;
                }

                _isToggled = value;
                UpdateActualTextColor();
                OnPropertyChanged(nameof(IsToggled));
            }
        }

        /// <summary>
        /// Gets the corresponding data point for series.
        /// </summary>
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

        /// <summary>
        /// Gets the origin of the legend item.
        /// </summary>
        public object? Source
        {
            get { return _source; }

            internal set
            {
                if (_source == value)
                {
                    return;
                }

                _source = value;
                OnPropertyChanged(nameof(Source));
            }
        }

        /// <summary>
        /// Gets or sets the corresponding label for legend item.
        /// </summary>
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
        /// Gets the font attribute type for the legend item label.
        /// </summary>
        public FontAttributes FontAttributes
        {
            get { return _fontAttributes; }
            internal set
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
        /// Gets the font family for the legend item label.
        /// </summary>
        public string FontFamily
        {
            get { return _fontFamily; }
            internal set
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
        /// Gets the font size for the legend label text. 
        /// </summary>
        public float FontSize
        {
            get { return _fontSize; }
            internal set
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
        /// Gets the icon type in legend.
        /// </summary>
        public ShapeType IconType
        {
            get { return _iconType; }
            internal set
            {
                if (_iconType == value)
                {
                    return;
                }

                _iconType = value;
                OnPropertyChanged(nameof(IconType));
            }
        }

        /// <summary>
        /// Gets the corresponding text color for legend item.
        /// </summary>
        public Color TextColor
        {
            get { return _textColor; }
            internal set
            {
                if (_textColor == value)
                {
                    return;
                }

                _textColor = value;
                UpdateActualTextColor();
                OnPropertyChanged(nameof(TextColor));
            }
        }

        /// <summary>
        /// Gets the margin of the legend text.
        /// </summary>
        public Thickness TextMargin
        {
            get { return _textMargin; }
            internal set
            {
                if (_textMargin == value)
                {
                    return;
                }

                _textMargin = value;
                OnPropertyChanged(nameof(TextMargin));
            }
        }

        /// <summary>
        /// Gets the actual text color for the legend item.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Color ActualTextColor
        {
            get { return _actualColor; }
            internal set
            {
                if (_actualColor == value)
                {
                    return;
                }

                _actualColor = value;
                OnPropertyChanged(nameof(ActualTextColor));
            }
        }

        #endregion

        #region Internal Properties

        internal Brush DisableBrush
        {
            get { return _disableBrush; }

            set
            {
                if (_disableBrush == value)
                {
                    return;
                }

                _disableBrush = value;
                UpdateActualTextColor();
                OnPropertyChanged(nameof(DisableBrush));
            }
        }

		#endregion

		#region Private Properties
		/// <exclude/>
		Brush ILegendItem.DisableBrush { get => DisableBrush; set => DisableBrush = value; }
		/// <exclude/>
		FontAttributes ILegendItem.FontAttributes { get => FontAttributes; set => FontAttributes = value; }
		/// <exclude/>
		string ILegendItem.FontFamily { get => FontFamily; set => FontFamily = value; }
		/// <exclude/>
		float ILegendItem.FontSize { get => FontSize; set => FontSize = value; }
		/// <exclude/>
		ShapeType ILegendItem.IconType { get => IconType; set => IconType = value; }
		/// <exclude/>
		Color ILegendItem.TextColor { get => TextColor; set => TextColor = value; }
		/// <exclude/>
		Thickness ILegendItem.TextMargin { get => TextMargin; set => TextMargin = value; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendItem"/> class.
        /// </summary>
        public LegendItem()
        {
        }

        #endregion

        #region event

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <exclude/>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Methods

        void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        void UpdateActualTextColor()
        {
            SolidColorBrush disableBrush = (SolidColorBrush)_disableBrush;
            ActualTextColor = _isToggled ? disableBrush.Color : _textColor;
        }

        #endregion
    }
}
