namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Represents the <see cref="PopupExtension"/> class.
	/// </summary>
	internal static partial class PopupExtension
	{
		/// <summary>
		/// Tracks open popups in Z-order (the last element is the topmost popup).
		/// </summary>
		internal static System.Collections.Generic.List<SfPopup> OpenPopups = new System.Collections.Generic.List<SfPopup>();

		/// <summary>
		/// Gets the top-most open popup, or null if none.
		/// </summary>
		internal static SfPopup? TopMostOpenPopup
		{
			get
			{
				int count = OpenPopups.Count;
				return count > 0 ? OpenPopups[count - 1] : null;
			}
		}

		#region Internal Methods

		/// <summary>
		/// Gets the current page of the application.
		/// </summary>
		/// <param name="shouldReturnOnlyMainPage">bool value to specify whether to return Navigation Page directly without returning the current page of Navigation Page
		/// if the Navigation page is used as the Main page in Application.</param>
		/// <returns>Returns the current page of the application.</returns>
		internal static Page? GetMainPage(bool shouldReturnOnlyMainPage = false)
		{
			var windowPage = PopupExtension.GetMainWindowPage();
			if (windowPage is null)
			{
				return null;
			}

			// An exception is thrown when showing the popup in the OnAppearing() method of a modally pushed page.
			if (windowPage.Navigation is not null && windowPage.Navigation.ModalStack is not null)
			{
				var modalPage = windowPage.Navigation.ModalStack.LastOrDefault();
				if (modalPage is not null)
				{
					// Calling Navigation.PushModalAsync(new NavigationPage(new ModalPage())) does not return the NavigationPage of the current page.
					if (modalPage is NavigationPage navPage)
					{
						return navPage.CurrentPage ?? windowPage;
					}

					return modalPage;
				}
			}

			if (windowPage is NavigationPage navigationPage && !shouldReturnOnlyMainPage)
			{
				return navigationPage.CurrentPage ?? windowPage;
			}
			else if (windowPage is Shell shellPage)
			{
				// 837430 : when shell current page is null, NullReferenceException is thrown in ios in release mode.
				return shellPage.CurrentPage ?? windowPage;
			}

			return windowPage;
		}

		/// <summary>
		/// Gets the main page of the application.
		/// </summary>
		/// <returns>Returns the main page of the application.</returns>
		internal static Page? GetMainWindowPage()
		{
			var application = IPlatformApplication.Current?.Application as Microsoft.Maui.Controls.Application;
			if (application is not null && application.Windows.Count > 0)
			{
				return application.Windows[0].Page;
			}

			return null;
		}

#if !IOS
		/// <summary>
		/// This method will returns the SafeAreaHeight.
		/// </summary>
		/// <param name="position">Position of the safe area.</param>
		/// <returns>Returns the SafeAreaHeight.</returns>
		internal static int GetSafeAreaHeight(string position) => 0;
#endif

		#endregion
	}
}
