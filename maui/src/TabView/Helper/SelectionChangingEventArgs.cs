using System;
using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.TabView;

/// <summary>
/// Provides data for the <see cref="SfTabView.SelectionChanging"/> event.
/// </summary>
public class SelectionChangingEventArgs : CancelEventArgs
{
    /// <summary>
    /// Gets the index value of the item that is about to be selected. 
    /// </summary>
    public int Index { get; internal set; }
}