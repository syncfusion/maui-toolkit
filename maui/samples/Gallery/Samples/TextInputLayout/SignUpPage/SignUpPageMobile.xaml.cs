using core = Syncfusion.Maui.Toolkit.TextInputLayout;

namespace Syncfusion.Maui.ControlsGallery.TextInputLayout.SfTextInputLayout
{
	public partial class SignUpPageMobile : SampleView
	{
		/// <summary>
		/// 
		/// </summary>
		public List<string>? CountryList { get; set; }

		public SignUpPageMobile()
		{
			InitializeComponent();
			this.AddListItems();
			this.Content.BindingContext = this;
		}
		private void SubmitButtonMobile_Clicked(object sender, EventArgs e)
		{
			ValidateText();
		}

		private void AddListItems()
		{
			List<string> countryList = new List<string>();
			countryList.Add("Afghanistan");
			countryList.Add("American Samoa");
			countryList.Add("Andorra");
			countryList.Add("Angola");
			countryList.Add("Argentina");
			countryList.Add("Armenia");
			countryList.Add("Aruba");
			countryList.Add("Ashmore and Cartier Islands");
			countryList.Add("Australia");
			countryList.Add("Austria");
			countryList.Add("Azerbaijan");
			countryList.Add("Bahrain");
			countryList.Add("Bangladesh");
			countryList.Add("Barbados");
			countryList.Add("Bassas da India");
			countryList.Add("Belarus");
			countryList.Add("Belgium");
			countryList.Add("Belize");
			countryList.Add("Benin");
			countryList.Add("Bermuda");
			countryList.Add("Bhutan");
			countryList.Add("Brazil");
			countryList.Add("British Indian Ocean Territory");
			countryList.Add("British Virgin Islands");
			countryList.Add("Brunei");
			countryList.Add("Bulgaria");
			countryList.Add("Burkina Faso");
			countryList.Add("Burma");
			countryList.Add("Burundi");
			countryList.Add("Cambodia");
			countryList.Add("Cameroon");
			countryList.Add("Canada");
			countryList.Add("Cape Verde");
			countryList.Add("Cayman Islands");
			countryList.Add("Central African Republic");
			countryList.Add("Chad");
			countryList.Add("Chile");
			countryList.Add("China");
			countryList.Add("Christmas Island");
			countryList.Add("Clipperton Island");
			countryList.Add("Cocos (Keeling) Islands");
			countryList.Add("Colombia");
			countryList.Add("Comoros");
			countryList.Add("Congo");
			countryList.Add("Congo, Republic of the");
			countryList.Add("Cook Islands");
			countryList.Add("Coral Sea Islands");
			countryList.Add("Costa Rica");
			countryList.Add("Cote d'Ivoire");
			countryList.Add("Croatia");
			countryList.Add("Cuba");
			countryList.Add("Cyprus");
			countryList.Add("Czech Republic");
			countryList.Add("Denmark");
			countryList.Add("Dhekelia");
			countryList.Add("Djibouti");
			countryList.Add("Dominica");
			countryList.Add("Dominican Republic");
			countryList.Add("Ecuador");
			countryList.Add("Egypt");
			countryList.Add("El Salvador");
			countryList.Add("Equatorial Guinea");
			countryList.Add("Eritrea");
			countryList.Add("Estonia");
			countryList.Add("Ethiopia");
			countryList.Add("Europa Island");
			countryList.Add("Falkland Islands");
			countryList.Add("Faroe Islands");
			countryList.Add("Fiji");
			countryList.Add("Finland");
			countryList.Add("France");
			countryList.Add("French Guiana");
			countryList.Add("French Polynesia");
			countryList.Add("French Southern and Antarctic Lands");
			countryList.Add("Gabon");
			countryList.Add("The Gambia");
			countryList.Add("Gaza Strip");
			countryList.Add("Georgia");
			countryList.Add("Germany");
			countryList.Add("Ghana");
			countryList.Add("Grenada");
			countryList.Add("Guatemala");
			countryList.Add("Guernsey");
			countryList.Add("Guinea");
			countryList.Add("Guinea-Bissau");
			countryList.Add("Guyana");
			countryList.Add("Haiti");
			countryList.Add("Heard Island and McDonald Islands");
			countryList.Add("Holy See");
			countryList.Add("Honduras");
			countryList.Add("Hong Kong");
			countryList.Add("Hungary");
			countryList.Add("Iceland");
			countryList.Add("India");
			countryList.Add("Indonesia");
			countryList.Add("Iran");
			countryList.Add("Iraq");
			countryList.Add("Ireland");
			countryList.Add("Isle of Man");
			countryList.Add("Israel");
			countryList.Add("Italy");
			countryList.Add("Jamaica");
			countryList.Add("Jan Mayen");
			countryList.Add("Japan");
			countryList.Add("Jersey");
			countryList.Add("Jordan");
			countryList.Add("Juan de Nova Island");
			countryList.Add("Kazakhstan");
			countryList.Add("Kenya");
			countryList.Add("Kiribati");
			countryList.Add("Korea, North");
			countryList.Add("Korea, South");
			countryList.Add("Kuwait");
			countryList.Add("Kyrgyzstan");
			countryList.Add("Laos");
			countryList.Add("Latvia");
			countryList.Add("Lebanon");
			countryList.Add("Lesotho");
			countryList.Add("Liberia");
			countryList.Add("Libya");
			countryList.Add("Liechtenstein");
			countryList.Add("Lithuania");
			countryList.Add("Luxembourg");
			countryList.Add("Macau");
			countryList.Add("Macedonia");
			countryList.Add("Madagascar");
			countryList.Add("Malawi");
			countryList.Add("Malaysia");
			countryList.Add("Maldives");
			countryList.Add("Mali");
			countryList.Add("Malta");
			countryList.Add("Marshall Islands");
			countryList.Add("Martinique");
			countryList.Add("Mauritania");
			countryList.Add("Mauritius");
			countryList.Add("Mayotte");
			countryList.Add("Mexico");
			countryList.Add("Micronesia");
			countryList.Add("Moldova");
			countryList.Add("Monaco");
			countryList.Add("Mongolia");
			countryList.Add("Montserrat");
			countryList.Add("Morocco");
			countryList.Add("Mozambique");
			countryList.Add("Namibia");
			countryList.Add("Nauru");
			countryList.Add("Navassa Island");
			countryList.Add("Nepal");
			countryList.Add("Netherlands");
			countryList.Add("Netherlands Antilles");
			countryList.Add("New Caledonia");
			countryList.Add("New Zealand");
			countryList.Add("Nicaragua");
			countryList.Add("Niger");
			countryList.Add("Nigeria");
			countryList.Add("Niue");
			countryList.Add("Norfolk Island");
			countryList.Add("Northern Mariana Islands");
			countryList.Add("Norway");
			countryList.Add("Oman");
			countryList.Add("Pakistan");
			countryList.Add("Palau");
			countryList.Add("Panama");
			countryList.Add("Papua New Guinea");
			countryList.Add("Paracel Islands");
			countryList.Add("Paraguay");
			countryList.Add("Peru");
			countryList.Add("Philippines");
			countryList.Add("Pitcairn Islands");
			countryList.Add("Poland");
			countryList.Add("Portugal");
			countryList.Add("Puerto Rico");
			countryList.Add("Qatar");
			countryList.Add("Reunion");
			countryList.Add("Romania");
			countryList.Add("Russia");
			countryList.Add("Rwanda");
			countryList.Add("Saint Helena");
			countryList.Add("Saint Kitts and Nevis");
			countryList.Add("Saint Lucia");
			countryList.Add("Saint Pierre and Miquelon");
			countryList.Add("Saint Vincent");
			countryList.Add("Samoa");
			countryList.Add("San Marino");
			countryList.Add("Saudi Arabia");
			countryList.Add("Singapore");
			countryList.Add("South Africa");
			countryList.Add("Spain");
			countryList.Add("Sri Lanka");
			countryList.Add("Sweden");
			countryList.Add("Switzerland");
			countryList.Add("Syria");
			countryList.Add("Taiwan");
			countryList.Add("Tajikistan");
			countryList.Add("Tanzania");
			countryList.Add("Thailand");
			countryList.Add("The Bahamas");
			countryList.Add("Timor-Leste");
			countryList.Add("Togo");
			countryList.Add("Tokelau");
			countryList.Add("Tonga");
			countryList.Add("Trinidad and Tobago");
			countryList.Add("Tromelin Island");
			countryList.Add("Tunisia");
			countryList.Add("Turkey");
			countryList.Add("Turkmenistan");
			countryList.Add("Turks and Caicos Islands");
			countryList.Add("Tuvalu");
			countryList.Add("Uganda");
			countryList.Add("Ukraine");
			countryList.Add("United Arab Emirates");
			countryList.Add("United Kingdom");
			countryList.Add("United States");
			countryList.Add("Uruguay");
			countryList.Add("Uzbekistan");
			countryList.Add("Vanuatu");
			countryList.Add("Venezuela");
			countryList.Add("Vietnam");
			countryList.Add("Virgin Islands");
			countryList.Add("Wake Island");
			countryList.Add("Wallis and Futuna");
			countryList.Add("West Bank");
			countryList.Add("Western Sahara");
			countryList.Add("Yemen");
			countryList.Add("Zambia");
			countryList.Add("Zimbabwe");
			CountryList = countryList;
		}
		private void ValidateText()
		{
			//this.FieldNullCheck(FirstNameFieldMobile);
			//this.FieldNullCheck(LastNameFieldMobile);
			//this.FieldNullCheck(GenderFieldMobile);
			//this.FieldNullCheck(CountryFieldMobile);
			//this.FieldNullCheck(PhoneNumberFieldMobile);
			//this.FieldNullCheck(PasswordFieldMobile);
			//this.FieldNullCheck(ConfirmPasswordFieldMobile);
			//this.FieldNullCheck(EmailFieldMobile);
			ValidatePhoneNumber();
			ValidateEmailAddress();
			ValidatePasswordField();
		}

		private void ValidatePasswordField()
		{
			if (ConfirmPasswordFieldMobile.IsEnabled && ConfirmPasswordFieldMobile.Text != PasswordFieldMobile.Text)
			{
				ConfirmPasswordFieldMobile.HasError = true;
			}
			else
			{
				ConfirmPasswordFieldMobile.HasError = false;
			}

			if (string.IsNullOrEmpty(PasswordFieldMobile.Text) || PasswordFieldMobile.Text?.Length < 5 || PasswordFieldMobile.Text?.Length > 8)
			{
				PasswordFieldMobile.HasError = true;
			}
			else
			{
				PasswordFieldMobile.HasError = false;
			}
		}

		private void ValidatePhoneNumber()
		{
			if (!double.TryParse(PhoneNumberFieldMobile.Text, out double result))
			{
				PhoneNumberFieldMobile.HasError = true;
			}
			else
			{
				PhoneNumberFieldMobile.HasError = false;
			}
		}

		private void ValidateEmailAddress()
		{
			if (EmailFieldMobile.Text == null || !EmailFieldMobile.Text.Contains("@", StringComparison.Ordinal) || !EmailFieldMobile.Text.Contains(".", StringComparison.Ordinal))
			{
				EmailFieldMobile.HasError = true;
			}
			else
			{
				EmailFieldMobile.HasError = false;
			}
		}

		private void FieldNullCheck(core.SfTextInputLayout inputLayout)
		{
			if (string.IsNullOrEmpty(inputLayout.Text))
			{
				inputLayout.HasError = true;
			}
			else
			{
				inputLayout.HasError = false;
			}
		}


		private void ResetButtonMobile_Clicked(object sender, EventArgs e)
		{
			firstNameEntry.Text = string.Empty;
			lastNameEntry.Text = string.Empty;
			genderComboBox.SelectedItem = null;
			countryAutocomplete.SelectedItem = null;
			phoneEntry.Text = string.Empty;
			emailEntry.Text = string.Empty;
			passwordEntry.Text = string.Empty;
			confirmPasswordEntry.Text = string.Empty;

			FirstNameFieldMobile.HasError = false;
			LastNameFieldMobile.HasError = false;
			GenderFieldMobile.HasError = false;
			CountryFieldMobile.HasError = false;
			PhoneNumberFieldMobile.HasError = false;
			EmailFieldMobile.HasError = false;
			PasswordFieldMobile.HasError = false;
			ConfirmPasswordFieldMobile.HasError = false;
		}

		private void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (sender is Entry entry)
			{
				if (e.NewTextValue.Length <= 8 && e.NewTextValue.Length >= 5)
				{
					ConfirmPasswordFieldMobile.IsEnabled = true;
					ConfirmPasswordFieldMobile.ShowHelperText = true;
				}
				else
				{
					ConfirmPasswordFieldMobile.IsEnabled = false;
					ConfirmPasswordFieldMobile.ShowHelperText = false;
				}
			}
		}
	}
}