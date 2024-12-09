namespace Syncfusion.Maui.Toolkit.Themes
{
	/// <summary>
	/// ThemeDictionary class for Syncfusion in which controls themes are to be included.
	/// </summary>
	[XamlCompilation(XamlCompilationOptions.Skip)]
	public partial class SyncfusionThemeDictionary : ResourceDictionary
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Syncfusion.Maui.Toolkit.Themes.SyncfusionThemeDictionary"/> class.
		/// </summary>
		public SyncfusionThemeDictionary()
		{
			InitializeElement();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="isDark"></param>
		public SyncfusionThemeDictionary(bool isDark = false)
		{
			if (isDark)
			{
				MergedDictionaries.Add(new DarkThemeColors());
			}
			else
			{
				MergedDictionaries.Add(new LightThemeColors());
			}

			InitializeElement();
		}

		/// <summary>
		/// 
		/// </summary>
		private void InitializeElement()
		{
			InitializeComponent();

			ThemeElement.AddStyleDictionary(this);
		}
	}
}