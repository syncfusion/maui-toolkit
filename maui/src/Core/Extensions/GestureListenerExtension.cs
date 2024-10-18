using System;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Represents the extension class that create instance for <see cref="GestureDetector"/> class and set to target class.
    /// </summary>
    public static class GestureListenerExtension
    {
        internal static BindableProperty GestureDetectorProperty = BindableProperty.Create(nameof(GestureDetector), typeof(GestureDetector), typeof(View), null);

        /// <summary>
        /// Create the Gesture detector and add the listener to it. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="listener"></param>
        public static void AddGestureListener(this View target, IGestureListener listener)
        {
            var gestureDetector = target.GetValue(GestureDetectorProperty) as GestureDetector;

            if (gestureDetector == null)
            {
                gestureDetector = new GestureDetector(target);
                target.SetValue(GestureDetectorProperty, gestureDetector);
            }

            gestureDetector.AddListener(listener);

        }

        /// <summary>
        /// Remove the listener and detector. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="listener"></param>
        public static void RemoveGestureListener(this View target, IGestureListener listener)
        {
            var gestureDetector = target.GetValue(GestureDetectorProperty) as GestureDetector;

            if (gestureDetector != null)
            {
                gestureDetector.RemoveListener(listener);
                if (!gestureDetector.HasListener())
                {
                    gestureDetector.Dispose();
                    target.SetValue(GestureDetectorProperty, null);
                }
            }
        }

        /// <summary>
        /// Clear the listeners and touch detector.
        /// </summary>
        /// <param name="target"></param>
        public static void ClearGestureListeners(this View target)
        {
            var gestureDetector = target.GetValue(GestureDetectorProperty) as GestureDetector;

            if (gestureDetector != null)
            {
                gestureDetector.Dispose();
                target.SetValue(GestureDetectorProperty, null);
            }
        }
    }
}

