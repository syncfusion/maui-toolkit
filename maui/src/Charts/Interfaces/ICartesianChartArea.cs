namespace Syncfusion.Maui.Toolkit.Charts
{
    internal interface ICartesianChartArea : IChartArea
    {
        #region Properties

        ChartAxis? PrimaryAxis { get; set; }

        RangeAxisBase? SecondaryAxis { get; set; }

        #endregion
    }
}
