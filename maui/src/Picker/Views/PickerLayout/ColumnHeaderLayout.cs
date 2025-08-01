using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// This represents a class that contains information about the picker column header layout.
    /// </summary>
    internal class ColumnHeaderLayout : SfView
    {
        #region Fields

        /// <summary>
        /// The picker view info.
        /// </summary>
        readonly IColumnHeaderView _columnHeaderInfo;

        /// <summary>
        /// The column header text.
        /// </summary>
        string _columnHeaderText;

        /// <summary>
        /// Gets or sets the virtual column header layout semantic nodes.
        /// </summary>
        List<SemanticsNode>? _semanticsNodes;

        /// <summary>
        /// Gets or sets the size of the semantic.
        /// </summary>
        Size _semanticsSize = Size.Zero;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnHeaderLayout"/> class.
        /// </summary>
        /// <param name="columnHeaderInfo">The picker column header view details.</param>
        /// <param name="headerText">The column header text value.</param>
        internal ColumnHeaderLayout(IColumnHeaderView columnHeaderInfo, string headerText)
        {
            DrawingOrder = DrawingOrder.BelowContent;
            _columnHeaderInfo = columnHeaderInfo;
            if (columnHeaderInfo.ColumnHeaderTemplate != null)
            {
                InitializeTemplateView();
            }

            _columnHeaderText = headerText;
#if IOS
            IgnoreSafeArea = true;
#endif
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method to trigger the picker column header draw.
        /// </summary>
        internal void TriggerColumnHeaderDraw()
        {
            InvalidateDrawable();
        }

        /// <summary>
        /// Method to update the column header text.
        /// </summary>
        /// <param name="columnHeaderText">The column header text.</param>
        internal void UpdateColumnHeaderText(string columnHeaderText)
        {
            if (_columnHeaderText == columnHeaderText || _columnHeaderInfo.ColumnHeaderTemplate != null)
            {
                return;
            }

            _columnHeaderText = columnHeaderText;
            InvalidateDrawable();
        }

        /// <summary>
        /// Method to create a template view.
        /// </summary>
        internal void InitializeTemplateView()
        {
            View? colmnHeaderTemplateView = null;
            if (_columnHeaderInfo.ColumnHeaderTemplate == null)
            {
                return;
            }

            //// Clear the previous data in columnheaderlayout.
            if (Children.Count > 0)
            {
                Children.Clear();
            }

            DataTemplate columnHeaderTemplate = _columnHeaderInfo.ColumnHeaderTemplate;

            switch (_columnHeaderInfo)
            {
                case SfDatePicker datepicker:
                    colmnHeaderTemplateView = PickerHelper.CreateLayoutTemplateViews(columnHeaderTemplate, _columnHeaderInfo.ColumnHeaderView, datepicker);
                    break;
                case SfDateTimePicker:
                    SfDateTimePicker datetimepicker = (SfDateTimePicker)_columnHeaderInfo;
                    colmnHeaderTemplateView = PickerHelper.CreateLayoutTemplateViews(columnHeaderTemplate, _columnHeaderInfo.ColumnHeaderView, datetimepicker);
                    break;
                case SfPicker picker:
                    colmnHeaderTemplateView = PickerHelper.CreateLayoutTemplateViews(columnHeaderTemplate, _columnHeaderInfo.ColumnHeaderView, picker);
                    break;
                case SfTimePicker timepicker:
                    colmnHeaderTemplateView = PickerHelper.CreateLayoutTemplateViews(columnHeaderTemplate, _columnHeaderInfo.ColumnHeaderView, timepicker);
                    break;
            }

            if (Children.Count == 0 && colmnHeaderTemplateView != null)
            {
                Children.Add(colmnHeaderTemplateView);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to trim the text based on available width.
        /// </summary>
        /// <param name="text">The text to trim.</param>
        /// <param name="width">The available width.</param>
        /// <param name="textStyle">The text style.</param>
        /// <returns>Returns the text for the available width.</returns>
        static string TrimText(string text, double width, PickerTextStyle textStyle)
        {
            if (width <= 0)
            {
                return string.Empty;
            }

            Size textSize = text.Measure(textStyle);
            if (textSize.Width < width)
            {
                return text;
            }

            string textTrim = text;
            while ((int)textSize.Width + 1 > width)
            {
                if (textTrim.Length == 0)
                {
                    break;
                }

                textTrim = textTrim.Substring(0, textTrim.Length - 1);
                textSize = (textTrim + "..").Measure((float)textStyle.FontSize, textStyle.FontAttributes, textStyle.FontFamily);
            }

            return textTrim + "..";
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// Method to draw the separator line for column header and picker layout.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRect">The dirtyRectangle.</param>
        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            if (dirtyRect.Width == 0 || dirtyRect.Height == 0 || _columnHeaderInfo.ColumnHeaderTemplate != null)
            {
                return;
            }

            canvas.SaveState();
            float width = dirtyRect.Width;
            Rect rectangle = new Rect(0, 0, width, dirtyRect.Height);
            Color fillColor = _columnHeaderInfo.ColumnHeaderView.Background.ToColor();
            if (fillColor != Colors.Transparent)
            {
                canvas.FillColor = fillColor;
                canvas.FillRectangle(rectangle);
            }

            string? headerText = TrimText(_columnHeaderText, width, _columnHeaderInfo.ColumnHeaderView.TextStyle);
            if (!string.IsNullOrEmpty(headerText))
            {
                canvas.DrawText(headerText, rectangle, HorizontalAlignment.Center, VerticalAlignment.Center, _columnHeaderInfo.ColumnHeaderView.TextStyle);
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// Method to create the semantics node for header layout.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>Returns semantic virtual view.</returns>
        protected override List<SemanticsNode>? GetSemanticsNodesCore(double width, double height)
        {
            Size newSize = new Size(width, height);

            _semanticsNodes = new List<SemanticsNode>();
            _semanticsSize = newSize;
            Rect rectangle = new Rect(0, 0, width, height);
            SemanticsNode node = new SemanticsNode()
            {
                Id = 0,
                Text = _columnHeaderText,
                Bounds = rectangle,
                IsTouchEnabled = true,
            };
            _semanticsNodes.Add(node);
            return _semanticsNodes;
        }

        #endregion
    }
}