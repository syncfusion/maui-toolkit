namespace Syncfusion.Maui.Toolkit.TabView;

using Syncfusion.Maui.Toolkit.Internals;
using PointerEventArgs = Syncfusion.Maui.Toolkit.Internals.PointerEventArgs;

internal partial class SfHorizontalContent
{
    #region Interface Methods

    /// <summary>
    /// This method triggers on any touch interaction on <see cref="SfHorizontalContent"/>.
    /// </summary>
    /// <param name="e">Pointer event arguments.</param>
    void ITouchListener.OnTouch(PointerEventArgs e)
    {
        // The method is left empty as the platform - specific implementations are provided in separate files.
    }

    #endregion
}