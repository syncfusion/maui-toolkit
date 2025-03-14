using System.Diagnostics;
using System.Numerics;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Windows.Graphics.Imaging;
using Image = Microsoft.UI.Xaml.Controls.Image;
using Size = Windows.Foundation.Size;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// Shadow helper extension class.
	/// </summary>
	internal static class ShadowExtensions
	{
		/// <summary>
		/// Gets the mask for the shadow.
		/// </summary>
		/// <param name="element">Instance of native popupView.</param>
		/// <param name="popupViewWidth">Width of native popupView.</param>
		/// <param name="popupViewHeight">Height of native popupView.</param>
		/// <returns>Returns mask value.</returns>
		internal static async Task<CompositionBrush> GetAlphaMaskAsync(this UIElement element, int popupViewWidth, int popupViewHeight)
		{
			CompositionBrush? mask = null;
			try
			{
				if (element is TextBlock textElement)
				{
					return textElement.GetAlphaMask();
				}

				if (element is Image image)
				{
					return image.GetAlphaMask();
				}

				if (element is Shape shape)
				{
					return shape.GetAlphaMask();
				}
				else if (element is FrameworkElement frameworkElement)
				{
					if (popupViewHeight > 0 && popupViewWidth > 0)
					{
						var visual = ElementCompositionPreview.GetElementVisual(element);
						var elementVisual = visual.Compositor.CreateSpriteVisual();
						elementVisual.Size = element.RenderSize.ToVector2();
						var bitmap = new RenderTargetBitmap();

						await bitmap.RenderAsync(
							element,
							popupViewWidth,
							popupViewHeight);

						var pixels = await bitmap.GetPixelsAsync();
						using (var softwareBitmap = SoftwareBitmap.CreateCopyFromBuffer(
							pixels,
							BitmapPixelFormat.Bgra8,
							bitmap.PixelWidth,
							bitmap.PixelHeight,
							BitmapAlphaMode.Premultiplied))
						{
							var brush = CompositionImageBrush.FromBGRASoftwareBitmap(
								visual.Compositor,
								softwareBitmap,
								new Size(bitmap.PixelWidth, bitmap.PixelHeight));
							mask = brush.Brush;
						}
					}
				}
			}
			catch (Exception exc)
			{
				Debug.WriteLine($"Failed to get AlphaMask {exc}");
				mask = null;
			}

			return mask!;
		}
	}
}