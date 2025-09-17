#if IOS || MACCATALYST
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Syncfusion.Maui.Toolkit.OtpInput
{
    /// <summary>
    /// Custom MauiTextField that provides backspace detection even on empty fields for iOS/macOS platforms.
    /// This class overrides the DeleteBackward method to allow backspace handling in empty entry fields.
    /// </summary>
    public class CustomMauiTextField : MauiTextField
    {
        /// <summary>
        /// Action to be invoked when the delete/backspace key is pressed.
        /// This allows external handlers to respond to backspace events even when the field is empty.
        /// </summary>
        public Action? OnDeleteBackward { get; set; }

        /// <summary>
        /// Overrides the default DeleteBackward behavior to provide custom backspace handling.
        /// This method is called whenever the backspace/delete key is pressed, regardless of whether
        /// the text field contains content or is empty.
        /// </summary>
        public override void DeleteBackward()
        {
            // Invoke the custom backspace handler if available
            OnDeleteBackward?.Invoke();
            
            // Call the base implementation to maintain standard behavior
            base.DeleteBackward();
        }
    }

    /// <summary>
    /// Custom EntryHandler for OTPEntry controls on iOS/macOS platforms that creates CustomMauiTextField instances
    /// instead of the default MauiTextField. This enables better backspace handling in OTP scenarios.
    /// </summary>
    public class OTPEntryiOSHandler : EntryHandler
    {
        /// <summary>
        /// Creates the platform-specific view (CustomMauiTextField) for the OTPEntry control.
        /// This overrides the default behavior to use our custom text field that supports
        /// enhanced backspace detection even when the field is empty.
        /// </summary>
        /// <returns>A new instance of CustomMauiTextField configured for OTP input scenarios.</returns>
        protected override MauiTextField CreatePlatformView()
        {
            var customTextField = new CustomMauiTextField
            {
                BorderStyle = UITextBorderStyle.None,
                BackgroundColor = UIColor.Clear,
                TextAlignment = UITextAlignment.Center,
                VerticalAlignment = UIControlContentVerticalAlignment.Center,
                AutocorrectionType = UITextAutocorrectionType.No,
                SpellCheckingType = UITextSpellCheckingType.No
            };

            // Set up the custom backspace handler
            customTextField.OnDeleteBackward = () =>
            {
                // Find the parent OTPInput control and trigger backspace handling
                if (VirtualView is OTPEntry otpEntry)
                {
                    HandleBackspaceForOTPEntry(otpEntry);
                }
            };

            return customTextField;
        }

        /// <summary>
        /// Handles backspace events for OTP entries by finding the parent SfOtpInput control
        /// and triggering its backspace handling logic with the correct entry context.
        /// </summary>
        /// <param name="otpEntry">The OTPEntry that triggered the backspace event.</param>
        private void HandleBackspaceForOTPEntry(OTPEntry otpEntry)
        {
            // Find the parent SfOtpInput control
            var parent = otpEntry.Parent;
            while (parent != null)
            {
                if (parent is SfOtpInput sfOtpInput)
                {
                    sfOtpInput.HandleKeyPress("Back");  
                    break;
                }
                parent = parent.Parent;
            }
        }
    }
}
#endif
