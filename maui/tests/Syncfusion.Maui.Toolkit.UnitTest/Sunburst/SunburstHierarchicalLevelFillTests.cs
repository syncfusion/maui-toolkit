using System.Reflection;
using Syncfusion.Maui.Toolkit.SunburstChart;

namespace Syncfusion.Maui.Toolkit.UnitTest.Sunburst
{
    [CollectionDefinition("SerialFillTests", DisableParallelization = true)]
    public class SerialFillTestsCollection { }

    [Collection("SerialFillTests")]
    public class SunburstHierarchicalLevelFillTests : BaseUnitTest
    {
        private static class Brushes
        {
            public static readonly Brush Red = new SolidColorBrush(Colors.Red);
            public static readonly Brush Blue = new SolidColorBrush(Colors.Blue);
            public static readonly Brush Black = new SolidColorBrush(Colors.Black);
            public static readonly Brush White = new SolidColorBrush(Colors.White);
            public static readonly Brush Gray = new SolidColorBrush(Colors.Gray);
            public static readonly Brush Orange = new SolidColorBrush(Colors.Orange);
            public static readonly Brush Pink = new SolidColorBrush(Colors.Pink);
            public static readonly Brush Brown = new SolidColorBrush(Colors.Brown);
            public static readonly Brush Teal = new SolidColorBrush(Colors.Teal);
        }

        private static readonly PropertyInfo NeedsRelayoutProperty =
            typeof(SfSunburstChart).Assembly.GetType("Syncfusion.Maui.Toolkit.SunburstChart.SunburstChartArea")!.BaseType!.GetProperty("NeedsRelayout", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;

        private static bool GetNeedsRelayout(SfSunburstChart chart) => (bool)NeedsRelayoutProperty.GetValue(chart.area)!;

        private static void SetNeedsRelayout(SfSunburstChart chart, bool value) => NeedsRelayoutProperty.SetValue(chart.area, value);

        private sealed class Row
        {
            public string? Country { get; set; }
            public string? State { get; set; }
            public string? City { get; set; }
            public double Value { get; set; }
        }

        private static readonly Brush[] ChartBrushes =
        {
            new SolidColorBrush(Colors.Red), new SolidColorBrush(Colors.Green), new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.Yellow), new SolidColorBrush(Colors.Purple)
        };

        private static List<Row> BuildData(int countries)
        {
            var rows = new List<Row>();
            for (int c = 0; c < countries; c++)
            {
                rows.Add(new Row { Country = "C" + c, State = "S" + c + "a", City = "T" + c + "a1", Value = 1 });
                rows.Add(new Row { Country = "C" + c, State = "S" + c + "a", City = "T" + c + "a2", Value = 1 });
                rows.Add(new Row { Country = "C" + c, State = "S" + c + "b", City = "T" + c + "b1", Value = 1 });
            }
            return rows;
        }

        private static SfSunburstChart CreateChart(int countries = 3, bool withCityLevel = true, IList<Brush>? chartBrushes = null)
        {
            var chart = new SfSunburstChart { ValueMemberPath = "Value" };
            chart.PaletteBrushes = chartBrushes ?? ChartBrushes.ToList();
            chart.Levels.Add(new SunburstHierarchicalLevel { GroupMemberPath = "Country" });
            chart.Levels.Add(new SunburstHierarchicalLevel { GroupMemberPath = "State" });
            if (withCityLevel)
            {
                chart.Levels.Add(new SunburstHierarchicalLevel { GroupMemberPath = "City" });
            }
            chart.ItemsSource = BuildData(countries);
            return chart;
        }

        private static List<SunburstSegment> Ring(SfSunburstChart chart, int level)
        {
            return chart.Segments.Where(s => s.CurrentLevel == level).ToList();
        }

        private static Brush? Color(SunburstSegment s) => s.Fill;

        [Fact]
        public void Fill_DefaultsToNull()
        {
            Assert.Null(new SunburstHierarchicalLevel().Fill);
        }

        [Fact]
        public void Unset_SegmentsUseChartPalette()
        {
            var chart = CreateChart(5);
            chart.GenerateSegments();

            Assert.NotEmpty(chart.Segments);
            Assert.All(chart.Segments, s => Assert.Same(ChartBrushes[s.Index % ChartBrushes.Length], s.Fill));
        }

        [Fact]
        public void Fill_AppliesUniformlyToEveryRingSegment()
        {
            var chart = CreateChart(5);
            chart.Levels[1].Fill = Brushes.Orange;
            chart.GenerateSegments();

            var ring = Ring(chart, 1);
            Assert.True(ring.Select(s => s.Index).Distinct().Count() > 1);
            Assert.All(ring, s => Assert.Same(Brushes.Orange, s.Fill));
            Assert.All(Ring(chart, 0), s => Assert.Same(ChartBrushes[s.Index % ChartBrushes.Length], s.Fill));
            Assert.All(Ring(chart, 2), s => Assert.Same(ChartBrushes[s.Index % ChartBrushes.Length], s.Fill));
        }

        [Fact]
        public void DifferentLevels_UseIndependentFills()
        {
            var chart = CreateChart(3);
            chart.Levels[0].Fill = Brushes.Black;
            chart.Levels[2].Fill = Brushes.Teal;
            chart.GenerateSegments();

            Assert.All(Ring(chart, 0), s => Assert.Same(Brushes.Black, s.Fill));
            Assert.All(Ring(chart, 1), s => Assert.Same(ChartBrushes[s.Index % ChartBrushes.Length], s.Fill));
            Assert.All(Ring(chart, 2), s => Assert.Same(Brushes.Teal, s.Fill));
        }

        [Fact]
        public void Transparent_IsHonoredAsExplicitFill()
        {
            var chart = CreateChart(3);
            chart.Levels[0].Fill = Brush.Transparent;
            chart.GenerateSegments();

            Assert.All(Ring(chart, 0), s => Assert.Same(Brush.Transparent, s.Fill));
        }

        [Fact]
        public void ClearingFill_RestoresChartPalette()
        {
            var chart = CreateChart(3);
            chart.Levels[0].Fill = Brushes.Black;
            chart.Levels[0].Fill = null!;
            chart.GenerateSegments();

            Assert.All(Ring(chart, 0), s => Assert.Same(ChartBrushes[s.Index % ChartBrushes.Length], s.Fill));
        }

        [Fact]
        public void Selection_HighlightByBrush_OverridesLevelFill()
        {
            var chart = CreateChart(3);
            var selection = new SolidColorBrush(Colors.HotPink);
            chart.SelectionSettings = new SunburstSelectionSettings
            {
                DisplayMode = SunburstSelectionDisplayMode.HighlightByBrush,
                Fill = selection
            };
            chart.Levels[0].Fill = Brushes.Black;
            chart.GenerateSegments();

            var segment = Ring(chart, 0)[0];
            Assert.Same(Brushes.Black, segment.Fill);

            segment.IsSelected = true;
            InvokePrivateMethod(chart, "SetFillColor", segment);
            Assert.Same(selection, segment.Fill);

            segment.IsSelected = false;
            InvokePrivateMethod(chart, "SetFillColor", segment);
            Assert.Same(Brushes.Black, segment.Fill);
        }

        [Fact]
        public void RuntimeFillChange_RequestsChartUpdate()
        {
            var chart = CreateChart(3);
            SetNeedsRelayout(chart, false);

            chart.Levels[0].Fill = Brushes.Black;
            Assert.True(GetNeedsRelayout(chart));

            SetNeedsRelayout(chart, false);
            chart.Levels[0].Fill = Brushes.Black;
            Assert.False(GetNeedsRelayout(chart));

            chart.Levels[0].Fill = Brushes.White;
            Assert.True(GetNeedsRelayout(chart));
            chart.GenerateSegments();
            Assert.All(Ring(chart, 0), s => Assert.Same(Brushes.White, s.Fill));
        }

        private sealed class ViewModel : System.ComponentModel.INotifyPropertyChanged
        {
            private Brush? ringBrush;
            public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
            public Brush? RingBrush
            {
                get => ringBrush;
                set
                {
                    ringBrush = value;
                    PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(RingBrush)));
                }
            }
        }

        private sealed class ImmediateDispatcher : Microsoft.Maui.Dispatching.IDispatcher
        {
            public bool IsDispatchRequired => false;
            public Microsoft.Maui.Dispatching.IDispatcherTimer CreateTimer() => throw new NotSupportedException();
            public bool Dispatch(Action action) { action(); return true; }
            public bool DispatchDelayed(TimeSpan delay, Action action) { action(); return true; }
        }

        private sealed class ImmediateDispatcherProvider : Microsoft.Maui.Dispatching.IDispatcherProvider
        {
            public Microsoft.Maui.Dispatching.IDispatcher? GetForCurrentThread() => new ImmediateDispatcher();
        }

        [Fact]
        public void BindingChange_RepaintsLevel()
        {
            Microsoft.Maui.Dispatching.DispatcherProvider.SetCurrent(new ImmediateDispatcherProvider());
            var chart = CreateChart(3);
            var vm = new ViewModel { RingBrush = Brushes.Black };
            chart.Levels[0].BindingContext = vm;
            chart.Levels[0].SetBinding(SunburstHierarchicalLevel.FillProperty, nameof(ViewModel.RingBrush));
            chart.GenerateSegments();
            Assert.All(Ring(chart, 0), s => Assert.Same(Brushes.Black, s.Fill));

            SetNeedsRelayout(chart, false);
            vm.RingBrush = Brushes.Gray;
            Assert.True(GetNeedsRelayout(chart));
            chart.GenerateSegments();
            Assert.All(Ring(chart, 0), s => Assert.Same(Brushes.Gray, s.Fill));
        }

        [Fact]
        public void LegendIcons_FollowFirstLevelFill()
        {
            var chart = CreateChart(3);
            chart.Levels[0].Fill = Brushes.Black;
            chart.GenerateSegments();

            Assert.All(Ring(chart, 0), s => Assert.Same(Brushes.Black, s.Fill));
            // The chart palette itself is untouched.
            for (int i = 0; i < 3; i++)
            {
                Assert.Same(ChartBrushes[i], chart.GetFillColor(i));
            }

            var area = chart.area;
            chart.Legend = new SunburstLegend { IsVisible = true };
            InvokePrivateMethod(area, "UpdateLegendItemsSource");
            Assert.Equal(3, area.LegendItems.Count);
            Assert.All(area.LegendItems, item => Assert.Same(Brushes.Black, item.IconBrush));

            chart.Levels[0].Fill = Brushes.Gray;
            InvokePrivateMethod(area, "UpdateLegendIconColor");
            Assert.All(area.LegendItems, item => Assert.Same(Brushes.Gray, item.IconBrush));
        }

        [Fact]
        public void LegendIcons_UseChartPalette_WhenFirstLevelFillUnset()
        {
            var chart = CreateChart(3);
            chart.Levels[1].Fill = Brushes.Orange;
            chart.GenerateSegments();

            var area = chart.area;
            chart.Legend = new SunburstLegend { IsVisible = true };
            InvokePrivateMethod(area, "UpdateLegendItemsSource");
            for (int i = 0; i < area.LegendItems.Count; i++)
            {
                Assert.Same(ChartBrushes[i % ChartBrushes.Length], area.LegendItems[i].IconBrush);
            }
        }

        [Fact]
        public void LevelWithoutGroupMemberPath_FillBelongsToRenderedRing()
        {
            var chart = new SfSunburstChart { ValueMemberPath = "Value" };
            chart.PaletteBrushes = ChartBrushes.ToList();
            var skipped = new SunburstHierarchicalLevel { Fill = Brushes.White };
            var state = new SunburstHierarchicalLevel { GroupMemberPath = "State", Fill = Brushes.Black };
            chart.Levels.Add(new SunburstHierarchicalLevel { GroupMemberPath = "Country" });
            chart.Levels.Add(skipped);
            chart.Levels.Add(state);
            chart.ItemsSource = BuildData(3);
            chart.GenerateSegments();

            // The state ring is rendered second (CurrentLevel 1) but must use its own Fill, not Levels[1] (the skipped level).
            var ring = Ring(chart, 1);
            Assert.NotEmpty(ring);
            Assert.All(ring, s => Assert.Same(Brushes.Black, s.Fill));
            Assert.All(Ring(chart, 0), s => Assert.Same(ChartBrushes[s.Index % ChartBrushes.Length], s.Fill));
        }

        [Fact]
        public void DrillDown_FillFollowsDataLevel()
        {
            var chart = CreateChart(3);
            chart.Levels[0].Fill = Brushes.Black;
            chart.Levels[1].Fill = Brushes.Orange;
            chart.Levels[2].Fill = Brushes.Teal;
            chart.GenerateSegments();

            var target = Ring(chart, 1)[0];
            var manager = new DrillDownManager(chart);
            chart.DrillDownManager = manager;
            manager.DrillDown(target);
            chart.GenerateSunburstItems();
            chart.GenerateSegments();

            Assert.True(manager.IsZoomed);
            Assert.NotEmpty(chart.Segments);
            Assert.DoesNotContain(chart.Segments, s => s.CurrentLevel == 0);
            Assert.All(chart.Segments.Where(s => s.CurrentLevel == 1), s => Assert.Same(Brushes.Orange, s.Fill));
            Assert.All(chart.Segments.Where(s => s.CurrentLevel == 2), s => Assert.Same(Brushes.Teal, s.Fill));

            manager.IsZoomed = false;
            manager.ZoomingLevel = -1;
            chart.InternalDataSource = (System.Collections.IEnumerable)chart.ItemsSource;
            chart.GenerateSunburstItems();
            chart.GenerateSegments();
            Assert.All(chart.Segments.Where(s => s.CurrentLevel == 0), s => Assert.Same(Brushes.Black, s.Fill));
            Assert.All(chart.Segments.Where(s => s.CurrentLevel == 1), s => Assert.Same(Brushes.Orange, s.Fill));
            Assert.All(chart.Segments.Where(s => s.CurrentLevel == 2), s => Assert.Same(Brushes.Teal, s.Fill));
        }
    }
}
