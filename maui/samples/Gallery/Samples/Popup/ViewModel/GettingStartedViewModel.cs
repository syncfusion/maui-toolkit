#if ANDROID
using Android.OS;
#endif
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Maui.ControlsGallery.Popup.SfPopup
{
    public class GettingStartedViewModel : INotifyPropertyChanged
    {
        #region Fields
        
        ObservableCollection<SimpleModel> _userDetails;
        ObservableCollection<ConfirmationModel> _ringtoneList;
        string _toastText;
        bool _isToastVisible;
        FullScreenModel _fullScreenModel;

        #endregion

        #region Properties
        public bool IsBlurButtonVisible { get; set; }
        public FullScreenModel FullScreenModel
        {
            get { return _fullScreenModel; }
            set
            {
                _fullScreenModel = value;
                OnPropertyChanged("FullScreenModel");
            }
        }
        public ObservableCollection<SimpleModel> UserDetails
        {
            get { return _userDetails; }
            set
            {
                _userDetails = value;
                OnPropertyChanged("UserDetails");
            }
        }
        public ObservableCollection<ConfirmationModel> RingtoneList
        {
            get { return _ringtoneList; }
            set
            {
                _ringtoneList = value;
                OnPropertyChanged("RingtoneList");
            }
        }
        public string ToastText
        {
            get { return _toastText; }
            set
            {
                _toastText = value;
                OnPropertyChanged("ToastText");
            }
        }
        public bool IsToastVisible
        {
            get { return _isToastVisible; }
            set
            {
                _isToastVisible = value;
                OnPropertyChanged("IsToastVisible");
            }
        }
        public ICommand ShowPopup { get; set; }     
        public ICommand FullScreenCommand { get; set; }
        public ICommand NotificationCommand { get; set; }
        public ICommand AcceptCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand UploadCommand { get; set; }
        public ICommand CopyCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand DeclineCommand { get; set; }
        public ICommand SelectionCommand { get; set; }
        public ICommand RingtoneSelectionCommand { get; set; }
        public ICommand SignUpCommand { get; set; }

        #endregion

        #region Constructor

        public GettingStartedViewModel()
        {
            UserDetails =
			[
				new SimpleModel() { UserName = "Michael", MailId = "michael40@sample.com", Image="sebastian.png", IsSelected = true},
                new SimpleModel() { UserName = "Laura", MailId = "laura13@jourrapide.com", Image ="clara.png", IsSelected = false},
                new SimpleModel() { UserName = "Tamara", MailId = "tamara17@rpy.com", Image = "lita.png", IsSelected = false},
            ];
            
            RingtoneList = [];
            for (int i=0; i< Ringtones.Length; i++)
            {
                var ringtone = new ConfirmationModel()
                {
                    Ringtone = Ringtones[i],
                };

				if (i == 0)
				{
					ringtone.SelectedRingtone = true;
				}
				else
				{
					ringtone.SelectedRingtone = false;
				}

                RingtoneList.Add(ringtone);
            }

            ShowPopup = new Command<Toolkit.Popup.SfPopup>(OnPopupOpened);
            FullScreenCommand = new Command<Toolkit.Popup.SfPopup>(OnFullScreen);
            NotificationCommand = new Command<Toolkit.Popup.SfPopup>(OnNotificationAction);           
            AcceptCommand = new Command<Toolkit.Popup.SfPopup>(OnAcceptAction);
            DeclineCommand = new Command<Toolkit.Popup.SfPopup>(OnDeclineAction);
            ShareCommand = new Command<Toolkit.Popup.SfPopup>(OnShare);
            UploadCommand = new Command<Toolkit.Popup.SfPopup>(OnUpload);
            CopyCommand = new Command<Toolkit.Popup.SfPopup>(OnCopy);
            PrintCommand = new Command<Toolkit.Popup.SfPopup>(OnPrint);
            DeleteCommand = new Command<Toolkit.Popup.SfPopup>(OnDelete);
            SignUpCommand = new Command<Toolkit.Popup.SfPopup>(OnSignUp);
            SelectionCommand = new Command(OnSelectionChanged);
            RingtoneSelectionCommand = new Command(OnRingtoneSelectionCommand);
            FullScreenModel = new FullScreenModel();
            ToastText = string.Empty;
            IsToastVisible = false;
            IsBlurButtonVisible = true;
#if ANDROID
            int androidVersion = (int)Build.VERSION.SdkInt;
            if(androidVersion < 31)
            {
                IsBlurButtonVisible = false;
            }
#endif
        }

        #endregion

        #region Private Methods

        void OnPopupOpened(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {
            popup.Show();
			//popup.IsOpen = true;
        }

		void OnFullScreen(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {
            popup.Show(true);
			//popup.IsOpen = false;
			//popup.IsFullScreen = true;
        }

        void OnNotificationAction(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {
#if ANDROID || IOS
            popup.StartY = 0;
#elif WINDOWS
            popup.RelativePosition = Syncfusion.Maui.Toolkit.Popup.PopupRelativePosition.AlignBottomRight;
#else
            popup.RelativePosition = Syncfusion.Maui.Toolkit.Popup.PopupRelativePosition.AlignTopRight;
#endif
			popup.Show();
        }

        void OnAcceptAction(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {
            popup.IsOpen = false;
        }
      
        void OnDeclineAction(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {
            popup.IsOpen = false;
        }

        void OnRingtoneSelectionCommand(object obj)
        {
			foreach (var item in RingtoneList)
			{
				item.SelectedRingtone = false;
			}
            (obj as ConfirmationModel)!.SelectedRingtone = true;
        }

        void OnSelectionChanged(object obj)
        {
			foreach (var item in UserDetails)
			{
				item.IsSelected = false;
			}
            (obj as SimpleModel)!.IsSelected = true;
        }        

        void OnShare(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {           
            ToastText = "File shared";
            OnFileAction(popup);
        }

        void OnUpload(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {                     
            ToastText = "File uploaded";
            OnFileAction(popup);
        }

        void OnCopy(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {                      
            ToastText = "File copied";
            OnFileAction(popup);
        }

        void OnPrint(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {                       
            ToastText = "File printed";
            OnFileAction(popup);
        }

        void OnDelete(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {          
            ToastText = "File deleted";
            OnFileAction(popup);
        }

        void OnSignUp(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {
			FullScreenModel.UserName = string.Empty;
			FullScreenModel.Email = string.Empty;
			FullScreenModel.Password = string.Empty;
			FullScreenModel.RePassword = string.Empty;
			popup.IsOpen = false;
        }

        async void OnFileAction(Syncfusion.Maui.Toolkit.Popup.SfPopup popup)
        {
            popup.IsOpen = false;
            IsToastVisible = true;
            await Task.Delay(1000);         
            IsToastVisible = false;
        }

        #endregion

        #region Collection

        string[] Ringtones = new string[]
        {
            "Asteroid",
            "Atomic Bell",
            "Beep Once",
            "Beep-Beep",
            "Chime Time",
            "Comet",
            "Cosmos",
            "Finding Galaxy",
            "Galaxy Bells",
            "Homecoming",
            "Moon Discovery",
            "Neptune",
        };

#endregion

		#region Interface Member

        public event PropertyChangedEventHandler PropertyChanged;

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
