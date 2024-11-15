using System.ComponentModel;

namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
	/// <summary>
	/// Class contains property and fields for NavigationDrawer.
	/// </summary>
	public class MailInfo : INotifyPropertyChanged
	{
		#region Fields

		string? profileName;
		string? name;
		string? subject;
		string? description;
		DateTime date;
		string? image;
		bool isAttached;
		bool isOpened;
		bool isImportant;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the Name and notifies user when collection value gets changed.
		/// </summary>
		public string? Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
				OnPropertyChanged("Name");
			}
		}

		/// <summary>
		/// Gets or sets the ProfileName and notifies user when collection value gets changed.
		/// </summary>
		public string? ProfileName
		{
			get { return profileName; }
			set
			{
				profileName = value;
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
				return subject;
			}

			set
			{
				subject = value;
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
				return description;
			}

			set
			{
				description = value;
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
				return date;
			}

			set
			{
				date = value;
				OnPropertyChanged("Date");
			}
		}

		/// <summary>
		/// Gets or sets the Image and notifies user when collection value gets changed.
		/// </summary>
		public string? Image
		{
			get
			{
				return image;
			}

			set
			{
				image = value;
				OnPropertyChanged("Image");
			}
		}

		/// <summary>
		/// Gets or sets the IsAttached and notifies user when collection value gets changed.
		/// </summary>
		public bool IsAttached
		{
			get { return isAttached; }
			set
			{
				isAttached = value;
				OnPropertyChanged("IsAttached");
			}
		}

		/// <summary>
		/// Gets or sets the IsImportant and notifies user when collection value gets changed.
		/// </summary>
		public bool IsImportant
		{
			get { return isImportant; }
			set
			{
				isImportant = value;
				OnPropertyChanged("IsImportant");
			}
		}

		/// <summary>
		/// Gets or sets the IsOpened and notifies user when collection value gets changed.
		/// </summary>
		public bool IsOpened
		{
			get { return isOpened; }
			set
			{
				isOpened = value;
				OnPropertyChanged("IsOpened");
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
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}

		#endregion
	}
}