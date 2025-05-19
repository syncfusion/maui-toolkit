using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.ProgressBar;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Toolkit.UnitTest
{
    public class ProgressBarBaseUnitTests
    {
        #region Constructor

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly_CircularProgressBar()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Assert.Equal(1000d, circularProgressBar.AnimationDuration);
            Assert.Equal(Easing.Linear, circularProgressBar.AnimationEasing);
            Assert.Equal(3000d, circularProgressBar.IndeterminateAnimationDuration);
            Assert.Empty(circularProgressBar.GradientStops);
            Assert.Equal(Easing.Linear, circularProgressBar.IndeterminateAnimationEasing);
            Assert.Equal(0.25d, circularProgressBar.IndeterminateIndicatorWidthFactor);
            Assert.False(circularProgressBar.IsIndeterminate);
            Assert.Equal(100d, circularProgressBar.Maximum);
            Assert.Equal(0d, circularProgressBar.Minimum);
            Assert.Equal(0d, circularProgressBar.Progress);
            Assert.Equal(1, circularProgressBar.SegmentCount);
            Assert.Equal(3d, circularProgressBar.SegmentGapWidth);
            Assert.Equal(Color.FromArgb("#6750A4"), ((SolidColorBrush)circularProgressBar.ProgressFill).Color);
            Assert.Equal(Color.FromArgb("#E7E0EC"), ((SolidColorBrush)circularProgressBar.TrackFill).Color);
        }

        [Fact]
        public void Constructor_InitializesDefaultsCorrectly_LinearProgressBar()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Assert.Equal(1000d, linearProgressBar.AnimationDuration);
            Assert.Equal(Easing.Linear, linearProgressBar.AnimationEasing);
            Assert.Equal(3000d, linearProgressBar.IndeterminateAnimationDuration);
            Assert.Empty(linearProgressBar.GradientStops);
            Assert.Equal(Easing.Linear, linearProgressBar.IndeterminateAnimationEasing);
            Assert.Equal(0.25d, linearProgressBar.IndeterminateIndicatorWidthFactor);
            Assert.False(linearProgressBar.IsIndeterminate);
            Assert.Equal(100d, linearProgressBar.Maximum);
            Assert.Equal(0d, linearProgressBar.Minimum);
            Assert.Equal(0d, linearProgressBar.Progress);
            Assert.Equal(1, linearProgressBar.SegmentCount);
            Assert.Equal(3d, linearProgressBar.SegmentGapWidth);
			Assert.Equal(Color.FromArgb("#6750A4"), ((SolidColorBrush)linearProgressBar.ProgressFill).Color);
            Assert.Equal(Color.FromArgb("#E7E0EC"), ((SolidColorBrush)linearProgressBar.TrackFill).Color);
        }

        #endregion

        #region Circular Progress Bar Public Properties

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void AnimationDuration_GetAndSet_UsingDouble(double expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.AnimationDuration = expectedValue;
            double actualValue = circularProgressBar.AnimationDuration;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void IndeterminateAnimationDuration_GetAndSet_UsingDouble(double expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.IndeterminateAnimationDuration = expectedValue;
            double actualValue = circularProgressBar.IndeterminateAnimationDuration;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void Maximum_GetAndSet_UsingDouble(double expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.Maximum = expectedValue;
            double actualValue = circularProgressBar.Maximum;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void Minimum_GetAndSet_UsingDouble(double expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.Minimum = expectedValue;
            double actualValue = circularProgressBar.Minimum;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void Progress_GetAndSet_UsingDouble(double expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.Progress = expectedValue;
            double actualValue = circularProgressBar.Progress;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void SegmentGapWidth_GetAndSet_UsingDouble(double expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.SegmentGapWidth = expectedValue;
            double actualValue = circularProgressBar.SegmentGapWidth;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void SegmentGapWidth_GetAndSet_UsingInt(int expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.SegmentCount = expectedValue;
            int actualValue = circularProgressBar.SegmentCount;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsIndeterminate_GetAndSet_UsingBool(bool expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
            
            circularProgressBar.IsIndeterminate = expectedValue;

            Assert.Equal(expectedValue, circularProgressBar.IsIndeterminate);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void ProgressFill_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            circularProgressBar.ProgressFill = expectedValue;
            Brush actualValue = circularProgressBar.ProgressFill;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void TrackFill_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            circularProgressBar.TrackFill = expectedValue;
            Brush actualValue = circularProgressBar.TrackFill;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingLinear()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.Linear;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingSpringIn()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SpringIn;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingSpringOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SpringOut;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingSinIn()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SinIn;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingSinOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SinOut;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingSinInOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SinInOut;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingBounceIn()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.BounceIn;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingBounceOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.BounceOut;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingCubicIn()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.CubicIn;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingCubicOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.CubicInOut;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingCubicInOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.CubicInOut;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AnimationEasing_GetAndSet_UsingDefault()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.Default;
            circularProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingLinear()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.Linear;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingSpringIn()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SpringIn;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingSpringOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SpringOut;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingSinIn()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SinIn;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingSinOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SinOut;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingSinInOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.SinInOut;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingBounceIn()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.BounceIn;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingBounceOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.BounceOut;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingCubicIn()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.CubicIn;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingCubicOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.CubicInOut;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingCubicInOut()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.CubicInOut;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void IndeterminateAnimationEasing_GetAndSet_UsingDefault()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            Easing expectedValue = Easing.Default;
            circularProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = circularProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void GradientStops_GetAndSet_UsingCollection()
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            ObservableCollection<ProgressGradientStop> expectedValue = new ObservableCollection<ProgressGradientStop>();
            expectedValue.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 ,ActualValue = 40});
            circularProgressBar.GradientStops = expectedValue;
            ObservableCollection<ProgressGradientStop> actualValue = expectedValue;

            Assert.Equal(expectedValue[0].Color, actualValue[0].Color);
            Assert.Equal(expectedValue[0].Value, actualValue[0].Value);
            Assert.Equal(expectedValue[0].ActualValue, actualValue[0].ActualValue);
            circularProgressBar.GradientStops.Clear();
        }

        [Theory]
        [InlineData(1, 50)]
        [InlineData(1, 250)]
        [InlineData(0, -40)]
        [InlineData(0, -850)]
        [InlineData(0, 0)]
        public void IndeterminateIndicatorWidthFactor_GetAndSet_UsingDouble(double expectedValue, double setValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.IndeterminateIndicatorWidthFactor = setValue;
            double actualValue = circularProgressBar.IndeterminateIndicatorWidthFactor;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Linear Progress Bar Public Properties


        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void LinearProgressBar_AnimationDuration_GetAndSet_UsingDouble(double expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.AnimationDuration = expectedValue;
            double actualValue = linearProgressBar.AnimationDuration;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void LinearProgressBar_IndeterminateAnimationDuration_GetAndSet_UsingDouble(double expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.IndeterminateAnimationDuration = expectedValue;
            double actualValue = linearProgressBar.IndeterminateAnimationDuration;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void LinearProgressBar_Maximum_GetAndSet_UsingDouble(double expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.Maximum = expectedValue;
            double actualValue = linearProgressBar.Maximum;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void LinearProgressBar_Minimum_GetAndSet_UsingDouble(double expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.Minimum = expectedValue;
            double actualValue = linearProgressBar.Minimum;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void LinearProgressBar_Progress_GetAndSet_UsingDouble(double expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.Progress = expectedValue;
            double actualValue = linearProgressBar.Progress;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void LinearProgressBar_SegmentGapWidth_GetAndSet_UsingDouble(double expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.SegmentGapWidth = expectedValue;
            double actualValue = linearProgressBar.SegmentGapWidth;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void LinearProgressBar_SegmentGapWidth_GetAndSet_UsingInt(int expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.SegmentCount = expectedValue;
            int actualValue = linearProgressBar.SegmentCount;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void LinearProgressBar_IsIndeterminate_GetAndSet_UsingBool(bool expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.IsIndeterminate = expectedValue;

            Assert.Equal(expectedValue, linearProgressBar.IsIndeterminate);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void LinearProgressBar_ProgressFill_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            linearProgressBar.ProgressFill = expectedValue;
            Brush actualValue = linearProgressBar.ProgressFill;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(255, 0, 0)]
        [InlineData(0, 255, 0)]
        [InlineData(0, 0, 255)]
        [InlineData(255, 255, 0)]
        [InlineData(0, 255, 255)]
        public void LinearProgressBar_TrackFill_GetAndSet_UsingBrush(byte red, byte green, byte blue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Brush expectedValue = Color.FromRgb(red, green, blue);
            linearProgressBar.TrackFill = expectedValue;
            Brush actualValue = linearProgressBar.TrackFill;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingLinear()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.Linear;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingSpringIn()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SpringIn;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingSpringOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SpringOut;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingSinIn()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SinIn;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingSinOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SinOut;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingSinInOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SinInOut;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingBounceIn()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.BounceIn;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingBounceOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.BounceOut;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingCubicIn()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.CubicIn;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingCubicOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.CubicInOut;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingCubicInOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.CubicInOut;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_AnimationEasing_GetAndSet_UsingDefault()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.Default;
            linearProgressBar.AnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.AnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingLinear()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.Linear;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingSpringIn()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SpringIn;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingSpringOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SpringOut;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingSinIn()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SinIn;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingSinOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SinOut;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingSinInOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.SinInOut;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingBounceIn()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.BounceIn;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingBounceOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.BounceOut;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingCubicIn()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.CubicIn;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingCubicOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.CubicInOut;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingCubicInOut()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.CubicInOut;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_IndeterminateAnimationEasing_GetAndSet_UsingDefault()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            Easing expectedValue = Easing.Default;
            linearProgressBar.IndeterminateAnimationEasing = expectedValue;
            Easing actualValue = linearProgressBar.IndeterminateAnimationEasing;

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void LinearProgressBar_GradientStops_GetAndSet_UsingCollection()
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            ObservableCollection<ProgressGradientStop> expectedValue = new ObservableCollection<ProgressGradientStop>();
            expectedValue.Add(new ProgressGradientStop { Color = Colors.Yellow, Value = 30 ,ActualValue = 40 });
            linearProgressBar.GradientStops = expectedValue;
            ObservableCollection<ProgressGradientStop> actualValue = expectedValue;

            Assert.Equal(expectedValue[0].Color, actualValue[0].Color);
            Assert.Equal(expectedValue[0].Value, actualValue[0].Value);
            Assert.Equal(expectedValue[0].ActualValue, actualValue[0].ActualValue);
            linearProgressBar.GradientStops.Clear();
        }

        [Theory]
        [InlineData(1, 50)]
        [InlineData(1, 250)]
        [InlineData(0, -40)]
        [InlineData(0, -850)]
        [InlineData(0, 0)]
        public void LinearProgressBar_IndeterminateIndicatorWidthFactor_GetAndSet_UsingDouble(double expectedValue, double setValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.IndeterminateIndicatorWidthFactor = setValue;
            double actualValue = linearProgressBar.IndeterminateIndicatorWidthFactor;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Internal Properties

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void ProgressValueEventArgs_CurrentProgress_GetAndSet_UsingDouble(double expectedValue)
        {
            
            ProgressValueEventArgs eventArgs = new ProgressValueEventArgs();

            eventArgs.CurrentProgress = expectedValue;
            double actualValue = eventArgs.CurrentProgress;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void ProgressValueEventArgs_Progress_GetAndSet_UsingDouble(double expectedValue)
        {

            ProgressValueEventArgs eventArgs = new ProgressValueEventArgs();

            eventArgs.CurrentProgress = expectedValue;
            double actualValue = eventArgs.Progress;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void Linear_ActualProgress_GetAndSet_UsingDouble(double expectedValue)
        {

            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.ActualProgress = expectedValue;
            double actualValue = linearProgressBar.ActualProgress;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(250)]
        [InlineData(-40)]
        [InlineData(-850)]
        [InlineData(0)]
        public void Circular_ActualProgress_GetAndSet_UsingDouble(double expectedValue)
        {

            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.ActualProgress = expectedValue;
            double actualValue = circularProgressBar.ActualProgress;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Circular_IsIndeterminateAnimationAborted_GetAndSet_UsingBool(bool expectedValue)
        {
            SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

            circularProgressBar.IsIndeterminateAnimationAborted = expectedValue;
            bool actualValue = circularProgressBar.IsIndeterminateAnimationAborted;

            Assert.Equal(expectedValue, actualValue);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Linear_IsIndeterminateAnimationAborted_GetAndSet_UsingBool(bool expectedValue)
        {
            SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

            linearProgressBar.IsIndeterminateAnimationAborted = expectedValue;
            bool actualValue = linearProgressBar.IsIndeterminateAnimationAborted;

            Assert.Equal(expectedValue, actualValue);
        }

        #endregion

        #region Utilities

        [Theory]
        [InlineData(20, 0)]
        [InlineData(20, 90)]
        [InlineData(20, 180)]
        [InlineData(20, 270)]
        [InlineData(20, 360)]
        [InlineData(0, 345)]
        public void GetMinMaxValue_ReturnsValue_WhenCalled(double expectedValue, int setDegree)
        {
            double actualValue = Utility.GetMinMaxValue(new Point(20,20),new Point(20,20),setDegree);

            Assert.Equal(expectedValue, actualValue);

        }

        #endregion
    }
}
