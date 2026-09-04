using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
    public class CartesianSeriesSuspensionTests
    {
        private SfCartesianChart CreateTestChart()
        {
            var chart = new SfCartesianChart();
            chart.XAxes.Add(new NumericalAxis());
            chart.YAxes.Add(new NumericalAxis());
            return chart;
        }

        private ColumnSeries CreateTestSeries()
        {
            var series = new ColumnSeries();
            series.XBindingPath = "X";
            series.YBindingPath = "Y";
            return series;
        }

        [Fact]
        public void SuspendResume_SingleItemAdd_ItemsApplied()
        {
            var chart = CreateTestChart();
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel> 
            { 
                new DataModel(1, 10) 
            };
            series.ItemsSource = data;
            chart.Series.Add(series);

            series.SuspendNotification();
            data.Add(new DataModel(2, 20));
            series.ResumeNotification();

            Assert.Equal(2, data.Count);
            Assert.Same(data, series.ItemsSource);
        }

        [Fact]
        public void BatchAdd_WithoutSuspension_AllItemsAdded()
        {
            // Arrange
            var chart = CreateTestChart();
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel>();
            series.ItemsSource = data;
            chart.Series.Add(series);

            for (int i = 0; i < 100; i++)
            {
                data.Add(new DataModel(i, i * 10));
            }

            Assert.Equal(100, data.Count);
            Assert.Same(data, series.ItemsSource);
        }

        [Fact]
        public void BatchAdd_WithSuspension_AllItemsAddedEfficiently()
        {
            var chart = CreateTestChart();
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel>();
            series.ItemsSource = data;
            chart.Series.Add(series);
            series.SuspendNotification();
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    data.Add(new DataModel(i, i * 10));
                }
            }
            finally
            {
                series.ResumeNotification();
            }

            Assert.Equal(100, data.Count);
            Assert.Same(data, series.ItemsSource);
        }

        [Fact]
        public void BatchAdd_LargeBatch_1000Items_ProcessedCorrectly()
        {
            var chart = CreateTestChart();
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel>();
            series.ItemsSource = data;
            chart.Series.Add(series);

            series.SuspendNotification();
            try
            {
                for (int i = 0; i < 1000; i++)
                {
                    data.Add(new DataModel(i, Math.Sin(i * 0.01) * 100));
                }
            }
            finally
            {
                series.ResumeNotification();
            }

            Assert.Equal(1000, data.Count);
            Assert.Same(data, series.ItemsSource);
        }

        [Fact]
        public void MixedOperations_DuringSuspension_FinalStateCorrect()
        {
            var chart = CreateTestChart();
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel>
            {
                new DataModel(1, 10),
                new DataModel(2, 20),
                new DataModel(3, 30),
                new DataModel(4, 40),
                new DataModel(5, 50)
            };
            series.ItemsSource = data;
            chart.Series.Add(series);

            series.SuspendNotification();
            try
            {
                data.Add(new DataModel(6, 60));      // Add
                data.RemoveAt(0);                     // Remove at index 0
                data[2] = new DataModel(3, 35);       // Replace at index 2
                data.Add(new DataModel(7, 70));       // Add
            }
            finally
            {
                series.ResumeNotification();
            }

            Assert.Equal(6, data.Count);  // 5 original - 1 removed + 2 added = 6
            Assert.Same(data, series.ItemsSource);
            Assert.Equal(20, data[0].Y);  // Original index 1 is now 0
            Assert.Equal(35, data[2].Y);  // Replaced value
            Assert.Equal(70, data[5].Y);  // Last added item
        }

        [Fact]
        public void Reset_DuringSuspension_ResetsAllPriorOperations()
        {
            var chart = CreateTestChart();
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel>
            {
                new DataModel(1, 10),
                new DataModel(2, 20)
            };
            series.ItemsSource = data;
            chart.Series.Add(series);

            // Act
            series.SuspendNotification();
            try
            {
                data.Add(new DataModel(3, 30));       // Add 1
                data.Add(new DataModel(4, 40));       // Add 2
                data.Clear();                         // Reset (should supersede prior adds)
                data.Add(new DataModel(5, 50));       // Add after reset
            }
            finally
            {
                series.ResumeNotification();
            }

            Assert.Single(data); 
            Assert.Equal(50, data[0].Y);
        }

        [Fact]
        public void Resume_WithoutSuspend_IsNoOp()
        {
            var chart = CreateTestChart();
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel> { new DataModel(1, 10) };
            series.ItemsSource = data;
            chart.Series.Add(series);

            series.ResumeNotification();
            Assert.Single(data);
        }

        [Fact]
        public void ExceptionDuringResume_SeriesRecoverable()
        {
            var chart = CreateTestChart();
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel>();
            series.ItemsSource = data;
            chart.Series.Add(series);

            series.SuspendNotification();
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    data.Add(new DataModel(i, i * 10));
                }
            }
            catch
            {
            }
            finally
            {
                series.ResumeNotification();
            }

            data.Add(new DataModel(100, 1000));
            Assert.Equal(11, data.Count);
        }

        [Fact]
        public void RealtimeStreaming_WithPeriodicSuspension_DataCorrect()
        {
            var chart = CreateTestChart();
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel>();
            series.ItemsSource = data;
            chart.Series.Add(series);

            int totalPoints = 0;

            series.SuspendNotification();
            for (int i = 0; i < 100; i++)
            {
                data.Add(new DataModel(totalPoints++, Math.Sin(totalPoints * 0.01) * 50));
            }
            series.ResumeNotification();
            series.SuspendNotification();
            for (int i = 0; i < 100; i++)
            {
                data.Add(new DataModel(totalPoints++, Math.Cos(totalPoints * 0.01) * 50));
            }
            series.ResumeNotification();

            series.SuspendNotification();
            for (int i = 0; i < 100; i++)
            {
                data.Add(new DataModel(totalPoints++, Math.Tan(totalPoints * 0.005) * 30));
            }
            series.ResumeNotification();

            Assert.Equal(300, data.Count);
            Assert.Same(data, series.ItemsSource);
        }

        [Fact]
        public void AxisRangeRecalculation_OccursOnceAfterResume()
        {
            var chart = CreateTestChart();
            var xAxis = (NumericalAxis)chart.XAxes[0];
            var yAxis = (NumericalAxis)chart.YAxes[0];
            
            var series = CreateTestSeries();
            var data = new ObservableCollection<DataModel>
            {
                new DataModel(0, 0),
                new DataModel(10, 10)
            };
            series.ItemsSource = data;
            chart.Series.Add(series);

            series.SuspendNotification();
            try
            {
                for (int i = 0; i < 50; i++)
                {
                    data.Add(new DataModel(i * 10, i * 20));
                }
            }
            finally
            {
                series.ResumeNotification();
            }

            Assert.Equal(52, data.Count);
            Assert.Same(data, series.ItemsSource);
        }
    }

    public class DataModel
    {
        public DataModel(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}
