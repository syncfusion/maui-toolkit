using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.ProgressBar;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.UnitTest;

public class LinearProgressBarGradientSortUnitTests : BaseUnitTest
{
	[Fact]
	public void GradientStops_AddedOutOfOrder_AreSortedByActualValue()
	{
		SfLinearProgressBar progressBar = new SfLinearProgressBar();
		progressBar.Minimum = 0;
		progressBar.Maximum = 100;
		progressBar.Progress = 80;

		var gradientStops = new ObservableCollection<ProgressGradientStop>
		{
			new ProgressGradientStop { Color = Colors.Orange, Value = 50, ActualValue = 50 },
			new ProgressGradientStop { Color = Colors.Red, Value = 0, ActualValue = 0 },
			new ProgressGradientStop { Color = Colors.Green, Value = 100, ActualValue = 100 },
			new ProgressGradientStop { Color = Colors.Yellow, Value = 25, ActualValue = 25 },
		};

		progressBar.GradientStops = gradientStops;

		Assert.Equal(4, progressBar.GradientStops.Count);
		Assert.Equal(Colors.Orange, progressBar.GradientStops[0].Color);
		Assert.Equal(Colors.Red, progressBar.GradientStops[1].Color);
		Assert.Equal(Colors.Green, progressBar.GradientStops[2].Color);
		Assert.Equal(Colors.Yellow, progressBar.GradientStops[3].Color);
	}

	[Fact]
	public void GradientStops_SingleStop_DoesNotThrow()
	{
		SfLinearProgressBar progressBar = new SfLinearProgressBar();
		progressBar.Minimum = 0;
		progressBar.Maximum = 100;
		progressBar.Progress = 50;

		var gradientStops = new ObservableCollection<ProgressGradientStop>
		{
			new ProgressGradientStop { Color = Colors.Blue, Value = 50, ActualValue = 50 }
		};

		progressBar.GradientStops = gradientStops;

		Assert.Single(progressBar.GradientStops);
		Assert.Equal(Colors.Blue, progressBar.GradientStops[0].Color);
	}

	[Fact]
	public void GradientStops_AlreadySorted_PreservesOrder()
	{
		SfLinearProgressBar progressBar = new SfLinearProgressBar();
		progressBar.Minimum = 0;
		progressBar.Maximum = 100;
		progressBar.Progress = 100;

		var gradientStops = new ObservableCollection<ProgressGradientStop>
		{
			new ProgressGradientStop { Color = Colors.Red, Value = 0, ActualValue = 0 },
			new ProgressGradientStop { Color = Colors.Yellow, Value = 50, ActualValue = 50 },
			new ProgressGradientStop { Color = Colors.Green, Value = 100, ActualValue = 100 },
		};

		progressBar.GradientStops = gradientStops;

		Assert.Equal(3, progressBar.GradientStops.Count);
		Assert.Equal(Colors.Red, progressBar.GradientStops[0].Color);
		Assert.Equal(Colors.Yellow, progressBar.GradientStops[1].Color);
		Assert.Equal(Colors.Green, progressBar.GradientStops[2].Color);
	}
}
