using Color = Microsoft.Maui.Graphics.Color;
#if WINDOWS
using WColor = Windows.UI.Color;
using GradientBrush = Microsoft.UI.Xaml.Media.GradientBrush;
#elif ANDROID
using Android.Content;
using Android.OS;
using Android.Util;
#endif

namespace Syncfusion.Maui.Toolkit.EntryView 
{
    /// <summary>
    /// The SfEntryView is a entry component which is used to create input view entry for NumericEntry.
    /// </summary>
    internal partial class SfEntryView : Entry 
	{
        #region Fields

        IDrawable? _drawable;

#if MACCATALYST || IOS
        Color? _focusedColor = Color.FromArgb("#8EBDFF");
#else
        Color? _focusedColor = Colors.Gray;
#endif

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfEntryView"/> class.
        /// </summary>
        public SfEntryView() 
		{
            Initialize();
        }

        void Initialize() 
		{
            Style = new Style(typeof(SfEntryView));
            BackgroundColor = Colors.Transparent;
            TextColor = Colors.Black;
            FontSize = 14d;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value for focused stroke.
        /// </summary>
        internal Color? FocusedStroke
		{
            get 
            {
                return _focusedColor;
            }
            set 
            {
                _focusedColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the drawable value.
        /// </summary>
        internal IDrawable? Drawable
		{
            get { return _drawable; }
            set { _drawable = value; }
        }


        /// <summary>
        /// Gets or sets the button size value.
        /// </summary>
        internal double ButtonSize { get; set; }

        #endregion

        #region Override Methods

        /// <summary>
        /// Handler changed method.
        /// </summary>
        protected override void OnHandlerChanged() 
		{
            base.OnHandlerChanged();
            // Unsubscribe from events to avoid duplicate handlers
            Focused -= SfEntry_Focused;
            Unfocused -= SfEntry_Unfocused;

            // Check if the Handler is available for attaching event handlers
            if (Handler != null)
            {
        #if WINDOWS
                if (Handler.PlatformView is Microsoft.UI.Xaml.Controls.TextBox textbox)
                {
                    AdjustWindowsTextBoxStyles(textbox);
                }
        #elif ANDROID
                if (Handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText textbox)
                {
                    AdjustAndroidTextBoxStyles(textbox);
                }
        #endif
                // Subscribe to events when the handler is set
                Focused += SfEntry_Focused;
                Unfocused += SfEntry_Unfocused;
            }
        }

        #if WINDOWS
        private void AdjustWindowsTextBoxStyles(Microsoft.UI.Xaml.Controls.TextBox textbox)
        {
            textbox.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
            textbox.Resources["TextControlBorderThemeThicknessFocused"] = textbox.BorderThickness;

            if (textbox.Resources["TextControlBorderBrushFocused"] is GradientBrush brush &&
                brush.GradientStops.FirstOrDefault()?.Color is WColor color)
            {
                FocusedStroke = new Color(color.R, color.G, color.B, color.A);
            }
        }
        #endif

        #if ANDROID
        private void AdjustAndroidTextBoxStyles(AndroidX.AppCompat.Widget.AppCompatEditText textbox)
        {
            textbox.SetBackgroundColor(Android.Graphics.Color.Transparent);
            var themeAccentColor = textbox.Context != null ? GetThemeAccentColor(textbox.Context) : Android.Graphics.Color.Transparent.ToArgb();
            var color = new Android.Graphics.Color(themeAccentColor);
            FocusedStroke = new Color(color.R, color.G, color.B, color.A);
        }
        #endif

        #endregion

        #region Methods

        /// <summary>
        /// It invoked when entry get unfocused.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The focus event args.</param>
        void SfEntry_Unfocused(object? sender, FocusEventArgs e)
		{
			if (_drawable is SfView sfView)
			{
				sfView.InvalidateDrawable();
			}
        }

        /// <summary>
        /// It invoked when the entry get focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The focus event args.</param>
        void SfEntry_Focused(object? sender, FocusEventArgs e)
		{
			if (_drawable is SfView sfView)
			{
				sfView.InvalidateDrawable();
			}
        }

#if ANDROID
        /// <summary>
        /// Get theme accent color method.
        /// </summary>
        /// <param name="context">The context.</param>        
        static int GetThemeAccentColor(Context context) 
		{
            int colorAttr = Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop
                ? Android.Resource.Attribute.ColorAccent
                : context.Resources?.GetIdentifier("colorAccent", "attr", context.PackageName) 
                    ?? Android.Resource.Color.BackgroundDark;

            TypedValue outValue = new();
            bool resolved = context.Theme?.ResolveAttribute(colorAttr, outValue, true) ?? false;
            
            return resolved ? outValue.Data : Android.Resource.Color.BackgroundDark;
        }
#endif
        #endregion
    }
}
