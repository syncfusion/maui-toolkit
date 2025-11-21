using Syncfusion.Maui.Toolkit.EffectsView;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using ITextElement = Syncfusion.Maui.Toolkit.Graphics.Internals.ITextElement;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Buttons
{
	/// <summary>
	/// The <see cref="SfButton"/> class provides a way for users to interact with the application 
	/// by clicking or tapping. It can display text, an icon, or both, and supports various customization options.
	/// </summary>
	/// <example>
	///  The below example shows how to initialize the <see cref="SfButton"/>
	/// # [XAML](#tab/tabid-1)
	/// <code Lang="XAML"><![CDATA[
	/// <toolkit:SfButton
	///       x:Name="button"
	///       Text="Button"
	///       HeightRequest="50"
	///       WidthRequest="200"/>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code Lang="C#"><![CDATA[
	/// SfButton button = new SfButton();
	/// button.Text = "Button";
	/// button.WidthRequest = 200;
	/// button.HeightRequest = 50;
	/// ]]></code>
	/// ***
	/// </example>
	[ContentProperty("Content")]
	public partial class SfButton : ButtonBase, ITextElement, IParentThemeElement
    {

		#region Fields

		/// <summary>
		/// Represents the number of lines used for text content in the button.
		/// </summary>
		double _numberOfLines = 1;

		/// <summary>
		/// Holds a custom view that is optionally added to the button's content.
		/// </summary>
		View? _customView;

		/// <summary>
		/// Defines the rectangular bounds for the icon within the button.
		/// </summary>
		RectF _iconRectF = new();

		/// <summary>
		/// A reference to the effects view used to add visual effects to the button.
		/// </summary>
		SfEffectsView? _effectsView;

		/// <summary>
		/// The size used for semantic purposes to describe the button's layout to assistive technologies.
		/// </summary>
		Size _buttonSemanticsSize = Size.Zero;

		/// <summary>
		/// Stores semantic nodes used for accessibility features of the button.
		/// </summary>
		readonly List<SemanticsNode> _buttonSemanticsNodes = [];

		/// <summary>
		/// Indicates whether the flow direction of the text is right-to-left.
		/// </summary>
		bool _isRightToLeft => ((this as IVisualElementController).EffectiveFlowDirection & EffectiveFlowDirection.RightToLeft) == EffectiveFlowDirection.RightToLeft;

		/// <summary>
		/// Indicates whether the button is currently being pressed.
		/// </summary>
		bool _isPressed = false;

#if WINDOWS || MACCATALYST
		/// <summary>
		/// Tracks whether the mouse is hovering over the button.
		/// </summary>
		bool _isMouseHover = false;

#if WINDOWS
		/// <summary>
		/// Tracks whether a press action has been invoked.
		/// </summary>
        bool _isPressInvoked = false;
#endif
#endif

#if IOS || MACCATALYST
		/// <summary>
		/// Captures the point where a touch-down interaction occurred.
		/// </summary>
		Point _touchDownPoint;

		/// <summary>
		/// This constant used to detect whether the interaction is touch or scroll.
		/// </summary>
		const double ScrollThreshold = 5.0;
#endif 

#if ANDROID

		/// <summary>
		/// Defines a margin value for text on Android.
		/// </summary>
		const float AndroidTextMargin = 3.0f;
#endif

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfButton"/> class.
		/// </summary>
		public SfButton()
		{
			InitializeElement();
		}

		#endregion

		#region Destructor

		/// <summary>
		/// Destructor of the <see cref="SfButton"/> class
		/// </summary>
		~SfButton()
		{
			UnhookEvents();
		}

		#endregion
	}
}
