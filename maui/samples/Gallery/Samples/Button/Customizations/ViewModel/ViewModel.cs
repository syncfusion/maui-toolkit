using Syncfusion.Maui.Toolkit.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Syncfusion.Maui.ControlsGallery.Buttons.Button;

public partial class ViewModel : INotifyPropertyChanged
{
    public string[] TextTransFormList { get; set; } = ["Default", "None", "Lowercase", "Uppercase"];

    public string[] AspectList { get; set; } = ["AspectFit", "AspectFill", "Fill", "Center"];

    #region Fields
    /// <summary>
    /// The background image of button
    /// </summary>
    public ImageSource? backgroundImage;

	/// <summary>
	/// The background image aspect for button.
	/// </summary>
	public Aspect backgroundImageAspect;

	/// <summary>
	/// The border color of button.
	/// </summary>
	private Color _borderColor = Color.FromArgb("A007A3");

    /// <summary>
    /// Represents the text color
    /// </summary>
    private Color _textColor = Colors.White;

    /// <summary>
    /// Represents the visibility of image
    /// </summary>
    private bool _showImage = true;

    /// <summary>
    /// The corner radius slider.
    /// </summary>
#if ANDROID || IOS
    private int _rightSlider = 8;
#elif WINDOWS || MACCATALYST
    private int _rightSlider = 8;
#endif

    /// <summary>
    /// The corner radius of button.
    /// </summary>
#if ANDROID || IOS
    private CornerRadius _cornerRadius = 8;
#elif WINDOWS || MACCATALYST
    private CornerRadius _cornerRadius = 8;
#endif

    /// <summary>
    /// The default corner radius.
    /// </summary>
#if ANDROID || IOS
    private int _leftSlider = 8;
#elif WINDOWS || MACCATALYST
    private int _leftSlider = 8;
#endif

    /// <summary>
    /// Represents the border width
    /// </summary>
    private int _borderWidth = 1;

    /// <summary>
    /// The text of button.
    /// </summary>
    private string _text = "Enter Text";

    /// <summary>
    /// The can show background image
    /// </summary>
    private bool _canShowBackgroundImage = false;

    /// <summary>
    /// Represents the enable or disable the shadow
    /// </summary>
    private bool _enableShadow;

    /// <summary>
    /// The TextTransform of Button Text
    /// </summary>
    private TextTransform _textTransform = TextTransform.Default;

    /// <summary>
    /// Selected Index of TextTransForm
    /// </summary>
    private int _textTransformSelectedIndex = 0;

	/// <summary>
	/// Selected Index of BackgroundImageAspect.
	/// </summary>
    private int _aspectSelectedIndex = 0;
    
    /// <summary>
    /// Represents whether the AspectList feature is enabled.
    /// </summary>
    private bool _enableAspectList = false;

    #endregion

    #region Property


    /// <summary>
    /// Gets or sets the color of the border of button.
    /// </summary>
    /// <value>The color of the border of button.</value>
    public Color BorderColor
    {
        get
        {
            return _borderColor;
        }

        set
        {
            _borderColor = value;
            OnPropertyChanged("BorderColor");
        }
    }

    /// <summary>
    /// Gets or Sets the text color
    /// </summary>
    public Color TextColor
    {
        get
        {
            return _textColor;
        }

        set
        {
            _textColor = value;
            OnPropertyChanged("TextColor");
        }
    }

    /// <summary>
    /// Gets or sets the slider value.
    /// </summary>
    /// <value>The slider value.</value>
#if ANDROID || IOS
    public int RightCornerRadius
    {
        get
        {
            return _rightSlider;
        }
        set
        {
            _rightSlider = value;
            CornerRadius = new CornerRadius(_cornerRadius.TopLeft, value, value, _cornerRadius.BottomRight);
            OnPropertyChanged("RightCornerRadius");
        }
    }
#elif WINDOWS || MACCATALYST
    public int RightCornerRadius
    {
        get
        {
            return _rightSlider;
        }
        set
        {
            _rightSlider = value;
            CornerRadius = new CornerRadius(_cornerRadius.TopLeft, value, value, _cornerRadius.BottomRight);
            OnPropertyChanged("RightCornerRadius");
        }
    }
#endif
    /// <summary>
    /// Gets or sets the slider value.
    /// </summary>
    /// <value>The slider value.</value>
#if ANDROID || IOS
    public int LeftCornerRadius
    {
        get
        {
            return _leftSlider;
        }
        set
        {
            _leftSlider = value;
            CornerRadius = new CornerRadius(value, _cornerRadius.TopRight, _cornerRadius.BottomLeft, value);
            OnPropertyChanged("LeftCornerRadius");
        }
    }
#elif MACCATALYST || WINDOWS
    public int LeftCornerRadius
    {
        get
        {
            return _leftSlider;
        }
        set
        {
            _leftSlider = value;
            CornerRadius = new CornerRadius(value, _cornerRadius.TopRight, _cornerRadius.BottomLeft, value);
            OnPropertyChanged("LeftCornerRadius");
        }
    }
#endif

    /// <summary>
    /// Gets or sets the border width.
    /// </summary>
    public int BorderWidth
    {
        get
        {
            return _borderWidth;
        }
        set
        {
            _borderWidth = value;
            OnPropertyChanged("BorderWidth");
        }
    }


    /// <summary>
    /// Gets or sets the corner radius.
    /// </summary>
    /// <value>The corner radius.</value>
#if ANDROID || IOS
    public CornerRadius CornerRadius
    {
        get
        {
            return _cornerRadius;
        }
        set
        {

            _cornerRadius = value;
            OnPropertyChanged("CornerRadius");
        }
    }
#elif WINDOWS || MACCATALYST
    public CornerRadius CornerRadius
    {
        get
        {
            return _cornerRadius;
        }
        set
        {

            _cornerRadius = value;
            OnPropertyChanged("CornerRadius");
        }
    }
#endif


    /// <summary>
    /// Gets or Sets the image visibility
    /// </summary>
    public bool ShowImage
    {
        get
        {
            return _showImage;
        }
        set
        {
            _showImage = value;

            OnPropertyChanged("ShowImage");
        }
    }


    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>The text.</value>
    public string Text
    {
        get
        {
			return _text;
        }
        set
        {
            _text = value;
            OnPropertyChanged("Text");
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="T:button.buttonViewModel"/> is enable stack.
    /// </summary>
    /// <value><c>true</c> if is enable stack; otherwise, <c>false</c>.</value>
    public bool CanShowBackgroundImage
    {
        get
        {
            return _canShowBackgroundImage;
        }
        set
        {
            _canShowBackgroundImage = value;
            if (value)
            {
				BackgroundImage = "april.png";
                EnableAspectList = true;
			}
            else
            {
                BackgroundImage = null;
                EnableAspectList = false;
            }
            OnPropertyChanged("CanShowBackgroundImage");
            OnPropertyChanged("BackgroundImage");
        }
    }

    /// <summary>
    /// Source for transparent background
    /// </summary>
    public ImageSource? BackgroundImage
    {
        get
        {

            return backgroundImage;
        }
        set
        {
            backgroundImage=value;
            OnPropertyChanged("BackgroundImage");
        }
    }

	/// <summary>
	/// Gets or sets the background image aspect for button..
	/// </summary>
	public Aspect BackgroundImageAspect
	{
		get { return backgroundImageAspect; }
		set
		{
			backgroundImageAspect = value;
			OnPropertyChanged("BackgroundImageAspect");
		}
	}

	/// <summary>
	/// Gets or sets the selected index of the BackgroundImageAspect
	/// </summary>
	public int AspectSelectedIndex
	{
		get { return _aspectSelectedIndex; }
		set
		{
			_aspectSelectedIndex = value;
			BackgroundImageAspect = Enum.Parse<Aspect>(AspectList[value]);
		}
	}

	/// <summary>
	/// Gets or sets whether shadow enable or disable
	/// </summary>
	public bool EnableShadow
    {
        get
        {
            return _enableShadow;
        }
        set
        {
            _enableShadow = value;
            OnPropertyChanged("EnableShadow");
        }
    }

    /// <summary>
    /// Gets or sets the TextTransform of the Text of button.
    /// </summary>
    /// <value>The TextTransform.</value>
    public TextTransform TextTransform
    {
        get
        {
            return _textTransform;
        }
        set
        {
            _textTransform = value;
            OnPropertyChanged("TextTransform");
        }
    }

    /// <summary>
    /// Gets or sets the selected index of the TextTransform.
    /// </summary>
    /// <value>The TextTransformSelectedIndex.</value>
    public int TextTransformSelectedIndex
    {
        get { return _textTransformSelectedIndex; }
        set
        {
            _textTransformSelectedIndex = value;
            TextTransform = Enum.Parse<TextTransform>(TextTransFormList[value]);
        }
    }

    public bool EnableAspectList 
    { 
        get { return _enableAspectList; } 
        set
        {
            _enableAspectList = value;
            OnPropertyChanged("EnableAspectList"); 
        }
    }

    #endregion

    #region Property changed method

    /// <summary>
    /// Occurs when property changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;


    public void OnPropertyChanged([CallerMemberName] string? name = null) =>
               PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    #endregion
}
