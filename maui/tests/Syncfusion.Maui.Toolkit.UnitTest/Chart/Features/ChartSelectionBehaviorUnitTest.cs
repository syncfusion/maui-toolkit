using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
	public class ChartSelectionBehaviorUnitTest : BaseUnitTest
	{
		[Fact]
		public void ClearSelection_Test()
		{
			var behavior = new DataPointSelectionBehavior
			{
				Source = new LineSeries(),
				SelectedIndex = 3,
				SelectedIndexes = [1, 2, 3]
			};

			behavior.ClearSelection();

			Assert.Equal(-1, behavior.SelectedIndex);
			Assert.Empty(behavior.SelectedIndexes);
		}

		[Fact]
		public void DataPointCanClearSelection_Test()
		{
			var selectionManager = new DataPointSelectionBehavior
			{
				Source = new LineSeries(),
			};

			var result = selectionManager.CanClearSelection();

			Assert.True(result);
		}

		[Fact]
		public void SeriesCanClearSelection_Test()
		{
			var series = new LineSeries()
			{
				IsVisible = true,
			};

			var selectionManager = new SeriesSelectionBehavior()
			{
				Chart = new SfCartesianChart()
			};

			selectionManager.Chart.Series.Add(series);

			var result = selectionManager.CanClearSelection();

			Assert.True(result);
		}

		[Fact]
		public void UpdateSelectionChanging_Test()
		{
			var selection = new DataPointSelectionBehavior
			{
				Type = ChartSelectionType.Multiple
			};
			var index = 2;

			selection.UpdateSelectionChanging(index, true);
			Assert.Equal(index, selection.ActualSelectedIndexes[0]);

			selection.Type = ChartSelectionType.SingleDeselect;
			selection.SelectedIndex = index;
			selection.UpdateSelectionChanging(index, true);
			Assert.Equal(-1, selection.SelectedIndex);

			selection.Type = ChartSelectionType.Single;
			selection.UpdateSelectionChanging(index, true);
			Assert.Equal(index, selection.SelectedIndex);
		}

		[Fact]
		public void MapActualIndexes_Test()
		{
			var selectionManager = new DataPointSelectionBehavior
			{
				SelectedIndex = 0,
				SelectedIndexes = [1, 2],
				ActualSelectedIndexes = []
			};

			selectionManager.MapActualIndexes();

			Assert.Contains(0, selectionManager.ActualSelectedIndexes);
			Assert.Contains(1, selectionManager.ActualSelectedIndexes);
			Assert.Contains(2, selectionManager.ActualSelectedIndexes);
		}

		[Theory]
		[InlineData(0, ChartSelectionType.Single, 0, false)]
		[InlineData(1, ChartSelectionType.Multiple, 1, true)]
		public void IsSelectionChangingInvoked_Test(
	int index, ChartSelectionType selectionType, int selectedIndex, bool cancel)
		{
			var selectionManager = new SeriesSelectionBehavior
			{
				Type = selectionType,
				SelectedIndex = selectedIndex,
				ActualSelectedIndexes = [1]
			};

			selectionManager.SelectionChanging += (sender, args) =>
			{
				args.Cancel = cancel;
			};

			var result = selectionManager.IsSelectionChangingInvoked(this, index);

			Assert.Equal(!cancel, result);
		}

		[Theory]
		[InlineData(0, ChartSelectionType.Multiple, 3)]
		[InlineData(1, ChartSelectionType.Single, 1)]
		public void InvokeSelectionChangedEvent_Test(
	int index, ChartSelectionType selectionType, int selectedIndex)
		{
			var selectionManager = new DataPointSelectionBehavior
			{
				Type = selectionType,
				SelectedIndex = selectedIndex,
				ActualSelectedIndexes = [selectedIndex]
			};

			var wasCalled = false;
			selectionManager.SelectionChanged += (sender, args) =>
			{
				wasCalled = true;
			};

			selectionManager.InvokeSelectionChangedEvent(this, index);

			Assert.True(wasCalled);
		}

		[Fact]
		public void GetSelectionBrush_Test()
		{
			var selectionManager = new DataPointSelectionBehavior
			{
				Type = ChartSelectionType.Single,
				ActualSelectedIndexes = [3],
				SelectionBrush = new SolidColorBrush(Colors.Red)
			};

			var index = 3;
			selectionManager.SelectedIndex = index;

			var result = selectionManager.GetSelectionBrush(index);

			Assert.Equal(selectionManager.SelectionBrush, result);
		}
	}
}
