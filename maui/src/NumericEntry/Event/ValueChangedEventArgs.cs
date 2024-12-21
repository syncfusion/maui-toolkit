namespace Syncfusion.Maui.Toolkit.NumericEntry
{
	/// <summary>
	/// Provides event data for the <see cref="SfNumericEntry.ValueChanged"/> event.
	/// </summary>
	public class NumericEntryValueChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NumericEntryValueChangedEventArgs"/> class.
		/// </summary>
		/// <param name="newValue">Contains the new value.</param>
		/// <param name="oldValue">Contains the old value.</param>
		internal NumericEntryValueChangedEventArgs(double? newValue, double? oldValue)
		{
			NewValue = newValue;
			OldValue = oldValue;
		}

		/// <summary>
		/// Gets the new <see cref="SfNumericEntry.Value"/> to be set for a <see cref="SfNumericEntry"/>.
		/// </summary>  
		public double? NewValue { get; }

		/// <summary>
		/// Gets the old <see cref="SfNumericEntry.Value"/> being replaced in <see cref="SfNumericEntry"/>.
		/// </summary>
		public double? OldValue { get; }
	}

}
