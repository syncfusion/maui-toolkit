namespace  Syncfusion.Maui.ControlsGallery.PyramidChart.SfPyramidChart
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

        public string? Department { get; set; }

        public List<double>? EmployeeAges { get; set; }

        public List<Learning>? LearningCategories { get; set; }

        public Brush? Color { get;  set; }

        public ChartDataModel(string department, List<double> employeeAges)
        {
            Department = department;
            EmployeeAges = employeeAges;
        }

        public ChartDataModel(string name, double value, Brush color)
        {
            Name = name;
            Value = value;
            Color = color;
        }
        public ChartDataModel(string name, double value)
        {
            Name = name;
            Value = value;
        }


        public ChartDataModel( double value, string department)
        {
            Department = department;
            Value = value;
        }

        public ChartDataModel(string name, double value, double horizontalErrorValue, double verticalErrorValue)
        {
            Name = name;
            Value = value;
            High = horizontalErrorValue;
            Low = verticalErrorValue;
        }

        public ChartDataModel(string name, double value, double size)
        {
            Name = name;
            Value = value;
            Size = size;
        }

        public ChartDataModel()
        {

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
    }

    public class Learning
    {
        public Brush? Color { get; set; }


        public string Category { get; set; }
        public Learning(string category)
        {
            Category = category;
        }

        public Learning(string category, Brush? color)
        {
            Category = category;
            Color = color;
        }

    }
}
