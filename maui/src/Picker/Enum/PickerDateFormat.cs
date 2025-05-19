namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Defines the picker date format for the SfDatePicker.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Used to specify the different date elements")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Used to specify the different date formats")]
    public enum PickerDateFormat
    {
        /// <summary>
        /// Represents the day and month in dd MM format.
        /// </summary>
        dd_MM,

        /// <summary>
        /// Represents the day, month, and year in dd MM yyyy format.
        /// </summary>
        dd_MM_yyyy,

        /// <summary>
        /// Represents the day, month, and year in dd MMM yyyy format.
        /// </summary>
        dd_MMM_yyyy,

        /// <summary>
        /// Represents the month, day, and year in M d yyyy format.
        /// </summary>
        M_d_yyyy,

        /// <summary>
        ///  Represents the month, day, and year in MM dd yyyy format.
        /// </summary>
        MM_dd_yyyy,

        /// <summary>
        /// Represents the month and year in MM yyyy format.
        /// </summary>
        MM_yyyy,

        /// <summary>
        /// Represents the month and year in MMM yyyy format.
        /// </summary>
        MMM_yyyy,

        /// <summary>
        /// Represents the year, month, and day in yyyy MM dd format.
        /// </summary>
        yyyy_MM_dd,

        /// <summary>
        /// Represents default culture based format of the Picker Date format.
        /// </summary>
        Default,
    }
}