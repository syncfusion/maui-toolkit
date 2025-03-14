using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit.OtpInput
{
    /// <summary>
    /// Specifies the style variants for the SfOtpInput control.
    /// </summary>
    public enum OtpInputStyle
    {
        /// <summary>
        /// Represents the style of the Otp input where the input is filled.
        /// </summary>
        Filled,
        /// <summary>
        /// Represents the style of the Otp input where the input is outlined.
        /// </summary>
        Outlined,
        /// <summary>
        /// Represents the style of the Otp input where the input is underlined. 
        /// </summary>
        Underlined
    }

    /// <summary>
    /// Specifies the types of input for the SfOtpInput control.
    /// </summary>
    public enum OtpInputType
    {
        /// <summary>
        /// Represents number input for the SfOtpInput control.
        /// </summary>  
        Number,

        /// <summary>
        /// Represents text input for the SfOtpInput control.
        /// </summary>
        Text,

        /// <summary>
        /// Represents password input for the SfOtpInput control.
        /// </summary>
        Password
    }

    /// <summary>
    /// Defines the visual state of the OTP input control.
    /// </summary>
    public enum OtpInputState
    {
        /// <summary>
        /// The default state of the OTP input field.
        /// </summary>  
        Default,

        /// <summary>
        /// Indicates the OTP input is successfully verified.
        /// </summary>  
        Success,

        /// <summary>
        /// Indicates an error state for the OTP input field, such as an invalid OTP.
        /// </summary>
        Error,

        /// <summary>
        /// Indicates a cautionary state for the OTP input field.
        /// </summary>
        Warning
    }
}
