using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.CartesianChart.SfCartesianChart
{
    public class PlotBandRecurrenceViewModel :BaseViewModel
    {
        public ObservableCollection<Model> FuelsPriceDetails { get; set; }

        public PlotBandRecurrenceViewModel()
        {
            FuelsPriceDetails = new ObservableCollection<Model>()
            {
                new Model (new DateTime(2014,01,01), 127.50, 133.46),
                new Model (new DateTime(2015,01,01), 111.13, 114.90),
                new Model (new DateTime(2016,01,01), 108.85, 110.13),
                new Model (new DateTime(2017,01,01), 117.59, 120.15),
                new Model (new DateTime(2018,01,01), 125.20, 129.98),
                new Model (new DateTime(2019,01,01), 124.88, 131.48),
                new Model (new DateTime(2020,01,01), 113.95, 119.14),
                new Model (new DateTime(2021,01,01), 131.27, 134.94),
                new Model (new DateTime(2022,01,01), 164.73, 177.66),
                new Model (new DateTime(2023,01,01), 147.75, 158.19),
            };
        }
    }

    public class Model
    {
        public DateTime Date { get; set; }
        public double NegativePetrolPrice { get; set; }
        public double DieselPrice { get; set; }

        public Model(DateTime date, double petrolPrice, double dieselPrice)
        {
            Date = date;
            NegativePetrolPrice = -petrolPrice;
            DieselPrice = dieselPrice;
        }
    }
}
