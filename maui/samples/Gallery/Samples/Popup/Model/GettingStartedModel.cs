using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.ControlsGallery.Popup.SfPopup
{
    public class SimpleModel : INotifyPropertyChanged
    {
        #region Fields
        
        string? _userName;       
        string? _mailId;
        string? _image;
        bool _isSelected;
        
        #endregion

        #region Properties
        
        public string? UserName
        {
            get { return _userName; }
            set
            {
				if (value is not null)
				{
					_userName = value;
					OnPropertyChanged("UserName");
				}
            }
        }

        public string? MailId
        {
            get { return _mailId; }
            set
            {
				if (value is not null)
				{
					_mailId = value;
					OnPropertyChanged("MailId");
				}
            }
        }

        public string? Image
        {
            get { return _image; }
            set
            {
				if (value is not null)
				{
					_image = value;
					OnPropertyChanged("Image");
				}
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

		#endregion

		#region Constructor

		public SimpleModel()
		{

		}

		#endregion

		#region Interface Member

		public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string name)
        {
			if (PropertyChanged is not null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

        #endregion
    }

    public class ConfirmationModel : INotifyPropertyChanged 
    {
        #region Fields

        string? _ringtone;
        bool _selectedRingtone;

		#endregion

		#region Properties

		public string? Ringtone
        {
            get { return _ringtone; }
            set
            {
				if (value is not null)
				{
					_ringtone = value;
					OnPropertyChanged("Ringtone");
				}
            }
        }

        public bool SelectedRingtone
        {
            get { return _selectedRingtone; }
            set
            {
                _selectedRingtone = value;
                OnPropertyChanged("SelectedRingtone");
            }
        }

        #endregion

        #region Constructor

        public ConfirmationModel()
        {

        }
        #endregion

        #region Interface Member

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string name)
        {
			if (PropertyChanged is not null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

        #endregion
    }

    public class FullScreenModel : INotifyPropertyChanged
    {
        #region Fields
        string? _userName;
        string? _email;
        string? _password;
        string? _rePassword;
        #endregion

        #region Properties
        [Display(Prompt = "Pick your username")]
        public string? UserName
        {
            get { return _userName; }
            set
            {
				if (value is not null)
				{
					_userName = value;
					OnPropertyChanged("UserName");
				}
            }
        }

        [Display(Prompt = "Enter your email address")]
        public string? Email
        {
            get { return _email; }
            set
            {
				if (value is not null)
				{
					_email = value;
					OnPropertyChanged("Email");
				}
            }
        }
        
        [Display(Prompt = "Enter your password", Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password
        {
            get { return _password; }
            set
            {
				if (value is not null)
				{
					_password = value;
					OnPropertyChanged("Password");
				}
            }
        }
        
        [Display(Prompt = "Re-enter your password", Name = "RePassword")]
        [DataType(DataType.Password)]
        public string? RePassword
        {
            get { return _rePassword; }
            set
            {
				if (value is not null)
				{
					_rePassword = value;
					OnPropertyChanged("RePassword");
				}
            }
        }
        #endregion

        #region Constructor
        public FullScreenModel() 
        {
            UserName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            RePassword = string.Empty;
        }
        #endregion

        #region Interface Member
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string name) 
        {
			if (PropertyChanged is not null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
        }
        #endregion
    }
}
