using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
	/// <summary>
	/// A class used to assign collection values for a Model properties
	/// </summary>
	public class MailInfoRepository
	{
		#region Fields

		Random random = new Random();

		#endregion

		#region Get mail info

		/// <summary>
		/// Used to assign the Collection values to Model Properties.
		/// </summary>
		/// <returns>Added MailInfos items</returns>
		internal ObservableCollection<MailInfo> GetMailInfo()
		{
			var empInfo = new ObservableCollection<MailInfo>();
			int k = 0;
			for (int i = 0; i < Subject.Count(); i++)
			{
				if (k > 5)
				{
					k = 0;
				}
				var record = new MailInfo()
				{
					ProfileName = ProfileList[i],
					Name = NameList[i],
					Subject = Subject[i],
					Date = DateTime.Today.AddDays(i * -3),
					Description = Descriptions[i],
					Image = Images[k],
					IsAttached = Attachments[i],
					IsImportant = Importants[i],
					IsOpened = Opens[i],
				};
				empInfo.Add(record);
				k++;
			}
			return empInfo;
		}

		/// <summary>
		/// Used to assign the Collection values to Model Properties while refreshing.
		/// </summary>
		/// <param name="itemsCount">Number of items to be added.</param>
		/// <returns>Added mailInfos items</returns>
		internal ObservableCollection<MailInfo> AddRefreshItems(int itemsCount)
		{
			var empInfo = new ObservableCollection<MailInfo>();
			int k = 0;
			for (int i = 0; i < Subject.Count(); i++)
			{
				if (i % itemsCount == 0)
				{
					var j = random.Next(Subject.Count());

					if (k > 5)
					{
						k = 0;
					}
					var record = new MailInfo()
					{
						ProfileName = ProfileList[j],
						Name = NameList[j],
						Subject = Subject[j],
						Date = DateTime.Today.AddDays(i * -3),
						Description = Descriptions[j],
						Image = Images[k],
						IsAttached = Attachments[j],
						IsImportant = Importants[j],
						IsOpened = Opens[j],
					};
					empInfo.Add(record);
					k++;
				}
			}

			return empInfo;
		}

		#endregion

		#region Employee Info

		internal string[] ProfileList = new string[]
		{
		"M",
		"MV",
		"MV",
		"T",
		"M",
		"LI",
		"M",
		"M",
		"SO",
		"OT",
		"MO",
		"MA",
		"BT",
		"M",
		"M",
		};

		internal string[] NameList = new string[]
		{
		"Microsoft",
		"Microsoft Viva",
		"Microsoft Viva",
		"Twitter",
		"Microsoft",
		"LinkedIn",
		"Microsoft",
		"Microsoft",
		"Stack Overflow",
		"Outlook Team",
		"Microsoft Outlook",
		"My Analytics",
		"Blog Team Site",
		"Microsoft",
		"Microsoft",
		};

		internal string[] Images = new string[]
		{
		"bluecircle.png",
		"greencircle.png",
		"lightbluecircle.png",
		"redcircle.png",
		"violetcircle.png",
		"yellowcircle.png",
		};

		internal bool[] Attachments = new bool[]
		{
		false,
		false,
		false,
		true,
		false,
		true,
		false,
		true,
		true,
		false,
		false,
		true,
		false,
		true,
		false,
		};

		internal bool[] Importants = new bool[]
		{
		false,
		true,
		false,
		false,
		false,
		false,
		true,
		false,
		false,
		true,
		true,
		false,
		true,
		false,
		false,
		};

		internal bool[] Opens = new bool[]
		{
		true,
		false,
		true,
		false,
		false,
		true,
		false,
		false,
		true,
		false,
		true,
		false,
		false,
		true,
		false,
		};

		internal string[] Subject = new string[]
		{
		"Dev Essentials: Learn about the future of .NET and celebrate Visual Studio's 25th anniversary",
		"Your daily briefing",
		"Your digest email",
		"Be more recognizable",
		"Dev Essentials: Announcing that the .NET Multiplatform App UI is generally available",
		"You have two new messages",
		"Start learning .NET MAUI and discover a new AI pair programmer",
		"Dev Essentials: Learn how to code with Java",
		"Your friendly, fear-free guide to getting started",
		"Get to know what's new in Outlook",
		"Microsoft Outlook test message",
		"My Analytics | Collaboration Edition",
		"You've joined the Blog Team Site group",
		"Microsoft .NET News: Get started with .NET 6.0 and watch sessions from .NET Conf 2022 on demand",
		"Microsoft .NET News: Learn about new tools and updates for .NET developers",
		};

		internal string[] Descriptions = new string[] {
		"Developer news, updates, and training resources.",
		"Dear developer, It's almost the end of the week",
		"Dear developer, Discover trends in your work habits",
		"Stand out with a profile photo.",
		"One codebase, many platforms: .NET Multiplatform App UI is generally available.",
		"You have two new messages.",
		"Explore resources to get started with .NET MAUI.",
		"Get started: Java for beginners",
		"How to learn and get started with Stack Overflow.",
		"Hello and welcome to Outlook.",
		"This email message was sent automatically by Microsoft Outlook while testing the settings of your account.",
		"Discover your habits. Work smarter.",
		"Welcome to the Blog Team Site group.",
		"The Xamarin Newsletter is now .NET News.",
		"Now available: Visual Studio 2019 version 16.9.",
	};

		#endregion
	}
}