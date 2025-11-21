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
        /// Represents 24-hour padded hour, minute, second, and milliseconds in <c>HH_mm_ss_fff</c> format.
        /// Example: <c>14_05_33_089</c> for 2:05 PM, 33 seconds, and 89 milliseconds.
        /// </summary>
        HH_mm_ss_fff,

        /// <summary>
        /// Represents the hour, minute, second, and AM/PM designator in hh mm ss tt format.
        /// </summary>
        hh_mm_ss_tt,

        /// <summary>
        /// Represents 12-hour padded hour, minute, second, milliseconds, and AM/PM in <c>hh_mm_ss_fff_tt</c> format.
        /// Example: <c>02_05_33_089_PM</c> for 2:05 PM, 33 seconds, and 89 milliseconds.
        /// </summary>
        hh_mm_ss_fff_tt,

        /// <summary>
        /// Represents the hour, minute, and AM/PM designator in hh mm tt format.
        /// </summary>
        hh_mm_tt,

        /// <summary>
        /// Represents the hour and AM/PM designator in hh tt format.
        /// </summary>
        hh_tt,

        /// <summary>
        /// Represents minutes and seconds in <c>mm_ss</c> format.
        /// Example: <c>02_45</c> for 2 minutes and 45 seconds.
        /// </summary>
        mm_ss,

        /// <summary>
        /// Represents minutes, seconds, and milliseconds in <c>mm_ss_fff</c> format.
        /// Example: <c>03_12_256</c> for 3 minutes, 12 seconds, and 256 milliseconds.
        /// </summary>
        mm_ss_fff,

        /// <summary>
        /// Represents seconds and milliseconds in <c>ss_fff</c> format.
        /// Example: <c>15_007</c> for 15 seconds and 500 milliseconds.
        /// </summary>
        ss_fff,

        /// <summary>
        /// Represents default culture based format of the Picker Time format.
        /// </summary>
        Default,
    }
}