using NativeBitmap = Android.Graphics.Bitmap;
using Android.Graphics;
using Stream = System.IO.Stream;
using Android.Content;
using Android.Provider;
using Path = System.IO.Path;
using Color = Android.Graphics.Color;
using Microsoft.Maui.Platform;
using Uri = Android.Net.Uri;

namespace Syncfusion.Maui.Toolkit.Internals
{
	/// <summary>
	/// Provides extension methods for views on the Android platform.
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
        public static async Task<Stream> GetStreamAsync(this View view, ImageFileFormat format)
        {
            if (view != null && view.Handler is IViewHandler viewHandler)
            {
                if (viewHandler.PlatformView is Android.Views.View nativeView)
                {
                    NativeBitmap bitmap = GetBitmapRender(nativeView, new Size(nativeView.Width, nativeView.Height), view.BackgroundColor);

                    if (bitmap != null)
                    {
                        Stream stream = new MemoryStream();
                        ConvertBitmapToStream(bitmap, format, stream);
                        //To return a Task<Stream> method, an async method with a delay of 1 millisecond is used
                        await Task.Delay(1);
                        stream.Position = 0;
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
        /// <param name="view"></param>
        /// <param name="fileName"></param>
        public static async void SaveAsImage(this View view, string fileName)
        {
            var uri = MediaStore.Images.Media.ExternalContentUri as Uri;
            if (uri != null)
            {
                string extension = Path.GetExtension(fileName);
                string imageExtension = string.IsNullOrEmpty(extension) ? "png" : extension.Trim('.').ToLowerInvariant();
                var imageFormat = imageExtension == "jpg" || imageExtension == "jpeg" ? ImageFileFormat.Jpeg : ImageFileFormat.Png;
                fileName = Path.GetFileNameWithoutExtension(fileName) + "." + imageFormat.ToString().ToLowerInvariant();
                ContentValues values = new ContentValues();
                values.Put(MediaStore.IMediaColumns.DisplayName, fileName);
                var content = Android.App.Application.Context.ContentResolver;

                if (content != null)
                {
                    var url = content.Insert(uri, values);

                    if (url != null)
                    {
                        using (Stream stream = await view.GetStreamAsync(imageFormat))
                        using (MemoryStream memoryStream = new MemoryStream())
                        using (var newStream = content.OpenOutputStream(url))
                        {
                            await stream.CopyToAsync(memoryStream);
                            newStream?.Write(memoryStream.ToArray(), 0, (int)memoryStream.Length);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// To render the native bitmap of the passed view.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        static NativeBitmap GetBitmapRender(Android.Views.View view, Size size, Microsoft.Maui.Graphics.Color? viewBackground)
        {
            if (NativeBitmap.Config.Argb8888 != null)
            {
                var bitmap = NativeBitmap.CreateBitmap((int)size.Width, (int)size.Height, NativeBitmap.Config.Argb8888);

                if (bitmap != null)
                {
                    var canvas = new Canvas(bitmap);
                    canvas.DrawColor(viewBackground != null ? viewBackground.ToPlatform() : Color.White);
                    view.Draw(canvas);
                    return bitmap;
                }
            }

            NativeBitmap native = GetBitmapRender(view, Size.Zero, viewBackground);
            return native;
        }

        /// <summary>
        /// To render the bitmap as a stream in the desired file format.
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="format"></param>
        /// <param name="stream"></param>
        static void ConvertBitmapToStream(NativeBitmap bitmap, ImageFileFormat format, Stream stream)
        {
            switch (format)
            {
                case ImageFileFormat.Png:
                    if (NativeBitmap.CompressFormat.Png != null)
                        bitmap.Compress(NativeBitmap.CompressFormat.Png, 100, stream);
                    break;
                case ImageFileFormat.Jpeg:
                default:
                    if (NativeBitmap.CompressFormat.Jpeg != null)
                        bitmap.Compress(NativeBitmap.CompressFormat.Jpeg, 100, stream);
                    break;
            }
        }
    }
}

