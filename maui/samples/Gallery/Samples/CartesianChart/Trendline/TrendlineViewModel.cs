
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart;

public class TrendlineViewModel : INotifyPropertyChanged
{
    public const string LinearType = "Linear";
    public const string ExponentialType = "Exponential";
    public const string PowerType = "Power";
    public const string PolynomialType = "Polynomial";
    public const string MovingAverageType = "MovingAvg";
    public const string LogarithmicType = "Logarithmic";

    private const int PolyMin = 2, PolyMax = 6;
    private const int PeriodMin = 2, PeriodMax = 6;

    private const int ForecastMin = 0, ForecastMax = 6;

    public ObservableCollection<QuarterRevenue> QuarterlyRevenue { get; } = new()
    {
        new QuarterRevenue(2018, 640),
        new QuarterRevenue(2019, 705),
        new QuarterRevenue(2020, 665),
        new QuarterRevenue(2021, 740),
        new QuarterRevenue(2022, 710),
        new QuarterRevenue(2023, 785),
        new QuarterRevenue(2024, 760),
        new QuarterRevenue(2025, 845)
    };

    public IList<string> TrendlineTypes { get; } =
        new List<string> { LinearType, ExponentialType, PowerType, PolynomialType, MovingAverageType, LogarithmicType };

    bool _isTrendlineVisible = true;
    public bool IsTrendlineVisible
    {
        get => _isTrendlineVisible;
        set => SetField(ref _isTrendlineVisible, value);
    }

    bool _showMarkers = true;
    public bool ShowMarkers
    {
        get => _showMarkers;
        set => SetField(ref _showMarkers, value);
    }

    bool _enableTrendlineTooltip = true;
    public bool EnableTrendlineTooltip
    {
        get => _enableTrendlineTooltip;
        set => SetField(ref _enableTrendlineTooltip, value);
    }

    string _selectedTrendlineType = LinearType;
    public string SelectedTrendlineType
    {
        get => _selectedTrendlineType;
        set
        {
            if (SetField(ref _selectedTrendlineType, value))
            {
                OnPropertyChanged(nameof(IsPolynomialVisible));
                OnPropertyChanged(nameof(IsMovingAverageVisible));
            }
        }
    }

    public bool IsPolynomialVisible => SelectedTrendlineType == PolynomialType;
    public bool IsMovingAverageVisible => SelectedTrendlineType == MovingAverageType;

    int _polynomialOrder = 3;
    public int PolynomialOrder
    {
        get => _polynomialOrder;
        set
        {
            if (SetField(ref _polynomialOrder, Clamp(value, PolyMin, PolyMax)))
                RaiseStepperCanExecutes();
        }
    }

    int _movingAveragePeriod = 4;
    public int MovingAveragePeriod
    {
        get => _movingAveragePeriod;
        set
        {
            if (SetField(ref _movingAveragePeriod, Clamp(value, PeriodMin, PeriodMax)))
                RaiseStepperCanExecutes();
        }
    }

    int forwardForecast;
    public int ForwardForecast
    {
        get => forwardForecast;
        set
        {
            if (SetField(ref forwardForecast, Clamp(value, ForecastMin, ForecastMax)))
                RaiseStepperCanExecutes();
        }
    }

    int _backwardForecast;
    public int BackwardForecast
    {
        get => _backwardForecast;
        set
        {
            if (SetField(ref _backwardForecast, Clamp(value, ForecastMin, ForecastMax)))
                RaiseStepperCanExecutes();
        }
    }

    public ICommand IncrementForwardCommand { get; }
    public ICommand DecrementForwardCommand { get; }
    public ICommand IncrementBackwardCommand { get; }
    public ICommand DecrementBackwardCommand { get; }

    public ICommand IncrementPolynomialOrderCommand => _incrementPolynomialOrderCommand;
    public ICommand DecrementPolynomialOrderCommand => _decrementPolynomialOrderCommand;

    public ICommand IncrementMAPeriodCommand => _incrementMAPeriodCommand;
    public ICommand DecrementMAPeriodCommand => _decrementMAPeriodCommand;

    private readonly RelayCommand _incrementPolynomialOrderCommand;
    private readonly RelayCommand _decrementPolynomialOrderCommand;

    private readonly RelayCommand _incrementMAPeriodCommand;
    private readonly RelayCommand _decrementMAPeriodCommand;

    public TrendlineViewModel()
    {
        IncrementForwardCommand = new RelayCommand(() => ForwardForecast = Math.Min(ForwardForecast + 1, ForecastMax));
        DecrementForwardCommand = new RelayCommand(() => ForwardForecast = Math.Max(ForwardForecast - 1, ForecastMin));
        IncrementBackwardCommand = new RelayCommand(() => BackwardForecast = Math.Min(BackwardForecast + 1, ForecastMax));
        DecrementBackwardCommand = new RelayCommand(() => BackwardForecast = Math.Max(BackwardForecast - 1, ForecastMin));

        _incrementPolynomialOrderCommand = new RelayCommand(
            () => PolynomialOrder += 1,
            () => PolynomialOrder < PolyMax);

        _decrementPolynomialOrderCommand = new RelayCommand(
            () => PolynomialOrder -= 1,
            () => PolynomialOrder > PolyMin);

        _incrementMAPeriodCommand = new RelayCommand(
            () => MovingAveragePeriod += 1,
            () => MovingAveragePeriod < PeriodMax);

        _decrementMAPeriodCommand = new RelayCommand(
            () => MovingAveragePeriod -= 1,
            () => MovingAveragePeriod > PeriodMin);
    }

    private static int Clamp(int value, int min, int max) =>
        Math.Max(min, Math.Min(max, value));

    private static double Clamp(double value, double min, double max) =>
        Math.Max(min, Math.Min(max, value));

    private void RaiseStepperCanExecutes()
    {
        _incrementPolynomialOrderCommand?.RaiseCanExecuteChanged();
        _decrementPolynomialOrderCommand?.RaiseCanExecuteChanged();

        _incrementMAPeriodCommand?.RaiseCanExecuteChanged();
        _decrementMAPeriodCommand?.RaiseCanExecuteChanged();

    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    bool SetField<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}

public record QuarterRevenue(double Year, double Revenue);

public sealed class RelayCommand : ICommand
{
    readonly Action _execute;
    readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        this._execute = execute;
        this._canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public void Execute(object? parameter) => _execute();

    public void RaiseCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
