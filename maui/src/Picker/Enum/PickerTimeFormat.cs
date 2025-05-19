namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Defines the picker time format for the SfTimePicker.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Used to specify the different time elements")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Used to specify the different time formats")]
    public enum PickerTimeFormat
    {
        /// <summary>
        /// Represents the hour and minute in H mm format.
        /// </summary>
        H_mm,

        /// <summary>
        /// Represents the hour, minute, and second in H mm ss format.
        /// </summary>
        H_mm_ss,

        /// <summary>
        /// Represents the hour, minute, second, and AM/PM designator in h mm ss tt format.
        /// </summary>
        h_mm_ss_tt,

        /// <summary>
        /// Represents the hour, minute, and AM/PM designator in h mm tt format.
        /// </summary>
        h_mm_tt,

        /// <summary>
        /// Represents the hour and minute in HH mm format.
        /// </summary>
        HH_mm,

        /// <summary>
        /// Represents the hour, minute, and second in HH mm ss format.
        /// </summary>
        HH_mm_ss,

        /// <summary>
        /// Represents the hour, minute, second, and AM/PM designator in hh mm ss tt format.
        /// </summary>
        hh_mm_ss_tt,

        /// <summary>
        /// Represents the hour, minute, and AM/PM designator in hh mm tt format.
        /// </summary>
        hh_mm_tt,

        /// <summary>
        /// Represents the hour and AM/PM designator in hh tt format.
        /// </summary>
        hh_tt,

        /// <summary>
        /// Represents default culture based format of the Picker Time format.
        /// </summary>
        Default,
    }
}