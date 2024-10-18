using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.IO;
using System.Threading.Tasks;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Provides extension methods for views on the Ios and Mac platform.
	/// </summary>
	public static partial class ViewExtensions
    {
        /// <summary>
        /// <para> To convert a view to a stream in a specific file format, the <b> GetStreamAsync </b> method is used. Currently, the supported file formats are <b> JPEG or PNG </b>. </para>
        /// <para> To get the stream for the view in <b> PNG </b> file format, use <b> await view.GetStreamAsync(ImageFileFormat.Png); </b> </para>
        /// <para> To get the stream for the view in <b> JPEG </b> file format, use <b> await view.GetStreamAsync(ImageFileFormat.Jpeg); </b> </para>
        /// <remarks> The view's stream can only be rendered when the view is added to the visual tree. </remarks>
        /// </summary>
        /// <param name="view"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public static async Task<Stream> GetStreamAsync(this View view, ImageFileFormat format)
        {
            if (view != null && view.Handler is IViewHandler viewHandler)
            {
                if (viewHandler.PlatformView is UIView uiView)
                {
                    UIGraphics.BeginImageContextWithOptions(uiView.Bounds.Size, false, 0);
                    var context = UIGraphics.GetCurrentContext();
                    uiView.DrawViewHierarchy(uiView.Bounds, afterScreenUpdates: true);
                    var image = UIGraphics.GetImageFromCurrentImageContext();
                    UIGraphics.EndImageContext();
                    //To return a Task<Stream> method, an async method with a delay of 1 millisecond is used
                    await Task.Delay(1);

                    if (image != null)
                    {
                        Stream stream = ConvertImageToStream(image, format);
                        return stream;
                    }
                }
            }
            return Stream.Null;
        }

		/// <summary>
		/// <para> To save a view as an image in the desired file format, the <b> SaveAsImage </b> is used.Currently, the supported image formats are <b> JPEG or PNG </b>. </para>
		/// <para> By default, the image format is <b> PNG </b>.For example, <b> view.SaveAsImage("Test"); </b> </para>
		/// <para> To save a view in the <b> PNG </b> image format, the filename should be passed with the <b> ".png" extension </b> 
		/// while to save the image in the <b> JPEG </b> image format, the filename should be passed with the <b> ".jpeg" extension </b>, 
		/// for example, <b> "view.SaveAsImage("Test.png")" and "view.SaveAsImage("Test.jpeg")" </b> respectively. </para>
		/// <para> <b> Saved location: </b>
		/// For <b> Windows, Android, and Mac </b>, the image will be saved in the <b> Pictures folder </b>, and for <b> iOS </b>, the image will be saved in the <b> Photos Album folder </b>. </para>
		/// <remarks> <para> In <b> Windows and Mac </b>, when you save an image with an already existing filename, the existing file is replaced with a new file, but the filename remains the same. </para>
		/// <para> In <b> Android </b>, when you save the same view with an existing filename, the new image will be saved with a filename with a number appended to it, 
		/// for example, Test(1).jpeg and the existing filename Test.jpeg will be removed.When you save a different view with an already existing filename, 
		/// the new image will be saved with a filename with a number will be appended to it, for example, Test(1).jpeg, and the existing filename Test.jpeg will remain in the folder. </para>
		/// <para> In <b> iOS </b>, due to its platform limitation, the image will be saved with the default filename, for example, IMG_001.jpeg, IMG_002.png and more. </para> </remarks>
		/// <remarks> The view can be saved as an image only when the view is added to the visual tree. </remarks>
		/// </summary>
		/// <param name="view">The view to save as an image.</param>
		/// <param name="fileName">The name of the file to save the image as.</param>
		public static async void SaveAsImage(this View view, string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string imageExtension = string.IsNullOrEmpty(extension) ? "png" : extension.Trim('.').ToLowerInvariant();
            var imageFormat = imageExtension == "jpg" || imageExtension == "jpeg" ? ImageFileFormat.Jpeg : ImageFileFormat.Png;
            fileName = Path.GetFileNameWithoutExtension(fileName) + "." + imageFormat.ToString().ToLowerInvariant();
            using (Stream stream = await view.GetStreamAsync(imageFormat))
            {
                var imageData = NSData.FromStream(stream);

                if (imageData != null)
                {
#if !MACCATALYST
                    var image = UIImage.LoadFromData(imageData);
                    image?.SaveToPhotosAlbum(new UIImage.SaveStatus(delegate (UIImage img, NSError error) { }));
#endif

                    NSFileManager fileManager = NSFileManager.DefaultManager;
                    string[] paths = NSSearchPath.GetDirectories(NSSearchPathDirectory.PicturesDirectory, NSSearchPathDomain.All, true);
                    string fullPath = Path.Combine(paths[0], fileName);
                    fileManager.CreateFile(fullPath, imageData, new NSDictionary());
                }
            }
        }

        /// <summary>
        /// To render the image as a stream in desired file format.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        static Stream ConvertImageToStream(UIImage image, ImageFileFormat format)
        {
            if (format == ImageFileFormat.Png)
            {
                var formattedImage = image.AsPNG();
                if (formattedImage != null)
                    return formattedImage.AsStream();
            }
            else
            {
                var formattedImage = image.AsJPEG();
                if (formattedImage != null)
                    return formattedImage.AsStream();
            }

            return Stream.Null;
        }
    }
}
