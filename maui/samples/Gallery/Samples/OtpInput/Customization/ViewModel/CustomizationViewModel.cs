using System.ComponentModel;
using Syncfusion.Maui.Toolkit.OtpInput;

namespace Syncfusion.Maui.ControlsGallery.OtpInput.OtpInput
{
	public class CustomizationViewModel : INotifyPropertyChanged
	{
		public string[] StylingModeList { get; set; } = { "Filled", "Outlined", "Underlined" };

		public string[] InputStateList { get; set; } = { "Default", "Success", "Error", "Warning" };


		#region Fields

		double _length = 4;
		OtpInputStyle _stylingMode = OtpInputStyle.Underlined;
		string _separator = string.Empty;
		OtpInputState _inputState = OtpInputState.Default;
		int _stylingModeIndex = 1;
		int _inputStateIndex = 0;

		#endregion

		#region Property

		public double Length
		{
			get { return _length; }
			set { _length = value; OnPropertyChanged(nameof(Length)); }
		}

		
		public OtpInputStyle StylingMode
		{
			get { return _stylingMode; }
			set { _stylingMode = value; OnPropertyChanged(nameof(StylingMode)); }
		}

		public OtpInputState InputState
		{
			get { return _inputState; }
			set { _inputState = value; OnPropertyChanged(nameof(InputState)); }
		}

		public string Separator
		{
			get { return _separator; }
			set { _separator = value; OnPropertyChanged(nameof(Separator)); }
		}

		public int StylingModeIndex
		{
			get { return _stylingModeIndex; }
			set
			{
				_stylingModeIndex = value;
				if (value >= 0)
				{
					StylingMode = Enum.Parse<OtpInputStyle>(StylingModeList[value]);
				}
			}
		}

		public int InputStateIndex
		{
			get { return _inputStateIndex; }
			set
			{
				_inputStateIndex = value;
				if (value >= 0)
				{
					InputState = Enum.Parse<OtpInputState>(InputStateList[value]);
				}
			}
		}

		#endregion

		#region OnPropertyChanged

		public event PropertyChangedEventHandler? PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
