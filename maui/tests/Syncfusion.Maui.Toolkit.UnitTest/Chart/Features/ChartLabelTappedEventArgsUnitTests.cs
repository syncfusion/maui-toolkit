using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.Charts;
using Xunit;

namespace Syncfusion.Maui.Toolkit.UnitTest.Charts
{
	/// <summary>
	/// Unit tests for AxisLabelTappedEventArgs and DataLabelTappedEventArgs
	/// </summary>
	public class ChartLabelTappedEventArgsUnitTests
	{

		[Fact]
		public void AxisLabelTappedEventArgs_Constructor_WithValidParameters_CreatesInstance()
		{
			// Arrange
			var axis = new NumericalAxis { Name = "XAxis" };
			var axisLabel = new ChartAxisLabel(0, "10");
			var position = new PointF(100, 50);

			// Act
			var args = new AxisLabelTappedEventArgs(axis, axisLabel, position);

			// Assert
			Assert.NotNull(args);
			Assert.Equal(axis, args.Axis);
			Assert.Equal(axisLabel, args.AxisLabel);
			Assert.Equal(position, args.Position);
		}

		[Fact]
		public void AxisLabelTappedEventArgs_Axis_Property_ReturnsCorrectAxis()
		{
			// Arrange
			var axis = new NumericalAxis { Name = "YAxis" };
			var axisLabel = new ChartAxisLabel(1, "50");
			var position = new PointF(150, 75);
			var args = new AxisLabelTappedEventArgs(axis, axisLabel, position);

			// Act
			var resultAxis = args.Axis;

			// Assert
			Assert.NotNull(resultAxis);
			Assert.Equal("YAxis", resultAxis.Name);
		}

		[Fact]
		public void AxisLabelTappedEventArgs_AxisLabel_Property_ReturnsCorrectLabel()
		{
			// Arrange
			var axis = new NumericalAxis();
			var axisLabel = new ChartAxisLabel(2, "100");
			var position = new PointF(200, 100);
			var args = new AxisLabelTappedEventArgs(axis, axisLabel, position);

			// Act
			var resultLabel = args.AxisLabel;

			// Assert
			Assert.NotNull(resultLabel);
			Assert.Equal("100", resultLabel.Content);
		}

		[Theory]
		[InlineData(0f, 0f)]
		[InlineData(100f, 50f)]
		[InlineData(500f, 300f)]
		[InlineData(float.MaxValue, float.MaxValue)]
		[InlineData(float.MinValue, float.MinValue)]
		public void AxisLabelTappedEventArgs_Position_Property_ReturnsCorrectPosition(float x, float y)
		{
			// Arrange
			var axis = new NumericalAxis();
			var axisLabel = new ChartAxisLabel(0, "Label");
			var position = new PointF(x, y);
			var args = new AxisLabelTappedEventArgs(axis, axisLabel, position);

			// Act
			var resultPosition = args.Position;

			// Assert
			Assert.Equal(x, resultPosition.X);
			Assert.Equal(y, resultPosition.Y);
		}

		[Fact]
		public void AxisLabelTappedEventArgs_WithCategoryAxis_ReturnsExpectedAxis()
		{
			// Arrange
			var axis = new CategoryAxis { Name = "CategoryXAxis" };
			var axisLabel = new ChartAxisLabel(0, "Jan");
			var position = new PointF(50, 25);

			// Act
			var args = new AxisLabelTappedEventArgs(axis, axisLabel, position);

			// Assert
			Assert.IsType<CategoryAxis>(args.Axis);
			Assert.Equal("CategoryXAxis", args.Axis.Name);
		}

		[Fact]
		public void AxisLabelTappedEventArgs_WithDateTimeAxis_ReturnsExpectedAxis()
		{
			// Arrange
			var axis = new DateTimeAxis { Name = "DateTimeAxis" };
			var axisLabel = new ChartAxisLabel(0, "01/01/2023");
			var position = new PointF(75, 40);

			// Act
			var args = new AxisLabelTappedEventArgs(axis, axisLabel, position);

			// Assert
			Assert.IsType<DateTimeAxis>(args.Axis);
			Assert.Equal("DateTimeAxis", args.Axis.Name);
		}

		[Fact]
		public void AxisLabelTappedEventArgs_MultipleInstances_AreIndependent()
		{
			// Arrange
			var axis1 = new NumericalAxis { Name = "Axis1" };
			var axis2 = new NumericalAxis { Name = "Axis2" };
			var label1 = new ChartAxisLabel(0, "Label1");
			var label2 = new ChartAxisLabel(1, "Label2");
			var position1 = new PointF(100, 50);
			var position2 = new PointF(200, 100);

			// Act
			var args1 = new AxisLabelTappedEventArgs(axis1, label1, position1);
			var args2 = new AxisLabelTappedEventArgs(axis2, label2, position2);

			// Assert
			Assert.NotEqual(args1.Axis.Name, args2.Axis.Name);
			Assert.NotEqual(args1.AxisLabel.Content, args2.AxisLabel.Content);
			Assert.NotEqual(args1.Position, args2.Position);
		}

		[Theory]
		[InlineData(0, "0")]
		[InlineData(5, "5")]
		[InlineData(10.5, "10.5")]
		[InlineData(double.MaxValue, "MaxValue")]
		public void AxisLabelTappedEventArgs_AxisLabel_WithDifferentPositions_CreatesInstance(double position, string content)
		{
			// Arrange
			var axis = new NumericalAxis();
			var axisLabel = new ChartAxisLabel(position, content);
			var tapPosition = new PointF(100, 50);

			// Act
			var args = new AxisLabelTappedEventArgs(axis, axisLabel, tapPosition);

			// Assert
			Assert.NotNull(args);
			Assert.Equal(content, args.AxisLabel.Content);
		}

		[Theory]
		[InlineData(null)]
		public void AxisLabelTappedEventArgs_WithNullContent_CreatesInstance(object? value)
		{
			// Arrange
			var axis = new NumericalAxis();
			value = value ?? string.Empty;
			var axisLabel = new ChartAxisLabel(0, value);
			var position = new PointF(100, 50);

			// Act
			var args = new AxisLabelTappedEventArgs(axis, axisLabel, position);

			// Assert
			Assert.NotNull(args);
			Assert.Equal(value, args.AxisLabel.Content);
		}

		[Fact]
		public void AxisLabelTappedEventArgs_EventArgs_InheritsFromEventArgs()
		{
			// Arrange
			var axis = new NumericalAxis();
			var axisLabel = new ChartAxisLabel(0, "Label");
			var position = new PointF(100, 50);

			// Act
			var args = new AxisLabelTappedEventArgs(axis, axisLabel, position);

			// Assert
			Assert.IsAssignableFrom<EventArgs>(args);
		}
	}

	public class DataLabelTappedEventArgsUnitTests
	{
		[Fact]
		public void DataLabelTappedEventArgs_Constructor_WithValidParameters_CreatesInstance()
		{
			// Arrange
			var series = new ColumnSeries();
			int dataIndex = 5;
			var dataItem = new { Value = 100 };
			var position = new PointF(150, 200);

			// Act
			var args = new DataLabelTappedEventArgs(series, dataIndex, dataItem, position);

			// Assert
			Assert.NotNull(args);
			Assert.Equal(series, args.Series);
			Assert.Equal(dataIndex, args.DataIndex);
			Assert.Equal(dataItem, args.DataItem);
			Assert.Equal(position, args.Position);
			Assert.Null(args.Segment);
		}

		[Fact]
		public void DataLabelTappedEventArgs_Constructor_WithNullDataItem_CreatesInstance()
		{
			// Arrange
			var series = new ColumnSeries();
			int dataIndex = 5;
			object? nullDataItem = null;
			var position = new PointF(150, 200);

			// Act
			var args = new DataLabelTappedEventArgs(series, dataIndex, nullDataItem, position);

			// Assert
			Assert.NotNull(args);
			Assert.Null(args.DataItem);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(100)]
		[InlineData(int.MaxValue)]
		public void DataLabelTappedEventArgs_DataIndex_WithValidValues_CreatesInstance(int dataIndex)
		{
			// Arrange
			var series = new ColumnSeries();
			var dataItem = new { Value = 100 };
			var position = new PointF(150, 200);

			// Act
			var args = new DataLabelTappedEventArgs(series, dataIndex, dataItem, position);

			// Assert
			Assert.Equal(dataIndex, args.DataIndex);
		}

		[Fact]
		public void DataLabelTappedEventArgs_Series_Property_ReturnsCorrectSeries()
		{
			// Arrange
			var series = new ColumnSeries { Label = "Sales" };
			int dataIndex = 5;
			var dataItem = new { Value = 100 };
			var position = new PointF(150, 200);
			var args = new DataLabelTappedEventArgs(series, dataIndex, dataItem, position);

			// Act
			var resultSeries = args.Series;

			// Assert
			Assert.NotNull(resultSeries);
		}

		[Fact]
		public void DataLabelTappedEventArgs_DataIndex_Property_ReturnsCorrectIndex()
		{
			// Arrange
			var series = new LineSeries();
			int expectedIndex = 42;
			var dataItem = new { Value = 100 };
			var position = new PointF(150, 200);
			var args = new DataLabelTappedEventArgs(series, expectedIndex, dataItem, position);

			// Act
			var resultIndex = args.DataIndex;

			// Assert
			Assert.Equal(expectedIndex, resultIndex);
		}

		[Fact]
		public void DataLabelTappedEventArgs_DataItem_Property_ReturnsCorrectItem()
		{
			// Arrange
			var series = new ColumnSeries();
			int dataIndex = 5;
			var dataItem = new { Value = 500, Category = "A" };
			var position = new PointF(150, 200);
			var args = new DataLabelTappedEventArgs(series, dataIndex, dataItem, position);

			// Act
			var resultItem = args.DataItem;

			// Assert
			Assert.NotNull(resultItem);
			Assert.Equal(dataItem, resultItem);
		}

		[Theory]
		[InlineData(0f, 0f)]
		[InlineData(100f, 50f)]
		[InlineData(500f, 300f)]
		[InlineData(float.MaxValue, float.MaxValue)]
		[InlineData(float.MinValue, float.MinValue)]
		public void DataLabelTappedEventArgs_Position_Property_ReturnsCorrectPosition(float x, float y)
		{
			// Arrange
			var series = new ColumnSeries();
			int dataIndex = 5;
			var dataItem = new { Value = 100 };
			var position = new PointF(x, y);
			var args = new DataLabelTappedEventArgs(series, dataIndex, dataItem, position);

			// Act
			var resultPosition = args.Position;

			// Assert
			Assert.Equal(x, resultPosition.X);
			Assert.Equal(y, resultPosition.Y);
		}

		[Fact]
		public void DataLabelTappedEventArgs_Constructor_WithNullSegment_CreatesInstance()
		{
			// Arrange
			var series = new ColumnSeries();
			int dataIndex = 5;
			var dataItem = new { Value = 100 };
			var position = new PointF(150, 200);
			ChartSegment? nullSegment = null;

			// Act
			var args = new DataLabelTappedEventArgs(series, dataIndex, dataItem, position, nullSegment);

			// Assert
			Assert.NotNull(args);
			Assert.Null(args.Segment);
		}

		[Fact]
		public void DataLabelTappedEventArgs_WithLineSeries_ReturnsExpectedSeries()
		{
			// Arrange
			var series = new LineSeries { Label = "LineSeries1" };
			int dataIndex = 10;
			var dataItem = new { X = 1, Y = 100 };
			var position = new PointF(200, 250);

			// Act
			var args = new DataLabelTappedEventArgs(series, dataIndex, dataItem, position);

			// Assert
			Assert.IsType<LineSeries>(args.Series);
		}

		[Fact]
		public void DataLabelTappedEventArgs_MultipleInstances_AreIndependent()
		{
			// Arrange
			var series1 = new ColumnSeries { Label = "Series1" };
			var series2 = new ColumnSeries { Label = "Series2" };
			var item1 = new { Value = 100 };
			var item2 = new { Value = 200 };
			var position1 = new PointF(100, 150);
			var position2 = new PointF(200, 250);

			// Act
			var args1 = new DataLabelTappedEventArgs(series1, 5, item1, position1);
			var args2 = new DataLabelTappedEventArgs(series2, 10, item2, position2);

			// Assert
			Assert.NotEqual(args1.DataIndex, args2.DataIndex);
			Assert.NotEqual(args1.Position, args2.Position);
		}

		[Fact]
		public void DataLabelTappedEventArgs_ZeroDataIndex_IsValid()
		{
			// Arrange
			var series = new ColumnSeries();
			int dataIndex = 0;
			var dataItem = new { Value = 100 };
			var position = new PointF(150, 200);

			// Act
			var args = new DataLabelTappedEventArgs(series, dataIndex, dataItem, position);

			// Assert
			Assert.Equal(0, args.DataIndex);
		}

		[Fact]
		public void DataLabelTappedEventArgs_WithComplexDataItem_ReturnsCorrectItem()
		{
			// Arrange
			var series = new ColumnSeries();
			int dataIndex = 5;
			var complexItem = new
			{
				Id = 1,
				Name = "Product A",
				Value = 500,
				Category = "Electronics",
				Timestamp = DateTime.Now
			};
			var position = new PointF(150, 200);

			// Act
			var args = new DataLabelTappedEventArgs(series, dataIndex, complexItem, position);

			// Assert
			Assert.NotNull(args.DataItem);
			Assert.Equal(complexItem, args.DataItem);
		}

		[Fact]
		public void DataLabelTappedEventArgs_SegmentProperty_WithoutSegment_IsNull()
		{
			// Arrange
			var series = new ColumnSeries();
			int dataIndex = 5;
			var dataItem = new { Value = 100 };
			var position = new PointF(150, 200);

			// Act
			var args = new DataLabelTappedEventArgs(series, dataIndex, dataItem, position);

			// Assert
			Assert.Null(args.Segment);
		}

		[Fact]
		public void DataLabelTappedEventArgs_EventArgs_InheritsFromEventArgs()
		{
			// Arrange
			var series = new ColumnSeries();
			var args = new DataLabelTappedEventArgs(series, 0, null, new PointF(0, 0));

			// Assert
			Assert.IsAssignableFrom<EventArgs>(args);
		}

	}
}
