using System.ComponentModel;
using Font = Microsoft.Maui.Font;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{

	/// <summary>
	/// Represents a text element with various font properties and methods to handle changes.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface ITextElement
	{
		/// <summary>
		/// Gets the font attributs.
		/// </summary>
		FontAttributes FontAttributes { get; }

		/// <summary>
		/// Gets the font family.
		/// </summary>
		string FontFamily { get; }

		/// <summary>
		/// Gets the font size.
		/// </summary>
		[System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
		double FontSize { get; }

		/// <summary>
		/// Gets the font family, style and size of the font.
		/// </summary>
		Font Font { get; }

		/// <summary>
		/// Gets the text color.
		/// </summary>
		Color TextColor { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether font auto-scaling is enabled.
		/// </summary>
		bool FontAutoScalingEnabled { get; set; }

		/// <summary>
		/// Gets the font manager.
		/// </summary>
		IFontManager? FontManager => GetFontManager();

		/// <summary>
		/// Called when the font family changes.
		/// </summary>
		/// <param name="oldValue">The old font family.</param>
		/// <param name="newValue">The new font family.</param>
		void OnFontFamilyChanged(string oldValue, string newValue);

		/// <summary>
		/// Called when the font size changes.
		/// </summary>
		/// <param name="oldValue">The old font size.</param>
		/// <param name="newValue">The new font size.</param>
		void OnFontSizeChanged(double oldValue, double newValue);

		/// <summary>
		/// Creates the default value for the font size.
		/// </summary>
		/// <returns>The default font size.</returns>
		double FontSizeDefaultValueCreator();

		/// <summary>
		/// Called when the font attributes changes.
		/// </summary>
		/// <param name="oldValue">The old font attribute.</param>
		/// <param name="newValue">The new font attribute.</param>
		void OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue);

		/// <summary>
		/// Called when the font changes.
		/// </summary>
		/// <param name="oldValue">The old font.</param>
		/// <param name="newValue">The new font.</param>
		void OnFontChanged(Font oldValue, Font newValue);

		/// <summary>
		/// Called when the font auto-scaling enabled state changes.
		/// </summary>
		/// <param name="oldValue">The old auto-scaling state.</param>
		/// <param name="newValue">The new auto-scaling state.</param>
		void OnFontAutoScalingEnabledChanged(bool oldValue, bool newValue);

		/// <summary>
		/// Get font manager from handler services or from application services.
		/// </summary>
		/// <remarks>
		/// Check maui-chart source, if you want to know how to set the font manager.
		/// </remarks>
		IFontManager? GetFontManager()
		{
			if (Application.Current != null && IPlatformApplication.Current != null)
			{
				return IPlatformApplication.Current.Services.GetRequiredService<IFontManager>();
			}

			return null;
		}
	}
}
