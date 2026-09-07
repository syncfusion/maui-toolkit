namespace Syncfusion.Maui.Toolkit.GridSplitter
{
    /// <summary>
    /// Cross-platform implementation of <see cref="SfGridSplitter"/>.
    /// Platform-specific behavior is provided by the corresponding
    /// platform partial classes.
    /// </summary>
    public partial class SfGridSplitter
    {
        // Standard / fallback partial. Platform partials override the
        // hook methods below to wire the appropriate platform events.
        //
        // We intentionally do NOT wire a pointer-leave listener on the
        // standard partial because the standard platform does not have
        // a reliable pointer-leave signal. The per-separator Exited
        // path remains the primary clear mechanism, and the per-
        // separator PointerExited on Windows / iOS / MacCatalyst /
        // Android covers the strip-to-outside transition. The
        // ForceClearAllSeparatorsHoverState safety net is invoked
        // from the per-pane collapse / expand paths so a stale hover
        // can never survive a structural layout change.
    }
}
