using Syncfusion.Maui.Toolkit.Internals;
using Syncfusion.Maui.Toolkit.Graphics.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;
using Syncfusion.Maui.Toolkit.EffectsView;

namespace Syncfusion.Maui.Toolkit.Popup.Helper
{
	/// <summary>
	/// Custom class for Button.
	/// </summary>
	internal class SfButton : ButtonBase
	{
		#region Fields

		/// <summary>
		/// The SfEffectsView associated with this SfButton control.
		/// </summary>
		SfEffectsViewAdv? _effectsView;

		/// <summary>
		/// The field sets and checks the new size of the drawn elements.
		/// </summary>
		Size _buttonSemanticsSize = Size.Zero;

		/// <summary>
		/// The list add the bounds values of drawing elements as nodes.
		/// </summary>
		List<SemanticsNode> _buttonSemanticsNodes = new List<SemanticsNode>();

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="SfButton"/> class.
		/// </summary>
		internal SfButton()
		{
			InitializeEffectsView();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// The effects for the SfButton will be initialized using this method.
		/// </summary>
		void InitializeEffectsView()
		{
			// Setting ShouldIgnoreTouches as true, the EffectsView will ignore touches, allowing the Button to handle click events.
			_effectsView = new SfEffectsViewAdv();
			_effectsView.ShouldIgnoreTouches = true;
			_effectsView.ClipToBounds = true;
			Children.Add(_effectsView);
		}

		/// <summary>
		/// This method will remove the effects when the action is released ,cancelled and exited.
		/// </summary>
		void RemoveEffects()
		{
			if (_effectsView is null)
			{
				return;
			}

			_effectsView.Reset();
		}

		#endregion

		#region Override Methods

		/// <summary>
		/// This method will trigger when the mouse is hovered over and clicked on the element.
		/// </summary>
		/// <param name="e">Represents the event data.</param>
		public override void OnTouch(PointerEventArgs e)
		{
			if (_effectsView is not null)
			{
				if (e.Action is PointerActions.Pressed)
				{
					_effectsView.ApplyEffects(SfEffects.Ripple, RippleStartPosition.Default, new System.Drawing.Point((int)e.TouchPoint.X, (int)e.TouchPoint.Y), false);
				}

				if (e.Action is PointerActions.Entered)
				{
					_effectsView.ApplyEffects(SfEffects.Highlight);
				}
			}

			if (e.Action is PointerActions.Released)
			{
				RemoveEffects();
				RaiseClicked(EventArgs.Empty);
			}
			else if (e.Action is PointerActions.Cancelled || e.Action is PointerActions.Exited)
			{
				RemoveEffects();
			}
		}

		/// <summary>
		/// Returns the semantics node list.
		/// </summary>
		/// <param name="width">Represents width of the SemanticsNode.</param>
		/// <param name="height">Represents height of the SemanticsNode.</param>
		/// <returns>Returns the List of all SemanticsNodes.</returns>
		protected override List<SemanticsNode> GetSemanticsNodesCore(double width, double height)
		{
			Size buttonSize = new(Width, Height);
			if (_buttonSemanticsNodes.Count is not 0 && _buttonSemanticsSize == buttonSize)
			{
				return _buttonSemanticsNodes;
			}

			_buttonSemanticsSize = buttonSize;
			SemanticsNode buttonNode = new SemanticsNode();
			buttonNode.Bounds = new Rect(0, 0, Width, Height);
			buttonNode.Text = Text + "Button" + "Double tap to activate";
			_buttonSemanticsNodes.Add(buttonNode);
			return _buttonSemanticsNodes;
		}

		#endregion
	}

	/// <summary>
	/// Represents a custom class that establishes the initial style for the control when it's initialized.
	/// </summary>
	internal class SfEffectsViewAdv : SfEffectsView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SfEffectsViewAdv"/> class.
		/// </summary>
		internal SfEffectsViewAdv()
		{
			SetDynamicResource(HighlightBackgroundProperty, "SfPopupHoverFooterButtonBackground");
			SetDynamicResource(RippleBackgroundProperty, "SfPopupFooterButtonRippleBackground");
			Style = new Style(typeof(SfEffectsViewAdv));
		}
	}
}