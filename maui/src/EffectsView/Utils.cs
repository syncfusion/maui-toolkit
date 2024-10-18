using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.EffectsView
{
    /// <summary>
    /// Flagged Enum extension.
    /// </summary>
    internal static partial class FlaggedEnumExt
    {
        /// <summary>
        /// Gets the all available items in given enum.
        /// </summary>
        /// <param name="targetEnum">Target enum.</param>
        /// <returns>Available enum values.</returns>
        internal static IEnumerable<Enum> GetAllItems(this Enum targetEnum)
        {
            foreach (Enum value in Enum.GetValues(targetEnum.GetType()))
            {
                //// If the flag value of the enum is zero, then the HasFlag method always returns true. Hence, the None is returned only if the value and the target enum is equals to None.
                if (value.ToString() != "None" && targetEnum.HasFlag(value))
                {
                    yield return value;
                }
                else if (value.ToString() == "None" && targetEnum.Equals(value))
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Getting the complement of target from source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <returns>The complement value.</returns>
        internal static SfEffects ComplementsOf(this SfEffects target, SfEffects source)
        {
            return target &= ~source;
        }
    }
}
