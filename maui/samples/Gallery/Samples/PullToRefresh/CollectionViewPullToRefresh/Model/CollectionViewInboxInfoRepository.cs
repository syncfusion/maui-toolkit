using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.PullToRefresh
{
	/// <summary>
	/// A class used to assign collection values for a Model properties
	/// </summary>
	public class CollectionViewInboxInfoRepository
	{
		#region Fields

		readonly Random _random = new();

		#endregion

		#region Get inbox info

		/// <summary>
		/// Used to assign the Collection values to Model Properties.
		/// </summary>
		/// <returns>Added InboxInfos items</returns>
		internal ObservableCollection<InboxInfo> GetInboxInfo()
		{
			var empInfo = new ObservableCollection<InboxInfo>();
			int k = 0;
			for (int i = 0; i < _subject.Length; i++)
			{
				if (k >= 10)
				{
					k = 0;
				}
				var record = new InboxInfo()
				{
					ProfileName = _profileList[i],
					Name = _nameList[i],
					Subject = _subject[i],
					Date = DateTime.Today.AddDays(i * -3),
					Description = _descriptions[i],
					IsAttached = _attachments[i],
					IsImportant = _importants[i],
					IsOpened = _opens[i],
					AvatarTextColor = Color.FromArgb(_avatarTextColors[k]),
					AvatarBackgroundColor = Color.FromArgb(_avatarBackgroundColors[k]),
				};
				empInfo.Add(record);
				k++;
			}
			return empInfo;
		}

		/// <summary>
		/// Used to assign the Collection values to Model Properties while refreshing.
		/// </summary>
		/// <param name="count">Number of items to be added.</param>
		/// <returns>Added inboxInfos items</returns>
		internal ObservableCollection<InboxInfo> AddRefreshItems(int count)
		{
			var empInfo = new ObservableCollection<InboxInfo>();
			int k = 0;
			for (int i = 0; i < count; i++)
			{
				var j = _random.Next(_subject.Length);

				if (k >= 10)
				{
					k = 0;
				}
				var record = new InboxInfo()
				{
					ProfileName = _profileList[j],
					Name = _nameList[j],
					Subject = _subject[j],
					Date = DateTime.Now.AddMinutes((i * 10)),
					Description = _descriptions[j],
					IsAttached = _attachments[j],
					IsImportant = _importants[j],
					IsOpened = false,
					AvatarTextColor = Color.FromArgb(_avatarTextColors[k]),
					AvatarBackgroundColor = Color.FromArgb(_avatarBackgroundColors[k]),
				};
				empInfo.Add(record);
				k++;
			}

			return empInfo;
		}

		#endregion

		#region Employee Info

		readonly string[] _profileList =
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

		readonly string[] _nameList =
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

		readonly bool[] _attachments =
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

		readonly bool[] _importants =
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

		readonly bool[] _opens =
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

		readonly string[] _subject =
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

		readonly string[] _descriptions = [
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

		readonly string[] _avatarBackgroundColors =
	   [
		"#8BEAF0",
		"#F5EF9A",
		"#BACF82",
		"#F3B3E4",
		"#B1C5FF",
		"#8AD6B7",
		"#F7BC70",
		"#8BD0F0",
		"#FFB2BA",
		"#CFBDFE",
	   ];

		readonly string[] _avatarTextColors =
	   [
		"#004448",
		"#414141",
		"#273500",
		"#4D1F47",
		"#162E60",
		"#003829",
		"#462A00",
		"#003546",
		"#561D27",
		"#36275D",
	   ];
		#endregion
	}
}