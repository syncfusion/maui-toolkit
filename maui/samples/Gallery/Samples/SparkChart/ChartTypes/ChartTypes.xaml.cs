namespace Syncfusion.Maui.ControlsGallery.SparkChart.SfSparkChart;

public partial class ChartTypes : SampleView
{
	public ChartTypes()
	{
		InitializeComponent();
		if (DeviceInfo.Idiom == DeviceIdiom.Phone || DeviceInfo.Idiom == DeviceIdiom.Tablet)
		{
			MainGrid.RowDefinitions.Clear();
			MainGrid.ColumnDefinitions.Clear();
			MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
		}
		else
		{
			MainGrid.RowDefinitions.Clear();
			MainGrid.ColumnDefinitions.Clear();
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
		}
	}

	public override void OnDisappearing()
	{
		base.OnDisappearing();
		sparkLine.Handler?.DisconnectHandler();
		sparkArea.Handler?.DisconnectHandler();
		sparkColumn.Handler?.DisconnectHandler();
		sparkWinLoss.Handler?.DisconnectHandler();
	}
}