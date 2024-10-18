namespace Syncfusion.Maui.Toolkit.Semantics
{
    using Microsoft.Maui.Graphics;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Automation.Peers;
    using Microsoft.UI.Xaml.Automation.Provider;
    using Microsoft.UI.Xaml.Media;
    using Syncfusion.Maui.Toolkit.Platform;
    using Syncfusion.Maui.Toolkit.Graphics.Internals;
    using System.Collections.Generic;

    internal class CustomAutomationPeer : FrameworkElementAutomationPeer
    {
        /// <summary>
        /// Holds the information for the semantics nodes.
        /// </summary>
        private SfView mauiView;

        /// <summary>
        /// Holds the semantics nodes for the view.
        /// </summary>
        private List<SemanticsNode>? semanticNodes;

        /// <summary>
        /// Holds the automation peers for the semantics nodes.
        /// </summary>
        private IList<AutomationPeer> automationPeers;

        internal CustomAutomationPeer(NativeGraphicsView owner, SfView mauiView) : base(owner)
        {
            this.mauiView = mauiView;
            this.automationPeers = new List<AutomationPeer>();
        }

        /// <summary>
        /// Return the class name and the value added to name value while hovering. 
        /// </summary>
        /// <returns>The class name.</returns>
        protected override string GetClassNameCore()
        {
            return "";
        }

        /// <summary>
        /// Return the control type. Here, this class used to hold list of children automation peer.
        /// </summary>
        /// <returns>The control type.</returns>
        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Custom;
        }

        /// <summary>
        /// Information of the automation peer.
        /// </summary>
        /// <returns>Name of automation peer.</returns>
        protected override string GetNameCore()
        {
            return "";
        }

        /// <summary>
        /// Invalidate the children automation peer.
        /// </summary>
        internal void InvalidateSemantics()
        {
            this.GetChildren().Clear();
        }

        /// <summary>
        /// Return the children of the automation peer.
        /// </summary>
        /// <returns>The children.</returns>
        protected override IList<AutomationPeer> GetChildrenCore()
        {
            this.automationPeers.Clear();
            this.semanticNodes = ((ISemanticsProvider)this.mauiView).GetSemanticsNodes(this.mauiView.Width, this.mauiView.Height);
            AutomationPeer? previousAutomationPeer = null;
            SemanticsNode? previousNode = null;
            if (this.semanticNodes != null)
            {
                for (int i = 0; i < this.semanticNodes.Count; i++)
                {
                    SemanticsNode semanticsNode = this.semanticNodes[i];
                    //// Convert the node position into screen position.
                    AutomationPeer peer;
                    if (semanticsNode.IsTouchEnabled)
                    {
                        peer = new CustomButtonAutomationPeer((FrameworkElement)this.Owner, semanticsNode, this.mauiView);
                    }
                    else
                    {
                        peer = new CustomTextAutomationPeer((FrameworkElement)this.Owner, semanticsNode, this.mauiView);
                    }

                    if (previousAutomationPeer != null && previousAutomationPeer is ICustomAutomationPeer)
                    {
                        ((ICustomAutomationPeer)previousAutomationPeer).NextSibling = peer;
                        ((ICustomAutomationPeer)previousAutomationPeer).NextNode = semanticsNode;
                    }

                    if (peer is ICustomAutomationPeer)
                    {
                        ((ICustomAutomationPeer)peer).PrevSibling = previousAutomationPeer;
                        ((ICustomAutomationPeer)peer).PrevNode = previousNode;
                    }

                    previousAutomationPeer = peer;
                    previousNode = semanticsNode;
                    this.automationPeers.Add(peer);
                }
            }

            return this.automationPeers;
        }

        /// <summary>
        /// Handles and return the automation peer while navigation.
        /// </summary>
        /// <param name="direction">Action performed on automation peer.</param>
        /// <returns>Return the automation peer.</returns>
        protected override object NavigateCore(AutomationNavigationDirection direction)
        {
            if (direction == AutomationNavigationDirection.FirstChild && this.automationPeers != null && this.automationPeers.Count > 0)
            {
                if (semanticNodes != null && semanticNodes.Count > 0)
                {
                    //// Perform scroll while the view placed inside the scroll view and the current
                    //// child is not in visible region.
                    ((ISemanticsProvider)this.mauiView).ScrollTo(this.semanticNodes[0]);
                }

                return this.automationPeers[0];
            }
            else if (direction == AutomationNavigationDirection.LastChild && this.automationPeers != null && this.automationPeers.Count > 0)
            {
                if (semanticNodes != null && semanticNodes.Count > 0)
                {
                    //// Perform scroll while the view placed inside the scroll view and the current
                    //// child is not in visible region.
                    ((ISemanticsProvider)this.mauiView).ScrollTo(this.semanticNodes[this.semanticNodes.Count - 1]);
                }

                return this.automationPeers[this.automationPeers.Count - 1];
            }

            return base.NavigateCore(direction);
        }

        /// <summary>
        /// Return the automation peer based on its position.
        /// </summary>
        /// <param name="pointInWindowCoordinates">The interacted point.</param>
        /// <returns>The automation peer.</returns>
        protected override object GetElementFromPointCore(Windows.Foundation.Point pointInWindowCoordinates)
        {
            if (this.semanticNodes != null)
            {
                NativeGraphicsView owner = (NativeGraphicsView)this.Owner;
                double scale = owner.XamlRoot.RasterizationScale;
                Point viewStartPosition = AccessibilityHelper.GetViewStartPosition(owner, scale);

                for (int i = 0; i < this.automationPeers.Count; i++)
                {
                    ICustomAutomationPeer automationPeer = (ICustomAutomationPeer)this.automationPeers[i];
                    Rect semanticNodeBounds = new Rect(automationPeer.Node.Bounds.Left * scale, automationPeer.Node.Bounds.Top * scale, automationPeer.Node.Bounds.Width * scale, automationPeer.Node.Bounds.Height * scale);
                    Rect bounds = new Rect(viewStartPosition.X + semanticNodeBounds.Left, viewStartPosition.Y + semanticNodeBounds.Top, semanticNodeBounds.Width, semanticNodeBounds.Height);
                    if (bounds.Contains(new Point(pointInWindowCoordinates.X, pointInWindowCoordinates.Y)))
                    {
                        return automationPeer;
                    }
                }
            }

            return base.GetElementFromPointCore(pointInWindowCoordinates);
        }
    }

    /// <summary>
    /// Interface that used to hold its siblings
    /// </summary>
    internal interface ICustomAutomationPeer
    {
        /// <summary>
        /// Holds the previous sibling of the automation peer.
        /// </summary>
        AutomationPeer? PrevSibling { get; set; }

        /// <summary>
        /// Holds the previous sibling node information.
        /// </summary>
        SemanticsNode? PrevNode { get; set; }

        /// <summary>
        /// Hold the next sibling if the automation peer.
        /// </summary>
        AutomationPeer? NextSibling { get; set; }

        /// <summary>
        /// Holds the next sibling node information.
        /// </summary>
        SemanticsNode? NextNode { get; set; }

        /// <summary>
        /// Used to get the node information.
        /// </summary>
        SemanticsNode Node { get; }

        /// <summary>
        /// Holds the parent SfView instance for scroll action.
        /// </summary>
        SfView MauiSfView { get; set; }
    }

    /// <summary>
    /// Text automation peer.
    /// Custom text automation class created because label instance needed while create framework automation peer.
    /// </summary>
    internal class CustomTextAutomationPeer : FrameworkElementAutomationPeer, ICustomAutomationPeer
    {
        /// <summary>
        /// The information for the automation peer.
        /// </summary>
        private SemanticsNode semanticsNode;

        internal CustomTextAutomationPeer(FrameworkElement owner, SemanticsNode semanticsNode, SfView mauiView) : base(owner)
        {
            this.semanticsNode = semanticsNode;
            this.MauiSfView = mauiView;
        }

        /// <summary>
        /// Holds the previous sibling of the automation peer.
        /// </summary>
        public AutomationPeer? PrevSibling { get; set; }

        /// <summary>
        /// Holds the next sibling of the automation peer.
        /// </summary>
        public AutomationPeer? NextSibling { get; set; }

        /// <summary>
        /// Holds the next sibling node information.
        /// </summary>
        public SemanticsNode? NextNode { get; set; }

        /// <summary>
        /// Holds the previous sibling node information.
        /// </summary>
        public SemanticsNode? PrevNode { get; set; }

        /// <summary>
        /// Gets the node information.
        /// </summary>
        public SemanticsNode Node => semanticsNode;

        /// <summary>
        /// Holds the parent SfView instance for scroll action.
        /// </summary>
        public SfView MauiSfView { get; set; }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Text;
        }

        protected override string GetClassNameCore()
        {
            return "";
        }

        /// <summary>
        /// Return the information that needed on hovering.
        /// </summary>
        /// <returns>The name information.</returns>
        protected override string GetNameCore()
        {
            return semanticsNode.Text;
        }

        /// <summary>
        /// Handles and return the automation peer while navigation.
        /// </summary>
        /// <param name="direction">Action performed on automation peer.</param>
        /// <returns>Return the automation peer.</returns>
        protected override object NavigateCore(AutomationNavigationDirection direction)
        {
            if (direction == AutomationNavigationDirection.NextSibling && this.NextSibling != null)
            {
                if (this.NextNode != null)
                {
                    //// Perform scroll while the view placed inside the scroll view and the current
                    //// child is not in visible region.
                    ((ISemanticsProvider)this.MauiSfView).ScrollTo(this.NextNode);
                }

                return this.NextSibling;
            }
            else if (direction == AutomationNavigationDirection.PreviousSibling && this.PrevSibling != null)
            {
                if (this.PrevNode != null)
                {
                    //// Perform scroll while the view placed inside the scroll view and the current
                    //// child is not in visible region.
                    ((ISemanticsProvider)this.MauiSfView).ScrollTo(this.PrevNode);
                }

                return this.PrevSibling;
            }

            return base.NavigateCore(direction);
        }

        /// <summary>
        /// Return the current automation peer bounds value related to screen.
        /// </summary>
        /// <returns>The rectangle bounds value.</returns>
        protected override Windows.Foundation.Rect GetBoundingRectangleCore()
        {
            Windows.Foundation.Rect rect = this.GetParent().GetBoundingRectangle();
            NativeGraphicsView owner = (NativeGraphicsView)this.Owner;
            double scale = owner.XamlRoot.RasterizationScale;
            Point viewStartPosition = AccessibilityHelper.GetViewStartPosition(owner, scale);
            Rect semanticNodeBounds = new Rect(this.semanticsNode.Bounds.Left * scale, this.semanticsNode.Bounds.Top * scale, this.semanticsNode.Bounds.Width * scale, this.semanticsNode.Bounds.Height * scale);
            Rect bounds = new Rect(viewStartPosition.X + semanticNodeBounds.Left, viewStartPosition.Y + semanticNodeBounds.Top, semanticNodeBounds.Width, semanticNodeBounds.Height);
            double yPosition = bounds.Y;
            double endYPosition = bounds.Y + bounds.Height;

            double xPosition = bounds.X;
            double endXPosition = bounds.X + bounds.Width;

            if (yPosition < rect.Top)
            {
                yPosition = rect.Top;
            }

            if (endYPosition > rect.Top + rect.Height)
            {
                endYPosition = rect.Top + rect.Height;
            }

            if (xPosition < rect.Left)
            {
                xPosition = rect.Left;
            }

            if (endXPosition > rect.Left + rect.Width)
            {
                endXPosition = rect.Left + rect.Width;
            }

            double width = endXPosition - xPosition;
            if (width < 0)
            {
                width = 0;
            }

            double height = endYPosition - yPosition;
            if (height < 0)
            {
                height = 0;
            }

            return new Windows.Foundation.Rect(xPosition, yPosition, width, height);
        }
    }

    /// <summary>
    /// Button automation peer.
    /// Custom button automation class created because button instance needed while create framework automation peer.
    /// </summary>
    internal class CustomButtonAutomationPeer : FrameworkElementAutomationPeer, ICustomAutomationPeer, IInvokeProvider
    {
        /// <summary>
        /// The information for the automation peer.
        /// </summary>
        private SemanticsNode semanticsNode;

        internal CustomButtonAutomationPeer(FrameworkElement owner, SemanticsNode semanticsNode, SfView mauiView) : base(owner)
        {
            this.semanticsNode = semanticsNode;
            this.MauiSfView = mauiView;
        }

        /// <summary>
        /// Holds the previous sibling of the automation peer.
        /// </summary>
        public AutomationPeer? PrevSibling { get; set; }

        /// <summary>
        /// Holds the previous sibling of the automation peer.
        /// </summary>
        public AutomationPeer? NextSibling { get; set; }

        /// <summary>
        /// Holds the next sibling node information.
        /// </summary>
        public SemanticsNode? NextNode { get; set; }

        /// <summary>
        /// Holds the previous sibling node information.
        /// </summary>
        public SemanticsNode? PrevNode { get; set; }

        /// <summary>
        /// Gets the node information.
        /// </summary>
        public SemanticsNode Node => semanticsNode;

        /// <summary>
        /// Holds the parent SfView instance for scroll action.
        /// </summary>
        public SfView MauiSfView { get; set; }

        /// <summary>
        /// Handles and return the automation peer while navigation.
        /// </summary>
        /// <param name="direction">Action performed on automation peer.</param>
        /// <returns>Return the automation peer.</returns>
        protected override object NavigateCore(AutomationNavigationDirection direction)
        {
            if (direction == AutomationNavigationDirection.NextSibling && this.NextSibling != null)
            {
                if (this.NextNode != null)
                {
                    //// Perform scroll while the view placed inside the scroll view and the current
                    //// child is not in visible region.
                    ((ISemanticsProvider)this.MauiSfView).ScrollTo(this.NextNode);
                }

                return this.NextSibling;
            }
            else if (direction == AutomationNavigationDirection.PreviousSibling && this.PrevSibling != null)
            {
                if (this.PrevNode != null)
                {
                    //// Perform scroll while the view placed inside the scroll view and the current
                    //// child is not in visible region.
                    ((ISemanticsProvider)this.MauiSfView).ScrollTo(this.PrevNode);
                }

                return this.PrevSibling;
            }

            return base.NavigateCore(direction);
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }

        /// <summary>
        /// Decide to handle the actions that related to this automation peer.
        /// </summary>
        /// <param name="patternInterface">Performed action.</param>
        /// <returns>The automation peer.</returns>
        protected override object GetPatternCore(PatternInterface patternInterface)
        {
            if (patternInterface == PatternInterface.Invoke)
                return this;

            return base.GetPatternCore(patternInterface);
        }

        /// <summary>
        /// Return the information that needed on hovering.
        /// </summary>
        /// <returns>The name information.</returns>
        protected override string GetNameCore()
        {
            return semanticsNode.Text;
        }

        /// <summary>
        /// Return the current automation peer bounds value related to screen.
        /// </summary>
        /// <returns>The rectangle bounds value.</returns>
        protected override Windows.Foundation.Rect GetBoundingRectangleCore()
        {
            Windows.Foundation.Rect rect = this.GetParent().GetBoundingRectangle();
            NativeGraphicsView owner = (NativeGraphicsView)this.Owner;
            double scale = owner.XamlRoot.RasterizationScale;
            Point viewStartPosition = AccessibilityHelper.GetViewStartPosition(owner, scale);
            Rect semanticNodeBounds = new Rect(this.semanticsNode.Bounds.Left * scale, this.semanticsNode.Bounds.Top * scale, this.semanticsNode.Bounds.Width * scale, this.semanticsNode.Bounds.Height * scale);
            Rect bounds = new Rect(viewStartPosition.X + semanticNodeBounds.Left, viewStartPosition.Y + semanticNodeBounds.Top, semanticNodeBounds.Width, semanticNodeBounds.Height);
            double yPosition = bounds.Y;
            double endYPosition = bounds.Y + bounds.Height;

            double xPosition = bounds.X;
            double endXPosition = bounds.X + bounds.Width;

            if (yPosition < rect.Top)
            {
                yPosition = rect.Top;
            }

            if (endYPosition > rect.Top + rect.Height)
            {
                endYPosition = rect.Top + rect.Height;
            }

            if (xPosition < rect.Left)
            {
                xPosition = rect.Left;
            }

            if (endXPosition > rect.Left + rect.Width)
            {
                endXPosition = rect.Left + rect.Width;
            }

            double width = endXPosition - xPosition;
            if (width < 0)
            {
                width = 0;
            }

            double height = endYPosition - yPosition;
            if (height < 0)
            {
                height = 0;
            }

            return new Windows.Foundation.Rect(xPosition, yPosition, width, height);
        }

        /// <summary>
        /// Sends a request to initiate or perform the invoke action of the provider control.
        /// </summary>
        void IInvokeProvider.Invoke()
        {
            if (this.semanticsNode.OnClick == null)
            {
                return;
            }

            this.semanticsNode.OnClick(this.semanticsNode);
        }
    }

    /// <summary>
    /// Holds the methods that used for accessibility implementation.
    /// </summary>
    internal static class AccessibilityHelper
    {
        /// <summary>
        /// Return the view start position form screen.
        /// </summary>
        /// <param name="element">Element or layout that needed to calculate the start position.</param>
        /// <param name="scale">Scale value of the screen.</param>
        /// <returns></returns>
        internal static Point GetViewStartPosition(UIElement element, double scale)
        {
            //// Specifying null consider as root element(application window).
            GeneralTransform transform = element.TransformToVisual(null);
            //// Convert the layout point(0,0) into screen position.
            Windows.Foundation.Point startPoint = transform.TransformPoint(new Windows.Foundation.Point(0, 0));
            //// return the value based on the scale value.
            return new Point(startPoint.X * scale, startPoint.Y * scale);
        }
    }
}