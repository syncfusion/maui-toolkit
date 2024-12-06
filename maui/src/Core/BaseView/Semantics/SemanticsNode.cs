namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{

	/// <summary>
	/// Holds the semantics node information.
	/// </summary>
	public class SemanticsNode
	{
		/// <summary>
		/// Gets and sets the bounds for semantics node.
		/// </summary>
		public Rect Bounds = Rect.Zero;

		/// <summary>
		/// Gets and sets the text or hint for semantics node.
		/// </summary>
		public string Text = string.Empty;

		/// <summary>
		/// Gets and sets the unique id for semantics node.
		/// </summary>
		public int Id;

		/// <summary>
		/// Gets and sets to define the semantics node is interactive or not.
		/// </summary>
		public bool IsTouchEnabled;

		/// <summary>
		/// Gets and sets the action need to be performed on semantics node interaction.
		/// </summary>
		public Action<SemanticsNode>? OnClick;
	}
}