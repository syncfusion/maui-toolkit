using CoreGraphics;
using UIKit;

namespace Syncfusion.Maui.Toolkit.Carousel
{
    /// <summary>
    /// The CustomCarouselPanGesture class.
    /// </summary>
    internal class CustomCarouselPanGesture : UIGestureRecognizerDelegate
    {
        /// <summary>
        /// The carousel view.
        /// </summary>
        private PlatformCarousel _carousel;

        /// <summary>
        /// The custom carousel PanGesture.
        /// </summary>
        /// <param name="carousel">The carousel.</param>
        public CustomCarouselPanGesture(PlatformCarousel carousel)
        {
            _carousel = carousel;
        }

        /// <summary>
        /// The gesture recognizer.
        /// </summary>
        /// <param name="gestureRecognizer">The gestureRecognizer.</param>
        /// <param name="otherGestureRecognizer">The otherGestureRecognizer.</param>
        /// <returns>The return value.</returns>
        public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            CGPoint stopLocation = new CGPoint();
            if (_carousel.gestureRecognizer != null)
            {
                 stopLocation = _carousel.gestureRecognizer.TranslationInView(_carousel);
            }
            if (Math.Abs(stopLocation.X) < Math.Abs(stopLocation.Y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}