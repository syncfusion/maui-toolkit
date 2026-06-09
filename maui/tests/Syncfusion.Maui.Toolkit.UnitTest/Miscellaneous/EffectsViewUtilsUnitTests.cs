using Syncfusion.Maui.Toolkit.EffectsView;
using Xunit;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class EffectsViewUtilsUnitTests
	{
		#region GetAllItems Tests

		[Fact]
		public void GetAllItems_ReturnsNone_WhenEnumIsNone()
		{
			var result = SfEffects.None.GetAllItems().ToList();

			Assert.Single(result);
			Assert.Equal(SfEffects.None, result[0]);
		}

		[Fact]
		public void GetAllItems_ReturnsSingleFlag_WhenSingleFlagSet()
		{
			var result = SfEffects.Ripple.GetAllItems().ToList();

			Assert.Single(result);
			Assert.Equal(SfEffects.Ripple, result[0]);
		}

		[Fact]
		public void GetAllItems_ReturnsMultipleFlags_WhenCombinedFlags()
		{
			var effects = SfEffects.Ripple | SfEffects.Highlight;
			var result = effects.GetAllItems().ToList();

			Assert.Contains(SfEffects.Ripple, result);
			Assert.Contains(SfEffects.Highlight, result);
			Assert.DoesNotContain(SfEffects.None, result);
		}

		#endregion

		#region GetAllAutoResetEffectsItems Tests

		[Fact]
		public void GetAllAutoResetEffectsItems_ReturnsNone_WhenEnumIsNone()
		{
			var result = AutoResetEffects.None.GetAllAutoResetEffectsItems().ToList();

			Assert.Single(result);
			Assert.Equal(AutoResetEffects.None, result[0]);
		}

		[Fact]
		public void GetAllAutoResetEffectsItems_ReturnsSingleFlag_WhenSingleFlagSet()
		{
			var result = AutoResetEffects.Ripple.GetAllAutoResetEffectsItems().ToList();

			Assert.Single(result);
			Assert.Equal(AutoResetEffects.Ripple, result[0]);
		}

		[Fact]
		public void GetAllAutoResetEffectsItems_ReturnsMultipleFlags_WhenCombinedFlags()
		{
			var effects = AutoResetEffects.Ripple | AutoResetEffects.Highlight;
			var result = effects.GetAllAutoResetEffectsItems().ToList();

			Assert.Contains(AutoResetEffects.Ripple, result);
			Assert.Contains(AutoResetEffects.Highlight, result);
			Assert.DoesNotContain(AutoResetEffects.None, result);
		}

		#endregion
	}
}
