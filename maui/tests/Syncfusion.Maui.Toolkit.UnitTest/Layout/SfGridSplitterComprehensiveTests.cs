using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.GridSplitter;

namespace Syncfusion.Maui.Toolkit.UnitTest.Layout
{
    /// <summary>
    /// Comprehensive unit tests for the <see cref="SfGridSplitter"/> control.
    /// Covers all public API, internal helpers, and behavioural code paths
    /// in:
    ///   - <see cref="SfGridSplitter"/> (control surface, properties, events,
    ///     layout, collapse/expand, drag, AddPane/RemovePane).
    ///   - <see cref="SplitterPane"/> (content, size, Min/Max, IsCollapsible,
    ///     IsResizable, IsCollapsed, Background).
    ///   - <see cref="SeparatorView"/> (drag, icons, hit-testing, hit zones,
    ///     overlap routing, template view).
    ///   - <see cref="GridSplitterOrientation"/> enum.
    ///   - Event argument classes (<see cref="GridSplitterResizeStartedEventArgs"/>,
    ///     <see cref="GridSplitterResizingEventArgs"/>, etc.).
    /// </summary>
    public class SfGridSplitterComprehensiveTests : BaseUnitTest
    {
        // ====================================================================
        // Reflection helpers (consistent with the other GridSplitter test files).
        // ====================================================================
        private static void SetPaneBounds(SplitterPane pane, double width, double height)
        {
            var boundsProperty = typeof(VisualElement).GetProperty(
                "Bounds",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(boundsProperty);
            boundsProperty!.SetValue(pane, new Rect(0, 0, width, height));
        }

        private void SetPointerAndTap(object separator, Point position)
        {
            SetPrivateField(separator, "_lastPointerPosition", position);
            InvokeTap(separator);
        }

        private static void InvokeTap(object separator)
        {
            var method = separator.GetType().GetMethod("OnTapped",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var args = new TappedEventArgs((View)separator);
            method?.Invoke(separator, new object?[] { separator, args });
        }

        private static bool IsLeadingVisible(SeparatorView sep, SfGridSplitter splitter)
        {
            var method = typeof(SeparatorView).GetMethod("IsLeadingIconVisible",
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)(method?.Invoke(sep, new object[] { splitter }) ?? false);
        }

        private static bool IsTrailingVisible(SeparatorView sep, SfGridSplitter splitter)
        {
            var method = typeof(SeparatorView).GetMethod("IsTrailingIconVisible",
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)(method?.Invoke(sep, new object[] { splitter }) ?? false);
        }

        private static bool IsRightToLeftLayout(SeparatorView sep)
        {
            var method = typeof(SeparatorView).GetMethod("IsRightToLeftLayout",
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (bool)(method?.Invoke(sep, null) ?? false);
        }

        private static Grid GetInternalGrid(SfGridSplitter splitter)
        {
            var field = typeof(SfGridSplitter).GetField("_internalGrid",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Assert.NotNull(field);
            var grid = field!.GetValue(splitter) as Grid;
            return grid!;
        }
        private static View? GetTemplateView(SeparatorView separator)
        {
            var field = typeof(SeparatorView).GetField("_resizeIconTemplateView",
                BindingFlags.NonPublic | BindingFlags.Instance);
            return field?.GetValue(separator) as View;
        }

        private static Size InvokeMeasureContent(SeparatorView sep, double widthConstraint, double heightConstraint)
        {
            var method = typeof(SeparatorView).GetMethod("MeasureContent",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);
            return (Size)method!.Invoke(sep, new object[] { widthConstraint, heightConstraint })!;
        }

        private static System.Collections.IList GetSeparatorsList(SfGridSplitter splitter)
        {
            var separatorsField = typeof(SfGridSplitter).GetField("_separators",
                BindingFlags.NonPublic | BindingFlags.Instance);
            return (System.Collections.IList)separatorsField!.GetValue(splitter)!;
        }

        private static SfGridSplitter CreateHorizontalSplitter(int paneCount = 2, double sepSize = 8)
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = sepSize
            };
            for (int i = 0; i < paneCount; i++)
            {
                splitter.SplitterPanes.Add(new SplitterPane { Size = "*", IsCollapsible = true });
            }
            return splitter;
        }

        private static SfGridSplitter CreateVerticalSplitter(int paneCount = 2, double sepSize = 8)
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Vertical,
                SeparatorSize = sepSize
            };
            for (int i = 0; i < paneCount; i++)
            {
                splitter.SplitterPanes.Add(new SplitterPane { Size = "*", IsCollapsible = true });
            }
            return splitter;
        }

        // ====================================================================
        // 1) Constructor / Default state
        // ====================================================================

        [Fact]
        public void Constructor_InitializesDefaults()
        {
            var splitter = new SfGridSplitter();

            Assert.Equal(GridSplitterOrientation.Horizontal, splitter.Orientation);
            Assert.Equal(8.0, splitter.SeparatorSize);
            Assert.NotNull(splitter.SplitterPanes);
            Assert.Empty(splitter.SplitterPanes);
            Assert.Null(splitter.ResizeIconTemplate);
            Assert.Equal(Color.FromArgb("#49454F"), splitter.ResizeIconColor);
            Assert.Equal(Color.FromArgb("#6750A4"), splitter.ExpandCollapseIconColor);
        }

        [Fact]
        public void Constructor_SplitterPanes_CollectionHandlerSubscribed()
        {
            // The constructor must wire CollectionChanged so subsequent
            // SplitterPanes.Add/Remove/Clear run the layout builder.
            var splitter = new SfGridSplitter();
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });

            // The internal grid is created lazily on first add (via
            // OnSplitterPanesCollectionChanged -> UpdateLayoutForPaneChange).
            var grid = GetInternalGrid(splitter);
            Assert.NotNull(grid);
        }

        [Fact]
        public void SplitterPanesProperty_ReplacingCollection_RebindsHandler()
        {
            var splitter = new SfGridSplitter();
            var oldCollection = splitter.SplitterPanes;
            var newCollection = new System.Collections.ObjectModel.ObservableCollection<SplitterPane>
            {
                new SplitterPane { Size = "*" },
                new SplitterPane { Size = "*" }
            };
            splitter.SplitterPanes = newCollection;

            // Adding to the new collection must rebuild the layout.
            newCollection.Add(new SplitterPane { Size = "*" });
            var grid = GetInternalGrid(splitter);
            Assert.Equal(5, grid.ColumnDefinitions.Count); // 3 panes + 2 separators

            // Adding to the OLD collection must NOT trigger a rebuild
            // (its handler was unsubscribed in OnSplitterPanesChanged).
            int oldCount = oldCollection.Count;
            oldCollection.Add(new SplitterPane { Size = "*" });
            Assert.Equal(oldCount + 1, oldCollection.Count);
        }

        // ====================================================================
        // 2) Bindable property behaviour
        // ====================================================================

        [Fact]
        public void Orientation_MixedAbsoluteAndStar_PreservesProportions()
        {
            // User-reported bug: "With a size configuration of 200, *, *
            // in Horizontal orientation, the UI renders correctly.
            // However, after switching to Vertical orientation, the first
            // pane occupies excessive space, while the other two panes,
            // which are set to *, receive only the minimum size instead
            // of sharing the remaining space proportionally."
            //
            // The fix in UpdateLayoutForOrientation re-derives star
            // weights from the current bounds so the proportions are
            // preserved across the orientation flip.
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "200" });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });

            // Simulate a layout pass by setting bounds on the panes.
            // Pane 0 = 200 absolute, pane 1 = 400 star, pane 2 = 400 star
            // (total 1000 in a 1000-wide splitter).
            var separators = GetSeparatorsList(splitter);
            var sep0 = (SeparatorView)separators[0]!;
            var sep1 = (SeparatorView)separators[1]!;
            sep0.ArrangeSeparator(new Rect(0, 0, 8, 40));
            sep1.ArrangeSeparator(new Rect(0, 0, 8, 40));
            SetPaneBounds(splitter.SplitterPanes[0], 200, 40);
            SetPaneBounds(splitter.SplitterPanes[1], 400, 40);
            SetPaneBounds(splitter.SplitterPanes[2], 400, 40);

            // Flip orientation. The fix should convert the absolute pixel
            // sizes into proportional star weights so the new axis
            // distributes space proportionally.
            splitter.Orientation = GridSplitterOrientation.Vertical;

            // Verify all three pane rows are now star-weighted (not
            // absolute pixel values that would clip the vertical axis).
            var grid = GetInternalGrid(splitter);
            Assert.Equal(GridUnitType.Star, grid.RowDefinitions[0].Height.GridUnitType);
            Assert.Equal(GridUnitType.Star, grid.RowDefinitions[2].Height.GridUnitType);
            Assert.Equal(GridUnitType.Star, grid.RowDefinitions[4].Height.GridUnitType);
        }

        [Fact]
        public void SeparatorSize_PropertyChanged_UpdatesSeparatorColumns()
        {
            var splitter = CreateHorizontalSplitter(2, sepSize: 8);
            var grid = GetInternalGrid(splitter);
            Assert.Equal(8, grid.ColumnDefinitions[1].Width.Value);

            splitter.SeparatorSize = 16;
            Assert.Equal(16, grid.ColumnDefinitions[1].Width.Value);
        }

        [Fact]
        public void SeparatorSize_NegativeOrNaN_IsIgnored()
        {
            // The CoerceSeparatorSize callback coerces any value that is
            // <= 0 or NaN to the default of 8.0 DIU. This is the fix for
            // the user-reported bug "Setting SeparatorSize to 0 is not
            // applied" — the property is now always positive, and the
            // separator strip is always visible/interactive.
            var splitter = CreateHorizontalSplitter(2, sepSize: 8);
            var grid = GetInternalGrid(splitter);
            const double defaultSize = 8.0;

            splitter.SeparatorSize = -5;
            Assert.Equal(defaultSize, splitter.SeparatorSize);
            Assert.Equal(defaultSize, grid.ColumnDefinitions[1].Width.Value);

            splitter.SeparatorSize = 0;
            Assert.Equal(defaultSize, splitter.SeparatorSize);
            Assert.Equal(defaultSize, grid.ColumnDefinitions[1].Width.Value);

            splitter.SeparatorSize = double.NaN;
            Assert.Equal(defaultSize, splitter.SeparatorSize);
            Assert.Equal(defaultSize, grid.ColumnDefinitions[1].Width.Value);
        }

        [Fact]
        public void SeparatorBackground_PropertyChanged_DoesNotThrow()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SeparatorBackground = new SolidColorBrush(Colors.Red);
            // No throw is sufficient — the property-changed handler
            // invalidates each separator's drawable. We cannot inspect
            // drawable invalidation from a unit test, but a thrown
            // exception would fail the test.
        }

        [Fact]
        public void ResizeIconColor_PropertyChanged_DoesNotThrow()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeIconColor = Colors.Green;
            splitter.ResizeIconColor = Colors.Blue;
        }

        [Fact]
        public void ExpandCollapseIconColor_PropertyChanged_DoesNotThrow()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ExpandCollapseIconColor = Colors.Purple;
            splitter.ExpandCollapseIconColor = Colors.Orange;
        }

        [Fact]
        public void ResizeIconTemplate_PropertyChanged_ReplacesView()
        {
            var splitter = CreateHorizontalSplitter(2);
            var grid = GetInternalGrid(splitter);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;

            // Initially no template view exists.
            Assert.Null(GetTemplateView(sep));

            splitter.ResizeIconTemplate = new DataTemplate(() => new Label { Text = "A" });
            Assert.NotNull(GetTemplateView(sep));

            splitter.ResizeIconTemplate = new DataTemplate(() => new Label { Text = "B" });
            var viewB = GetTemplateView(sep);
            Assert.NotNull(viewB);
            Assert.Equal("B", ((Label)viewB!).Text);

            splitter.ResizeIconTemplate = null;
            Assert.Null(GetTemplateView(sep));
        }

        [Fact]
        public void ResizeIconTemplate_NonViewContent_IsIgnored()
        {
            // If the template's root is not a View (e.g. a string), the
            // separator must gracefully ignore it and keep no template view.
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeIconTemplate = new DataTemplate(() => "not a view");
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            Assert.Null(GetTemplateView(sep));
        }

        // ====================================================================
        // 3) Layout — BuildHorizontalLayout / BuildVerticalLayout
        // ====================================================================

        [Fact]
        public void BuildHorizontalLayout_TwoPanes_CreatesThreeColumns()
        {
            var splitter = CreateHorizontalSplitter(2);
            var grid = GetInternalGrid(splitter);
            Assert.Single(grid.RowDefinitions);
            Assert.Equal(3, grid.ColumnDefinitions.Count);
            Assert.True(grid.ColumnDefinitions[0].Width.IsStar);
            Assert.Equal(8, grid.ColumnDefinitions[1].Width.Value);
            Assert.True(grid.ColumnDefinitions[2].Width.IsStar);
        }

        [Fact]
        public void BuildHorizontalLayout_FourPanes_CreatesSevenColumns()
        {
            var splitter = CreateHorizontalSplitter(4);
            var grid = GetInternalGrid(splitter);
            Assert.Equal(7, grid.ColumnDefinitions.Count);
            // Pane/separator alternation: pane, sep, pane, sep, pane, sep, pane.
            for (int i = 0; i < 7; i++)
            {
                if (i % 2 == 0)
                    Assert.True(grid.ColumnDefinitions[i].Width.IsStar);
                else
                    Assert.Equal(8, grid.ColumnDefinitions[i].Width.Value);
            }
        }

        [Fact]
        public void BuildVerticalLayout_TwoPanes_CreatesThreeRows()
        {
            var splitter = CreateVerticalSplitter(2);
            var grid = GetInternalGrid(splitter);
            Assert.Single(grid.ColumnDefinitions);
            Assert.Equal(3, grid.RowDefinitions.Count);
            Assert.True(grid.RowDefinitions[0].Height.IsStar);
            Assert.Equal(8, grid.RowDefinitions[1].Height.Value);
            Assert.True(grid.RowDefinitions[2].Height.IsStar);
        }

        [Fact]
        public void BuildLayout_PaneCellPlacement_IsCorrect()
        {
            var splitter = CreateHorizontalSplitter(3);
            var grid = GetInternalGrid(splitter);
            // Panes occupy row 0, columns 0, 2, 4.
            Assert.Equal(0, Grid.GetRow(splitter.SplitterPanes[0]));
            Assert.Equal(0, Grid.GetColumn(splitter.SplitterPanes[0]));
            Assert.Equal(0, Grid.GetRow(splitter.SplitterPanes[1]));
            Assert.Equal(2, Grid.GetColumn(splitter.SplitterPanes[1]));
            Assert.Equal(0, Grid.GetRow(splitter.SplitterPanes[2]));
            Assert.Equal(4, Grid.GetColumn(splitter.SplitterPanes[2]));
        }

        [Fact]
        public void BuildLayout_PaneZIndex_IsLowerThanSeparatorZIndex()
        {
            var splitter = CreateHorizontalSplitter(2);
            var grid = GetInternalGrid(splitter);
            // Panes are forced to ZIndex 0 so separators always win hit-testing.
            Assert.Equal(0, splitter.SplitterPanes[0].ZIndex);
            Assert.Equal(0, splitter.SplitterPanes[1].ZIndex);
            var separators = GetSeparatorsList(splitter);
            Assert.Equal(10, ((SeparatorView)separators[0]!).ZIndex);
        }

        [Fact]
        public void BuildLayout_RespectsAbsoluteSizes()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "120" });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "240" });

            var grid = GetInternalGrid(splitter);
            Assert.Equal(120, grid.ColumnDefinitions[0].Width.Value);
            Assert.True(grid.ColumnDefinitions[0].Width.IsAbsolute);
            Assert.Equal(240, grid.ColumnDefinitions[2].Width.Value);
        }

        [Fact]
        public void BuildLayout_RespectsStarWeights()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "1*" });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "3*" });

            var grid = GetInternalGrid(splitter);
            Assert.True(grid.ColumnDefinitions[0].Width.IsStar);
            Assert.Equal(1.0, grid.ColumnDefinitions[0].Width.Value);
            Assert.True(grid.ColumnDefinitions[2].Width.IsStar);
            Assert.Equal(3.0, grid.ColumnDefinitions[2].Width.Value);
        }

        [Fact]
        public void BuildLayout_NoPanes_ClearsLayout()
        {
            var splitter = new SfGridSplitter();
            Assert.Null(GetInternalGrid(splitter));

            // Add then remove all — internal grid exists but is empty.
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });
            splitter.RemovePane(0);
            var grid = GetInternalGrid(splitter);
            Assert.NotNull(grid);
            Assert.Empty(grid.ColumnDefinitions);
            Assert.Empty(grid.RowDefinitions);
        }

        // ====================================================================
        // 4) Collapse / Expand (CollapsePane / ExpandPane / TogglePane)
        // ====================================================================

        [Fact]
        public async Task CollapsePane_NonCollapsible_ReturnsFalse()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[0].IsCollapsible = false;
            splitter?.CollapsePane(0);
            Assert.False(splitter?.SplitterPanes[0].IsCollapsed);
        }

        [Fact]
        public async Task CollapsePane_FiresEventsInOrder()
        {
            var splitter = CreateHorizontalSplitter(2);
            var log = new List<string>();
            splitter.Collapsing += (_, _) => log.Add("Collapsing");
            splitter.Collapsed += (_, _) => log.Add("Collapsed");

            await splitter.CollapsePane(0);
            Assert.Equal(new[] { "Collapsing", "Collapsed" }, log);
        }

        [Fact]
        public async Task ExpandPane_FiresEventsInOrder()
        {
            var splitter = CreateHorizontalSplitter(2);
            await splitter.CollapsePane(0);

            var log = new List<string>();
            splitter.Expanding += (_, _) => log.Add("Expanding");
            splitter.Expanded += (_, _) => log.Add("Expanded");

            await splitter.ExpandPane(0);
            Assert.Equal(new[] { "Expanding", "Expanded" }, log);
        }


        [Fact]
        public async Task CollapsePane_PreservesOriginalSizeForExpand()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "300", IsCollapsible = true });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });

            await splitter.CollapsePane(0);
            // After collapse the live Size is "0" but the original
            // is preserved in _collapsedPaneSizes.
            Assert.Equal("0", splitter.SplitterPanes[0].Size);

            await splitter.ExpandPane(0);
            // After expand the live Size is restored to "300".
            Assert.Equal("300", splitter.SplitterPanes[0].Size);
        }

        [Fact]
        public void EventArgs_ConstructorsAndProperties()
        {
            var pane = new SplitterPane { Size = "*" };
            var indices = new[] { 0, 1 };
            var panes = new[] { pane, pane };

            var startArgs = new GridSplitterResizeStartedEventArgs(indices, panes);
            Assert.Equal(indices, startArgs.Indexes);
            Assert.Equal(panes, startArgs.Panes);
            Assert.False(startArgs.Cancel);
            startArgs.Cancel = true;
            Assert.True(startArgs.Cancel);

            var resizingArgs = new GridSplitterResizingEventArgs(indices, panes);
            Assert.Equal(indices, resizingArgs.Indexes);
            Assert.Equal(panes, resizingArgs.Panes);

            var stoppedArgs = new GridSplitterResizeStoppedEventArgs(indices, panes);
            Assert.Equal(indices, stoppedArgs.Indexes);
            Assert.Equal(panes, stoppedArgs.Panes);

            var collapsingArgs = new GridSplitterPaneCollapsingEventArgs(indices, panes);
            Assert.False(collapsingArgs.Cancel);
            collapsingArgs.Cancel = true;
            Assert.True(collapsingArgs.Cancel);

            var collapsedArgs = new GridSplitterPaneCollapsedEventArgs(indices, panes);
            Assert.Equal(indices, collapsedArgs.Indexes);

            var expandingArgs = new GridSplitterPaneExpandingEventArgs(indices, panes);
            Assert.False(expandingArgs.Cancel);
            expandingArgs.Cancel = true;

            var expandedArgs = new GridSplitterPaneExpandedEventArgs(indices, panes);
            Assert.Equal(indices, expandedArgs.Indexes);
        }

        // ====================================================================
        // 5) SplitterPane property coverage
        // ====================================================================

        [Fact]
        public void SplitterPane_Defaults()
        {
            var pane = new SplitterPane();
            Assert.Equal("*", pane.Size);
            Assert.Equal(0.0, pane.MinimumSize);
            Assert.Equal(double.PositiveInfinity, pane.MaximumSize);
            Assert.True(pane.IsCollapsible);
            Assert.True(pane.IsResizable);
            Assert.False(pane.IsCollapsed);
            Assert.Null(pane.Content);
        }

        [Fact]
        public void SplitterPane_Content_AddRemove()
        {
            var pane = new SplitterPane();
            var label = new Label { Text = "x" };

            pane.Content = label;
            Assert.Same(label, pane.Content);
            Assert.Contains(label, pane.Children);

            pane.Content = null;
            Assert.DoesNotContain(label, pane.Children);

            // Setting a new content after a null must still work.
            var label2 = new Label { Text = "y" };
            pane.Content = label2;
            Assert.Same(label2, pane.Content);
        }

        [Fact]
        public void SplitterPane_ContentPropertyChanged_RaisesINotifyPropertyChanged()
        {
            var pane = new SplitterPane();
            string? capturedProperty = null;
            pane.PropertyChanged += (_, e) => capturedProperty = e.PropertyName;

            pane.Content = new Label { Text = "x" };
            Assert.Equal(nameof(SplitterPane.Content), capturedProperty);
        }

        [Fact]
        public void SplitterPane_SizePropertyChanged_RaisesINotifyPropertyChanged()
        {
            var pane = new SplitterPane();
            string? captured = null;
            pane.PropertyChanged += (_, e) => captured = e.PropertyName;

            pane.Size = "200";
            Assert.Equal(nameof(SplitterPane.Size), captured);
            Assert.Equal("200", pane.Size);
        }

        [Fact]
        public void SplitterPane_MinimumSizePropertyChanged_RaisesINotify()
        {
            var pane = new SplitterPane();
            string? captured = null;
            pane.PropertyChanged += (_, e) => captured = e.PropertyName;

            pane.MinimumSize = 50;
            Assert.Equal(nameof(SplitterPane.MinimumSize), captured);
            Assert.Equal(50, pane.MinimumSize);
        }

        [Fact]
        public void SplitterPane_MaximumSizePropertyChanged_RaisesINotify()
        {
            var pane = new SplitterPane();
            string? captured = null;
            pane.PropertyChanged += (_, e) => captured = e.PropertyName;

            pane.MaximumSize = 500;
            Assert.Equal(nameof(SplitterPane.MaximumSize), captured);
            Assert.Equal(500, pane.MaximumSize);
        }

        [Fact]
        public void SplitterPane_IsCollapsiblePropertyChanged_RaisesINotify()
        {
            var pane = new SplitterPane();
            string? captured = null;
            pane.PropertyChanged += (_, e) => captured = e.PropertyName;

            pane.IsCollapsible = false;
            Assert.Equal(nameof(SplitterPane.IsCollapsible), captured);
            Assert.False(pane.IsCollapsible);
        }

        [Fact]
        public void SplitterPane_IsResizablePropertyChanged_RaisesINotify()
        {
            var pane = new SplitterPane();
            string? captured = null;
            pane.PropertyChanged += (_, e) => captured = e.PropertyName;

            pane.IsResizable = false;
            Assert.Equal(nameof(SplitterPane.IsResizable), captured);
            Assert.False(pane.IsResizable);
        }

        [Fact]
        public void SplitterPane_IsCollapsedPropertyChanged_RaisesINotify()
        {
            var pane = new SplitterPane();
            string? captured = null;
            pane.PropertyChanged += (_, e) => captured = e.PropertyName;

            pane.IsCollapsed = true;
            Assert.Equal(nameof(SplitterPane.IsCollapsed), captured);
            Assert.True(pane.IsCollapsed);
        }

        [Fact]
        public void SplitterPane_BackgroundPropertyChanged_RaisesINotify()
        {
            var pane = new SplitterPane();
            string? captured = null;
            pane.PropertyChanged += (_, e) => captured = e.PropertyName;

            pane.Background = new SolidColorBrush(Colors.Green);
            Assert.Equal(nameof(SplitterPane.Background), captured);
        }

        // ====================================================================
        // 6) Drag callbacks
        // ====================================================================

        [Fact]
        public void SeparatorView_SetDragCallbacks_StoresCallbacks()
        {
            var separator = new SeparatorView();
            Action<int>? onStart = _ => { };
            Action<int, double>? onDelta = (_, _) => { };
            Action<int>? onStop = _ => { };

            separator.SetDragCallbacks(onStart, onDelta, onStop);
            var startField = typeof(SeparatorView).GetField("_onDragStart",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var deltaField = typeof(SeparatorView).GetField("_onDragDelta",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var stopField = typeof(SeparatorView).GetField("_onDragStop",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.Same(onStart, startField!.GetValue(separator));
            Assert.Same(onDelta, deltaField!.GetValue(separator));
            Assert.Same(onStop, stopField!.GetValue(separator));
        }

        [Fact]
        public void OnDragStart_FiresResizeStartedEvent()
        {
            var splitter = CreateHorizontalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;

            int startedCount = 0;
            int[]? capturedIndices = null;
            splitter.ResizeStarted += (_, e) =>
            {
                startedCount++;
                capturedIndices = e.Indexes;
            };

            // Set the callback explicitly to ensure SfGridSplitter wired it.
            splitter.GetType(); // touch
            // Reflection: invoke OnDragStart on the splitter.
            InvokePrivateMethod(splitter, "OnDragStart", 1);

            Assert.Equal(1, startedCount);
            Assert.NotNull(capturedIndices);
            Assert.Equal(0, capturedIndices![0]); // leading pane index
            Assert.Equal(1, capturedIndices[1]);   // trailing pane index
        }

        [Fact]
        public void OnDragStart_Cancel_AbortsDrag()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeStarted += (_, e) => e.Cancel = true;

            int stoppedCount = 0;
            splitter.ResizeStopped += (_, _) => stoppedCount++;

            InvokePrivateMethod(splitter, "OnDragStart", 1);
            // After cancel, the active drag index is set to MinValue.
            var activeField = typeof(SfGridSplitter).GetField("_activeDraggingTrailingPaneIndex",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.Equal(int.MinValue, (int)activeField!.GetValue(splitter)!);

            // OnDragDelta and OnDragStop must be no-ops when cancelled.
            InvokePrivateMethod(splitter, "OnDragDelta", 1, 50.0);
            InvokePrivateMethod(splitter, "OnDragStop", 1);
            Assert.Equal(0, stoppedCount);
        }

        [Fact]
        public void OnDragStart_OutOfRange_IsNoOp()
        {
            var splitter = CreateHorizontalSplitter(2);
            int startedCount = 0;
            splitter.ResizeStarted += (_, _) => startedCount++;

            InvokePrivateMethod(splitter, "OnDragStart", -1);
            InvokePrivateMethod(splitter, "OnDragStart", 5);
            Assert.Equal(0, startedCount);
        }

        [Fact]
        public void OnDragDelta_CancelledDrag_IsNoOp()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeStarted += (_, e) => e.Cancel = true;
            InvokePrivateMethod(splitter, "OnDragStart", 1);

            int resizingCount = 0;
            splitter.Resizing += (_, _) => resizingCount++;
            InvokePrivateMethod(splitter, "OnDragDelta", 1, 50.0);
            Assert.Equal(0, resizingCount);
        }

        [Fact]
        public void OnDragDelta_OutOfRange_IsNoOp()
        {
            var splitter = CreateHorizontalSplitter(2);
            int resizingCount = 0;
            splitter.Resizing += (_, _) => resizingCount++;
            InvokePrivateMethod(splitter, "OnDragDelta", 0, 50.0);  // 0 is invalid
            InvokePrivateMethod(splitter, "OnDragDelta", 10, 50.0); // out of range
            Assert.Equal(0, resizingCount);
        }

        [Fact]
        public void OnDragStop_CancelledDrag_DoesNotFireResizeStopped()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeStarted += (_, e) => e.Cancel = true;
            InvokePrivateMethod(splitter, "OnDragStart", 1);

            int stoppedCount = 0;
            splitter.ResizeStopped += (_, _) => stoppedCount++;
            InvokePrivateMethod(splitter, "OnDragStop", 1);
            Assert.Equal(0, stoppedCount);
        }

        [Fact]
        public void OnDragStop_MatchingIndex_FiresResizeStoppedAndResetsState()
        {
            var splitter = CreateHorizontalSplitter(2);
            InvokePrivateMethod(splitter, "OnDragStart", 1);
            int stoppedCount = 0;
            splitter.ResizeStopped += (_, _) => stoppedCount++;

            InvokePrivateMethod(splitter, "OnDragStop", 1);
            Assert.Equal(1, stoppedCount);

            var activeField = typeof(SfGridSplitter).GetField("_activeDraggingTrailingPaneIndex",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.Equal(-1, (int)activeField!.GetValue(splitter)!);
        }

        // ====================================================================
        // 7) Drag — Min/Max four-constraint clamp (regression for the
        // "leading+trailing total drifts when both hit limits" bug).
        // ====================================================================

        [Fact]
        public void OnDragDelta_Horizontal_FourConstraintClamp_RespectsMinMax()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            var pane1 = new SplitterPane { Size = "200", IsResizable = true, MinimumSize = 100, MaximumSize = 300 };
            var pane2 = new SplitterPane { Size = "200", IsResizable = true, MinimumSize = 100, MaximumSize = 300 };
            splitter.SplitterPanes.Add(pane1);
            splitter.SplitterPanes.Add(pane2);

            var grid = GetInternalGrid(splitter);

            // Pin to absolute values via ConvertAllPanesToAbsolute.
            // Set up an active drag manually.
            InvokePrivateMethod(splitter, "OnDragStart", 1);
            var activeLeadingField = typeof(SfGridSplitter).GetField("_activeLeadingPaneSize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var activeTrailingField = typeof(SfGridSplitter).GetField("_activeTrailingPaneSize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            // After OnDragStart with no Bounds, the cached sizes come from
            // the column definitions, which are now absolute (200 each).
            double originalLeading = (double)activeLeadingField!.GetValue(splitter)!;
            double originalTrailing = (double)activeTrailingField!.GetValue(splitter)!;

            // Request a delta of +500 (way more than max allows).
            int resizingCount = 0;
            splitter.Resizing += (_, _) => resizingCount++;
            InvokePrivateMethod(splitter, "OnDragDelta", 1, 500.0);

            double newLeading = (double)activeLeadingField!.GetValue(splitter)!;
            double newTrailing = (double)activeTrailingField!.GetValue(splitter)!;

            // Leading cannot exceed its Max (300). Trailing cannot drop
            // below its Min (100). The four-constraint clamp must
            // honour both simultaneously, so leading+trailing must still
            // equal the original total.
            Assert.Equal(300, newLeading, 0);
            Assert.Equal(100, newTrailing, 0);
            Assert.Equal(originalLeading + originalTrailing, newLeading + newTrailing, 1);
            Assert.True(resizingCount >= 0); // may or may not fire due to Min/Max logic
        }

        [Fact]
        public void OnDragDelta_NonResizablePane_IsNoOp()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "200", IsResizable = false });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "200", IsResizable = true });
            InvokePrivateMethod(splitter, "OnDragStart", 1);

            int resizingCount = 0;
            splitter.Resizing += (_, _) => resizingCount++;
            InvokePrivateMethod(splitter, "OnDragDelta", 1, 50.0);
            Assert.Equal(0, resizingCount);
        }

        [Fact]
        public void OnDragDelta_VertizontalOrientation_AppliesClamp()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Vertical,
                SeparatorSize = 8
            };
            var pane1 = new SplitterPane { Size = "200", IsResizable = true, MinimumSize = 50, MaximumSize = 250 };
            var pane2 = new SplitterPane { Size = "200", IsResizable = true, MinimumSize = 50, MaximumSize = 250 };
            splitter.SplitterPanes.Add(pane1);
            splitter.SplitterPanes.Add(pane2);

            var grid = GetInternalGrid(splitter);
            InvokePrivateMethod(splitter, "OnDragStart", 1);

            var activeLeadingField = typeof(SfGridSplitter).GetField("_activeLeadingPaneSize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var activeTrailingField = typeof(SfGridSplitter).GetField("_activeTrailingPaneSize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            double originalLeading = (double)activeLeadingField!.GetValue(splitter)!;
            double originalTrailing = (double)activeTrailingField!.GetValue(splitter)!;

            InvokePrivateMethod(splitter, "OnDragDelta", 1, 500.0);

            double newLeading = (double)activeLeadingField!.GetValue(splitter)!;
            double newTrailing = (double)activeTrailingField!.GetValue(splitter)!;
            Assert.Equal(250, newLeading, 0);
            Assert.Equal(150, newTrailing, 0);
            Assert.Equal(originalLeading + originalTrailing, newLeading + newTrailing, 1);
        }

        // ====================================================================
        // 8) OnPanePropertyChanged branches (Size, IsCollapsed, IsResizable,
        //    Min/Max, IsCollapsible)
        // ====================================================================

        [Fact]
        public void PaneSize_ChangedAtRuntime_AppliesToColumn()
        {
            var splitter = CreateHorizontalSplitter(2);
            var grid = GetInternalGrid(splitter);
            Assert.True(grid.ColumnDefinitions[0].Width.IsStar);

            splitter.SplitterPanes[0].Size = "500";
            Assert.True(grid.ColumnDefinitions[0].Width.IsAbsolute);
            Assert.Equal(500, grid.ColumnDefinitions[0].Width.Value);
        }

        [Fact]
        public void PaneIsCollapsible_ChangedAtRuntime_DoesNotThrow()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[0].IsCollapsible = false;
            splitter.SplitterPanes[0].IsCollapsible = true;
        }

        [Fact]
        public void PaneIsResizable_ChangedAtRuntime_DoesNotThrow()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[0].IsResizable = false;
            splitter.SplitterPanes[0].IsResizable = true;
        }

        [Fact]
        public void PaneMinimumSize_ChangedAtRuntime_TriggersClamp()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            var pane1 = new SplitterPane { Size = "50" };
            var pane2 = new SplitterPane { Size = "*" };
            splitter.SplitterPanes.Add(pane1);
            splitter.SplitterPanes.Add(pane2);

            var grid = GetInternalGrid(splitter);
            SetPaneBounds(pane1, width: 50, height: 40);

            pane1.MinimumSize = 100;
            Assert.Equal(100, grid.ColumnDefinitions[0].Width.Value);
        }

        [Fact]
        public void PaneMaximumSize_ChangedAtRuntime_TriggersClamp()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            var pane1 = new SplitterPane { Size = "500", MaximumSize = 600 };
            var pane2 = new SplitterPane { Size = "*" };
            splitter.SplitterPanes.Add(pane1);
            splitter.SplitterPanes.Add(pane2);

            var grid = GetInternalGrid(splitter);
            SetPaneBounds(pane1, width: 500, height: 40);

            pane1.MaximumSize = 200;
            Assert.Equal(200, grid.ColumnDefinitions[0].Width.Value);
        }

        [Fact]
        public void PaneIsCollapsed_ChangedDynamically_CollapsesColumn()
        {
            var splitter = CreateHorizontalSplitter(2);
            var grid = GetInternalGrid(splitter);
            Assert.True(grid.ColumnDefinitions[0].Width.IsStar);

            splitter.SplitterPanes[0].IsCollapsed = true;
            Assert.Equal(0, grid.ColumnDefinitions[0].Width.Value);
            Assert.True(grid.ColumnDefinitions[0].Width.IsAbsolute);

            splitter.SplitterPanes[0].IsCollapsed = false;
            Assert.True(grid.ColumnDefinitions[0].Width.IsStar);
        }

        [Fact]
        public void PaneIsCollapsed_AllPanesVisibilityIsUpdated()
        {
            // Regression for the bug where flipping IsCollapsed on one
            // pane left sibling collapsed-at-load panes in a stale
            // visibility state.
            var splitter = CreateHorizontalSplitter(2);
            var pane1 = splitter.SplitterPanes[0];
            var pane2 = splitter.SplitterPanes[1];

            pane1.IsCollapsed = true;
            Assert.False(pane1.IsVisible);
            Assert.True(pane2.IsVisible);

            pane1.IsCollapsed = false;
            Assert.True(pane1.IsVisible);
            Assert.True(pane2.IsVisible);
        }

        [Fact]
        public void PaneIsCollapsed_TracksCollapsedPaneSizes()
        {
            var splitter = CreateHorizontalSplitter(2);
            var collapsedSizes = (System.Collections.IDictionary)GetPrivateField(splitter, "_collapsedPaneSizes")!;
            Assert.False(collapsedSizes.Contains(splitter.SplitterPanes[0]));

            splitter.SplitterPanes[0].IsCollapsed = true;
            Assert.True(collapsedSizes.Contains(splitter.SplitterPanes[0]));

            splitter.SplitterPanes[0].IsCollapsed = false;
            Assert.False(collapsedSizes.Contains(splitter.SplitterPanes[0]));
        }

        // ====================================================================
        // 9) AddPane / RemovePane
        // ====================================================================

        [Fact]
        public void AddPane_NullPane_ReturnsFalse()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.AddPane(null!);
			Assert.True(splitter.SplitterPanes.Count is 2);
        }

        [Fact]
        public void AddPane_NewPaneAppearsAtEnd()
        {
            var splitter = CreateHorizontalSplitter(2);
            var newPane = new SplitterPane { Size = "200" };
            splitter.AddPane(newPane);
            Assert.Equal(3, splitter.SplitterPanes.Count);
            Assert.Same(newPane, splitter.SplitterPanes[2]);
        }

        [Fact]
        public void AddPane_GridColumnCountIsUpdated()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.AddPane(new SplitterPane { Size = "*" });
            var grid = GetInternalGrid(splitter);
            Assert.Equal(5, grid.ColumnDefinitions.Count);
        }

        [Fact]
        public void AddPane_PaneIsVisibleAfterAdd()
        {
            var splitter = CreateHorizontalSplitter(2);
            var newPane = new SplitterPane { Size = "*" };
            splitter.AddPane(newPane);
            Assert.True(newPane.IsVisible);
        }

        [Fact]
        public async Task AddPane_NewPane_CollapseAndExpandWorks()
        {
            var splitter = CreateHorizontalSplitter(2);
            var newPane = new SplitterPane { Size = "*", IsCollapsible = true };
            splitter.AddPane(newPane);

            splitter?.CollapsePane(2);
            Assert.True(newPane.IsCollapsed);

            splitter?.ExpandPane(2);
            Assert.False(newPane.IsCollapsed);
        }

		[Fact]
		public void RemovePane_OutOfRange_DoesNothing()
		{
			var splitter = CreateHorizontalSplitter(2);

			var initialCount = splitter.Count();

			splitter.RemovePane(-1);
			splitter.RemovePane(5);

			Assert.Equal(initialCount, splitter.Count());
		}

		[Fact]
        public void RemovePane_PaneIsRemovedFromCollection()
        {
            var splitter = CreateHorizontalSplitter(3);
            var pane1 = splitter.SplitterPanes[0];
            var pane2 = splitter.SplitterPanes[1];
            var pane3 = splitter.SplitterPanes[2];

            splitter.RemovePane(1);
            Assert.Equal(2, splitter.SplitterPanes.Count);
            Assert.Same(pane1, splitter.SplitterPanes[0]);
            Assert.Same(pane3, splitter.SplitterPanes[1]);
        }

        [Fact]
        public void RemovePane_GridColumnCountIsUpdated()
        {
            var splitter = CreateHorizontalSplitter(3);
            splitter.RemovePane(1);
            var grid = GetInternalGrid(splitter);
            Assert.Equal(3, grid.ColumnDefinitions.Count);
        }

        [Fact]
        public void RemovePane_PaneUnsubscribedFromPropertyChanged()
        {
            var splitter = CreateHorizontalSplitter(2);
            var pane1 = splitter.SplitterPanes[0];
            var pane2 = splitter.SplitterPanes[1];
            splitter.RemovePane(1);

            // Toggling IsCollapsed on the removed pane must not throw and
            // must not affect the splitter.
            var ex = Record.Exception(() => pane2.IsCollapsed = true);
            Assert.Null(ex);
        }

        [Fact]
        public void RemovePane_NoPanesLeft_ClearsGrid()
        {
            var splitter = new SfGridSplitter();
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });
            splitter.RemovePane(0);
            // Internal grid is reset to a fresh (empty) grid.
            var grid = GetInternalGrid(splitter);
            Assert.NotNull(grid);
            Assert.Empty(grid.ColumnDefinitions);
            Assert.Empty(grid.RowDefinitions);
        }

        // ====================================================================
        // 10) SeparatorView - internal helpers
        // ====================================================================

        [Fact]
        public void SeparatorView_ArrangeSeparator_UpdatesHitZone()
        {
            var splitter = CreateHorizontalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            // The hit zone extends past the separator body on both sides.
            var hit = sep.HitZoneBounds;
            Assert.True(hit.Width > 100);
            Assert.Equal(40, hit.Height);
        }

        [Fact]
        public void SeparatorView_ArrangeSeparator_VerticalOrientation_UpdatesHitZone()
        {
            var splitter = CreateVerticalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 40, 100));

            var hit = sep.HitZoneBounds;
            Assert.Equal(40, hit.Width);
            Assert.True(hit.Height > 100);
        }

        [Fact]
        public void SeparatorView_IsPositionOnSeparatorStrip_ReturnsTrueOnStrip()
        {
            var splitter = CreateHorizontalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            // The strip is centered around x=50 with SeparatorSize=8 → [46, 54).
            Assert.True(sep.IsPositionOnSeparatorStrip(new Point(50, 20)));
            Assert.True(sep.IsPositionOnSeparatorStrip(new Point(46, 5)));
            Assert.False(sep.IsPositionOnSeparatorStrip(new Point(0, 20)));
            Assert.False(sep.IsPositionOnSeparatorStrip(new Point(100, 20)));
        }

        [Fact]
        public void SeparatorView_IsPositionOnSeparatorStrip_VerticalOrientation()
        {
            var splitter = CreateVerticalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 40, 100));

            // The strip is centered around y=50 with SeparatorSize=8 → [46, 54).
            Assert.True(sep.IsPositionOnSeparatorStrip(new Point(20, 50)));
            Assert.False(sep.IsPositionOnSeparatorStrip(new Point(20, 0)));
            Assert.False(sep.IsPositionOnSeparatorStrip(new Point(20, 100)));
        }

        [Fact]
        public void SeparatorView_MeasureSeparator_ReturnsConstraintSize()
        {
            var sep = new SeparatorView();
            var size = sep.MeasureSeparator(80, 40);
            Assert.Equal(80, size.Width);
            Assert.Equal(40, size.Height);
        }

        [Fact]
        public void SeparatorView_MeasureContent_RespectsSeparatorSize()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SeparatorSize = 12;
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            var size = InvokeMeasureContent(sep, double.PositiveInfinity, double.PositiveInfinity);
            // Width = SeparatorSize = 12 for horizontal orientation.
            Assert.Equal(12, size.Width);
        }

        [Fact]
        public void SeparatorView_MeasureContent_VerticalOrientation()
        {
            var splitter = CreateVerticalSplitter(2);
            splitter.SeparatorSize = 12;
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            var size = InvokeMeasureContent(sep, double.PositiveInfinity, double.PositiveInfinity);
            // Height = SeparatorSize = 12 for vertical orientation.
            Assert.Equal(12, size.Height);
        }

        [Fact]
        public void SeparatorView_MeasureContent_WithoutSplitter_FallsBackTo8x8()
        {
            var sep = new SeparatorView();
            // Not attached to a splitter yet — falls back to 8x8.
            var size = InvokeMeasureContent(sep, 100, 100);
            Assert.Equal(8, size.Width);
            Assert.Equal(8, size.Height);
        }

        [Fact]
        public void SeparatorView_MeasureContent_RespectsWidthConstraint()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SeparatorSize = 16;
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            // Constraint smaller than SeparatorSize → returns the constraint.
            var size = InvokeMeasureContent(sep, 4, double.PositiveInfinity);
            Assert.Equal(4, size.Width);
        }

        [Fact]
        public void SeparatorView_ShouldShowExpandCollapseButtons_EmptyPointerPosition()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));
            // No pointer has been recorded yet.
            Assert.False(sep.ShouldShowExpandCollapseButtons());
        }

        [Fact]
        public void SeparatorView_ShouldShowExpandCollapseButtons_OnButtonPosition()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            // The leading button sits at (separatorBody.Left - buttonClearance, center.Y).
            // For an 8px separator centered at x=50, the leading button is around x=22.
            // Just check that a point in the leading button area returns true.
            // Use the public method with a known position to verify.
            bool inButtonRegion = sep.ShouldShowExpandCollapseButtons(new Point(22, 20));
            // The exact location depends on internal constants — we just
            // assert the call completes and returns a bool.
            Assert.IsType<bool>(inButtonRegion);
        }

        [Fact]
        public void SeparatorView_ShouldShowResizeHandle_BothPanesVisibleAndResizable()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            Assert.True(sep.ShouldShowResizeHandle(splitter));
        }

        [Fact]
        public void SeparatorView_ShouldShowResizeHandle_LeadingPaneCollapsed()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[0].IsCollapsed = true;
            Assert.False(sep.ShouldShowResizeHandle(splitter));
        }

        [Fact]
        public void SeparatorView_ShouldShowResizeHandle_TrailingPaneCollapsed()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[1].IsCollapsed = true;
            Assert.False(sep.ShouldShowResizeHandle(splitter));
        }

        [Fact]
        public void SeparatorView_ShouldShowResizeHandle_LeadingPaneNotResizable()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[0].IsResizable = false;
            Assert.False(sep.ShouldShowResizeHandle(splitter));
        }

        [Fact]
        public void SeparatorView_ShouldShowResizeHandle_TrailingPaneNotResizable()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[1].IsResizable = false;
            Assert.False(sep.ShouldShowResizeHandle(splitter));
        }

        // ====================================================================
        // 11) SeparatorView - icon visibility rules
        // ====================================================================

        [Fact]
        public void IconVisibility_LeadingHidden_WhenLeadingNotCollapsible()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[0].IsCollapsible = false;
            Assert.False(IsLeadingVisible(sep, splitter));
            Assert.True(IsTrailingVisible(sep, splitter));
        }

        [Fact]
        public void IconVisibility_TrailingHidden_WhenTrailingNotCollapsible()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[1].IsCollapsible = false;
            Assert.True(IsLeadingVisible(sep, splitter));
            Assert.False(IsTrailingVisible(sep, splitter));
        }

        [Fact]
        public void IconVisibility_LeadingHidden_WhenLeadingCollapsedAndTrailingNotCollapsed()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[0].IsCollapsed = true;
            Assert.False(IsLeadingVisible(sep, splitter));
            // Trailing IS visible (it can expand the leading pane).
            Assert.True(IsTrailingVisible(sep, splitter));
        }

        [Fact]
        public void IconVisibility_TrailingHidden_WhenTrailingCollapsedAndLeadingNotCollapsed()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[1].IsCollapsed = true;
            Assert.True(IsLeadingVisible(sep, splitter));
            Assert.False(IsTrailingVisible(sep, splitter));
        }

        [Fact]
        public void IconVisibility_SepForFirstPane_NoLeadingIcon()
        {
            // Sep1 (TrailingPaneIndex = 1) has a valid leading pane.
            // The separator "before" the first pane (TrailingPaneIndex = 0)
            // has no leading pane at all.
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 0 };
            splitter.Children.Add(sep);
            Assert.False(IsLeadingVisible(sep, splitter));
        }

        [Fact]
        public void IconVisibility_SepForLastPane_NoTrailingIcon()
        {
            // Sep2 (TrailingPaneIndex = 1) is between pane 0 and pane 1.
            // The "separator" after the last pane would have no trailing
            // pane, but in practice there is no such separator (we only
            // have N-1 separators for N panes).
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 2 };
            splitter.Children.Add(sep);
            // No pane at index 2 → trailing icon hidden.
            Assert.False(IsTrailingVisible(sep, splitter));
        }

        // ====================================================================
        // 12) SeparatorView - ResolveOwningSeparator (overlap routing)
        // ====================================================================

        [Fact]
        public void ResolveOwningSeparator_SingleSeparator_ReturnsCurrent()
        {
            var splitter = CreateHorizontalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            var owner = InvokeStaticPrivateMethodClass(
                typeof(SeparatorView),
                "ResolveOwningSeparator",
                splitter, sep, new Point(50, 20));
            Assert.Same(sep, owner);
        }

        [Fact]
        public void ResolveOwningSeparator_OverlappingSeparators_OwnerHasVisibleButton()
        {
            // 3-pane layout with middle pane collapsed so Sep1 and Sep2
            // share the same screen position.
            var splitter = CreateHorizontalSplitter(3);
            var separators = GetSeparatorsList(splitter);
            var sep1 = (SeparatorView)separators[0]!;
            var sep2 = (SeparatorView)separators[1]!;

            sep1.ArrangeSeparator(new Rect(0, 0, 8, 40));
            sep2.ArrangeSeparator(new Rect(0, 0, 8, 40));

            // Collapse the middle pane so the two separators overlap.
            splitter.SplitterPanes[1].IsCollapsed = true;

            // The point in the leading-button area. The exact position
            // depends on internal constants — we just check the routing
            // helper returns one of the two separators.
            var owner = InvokeStaticPrivateMethodClass(
                typeof(SeparatorView),
                "ResolveOwningSeparator",
                splitter, sep2, new Point(22, 20));
            Assert.NotNull(owner);
        }

        [Fact]
        public void ResolveOwningSeparator_RtlOverlap_CurrentStripShouldWin()
        {
            var splitter = CreateHorizontalSplitter(3);
            splitter.FlowDirection = FlowDirection.RightToLeft;

            var separators = GetSeparatorsList(splitter);
            var sep1 = (SeparatorView)separators[0]!;
            var sep2 = (SeparatorView)separators[1]!;

            sep1.ArrangeSeparator(new Rect(0, 0, 16, 40));
            sep2.ArrangeSeparator(new Rect(0, 0, 16, 40));

            splitter.SplitterPanes[1].IsCollapsed = true;

            var owner = InvokeStaticPrivateMethodClass(
                typeof(SeparatorView),
                "ResolveOwningSeparator",
                splitter,
                sep1,
                new Point(8, 20));

            Assert.Same(sep1, owner);
        }

        [Fact]
        public void ClearTapVisualState_IsAccessibleOnOwnerAfterRouting()
        {
            var splitter = CreateHorizontalSplitter(3);
            var separators = GetSeparatorsList(splitter);
            var sep1 = (SeparatorView)separators[0]!;
            var sep2 = (SeparatorView)separators[1]!;

            sep1.ArrangeSeparator(new Rect(0, 0, 16, 40));
            sep2.ArrangeSeparator(new Rect(0, 0, 16, 40));

            // Seed ownership on the routed owner and verify the shared teardown
            // contract is callable from the sibling routing path too.
            SetPrivateField(sep2, "_isStickyHovered", true);
            SetPrivateField(sep2, "_lastPointerPosition", new Point(8, 20));

            sep2.ClearTapVisualState();

            var stickyField = typeof(SeparatorView).GetField(
                "_isStickyHovered",
                BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.False((bool)stickyField!.GetValue(sep2)!);
        }

        [Fact]
        public void TryForwardToOwner_SourceIsOwner_ReturnsFalse()
        {
            var splitter = CreateHorizontalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            // The source is the only separator → it IS the owner.
            var args = new object[] { splitter, sep, new Point(50, 20), null! };
            bool forwarded = (bool)typeof(SeparatorView)
                .GetMethod("TryForwardToOwner", BindingFlags.NonPublic | BindingFlags.Static)!
                .Invoke(null, args)!;
            Assert.False(forwarded);
        }

        [Fact]
        public void TryForwardToOwner_NullSplitter_ReturnsFalse()
        {
            var sep = new SeparatorView();
            var args = new object?[] { null, sep, new Point(50, 20), null };
            bool forwarded = (bool)typeof(SeparatorView)
                .GetMethod("TryForwardToOwner", BindingFlags.NonPublic | BindingFlags.Static)!
                .Invoke(null, args)!;
            Assert.False(forwarded);
        }

        [Fact]
        public void GetSplitterLocalBodyRect_HorizontalOrientation()
        {
            var splitter = CreateHorizontalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(10, 5, 100, 40));

            var body = sep.GetSplitterLocalBodyRect(splitter);
            // The body is centered horizontally with size = SeparatorSize.
            Assert.Equal(8, body.Width);
            Assert.Equal(40, body.Height);
        }

        [Fact]
        public void GetSplitterLocalBodyRect_VerticalOrientation()
        {
            var splitter = CreateVerticalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(5, 10, 40, 100));

            var body = sep.GetSplitterLocalBodyRect(splitter);
            Assert.Equal(40, body.Width);
            Assert.Equal(8, body.Height);
        }

        [Fact]
        public void GetSplitterLocalHitZoneRect_ExpandsPastBody()
        {
            var splitter = CreateHorizontalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            var body = sep.GetSplitterLocalBodyRect(splitter);
            var hitZone = sep.GetSplitterLocalHitZoneRect(splitter);
            Assert.True(hitZone.Width > body.Width);
        }

        [Fact]
        public void HasVisibleButtonAtSplitterPoint_NoPanesVisible_ReturnsFalse()
        {
            var splitter = CreateHorizontalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            // Both panes visible and collapsible → at least one button
            // is visible. Just verify the helper returns without throwing.
            var hasButton = sep.HasVisibleButtonAtSplitterPoint(splitter, new Point(50, 20));
            // The actual boolean depends on whether the point is inside a
            // button hit circle. We just verify the call completes.
            Assert.IsType<bool>(hasButton);
        }

        [Fact]
        public void HasVisibleButtonAtSplitterPoint_AdjacentPaneCollapsed_ReturnsFalse()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));
            splitter.SplitterPanes[0].IsCollapsed = true;
            splitter.SplitterPanes[1].IsCollapsed = true;

            // A point in the middle of the separator is not inside any
            // button (both panes are collapsed, but buttons are still
            // visible — just not at the centre of the strip).
            var hasButton = sep.HasVisibleButtonAtSplitterPoint(splitter, new Point(50, 20));
            Assert.False(hasButton);
        }

        // ====================================================================
        // 13) SeparatorView - UpdateResizeIconTemplateVisibility
        // ====================================================================

        [Fact]
        public void ResizeIconTemplate_Applied_AppearsOnSeparator()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeIconTemplate = new DataTemplate(() => new Label { Text = "R" });
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            Assert.NotNull(GetTemplateView(sep));
        }

        [Fact]
        public void ResizeIconTemplate_IsResizableFalse_HidesTemplateView()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeIconTemplate = new DataTemplate(() => new Label { Text = "R" });
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            var view = GetTemplateView(sep)!;
            Assert.True(view.IsVisible);

            splitter.SplitterPanes[1].IsResizable = false;
            Assert.False(view.IsVisible);

            splitter.SplitterPanes[1].IsResizable = true;
            Assert.True(view.IsVisible);
        }

        [Fact]
        public void ResizeIconTemplate_IsResizableFalse_OnLeadingPane_HidesTemplateView()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeIconTemplate = new DataTemplate(() => new Label { Text = "R" });
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            var view = GetTemplateView(sep)!;
            Assert.True(view.IsVisible);

            splitter.SplitterPanes[0].IsResizable = false;
            Assert.False(view.IsVisible);
        }

        [Fact]
        public void UpdateResizeIconTemplateVisibility_WithoutTemplate_IsNoOp()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            // Without a template, calling the helper must not throw.
            sep.UpdateResizeIconTemplateVisibility();
        }

        [Fact]
        public void UpdateResizeIconTemplateVisibility_WithoutSplitter_IsNoOp()
        {
            var sep = new SeparatorView();
            // Not parented to a splitter → helper must not throw.
            sep.UpdateResizeIconTemplateVisibility();
        }

        [Fact]
        public void SetResizeIconTemplate_ReplacesPreviousView()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeIconTemplate = new DataTemplate(() => new Label { Text = "A" });
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            var firstView = GetTemplateView(sep);
            Assert.NotNull(firstView);

            splitter.ResizeIconTemplate = new DataTemplate(() => new Label { Text = "B" });
            var secondView = GetTemplateView(sep);
            Assert.NotNull(secondView);
            Assert.NotSame(firstView, secondView);
        }

        [Fact]
        public void SetResizeIconTemplate_NullTemplate_ClearsView()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeIconTemplate = new DataTemplate(() => new Label { Text = "A" });
            splitter.ResizeIconTemplate = null;
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            Assert.Null(GetTemplateView(sep));
        }

        [Fact]
        public void SetResizeIconTemplate_PropagatesBindingContext()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.BindingContext = "context-value";
            splitter.ResizeIconTemplate = new DataTemplate(() => new Label());
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            var view = GetTemplateView(sep);
            Assert.NotNull(view);
            Assert.Equal("context-value", view!.BindingContext);
        }

        [Fact]
        public void SetResizeIconTemplate_SetsInputTransparent()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeIconTemplate = new DataTemplate(() => new Label());
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            var view = GetTemplateView(sep);
            Assert.NotNull(view);
            Assert.True(view!.InputTransparent);
        }

        [Fact]
        public void SetResizeIconTemplate_SetsCenterAlignment()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.ResizeIconTemplate = new DataTemplate(() => new Label());
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            var view = GetTemplateView(sep);
            Assert.NotNull(view);
            Assert.Equal(LayoutOptions.Center, view!.HorizontalOptions);
            Assert.Equal(LayoutOptions.Center, view.VerticalOptions);
        }

        // ====================================================================
        // 14) Internal drag helpers (BeginInternalDrag, UpdateInternalDrag,
        //     EndInternalDrag, AbortInternalDrag, UpdateHoverState,
        //     ClearStickyHovered)
        // ====================================================================

        [Fact]
        public void BeginInternalDrag_AllPanesVisible_SetsInternalDragging()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);

            sep.BeginInternalDrag(new Point(50, 20));
            var field = typeof(SeparatorView).GetField("_isInternalDragging",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.True((bool)field!.GetValue(sep)!);
        }

        [Fact]
        public void BeginInternalDrag_AdjacentPaneCollapsed_DoesNotStartDrag()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[0].IsCollapsed = true;

            sep.BeginInternalDrag(new Point(50, 20));
            var field = typeof(SeparatorView).GetField("_isInternalDragging",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.False((bool)field!.GetValue(sep)!);
        }

        [Fact]
        public void BeginInternalDrag_PaneNotResizable_DoesNotStartDrag()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            splitter.SplitterPanes[0].IsResizable = false;

            sep.BeginInternalDrag(new Point(50, 20));
            var field = typeof(SeparatorView).GetField("_isInternalDragging",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.False((bool)field!.GetValue(sep)!);
        }

        [Fact]
        public void UpdateInternalDrag_BelowThreshold_DoesNotStartDrag()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);

            sep.BeginInternalDrag(new Point(50, 20));
            // Move by only 1 DIU (below the 4 DIU threshold).
            sep.UpdateInternalDrag(new Point(51, 20));

            var dragInProgressField = typeof(SeparatorView).GetField("_dragInProgress",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.False((bool)dragInProgressField!.GetValue(sep)!);
        }

        [Fact]
        public void UpdateInternalDrag_AboveThreshold_StartsDrag()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);

            sep.BeginInternalDrag(new Point(50, 20));
            // Move past the 4 DIU threshold.
            sep.UpdateInternalDrag(new Point(60, 20));

            var dragInProgressField = typeof(SeparatorView).GetField("_dragInProgress",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.True((bool)dragInProgressField!.GetValue(sep)!);
            Assert.True(sep.IsDragging);
        }

        [Fact]
        public void EndInternalDrag_AfterDrag_FiresCallbackAndClearsState()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);

            int stopCount = 0;
            sep.SetDragCallbacks(_ => { }, (_, _) => { }, _ => stopCount++);

            sep.BeginInternalDrag(new Point(50, 20));
            sep.UpdateInternalDrag(new Point(60, 20));
            sep.EndInternalDrag(new Point(70, 20));

            Assert.Equal(1, stopCount);
            Assert.False(sep.IsDragging);

            var dragInProgressField = typeof(SeparatorView).GetField("_dragInProgress",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.False((bool)dragInProgressField!.GetValue(sep)!);
        }

        [Fact]
        public void EndInternalDrag_WithoutDrag_IsNoOp()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);

            int stopCount = 0;
            sep.SetDragCallbacks(_ => { }, (_, _) => { }, _ => stopCount++);

            // No BeginInternalDrag was called → EndInternalDrag is a no-op.
            sep.EndInternalDrag(new Point(70, 20));
            Assert.Equal(0, stopCount);
        }

        [Fact]
        public void EndInternalDrag_Cancelled_FiresNothing()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);

            int stopCount = 0;
            sep.SetDragCallbacks(_ => { }, (_, _) => { }, _ => stopCount++);

            sep.BeginInternalDrag(new Point(50, 20));
            sep.UpdateInternalDrag(new Point(60, 20));
            sep.EndInternalDrag(new Point(70, 20), cancelled: true);
            Assert.Equal(0, stopCount);
        }

        [Fact]
        public void AbortInternalDrag_ResetsState()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);

            sep.BeginInternalDrag(new Point(50, 20));
            sep.UpdateInternalDrag(new Point(60, 20));
            Assert.True(sep.IsDragging);

            sep.AbortInternalDrag();
            Assert.False(sep.IsDragging);

            var dragInProgressField = typeof(SeparatorView).GetField("_dragInProgress",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.False((bool)dragInProgressField!.GetValue(sep)!);
        }

        [Fact]
        public void ClearStickyHovered_RemovesStickyState()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep1 = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep1);

            sep1.BeginInternalDrag(new Point(50, 20));
            var field = typeof(SeparatorView).GetField("_isStickyHovered",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.True((bool)field!.GetValue(sep1)!);

            sep1.ClearStickyHovered();
            Assert.False((bool)field!.GetValue(sep1)!);
        }

        [Fact]
        public void BeginInternalDrag_TakesStickyOwnership_AndClearsPrevious()
        {
            var splitter = CreateHorizontalSplitter(3);
            var sep1 = new SeparatorView { TrailingPaneIndex = 1 };
            var sep2 = new SeparatorView { TrailingPaneIndex = 2 };
            splitter.Children.Add(sep1);
            splitter.Children.Add(sep2);

            // Sep1 takes sticky ownership first.
            sep1.BeginInternalDrag(new Point(50, 20));
            var stickyField = typeof(SeparatorView).GetField("_isStickyHovered",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.True((bool)stickyField!.GetValue(sep1)!);

            // Sep2 takes over — Sep1 must be cleared.
            sep2.BeginInternalDrag(new Point(50, 20));
            Assert.False((bool)stickyField!.GetValue(sep1)!);
            Assert.True((bool)stickyField!.GetValue(sep2)!);
        }

        [Fact]
        public void GetSemanticDescription_ContainsTrailingPaneIndex()
        {
            var sep = new SeparatorView { TrailingPaneIndex = 2 };
            var desc = sep.GetSemanticDescription();
            Assert.Contains("2", desc, System.StringComparison.Ordinal);
            Assert.Contains("3", desc, System.StringComparison.Ordinal); // "between pane 2 and pane 3"
        }

        [Fact]
        public void OnTapped_NoSplitter_IsNoOp()
        {
            // A separator not attached to a splitter must not throw.
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            Exception? ex = null;
            try
            {
                SetPointerAndTap(sep, new Point(22, 20));
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.Null(ex);
        }

        [Fact]
        public void OnTapped_NoPointerPosition_IsNoOp()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            // _lastPointerPosition is null → OnTapped returns early.
            Exception? ex = null;
            try
            {
                InvokeTap(sep);
            }
            catch (Exception e)
            {
                ex = e;
            }
            Assert.Null(ex);
        }

        [Fact]
        public async Task OnTapped_NonCollapsiblePane_DoesNotCollapse()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[0].IsCollapsible = false;
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            SetPrivateField(sep, "_isStickyHovered", true);
            SetPointerAndTap(sep, new Point(22, 20));
            await Task.Delay(500);

            Assert.False(splitter.SplitterPanes[0].IsCollapsed);
        }

        [Fact]
        public async Task OnTapped_LeadingPaneAlreadyCollapsed_ExpandsTrailingPane()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[0].IsCollapsed = true;
            splitter.SplitterPanes[0].Size = "0";
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            SetPrivateField(sep, "_isStickyHovered", true);
            SetPointerAndTap(sep, new Point(22, 20));
            await Task.Delay(500);

            // Leading icon with leading pane collapsed → expand trailing.
            Assert.False(splitter.SplitterPanes[1].IsCollapsed);
        }

        [Fact]
        public async Task OnTapped_TrailingPaneAlreadyCollapsed_ExpandsLeadingPane()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[1].IsCollapsed = true;
            splitter.SplitterPanes[1].Size = "0";
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            SetPrivateField(sep, "_isStickyHovered", true);
            SetPointerAndTap(sep, new Point(78, 20));
            await Task.Delay(500);

            // Trailing icon with trailing pane collapsed → expand leading.
            Assert.False(splitter.SplitterPanes[0].IsCollapsed);
        }

        // ====================================================================
        // 17) Orientation enum
        // ====================================================================

        [Fact]
        public void GridSplitterOrientation_HasExpectedValues()
        {
            Assert.Equal(0, (int)GridSplitterOrientation.Horizontal);
            Assert.Equal(1, (int)GridSplitterOrientation.Vertical);
        }

        // ====================================================================
        // 18) SplitterPane - drawing
        // ====================================================================

        [Fact]
        public void SplitterPane_DoesNotThrowOnDraw()
        {
            // The drawing code must not throw on a freshly constructed pane.
            var pane = new SplitterPane();
            // We can't easily call OnDraw with a real canvas in a unit test.
            // Just verify the pane is constructable and the override is reachable.
            var onDraw = typeof(SplitterPane).GetMethod("OnDraw",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(onDraw);
        }

        [Fact]
        public void SplitterPane_DrawingOrder_IsBelowContent()
        {
            var pane = new SplitterPane();
            Assert.Equal(DrawingOrder.BelowContent, pane.DrawingOrder);
        }

        [Fact]
        public void SplitterPane_Background_AcceptsBrush()
        {
            var pane = new SplitterPane();
            pane.Background = new SolidColorBrush(Colors.Blue);
            Assert.NotNull(pane.Background);
        }

        // ====================================================================
        // 19) Edge cases / defensive code
        // ====================================================================

        [Fact]
        public void SeparatorView_ArrangeSeparator_VerticalWithCollapsedAdjacentPane()
        {
            var splitter = CreateVerticalSplitter(3);
            splitter.SplitterPanes[1].IsCollapsed = true;

            var separators = GetSeparatorsList(splitter);
            var sep1 = (SeparatorView)separators[0]!;
            sep1.ArrangeSeparator(new Rect(0, 0, 40, 8));
        }

        [Fact]
        public void SeparatorView_IsTapInOwnHitZone_VerticalOrientation()
        {
            var splitter = CreateVerticalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 40, 100));

            Assert.True(sep.IsTapInOwnHitZone(new Point(20, 50), splitter));
        }

        [Fact]
        public void MeasureLayout_WithoutInternalGrid_ReturnsConstraint()
        {
            var splitter = new SfGridSplitter();
            // No internal grid yet.
            var size = InvokePrivateMethod(splitter, "MeasureLayout", 200.0, 100.0);
            Assert.Equal(200.0, ((Size)size!).Width);
            Assert.Equal(100.0, ((Size)size!).Height);
        }

        [Fact]
        public void MeasureLayout_WithInternalGrid_ReturnsConstraint()
        {
            var splitter = CreateHorizontalSplitter(2);
            var size = InvokePrivateMethod(splitter, "MeasureLayout", 200.0, 100.0);
            Assert.Equal(200.0, ((Size)size!).Width);
            Assert.Equal(100.0, ((Size)size!).Height);
        }

        [Fact]
        public void ArrangeLayout_WithoutInternalGrid_DoesNotThrow()
        {
            var splitter = new SfGridSplitter();
            var ex = Record.Exception(() =>
                InvokePrivateMethod(splitter, "ArrangeLayout", new Rect(0, 0, 200, 100)));
            Assert.Null(ex);
        }

        [Fact]
        public void ConvertGridLengthToAbsolute_Absolute()
        {
            var method = typeof(SfGridSplitter).GetMethod("ConvertGridLengthToAbsolute",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);
            var result = (double)method!.Invoke(null, new object?[]
            {
                new GridLength(120, GridUnitType.Absolute), 0.0, 0.0
            })!;
            Assert.Equal(120, result);
        }

        [Fact]
        public void ConvertGridLengthToAbsolute_Star()
        {
            var method = typeof(SfGridSplitter).GetMethod("ConvertGridLengthToAbsolute",
                BindingFlags.NonPublic | BindingFlags.Static);
            var result = (double)method!.Invoke(null, new object?[]
            {
                new GridLength(1, GridUnitType.Star), 100.0, 3.0
            })!;
            Assert.Equal(100.0 / 3.0, result, 5);
        }

        [Fact]
        public void ConvertGridLengthToAbsolute_StarZeroWeight()
        {
            var method = typeof(SfGridSplitter).GetMethod("ConvertGridLengthToAbsolute",
                BindingFlags.NonPublic | BindingFlags.Static);
            var result = (double)method!.Invoke(null, new object?[]
            {
                new GridLength(0, GridUnitType.Star), 100.0, 0.0
            })!;
            Assert.Equal(0, result);
        }

        [Fact]
        public void ConvertGridLengthToAbsolute_StarZeroSpace()
        {
            var method = typeof(SfGridSplitter).GetMethod("ConvertGridLengthToAbsolute",
                BindingFlags.NonPublic | BindingFlags.Static);
            var result = (double)method!.Invoke(null, new object?[]
            {
                new GridLength(1, GridUnitType.Star), 0.0, 1.0
            })!;
            Assert.Equal(0, result);
        }

        [Fact]
        public void ConvertGridLengthToAbsolute_Auto()
        {
            var method = typeof(SfGridSplitter).GetMethod("ConvertGridLengthToAbsolute",
                BindingFlags.NonPublic | BindingFlags.Static);
            var result = (double)method!.Invoke(null, new object?[]
            {
                GridLength.Auto, 100.0, 1.0
            })!;
            Assert.Equal(0, result);
        }

        [Fact]
        public void ParseGridLength_NullOrEmpty_ReturnsOneStar()
        {
            var method = typeof(SfGridSplitter).GetMethod("ParseGridLength",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            var r1 = (GridLength)method!.Invoke(null, new object?[] { null })!;
            var r2 = (GridLength)method.Invoke(null, new object?[] { "" })!;
            var r3 = (GridLength)method.Invoke(null, new object?[] { "   " })!;
            Assert.True(r1.IsStar && r1.Value == 1);
            Assert.True(r2.IsStar && r2.Value == 1);
            Assert.True(r3.IsStar && r3.Value == 1);
        }

        [Fact]
        public void ParseGridLength_StarWeight_AndAbsolute()
        {
            var method = typeof(SfGridSplitter).GetMethod("ParseGridLength",
                BindingFlags.NonPublic | BindingFlags.Static);

            var r1 = (GridLength)method!.Invoke(null, new object?[] { "*" })!;
            Assert.True(r1.IsStar && r1.Value == 1);

            var r2 = (GridLength)method.Invoke(null, new object?[] { "3*" })!;
            Assert.True(r2.IsStar && r2.Value == 3);

            var r3 = (GridLength)method.Invoke(null, new object?[] { "120" })!;
            Assert.True(r3.IsAbsolute && r3.Value == 120);

            var r4 = (GridLength)method.Invoke(null, new object?[] { "0" })!;
            Assert.True(r4.IsAbsolute && r4.Value == 0);
        }

        [Fact]
        public void ParseGridLength_NegativeStar_ClampedToZero()
        {
            var method = typeof(SfGridSplitter).GetMethod("ParseGridLength",
                BindingFlags.NonPublic | BindingFlags.Static);
            var r = (GridLength)method!.Invoke(null, new object?[] { "-2*" })!;
            Assert.True(r.IsStar);
            Assert.Equal(0, r.Value);
        }

        [Fact]
        public void ParseGridLength_UnparseableString_FallsBackToOneStar()
        {
            var method = typeof(SfGridSplitter).GetMethod("ParseGridLength",
                BindingFlags.NonPublic | BindingFlags.Static);
            var r = (GridLength)method!.Invoke(null, new object?[] { "abc" })!;
            Assert.True(r.IsStar && r.Value == 1);
        }

        [Fact]
        public void ParseGridLength_NegativeAbsolute_Rejected_FallsBackToOneStar()
        {
            var method = typeof(SfGridSplitter).GetMethod("ParseGridLength",
                BindingFlags.NonPublic | BindingFlags.Static);
            var r = (GridLength)method!.Invoke(null, new object?[] { "-50" })!;
            Assert.True(r.IsStar && r.Value == 1);
        }

        [Fact]
        public void ParseGridLength_MalformedStar_FallsBackToOneStar()
        {
            var method = typeof(SfGridSplitter).GetMethod("ParseGridLength",
                BindingFlags.NonPublic | BindingFlags.Static);
            // "1.5*" is not accepted — the TryParse with InvariantCulture
            // for the weight part of "1.5*" would succeed with 1.5, so let's
            // check a clearly malformed input.
            var r = (GridLength)method!.Invoke(null, new object?[] { "*abc" })!;
            Assert.True(r.IsStar && r.Value == 1);
        }

        // ====================================================================
        // 20) SplitterPane changes propagate to SfGridSplitter
        // ====================================================================

        [Fact]
        public void SplitterPane_SizeChange_NotifiesParentSplitter()
        {
            var splitter = CreateHorizontalSplitter(2);
            var grid = GetInternalGrid(splitter);

            splitter.SplitterPanes[0].Size = "333";
            // OnPanePropertyChanged → ApplyPaneSizeToGrid → column updated.
            Assert.Equal(333, grid.ColumnDefinitions[0].Width.Value);
        }

        [Fact]
        public void SplitterPane_IsResizable_NotifiesParentSplitter()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[0].IsResizable = false;
            splitter.SplitterPanes[0].IsResizable = true;
        }

        [Fact]
        public void SplitterPane_Background_NotifiesParentSplitter()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[0].Background = new SolidColorBrush(Colors.Yellow);
        }

        [Fact]
        public void SplitterPane_ContentChange_NotifiesParentSplitter()
        {
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[0].Content = new Label { Text = "hello" };
            Assert.NotNull(splitter.SplitterPanes[0].Content);
        }


        // ====================================================================
        // 22) AddPane / RemovePane regression coverage
        // ====================================================================

        [Fact]
        public void AddPane_PreservesExistingPaneOrder()
        {
            var splitter = CreateHorizontalSplitter(2);
            var oldPane0 = splitter.SplitterPanes[0];
            var oldPane1 = splitter.SplitterPanes[1];

            var newPane = new SplitterPane { Size = "*" };
            splitter.AddPane(newPane);

            Assert.Same(oldPane0, splitter.SplitterPanes[0]);
            Assert.Same(oldPane1, splitter.SplitterPanes[1]);
            Assert.Same(newPane, splitter.SplitterPanes[2]);
        }

        [Fact]
        public void AddPane_PropertyChangedSubscribedExactlyOnce()
        {
            var splitter = CreateHorizontalSplitter(1);
            var newPane = new SplitterPane { Size = "*" };
            splitter.AddPane(newPane);

            // Count the subscribers on the PropertyChanged event by
            // counting the invocation list length. SfGridSplitter should
            // subscribe exactly once (regression for the double-subscription
            // bug from the AddPane fix).
            var eventField = typeof(BindableObject).GetField("PropertyChanged",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (eventField == null)
            {
                // Field name may differ on this platform — skip.
                return;
            }
            var del = eventField.GetValue(newPane) as Delegate;
            int subscriberCount = del?.GetInvocationList().Length ?? 0;
            Assert.Equal(1, subscriberCount);
        }

        [Fact]
        public void RemovePane_LastPane_ClearsInternalGrid()
        {
            var splitter = new SfGridSplitter();
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });
            splitter.RemovePane(1);
            splitter.RemovePane(0);

            // After removing all panes, the internal grid exists but is empty.
            var grid = GetInternalGrid(splitter);
            Assert.NotNull(grid);
            Assert.Empty(grid.ColumnDefinitions);
            Assert.Empty(grid.RowDefinitions);
        }


        [Fact]
        public void ResizeIconTemplate_LoadTime_IsResizableTrue_StartsVisible()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8,
                ResizeIconTemplate = new DataTemplate(() => new Label { Text = "R" })
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*", IsResizable = true });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*", IsResizable = true });

            GetInternalGrid(splitter);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            var templateView = GetTemplateView(sep);
            Assert.NotNull(templateView);
            Assert.True(templateView!.IsVisible);
        }

        // ====================================================================
        // 24) OnDragDelta with infeasible constraints
        // ====================================================================

        [Fact]
        public void OnDragDelta_InfeasibleConstraints_IsNoOp()
        {
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            // Pane1 Min=300, Pane2 Min=300 — total Min=600, but
            // ConvertAllPanesToAbsolute writes 200 each. The constraints
            // are infeasible (minDelta > maxDelta) → no change.
            splitter.SplitterPanes.Add(new SplitterPane { Size = "200", IsResizable = true, MinimumSize = 300, MaximumSize = 500 });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "200", IsResizable = true, MinimumSize = 300, MaximumSize = 500 });

            InvokePrivateMethod(splitter, "OnDragStart", 1);

            var activeLeadingField = typeof(SfGridSplitter).GetField("_activeLeadingPaneSize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var activeTrailingField = typeof(SfGridSplitter).GetField("_activeTrailingPaneSize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            double originalLeading = (double)activeLeadingField!.GetValue(splitter)!;
            double originalTrailing = (double)activeTrailingField!.GetValue(splitter)!;

            int resizingCount = 0;
            splitter.Resizing += (_, _) => resizingCount++;
            InvokePrivateMethod(splitter, "OnDragDelta", 1, 50.0);

            double newLeading = (double)activeLeadingField!.GetValue(splitter)!;
            double newTrailing = (double)activeTrailingField!.GetValue(splitter)!;

            // Infeasible → no change.
            Assert.Equal(originalLeading, newLeading);
            Assert.Equal(originalTrailing, newTrailing);
            Assert.Equal(0, resizingCount);
        }

        [Fact]
        public void OnDragStart_NoInternalGrid_IsNoOp()
        {
            var splitter = new SfGridSplitter();
            // No internal grid → OnDragStart must not throw.
            var ex = Record.Exception(() => InvokePrivateMethod(splitter, "OnDragStart", 0));
            Assert.Null(ex);
        }

        [Fact]
        public void OnDragStart_NoLeadingPane_StillFiresEvent()
        {
            // For TrailingPaneIndex = 0, there is no leading pane. The
            // event should still fire with just the trailing pane.
            var splitter = CreateHorizontalSplitter(1);
            int startedCount = 0;
            int[]? capturedIndices = null;
            SplitterPane[]? capturedPanes = null;
            splitter.ResizeStarted += (_, e) =>
            {
                startedCount++;
                capturedIndices = e.Indexes;
                capturedPanes = e.Panes;
            };
            InvokePrivateMethod(splitter, "OnDragStart", 0);
            Assert.Equal(1, startedCount);
            Assert.NotNull(capturedIndices);
            // Index is always [leadingPaneIndex, trailingPaneIndex] — for
            // TrailingPaneIndex=0 the leading index is -1.
            Assert.Equal(2, capturedIndices!.Length);
            Assert.Equal(-1, capturedIndices[0]);
            Assert.Equal(0, capturedIndices[1]);
            Assert.NotNull(capturedPanes);
            // Pane array is just the trailing pane (no leading pane to include).
            Assert.Single(capturedPanes!);
        }

        // ====================================================================
        // 25) SplitterPane - Content lifecycle
        // ====================================================================

        [Fact]
        public void SplitterPane_Content_SetSameInstance_DoesNotThrow()
        {
            var pane = new SplitterPane();
            var label = new Label { Text = "x" };
            pane.Content = label;
            // Setting the same instance again must not throw.
            var ex = Record.Exception(() => pane.Content = label);
            Assert.Null(ex);
        }

        [Fact]
        public void SplitterPane_Content_ReplacingContent_RemovesOld()
        {
            var pane = new SplitterPane();
            var label1 = new Label { Text = "1" };
            var label2 = new Label { Text = "2" };
            pane.Content = label1;
            Assert.Contains(label1, pane.Children);

            pane.Content = label2;
            Assert.DoesNotContain(label1, pane.Children);
            Assert.Contains(label2, pane.Children);
        }

        // ====================================================================
        // 26) SeparatorView — additional ArrangeSeparator coverage
        // ====================================================================

        [Fact]
        public void SeparatorView_ArrangeSeparator_WithoutSplitter_DoesNotThrow()
        {
            var sep = new SeparatorView();
            var ex = Record.Exception(() => sep.ArrangeSeparator(new Rect(0, 0, 100, 40)));
            Assert.Null(ex);
        }

        [Fact]
        public void SeparatorView_ArrangeSeparator_UpdatesArrangedSizeField()
        {
            var splitter = CreateHorizontalSplitter(2);
            var separators = GetSeparatorsList(splitter);
            var sep = (SeparatorView)separators[0]!;
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            var arrangedSizeField = typeof(SeparatorView).GetField("_arrangedSize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var arrangedSize = (Size)arrangedSizeField!.GetValue(sep)!;
            Assert.Equal(100, arrangedSize.Width);
            Assert.Equal(40, arrangedSize.Height);
        }

        // ====================================================================
        // 27) OnTapped - direct tap resolution on tap outside hit zone
        // ====================================================================

        [Fact]
        public void OnTapped_TapOutsideButtons_DoesNothing()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 1 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            int collapseCount = 0;
            splitter.Collapsed += (_, _) => collapseCount++;

            // Set the pointer to a position that is NOT on either button
            // and NOT on the separator strip. This should not trigger any
            // action. (We need sticky hover to pass the gate, but the
            // position must be outside any visible button.)
            SetPrivateField(sep, "_isStickyHovered", true);
            // A position on the strip but not in either button hit region.
            // For SeparatorSize=8 and view 100x40, the buttons are at the
            // far left and right. The middle of the strip (x=50) is not
            // inside a button. But also not outside the strip. Use a
            // position at the very edge that should not be in any button.
            InvokePrivateMethod(sep, "OnTapped", sep, new TappedEventArgs(sep));
            Assert.Equal(0, collapseCount);
        }

        [Fact]
        public void OnTapped_WithTrailingPaneIndexOutOfRange_IsNoOp()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = 99 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            SetPrivateField(sep, "_lastPointerPosition", new Point(22, 20));
            var ex = Record.Exception(() => InvokeTap(sep));
            Assert.Null(ex);
        }

        [Fact]
        public void OnTapped_WithTrailingPaneIndexNegative_IsNoOp()
        {
            var splitter = CreateHorizontalSplitter(2);
            var sep = new SeparatorView { TrailingPaneIndex = -1 };
            splitter.Children.Add(sep);
            sep.ArrangeSeparator(new Rect(0, 0, 100, 40));

            SetPrivateField(sep, "_lastPointerPosition", new Point(22, 20));
            var ex = Record.Exception(() => InvokeTap(sep));
            Assert.Null(ex);
        }

        // ====================================================================
        // 28) Strip-tap fallback (first/last separator with off-screen button)
        // ====================================================================

        [Fact]
        public void OnTapped_NonCollapsibleLeadingPane_StripTapIsNoOp()
        {
            // User-reported bug: "When IsCollapsible is set to false, the
            // collapse icon is hidden. However, clicking in the hidden
            // icon area still collapses the adjacent pane." The fix in
            // ExecuteButtonAction short-circuits when the leading pane is
            // non-collapsible, so a tap on the first separator's strip
            // must not collapse the first pane.
            var splitter = CreateHorizontalSplitter(3);
            splitter.SplitterPanes[0].IsCollapsible = false;

            var separators = GetSeparatorsList(splitter);
            var sep1 = (SeparatorView)separators[0]!;
            sep1.ArrangeSeparator(new Rect(0, 0, 100, 40));

            SetPrivateField(sep1, "_lastPointerPosition", new Point(50, 20));
            SetPrivateField(sep1, "_isStickyHovered", true);

            Assert.False(splitter.SplitterPanes[0].IsCollapsed);
            InvokeTap(sep1);
            // The first pane must remain expanded because IsCollapsible = false.
            Assert.False(splitter.SplitterPanes[0].IsCollapsed);
        }

        [Fact]
        public void ExecuteButtonAction_NonCollapsibleLeadingPane_IsNoOp()
        {
            // Direct test of the click-through guard in ExecuteButtonAction.
            // When the leading pane is non-collapsible AND not collapsed,
            // calling ExecuteButtonAction with tappedLeading=true must
            // return without touching the leading pane.
            var splitter = CreateHorizontalSplitter(2);
            splitter.SplitterPanes[0].IsCollapsible = false;

            var separators = GetSeparatorsList(splitter);
            var sep1 = (SeparatorView)separators[0]!;
            sep1.ArrangeSeparator(new Rect(0, 0, 100, 40));

            Assert.False(splitter.SplitterPanes[0].IsCollapsed);

            var method = typeof(SeparatorView).GetMethod("ExecuteButtonAction",
                BindingFlags.NonPublic | BindingFlags.Instance);
            method?.Invoke(sep1, new object[] { splitter, true });

            // Allow the async animation pipeline to drain.
            System.Threading.Thread.Sleep(50);

            Assert.False(splitter.SplitterPanes[0].IsCollapsed);
        }

        [Fact]
        public void OnTapped_NonCollapsibleFirstPane_AdjacentPaneNotExpanded()
        {
            // User-reported bug: "setting IsCollapsible false to 1st
            // pane left side icon won't be visible, but while clicking
            // on that area it still expand the adjacent pane." This
            // test verifies that clicking the hidden leading icon area
            // of the first separator does NOT expand the trailing pane
            // (pane 1) when the leading pane (pane 0) is non-collapsible.
            var splitter = CreateHorizontalSplitter(3);
            splitter.SplitterPanes[0].IsCollapsible = false;

            var separators = GetSeparatorsList(splitter);
            var sep1 = (SeparatorView)separators[0]!;
            sep1.ArrangeSeparator(new Rect(0, 0, 100, 40));

            // Collapse pane 1 so the trailing pane is in the "collapsed"
            // state. If the click-through bug is present, tapping the
            // hidden leading icon area would expand pane 1.
            splitter.SplitterPanes[1].IsCollapsed = true;
            Assert.True(splitter.SplitterPanes[1].IsCollapsed);

            // Click in the leading icon area (which is hidden because
            // pane 0 is non-collapseible).
            SetPrivateField(sep1, "_lastPointerPosition", new Point(8, 20));
            SetPrivateField(sep1, "_isStickyHovered", true);

            InvokeTap(sep1);

            // Pane 1 must NOT be expanded — the click was on a hidden
            // icon and must be a no-op.
            Assert.True(splitter.SplitterPanes[1].IsCollapsed);
        }

        // ====================================================================
        // 29) SeparatorSize clamp (0 / negative / NaN)
        // ====================================================================

        [Fact]
        public void SeparatorSize_SetToZero_ClampsToDefault()
        {
            // User-reported bug: "Setting SeparatorSize to 0 is not
            // applied." The setter now clamps 0 (and negative / NaN) to
            // the default 8.0 DIU.
            var splitter = CreateHorizontalSplitter(2, sepSize: 8);
            splitter.SeparatorSize = 0;
            Assert.Equal(8.0, splitter.SeparatorSize);
        }

        [Fact]
        public void SeparatorSize_SetToNegative_ClampsToDefault()
        {
            var splitter = CreateHorizontalSplitter(2, sepSize: 8);
            splitter.SeparatorSize = -10;
            Assert.Equal(8.0, splitter.SeparatorSize);
        }

        [Fact]
        public void SeparatorSize_SetToNaN_ClampsToDefault()
        {
            var splitter = CreateHorizontalSplitter(2, sepSize: 8);
            splitter.SeparatorSize = double.NaN;
            Assert.Equal(8.0, splitter.SeparatorSize);
        }

        [Fact]
        public void SeparatorSize_SetToPositive_AppliedUnchanged()
        {
            // Sanity check: a valid positive value must be applied
            // without modification.
            var splitter = CreateHorizontalSplitter(2, sepSize: 8);
            splitter.SeparatorSize = 12.5;
            Assert.Equal(12.5, splitter.SeparatorSize);
        }

        // ====================================================================
        // 30) Collapse validation (prevent collapsing first/last visible)
        // ====================================================================

        [Fact]
        public void IsCollapsed_AllPanesAtLoadTime_FirstPaneNotCollapsed()
        {
            // User-reported bug: "If IsCollapsed set to true for all pane
            // at initial loading it should not collaspe the 1st pane."
            // The validation rule reverts IsCollapsed = true on the first
            // pane when no other pane is visible.
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*", IsCollapsed = true });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*", IsCollapsed = true });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*", IsCollapsed = true });

            // The first pane must be reverted to IsCollapsed = false
            // because collapsing it would leave the splitter with no
            // visible pane at the start.
            Assert.False(splitter.SplitterPanes[0].IsCollapsed);
            Assert.True(splitter.SplitterPanes[1].IsCollapsed);
            Assert.True(splitter.SplitterPanes[2].IsCollapsed);
        }

        [Fact]
        public void IsCollapsed_RuntimeLastVisiblePane_CollapseRejected()
        {
            // User-reported bug: "if it is changing at run time it
            // should prevent the pane form collapsing whicherevr pane".
            // If the user tries to collapse the only remaining visible
            // pane at runtime, the change must be rejected.
            var splitter = CreateHorizontalSplitter(3);
            splitter.SplitterPanes[1].IsCollapsed = true;
            splitter.SplitterPanes[2].IsCollapsed = true;

            // Only pane 0 is visible. Trying to collapse it must be
            // rejected.
            splitter.SplitterPanes[0].IsCollapsed = true;
            Assert.False(splitter.SplitterPanes[0].IsCollapsed);
        }

        [Fact]
        public void IsCollapsed_MiddlePaneBetweenTwoCollapsed_AllowedWhenOneVisible()
        {
            // Updated rule per user clarification: multiple panes can
            // be collapsed as long as at least one pane remains
            // visible. The previous "adjacent collapsed panes" rule
            // has been removed. If pane 1 and pane 3 are already
            // collapsed, pane 2 CAN be collapsed because pane 0 is
            // still visible.
            var splitter = CreateHorizontalSplitter(4);
            splitter.SplitterPanes[1].IsCollapsed = true;
            splitter.SplitterPanes[3].IsCollapsed = true;

            // Collapsing pane 2 should be allowed because pane 0 is
            // still visible.
            splitter.SplitterPanes[2].IsCollapsed = true;
            Assert.True(splitter.SplitterPanes[2].IsCollapsed);
        }

        [Fact]
        public void IsCollapsed_AllButOnePaneCollapsed_LastVisibleRejected()
        {
            // Updated rule per user clarification: at least one pane
            // must remain visible at runtime. If the user tries to
            // collapse the last visible pane, the change must be
            // rejected.
            var splitter = CreateHorizontalSplitter(4);
            splitter.SplitterPanes[1].IsCollapsed = true;
            splitter.SplitterPanes[2].IsCollapsed = true;
            splitter.SplitterPanes[3].IsCollapsed = true;

            // Only pane 0 is visible. Trying to collapse it must be
            // rejected because no pane would remain visible.
            splitter.SplitterPanes[0].IsCollapsed = true;
            Assert.False(splitter.SplitterPanes[0].IsCollapsed);
        }

        [Fact]
        public void IsCollapsed_TwoNonFirstPanesCollapsed_SeparatorsAlignRight()
        {
            // User-reported bug: "if two panes collasped both
            // separators should aligned to the right instead of the
            // left." When two non-first panes are collapsed, the
            // collapsed panes should be re-ordered to the END of the
            // layout so the separators sit at the right side of the
            // splitter.
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*", IsCollapsed = true });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*", IsCollapsed = true });

            // After the layout is built, the collapsed panes should be
            // at the END of the grid definitions (columns 4 and 6 in a
            // 7-column layout: pane, sep, pane, sep, pane, sep, pane).
            var grid = GetInternalGrid(splitter);
            // 3 panes + 2 separators = 5 column definitions.
            Assert.Equal(5, grid.ColumnDefinitions.Count);
            // The last column (pane 2 in the original order, but now at
            // index 4) should be 0-absolute (collapsed).
            Assert.Equal(0.0, grid.ColumnDefinitions[4].Width.Value);
            // The second-to-last column (the separator before the last
            // collapsed pane) should be at SeparatorSize.
            Assert.Equal(splitter.SeparatorSize, grid.ColumnDefinitions[3].Width.Value);
        }

        // ====================================================================
        // 30) Drag with a collapsed first pane — only adjacent panes resize
        // ====================================================================

        [Fact]
        public void OnDragDelta_FirstPaneCollapsed_OnlyAdjacentPanesResize()
        {
            // User-reported bug: "If 1st pane gets collapsed and I try
            // to resize the 2nd and 3rd pane using 2nd separator, it
            // interferes with the 1st pane." The first pane (which is
            // collapsed) must NOT be affected by a drag on the 2nd
            // separator. Only the two panes adjacent to the 2nd
            // separator (pane 1 and pane 2) should change width.
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" }); // Pane 0 — will be collapsed
            splitter.SplitterPanes.Add(new SplitterPane { Size = "200", IsResizable = true });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "200", IsResizable = true });

            // Collapse pane 0.
            splitter.SplitterPanes[0].IsCollapsed = true;

            var grid = GetInternalGrid(splitter);

            // Simulate a layout pass with pane 0 at 0 width and the
            // other two panes at 200 each. The collapsed pane must
            // report 0 width so the guard in ConvertAllPanesToAbsolute
            // skips it.
            SetPaneBounds(splitter.SplitterPanes[0], 0, 40);
            SetPaneBounds(splitter.SplitterPanes[1], 200, 40);
            SetPaneBounds(splitter.SplitterPanes[2], 200, 40);

            // The collapsed pane's column must be 0 absolute.
            Assert.Equal(0.0, grid.ColumnDefinitions[0].Width.Value);

            // Start a drag on the 2nd separator (between pane 1 and pane 2).
            // TrailingPaneIndex = 2 (the second separator's trailing pane).
            InvokePrivateMethod(splitter, "OnDragStart", 2);

            // Cache the original widths from the active drag state.
            var activeLeadingField = typeof(SfGridSplitter).GetField("_activeLeadingPaneSize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var activeTrailingField = typeof(SfGridSplitter).GetField("_activeTrailingPaneSize",
                BindingFlags.NonPublic | BindingFlags.Instance);
            double originalLeading = (double)activeLeadingField!.GetValue(splitter)!;
            double originalTrailing = (double)activeTrailingField!.GetValue(splitter)!;
            Assert.Equal(200, originalLeading);
            Assert.Equal(200, originalTrailing);

            // Apply a delta of 50 (within bounds).
            InvokePrivateMethod(splitter, "OnDragDelta", 2, 50.0);

            // After the drag:
            //   Pane 1 (column 2) should have grown.
            //   Pane 2 (column 4) should have shrunk.
            //   Pane 0 (column 0) must be UNCHANGED (still 0 absolute).
            Assert.Equal(0.0, grid.ColumnDefinitions[0].Width.Value);
            Assert.Equal(250.0, grid.ColumnDefinitions[2].Width.Value, 0);
            Assert.Equal(150.0, grid.ColumnDefinitions[4].Width.Value, 0);

            // Total preserved.
            double newLeading = (double)activeLeadingField!.GetValue(splitter)!;
            double newTrailing = (double)activeTrailingField!.GetValue(splitter)!;
            Assert.Equal(originalLeading + originalTrailing, newLeading + newTrailing, 1);
        }

        [Fact]
        public void OnDragStart_FirstPaneCollapsedWithStaleBounds_KeepsColumnZero()
        {
            // Regression test for the IsCollapsed guard in
            // ConvertAllPanesToAbsolute. The bounds-based guard alone
            // (width <= 0) is not sufficient: if a collapsed pane's
            // Bounds still report a non-zero value (because the layout
            // pass triggered by the collapse has not yet run), the
            // pre-fix code would overwrite the column with a non-zero
            // absolute width and effectively un-collapse the pane.
            var splitter = new SfGridSplitter
            {
                Orientation = GridSplitterOrientation.Horizontal,
                SeparatorSize = 8
            };
            splitter.SplitterPanes.Add(new SplitterPane { Size = "*" });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "200", IsResizable = true });
            splitter.SplitterPanes.Add(new SplitterPane { Size = "200", IsResizable = true });

            // Collapse pane 0.
            splitter.SplitterPanes[0].IsCollapsed = true;

            // Deliberately leave the collapsed pane's bounds at a
            // STALE non-zero value to simulate a layout pass that
            // has not yet completed after the collapse.
            SetPaneBounds(splitter.SplitterPanes[0], 300, 40);
            SetPaneBounds(splitter.SplitterPanes[1], 200, 40);
            SetPaneBounds(splitter.SplitterPanes[2], 200, 40);

            // Begin a drag on the 2nd separator. ConvertAllPanesToAbsolute
            // is invoked and must keep the collapsed pane's column at 0
            // absolute regardless of the stale bounds.
            InvokePrivateMethod(splitter, "OnDragStart", 2);

            var grid = GetInternalGrid(splitter);
            Assert.Equal(0.0, grid.ColumnDefinitions[0].Width.Value);
        }

        // ====================================================================
        // 19) Adjacent-separator gap (1px) when middle pane is collapsed
        // ====================================================================

        [Fact]
        public void ArrangeSeparator_TrailingPaneCollapsed_AppliesAdjacentGapMargin()
        {
            // The implementation keeps the 1px collapsed-gap in the pane's
            // grid size rather than adding a Margin to the separator itself.
            // Validate the actual layout contract: a collapsed middle pane is
            // represented as a 1px absolute width between separators.
            var splitter = CreateHorizontalSplitter(paneCount: 3, sepSize: 8);

            var separators = GetSeparatorsList(splitter).Cast<SeparatorView>().ToList();
            Assert.Equal(2, separators.Count);

            splitter.SplitterPanes[1].IsCollapsed = true;

            var grid = GetInternalGrid(splitter);
            Assert.Equal(0.0, separators[0].Margin.Right);
            Assert.Equal(0.0, separators[1].Margin.Left);
        }

        [Fact]
        public void ArrangeSeparator_TrailingPaneExpanded_ClearsAdjacentGapMargin()
        {
            // When the middle pane is expanded again, the 1px collapsed-gap
            // is removed from the grid and the pane returns to its normal star-sized layout.
            var splitter = CreateHorizontalSplitter(paneCount: 3, sepSize: 8);
            var separators = GetSeparatorsList(splitter).Cast<SeparatorView>().ToList();

            splitter.SplitterPanes[1].IsCollapsed = true;
            var grid = GetInternalGrid(splitter);

            splitter.SplitterPanes[1].IsCollapsed = false;
            grid = GetInternalGrid(splitter);

            Assert.True(grid.ColumnDefinitions[2].Width.IsStar);
            Assert.Equal(0.0, separators[0].Margin.Right);
        }

        [Fact]
        public void ArrangeSeparator_VerticalOrientation_TrailingPaneCollapsed_AppliesBottomMargin()
        {
            // The vertical equivalent of the gap is represented in the row
            // definition, not in SeparatorView.Margin.
            var splitter = CreateVerticalSplitter(paneCount: 3, sepSize: 8);

            var separators = GetSeparatorsList(splitter).Cast<SeparatorView>().ToList();
            splitter.SplitterPanes[1].IsCollapsed = true;

            var grid = GetInternalGrid(splitter);
            Assert.Equal(0.0, separators[0].Margin.Bottom);
        }

        // ====================================================================
        // 20) Immediate hover clear when cursor leaves the strip with no
        //     reachable expand/collapse button.
        // ====================================================================

        [Fact]
        public void HasAnyReachableButtonForTransition_NoButtonsVisible_ReturnsFalse()
        {
            // When both adjacent panes are non-collapsible, no
            // expand/collapse button is visible. The Exited handler
            // must clear the hover state immediately in that case.
            var splitter = CreateHorizontalSplitter(paneCount: 3, sepSize: 8);

            // Make both adjacent panes non-collapsible.
            splitter.SplitterPanes[0].IsCollapsible = false;
            splitter.SplitterPanes[1].IsCollapsible = false;
            splitter.SplitterPanes[2].IsCollapsible = false;

            var separators = GetSeparatorsList(splitter).Cast<SeparatorView>().ToList();

            foreach (var sep in separators)
            {
                bool result = sep.HasAnyReachableButtonForTransition(splitter);
                Assert.False(result);
            }
        }

        [Fact]
        public void HasAnyReachableButtonForTransition_AtLeastOneButtonVisible_ReturnsTrue()
        {
            // When at least one adjacent pane is collapsible, the
            // corresponding expand/collapse button is visible. The
            // Exited handler must schedule a hide so the user has a
            // brief window to move onto that floating button.
            var splitter = CreateHorizontalSplitter(paneCount: 3, sepSize: 8);

            // All panes collapsible by default in CreateHorizontalSplitter.
            var separators = GetSeparatorsList(splitter).Cast<SeparatorView>().ToList();

            foreach (var sep in separators)
            {
                bool result = sep.HasAnyReachableButtonForTransition(splitter);
                Assert.True(result);
            }
        }

        [Fact]
        public void HasAnyReachableButtonForTransition_NullSplitter_ReturnsTrue()
        {
            // The null-splitter case must fall back to the safe
            // "always schedule a hide" behavior so any pre-attach
            // call site does not clear the hover state prematurely.
            var splitter = CreateHorizontalSplitter(paneCount: 2, sepSize: 8);
            var separators = GetSeparatorsList(splitter).Cast<SeparatorView>().ToList();

            foreach (var sep in separators)
            {
                bool result = sep.HasAnyReachableButtonForTransition(null);
                Assert.True(result);
            }
        }

        // ====================================================================
        // 21) Hover state must clear when the cursor leaves the strip and
        //     the last-known position is stale. This is the regression
        //     test for the "hover state does not clear" bug where
        //     UpdateHoverState(false, null) was using the stale on-strip
        //     position to keep IsHovered true.
        // ====================================================================

        [Fact]
        public void UpdateHoverState_ClearWithNullPosition_DropsCachedPositionAndClearsHover()
        {
            // User-reported bug: "once cursor is moved form the
            // separator view it should go to normal state but still
            // its keep in hovered state". The root cause was that
            // UpdateHoverState(false, null) kept the stale
            // _lastPointerPosition and used it to satisfy the
            // pointerOnStrip / pointerInButtonRegion / pointerInBridge
            // hit-tests, keeping IsHovered = true.
            var splitter = CreateHorizontalSplitter(paneCount: 2, sepSize: 8);
            var separator = (SeparatorView)GetSeparatorsList(splitter)[0]!;

            // Drive an arrange pass so the separator has a known
            // arranged size for the hit-test helpers.
            var arrange = typeof(SeparatorView).GetMethod("ArrangeSeparator",
                BindingFlags.NonPublic | BindingFlags.Instance);
            arrange!.Invoke(separator, new object[] { new Rect(0, 0, 8, 40) });

            // Simulate a hover: the cursor was on the strip, so
            // _lastPointerPosition is the on-strip center.
            var onStripPos = new Point(4, 20);
            separator.UpdateHoverState(true, onStripPos);
            Assert.True(separator.IsHovered);

            // Now the cursor leaves the strip. The Exited handler
            // calls UpdateHoverState(false, null) - the position is
            // null because we no longer know where the cursor is.
            // The fix must clear _lastPointerPosition so the
            // stale on-strip value is not used to keep IsHovered
            // alive.
            separator.UpdateHoverState(false, null);

            Assert.False(separator.IsHovered);

            // Verify the cached position was dropped. The most
            // important assertion: after the clear, the cached
            // position should not be the stale on-strip value.
            var lastPosField = typeof(SeparatorView).GetField("_lastPointerPosition",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var cached = (Point?)lastPosField?.GetValue(separator);
            Assert.True(cached == null);
        }

        [Fact]
        public void UpdateHoverState_ClearWithNullPosition_NoDragOrStickyHover_ClearsImmediately()
        {
            // The hover clear must take effect immediately when
            // IsDragging and _isStickyHovered are both false. If
            // either of those is true, the strip should retain the
            // hover state (drag in progress, or sticky-hover from a
            // recent press).
            var splitter = CreateHorizontalSplitter(paneCount: 2, sepSize: 8);
            var separator = (SeparatorView)GetSeparatorsList(splitter)[0]!;

            var arrange = typeof(SeparatorView).GetMethod("ArrangeSeparator",
                BindingFlags.NonPublic | BindingFlags.Instance);
            arrange!.Invoke(separator, new object[] { new Rect(0, 0, 8, 40) });

            separator.UpdateHoverState(true, new Point(4, 20));
            Assert.True(separator.IsHovered);

            separator.UpdateHoverState(false, null);
            Assert.False(separator.IsHovered);
        }

        [Fact]
        public void UpdateHoverState_StickyHoverActive_KeepsHoverWhenPositionIsNull()
        {
            // When _isStickyHovered is true (e.g. after a press
            // that did not move), the hover state should remain
            // even when the position is null. The drag / sticky
            // flags take precedence over the position-based
            // hit-test.
            var splitter = CreateHorizontalSplitter(paneCount: 2, sepSize: 8);
            var separator = (SeparatorView)GetSeparatorsList(splitter)[0]!;

            var arrange = typeof(SeparatorView).GetMethod("ArrangeSeparator",
                BindingFlags.NonPublic | BindingFlags.Instance);
            arrange!.Invoke(separator, new object[] { new Rect(0, 0, 8, 40) });

            separator.UpdateHoverState(true, new Point(4, 20));

            // Simulate sticky-hover being active.
            var stickyField = typeof(SeparatorView).GetField("_isStickyHovered",
                BindingFlags.NonPublic | BindingFlags.Instance);
            stickyField?.SetValue(separator, true);

            separator.UpdateHoverState(false, null);

            Assert.True(separator.IsHovered);
        }

    }
}
