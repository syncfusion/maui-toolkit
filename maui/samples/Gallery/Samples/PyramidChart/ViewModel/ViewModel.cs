using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Syncfusion.Maui.ControlsGallery.PyramidChart.SfPyramidChart
{
    public class ViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> PyramidData { get; set; }

        public ObservableCollection<ChartDataModel> FinancialData { get; set; }
        public ObservableCollection<ChartDataModel> MusicData { get; set; }


        public ViewModel()
        {
            PyramidData = new ObservableCollection<ChartDataModel>
            {
           //new ChartDataModel("Lecture", 5) { Color =Color.FromArgb("#95DB9C"),  LearningCategories = new List<Learning>{ new Learning("Lecture")}},
           //new ChartDataModel("Read", 10){Color =Color.FromArgb("#B95375"),LearningCategories = new List<Learning>{ new Learning("Reading") }},
           //new ChartDataModel("Listen", 20){Color =Color.FromArgb("#56BBAF"),LearningCategories = new List<Learning>{ new Learning("Hearing Words") }},
           //new ChartDataModel("Audiovisual", 30){Color =Color.FromArgb("#606D7F"),LearningCategories = new List<Learning>{ 
           //    new Learning("View Images"),
           //    new Learning("Watch Videos"), 
           //    }},
           new ChartDataModel("Passive Verbal",50){LearningCategories = new List<Learning>{
               new Learning("Attending Lecture", Color.FromArgb("#A033F5")),
               new Learning("Reading Books", Color.FromArgb("#A033F5")),
               new Learning("Listening Others", Color.FromArgb("#A033F5")),
               new Learning("Multimedia Listening", Color.FromArgb("#A033F5"))}},
           new ChartDataModel("Discussion", 50) {LearningCategories = new List<Learning>{
               new Learning("Attend Exhibits",Color.FromArgb("#E75A6E")),
               new Learning("Group Discussion",Color.FromArgb("#E75A6E")),
               new Learning("Watch a Demonstration",Color.FromArgb("#E75A6E"))}},
           new ChartDataModel("Practice", 75){LearningCategories = new List<Learning>{
               new Learning("Hands-on-Workshop",Color.FromArgb("#0DC920")),
               new Learning("Develop a Class Project",Color.FromArgb("#0DC920"))}},
           new ChartDataModel("Teach Others", 90){LearningCategories = new List<Learning>{
               new Learning("Simulate Model",Color.FromArgb("#2EC4B6")),
               new Learning("Do the Real Thing",Color.FromArgb("#2EC4B6")) }},
            };

            FinancialData = new ObservableCollection<ChartDataModel>
            {
#if WINDOWS || MACCATALYST
           new ChartDataModel("Retail",62,10),
          new ChartDataModel("Manufacturing",85,14.25),
          new ChartDataModel("Marketing",106,17.82),
          new ChartDataModel("Shipping",144,24.51),
          new ChartDataModel("R&D",193,32.71)
#else
          new ChartDataModel("Retail",83,14),
          new ChartDataModel("Manufacturing",85,14.25),
          new ChartDataModel("Marketing",106,17.82),
          new ChartDataModel("Shipping",144,22.51),
          new ChartDataModel("R&D",193,28.71)
#endif
            };

            MusicData = new ObservableCollection<ChartDataModel>
            {
#if WINDOWS || MACCATALYST
                new ChartDataModel("CD/Cassette",10),
                new ChartDataModel("Digital files",18),
                new ChartDataModel("Streaming",32),
                new ChartDataModel("Radio",36)
#else
                new ChartDataModel("CD/Cassette",18),
                new ChartDataModel("Digital files",20),
                new ChartDataModel("Streaming",29),
                new ChartDataModel("Radio",33)
#endif
            };
        }
    }
}
