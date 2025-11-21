using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	/// <summary>
	/// Class contains property and fields for CollectionViewPullToRefresh.
	/// </summary>
	public partial class InboxInfo : INotifyPropertyChanged
	{
		#region Fields

		string? _profileName;
		string? _name;
		string? _subject;
		string? _description;
		DateTime _date;
		bool _isAttached;
		bool _isOpened;
		bool _isImportant;
		Color? _avatarBackgroundColor;
		Color? _avatarTextColor;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the Name and notifies user when collection value gets changed.
		/// </summary>
		public string? Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		/// <summary>
		/// Gets or sets the ProfileName and notifies user when collection value gets changed.
		/// </summary>
		public string? ProfileName
		{
			get { return _profileName; }
			set
			{
				_profileName = value;
				OnPropertyChanged("ProfileName");
			}
		}

		/// <summary>
		/// Gets or sets the Subject and notifies user when collection value gets changed.
		/// </summary>
		public string? Subject
		{
			get
			{
				return _subject;
			}

			set
			{
				_subject = value;
				OnPropertyChanged("Subject");
			}
		}

		/// <summary>
		/// Gets or sets the Description and notifies user when collection value gets changed.
		/// </summary>
		public string? Description
		{
			get
			{
				return _description;
			}

			set
			{
				_description = value;
				OnPropertyChanged("Description");
			}
		}

		/// <summary>
		/// Gets or sets the Date and notifies user when collection value gets changed.
		/// </summary>
		public DateTime Date
		{
			get
			{
				return _date;
			}

			set
			{
				_date = value;
				OnPropertyChanged("Date");
			}
		}

		/// <summary>
		/// Gets or sets the IsAttached and notifies user when collection value gets changed.
		/// </summary>
		public bool IsAttached
		{
			get { return _isAttached; }
			set
			{
				_isAttached = value;
				OnPropertyChanged("IsAttached");
			}
		}

		/// <summary>
		/// Gets or sets the IsImportant and notifies user when collection value gets changed.
		/// </summary>
		public bool IsImportant
		{
			get { return _isImportant; }
			set
			{
				_isImportant = value;
				OnPropertyChanged("IsImportant");
			}
		}

		/// <summary>
		/// Gets or sets the IsOpened and notifies user when collection value gets changed.
		/// </summary>
		public bool IsOpened
		{
			get { return _isOpened; }
			set
			{
				_isOpened = value;
				OnPropertyChanged("IsOpened");
			}
		}

		/// <summary>
		/// Gets or sets the AvatarBackgroundColor and notifies user when collection value gets changed.
		/// </summary>
		public Color? AvatarBackgroundColor
		{
			get { return _avatarBackgroundColor; }
			set
			{
				_avatarBackgroundColor = value;
				OnPropertyChanged("AvatarBackgroundColor");
			}
		}

		/// <summary>
		/// Gets or sets the AvatarTextColor and notifies user when collection value gets changed.
		/// </summary>
		public Color? AvatarTextColor
		{
			get { return _avatarTextColor; }
			set
			{
				_avatarTextColor = value;
				OnPropertyChanged("AvatarTextColor");
			}
		}
		#endregion

		#region Interface Member

		/// <summary>
		/// Represents the method that will handle the <see cref="System.ComponentModel.INotifyPropertyChanged.PropertyChanged"></see> event raised when a property is changed on a component
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// Triggers when Items Collections Changed.
		/// </summary>
		/// <param name="name">string type parameter represent propertyName as name</param>
		public void OnPropertyChanged(string name)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		#endregion
	}
}
