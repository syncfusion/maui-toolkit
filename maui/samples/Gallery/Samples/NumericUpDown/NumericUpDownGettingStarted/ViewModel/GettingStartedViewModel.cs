using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery.NumericUpDown
{
    internal class GettingStartedViewModel
    {
        #region Fields

        private ObservableCollection<ProductInfo>? _productInfo;

        #endregion

        #region Constructor

        public GettingStartedViewModel()
        {
            GenerateSource();
        }

        #endregion

        #region Properties

        public ObservableCollection<ProductInfo>? ProductInfo
        {
            get { return _productInfo; }
            set { _productInfo = value; }
        }

        #endregion

        #region Generate Source

        private void GenerateSource()
        {
            ProductInfoRepository productinfo = new();
            ProductInfo = productinfo.GetProductInfo();
        }

        #endregion
    }
}
