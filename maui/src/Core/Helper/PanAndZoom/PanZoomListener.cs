using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using System;
using System.ComponentModel;

namespace Syncfusion.Maui.Toolkit.Internals
{
    /// <summary>
    /// Represents a helper class that assits in providing zoom and pan functionalities for a View.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// PanZoomListener panZoomListener = new PanZoomListener();
    /// Image imageView = new Image();
    /// imageView.AddGestureListener(panZoomListener);
    /// ]]>
    /// </code>
    /// </example>
    public class PanZoomListener : IPinchGestureListener, IPanGestureListener, IDoubleTapGestureListener, ITapGestureListener, ILongPressGestureListener, ITouchListener, IKeyboardListener, INotifyPropertyChanged
    {
        #region Private variables
        private bool m_stopScroll = false;
        private double m_currentZoomFactor = 1;
        private double m_minZoomFactor = 0.1;
        private double m_maxZoomFactor = 10;
        private bool m_allowPinchZoom = true;
        private bool m_allowDoubleTapZoom = true;
        private bool m_allowMouseWheelZoom = true;
        private PanMode m_panMode = PanMode.Both;
        private MouseWheelSettings? m_mouseWheelSettings;
        private DoubleTapSettings? m_doubleTapSettings;
        #endregion

        #region Variables exposed as internal for unit test automation
        internal KeyboardKey m_currentKeyModifier = KeyboardKey.None;
        #endregion

        bool IGestureListener.IsRequiredSingleTapGestureRecognizerToFail => false;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of <see cref="Syncfusion.Maui.Toolkit.Internals.PanZoomListener"/>
        /// </summary>
        public PanZoomListener()
        {
            m_mouseWheelSettings = new MouseWheelSettings();
            m_doubleTapSettings = new DoubleTapSettings();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the current zoom factor. It allows you to reset (or update) the zoom factor to a specific value.
        /// </summary>
        public double CurrentZoomFactor
        {
            get => m_currentZoomFactor;
            set
            {
                m_currentZoomFactor = value;
                OnPropertyChanged("CurrentZoomFactor");
            }
        }

        /// <summary>
        /// Gets the default mouse wheel settings used for zoom. It also allows you to change the default options. 
        /// </summary>
        public MouseWheelSettings? MouseWheelSettings => m_mouseWheelSettings;

        /// <summary>
        /// Gets the default double tap gesture settings used for zoom. It also allows you to change the default options. 
        /// </summary>
        public DoubleTapSettings? DoubleTapSettings => m_doubleTapSettings;

        /// <summary>
        /// Gets or sets the minimum zoom allowed. The default value is 0.1.
        /// </summary>
        /// <remarks>
        /// The value should not exceed the <see cref="PanZoomListener.MaxZoomFactor"/>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// PanZoomListener PanZoomListener = new PanZoomListener();
        /// PanZoomListener.MinZoomFactor = 0.5;
        /// ]]>
        /// </code>
        /// </example>
        public double MinZoomFactor
        {
            get => m_minZoomFactor;
            set
            {
                m_minZoomFactor = value;
                OnPropertyChanged("MinZoomFactor");
            }
        }

        /// <summary>
        /// Gets or sets the maximum zoom allowed. The default value is 10.
        /// </summary>
        /// <remarks>
        /// The value should not be lower than <see cref="PanZoomListener.MinZoomFactor"/>.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// PanZoomListener PanZoomListener = new PanZoomListener();
        /// PanZoomListener.MaxZoomFactor = 4;
        /// ]]>
        /// </code>
        /// </example>
        public double MaxZoomFactor
        {
            get => m_maxZoomFactor;
            set
            {
                m_maxZoomFactor = value;
                OnPropertyChanged("MaxZoomFactor");
            }
        }

        /// <summary>
        /// Get or sets the pan mode. The default value is <see cref="PanMode.Both"/>.
        /// </summary>
        /// <remarks>
        /// Setting the value to <see cref="PanMode.None"/> disables the pan.
        /// </remarks>
        public PanMode PanMode
        {
            get => m_panMode;
            set
            {
                m_panMode = value;
                OnPropertyChanged("PanMode");
            }
        }

        /// <summary>
        /// Get or sets a value indicating whether or not the double-tapping zoom gesture is allowed.
        /// </summary>
        public bool AllowDoubleTapZoom
        {
            get => m_allowDoubleTapZoom;
            set
            {
                m_allowDoubleTapZoom = value;
                OnPropertyChanged("AllowDoubleTapZoom");
            }
        }

        /// <summary>
        /// Get or sets a value indicating whether or not the pinch to zoom gesture is allowed.
        /// </summary>
        public bool AllowPinchZoom
        {
            get => m_allowPinchZoom;
            set
            {
                m_allowPinchZoom = value;
                OnPropertyChanged("AllowPinchZoom");
            }
        }

        /// <summary>
        /// Get or sets a value indicating whether or not the zoom by mouse wheel scroll is allowed.
        /// </summary>
        public bool AllowMouseWheelZoom
        {
            get => m_allowMouseWheelZoom;
            set
            {
                m_allowMouseWheelZoom = value;
                OnPropertyChanged("AllowMouseWheelZoom");
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the zoom is changed.
        /// </summary>
        public event EventHandler<ZoomEventArgs>? ZoomChanged;

        /// <summary>
        /// Occurs when the zoom is started.
        /// </summary>	
        public event EventHandler<ZoomEventArgs>? ZoomStarted;

        /// <summary>
        /// Occurs when the zoom is ended.
        /// </summary>	
        public event EventHandler<ZoomEventArgs>? ZoomEnded;

        /// <summary>
        /// Occurs when the pan is updated.
        /// </summary>
        public event EventHandler<PanEventArgs>? PanUpdated;

        /// <summary>
        /// Occurs when the target view is tapped.
        /// </summary>
        internal event EventHandler<TapEventArgs>? OnTap;

        internal event EventHandler<PointerEventArgs>? OnTouch;
        internal event EventHandler<LongPressEventArgs>? OnLongPress;
        internal event EventHandler<TapEventArgs>? OnDoubleTap;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion

        #region Interface implementations
        bool IGestureListener.IsTouchHandled => m_stopScroll;

        /// <summary>
        /// Invokes on double tap interaction.
        /// </summary>
        void IDoubleTapGestureListener.OnDoubleTap(TapEventArgs e)
        {
            OnDoubleTap?.Invoke(this, e);
            if (AllowDoubleTapZoom == false)
                return;

            if (ZoomStarted != null || ZoomChanged != null || ZoomEnded != null)
            {
                double desiredZoomFactor = CurrentZoomFactor != DoubleTapSettings!.DefaultZoomFactor ?
                    DoubleTapSettings!.DefaultZoomFactor :
                    DoubleTapSettings!.DefaultZoomFactor * ((100 + DoubleTapSettings!.ZoomDeltaPercent) / 100);
                ZoomTo(desiredZoomFactor, e.TapPoint);
            }
        }

        void ILongPressGestureListener.OnLongPress(LongPressEventArgs e)
        {
            OnLongPress?.Invoke(this, e);
        }

        /// <summary>
        /// Invoked when the target view is tapped.
        /// </summary>
        /// <param name="e"></param>
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            OnTap?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes on pinch interaction.
        /// </summary>
        public virtual void OnPinch(PinchEventArgs e)
        {
            if (AllowPinchZoom == false)
                return;

            if (ZoomStarted != null || ZoomChanged != null || ZoomEnded != null)
            {
                switch (e.Status)
                {
                    case GestureStatus.Started:
                        BeginZoom(e.TouchPoint, e.Angle);
                        break;
                    case GestureStatus.Canceled:
                        m_stopScroll = false;
                        break;
                    case GestureStatus.Running:
                        OnZoom(CurrentZoomFactor * e.Scale, e.TouchPoint, e.Angle);
                        break;
                    case GestureStatus.Completed:
                        EndZoom(e.TouchPoint, e.Angle);
                        break;
                }
            }
        }

        /// <summary>
        /// Invokes on mouse wheel action.
        /// </summary>
        /// <param name="e"></param>
        void ITouchListener.OnScrollWheel(ScrollEventArgs e)
        {
            if (AllowMouseWheelZoom == false || e.ScrollDelta == 0)
                return;

            if (ZoomStarted != null || ZoomChanged != null || ZoomEnded != null)
            {
                if (m_currentKeyModifier == MouseWheelSettings!.ZoomKeyModifier)
                {
                    double desiredZoomFactor = e.ScrollDelta > 0 ?
                        CurrentZoomFactor * ((100 + MouseWheelSettings!.ZoomDeltaPercent) / 100) :
                        CurrentZoomFactor * ((100 - MouseWheelSettings!.ZoomDeltaPercent) / 100);
                    ZoomTo(desiredZoomFactor, e.TouchPoint);
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Invokes on pan interaction.
        /// </summary>
        void IPanGestureListener.OnPan(PanEventArgs e)
        {
            if (PanMode == PanMode.None)
                return;

            switch (e.Status)
            {
                case GestureStatus.Started:
                    m_stopScroll = true;
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    m_stopScroll = false;
                    break;
            }
            OnPanUpdated(e);
        }

        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            OnTouch?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes on key up action.
        /// </summary>
        void IKeyboardListener.OnKeyUp(KeyEventArgs args)
        {
            UpdateModifierKey(args);
        }

        /// <summary>
        /// Invokes on key down action.
        /// </summary>
        void IKeyboardListener.OnKeyDown(KeyEventArgs args)
        {
            UpdateModifierKey(args);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Zooms to the specified factor at the given origin.
        /// </summary>
        /// <param name=" zoomFactor ">The zoom factor to be applied.</param>
        /// <param name="zoomOrigin">The origin location from which the zoom must be performed. The default origin location is the top left.</param>
        public void ZoomTo(double zoomFactor, Point? zoomOrigin = null)
        {
            if (zoomOrigin == null)
                zoomOrigin = Point.Zero;

            //Begin zoom with the previous zoom factor.
            BeginZoom(zoomOrigin.Value);

            //Set the updated zoom factor and request zoom change.
            OnZoom(zoomFactor, zoomOrigin.Value);

            //End zoom with the current zoom factor.
            EndZoom(zoomOrigin.Value);
        }

        /// <summary>
        /// Method used to identify whether the zooming changed is hooked or not.
        /// </summary>
        /// <returns>Returns true/false.</returns>
        protected bool IsZoomingEnabled()
        {
            return (ZoomStarted != null || ZoomChanged != null || ZoomEnded != null);
        }

        #endregion

        #region Helper methods
        void BeginZoom(Point zoomOrigin, double? angle = null)
        {
            m_stopScroll = true;
            if (ZoomStarted != null)
            {
                ZoomEventArgs zoomArgs = new ZoomEventArgs(CurrentZoomFactor, zoomOrigin);
                zoomArgs.Angle = angle;
                ZoomStarted(this, zoomArgs);
            }
        }

        /// <summary>
        /// Methood called whenever the zoom gets changed.
        /// </summary>
        /// <remarks>
        /// This method is marked as protected to access this method from the internal assembly.
        /// </remarks>
        /// <param name="zoomFactor">The zoom level.</param>
        /// <param name="zoomOrigin">The zoom origin.</param>
        /// <param name="angle">The angle.</param>
        protected void OnZoom(double zoomFactor, Point zoomOrigin, double? angle = null)
        {
            double oldZoomFactor = CurrentZoomFactor;
            CurrentZoomFactor = Math.Clamp(zoomFactor, MinZoomFactor, MaxZoomFactor);
            if (ZoomChanged != null)
            {
                if (oldZoomFactor != CurrentZoomFactor)
                {
                    ZoomEventArgs zoomArgs = new ZoomEventArgs(CurrentZoomFactor, zoomOrigin);
                    zoomArgs.Angle = angle;
                    ZoomChanged(this, zoomArgs);
                }
            }
        }

        void EndZoom(Point zoomOrigin, double? angle = null)
        {
            if (ZoomEnded != null)
            {
                ZoomEventArgs zoomArgs = new ZoomEventArgs(CurrentZoomFactor, zoomOrigin);
                zoomArgs.Angle = angle;
                ZoomEnded(this, zoomArgs);
            }
            m_stopScroll = false;
        }

        void OnPanUpdated(PanEventArgs panEventArgs)
        {
            if (PanUpdated == null)
                return;

            if (PanMode == PanMode.Both)
                PanUpdated(this, panEventArgs);
            else
            {
                Point translationPoint = Point.Zero;
                switch (PanMode)
                {
                    case PanMode.Vertical:
                        translationPoint.Y = panEventArgs.TranslatePoint.Y;
                        break;
                    case PanMode.Horizontal:
                        translationPoint.X = panEventArgs.TranslatePoint.X;
                        break;
                }
                PanEventArgs eventArgs = new PanEventArgs(panEventArgs.Status, panEventArgs.TouchPoint, translationPoint, panEventArgs.Velocity);
                PanUpdated(this, eventArgs);
            }
        }

        void UpdateModifierKey(KeyEventArgs args)
        {
            if (args.IsCtrlKeyPressed)
                m_currentKeyModifier = KeyboardKey.Ctrl;
            else if (args.IsAltKeyPressed)
                m_currentKeyModifier = KeyboardKey.Alt;
            else if (args.IsShiftKeyPressed)
                m_currentKeyModifier = KeyboardKey.Shift;
            else if (args.IsCommandKeyPressed)
                m_currentKeyModifier = KeyboardKey.Command;
            else
                m_currentKeyModifier = KeyboardKey.None;
        }

        void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}