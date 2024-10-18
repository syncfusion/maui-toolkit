
namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class ChartDataModel
    {
        public string? Name { get; set; }

        public double Data { get; set; }

        public string? Label { get; set; }
        public DateTime Date { get; set; }

        public double Value { get; set; }

        public double Value1 { get; set; }

        public double Size { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public bool IsSummary { get; set; }

        public string? Levels { get; set; }
        public string? Department { get; set; }

        public List<double>? Energy { get; set; }

        public string? Year { get; set; }

        public double Organic { get; set; }

        public double FairTrade { get; set; }

        public double VegAlternatives { get; set; }

        public double Others { get; set; }

        public double Peru { get; set; }

        public double Canada { get; set; }

        public double Ethiopia { get; set; }

        public double Mali { get; set; }

        public double TotalGross { get; set; }

        public double TotalGrossInBillion { get; set; }

        public double GrossLastYearDelta { get; set; }

        public double AverageGross { get; set; }

        public double Releases { get; set; }

        public string? TopMovie { get; set; }

        public ChartDataModel() { }

        public ChartDataModel(string department, List<double> employeeAges)
        {
            Levels = department;
            Energy = employeeAges;
        }

        public ChartDataModel(string name, double value)
        {
            Name = name;
            Value = value;
        }

        public ChartDataModel(string name, double value, double high, double low)
        {
            Name = name;
            Value = value;
            High = high;
            Low = low;
        }

        public ChartDataModel(string name, double value, double size)
        {
            Name = name;
            Value = value;
            Size = size;
        }

        public ChartDataModel(DateTime date, double value, double size)
        {
            Date = date;
            Value = value;
            Size = size;
        }

        public ChartDataModel(double value, double value1, double size)
        {
            Value1 = value;
            Value = value1;
            Size = size;
        }

        public ChartDataModel(double value1, double value, double size, string label)
        {
            Value1 = value1;
            Value = value;
            Size = size;
            Label = label;
        }

        public ChartDataModel(string name, double high, double low, double open, double close)
        {
            Name = name;
            High = high;
            Low = low;
            Value = open;
            Size = close;
        }

        public ChartDataModel(double name, double high, double low, double open, double close)
        {
            Data = name;
            High = high;
            Low = low;
            Value = open;
            Size = close;
        }

        public ChartDataModel(DateTime date, double high, double low, double open, double close)
        {
            Date = date;
            High = high;
            Low = low;
            Value = open;
            Size = close;
        }
        public ChartDataModel(double value, double size)
        {
            Value = value;
            Size = size;
        }
        public ChartDataModel(DateTime dateTime, double value)
        {
            Date = dateTime;
            Value = value;
        }

        public ChartDataModel(string name, double value, bool isSummary)
        {
            Name = name;
            Value = value;
            IsSummary = isSummary;
        }

        public ChartDataModel(DateTime date, double value, double value1, double value2)
        {
            Date = date;
            Value = value;
            High = value1;
            Low = value2;
        }

        public ChartDataModel(double organic, double fairTrade, double vegAlternatives, double others, string year)
        {
            Organic = organic;
            FairTrade = fairTrade;
            VegAlternatives = vegAlternatives;
            Others = others;
            Year = year;
        }

        public ChartDataModel(double peru, double canada, string year, double ethiopia, double mali)
        {
            Peru = peru;
            Canada = canada;
            Year = year;
            Ethiopia = ethiopia;
            Mali = mali;
        }

        public ChartDataModel(string year, double totalGross, double grossLastYearDelta, double averageGross, double releases, string topMovie)
        {
            Year = year;
            TotalGross = totalGross;
            GrossLastYearDelta = grossLastYearDelta;
            AverageGross = averageGross;
            Releases = releases;
            TopMovie = topMovie;
            TotalGrossInBillion = TotalGross / 1000000000;
        }
    }
}