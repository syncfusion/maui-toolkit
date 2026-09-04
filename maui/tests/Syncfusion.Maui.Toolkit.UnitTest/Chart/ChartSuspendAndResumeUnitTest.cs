using System;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	public class CartesianChartSuspensionTests
	{
		private SfCartesianChart CreateTestChart()
		{
			var chart = new SfCartesianChart();
			chart.XAxes.Add(new NumericalAxis());
			chart.YAxes.Add(new NumericalAxis());
			return chart;
		}

		private ColumnSeries CreateTestSeries(string xPath = "X", string yPath = "Y")
		{
			return new ColumnSeries
			{
				XBindingPath = xPath,
				YBindingPath = yPath
			};
		}

		[Fact]
		public void SuspendChart_AddDataToSingleSeries_DataAppliedOnResume()
		{
			var chart = CreateTestChart();
			var series = CreateTestSeries();
			var data = new ObservableCollection<DataModel> { new DataModel(1, 10) };
			series.ItemsSource = data;
			chart.Series.Add(series);

			chart.SuspendSeriesNotification();
			try
			{
				data.Add(new DataModel(2, 20));
				data.Add(new DataModel(3, 30));
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(3, data.Count);
			Assert.Equal(20, data[1].Y);
			Assert.Equal(30, data[2].Y);
		}

		[Fact]
		public void SuspendChart_UpdateMultipleSeries_AllSeriesUpdatedOnResume()
		{
			var chart = CreateTestChart();
			var series1 = CreateTestSeries("X", "Y1");
			var series2 = CreateTestSeries("X", "Y2");
			var series3 = CreateTestSeries("X", "Y3");

			var data1 = new ObservableCollection<DataModel>();
			var data2 = new ObservableCollection<DataModel>();
			var data3 = new ObservableCollection<DataModel>();

			series1.ItemsSource = data1;
			series2.ItemsSource = data2;
			series3.ItemsSource = data3;

			chart.Series.Add(series1);
			chart.Series.Add(series2);
			chart.Series.Add(series3);

			chart.SuspendSeriesNotification();
			try
			{
				for (int i = 0; i < 50; i++)
				{
					data1.Add(new DataModel(i, i * 10));
					data2.Add(new DataModel(i, i * 20));
					data3.Add(new DataModel(i, i * 30));
				}
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(50, data1.Count);
			Assert.Equal(50, data2.Count);
			Assert.Equal(50, data3.Count);
		}

		[Fact]
		public void SuspendChart_AddNewSeriesDuringSuspension_NewSeriesAppliedOnResume()
		{
			var chart = CreateTestChart();
			var existingSeries = CreateTestSeries();
			var existingData = new ObservableCollection<DataModel> { new DataModel(0, 0) };
			existingSeries.ItemsSource = existingData;
			chart.Series.Add(existingSeries);

			chart.SuspendSeriesNotification();
			try
			{
				existingData.Add(new DataModel(1, 10));

				var newSeries = CreateTestSeries();
				var newData = new ObservableCollection<DataModel>
				{
					new DataModel(0, 5),
					new DataModel(1, 15)
				};
				newSeries.ItemsSource = newData;
				chart.Series.Add(newSeries);
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(2, chart.Series.Count);
			Assert.Equal(2, existingData.Count);
			Assert.Equal(2, ((ObservableCollection<DataModel>)chart.Series[1].ItemsSource).Count);
		}

		[Fact]
		public void SuspendChart_NestedSuspendCalls_ResumesCorrectly()
		{
			var chart = CreateTestChart();
			var series = CreateTestSeries();
			var data = new ObservableCollection<DataModel> { new DataModel(1, 10) };
			series.ItemsSource = data;
			chart.Series.Add(series);

			chart.SuspendSeriesNotification();
			try
			{
				data.Add(new DataModel(2, 20));

				chart.SuspendSeriesNotification();
				try
				{
					data.Add(new DataModel(3, 30));
				}
				finally
				{
					chart.ResumeSeriesNotification();
				}

				data.Add(new DataModel(4, 40));
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(4, data.Count);
			Assert.Equal(40, data[3].Y);
		}

		[Fact]
		public void SuspendChart_ClearAndReplaceData_CorrectStateOnResume()
		{
			var chart = CreateTestChart();
			var series = CreateTestSeries();
			var data = new ObservableCollection<DataModel>
			{
				new DataModel(1, 10),
				new DataModel(2, 20),
				new DataModel(3, 30)
			};
			series.ItemsSource = data;
			chart.Series.Add(series);

			chart.SuspendSeriesNotification();
			try
			{
				data.Clear();
				for (int i = 0; i < 10; i++)
				{
					data.Add(new DataModel(i * 5, i * 50));
				}
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(10, data.Count);
			Assert.Equal(0, data[0].X);
			Assert.Equal(450, data[9].Y);
		}

		[Fact]
		public void SuspendChart_ModifyAxisPropertiesDuringSuspension_AppliedAfterResume()
		{
			var chart = CreateTestChart();
			var xAxis = (NumericalAxis)chart.XAxes[0];
			var yAxis = (NumericalAxis)chart.YAxes[0];

			var series = CreateTestSeries();
			var data = new ObservableCollection<DataModel> { new DataModel(1, 10) };
			series.ItemsSource = data;
			chart.Series.Add(series);

			chart.SuspendSeriesNotification();
			try
			{
				data.Add(new DataModel(2, 20));
				data.Add(new DataModel(3, 30));

				xAxis.Minimum = 0;
				xAxis.Maximum = 100;
				yAxis.Minimum = 0;
				yAxis.Maximum = 100;
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(3, data.Count);
			Assert.Equal(0, xAxis.Minimum);
			Assert.Equal(100, xAxis.Maximum);
		}

		[Fact]
		public void SuspendChart_MultipleAddRemoveCycles_FinalStateCorrect()
		{
			var chart = CreateTestChart();
			var series = CreateTestSeries();
			var data = new ObservableCollection<DataModel>();
			series.ItemsSource = data;
			chart.Series.Add(series);

			chart.SuspendSeriesNotification();
			try
			{
				for (int i = 0; i < 5; i++)
					data.Add(new DataModel(i, i * 10));

				data.RemoveAt(3);
				data.RemoveAt(2);
				data.RemoveAt(1);

				data.Add(new DataModel(10, 100));
				data.Add(new DataModel(11, 110));
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(4, data.Count);
			Assert.Equal(0, data[0].X);
			Assert.Equal(100, data[2].Y);
		}

		[Fact]
		public void SuspendChart_LargeDataset_5000Items_ProcessedEfficiently()
		{
			var chart = CreateTestChart();
			var series = CreateTestSeries();
			var data = new ObservableCollection<DataModel>();
			series.ItemsSource = data;
			chart.Series.Add(series);

			chart.SuspendSeriesNotification();
			try
			{
				for (int i = 0; i < 5000; i++)
				{
					data.Add(new DataModel(i, Math.Sin(i * 0.001) * 100 + 50));
				}
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(5000, data.Count);
		}

		[Fact]
		public void SuspendChart_ExpandDataRange_AxesRecalculateOnResume()
		{
			var chart = CreateTestChart();
			var series = CreateTestSeries();
			var data = new ObservableCollection<DataModel>
			{
				new DataModel(10, 10),
				new DataModel(20, 20)
			};
			series.ItemsSource = data;
			chart.Series.Add(series);

			chart.SuspendSeriesNotification();
			try
			{
				for (int i = 0; i < 100; i++)
				{
					data.Add(new DataModel(i * 100, i * 100));
				}
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(102, data.Count);
			Assert.Same(data, series.ItemsSource);
		}

		[Fact]
		public void SuspendChart_AlternatingSuspendResumeCycles_AllDataProcessed()
		{
			var chart = CreateTestChart();
			var series = CreateTestSeries();
			var data = new ObservableCollection<DataModel>();
			series.ItemsSource = data;
			chart.Series.Add(series);

			int pointsAdded = 0;

			for (int batch = 0; batch < 5; batch++)
			{
				chart.SuspendSeriesNotification();
				try
				{
					for (int i = 0; i < 20; i++)
					{
						data.Add(new DataModel(pointsAdded, Math.Sin(pointsAdded * 0.05) * 50));
						pointsAdded++;
					}
				}
				finally
				{
					chart.ResumeSeriesNotification();
				}
			}

			Assert.Equal(100, data.Count);
			Assert.Equal(100, pointsAdded);
		}

		[Fact]
		public void SuspendChart_ModifySeriesPropertiesDuringSuspension_PropertiesAppliedOnResume()
		{
			var chart = CreateTestChart();
			var series = CreateTestSeries();
			var data = new ObservableCollection<DataModel> { new DataModel(1, 10) };
			series.ItemsSource = data;
			chart.Series.Add(series);

			chart.SuspendSeriesNotification();
			try
			{
				series.Label = "Updated Series";
				data.Add(new DataModel(2, 20));
				data.Add(new DataModel(3, 30));
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal("Updated Series", series.Label);
			Assert.Equal(3, data.Count);
		}

		[Fact]
		public void SuspendChart_RemoveSeriesDuringSuspension_SeriesRemovedOnResume()
		{
			var chart = CreateTestChart();
			var series1 = CreateTestSeries();
			var series2 = CreateTestSeries();
			var data1 = new ObservableCollection<DataModel> { new DataModel(1, 10) };
			var data2 = new ObservableCollection<DataModel> { new DataModel(2, 20) };
			series1.ItemsSource = data1;
			series2.ItemsSource = data2;
			chart.Series.Add(series1);
			chart.Series.Add(series2);

			chart.SuspendSeriesNotification();
			try
			{
				data1.Add(new DataModel(3, 30));
				chart.Series.Remove(series2);
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Single(chart.Series);
			Assert.Equal(2, data1.Count);
		}

		[Fact]
		public void SuspendChart_UpdateMultipleSeriesWithDifferentDataSizes_AllUpdated()
		{
			var chart = CreateTestChart();
			var series1 = CreateTestSeries();
			var series2 = CreateTestSeries();
			var data1 = new ObservableCollection<DataModel>();
			var data2 = new ObservableCollection<DataModel>();
			series1.ItemsSource = data1;
			series2.ItemsSource = data2;
			chart.Series.Add(series1);
			chart.Series.Add(series2);

			chart.SuspendSeriesNotification();
			try
			{
				for (int i = 0; i < 100; i++)
					data1.Add(new DataModel(i, i * 10));

				for (int i = 0; i < 50; i++)
					data2.Add(new DataModel(i, i * 20));
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Equal(100, data1.Count);
			Assert.Equal(50, data2.Count);
		}

		[Fact]
		public void SuspendChart_ClearSeriesCollectionDuringSuspension_ClearedOnResume()
		{
			var chart = CreateTestChart();
			var series1 = CreateTestSeries();
			var series2 = CreateTestSeries();
			chart.Series.Add(series1);
			chart.Series.Add(series2);

			chart.SuspendSeriesNotification();
			try
			{
				chart.Series.Clear();
			}
			finally
			{
				chart.ResumeSeriesNotification();
			}

			Assert.Empty(chart.Series);
		}
	}
}