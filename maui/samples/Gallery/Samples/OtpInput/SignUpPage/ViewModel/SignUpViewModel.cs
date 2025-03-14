using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace Syncfusion.Maui.ControlsGallery.OtpInput.OtpInput;

public class SignUpViewModel: INotifyPropertyChanged
{
	#region Fields

	string _name = string.Empty;

	string _phoneNumber = string.Empty;

	string _otp = string.Empty;

	bool _isPopUpOpen = false;

	public ICommand CopyOtpCommand { get; }

	#endregion

	public SignUpViewModel()
	{
		CopyOtpCommand = new Command(OTPCopiedTapped);
	}

	#region Properties
	public string Name
	{
		get => _name;
		set
		{
			_name = value;
			OnPropertyChanged(nameof(Name));
		}
	}

	public bool IsPopUpOpen
	{
		get => _isPopUpOpen;
		set
		{
			_isPopUpOpen = value;
			OnPropertyChanged(nameof(IsPopUpOpen));
		}
	}

	public string OTP
	{
		get => _otp;
		set
		{
			_otp = value;
			OnPropertyChanged(nameof(OTP));
		}
	}

	public string PhoneNumber
	{
		get => _phoneNumber;
		set
		{
			_phoneNumber = value;
			OnPropertyChanged(nameof(PhoneNumber));
		}
	}

	#endregion

	public event PropertyChangedEventHandler? PropertyChanged;

	protected void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	async void OTPCopiedTapped()
	{
		await Clipboard.SetTextAsync(OTP);
		IsPopUpOpen = false;
	}
	
}

public class FirstFourNumberConverter : IValueConverter
{
	public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo culture)
	{
		string? phoneNumber = value as string;
		return phoneNumber?.Substring(0, Math.Min(4, phoneNumber.Length));
	}

	public object? ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
	{
		throw new NotImplementedException();
	}
}
