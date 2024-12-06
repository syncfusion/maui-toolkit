namespace Syncfusion.Maui.Toolkit.Themes
{

	/// <summary>
	/// Represents a collection of theme resources for Syncfusion controls.
	/// </summary>
	/// <remarks>
	/// This class is used to include various themes for Syncfusion controls. Themes can be added to this ResourceDictionary to apply consistent styling across the application.
	/// </remarks>
	public partial class SyncfusionThemeResourceDictionary : ResourceDictionary
	{

		private SfVisuals _sfVisualTheme = SfVisuals.MaterialLight;

		/// <summary>
		/// Gets or sets the visual theme for the Syncfusion control.
		/// </summary>
		/// <value>
		/// Accepts an <see cref="SfVisuals"/> value, with the default being <see cref="SfVisuals.MaterialLight"/>.
		/// </value>
		public SfVisuals VisualTheme
		{
			get
			{
				return _sfVisualTheme;
			}
			set
			{
				_sfVisualTheme = value;

				UpdateVisualTheme();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SyncfusionThemeResourceDictionary"/> class.
		/// </summary>
		public SyncfusionThemeResourceDictionary()
		{
			UpdateDefaultTheme();
		}

		private void UpdateVisualTheme()
		{

			MergedDictionaries.Clear();
			if (VisualTheme == SfVisuals.MaterialLight)
			{
				UpdateDefaultTheme();
			}
			else if (VisualTheme == SfVisuals.MaterialDark)
			{
				UpdateDefaultTheme(true);
			}
		}

		private void UpdateDefaultTheme(bool isDark = false)
		{
			MergedDictionaries.Clear();
			MergedDictionaries.Add(new DefaultTheme(isDark));
		}

	}

	/// <summary>
	/// Specifies the visual theme types in Syncfusion.
	/// </summary>
	public enum SfVisuals
	{
		/// <summary>
		/// 
		/// </summary>
		MaterialLight,

		/// <summary>
		/// 
		/// </summary>
		MaterialDark

	}
}
