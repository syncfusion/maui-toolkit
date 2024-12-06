using Syncfusion.Maui.Toolkit.EffectsView;
using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Maui.ControlsGallery.EffectsView.SfEffectsView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScaleAnimation : SampleView
	{
		public ScaleAnimation()
		{
			InitializeComponent();
		}

		private void AnimationCompleted(object sender, EventArgs e)
		{
			var effectsView = (Syncfusion.Maui.Toolkit.EffectsView.SfEffectsView)sender;
			if (effectsView.ScaleFactor == 0.85)
			{
				effectsView.ScaleFactor = 1;
			}
			else
			{

				effectsView.ScaleFactor = 0.85;
			}
		}
	}

	public partial class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		private double _scaleFactorValue = 0.85;

		public double ScaleFactorValue
		{
			get
			{
				return _scaleFactorValue;
			}
			set
			{
				if (_scaleFactorValue != value)
				{
					_scaleFactorValue = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaleFactorValue)));
				}
			}
		}

		private double _scaleDuration = 150;

		public double ScaleDuration
		{
			get
			{
				return _scaleDuration;
			}
			set
			{
				if (_scaleDuration != value)
				{
					_scaleDuration = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ScaleDuration)));
				}
			}
		}

		private SfEffects _touchUpEffectsValue = SfEffects.Scale;
		public SfEffects TouchUpEffectsValue
		{
			get
			{
				return _touchUpEffectsValue;
			}
			set
			{
				_touchUpEffectsValue = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TouchUpEffectsValue)));
			}
		}
	}

	public class VisibilityConverter : IValueConverter
	{
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			if (parameter != null)
			{
				Syncfusion.Maui.Toolkit.EffectsView.SfEffectsView effectsView = (Syncfusion.Maui.Toolkit.EffectsView.SfEffectsView)parameter;
				return effectsView.ScaleFactor == 1;
			}
			return null;
		}

		public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
		{
			return value;
		}
	}
}