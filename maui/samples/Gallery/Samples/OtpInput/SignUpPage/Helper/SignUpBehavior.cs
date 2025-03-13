using System.Text.RegularExpressions;
using Syncfusion.Maui.ControlsGallery.CustomView;
using Syncfusion.Maui.Toolkit.OtpInput;
using Syncfusion.Maui.Toolkit.Popup;
using Syncfusion.Maui.Toolkit.TextInputLayout;

namespace Syncfusion.Maui.ControlsGallery.OtpInput.OtpInput;

public class SignUpBehavior:Behavior<SampleView>
{
	Grid? _signUpView;
	Grid? _confirmOtpView;
	Button? _register;
	Button? _confirmOtp;
	SfEffectsViewAdv? _confirmOtpBackButton;
	SfOtpInput? _otpVerify;
	SfEntry? _nameEntry;
	SfEntry? _phoneEntry;
	SfTextInputLayout? _nameInputLayout;
	SfTextInputLayout? _phoneInputLayout;
	SignUpViewModel? _signUpViewModel;
	SampleView? _signUp;
	SfPopup? _otpPopUp;
	SfPopup? _otpVerifyPopUp;
	SfPopup? _failedPopUp;
	Span? _resendOTP;
	bool? _nameVerification;
	bool? _phoneVerification;


	/// <summary>
	/// You can override this method to subscribe to AssociatedObject events and initialize properties.
	/// </summary>
	/// <param name="bindable">SampleView type parameter named as bindable.</param>
	protected override void OnAttachedTo(SampleView bindable)
	{
		_signUp = bindable;	
		_signUpView = bindable.FindByName<Grid>("SignUp");
		_confirmOtpView = bindable.FindByName<Grid>("ConfirmOTP");
		_register = bindable.FindByName<Button>("Register");
		_confirmOtp = bindable.FindByName<Button>("ConfirmOTPButton");
		_nameEntry = bindable.FindByName<SfEntry>("NameEntry");
		_phoneEntry = bindable.FindByName<SfEntry>("PhoneEntry");
		_nameInputLayout = bindable.FindByName<SfTextInputLayout>("NameTextInput");
		_phoneInputLayout = bindable.FindByName<SfTextInputLayout>("PhoneNumberTextInput");
		_otpPopUp = bindable.FindByName<SfPopup>("OtpPopup");
		_otpVerifyPopUp = bindable.FindByName<SfPopup>("OtpVerficationPopup");
		_failedPopUp = bindable.FindByName<SfPopup>("FailedPopup");
		_otpVerify = bindable.FindByName<SfOtpInput>("OTPVerify");
		_resendOTP = bindable.FindByName<Span>("ResendOTP");
		_confirmOtpBackButton = bindable.FindByName<SfEffectsViewAdv>("ConfirmOtpBackButton");
		_register.Clicked += Register_Clicked;
		_confirmOtp.Clicked += ConfirmOtp_Clicked;

		_nameEntry.TextChanged += NameEntry_TextChanged;
		_phoneEntry.TextChanged += PhoneEntry_TextChanged;

		TapGestureRecognizer confirmOtpBackButtonTapped = new TapGestureRecognizer();
		confirmOtpBackButtonTapped.Tapped += ConfirmOtpBackButtonTapped;
		_confirmOtpBackButton.GestureRecognizers.Add(confirmOtpBackButtonTapped);

		TapGestureRecognizer resentOTP = new TapGestureRecognizer();
		resentOTP.Tapped += ResentOTPTapped;
		_resendOTP.GestureRecognizers.Add(resentOTP);

		base.OnAttachedTo(bindable);
	}

	void ResentOTPTapped(object? sender, TappedEventArgs e)
	{
		_signUpViewModel = _signUp?.BindingContext as SignUpViewModel;
		if (_signUpViewModel is not null && _otpPopUp is not null && _otpVerify is not null)
		{
			_signUpViewModel.OTP = new Random().Next(10000, 99999).ToString();
			_otpVerify.Value = string.Empty;
			_otpVerify.InputState = OtpInputState.Default;
			_otpVerify.AutoFocus = false;
			_otpPopUp.IsOpen = true;
		}
	}

	void PhoneEntry_TextChanged(object? sender, TextChangedEventArgs e)
	{
		if (_phoneEntry is not null && _phoneInputLayout is not null)
		{
			bool isValid = Regex.IsMatch(_phoneEntry.Text, @"^\d+$");
			if (!isValid)
			{
				if (!string.IsNullOrEmpty(_phoneEntry.Text))
				{
					_phoneVerification = false;
					_phoneInputLayout.HasError = true;
					_phoneInputLayout.ErrorText = "Enter a 10-digit mobile number";
					_phoneInputLayout.ErrorLabelStyle.TextColor = Colors.Red;
				}
			}
			else
			{
				if (_phoneEntry.Text.Length == 10)
				{
					_phoneVerification = true;
					_phoneInputLayout.HasError = false;
					_phoneInputLayout.ErrorText = string.Empty;
				}
				else
				{
					_phoneVerification = false;
					_phoneInputLayout.HasError = true;
					_phoneInputLayout.ErrorText = "Enter a 10-digit mobile number";
					_phoneInputLayout.ErrorLabelStyle.TextColor = Colors.Red;
				}
			}
		}
	}

	void NameEntry_TextChanged(object? sender, TextChangedEventArgs e)
	{
		if(_nameEntry is not null && _nameInputLayout is not null)
		{
			bool isValid = Regex.IsMatch(_nameEntry.Text, @"^[a-zA-Z]+(?:\s+[a-zA-Z]+)*$");
			if(!isValid)
			{
				if(!string.IsNullOrEmpty(_nameEntry.Text))
				{
					_nameVerification = false;
					_nameInputLayout.HasError = true;
					_nameInputLayout.ErrorText = "Invalid input";
					_nameInputLayout.ErrorLabelStyle.TextColor = Colors.Red;
				}
				
			}
			else
			{
				_nameVerification = true;
				_nameInputLayout.HasError = false;
				_nameInputLayout.ErrorText = string.Empty;
			}
		}
	}

	void ConfirmOtpBackButtonTapped(object? sender, TappedEventArgs e)
	{
		if (_signUpViewModel is not null && _otpVerify is not null)
		{
			_signUpViewModel.OTP = string.Empty;
			_otpVerify.Value = string.Empty;
		}
		if(_confirmOtpView is not null && _signUpView is not null)
		{
			_signUpView.IsVisible = true;
			_confirmOtpView.IsVisible = false;
		}
	}

	void ConfirmOtp_Clicked(object? sender, EventArgs e)
	{
		_signUpViewModel = _signUp?.BindingContext as SignUpViewModel;
		if (_signUpViewModel is not null)
		{
			string otp = _signUpViewModel.OTP;
			if( _otpVerify is not null && _failedPopUp is not null && _otpVerifyPopUp is not null)
			{
				if (otp != null  && otp.Equals(_otpVerify.Value, StringComparison.Ordinal))
				{
					_otpVerify.InputState = OtpInputState.Success;
					_otpVerifyPopUp.IsOpen = true;
				}
				else
				{
					_failedPopUp.IsOpen = true;
					_otpVerify.InputState = OtpInputState.Error;
				}
				
			}
		}
	}

	void Register_Clicked(object? sender, EventArgs e)
	{
		_signUpViewModel = _signUp?.BindingContext as SignUpViewModel;
		if (_confirmOtpView is not null && _signUpView is not null && _otpPopUp is not null && _signUpViewModel is not null)
		{
			if (_nameVerification==true && _phoneVerification==true)
			{
				_signUpViewModel.OTP = new Random().Next(10000, 99999).ToString();
				_signUpView.IsVisible = false;
				_confirmOtpView.IsVisible = true;
				_otpPopUp.IsOpen = true;
			}
			else
			{
				if (_nameEntry is not null && _nameInputLayout is not null && string.IsNullOrEmpty(_nameEntry.Text))
				{
					_nameVerification = false;
					_nameInputLayout.HasError = true;
					_nameInputLayout.ErrorText = "Invalid input";
					_nameInputLayout.ErrorLabelStyle.TextColor = Colors.Red;
				}

				if (_phoneEntry is not null && _phoneInputLayout is not null && string.IsNullOrEmpty(_phoneEntry.Text))
				{
					_phoneVerification = false;
					_phoneInputLayout.HasError = true;
					_phoneInputLayout.ErrorText = "Invalid input";
					_phoneInputLayout.ErrorLabelStyle.TextColor = Colors.Red;
				}
			}
		}
	}
}
