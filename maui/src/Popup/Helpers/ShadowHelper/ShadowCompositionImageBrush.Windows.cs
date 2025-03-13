#nullable disable

using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Graphics.DirectX;
using Microsoft.UI.Composition;
using Windows.Graphics.Imaging;
using Rect = Windows.Foundation.Rect;
using Size = Windows.Foundation.Size;

namespace Syncfusion.Maui.Toolkit.Popup
{
	/// <summary>
	/// CompositionImageBrush to paint SpriteVisual.
	/// </summary>
	internal class CompositionImageBrush : IDisposable
	{
		#region Fields

		CompositionGraphicsDevice _graphicsDevice;
		CompositionDrawingSurface _drawingSurface;
		CompositionSurfaceBrush _drawingBrush;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="CompositionImageBrush"/> class.
		/// </summary>
		public CompositionImageBrush()
		{
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the brush to paint the spritevisual.
		/// </summary>
		public CompositionBrush Brush => _drawingBrush;

		#endregion

		#region Public Methods

		/// <summary>
		/// Creates a CompositionImageBrush from a SoftwareBitmap.
		/// </summary>
		/// <param name="compositor"><see cref="Compositor"/> of visual.</param>
		/// <param name="bitmap">bitmap of native popupView.</param>
		/// <param name="outputSize">bitmap size.</param>
		/// <returns>Returns CompositionImageBrush.</returns>
		public static CompositionImageBrush FromBGRASoftwareBitmap(Compositor compositor, SoftwareBitmap bitmap, Size outputSize)
		{
			CompositionImageBrush brush = new CompositionImageBrush();
			brush.CreateDevice(compositor);
			brush.CreateDrawingSurface(outputSize);
			brush.DrawSoftwareBitmap(bitmap, outputSize);
			brush.CreateSurfaceBrush(compositor);
			return brush;
		}

		/// <summary>
		/// Disposes the objects used here.
		/// </summary>
		public void Dispose()
		{
			_drawingBrush.Dispose();
			_drawingSurface.Dispose();
			_graphicsDevice.Dispose();
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Creates a CompositionGraphicsDevice from a Compositor.
		/// </summary>
		/// <param name="compositor"><see cref="Compositor"/> of visual.</param>
		void CreateDevice(Compositor compositor)
		{
			_graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(
				compositor, CanvasDevice.GetSharedDevice());
		}

		/// <summary>
		/// Creates drawing surface.
		/// </summary>
		/// <param name="drawSize">bitmap size.</param>
		void CreateDrawingSurface(global::Windows.Foundation.Size drawSize)
		{
			_drawingSurface = _graphicsDevice.CreateDrawingSurface(
				drawSize,
				DirectXPixelFormat.B8G8R8A8UIntNormalized,
				DirectXAlphaMode.Premultiplied);
		}

		/// <summary>
		/// Creates a CompositionSurfaceBrush from a CompositionDrawingSurface.
		/// </summary>
		/// <param name="compositor"><see cref="Compositor"/> of visual.</param>
		void CreateSurfaceBrush(Compositor compositor)
		{
			_drawingBrush = compositor.CreateSurfaceBrush(_drawingSurface);
		}

		/// <summary>
		/// DrawSoftwareBitmap to the bitmap.
		/// </summary>
		/// <param name="softwareBitmap">bitmap of native popupView.</param>
		/// <param name="renderSize">bitmap size.</param>
		void DrawSoftwareBitmap(SoftwareBitmap softwareBitmap, Size renderSize)
		{
			using (var drawingSession = CanvasComposition.CreateDrawingSession(_drawingSurface))
			using (var bitmap = CanvasBitmap.CreateFromSoftwareBitmap(drawingSession.Device, softwareBitmap))
			{
				drawingSession.DrawImage(bitmap, new Rect(0, 0, renderSize.Width, renderSize.Height));
			}
		}

		#endregion
	}
}