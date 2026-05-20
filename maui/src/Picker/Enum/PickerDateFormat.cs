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
        /// Represents month and day in <c>MM_dd</c> format.
        /// </summary>
        MM_dd,

        /// <summary>
        /// Represents abbreviated month, day, and year in <c>MMM_dd_yyyy</c> format.
        /// </summary>
        MMM_dd_yyyy,

        /// <summary>
        /// Represents full month, day, and year in <c>MMMM_dd_yyyy</c> format.
        /// </summary>
        MMMM_dd_yyyy,

        /// <summary>
        /// Represents full month and year in <c>MMMM_yyyy</c> format.
        /// </summary>
        MMMM_yyyy,

        /// <summary>
        /// Represents a date with four-digit year and two-digit month in <c>yyyy_MM</c> format.
        /// </summary>
        yyyy_MM,

        /// <summary>
        /// Represents year and abbreviated month in <c>yyyy_MMM</c> format.
        /// </summary>
        yyyy_MMM,

        /// <summary>
        /// Represents year and full month in <c>yyyy_MMMM</c> format.
        /// </summary>
        yyyy_MMMM,

        /// <summary>
        /// Represents year, abbreviated month, and day in <c>yyyy_MMM_dd</c> format.
        /// </summary>
        yyyy_MMM_dd,

        /// <summary>
        /// Represents year, full month, and day in <c>yyyy_MMMM_dd</c> format.
        /// </summary>
        yyyy_MMMM_dd,

        /// <summary>
        /// Represents day and abbreviated month in <c>dd_MMM</c> format.
        /// </summary>
        dd_MMM,

        /// <summary>
        /// Represents day and full month in <c>dd_MMMM</c> format.
        /// </summary>
        dd_MMMM,

        /// <summary>
        /// Represents day, full month, and year in <c>dd_MMMM_yyyy</c> format.
        /// </summary>
        dd_MMMM_yyyy,

        /// <summary>
        /// Represents a date in the format weekday, day_month_year, mixing a short weekday name with numeric date parts.
        /// </summary>
        ddd_dd_MM_YYYY,

        /// <summary>
        /// Represents a date in the format year, month, abbreviated weekday, and day, combining numeric year and month with a short weekday name and numeric day.
        /// </summary>
        yyyy_MM_ddd_dd,

        /// <summary>
        /// Represents a date in the format month, abbreviated weekday (twice), and year, combining numeric month with two short weekday names and numeric year.
        /// </summary>
        MM_ddd_ddd_yyyy,

        /// <summary>
        /// Represents default culture based format of the Picker Date format.
        /// </summary>
        Default,
    }
}