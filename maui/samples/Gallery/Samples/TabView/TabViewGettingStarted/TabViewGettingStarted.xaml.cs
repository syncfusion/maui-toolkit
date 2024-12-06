using System.Reflection;

namespace Syncfusion.Maui.ControlsGallery.TabView.SfTabView
{
    /// <summary>
    /// A sample view demonstrating the usage of SfTabView.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabViewGettingStarted : SampleView
    {
       
        public TabViewGettingStarted()
        {
            InitializeComponent();
            BindingContext = new TabViewModel();
        }

        /// <summary>
        /// Handles the SelectionChanged event of the tabView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Syncfusion.Maui.Toolkit.TabView.TabSelectionChangedEventArgs"/> instance containing the event data.</param>
        private void tabView_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.TabView.TabSelectionChangedEventArgs e)
        {
            switch(e.NewIndex)
            {
                case 0:
                    if(Application.Current != null)
                    {
                        FavoritesIcon.Color = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
                        RecentsIcon.Color = ContactsIcon.Color = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["TextColourDark"] : (Color)App.Current.Resources["TextColourLight"];
                    }

                    break;

                case 1:
                    if(Application.Current != null)
                    {
                        RecentsIcon.Color = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
                        FavoritesIcon.Color = ContactsIcon.Color = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["TextColourDark"] : (Color)App.Current.Resources["TextColourLight"];
                    }

                    break;

                case 2:
                    if(Application.Current != null)
                    {
                        ContactsIcon.Color = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
                        FavoritesIcon.Color = RecentsIcon.Color = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["TextColourDark"] : (Color)App.Current.Resources["TextColourLight"];
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TabModel
    {
        /// <summary>
        /// Gets or sets the name of the tab.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the background color of the image.
        /// </summary>
        public Color? ImageBackground { get; set; }

        /// <summary>
        /// Gets or sets the image source for the tab.
        /// </summary>
        public ImageSource? ImageSource { get; set; }
    }

    public class TabViewModel
    {

        /// <summary>
        /// Gets or sets the source of TabModel.
        /// </summary>
        public List<TabModel> TabModelSource { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TabViewModel"/> class.
        /// </summary>
        public TabViewModel()
        {

/* Unmerged change from project 'Syncfusion.Maui.ControlsGallery (net9.0-windows10.0.19041.0)'
Before:
            TabModelSource = new List<TabModel>();
            List<TabModel> TabModels = new List<TabModel>();
After:
            TabModelSource = [];
            List<TabModel> TabModels = new List<TabModel>();
*/
            TabModelSource = [];
            List<TabModel> TabModels =
			[
				new TabModel() { ImageBackground = Color.FromArgb("#F0F361"), Name = "Alex", ImageSource = "alexandar.png" },
				new TabModel() { ImageBackground = Color.FromArgb("#FFC252"), Name = "Clara", ImageSource = "clara.png" },
				new TabModel() { ImageBackground = Color.FromArgb("#8AF8FF"), Name = "Steve", ImageSource = "sebastian.png" },
				new TabModel() { ImageBackground = Color.FromArgb("#A1B2FF"), Name = "Richard", ImageSource = "jackson.png" },
				new TabModel() { ImageBackground = Color.FromArgb("#7A7A7A"), Name = "Nora", ImageSource = "nora.png" },
				new TabModel() { ImageBackground = Color.FromArgb("#FFB381"), Name = "David", ImageSource = "tye.png" },
				new TabModel() { ImageBackground = Color.FromArgb("#7FE8EE"), Name = "Gabriella", ImageSource = "gabriella.png" },
				new TabModel() { ImageBackground = Color.FromArgb("#FFF27C"), Name = "Lita", ImageSource = "lita.png" },
			];
#if WINDOWS || MACCATALYST
            TabModels.Add(new TabModel() { ImageBackground = Color.FromArgb("#EB70FF"), Name = "Liam", ImageSource = "liam.png" });
            TabModels.Add(new TabModel() { ImageBackground = Color.FromArgb("#F0F361"), Name = "Dave", ImageSource = "alexandar.png" });
            TabModels.Add(new TabModel() { ImageBackground = Color.FromArgb("#8AF8FF"), Name = "Ben", ImageSource = "sebastian.png" });
#endif
            TabModelSource = TabModels;
        }

    }
}
