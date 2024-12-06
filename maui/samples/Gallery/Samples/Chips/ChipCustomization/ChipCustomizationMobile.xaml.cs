namespace Syncfusion.Maui.ControlsGallery.Chips.SfChip
{
	public partial class ChipCustomizationMobile : SampleView
	{
		public ChipCustomizationMobile()
		{
			InitializeComponent();
		}

		private void TextColorSegment_Clicked(object sender, EventArgs e)
		{
			chip.TextColor = Color.FromArgb("#FFFFFF");
			chip.SelectionIndicatorColor = Color.FromArgb("#FFFFFF");
			chip.CloseButtonColor = Color.FromArgb("#FFFFFF");
			if (Application.Current != null)
			{
				text1.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			text2.Stroke = Colors.Transparent;
			text3.Stroke = Colors.Transparent;
			text4.Stroke = Colors.Transparent;
			text5.Stroke = Colors.Transparent;
			text6.Stroke = Colors.Transparent;
			text7.Stroke = Colors.Transparent;
			text8.Stroke = Colors.Transparent;
		}


		private void TextColorSegment_Clicked_1(object sender, EventArgs e)
		{
			chip.TextColor = Color.FromArgb("#C76B00");
			chip.SelectionIndicatorColor = Color.FromArgb("#C76B00");
			chip.CloseButtonColor = Color.FromArgb("#C76B00");
			text1.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				text2.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			text3.Stroke = Colors.Transparent;
			text4.Stroke = Colors.Transparent;
			text5.Stroke = Colors.Transparent;
			text6.Stroke = Colors.Transparent;
			text7.Stroke = Colors.Transparent;
			text8.Stroke = Colors.Transparent;
		}

		private void TextColorSegment_Clicked_2(object sender, EventArgs e)
		{
			chip.TextColor = Color.FromArgb("#A007A3");
			chip.SelectionIndicatorColor = Color.FromArgb("#A007A3");
			chip.CloseButtonColor = Color.FromArgb("#A007A3");
			text1.Stroke = Colors.Transparent;
			text2.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				text3.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			text4.Stroke = Colors.Transparent;
			text5.Stroke = Colors.Transparent;
			text6.Stroke = Colors.Transparent;
			text7.Stroke = Colors.Transparent;
			text8.Stroke = Colors.Transparent;

		}

		private void TextColorSegment_Clicked_3(object sender, EventArgs e)
		{
			chip.TextColor = Color.FromArgb("#00C737");
			chip.SelectionIndicatorColor = Color.FromArgb("#00C737");
			chip.CloseButtonColor = Color.FromArgb("#00C737");
			text1.Stroke = Colors.Transparent;
			text2.Stroke = Colors.Transparent;
			text3.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				text4.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			text5.Stroke = Colors.Transparent;
			text6.Stroke = Colors.Transparent;
			text7.Stroke = Colors.Transparent;
			text8.Stroke = Colors.Transparent;
		}

		private void TextColorSegment_Clicked_4(object sender, EventArgs e)
		{
			chip.TextColor = Color.FromArgb("#000000");
			chip.SelectionIndicatorColor = Color.FromArgb("#000000");
			chip.CloseButtonColor = Color.FromArgb("#000000");
			text1.Stroke = Colors.Transparent;
			text2.Stroke = Colors.Transparent;
			text3.Stroke = Colors.Transparent;
			text4.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				text5.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			text6.Stroke = Colors.Transparent;
			text7.Stroke = Colors.Transparent;
			text8.Stroke = Colors.Transparent;

		}
		private void TextColorSegment_Clicked_5(object sender, EventArgs e)
		{
			chip.TextColor = Color.FromArgb("#0012B2");
			chip.SelectionIndicatorColor = Color.FromArgb("#0012B2");
			chip.CloseButtonColor = Color.FromArgb("#0012B2");
			text1.Stroke = Colors.Transparent;
			text2.Stroke = Colors.Transparent;
			text3.Stroke = Colors.Transparent;
			text4.Stroke = Colors.Transparent;
			text5.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				text6.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			text7.Stroke = Colors.Transparent;
			text8.Stroke = Colors.Transparent;

		}
		private void TextColorSegment_Clicked_6(object sender, EventArgs e)
		{
			chip.TextColor = Color.FromArgb("#D1B400");
			chip.SelectionIndicatorColor = Color.FromArgb("#D1B400");
			chip.CloseButtonColor = Color.FromArgb("#D1B400");
			text1.Stroke = Colors.Transparent;
			text2.Stroke = Colors.Transparent;
			text3.Stroke = Colors.Transparent;
			text4.Stroke = Colors.Transparent;
			text5.Stroke = Colors.Transparent;
			text6.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				text7.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			text8.Stroke = Colors.Transparent;

		}
		private void TextColorSegment_Clicked_7(object sender, EventArgs e)
		{
			chip.TextColor = Color.FromArgb("#CC0000");
			chip.SelectionIndicatorColor = Color.FromArgb("#CC0000");
			chip.CloseButtonColor = Color.FromArgb("#CC0000");
			text1.Stroke = Colors.Transparent;
			text2.Stroke = Colors.Transparent;
			text3.Stroke = Colors.Transparent;
			text4.Stroke = Colors.Transparent;
			text5.Stroke = Colors.Transparent;
			text6.Stroke = Colors.Transparent;
			text7.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				text8.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}
		}
		private void BackgroundColorSegment_Clicked(object sender, EventArgs e)
		{
			chip.Background = Color.FromArgb("#FFFFFF");
			chip.StrokeThickness = viewModel.BorderThickness;
			if (Application.Current != null)
			{
				background1.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			background2.Stroke = Colors.Transparent;
			background3.Stroke = Colors.Transparent;
			background4.Stroke = Colors.Transparent;
			background5.Stroke = Colors.Transparent;
			background6.Stroke = Colors.Transparent;
			background7.Stroke = Colors.Transparent;
			background8.Stroke = Colors.Transparent;
		}

		private void BackgroundColorSegment_Clicked_1(object sender, EventArgs e)
		{
			chip.Background = Color.FromArgb("#C76B00");
			chip.StrokeThickness = viewModel.BorderThickness;
			background1.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				background2.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			background3.Stroke = Colors.Transparent;
			background4.Stroke = Colors.Transparent;
			background5.Stroke = Colors.Transparent;
			background6.Stroke = Colors.Transparent;
			background7.Stroke = Colors.Transparent;
			background8.Stroke = Colors.Transparent;
		}

		private void BackgroundColorSegment_Clicked_2(object sender, EventArgs e)
		{
			chip.Background = Color.FromArgb("#A007A3");
			chip.StrokeThickness = viewModel.BorderThickness;
			background1.Stroke = Colors.Transparent;
			background2.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				background3.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			background4.Stroke = Colors.Transparent;
			background5.Stroke = Colors.Transparent;
			background6.Stroke = Colors.Transparent;
			background7.Stroke = Colors.Transparent;
			background8.Stroke = Colors.Transparent;
		}

		private void BackgroundColorSegment_Clicked_3(object sender, EventArgs e)
		{
			chip.Background = Color.FromArgb("#00C737");
			chip.StrokeThickness = viewModel.BorderThickness;
			background1.Stroke = Colors.Transparent;
			background2.Stroke = Colors.Transparent;
			background3.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				background4.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			background5.Stroke = Colors.Transparent;
			background6.Stroke = Colors.Transparent;
			background7.Stroke = Colors.Transparent;
			background8.Stroke = Colors.Transparent;
		}

		private void BackgroundColorSegment_Clicked_4(object sender, EventArgs e)
		{
			chip.Background = Color.FromArgb("#000000");
			chip.StrokeThickness = viewModel.BorderThickness;
			background1.Stroke = Colors.Transparent;
			background2.Stroke = Colors.Transparent;
			background3.Stroke = Colors.Transparent;
			background4.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				background5.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			background6.Stroke = Colors.Transparent;
			background7.Stroke = Colors.Transparent;
			background8.Stroke = Colors.Transparent;
		}
		private void BackgroundColorSegment_Clicked_5(object sender, EventArgs e)
		{
			chip.Background = Color.FromArgb("#0012B2");
			chip.StrokeThickness = viewModel.BorderThickness;
			background1.Stroke = Colors.Transparent;
			background2.Stroke = Colors.Transparent;
			background3.Stroke = Colors.Transparent;
			background4.Stroke = Colors.Transparent;
			background5.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				background6.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			background7.Stroke = Colors.Transparent;
			background8.Stroke = Colors.Transparent;
		}
		private void BackgroundColorSegment_Clicked_6(object sender, EventArgs e)
		{
			chip.Background = Color.FromArgb("#D1B400");
			chip.StrokeThickness = viewModel.BorderThickness;
			background1.Stroke = Colors.Transparent;
			background2.Stroke = Colors.Transparent;
			background3.Stroke = Colors.Transparent;
			background4.Stroke = Colors.Transparent;
			background5.Stroke = Colors.Transparent;
			background6.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				background7.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			background8.Stroke = Colors.Transparent;
		}
		private void BackgroundColorSegment_Clicked_7(object sender, EventArgs e)
		{
			chip.Background = Color.FromArgb("#CC0000");
			chip.StrokeThickness = viewModel.BorderThickness;
			background1.Stroke = Colors.Transparent;
			background2.Stroke = Colors.Transparent;
			background3.Stroke = Colors.Transparent;
			background4.Stroke = Colors.Transparent;
			background5.Stroke = Colors.Transparent;
			background6.Stroke = Colors.Transparent;
			background7.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				background8.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}
		}

		private void BorderColorSegment_Clicked(object sender, EventArgs e)
		{
			chip.Stroke = Color.FromArgb("#FFFFFF");
			if (Application.Current != null)
			{
				border1.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			border2.Stroke = Colors.Transparent;
			border3.Stroke = Colors.Transparent;
			border4.Stroke = Colors.Transparent;
			border5.Stroke = Colors.Transparent;
			border6.Stroke = Colors.Transparent;
			border7.Stroke = Colors.Transparent;
			border8.Stroke = Colors.Transparent;
		}
		private void BorderColorSegment_Clicked_1(object sender, EventArgs e)
		{
			chip.Stroke = Color.FromArgb("#C76B00");
			border1.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				border2.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			border3.Stroke = Colors.Transparent;
			border4.Stroke = Colors.Transparent;
			border5.Stroke = Colors.Transparent;
			border6.Stroke = Colors.Transparent;
			border7.Stroke = Colors.Transparent;
			border8.Stroke = Colors.Transparent;
		}

		private void BorderColorSegment_Clicked_2(object sender, EventArgs e)
		{
			chip.Stroke = Color.FromArgb("#A007A3");
			border1.Stroke = Colors.Transparent;
			border2.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				border3.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			border4.Stroke = Colors.Transparent;
			border5.Stroke = Colors.Transparent;
			border6.Stroke = Colors.Transparent;
			border7.Stroke = Colors.Transparent;
			border8.Stroke = Colors.Transparent;
		}

		private void BorderColorSegment_Clicked_3(object sender, EventArgs e)
		{
			chip.Stroke = Color.FromArgb("#00C737");
			border1.Stroke = Colors.Transparent;
			border2.Stroke = Colors.Transparent;
			border3.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				border4.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			border5.Stroke = Colors.Transparent;
			border6.Stroke = Colors.Transparent;
			border7.Stroke = Colors.Transparent;
			border8.Stroke = Colors.Transparent;
		}

		private void BorderColorSegment_Clicked_4(object sender, EventArgs e)
		{
			chip.Stroke = Color.FromArgb("#141414");
			border1.Stroke = Colors.Transparent;
			border2.Stroke = Colors.Transparent;
			border3.Stroke = Colors.Transparent;
			border4.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				border5.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			border6.Stroke = Colors.Transparent;
			border7.Stroke = Colors.Transparent;
			border8.Stroke = Colors.Transparent;
		}
		private void BorderColorSegment_Clicked_5(object sender, EventArgs e)
		{
			chip.Stroke = Color.FromArgb("#0012B2");
			border1.Stroke = Colors.Transparent;
			border2.Stroke = Colors.Transparent;
			border3.Stroke = Colors.Transparent;
			border4.Stroke = Colors.Transparent;
			border5.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				border6.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			border7.Stroke = Colors.Transparent;
			border8.Stroke = Colors.Transparent;
		}
		private void BorderColorSegment_Clicked_6(object sender, EventArgs e)
		{
			chip.Stroke = Color.FromArgb("#D1B400");
			border1.Stroke = Colors.Transparent;
			border2.Stroke = Colors.Transparent;
			border3.Stroke = Colors.Transparent;
			border4.Stroke = Colors.Transparent;
			border5.Stroke = Colors.Transparent;
			border6.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				border7.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}

			border8.Stroke = Colors.Transparent;
		}
		private void BorderColorSegment_Clicked_7(object sender, EventArgs e)
		{
			chip.Stroke = Color.FromArgb("#CC0000");
			border1.Stroke = Colors.Transparent;
			border2.Stroke = Colors.Transparent;
			border3.Stroke = Colors.Transparent;
			border4.Stroke = Colors.Transparent;
			border5.Stroke = Colors.Transparent;
			border6.Stroke = Colors.Transparent;
			border7.Stroke = Colors.Transparent;
			if (Application.Current != null)
			{
				border8.Stroke = Application.Current.UserAppTheme == AppTheme.Dark ? (Color)App.Current.Resources["PrimaryBackgroundDark"] : (Color)App.Current.Resources["PrimaryBackgroundLight"];
			}
		}

	}
}