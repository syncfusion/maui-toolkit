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
			AddListItems();
			Content.BindingContext = this;
		}
		private void SubmitButtonMobile_Clicked(object sender, EventArgs e)
		{
			ValidateText();
		}

		private void AddListItems()
		{
			List<string> countryList =
			[
				"Afghanistan",
				"American Samoa",
				"Andorra",
				"Angola",
				"Argentina",
				"Armenia",
				"Aruba",
				"Ashmore and Cartier Islands",
				"Australia",
				"Austria",
				"Azerbaijan",
				"Bahrain",
				"Bangladesh",
				"Barbados",
				"Bassas da India",
				"Belarus",
				"Belgium",
				"Belize",
				"Benin",
				"Bermuda",
				"Bhutan",
				"Brazil",
				"British Indian Ocean Territory",
				"British Virgin Islands",
				"Brunei",
				"Bulgaria",
				"Burkina Faso",
				"Burma",
				"Burundi",
				"Cambodia",
				"Cameroon",
				"Canada",
				"Cape Verde",
				"Cayman Islands",
				"Central African Republic",
				"Chad",
				"Chile",
				"China",
				"Christmas Island",
				"Clipperton Island",
				"Cocos (Keeling) Islands",
				"Colombia",
				"Comoros",
				"Congo",
				"Congo, Republic of the",
				"Cook Islands",
				"Coral Sea Islands",
				"Costa Rica",
				"Cote d'Ivoire",
				"Croatia",
				"Cuba",
				"Cyprus",
				"Czech Republic",
				"Denmark",
				"Dhekelia",
				"Djibouti",
				"Dominica",
				"Dominican Republic",
				"Ecuador",
				"Egypt",
				"El Salvador",
				"Equatorial Guinea",
				"Eritrea",
				"Estonia",
				"Ethiopia",
				"Europa Island",
				"Falkland Islands",
				"Faroe Islands",
				"Fiji",
				"Finland",
				"France",
				"French Guiana",
				"French Polynesia",
				"French Southern and Antarctic Lands",
				"Gabon",
				"The Gambia",
				"Gaza Strip",
				"Georgia",
				"Germany",
				"Ghana",
				"Grenada",
				"Guatemala",
				"Guernsey",
				"Guinea",
				"Guinea-Bissau",
				"Guyana",
				"Haiti",
				"Heard Island and McDonald Islands",
				"Holy See",
				"Honduras",
				"Hong Kong",
				"Hungary",
				"Iceland",
				"India",
				"Indonesia",
				"Iran",
				"Iraq",
				"Ireland",
				"Isle of Man",
				"Israel",
				"Italy",
				"Jamaica",
				"Jan Mayen",
				"Japan",
				"Jersey",
				"Jordan",
				"Juan de Nova Island",
				"Kazakhstan",
				"Kenya",
				"Kiribati",
				"Korea, North",
				"Korea, South",
				"Kuwait",
				"Kyrgyzstan",
				"Laos",
				"Latvia",
				"Lebanon",
				"Lesotho",
				"Liberia",
				"Libya",
				"Liechtenstein",
				"Lithuania",
				"Luxembourg",
				"Macau",
				"Macedonia",
				"Madagascar",
				"Malawi",
				"Malaysia",
				"Maldives",
				"Mali",
				"Malta",
				"Marshall Islands",
				"Martinique",
				"Mauritania",
				"Mauritius",
				"Mayotte",
				"Mexico",
				"Micronesia",
				"Moldova",
				"Monaco",
				"Mongolia",
				"Montserrat",
				"Morocco",
				"Mozambique",
				"Namibia",
				"Nauru",
				"Navassa Island",
				"Nepal",
				"Netherlands",
				"Netherlands Antilles",
				"New Caledonia",
				"New Zealand",
				"Nicaragua",
				"Niger",
				"Nigeria",
				"Niue",
				"Norfolk Island",
				"Northern Mariana Islands",
				"Norway",
				"Oman",
				"Pakistan",
				"Palau",
				"Panama",
				"Papua New Guinea",
				"Paracel Islands",
				"Paraguay",
				"Peru",
				"Philippines",
				"Pitcairn Islands",
				"Poland",
				"Portugal",
				"Puerto Rico",
				"Qatar",
				"Reunion",
				"Romania",
				"Russia",
				"Rwanda",
				"Saint Helena",
				"Saint Kitts and Nevis",
				"Saint Lucia",
				"Saint Pierre and Miquelon",
				"Saint Vincent",
				"Samoa",
				"San Marino",
				"Saudi Arabia",
				"Singapore",
				"South Africa",
				"Spain",
				"Sri Lanka",
				"Sweden",
				"Switzerland",
				"Syria",
				"Taiwan",
				"Tajikistan",
				"Tanzania",
				"Thailand",
				"The Bahamas",
				"Timor-Leste",
				"Togo",
				"Tokelau",
				"Tonga",
				"Trinidad and Tobago",
				"Tromelin Island",
				"Tunisia",
				"Turkey",
				"Turkmenistan",
				"Turks and Caicos Islands",
				"Tuvalu",
				"Uganda",
				"Ukraine",
				"United Arab Emirates",
				"United Kingdom",
				"United States",
				"Uruguay",
				"Uzbekistan",
				"Vanuatu",
				"Venezuela",
				"Vietnam",
				"Virgin Islands",
				"Wake Island",
				"Wallis and Futuna",
				"West Bank",
				"Western Sahara",
				"Yemen",
				"Zambia",
				"Zimbabwe",
			];
			CountryList = countryList;
		}
		private void ValidateText()
		{
			SignUpPageMobile.FieldNullCheck(FirstNameFieldMobile);
			SignUpPageMobile.FieldNullCheck(LastNameFieldMobile);
			SignUpPageMobile.FieldNullCheck(GenderFieldMobile);
			SignUpPageMobile.FieldNullCheck(CountryFieldMobile);
			SignUpPageMobile.FieldNullCheck(PhoneNumberFieldMobile);
			SignUpPageMobile.FieldNullCheck(PasswordFieldMobile);
			SignUpPageMobile.FieldNullCheck(ConfirmPasswordFieldMobile);
			SignUpPageMobile.FieldNullCheck(EmailFieldMobile);
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
			if (!double.TryParse(PhoneNumberFieldMobile.Text, out _))
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
			if (EmailFieldMobile.Text == null || !EmailFieldMobile.Text.Contains('@', StringComparison.Ordinal) || !EmailFieldMobile.Text.Contains('.', StringComparison.Ordinal))
			{
				EmailFieldMobile.HasError = true;
			}
			else
			{
				EmailFieldMobile.HasError = false;
			}
		}

		private static void FieldNullCheck(core.SfTextInputLayout inputLayout)
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
			if (sender is Entry)
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