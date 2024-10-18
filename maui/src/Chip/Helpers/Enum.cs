namespace Syncfusion.Maui.Toolkit.Chips
{
    #region Enums

    /// <summary>
    /// Defines the position of the image in <see cref="SfChip"/> control.
    /// </summary>
    public enum Alignment
    {
        /// <summary>
        /// It aligns the image at start position of the chip.
        /// </summary>
        Start,

        /// <summary>
        /// It aligns the image at end position of the chip.
        /// </summary>
        End,

        /// <summary>
        /// It aligns the image at top position of the chip.
        /// </summary>
        Top,

        /// <summary>
        /// It aligns the image at bottom position of the chip.
        /// </summary>
        Bottom,

        /// <summary>
        /// It aligns the image at left position of the chip.
        /// </summary>
        Left,

        /// <summary>
        /// It aligns the image at right position of the chip.
        /// </summary>
        Right,

        /// <summary>
        /// It aligns the image at default position of the chip.
        /// </summary>
        Default
    }

    /// <summary>
    /// Contains values to the <see cref="SfChipsType" /> enumeration that determines the SfChipsType as Input, Filter, Choice, or Action.
    /// </summary>
    public enum SfChipsType
    {
        /// <summary>
        /// The chip group have an editor as the last child in layout.
        /// </summary>
        Input,

        /// <summary>
        /// Allows to select multiple chips in a group.
        /// </summary>
        Filter,

        /// <summary>
        /// Allows to select/toggle at most only one chip.
        /// </summary>
        Choice,

        /// <summary>
        /// Triggers an action while tapping the chip.
        /// </summary>
        Action,
    }

    /// <summary>
    /// Represents the possible selection mode for <see cref="SfChipsType.Choice"/> type.
    /// </summary>
    public enum ChoiceMode
    {
        /// <summary>
        /// Enables selecting only one chip item at a time. If you select a new item, the previously selected one is deselected and the new item is selected. At least one item must be in a selected state, and the selected item cannot be deselected.
        /// </summary>
        Single,

        /// <summary>
        /// Enables selecting only one chip item at a time. If you select a new item, the previously selected one is deselected, and the new item is selected. Unlike <see cref="ChoiceMode.Single"/>, it is possible to deselect the currently selected item, allowing all items in an <see cref="SfChipGroup"/> to be deselected.
        /// </summary>
        SingleOrNone,
    }

    #endregion
}