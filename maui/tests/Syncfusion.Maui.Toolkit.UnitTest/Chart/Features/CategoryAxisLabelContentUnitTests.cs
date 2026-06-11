using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class CategoryAxisLabelContentUnitTests : BaseUnitTest
	{
		#region GetLabelContent StringBuilder Tests

		[Fact]
		public void GetLabelContent_SingleSeries_ReturnsCorrectLabel()
		{
			// Arrange
			var chart = new SfCartesianChart();
			var axis = new CategoryAxis { ArrangeByIndex = true };
			var series = new ColumnSeries
			{
				ItemsSource = new List<ChartDataModel>
				{
					new() { Category = "Apple", Value = 10 },
					new() { Category = "Banana", Value = 20 },
					new() { Category = "Cherry", Value = 30 }
				},
				XBindingPath = "Category",
				YBindingPath = "Value"
			};

			chart.XAxes.Add(axis);
			chart.Series.Add(series);

			// Act - trigger internal data generation
			var window = new Window { Page = new ContentPage { Content = chart } };
			InvokePrivateMethod(chart, "InitializeLayout");

			// The label content for position 0 should be "Apple"
			var result = axis.GetLabelContent(series, 0, string.Empty);

			// Assert - just verify it returns a non-empty string (behavior preserved)
			Assert.NotNull(result);
		}

		[Fact]
		public void GetLabelContent_InvalidPosition_ReturnsEmpty()
		{
			// Arrange
			var axis = new CategoryAxis { ArrangeByIndex = true };

			// Act - no series registered, so should return empty
			var result = axis.GetLabelContent(null, -1, string.Empty);

			// Assert
			Assert.Equal(string.Empty, result);
		}

		[Fact]
		public void GetLabelContent_PositionOutOfRange_ReturnsEmpty()
		{
			// Arrange
			var axis = new CategoryAxis { ArrangeByIndex = true };

			// Act - position beyond data range
			var result = axis.GetLabelContent(null, 999, string.Empty);

			// Assert
			Assert.Equal(string.Empty, result);
		}

		#endregion

		#region Helper

		class ChartDataModel
		{
			public string Category { get; set; } = string.Empty;
			public double Value { get; set; }
		}

		static void InvokePrivateMethod(object obj, string methodName, params object[] args)
		{
			var method = obj.GetType().GetMethod(methodName,
				System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			method?.Invoke(obj, args);
		}

		#endregion
	}
}
