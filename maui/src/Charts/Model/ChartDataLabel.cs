using Microsoft.Maui.Controls;
using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    /// <summary>
    /// ChartDataLabel is used to customize the appearance of the data label.
    /// </summary>
    public class ChartDataLabel : INotifyPropertyChanged
    {
        #region Fields

        string _label = string.Empty;
        Brush _background = Brush.Transparent;
        int _index = -1;
        double _xPosition = double.NaN;
        double _yPosition = double.NaN;
        object? _item;
        ChartDataLabelStyle? _labelStyle;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataLabel"/> class.
        /// </summary>
        public ChartDataLabel()
        {

        }

        #region public Properties

        /// <summary>
        /// Gets or sets the data label content.
        /// </summary>
        public string Label
        {
            get
            {
                return _label;
            }

            set
            {
                if (_label == value)
                {
                    return;
                }

                _label = value;
                OnPropertyChanged(nameof(Label));
            }
        }

        /// <summary>
        /// Returns the background color of the data label.
        /// </summary>
        public Brush Background
        {
            get
            {
                return _background;
            }

            internal set
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
        /// Returns the index of the data label.
        /// </summary>
        public int Index
        {
            get
            {
                return _index;
            }

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
        /// Returns the x-position of data label placement.
        /// </summary>
        public double XPosition
        {
            get
            {
                return _xPosition;
            }

            internal set
            {
                if (_xPosition == value)
                {
                    return;
                }

                _xPosition = value;
                OnPropertyChanged(nameof(XPosition));
            }
        }

        /// <summary>
        /// Returns the y-position of data label placement.
        /// </summary>
        public double YPosition
        {
            get
            {
                return _yPosition;
            }

            internal set
            {
                if (_yPosition == value)
                {
                    return;
                }

                _yPosition = value;
                OnPropertyChanged(nameof(YPosition));
            }
        }

        /// <summary>
        /// Gets the underlying item of the data label.
        /// </summary>
        public object? Item
        {
            get
            {
                return _item;
            }

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
        /// Gets or sets the label style to customize the appearance of the data label.
        /// </summary>
        internal ChartDataLabelStyle? LabelStyle
        {
            get
            {
                return _labelStyle;
            }

            set
            {
                if (_labelStyle == value)
                {
                    return;
                }

                _labelStyle = value;
                OnPropertyChanged(nameof(LabelStyle));
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <exclude/>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
