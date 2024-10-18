using System;

namespace Syncfusion.Maui.Toolkit.TextInputLayout
{
    /// <summary>
    /// Provides data for the <see cref="SfTextInputLayout.PasswordVisibilityToggled"/> event in <see cref="SfTextInputLayout"/>.
    /// </summary>
    /// <remarks>
    /// This class contains information about the current visibility state of the password
    /// when the toggle icon is activated.
    /// </remarks>
    public class PasswordVisibilityToggledEventArgs : EventArgs
    {
        /// <summary>
        /// Gets a value indicating whether the password is currently visible.
        /// </summary>
        /// <value>
        /// <c>true</c> if the password is visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsPasswordVisible { get; internal set; }
    }
}
