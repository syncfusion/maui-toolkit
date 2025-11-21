using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.Shimmer.SfShimmer
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Chats : SampleView
	{
		readonly IDispatcherTimer _timer;

		public Chats()
		{
			InitializeComponent();
			_timer = Dispatcher.CreateTimer();
			_timer.Interval = TimeSpan.FromMilliseconds(3000);
			_timer.Tick += Timer_Tick;
			_timer.Start();
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);

			if (listView == null || shimmer == null)
			{
				return;
			}
			if (DeviceInfo.Current.Platform == DevicePlatform.WinUI || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
			{
				height = 570;
			}

		}

		private void Timer_Tick(object? sender, EventArgs e)
		{
			shimmer.IsActive = false;
			_timer.Stop();
		}

		public override void OnDisappearing()
		{
			_timer.Stop();
			_timer.Tick -= Timer_Tick;
		}
	}


	public class ViewModel
	{
		private ObservableCollection<ContactInfo> _info;

		public ObservableCollection<ContactInfo> Info
		{
			get { return _info; }
			set { _info = value; }
		}

		public ViewModel()
		{
			_info =
		[
			new ContactInfo() { ContactImage = "peoplecircle17.png", ContactName = "Brenda", Message = "Hey, how's it going? What have you been up to lately?" },
			new ContactInfo() { ContactImage = "peoplecircle9.png", ContactName = "Jennifer", Message = "Do you have any plans for the weekend?" },
			new ContactInfo() { ContactImage = "peoplecircle5.png", ContactName = "Watson", Message = "Have you watched any good movies or TV shows lately?" },
			new ContactInfo() { ContactImage = "peoplecircle23.png", ContactName = "Torrey", Message = "Hi, how are u?" },
			new ContactInfo() { ContactImage = "peoplecircle16.png", ContactName = "Georgia", Message = "How you doin'?" },
			new ContactInfo() { ContactImage = "peoplecircle26.png", ContactName = "Daniel", Message = "Call me at 6" },
			new ContactInfo() { ContactImage = "peoplecircle25.png", ContactName = "Katie", Message = "Shall we go?" },
			new ContactInfo() { ContactImage = "peoplecircle18.png", ContactName = "Peter", Message = "Join the meeting!" },
		];
		}
	}

	public partial class ContactInfo : INotifyPropertyChanged
	{
		#region Fields

		private string? _contactName;
		private string? _contactImage;
		private string? _message;

		#endregion

		#region Constructor

		public ContactInfo()
		{

		}

		#endregion

		#region Public Properties

		public string? ContactName
		{
			get { return _contactName; }
			set
			{
				_contactName = value;
				RaisePropertyChanged("ContactName");
			}
		}

		public string? Message
		{
			get { return _message; }
			set
			{
				_message = value;
				RaisePropertyChanged("Message");
			}
		}

		public string? ContactImage
		{
			get { return _contactImage; }
			set
			{
				if (value != null)
				{
					_contactImage = value;
					RaisePropertyChanged("ContactImage");
				}
			}
		}

		#endregion

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler? PropertyChanged;

		private void RaisePropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion
	}
}
