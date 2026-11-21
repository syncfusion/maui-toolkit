namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
	/// <summary>
	/// Provides theme-aware brush helpers for chart series fills using palette gradients.
	/// </summary>
	public class ChartBrushes
	{
		public static void ApplyGradientToggle(
		Syncfusion.Maui.Toolkit.Charts.ChartSeries series,
		ResourceDictionary? scope,
		int paletteIndex,
		bool useGradient,
		Dictionary<int, Brush?> originalFillCache,
		int cacheIndex)
		{
			if (series == null)
				return;

			if (useGradient)
			{
				// Cache the current fill if not already cached
				if (!originalFillCache.ContainsKey(cacheIndex))
					originalFillCache[cacheIndex] = series.Fill;

				// Apply theme-aware gradient from PaletteBrushes
				series.Fill = GetPaletteGradientBrush(scope, paletteIndex);
			}
			else
			{
				// Restore the cached fill or the solid palette brush
				if (originalFillCache.TryGetValue(cacheIndex, out var cachedFill) && cachedFill != null)
					series.Fill = cachedFill;
				else
					series.Fill = GetPaletteSolidBrush(scope, paletteIndex);
			}
		}

		/// <summary>
		/// Applies gradient toggle for line-based series (Line, Spline, StepLine) with dark-center gradients.
		/// </summary>
		public static void ApplyLineGradientToggle(
			Syncfusion.Maui.Toolkit.Charts.ChartSeries series,
			ResourceDictionary? scope,
			int paletteIndex,
			bool useGradient,
			Dictionary<int, Brush?> originalFillCache,
			int cacheIndex)
		{
			if (series == null)
				return;

			if (useGradient)
			{
				// Cache the current fill if not already cached
				if (!originalFillCache.ContainsKey(cacheIndex))
					originalFillCache[cacheIndex] = series.Fill;

				// Apply line-specific gradient with dark center
				series.Fill = GetLineGradientBrush(scope, paletteIndex);
			}
			else
			{
				// Restore the cached fill or the solid palette brush
				if (originalFillCache.TryGetValue(cacheIndex, out var cachedFill) && cachedFill != null)
					series.Fill = cachedFill;
				else
					series.Fill = GetPaletteSolidBrush(scope, paletteIndex);
			}
		}

		/// <summary>
		/// Returns the per-series gradient brush from the gradient palette for slot <paramref name="index"/> (0..4), theme-aware.
		/// Retrieves from GradientPaletteBrushesLight or GradientPaletteBrushesDark collections (for column/range charts).
		/// </summary>
		public static Brush GetPaletteGradientBrush(ResourceDictionary? scope, int index)
		{
			var key = Application.Current?.RequestedTheme == AppTheme.Dark
				? "GradientPaletteBrushesDark"
				: "GradientPaletteBrushesLight";
			return GetFromPalette(scope, key, index, fallback: new SolidColorBrush(Color.FromArgb("#3068F7")));
		}

		/// <summary>
		/// Returns the per-series gradient brush for line-based series (Line, Spline, StepLine) with dark center and light sides.
		/// Retrieves from GradientPaletteBrushesLightLine or GradientPaletteBrushesDarkLine collections.
		/// </summary>
		public static Brush GetLineGradientBrush(ResourceDictionary? scope, int index)
		{
			var key = Application.Current?.RequestedTheme == AppTheme.Dark
				? "GradientPaletteBrushesDarkLine"
				: "GradientPaletteBrushesLightLine";
			return GetFromPalette(scope, key, index, fallback: new SolidColorBrush(Color.FromArgb("#3068F7")));
		}

		/// <summary>
		/// Returns the per-series solid brush from the palette for slot <paramref name="index"/> (0..4), theme-aware.
		/// Note: With the updated palette, this retrieves gradient brushes. For solid-only fallback, use the solid color directly.
		/// </summary>
		public static Brush GetPaletteSolidBrush(ResourceDictionary? scope, int index)
		{
			var key = Application.Current?.RequestedTheme == AppTheme.Dark
				? "PaletteBrushesDark"
				: "PaletteBrushesLight";
			return GetFromPalette(scope, key, index, fallback: new SolidColorBrush(Color.FromArgb("#3068F7")));
		}

		private static Brush GetFromPalette(ResourceDictionary? scope, string key, int index, Brush fallback)
		{
			System.Collections.IList? list = null;
			if (scope != null && scope.TryGetValue(key, out var scoped))
				list = scoped as System.Collections.IList;
			if (list == null && Application.Current?.Resources.TryGetValue(key, out var appVal) == true)
				list = appVal as System.Collections.IList;

			if (list != null && index >= 0 && index < list.Count && list[index] is Brush b)
				return b;
			return fallback;
		}
	}
}
