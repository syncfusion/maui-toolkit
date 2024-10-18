using System.Collections.ObjectModel;

namespace Syncfusion.Maui.ControlsGallery
{
    /// <summary>
    /// 
    /// </summary>
    public class ControlModel
    {
        /// <summary>
        /// 
        /// </summary>
        public String? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? StatusTag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPreview { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SampleCategoryModel>? SampleCategories { get; set; }
    }
}
