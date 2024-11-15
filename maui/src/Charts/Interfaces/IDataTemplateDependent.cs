using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.Charts
{
    internal interface IDataTemplateDependent
    {
        #region Properties

        ObservableCollection<ChartDataLabel> DataLabels { get; }

        DataTemplate LabelTemplate { get; }

        bool IsVisible { get; }

        #endregion

        #region Methods

        bool IsTemplateItemsChanged() => true;

        #endregion
    }
}
