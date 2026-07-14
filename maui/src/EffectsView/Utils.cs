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
		internal static IEnumerable<SfEffects> GetAllItems(this SfEffects targetEnum)
		{
			foreach (SfEffects value in Enum.GetValues<SfEffects>())
			{
				//// If the flag value of the enum is zero, then the HasFlag method always returns true. Hence, the None is returned only if the value and the target enum is equals to None.
				if (value != SfEffects.None && targetEnum.HasFlag(value))
				{
					yield return value;
				}
				else if (value == SfEffects.None && targetEnum == SfEffects.None)
				{
					yield return value;
				}
			}
		}

		internal static IEnumerable<AutoResetEffects> GetAllAutoResetEffectsItems(this AutoResetEffects targetEnum)
		{
			foreach (AutoResetEffects value in Enum.GetValues<AutoResetEffects>())
			{
				//// If the flag value of the enum is zero, then the HasFlag method always returns true. Hence, the None is returned only if the value and the target enum is equals to None.
				if (value != AutoResetEffects.None && targetEnum.HasFlag(value))
				{
					yield return value;
				}
				else if (value == AutoResetEffects.None && targetEnum == AutoResetEffects.None)
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
