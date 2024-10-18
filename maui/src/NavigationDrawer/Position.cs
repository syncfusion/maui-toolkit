namespace Syncfusion.Maui.Toolkit.NavigationDrawer
{
    /// <summary>
    /// Describes the possible values for the position of navigation drawer in SfNavigationDrawer control.
    /// </summary>
    public enum Position
    {
        /// <summary>
        /// Places drawer at the left position.
        /// </summary>
        Left,

        /// <summary>
        /// Places drawer at the right position.
        /// </summary>
        Right,

        /// <summary>
        /// Places drawer at the top position.
        /// </summary>
        Top,

        /// <summary>
        /// Places drawer at the bottom position.
        /// </summary>
        Bottom,
    }

    /// <summary>
    /// Defines constants that specify how the drawer is animated when opened and closed.
    /// </summary>
    public enum Transition
    {
        /// <summary>
        /// Drawer gets opened pushing the main content.
        /// </summary>
        Push,

        /// <summary>
        /// Drawer will be placed beneath the main content and gets revealed moving the main content.
        /// </summary>
        Reveal,

        /// <summary>
        /// Drawer will be above the main content and slides over the main content.
        /// </summary>
        SlideOnTop,
    }
}