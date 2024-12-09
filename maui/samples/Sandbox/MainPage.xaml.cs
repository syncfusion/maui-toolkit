namespace Syncfusion.Maui.Samples.Sandbox
{
	public partial class MainPage : ContentPage
	{
		int _count;

		public MainPage()
		{
			InitializeComponent();
		}

		private void OnCounterClicked(object sender, EventArgs e)
		{
			_count++;

			if (_count == 1)
			{
				CounterBtn.Text = $"Clicked {_count} time";
			}
			else
			{
				CounterBtn.Text = $"Clicked {_count} times";
			}

			SemanticScreenReader.Announce(CounterBtn.Text);
		}
	}

}
