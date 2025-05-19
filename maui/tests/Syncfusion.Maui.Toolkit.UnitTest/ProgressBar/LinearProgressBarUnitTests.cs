using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.ProgressBar;
using System;

namespace Syncfusion.Maui.Toolkit.UnitTest;
public class LinearProgressBarUnitTests : BaseUnitTest
{
    #region Public Properties

    [Fact]
    public void Constructor_InitializesDefaultsCorrectly()
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        Assert.Equal(0d,linearProgressBar.ProgressCornerRadius);
        Assert.Equal(0d, linearProgressBar.SecondaryProgressCornerRadius);
        Assert.Equal(5d, linearProgressBar.ProgressHeight);
        Assert.Equal(5d, linearProgressBar.TrackHeight);
        Assert.Equal(5d, linearProgressBar.SecondaryProgressHeight);
        Assert.Equal(0d, linearProgressBar.ProgressPadding);
        Assert.Equal(1500d, linearProgressBar.SecondaryAnimationDuration);
        Assert.Equal(0d, linearProgressBar.SecondaryProgress);
        Assert.Equal(0d, linearProgressBar.TrackCornerRadius);
        Assert.Equal(Color.FromArgb("#806750a4"), ((SolidColorBrush)linearProgressBar.SecondaryProgressFill).Color);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ProgressCornerradius_GetAndSet_UsingDouble(double cornerRadiusValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        CornerRadius expectedValue = new CornerRadius(cornerRadiusValue);
        linearProgressBar.ProgressCornerRadius = expectedValue;
        CornerRadius actualValue = linearProgressBar.ProgressCornerRadius;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void SecondaryProgressCornerRadius_GetAndSet_UsingDouble(double cornerRadiusValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        CornerRadius expectedValue = new CornerRadius(cornerRadiusValue);
        linearProgressBar.SecondaryProgressCornerRadius = expectedValue;
        CornerRadius actualValue = linearProgressBar.SecondaryProgressCornerRadius;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void TrackCornerRadius_GetAndSet_UsingDouble(double cornerRadiusValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        CornerRadius expectedValue = new CornerRadius(cornerRadiusValue);
        linearProgressBar.TrackCornerRadius = expectedValue;
        CornerRadius actualValue = linearProgressBar.TrackCornerRadius;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ProgressHeight_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        linearProgressBar.ProgressHeight = expectedValue;
        double actualValue = linearProgressBar.ProgressHeight;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void TrackHeight_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        linearProgressBar.TrackHeight = expectedValue;
        double actualValue = linearProgressBar.TrackHeight;

        Assert.Equal(expectedValue, actualValue);
    }


    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void SecondaryProgressHeight_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        linearProgressBar.SecondaryProgressHeight = expectedValue;
        double actualValue = linearProgressBar.SecondaryProgressHeight;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void SecondaryAnimationDuration_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        linearProgressBar.SecondaryAnimationDuration = expectedValue;
        double actualValue = linearProgressBar.SecondaryAnimationDuration;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void SecondaryProgress_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        linearProgressBar.SecondaryProgress = expectedValue;
        double actualValue = linearProgressBar.SecondaryProgress;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ProgressPAdding_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        linearProgressBar.ProgressPadding = expectedValue;
        double actualValue = linearProgressBar.ProgressPadding;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(255, 0, 0)]
    [InlineData(0, 255, 0)]
    [InlineData(0, 0, 255)]
    [InlineData(255, 255, 0)]
    [InlineData(0, 255, 255)]
    public void SecondaryProgressFill_GetAndSet_UsingBrush(byte red, byte green, byte blue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        Brush expectedValue = Color.FromRgb(red, green, blue);
        linearProgressBar.SecondaryProgressFill = expectedValue;
        Brush actualValue = linearProgressBar.SecondaryProgressFill;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(255, 0, 0)]
    [InlineData(0, 255, 0)]
    [InlineData(0, 0, 255)]
    [InlineData(255, 255, 0)]
    [InlineData(0, 255, 255)]
    public void LinearProgressBarBackground_Internal_GetAndSet_UsingColor(byte red, byte green, byte blue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        Color expectedValue = Color.FromRgb(red, green, blue);
        linearProgressBar.LinearProgressBarBackground = expectedValue;
        Brush actualValue = linearProgressBar.LinearProgressBarBackground;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ProgressCornerradiusWithSize_GetAndSet_UsingDouble(double cornerRadiusValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        CornerRadius expectedValue = new CornerRadius(cornerRadiusValue);
        linearProgressBar.ProgressCornerRadius = expectedValue;
        CornerRadius actualValue = linearProgressBar.ProgressCornerRadius;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void SecondaryProgressCornerRadiusWithSize_GetAndSet_UsingDouble(double cornerRadiusValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        CornerRadius expectedValue = new CornerRadius(cornerRadiusValue);
        linearProgressBar.SecondaryProgressCornerRadius = expectedValue;
        CornerRadius actualValue = linearProgressBar.SecondaryProgressCornerRadius;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void TrackCornerRadiusWithSize_GetAndSet_UsingDouble(double cornerRadiusValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        CornerRadius expectedValue = new CornerRadius(cornerRadiusValue);
        linearProgressBar.TrackCornerRadius = expectedValue;
        CornerRadius actualValue = linearProgressBar.TrackCornerRadius;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ProgressHeightWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        linearProgressBar.ProgressHeight = expectedValue;
        double actualValue = linearProgressBar.ProgressHeight;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void TrackHeightWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        linearProgressBar.TrackHeight = expectedValue;
        double actualValue = linearProgressBar.TrackHeight;

        Assert.Equal(expectedValue, actualValue);
    }


    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void SecondaryProgressHeightWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        linearProgressBar.SecondaryProgressHeight = expectedValue;
        double actualValue = linearProgressBar.SecondaryProgressHeight;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void SecondaryAnimationDurationWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        linearProgressBar.SecondaryAnimationDuration = expectedValue;
        double actualValue = linearProgressBar.SecondaryAnimationDuration;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void SecondaryProgressWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        linearProgressBar.SecondaryProgress = expectedValue;
        double actualValue = linearProgressBar.SecondaryProgress;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ProgressPaddingWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        linearProgressBar.ProgressPadding = expectedValue;
        double actualValue = linearProgressBar.ProgressPadding;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(255, 0, 0)]
    [InlineData(0, 255, 0)]
    [InlineData(0, 0, 255)]
    [InlineData(255, 255, 0)]
    [InlineData(0, 255, 255)]
    public void SecondaryProgressFillWithSize_GetAndSet_UsingBrush(byte red, byte green, byte blue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        Brush expectedValue = Color.FromRgb(red, green, blue);
        linearProgressBar.SecondaryProgressFill = expectedValue;
        Brush actualValue = linearProgressBar.SecondaryProgressFill;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(255, 0, 0)]
    [InlineData(0, 255, 0)]
    [InlineData(0, 0, 255)]
    [InlineData(255, 255, 0)]
    [InlineData(0, 255, 255)]
    public void LinearProgressBarBackgroundWithSize_Internal_GetAndSet_UsingColor(byte red, byte green, byte blue)
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { AvailableSize = new Size(100, 100) };

        Color expectedValue = Color.FromRgb(red, green, blue);
        linearProgressBar.LinearProgressBarBackground = expectedValue;
        Brush actualValue = linearProgressBar.LinearProgressBarBackground;

        Assert.Equal(expectedValue, actualValue);
    }

    #endregion

    #region Events

    [Fact]
    public void ProgressChanged_ProgressChanges_WhenCalled()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { Progress = 20 };
        var fired = false;

        circularProgressBar.ProgressChanged += (sender, e) => fired = true;
        InvokePrivateMethod(circularProgressBar, "OnProgressAnimationFinished", 70, false);

        Assert.True(fired);
    }

    [Fact]
    public void ProgressCompleted_ProgressCompletes_WhenCalled()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { Progress = 100 };
        var fired = false;

        circularProgressBar.ProgressCompleted += (sender, e) => fired = true;
        InvokePrivateMethod(circularProgressBar, "OnProgressAnimationFinished", 100, true);

        Assert.True(fired);
    }

    #endregion

    #region Methods

    [Fact]
    public void SetProgress_ChangesProgress_WhenCalled()
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { Progress = 20 };

        linearProgressBar.SetProgress(70, 0);

        Assert.Equal(70, linearProgressBar.Progress);
    }

    [Fact]
    public void CalculateIndicatorWidth_ReturnsWidth_WhenCalled()
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        double actulValue = Convert.ToDouble(InvokePrivateMethod(linearProgressBar, "CalculateIndicatorWidth",500,20));

        Assert.Equal(-20, actulValue);
    }

    [Fact]
    public void GetWidthFromValue_ReturnsWidth_WhenCalled()
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        double actulValue = Convert.ToDouble(InvokePrivateMethod(linearProgressBar, "GetWidthFromValue", 500, 20));

        Assert.Equal(-20, actulValue);
    }

    [Fact]
    public void CalculateIndicatorWidthElse_ReturnsWidth_WhenCalled()
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar() { SegmentGapWidth = 0, SegmentCount = 0 };

        double actulValue = Convert.ToDouble(InvokePrivateMethod(linearProgressBar, "CalculateIndicatorWidth", 500, 20));

        Assert.Equal(-20, actulValue);
    }

    [Fact]
    public void OnIndeterminateAnimationUpdate_SetValue_WhenCalled()
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        InvokePrivateMethod(linearProgressBar, "OnIndeterminateAnimationUpdate", 120);
        double actulValue = Convert.ToDouble(GetPrivateField(linearProgressBar,"_indeterminateAnimationValue"));

        Assert.Equal(120, actulValue);
    }

    [Fact]
    public void OnSecondaryAnimationUpdate_SetValue_WhenCalled()
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        InvokePrivateMethod(linearProgressBar, "OnSecondaryAnimationUpdate", 120);
        double actulValue = Convert.ToDouble(GetPrivateField(linearProgressBar, "_actualSecondaryProgress"));

        Assert.Equal(120, actulValue);
    }

    [Fact]
    public void OnSecondaryAnimationFinished_SetValue_WhenCalled()
    {
        SfLinearProgressBar linearProgressBar = new SfLinearProgressBar();

        InvokePrivateMethod(linearProgressBar, "OnSecondaryAnimationFinished", 120, true);
        bool actulValue = Convert.ToBoolean(GetPrivateField(linearProgressBar, "_canAnimateSecondaryProgress"));

        Assert.False(actulValue);
    }

    #endregion
}