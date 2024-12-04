namespace Syncfusion.Maui.Toolkit.BottomSheet
{
	using System;

    /// <summary>
    /// Represents BottomSheet’s state changed event arguments.
    /// </summary>
    /// <remarks>
    /// It contains information like old value and new value.
    /// </remarks> 
    public class StateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a old value when the BottomSheet’s state is changed.
        /// </summary>
        public BottomSheetState OldState { get; internal set; }

        /// <summary>
        /// Gets or sets an new value when the BottomSheet’s state is changed.
        /// </summary>
        public BottomSheetState NewState { get; internal set; }
    }
}
