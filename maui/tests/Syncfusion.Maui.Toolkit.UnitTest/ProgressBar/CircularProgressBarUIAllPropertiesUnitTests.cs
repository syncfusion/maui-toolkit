using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Maui.Toolkit.ProgressBar;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class CircularProgressBarUIAllPropertiesUnitTests : BaseUnitTest
    {
        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Animation_Duration_001()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.Progress = -49;
            circularProgressBar.AnimationDuration = 4000;

            Assert.Equal(-49, circularProgressBar.Progress);
            Assert.Equal(4000, circularProgressBar.AnimationDuration);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Animation_Duration_002()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.Progress = 28;
            circularProgressBar.AnimationDuration = 100;
            circularProgressBar.AnimationEasing = Easing.CubicInOut;

            Assert.Equal(28, circularProgressBar.Progress);
            Assert.Equal(Easing.CubicInOut, circularProgressBar.AnimationEasing);
            Assert.Equal(100, circularProgressBar.AnimationDuration);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_ChangeAngle()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.StartAngle = 234;
            circularProgressBar.EndAngle = 386;

            Assert.Equal(234, circularProgressBar.StartAngle);
            Assert.Equal(386, circularProgressBar.EndAngle);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_ChangeMinMax()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.Minimum = 27;
            circularProgressBar.Maximum = 78;

            Assert.Equal(78, circularProgressBar.Maximum);
            Assert.Equal(27, circularProgressBar.Minimum);

        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Custom_CircularProgressBar_001()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.Minimum = 36;
            circularProgressBar.Maximum = 53;
            circularProgressBar.StartAngle = 187;
            circularProgressBar.Progress = 40;
            circularProgressBar.TrackFill = Colors.Yellow;
            circularProgressBar.ProgressFill = Colors.Yellow;
            circularProgressBar.IndeterminateIndicatorWidthFactor = 0.40;
            circularProgressBar.TrackRadiusFactor = 0.67;
            circularProgressBar.TrackThickness = 25;
            circularProgressBar.ProgressRadiusFactor = 0.67;
            circularProgressBar.ProgressThickness = 14;
            circularProgressBar.TrackCornerStyle = CornerStyle.EndCurve;
            circularProgressBar.ProgressCornerStyle = CornerStyle.BothCurve;

            Assert.Equal(36, circularProgressBar.Minimum);
            Assert.Equal(53, circularProgressBar.Maximum);
            Assert.Equal(187, circularProgressBar.StartAngle);
            Assert.Equal(40, circularProgressBar.Progress);
            Assert.Equal(Colors.Yellow, circularProgressBar.TrackFill);
            Assert.Equal(Colors.Yellow, circularProgressBar.ProgressFill);
            Assert.Equal(0.40, circularProgressBar.IndeterminateIndicatorWidthFactor);
            Assert.Equal(0.67, circularProgressBar.TrackRadiusFactor);
            Assert.Equal(25, circularProgressBar.TrackThickness);
            Assert.Equal(0.67, circularProgressBar.ProgressRadiusFactor);
            Assert.Equal(14, circularProgressBar.ProgressThickness);
            Assert.Equal(CornerStyle.EndCurve, circularProgressBar.TrackCornerStyle);
            Assert.Equal(CornerStyle.BothCurve, circularProgressBar.ProgressCornerStyle);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Custom_CircularProgressBar_002()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.TrackFill = Color.FromArgb("#DCD5F6");
            circularProgressBar.ProgressFill = Colors.Yellow;
            circularProgressBar.TrackRadiusFactor = 0.63;
            circularProgressBar.TrackThickness = 25;
            circularProgressBar.ProgressRadiusFactor = 0.67;
            circularProgressBar.ProgressThickness = 19;
            circularProgressBar.GradientStops.Add(new ProgressGradientStop() { Value = 0, Color = Colors.Red });
            circularProgressBar.GradientStops.Add(new ProgressGradientStop() { Value = 20, Color = Colors.Yellow });
            circularProgressBar.GradientStops.Add(new ProgressGradientStop() { Value = 50, Color = Colors.Orange });

            Assert.Equal(Color.FromArgb("#DCD5F6"), ((SolidColorBrush)circularProgressBar.TrackFill).Color);
            Assert.Equal(Colors.Yellow, circularProgressBar.ProgressFill);
            Assert.Equal(0.63, circularProgressBar.TrackRadiusFactor);
            Assert.Equal(25, circularProgressBar.TrackThickness);
            Assert.Equal(0.67, circularProgressBar.ProgressRadiusFactor);
            Assert.Equal(19, circularProgressBar.ProgressThickness);
            Assert.Equal(Colors.Orange, circularProgressBar.GradientStops[2].Color);

            circularProgressBar.GradientStops.Clear();
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Custom_CircularProgressBar_003()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.TrackCornerStyle = CornerStyle.EndCurve;
            circularProgressBar.ProgressCornerStyle = CornerStyle.StartCurve;
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.IndeterminateIndicatorWidthFactor = 0.41;
            circularProgressBar.TrackRadiusFactor = 0.91;
            circularProgressBar.TrackThickness = 52;
            circularProgressBar.ProgressRadiusFactor = 0.88;
            circularProgressBar.ProgressThickness = 55;

            Assert.Equal(CornerStyle.EndCurve, circularProgressBar.TrackCornerStyle);
            Assert.Equal(CornerStyle.StartCurve, circularProgressBar.ProgressCornerStyle);
            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(0.41, circularProgressBar.IndeterminateIndicatorWidthFactor);
            Assert.Equal(0.91, circularProgressBar.TrackRadiusFactor);
            Assert.Equal(52, circularProgressBar.TrackThickness);
            Assert.Equal(0.88, circularProgressBar.ProgressRadiusFactor);
            Assert.Equal(55, circularProgressBar.ProgressThickness);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Custom_CircularProgressBar_004()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.TrackCornerStyle = CornerStyle.StartCurve;
            circularProgressBar.ProgressCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.TrackFill = Colors.Yellow;
            circularProgressBar.ProgressFill = Colors.Pink;

            Assert.Equal(Colors.Yellow, circularProgressBar.TrackFill);
            Assert.Equal(Colors.Pink, circularProgressBar.ProgressFill);
            Assert.Equal(CornerStyle.StartCurve, circularProgressBar.TrackCornerStyle);
            Assert.Equal(CornerStyle.BothCurve, circularProgressBar.ProgressCornerStyle);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Custom_CircularProgressBar_005()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ThicknessUnit = SizeUnit.Factor;
            circularProgressBar.ProgressFill = Colors.Violet;
            circularProgressBar.ProgressThickness = 0.66;
            circularProgressBar.TrackThickness = 0.66;
            circularProgressBar.SegmentCount = 7;
            circularProgressBar.SegmentGapWidth = 10;

            Assert.Equal(SizeUnit.Factor, circularProgressBar.ThicknessUnit);
            Assert.Equal(0.66, circularProgressBar.ProgressThickness);
            Assert.Equal(0.66, circularProgressBar.TrackThickness);
            Assert.Equal(Colors.Violet, circularProgressBar.ProgressFill);
            Assert.Equal(7, circularProgressBar.SegmentCount);
            Assert.Equal(10, circularProgressBar.SegmentGapWidth);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_GradientdStop()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.GradientStops.Add(new ProgressGradientStop() { Value = 0, Color = Colors.Red });
            circularProgressBar.GradientStops.Add(new ProgressGradientStop() { Value = 20, Color = Colors.Yellow });
            circularProgressBar.GradientStops.Add(new ProgressGradientStop() { Value = 50, Color = Colors.Orange });

            Assert.Equal(Colors.Orange, circularProgressBar.GradientStops[2].Color);

            circularProgressBar.GradientStops.Clear();
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_GradientdStopToNull()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            circularProgressBar.GradientStops = null;
            #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            circularProgressBar.ProgressFill = Colors.Gray;

            Assert.Equal(Colors.Gray, circularProgressBar.ProgressFill);
            Assert.Null(circularProgressBar.GradientStops);
        }

        [Theory]
        [InlineData(24)]
        [InlineData(61)]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_DecreaseProgress(double expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.Progress = expectedValue;
            double actualValue = circularProgressBar.Progress;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_ProgressFill_001()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.ProgressFill = Colors.Yellow;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(Colors.Yellow, circularProgressBar.ProgressFill);

        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_TrackFill_001()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.TrackFill = Colors.Pink;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(Colors.Pink, circularProgressBar.TrackFill);

        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_Annimation_Duration()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.AnimationEasing = Easing.SpringIn;
            circularProgressBar.IndeterminateAnimationDuration = 300;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(300, circularProgressBar.IndeterminateAnimationDuration);
            Assert.Equal(Easing.SpringIn, circularProgressBar.AnimationEasing);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_EasingEffect_001()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.AnimationEasing = Easing.Linear;
            
            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(Easing.Linear, circularProgressBar.AnimationEasing);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_EasingEffect_003()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.AnimationEasing = Easing.BounceOut;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(Easing.BounceOut, circularProgressBar.AnimationEasing);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_EasingEffect_005()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.AnimationEasing = Easing.SpringIn;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(Easing.SpringIn, circularProgressBar.AnimationEasing);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_EasingEffect_008()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.AnimationEasing = Easing.SinIn;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(Easing.SinIn, circularProgressBar.AnimationEasing);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_EasingEffect_009()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.AnimationEasing = Easing.SinOut;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(Easing.SinOut, circularProgressBar.AnimationEasing);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_IndeterminateWidthFactor()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.IndeterminateIndicatorWidthFactor = 0.48;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(0.48, circularProgressBar.IndeterminateIndicatorWidthFactor);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_Progress_RadiusFactor()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.ProgressRadiusFactor = 0.77;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(0.77, circularProgressBar.ProgressRadiusFactor);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_Progress_Thickness()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.ProgressThickness = 14;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(14, circularProgressBar.ProgressThickness);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_TrackRadiusFactor()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.TrackRadiusFactor = 0.71;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(0.71, circularProgressBar.TrackRadiusFactor);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_Indeterminate_Unit_TrackThickness()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.TrackThickness = 16;

            Assert.True(circularProgressBar.IsIndeterminate);
            Assert.Equal(16, circularProgressBar.TrackThickness);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_ProgressFill_001()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ProgressFill = Color.FromArgb("#DCD5F6");

            Assert.Equal(Color.FromArgb("#DCD5F6"), ((SolidColorBrush)circularProgressBar.ProgressFill).Color);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_ProgressFill_002()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ProgressFill = Color.FromArgb("#FF512BD4");

            Assert.Equal(Color.FromArgb("#FF512BD4"), ((SolidColorBrush)circularProgressBar.ProgressFill).Color);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_ProgressFill_003()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ProgressFill = Colors.Red;

            Assert.Equal(Colors.Red, circularProgressBar.ProgressFill);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_ProgressFill_004()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ProgressFill = Colors.Yellow;

            Assert.Equal(Colors.Yellow, circularProgressBar.ProgressFill);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_ProgressFill_006()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ProgressFill = Colors.Violet;

            Assert.Equal(Colors.Violet, circularProgressBar.ProgressFill);
        }

        [Theory]
        [InlineData(20,3)]
        [InlineData(10,12)]
        [InlineData(40,5)]
        public void MAUI_SfCircularProgressBar_AllProperties_SegmentCountWith_GapWidth_001(int expectedValue1, double expectedValue2)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.SegmentCount = expectedValue1;
            circularProgressBar.SegmentGapWidth = expectedValue2;

            int actualValue1 = circularProgressBar.SegmentCount;
            double actualValue2 = circularProgressBar.SegmentGapWidth;

            Assert.Equal(expectedValue1, actualValue1);
            Assert.Equal(expectedValue2, actualValue2);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackFill_001()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.TrackFill = Color.FromArgb("#DCD5F6");

            Assert.Equal(Color.FromArgb("#DCD5F6"), ((SolidColorBrush)circularProgressBar.TrackFill).Color);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackFill_002()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.TrackFill = Color.FromArgb("#FF512BD4");

            Assert.Equal(Color.FromArgb("#FF512BD4"), ((SolidColorBrush)circularProgressBar.TrackFill).Color);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackFill_003()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.TrackFill = Colors.Red;

            Assert.Equal(Colors.Red, circularProgressBar.TrackFill);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackFill_004()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.TrackFill = Colors.Yellow;

            Assert.Equal(Colors.Yellow, circularProgressBar.TrackFill);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackFill_005()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.TrackFill = Colors.Pink;

            Assert.Equal(Colors.Pink, circularProgressBar.TrackFill);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackFill_006()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.TrackFill = Colors.Violet;

            Assert.Equal(Colors.Violet, circularProgressBar.TrackFill);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackThickness_Unit_Progress_RadiusFactor()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ThicknessUnit = SizeUnit.Factor;
            circularProgressBar.ProgressRadiusFactor = 0.71;

            Assert.Equal(SizeUnit.Factor, circularProgressBar.ThicknessUnit);
            Assert.Equal(0.71, circularProgressBar.ProgressRadiusFactor);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackThickness_Unit_Progress_Thickness()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ThicknessUnit = SizeUnit.Factor;
            circularProgressBar.ProgressThickness = 0.21;

            Assert.Equal(SizeUnit.Factor, circularProgressBar.ThicknessUnit);
            Assert.Equal(0.21, circularProgressBar.ProgressThickness);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackThickness_Unit_SegmentCountWith_GapWidth()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ThicknessUnit = SizeUnit.Factor;
            circularProgressBar.SegmentCount = 5;

            Assert.Equal(SizeUnit.Factor, circularProgressBar.ThicknessUnit);
            Assert.Equal(5, circularProgressBar.SegmentCount);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_AllProperties_TrackThickness_Unit_TrackRadiusFactor()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.ThicknessUnit = SizeUnit.Factor;
            circularProgressBar.TrackRadiusFactor = 0.61;

            Assert.Equal(SizeUnit.Factor, circularProgressBar.ThicknessUnit);
            Assert.Equal(0.61, circularProgressBar.TrackRadiusFactor);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MAUI_SfCircularProgressBar_Indeterminate_Layout_CodeBehind(bool expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.Maximum = 280;
            circularProgressBar.Minimum = 0;
            circularProgressBar.Progress = 40;
            circularProgressBar.StartAngle = 120;
            circularProgressBar.EndAngle = 360;
            circularProgressBar.SegmentCount = 3;
            circularProgressBar.SegmentGapWidth = 5;
            circularProgressBar.ThicknessUnit = SizeUnit.Pixel;
            circularProgressBar.BackgroundColor = Colors.Transparent;
            circularProgressBar.TrackCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.ProgressCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.TrackFill = Color.FromArgb("00bdaf");
            circularProgressBar.ProgressFill = Color.FromArgb("e9648e");
            
            circularProgressBar.IsIndeterminate = expectedValue;
            bool actualValue = circularProgressBar.IsIndeterminate;

            ContentPage contentPage = new ContentPage();
            contentPage.Content = circularProgressBar;

            Assert.Equal(circularProgressBar, contentPage.Content);
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MAUI_SfCircularProgressBar_Indeterminate_Layout_ContentPage(bool expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.Maximum = 280;
            circularProgressBar.Minimum = 0;
            circularProgressBar.Progress = 40;
            circularProgressBar.StartAngle = 120;
            circularProgressBar.EndAngle = 360;
            circularProgressBar.SegmentCount = 2;
            circularProgressBar.SegmentGapWidth = 2;
            circularProgressBar.ThicknessUnit = SizeUnit.Pixel;
            circularProgressBar.BackgroundColor = Colors.Transparent;
            circularProgressBar.TrackCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.ProgressCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.TrackFill = Colors.Red;
            circularProgressBar.ProgressFill = Colors.Yellow;

            circularProgressBar.IsIndeterminate = expectedValue;
            bool actualValue = circularProgressBar.IsIndeterminate;

            ContentPage contentPage = new ContentPage();
            contentPage.Content = circularProgressBar;

            Assert.Equal(circularProgressBar, contentPage.Content);
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MAUI_SfCircularProgressBar_Indeterminate_Layout_Grid()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.Maximum = 280;
            circularProgressBar.Minimum = 0;
            circularProgressBar.Progress = 40;
            circularProgressBar.StartAngle = 120;
            circularProgressBar.EndAngle = 360;
            circularProgressBar.SegmentCount = 2;
            circularProgressBar.SegmentGapWidth = 2;
            circularProgressBar.IsIndeterminate = true;
            circularProgressBar.ThicknessUnit = SizeUnit.Pixel;
            circularProgressBar.BackgroundColor = Colors.Transparent;
            circularProgressBar.TrackCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.ProgressCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.TrackFill = Colors.Red;
            circularProgressBar.ProgressFill = Colors.Yellow;

            Grid grid = new Grid();
            grid.Children.Add(circularProgressBar);

            Assert.Single(grid);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MAUI_SfCircularProgressBar_Indeterminate_Layout_Scrollview(bool expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.Maximum = 280;
            circularProgressBar.Minimum = 0;
            circularProgressBar.Progress = 40;
            circularProgressBar.StartAngle = 120;
            circularProgressBar.EndAngle = 360;
            circularProgressBar.SegmentCount = 2;
            circularProgressBar.SegmentGapWidth = 2;
            circularProgressBar.ThicknessUnit = SizeUnit.Pixel;
            circularProgressBar.BackgroundColor = Colors.Transparent;
            circularProgressBar.TrackCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.ProgressCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.TrackFill = Colors.Red;
            circularProgressBar.ProgressFill = Colors.Yellow;

            circularProgressBar.IsIndeterminate = expectedValue;
            bool actualValue = circularProgressBar.IsIndeterminate;

            ScrollView scrollView = new ScrollView();
            scrollView.Content = circularProgressBar;


            Assert.Equal(circularProgressBar, scrollView.Content);
            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MAUI_SfCircularProgressBar_Indeterminate_Layout_Stacklayout(bool expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            circularProgressBar.Maximum = 280;
            circularProgressBar.Minimum = 0;
            circularProgressBar.Progress = 40;
            circularProgressBar.StartAngle = 120;
            circularProgressBar.EndAngle = 360;
            circularProgressBar.SegmentCount = 2;
            circularProgressBar.SegmentGapWidth = 2;
            circularProgressBar.ThicknessUnit = SizeUnit.Pixel;
            circularProgressBar.BackgroundColor = Colors.Transparent;
            circularProgressBar.TrackCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.ProgressCornerStyle = CornerStyle.BothCurve;
            circularProgressBar.TrackFill = Colors.Red;
            circularProgressBar.ProgressFill = Colors.Yellow;

            circularProgressBar.IsIndeterminate = expectedValue;
            bool actualValue = circularProgressBar.IsIndeterminate;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(circularProgressBar);

            Assert.Single(stackLayout);
            Assert.Equal(expectedValue, actualValue);
        }
    }
}