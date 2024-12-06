using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.NavigationDrawer.NavigationDrawer
{
	/// <summary>
	/// A class used to assign collection values for a Model properties
	/// </summary>
	public class MailInfoRepository
	{
		#region Fields

		readonly Random _random = new();

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
			for (int i = 0; i < _subject.Length; i++)
			{
				if (k > 5)
				{
					k = 0;
				}
				var record = new MailInfo()
				{
					ProfileName = _profileList[i],
					Name = _nameList[i],
					Subject = _subject[i],
					Date = DateTime.Today.AddDays(i * -3),
					Description = _descriptions[i],
					Image = _images[k],
					IsAttached = _attachments[i],
					IsImportant = _importants[i],
					IsOpened = _opens[i],
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
			for (int i = 0; i < _subject.Length; i++)
			{
				if (i % itemsCount == 0)
				{
					var j = _random.Next(_subject.Length);

					if (k > 5)
					{
						k = 0;
					}
					var record = new MailInfo()
					{
						ProfileName = _profileList[j],
						Name = _nameList[j],
						Subject = _subject[j],
						Date = DateTime.Today.AddDays(i * -3),
						Description = _descriptions[j],
						Image = _images[k],
						IsAttached = _attachments[j],
						IsImportant = _importants[j],
						IsOpened = _opens[j],
					};
					empInfo.Add(record);
					k++;
				}
			}

			return empInfo;
		}

		#endregion

		#region Employee Info

		internal string[] _profileList =
		[
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
		];

		internal string[] _nameList =
		[
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
		];

		internal string[] _images =
		[
		"bluecircle.png",
		"greencircle.png",
		"lightbluecircle.png",
		"redcircle.png",
		"violetcircle.png",
		"yellowcircle.png",
		];

		internal bool[] _attachments =
		[
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
		];

		internal bool[] _importants =
		[
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
		];

		internal bool[] _opens =
		[
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
		];

		internal string[] _subject =
		[
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
		];

		internal string[] _descriptions = [
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
	];

		#endregion
	}
}