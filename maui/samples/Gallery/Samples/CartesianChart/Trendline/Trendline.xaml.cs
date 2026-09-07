using System.ComponentModel;
using Syncfusion.Maui.Toolkit.Charts;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart;

public partial class Trendlines : SampleView
{
    ChartTrendline? _currentTrendline;
    TrendlineViewModel? _viewModel;

    public Trendlines()
    {
        InitializeComponent();
        AttachViewModel(BindingContext as TrendlineViewModel);
        BindingContextChanged += OnBindingContextChanged;
    }

    void OnBindingContextChanged(object? sender, EventArgs e)
    {
        AttachViewModel(BindingContext as TrendlineViewModel);
    }

    void AttachViewModel(TrendlineViewModel? newViewModel)
    {
        if (_viewModel != null)
        {
            _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        _viewModel = newViewModel;

        if (_viewModel != null)
        {
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
            UpdateTrendline();
        }
    }

    void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TrendlineViewModel.SelectedTrendlineType))
        {
            UpdateTrendline();
        }
    }

    void UpdateTrendline()
    {
        if (RevenueSeries == null || _viewModel == null)
        {
            return;
        }

        RevenueSeries.Trendlines.Clear();

        var trendline = CreateTrendline(_viewModel.SelectedTrendlineType);
        trendline.BindingContext = _viewModel;
        ApplyCommonBindings(trendline);

        RevenueSeries.Trendlines.Add(trendline);
        _currentTrendline = trendline;
    }

    ChartTrendline CreateTrendline(string type)
    {
        ChartTrendline trendline = type switch
        {
            TrendlineViewModel.ExponentialType => new ExponentialTrendline(),
            TrendlineViewModel.PowerType => new PowerTrendline(),
            TrendlineViewModel.PolynomialType => new PolynomialTrendline(),
            TrendlineViewModel.MovingAverageType => new MovingAverageTrendline(),
            TrendlineViewModel.LogarithmicType => new LogarithmicTrendline(),
            _ => new LinearTrendline()
        };

        trendline.Label = "Trend";
        trendline.Stroke = new SolidColorBrush(Color.FromArgb("#1A73E8"));
        return trendline;
    }

    void ApplyCommonBindings(ChartTrendline trendline)
    {
        if (_viewModel == null)
        {
            return;
        }

        trendline.SetBinding(ChartTrendline.ShowMarkersProperty,
            new Binding(nameof(TrendlineViewModel.ShowMarkers)));
        trendline.SetBinding(ChartTrendline.EnableTooltipProperty,
            new Binding(nameof(TrendlineViewModel.EnableTrendlineTooltip)));
        trendline.SetBinding(ChartTrendline.ForwardForecastProperty,
            new Binding(nameof(TrendlineViewModel.ForwardForecast)));
        trendline.SetBinding(ChartTrendline.BackwardForecastProperty,
            new Binding(nameof(TrendlineViewModel.BackwardForecast)));
        trendline.SetBinding(ChartTrendline.IsVisibleProperty,
            new Binding(nameof(TrendlineViewModel.IsTrendlineVisible), mode: BindingMode.TwoWay));

        var markerSettings = new ChartMarkerSettings
        {
            Fill = new SolidColorBrush(Color.FromArgb("#CFE2FF")),
            Stroke = new SolidColorBrush(Color.FromArgb("#1A73E8")),
            StrokeWidth = 2,
            Type = ShapeType.Diamond
        };
        markerSettings.BindingContext = _viewModel;
        trendline.MarkerSettings = markerSettings;

        if (trendline is PolynomialTrendline polynomialTrendline)
        {
            polynomialTrendline.SetBinding(PolynomialTrendline.OrderProperty,
                new Binding(nameof(TrendlineViewModel.PolynomialOrder)));
        }
        else if (trendline is MovingAverageTrendline movingAverageTrendline)
        {
            movingAverageTrendline.SetBinding(MovingAverageTrendline.PeriodProperty,
                new Binding(nameof(TrendlineViewModel.MovingAveragePeriod)));
        }
    }
}
