using Microsoft.Maui.Controls;
using Syncfusion.Maui.Toolkit.ProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class LinearProgressBarUIAllPropertiesUnitTests
    {
        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ChangeMaxandMin()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.Maximum = 85;
            linearProgressBar.Minimum = 24;

            Assert.Equal(24, linearProgressBar.Minimum);
            Assert.Equal(85, linearProgressBar.Maximum);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_CustomLinearProgressBar_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.Maximum = 68;
            linearProgressBar.Minimum = 26;
            linearProgressBar.Progress = 32;
            linearProgressBar.SecondaryProgress = 49;
            linearProgressBar.TrackFill = Colors.Green;
            linearProgressBar.ProgressFill = Colors.Blue;
            linearProgressBar.TrackCornerRadius = new CornerRadius(2, 2, 2, 2);
            linearProgressBar.ProgressCornerRadius = new CornerRadius(0, 0, 0, 2);
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(0, 0, 2, 0);

            Assert.Equal(68, linearProgressBar.Maximum);
            Assert.Equal(26, linearProgressBar.Minimum);
            Assert.Equal(32, linearProgressBar.Progress);
            Assert.Equal(49, linearProgressBar.SecondaryProgress);
            Assert.Equal(Colors.Green, linearProgressBar.TrackFill);
            Assert.Equal(Colors.Blue, linearProgressBar.ProgressFill);
            Assert.Equal(new CornerRadius(2, 2, 2, 2), linearProgressBar.TrackCornerRadius);
            Assert.Equal(new CornerRadius(0, 0, 0, 2), linearProgressBar.ProgressCornerRadius);
            Assert.Equal(new CornerRadius(0, 0, 2, 0), linearProgressBar.SecondaryProgressCornerRadius);

        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_CustomLinearProgressBar_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.IsIndeterminate = true;
            linearProgressBar.SegmentCount = 5;
            linearProgressBar.SegmentGapWidth = 8;

            Assert.Equal(5, linearProgressBar.SegmentCount);
            Assert.Equal(8, linearProgressBar.SegmentGapWidth);
            Assert.True(linearProgressBar.IsIndeterminate);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_CustomLinearProgressBar_003()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.IsIndeterminate = true;
            linearProgressBar.IndeterminateIndicatorWidthFactor = 0.85;
            linearProgressBar.ProgressHeight = 44;
            linearProgressBar.TrackHeight = 54;
            linearProgressBar.SecondaryProgressHeight = 45;
            linearProgressBar.ProgressPadding = 67;

            Assert.Equal(0.85, linearProgressBar.IndeterminateIndicatorWidthFactor);
            Assert.Equal(44, linearProgressBar.ProgressHeight);
            Assert.Equal(54, linearProgressBar.TrackHeight);
            Assert.Equal(45, linearProgressBar.SecondaryProgressHeight);
            Assert.Equal(67, linearProgressBar.ProgressPadding);
            Assert.True(linearProgressBar.IsIndeterminate);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_CustomLinearProgressBar_004()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.IsIndeterminate = true;
            linearProgressBar.Maximum = 46;
            linearProgressBar.Minimum = 17;
            linearProgressBar.Progress = 38;
            linearProgressBar.SecondaryProgress = 46;
            linearProgressBar.ProgressFill = Colors.Blue;
            linearProgressBar.TrackFill = Colors.Blue;
            linearProgressBar.ProgressCornerRadius = new CornerRadius(2, 3, 4, 5);

            linearProgressBar.GradientStops.Add(new ProgressGradientStop() { Value = 0, Color = Colors.Red });
            linearProgressBar.GradientStops.Add(new ProgressGradientStop() { Value = 20, Color = Colors.Yellow });
            linearProgressBar.GradientStops.Add(new ProgressGradientStop() { Value = 50, Color = Colors.Orange });

            Assert.Equal(46, linearProgressBar.Maximum);
            Assert.Equal(17, linearProgressBar.Minimum);
            Assert.Equal(38, linearProgressBar.Progress);
            Assert.Equal(46, linearProgressBar.SecondaryProgress);
            Assert.Equal(Colors.Blue, linearProgressBar.TrackFill);
            Assert.Equal(Colors.Blue, linearProgressBar.ProgressFill);
            Assert.Equal(new CornerRadius(2,3,4,5), linearProgressBar.ProgressCornerRadius);
            Assert.Equal(Colors.Orange, linearProgressBar.GradientStops[2].Color);
            Assert.True(linearProgressBar.IsIndeterminate);

            linearProgressBar.GradientStops.Clear();
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_CustomLinearProgressBar_005()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { Progress = 20 };

            var progressChangedFired = false;
            var progressCompletedFired = false;


            linearProgressBar.ProgressCompleted += (sender, e) => progressCompletedFired = true;
            linearProgressBar.ProgressChanged += (sender, e) => progressChangedFired = true;

            linearProgressBar.SetProgress(100, 0);
            Assert.True(progressChangedFired);
            Assert.True(progressCompletedFired);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(70)]
        public void MAUI_SfLinearProgressBar_AllProperties_Decrease_Progress(double expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.Progress = expectedValue;

            double actualValue = linearProgressBar.Progress;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_Easing_AnimationDuration_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.AnimationEasing = Easing.Linear;
            linearProgressBar.IndeterminateAnimationDuration = 6000;
            linearProgressBar.AnimationDuration = 3000;

            Assert.Equal(Easing.Linear, linearProgressBar.AnimationEasing);
            Assert.Equal(6000, linearProgressBar.IndeterminateAnimationDuration);
            Assert.Equal(3000, linearProgressBar.AnimationDuration);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_Easing_AnimationDuration_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.AnimationEasing = Easing.CubicInOut;
            linearProgressBar.IndeterminateAnimationDuration = 300;
            linearProgressBar.SecondaryAnimationDuration = 150;

            Assert.Equal(Easing.CubicInOut, linearProgressBar.AnimationEasing);
            Assert.Equal(300, linearProgressBar.IndeterminateAnimationDuration);
            Assert.Equal(150, linearProgressBar.SecondaryAnimationDuration);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_GradientStoNull()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Colors.Gray;

            #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            linearProgressBar.GradientStops = null;
            #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            Assert.Equal(Colors.Gray, linearProgressBar.ProgressFill);
            Assert.Null(linearProgressBar.GradientStops);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_IndeterminateEasing_AnimationDuration_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.IsIndeterminate = true;
            linearProgressBar.AnimationDuration = 10000;

            Assert.Equal(10000, linearProgressBar.AnimationDuration);
            Assert.True(linearProgressBar.IsIndeterminate);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_IndeterminateEasing_AnimationDuration_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.IsIndeterminate = true;
            linearProgressBar.AnimationDuration = 100;
            linearProgressBar.IndeterminateAnimationEasing = Easing.CubicInOut;

            Assert.Equal(100, linearProgressBar.AnimationDuration);
            Assert.Equal(Easing.CubicInOut, linearProgressBar.IndeterminateAnimationEasing);
            Assert.True(linearProgressBar.IsIndeterminate);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(2, 0, 0, 0);

            Assert.Equal(new CornerRadius(2, 0, 0, 0), linearProgressBar.ProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(0, 2, 0, 0);

            Assert.Equal(new CornerRadius(0, 2, 0, 0), linearProgressBar.ProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_003()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(0, 0, 2, 0);

            Assert.Equal(new CornerRadius(0, 0, 2, 0), linearProgressBar.ProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_004()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(0, 0, 0, 2);

            Assert.Equal(new CornerRadius(0, 0, 0, 2), linearProgressBar.ProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_005()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(2, 2, 2, 2);

            Assert.Equal(new CornerRadius(2, 2, 2, 2), linearProgressBar.ProgressCornerRadius);
        }


        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_006()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(2, 4, 3, 5);

            Assert.Equal(new CornerRadius(2, 4, 3, 5), linearProgressBar.ProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_007()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(30, 0, 0, 0);

            Assert.Equal(new CornerRadius(30, 0, 0, 0), linearProgressBar.ProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_008()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(0, 30, 0, 0);

            Assert.Equal(new CornerRadius(0, 30, 0, 0), linearProgressBar.ProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_009()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(0, 0, 30, 0);

            Assert.Equal(new CornerRadius(0, 0, 30, 0), linearProgressBar.ProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressCornerRadius_011()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressCornerRadius = new CornerRadius(30, 30, 30, 30);

            Assert.Equal(new CornerRadius(30, 30, 30, 30), linearProgressBar.ProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Colors.Red;

            Assert.Equal(Colors.Red, linearProgressBar.ProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Colors.Green;

            Assert.Equal(Colors.Green, linearProgressBar.ProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_003()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Color.FromArgb("#DCD5F6");

            Assert.Equal(Color.FromArgb("#DCD5F6"), ((SolidColorBrush)linearProgressBar.ProgressFill).Color);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_004()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Color.FromArgb("#FF512BD4");

            Assert.Equal(Color.FromArgb("#FF512BD4"), ((SolidColorBrush)linearProgressBar.ProgressFill).Color);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_005()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Color.FromArgb("#A895E9");

            Assert.Equal(Color.FromArgb("#A895E9"), ((SolidColorBrush)linearProgressBar.ProgressFill).Color);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_006()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Colors.Blue;

            Assert.Equal(Colors.Blue, linearProgressBar.ProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_007()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Colors.Yellow;

            Assert.Equal(Colors.Yellow, linearProgressBar.ProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_008()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Colors.Orange;

            Assert.Equal(Colors.Orange, linearProgressBar.ProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_009()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Colors.Black;

            Assert.Equal(Colors.Black, linearProgressBar.ProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_ProgressFill_010()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.ProgressFill = Colors.White;

            Assert.Equal(Colors.White, linearProgressBar.ProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressBar_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressFill = Colors.Green;

            Assert.Equal(Colors.Green, linearProgressBar.SecondaryProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressBar_003()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressFill = Color.FromArgb("#DCD5F6");

            Assert.Equal(Color.FromArgb("#DCD5F6"), ((SolidColorBrush)linearProgressBar.SecondaryProgressFill).Color);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressBar_005()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressFill = Color.FromArgb("#A895E9");

            Assert.Equal(Color.FromArgb("#A895E9"), ((SolidColorBrush)linearProgressBar.SecondaryProgressFill).Color);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressBar_006()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressFill = Colors.Blue;

            Assert.Equal(Colors.Blue, linearProgressBar.SecondaryProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressBar_007()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressFill = Colors.Yellow;

            Assert.Equal(Colors.Yellow, linearProgressBar.SecondaryProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressBar_008()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressFill = Colors.Orange;

            Assert.Equal(Colors.Orange, linearProgressBar.SecondaryProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressBar_009()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressFill = Colors.Black;

            Assert.Equal(Colors.Black, linearProgressBar.SecondaryProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressBar_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressFill = Colors.White;

            Assert.Equal(Colors.White, linearProgressBar.SecondaryProgressFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(2, 0, 0, 0);

            Assert.Equal(new CornerRadius(2, 0, 0, 0), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(0, 2, 0, 0);

            Assert.Equal(new CornerRadius(0, 2, 0, 0), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_003()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(0, 0, 2, 0);

            Assert.Equal(new CornerRadius(0, 0, 2, 0), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_004()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(0, 0, 0, 2);

            Assert.Equal(new CornerRadius(0, 0, 0, 2), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_005()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(2, 2, 2, 2);

            Assert.Equal(new CornerRadius(2, 2, 2, 2), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_006()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(2, 4, 3, 5);

            Assert.Equal(new CornerRadius(2, 4, 3, 5), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_007()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(30, 0, 0, 0);

            Assert.Equal(new CornerRadius(30, 0, 0, 0), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_008()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(0, 30, 0, 0);

            Assert.Equal(new CornerRadius(0, 30, 0, 0), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_009()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(0, 0, 30, 0);

            Assert.Equal(new CornerRadius(0, 0, 30, 0), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_010()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(0, 0, 0, 30);

            Assert.Equal(new CornerRadius(0, 0, 0, 30), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_011()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(30, 30, 30, 30);

            Assert.Equal(new CornerRadius(30, 30, 30, 30), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SecondaryProgressCornerRadius_012()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SecondaryProgressCornerRadius = new CornerRadius(25, 14, 10, 30);

            Assert.Equal(new CornerRadius(25, 14, 10, 30), linearProgressBar.SecondaryProgressCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SegmentCountWithWidthGap_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SegmentCount = 3;
            linearProgressBar.SegmentGapWidth = 1;

            Assert.Equal(1, linearProgressBar.SegmentGapWidth);
            Assert.Equal(3, linearProgressBar.SegmentCount );
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SegmentCountWithWidthGap_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SegmentCount = 15;
            linearProgressBar.SegmentGapWidth = 6;

            Assert.Equal(6, linearProgressBar.SegmentGapWidth);
            Assert.Equal(15, linearProgressBar.SegmentCount);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_SegmentCountWithWidthGap_003()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SegmentCount = 40;
            linearProgressBar.SegmentGapWidth = 5;

            Assert.Equal(5, linearProgressBar.SegmentGapWidth);
            Assert.Equal(40, linearProgressBar.SegmentCount);
        }

        [Theory]
        [InlineData(9)]
        [InlineData(466)]
        public void MAUI_SfLinearProgressBar_AllProperties_SegmentCount_001(int expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.SegmentCount = expectedValue;
            int actualValue = linearProgressBar.SegmentCount;
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(2, 0, 0, 0);

            Assert.Equal(new CornerRadius(2, 0, 0, 0), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(0, 2, 0, 0);

            Assert.Equal(new CornerRadius(0, 2, 0, 0), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_003()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(0, 0, 2, 0);

            Assert.Equal(new CornerRadius(0, 0, 2, 0), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_004()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(0, 0, 0, 2);

            Assert.Equal(new CornerRadius(0, 0, 0, 2), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_005()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(2, 2, 2, 2);

            Assert.Equal(new CornerRadius(2, 2, 2, 2), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_006()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(2, 4, 3, 5);

            Assert.Equal(new CornerRadius(2, 4, 3, 5), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_007()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(30, 0, 0, 0);

            Assert.Equal(new CornerRadius(30, 0, 0, 0), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_008()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(0, 30, 0, 0);

            Assert.Equal(new CornerRadius(0, 30, 0, 0), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_009()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(0, 0, 30, 0);

            Assert.Equal(new CornerRadius(0, 0, 30, 0), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_010()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(0, 0, 0, 30);

            Assert.Equal(new CornerRadius(0, 0, 0, 30), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackcornerRadius_012()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackCornerRadius = new CornerRadius(25, 14, 10, 30);

            Assert.Equal(new CornerRadius(25, 14, 10, 30), linearProgressBar.TrackCornerRadius);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackFill_001()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackFill = Colors.Red;

            Assert.Equal(Colors.Red, linearProgressBar.TrackFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackFill_002()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackFill = Colors.Green;

            Assert.Equal(Colors.Green, linearProgressBar.TrackFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackFill_003()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackFill = Color.FromArgb("#DCD5F6");

            Assert.Equal(Color.FromArgb("#DCD5F6"), ((SolidColorBrush)linearProgressBar.TrackFill).Color);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackFill_004()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackFill = Color.FromArgb("#FF512BD4");

            Assert.Equal(Color.FromArgb("#FF512BD4"), ((SolidColorBrush)linearProgressBar.TrackFill).Color);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackFill_007()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackFill = Colors.Yellow;

            Assert.Equal(Colors.Yellow, linearProgressBar.TrackFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackFill_008()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackFill = Colors.Orange;

            Assert.Equal(Colors.Orange, linearProgressBar.TrackFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackFill_009()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackFill = Colors.Black;

            Assert.Equal(Colors.Black, linearProgressBar.TrackFill);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_AllProperties_TrackFill_010()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.TrackFill = Colors.White;

            Assert.Equal(Colors.White, linearProgressBar.TrackFill);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MAUI_SfLinearProgressBar_Layout_ContentPage(bool setValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.Maximum = 100;
            linearProgressBar.Minimum = 0;
            linearProgressBar.Progress = 50;
            linearProgressBar.SegmentCount = 5;
            linearProgressBar.BackgroundColor = Colors.Transparent;
            linearProgressBar.SegmentGapWidth = 2;
            linearProgressBar.SecondaryProgress = 75;
            linearProgressBar.ProgressFill = Color.FromArgb("FFffbe06");
            linearProgressBar.SecondaryProgressFill = Color.FromArgb("FFff37");
            linearProgressBar.TrackFill = Color.FromArgb("3351483a");
            linearProgressBar.IsIndeterminate = setValue;

            ContentPage contentPage = new ContentPage();
            contentPage.Content = linearProgressBar;

            Assert.Equal(linearProgressBar, contentPage.Content);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void MAUI_SfLinearProgressBar_Layout_Grid(bool setValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.Maximum = 100;
            linearProgressBar.Minimum = 0;
            linearProgressBar.Progress = 50;
            linearProgressBar.SegmentCount = 5;
            linearProgressBar.BackgroundColor = Colors.Transparent;
            linearProgressBar.SegmentGapWidth = 2;
            linearProgressBar.SecondaryProgress = 75;
            linearProgressBar.ProgressFill = Color.FromArgb("FFffbe06");
            linearProgressBar.SecondaryProgressFill = Color.FromArgb("FFff37");
            linearProgressBar.TrackFill = Color.FromArgb("3351483a");
            linearProgressBar.IsIndeterminate = setValue;

            Grid grid = new Grid();
            grid.Children.Add(linearProgressBar);

            Assert.Single(grid);
        }

        [Fact]
        public void MAUI_SfLinearProgressBar_Layout_ScrollView()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.Maximum = 100;
            linearProgressBar.Minimum = 0;
            linearProgressBar.Progress = 50;
            linearProgressBar.SegmentCount = 5;
            linearProgressBar.BackgroundColor = Colors.Transparent;
            linearProgressBar.SegmentGapWidth = 2;
            linearProgressBar.SecondaryProgress = 75;
            linearProgressBar.ProgressFill = Color.FromArgb("FFffbe06");
            linearProgressBar.SecondaryProgressFill = Color.FromArgb("FFff37");
            linearProgressBar.TrackFill = Color.FromArgb("3351483a");
            linearProgressBar.IsIndeterminate = false;

            ScrollView scrollView = new ScrollView();
            scrollView.Content = linearProgressBar;

            Assert.Equal(linearProgressBar, scrollView.Content);
        }


        [Fact]
        public void MAUI_SfLinearProgressBar_Layout_StackLayout()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();
            linearProgressBar.Maximum = 100;
            linearProgressBar.Minimum = 0;
            linearProgressBar.Progress = 50;
            linearProgressBar.SegmentCount = 5;
            linearProgressBar.BackgroundColor = Colors.Transparent;
            linearProgressBar.SegmentGapWidth = 2;
            linearProgressBar.SecondaryProgress = 75;
            linearProgressBar.ProgressFill = Color.FromArgb("FFffbe06");
            linearProgressBar.SecondaryProgressFill = Color.FromArgb("FFff37");
            linearProgressBar.TrackFill = Color.FromArgb("3351483a");
            linearProgressBar.IsIndeterminate = false;

            StackLayout stackLayout = new StackLayout();
            stackLayout.Children.Add(linearProgressBar);

            Assert.Single(stackLayout);
        }

    }
}
