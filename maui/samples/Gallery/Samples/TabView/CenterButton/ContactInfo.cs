using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.ControlsGallery.Samples.TabView.CenterButton
{
	public class ContactsInfo : INotifyPropertyChanged
	{
		#region Fields

		private string? contactName;
		private string? contactNo;
		private string? image;
		private string? contactType;
		private DateTime messagingtime;
		#endregion

		#region Constructor

		public ContactsInfo()
		{

		}

		#endregion

		#region Public Properties

		public string? ContactName
		{
			get { return contactName; }
			set
			{
				contactName = value;
				RaisePropertyChanged("ContactName");
			}
		}

		public string? ContactNumber
		{
			get { return contactNo; }
			set
			{
				contactNo = value;
				RaisePropertyChanged("ContactNumber");
			}
		}

		public string? ContactReadType
		{
			get { return contactType; }
			set
			{
				contactType = value;
				RaisePropertyChanged("ContactReadType");
			}
		}

		public string? ContactImage
		{
			get { return image; }
			set
			{
				image = value;
				this.RaisePropertyChanged("ContactImage");
			}
		}

		public DateTime MessagingTime
		{
			get { return this.messagingtime; }
			set
			{
				this.messagingtime = value;
				this.RaisePropertyChanged("MessagingTime");
			}
		}

		public string? Message { get; set; }

		#endregion

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler? PropertyChanged;

		private void RaisePropertyChanged(String name)
		{
			if (PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(name));
		}

		#endregion
	}
}
