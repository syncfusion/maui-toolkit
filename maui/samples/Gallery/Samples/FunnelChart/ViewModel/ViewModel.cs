using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Syncfusion.Maui.ControlsGallery.FunnelChart.SfFunnelChart
{
    public class ViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> FunnelData { get; set; }
        public ObservableCollection<ChartDataModel>? RecruitmentData { get; set; }
        public ObservableCollection<ChartDataModel>? CustomerData { get; set; }

        public ViewModel()
        {
            FunnelData = new ObservableCollection<ChartDataModel>
            {
          new ChartDataModel("Renewed", 18),
          new ChartDataModel("Subscribed", 34),
          new ChartDataModel("Contacted Support", 52),
          new ChartDataModel("Downloaded a Trial", 68),
          new ChartDataModel("Visited the Website", 100),
           };

            RecruitmentData = new ObservableCollection<ChartDataModel>
            {
          new ChartDataModel("Candidates Recruited", 150,30),
          new ChartDataModel("HR Interview",220,44),
          new ChartDataModel("Technical Interview", 341,68.2),
          new ChartDataModel("Aptitude Test", 446,89.2),
          new ChartDataModel("Candidates Applied", 500,100),
           };


            CustomerData = new ObservableCollection<ChartDataModel>
            {
          new ChartDataModel("Closed Deals", 80,"80% of 100"),
          new ChartDataModel("Negotiations", 100,"67% of 150"),
          new ChartDataModel("Proposal",150,"75% of 200"),
          new ChartDataModel("Needs Analysis", 200,"50% of 400"),
          new ChartDataModel("Qualified Leads", 400,"80% of 500"),
          new ChartDataModel("Lead Generation", 500,"100%"),
           };


        }
    }
}
