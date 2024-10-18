using System.Reflection;

namespace Syncfusion.Maui.ControlsGallery
{
    /// <summary>
    /// 
    /// </summary>
    public class SampleModel
    {
        /// <summary>
        /// 
        /// </summary>
        public String? SampleName { get; set; }

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
        public String? SearchTags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? StatusTag { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? SamplePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? ControlName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? ControlShortName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Assembly? AssemblyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? CodeViewerPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? CategoryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? Platforms { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ShowExpandIcon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsGettingStarted { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? VideoLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? SourceLink { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String? WhatsNew { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SampleModel()
        {
            ShowExpandIcon = true;
        }

    }
}
