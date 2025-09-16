using System.Collections.ObjectModel;


namespace Syncfusion.Maui.ControlsGallery.SunburstChart.SfSunburstChart
{
    public class ViewModel : BaseViewModel
    {
        public ObservableCollection<Model> DataSource { get; set; }
        public ObservableCollection<Model> DataSource1 { get; set; }
        public ObservableCollection<Model> DataSource2 { get; set; }
        public ObservableCollection<Model> DataSource3 { get; set; }
        public string[] DataLabelOption => ["Angle", "Normal"];
        public string[] DataLabelOption1 => ["Trim", "Hide"];
        public string[] SelectionType => ["Child", "Parent", "Single", "Group" ];
        public string[] SelectionDisplayMode => ["Fill", "Opacity", "Stroke"];
        public string[] SelectionFill => ["Mexican Pink", "Dark Yellow", "Tomato Red"];
        public string[] SelectionStroke => ["Black", "Yellow", "Turquoise"];

        public ViewModel()
        {
            //DataLabel
            DataSource = new ObservableCollection<Model>
            { 
                new Model ( "USA", "Sales", "Executive",  50 ),
                new Model ( "USA", "Sales", "Analyst",  40 ),
                new Model ( "USA", "Marketing",  40 ),
                new Model ( "USA", "Technical", "Testers",  35 ),
                new Model ( "USA", "Technical", "Developers",  175 ),
                new Model ( "USA", "Technical", "Developers",  70 ),
                new Model ( "USA", "Management",  40 ),
                new Model ( "USA", "Accounts",  60 ),
                new Model ( "India", "Technical", "Testers",  33 ),
                new Model ( "India", "Technical", "Developers",  125 ),
                new Model ( "India", "Technical", "Developers",  60 ),
                new Model ( "India", "HR Executives",  70 ),
                new Model ( "India", "Accounts",  45 ),
                new Model ( "Germany", "Sales", "Executive",  30 ),
                new Model ( "Germany", "Sales", "Analyst",  40 ),
                new Model ( "Germany", "Marketing",  50 ),
                new Model ( "Germany", "Technical", "Testers",  40 ),
                new Model ( "Germany", "Technical", "Developers",  60 ),
                new Model ( "Germany", "Technical", "Developers",  27 ),
                new Model ( "Germany", "Management",  40 ),
                new Model ( "Germany", "Accounts",  55 ),
                new Model ( "UK", "Technical", "Testers",  96 ),
                new Model ( "UK", "Technical", "Developers",  55 ),
                new Model ( "UK", "HR Executives",  60 ),
                new Model ( "UK", "Accounts",  45 )
            };

            //Tooltip
            DataSource1 = new ObservableCollection<Model>
            {
                new Model("Ontario", "North America", 23,"Canada"),
                new Model("New York", "North America", 19,"United States"),
                new Model("Ohio", "North America", 18, "United States"),
                new Model("Buenos Aires", "South America", 22, "Argentina"),
                new Model("Minas Gerais", "South America", 20, "Brazil"),
                new Model("Rio de Janeiro", "South America", 16, "Brazil"),
                new Model("Bahia", "South America", 15, "Brazil"),
                new Model("Parana", "South America", 19, "Brazil"),
                new Model("Chittagong", "Asia", 28, "Bangladesh"),
                new Model("Raj Shahi", "Asia", 18, "Bangladesh"),
                new Model("Khulna", "Asia", 15, "Bangladesh"),
                new Model("Liaoning", "Asia", 43, "China"),
                new Model("Shaanxi", "Asia", 37, "China"),
                new Model("Fujian", "Asia", 37, "China"),
                new Model("Shaanxi", "Asia",  35, "China"),
                new Model("Kerala", "Asia", 33, "India"),
                new Model("Punjab", "Asia",  27, "India"),
                new Model("Haryana", "Asia", 25, "India"),
                new Model("Delhi", "Asia", 16, "India"),
                new Model("Jammu", "Asia", 12, "India"),
                new Model("West Java", "Asia", 43, "Indonesia"),
                new Model("East Java", "Asia", 37, "Indonesia"),
                new Model("Banten", "Asia", 18, "Indonesia"),
                new Model("Tianjin", "Africa", 24, "Ethiopia"),
                new Model("Tianjin", "Africa", 15, "Ethiopia"),
                new Model("Lagos", "Africa", 23, "Nigeria"),
                new Model("Kano", "Africa",  25, "Nigeria"),
                new Model("Gauteng", "Africa", 16, "South Africa"),
                new Model("KwaZulu-Natal", "Africa", 20, "South Africa"),
                new Model("North Rhine-Westphalia", "Europe", 17, "Germany"),
                new Model("Bavaria", "Europe", 15, "Germany"),
                new Model("NBaden-Wurttemberg", "Europe", 20, "Germany"),
                new Model("England", "Europe", 51, "United Kingdom")
            };

            //Center view 
            DataSource2 = new ObservableCollection<Model>
            {
                new Model("Upstream",18, "Building material"),
                new Model("Upstream",20, "Building industry"),
                new Model("Upstream",23, "Financial industry"),
                new Model("Upstream",21, "Transportation industry"),
                new Model("Upstream",19, "Chemical industry"),

                new Model("Midstream",20, "Architectural design"),
                new Model("Midstream",24, "House building"),
                new Model("Midstream",21, "Marketing"),
                new Model("Midstream",18, "Operation management"),

                new Model("Downstream",18, "Furniture industry"),
                new Model("Downstream",20, "Appliance industry"),
                new Model("Downstream",23, "Decoration industry"),
                new Model("Downstream",21, "Leasing industry"),
            };

            //Default
            DataSource3 = new ObservableCollection<Model>
            {
                new Model(11, "Q1", "Jan"),
                new Model(8, "Q1", "Feb"),
                new Model(5, "Q1", "Mar"),
                new Model(13, "Q2", "Apr"),
                new Model(12, "Q2","May"),
                new Model(17, "Q2","Jun"),
                new Model(5, "Q3", "Jul"),
                new Model(4, "Q3","Aug"),
                new Model(5, "Q3","Sep"),
                new Model(7, "Q4","Oct"),
                new Model(18, "Q4", "Nov"),
                new Model(5, "Q4", "Dec", "W1"),
                new Model(5, "Q4", "Dec", "W2"),
                new Model(5, "Q4", "Dec", "W3"),
                new Model(5, "Q4", "Dec", "W4"),
            };
        }
    }
}