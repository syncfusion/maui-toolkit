
namespace Syncfusion.Maui.ControlsGallery.SunburstChart.SfSunburstChart
{
    public class Model
    {
        public string? JobDescription { get; set; }

        public string? JobGroup { get; set; }

        public double EmployeesCount { get; set; }

        public string? Country { get; set; }

        public string? Continent { get; set; }

        public string? State { get; set; }

        public double Population { get; set; }

        public double Percentage { get; set; }

        public string? Quarter { get; set; }

        public string? Month { get; set; }

        public string? Week { get; set; }

        public double Sales { get; set; }

        public Model(string country, string jobDescription, string jobGroup, double employeesCount)
        {
            Country = country;
            JobDescription = jobDescription;
            JobGroup = jobGroup;
            EmployeesCount = employeesCount;
        }

        public Model(string country, string jobDescription, double employeesCount)
        {
            Country = country;
            JobDescription = jobDescription;
            EmployeesCount = employeesCount;
        }

        public Model(string state, string continent, double population, string country)
        {
            State = state;
            Continent = continent;
            Population = population;
            Country = country;
        }

        public Model(string continent, double percentage, string country)
        {
            Continent = continent;
            Percentage = percentage;
            Country = country;          
        }

        public Model(double sales, string quarter, string month)
        {
            Sales = sales;
            Quarter = quarter;
            Month = month;
        }

        public Model(double sales, string quarter, string month, string week)
        {
            Sales = sales;
            Quarter = quarter;
            Month = month;
            Week = week;
        }
    }
}