using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Toolkit.ProgressBar;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Toolkit.UnitTest;
public class CircularProgressBarUnitTests : BaseUnitTest
{
    #region Public Properties

    [Fact]
    public void Constructor_InitializesDefaultsCorrectly()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        Assert.Equal(630d,circularProgressBar.EndAngle);
        Assert.Equal(CornerStyle.BothFlat ,circularProgressBar.ProgressCornerStyle);
        Assert.Equal(0.9d, circularProgressBar.ProgressRadiusFactor);
        Assert.Equal(5d, circularProgressBar.ProgressThickness);
        Assert.Equal(270d, circularProgressBar.StartAngle);
        Assert.Equal(SizeUnit.Pixel, circularProgressBar.ThicknessUnit);
        Assert.Equal(CornerStyle.BothFlat, circularProgressBar.TrackCornerStyle);
        Assert.Equal(0.9d, circularProgressBar.TrackRadiusFactor);
        Assert.Equal(5d, circularProgressBar.TrackThickness);
        Assert.Null(circularProgressBar.Content);

    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void StartAngle_GetAndSet_UsingDouble(double expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        circularProgressBar.StartAngle = expectedValue;
        double actualValue = circularProgressBar.StartAngle;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void EndAngle_GetAndSet_UsingDouble(double expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        circularProgressBar.EndAngle = expectedValue;
        double actualValue = circularProgressBar.EndAngle;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(1,50)]
    [InlineData(1,250)]
    [InlineData(0,-40)]
    [InlineData(0, -850)]
    [InlineData(0,0)]
    public void ProgressRadiusFactor_GetAndSet_UsingDouble(double expectedValue,double setValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        circularProgressBar.ProgressRadiusFactor = setValue;
        double actualValue = circularProgressBar.ProgressRadiusFactor;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ProgressThickness_GetAndSet_UsingDouble(double expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        circularProgressBar.ProgressThickness = expectedValue;
        double actualValue = circularProgressBar.ProgressThickness;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(1, 50)]
    [InlineData(1, 250)]
    [InlineData(0, -40)]
    [InlineData(0, -850)]
    [InlineData(0, 0)]
    public void TrackRadiusFactor_GetAndSet_UsingDouble(double expectedValue,double setValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        circularProgressBar.TrackRadiusFactor = setValue;
        double actualValue = circularProgressBar.TrackRadiusFactor;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void TrackThickness_GetAndSet_UsingDouble(double expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        circularProgressBar.TrackThickness = expectedValue;
        double actualValue = circularProgressBar.TrackThickness;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(CornerStyle.BothFlat)]
    [InlineData(CornerStyle.BothCurve)]
    [InlineData(CornerStyle.EndCurve)]
    [InlineData(CornerStyle.StartCurve)]
    public void ProgressCornerStyle_GetAndSet_UsingCornerStyle(CornerStyle expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        circularProgressBar.ProgressCornerStyle = expectedValue;
        CornerStyle actualValue = circularProgressBar.ProgressCornerStyle;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(CornerStyle.BothFlat)]
    [InlineData(CornerStyle.BothCurve)]
    [InlineData(CornerStyle.EndCurve)]
    [InlineData(CornerStyle.StartCurve)]
    public void TrackCornerStyle_GetAndSet_UsingCornerStyle(CornerStyle expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        circularProgressBar.TrackCornerStyle = expectedValue;
        CornerStyle actualValue = circularProgressBar.TrackCornerStyle;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(SizeUnit.Factor)]
    [InlineData(SizeUnit.Pixel)]
    public void ThicknessUnit_GetAndSet_UsingSizeUnit(SizeUnit expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        circularProgressBar.ThicknessUnit = expectedValue;
        SizeUnit actualValue = circularProgressBar.ThicknessUnit;

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void Content_GetAndSet_UsingViews()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
        Label expectedValue = new Label() { Text = "NewLabel" };

        circularProgressBar.Content = expectedValue;
        Label? actualValue = circularProgressBar.Content as Label;

        Assert.Equal(expectedValue.Text, actualValue?.Text);
    }


    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void StartAngleWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };

        circularProgressBar.StartAngle = expectedValue;
        double actualValue = circularProgressBar.StartAngle;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void EndAngleWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };

        circularProgressBar.EndAngle = expectedValue;
        double actualValue = circularProgressBar.EndAngle;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(1, 50)]
    [InlineData(1, 250)]
    [InlineData(0, -40)]
    [InlineData(0, -850)]
    [InlineData(0, 0)]
    public void ProgressRadiusFactorWithSize_GetAndSet_UsingDouble(double expectedValue, double setValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };

        circularProgressBar.ProgressRadiusFactor = setValue;
        double actualValue = circularProgressBar.ProgressRadiusFactor;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ProgressThicknessWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };

        circularProgressBar.ProgressThickness = expectedValue;
        double actualValue = circularProgressBar.ProgressThickness;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(1, 50)]
    [InlineData(1, 250)]
    [InlineData(0, -40)]
    [InlineData(0, -850)]
    [InlineData(0, 0)]
    public void TrackRadiusFactorWithSize_GetAndSet_UsingDouble(double expectedValue, double setValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };

        circularProgressBar.TrackRadiusFactor = setValue;
        double actualValue = circularProgressBar.TrackRadiusFactor;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void TrackThicknessWithSize_GetAndSet_UsingDouble(double expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };

        circularProgressBar.TrackThickness = expectedValue;
        double actualValue = circularProgressBar.TrackThickness;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(CornerStyle.BothFlat)]
    [InlineData(CornerStyle.BothCurve)]
    [InlineData(CornerStyle.EndCurve)]
    [InlineData(CornerStyle.StartCurve)]
    public void ProgressCornerStyleWithSize_GetAndSet_UsingCornerStyle(CornerStyle expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };

        circularProgressBar.ProgressCornerStyle = expectedValue;
        CornerStyle actualValue = circularProgressBar.ProgressCornerStyle;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(CornerStyle.BothFlat)]
    [InlineData(CornerStyle.BothCurve)]
    [InlineData(CornerStyle.EndCurve)]
    [InlineData(CornerStyle.StartCurve)]
    public void TrackCornerStyleWithSize_GetAndSet_UsingCornerStyle(CornerStyle expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };

        circularProgressBar.TrackCornerStyle = expectedValue;
        CornerStyle actualValue = circularProgressBar.TrackCornerStyle;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(SizeUnit.Factor)]
    [InlineData(SizeUnit.Pixel)]
    public void ThicknessUnitWithSize_GetAndSet_UsingSizeUnit(SizeUnit expectedValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };

        circularProgressBar.ThicknessUnit = expectedValue;
        SizeUnit actualValue = circularProgressBar.ThicknessUnit;

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ContentWithSize_GetAndSet_UsingViews()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(100, 100) };
        Label expectedValue = new Label() { Text = "NewLabel" };

        circularProgressBar.Content = expectedValue;
        Label? actualValue = circularProgressBar.Content as Label;

        Assert.Equal(expectedValue.Text, actualValue?.Text);

    }

    #endregion

    #region Internal Properties

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ArcInfo_StartAngle_GetAndSet_UsingFloat(float expectedValue)
    {
        CircularProgressBarArcInfo circularProgressBarArcInfo = new CircularProgressBarArcInfo();
        
        circularProgressBarArcInfo.StartAngle = expectedValue;
        float actualValue = circularProgressBarArcInfo.StartAngle;
        
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ArcInfo_EndAngle_GetAndSet_UsingFloat(float expectedValue)
    {
        CircularProgressBarArcInfo circularProgressBarArcInfo = new CircularProgressBarArcInfo();

        circularProgressBarArcInfo.EndAngle = expectedValue;
        float actualValue = circularProgressBarArcInfo.EndAngle;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ArcInfo_InnerStartRadius_GetAndSet_UsingDouble(double expectedValue)
    {
        CircularProgressBarArcInfo circularProgressBarArcInfo = new CircularProgressBarArcInfo();

        circularProgressBarArcInfo.InnerStartRadius = expectedValue;
        double actualValue = circularProgressBarArcInfo.InnerStartRadius;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50)]
    [InlineData(250)]
    [InlineData(-40)]
    [InlineData(-850)]
    [InlineData(0)]
    public void ArcInfo_InnerEndRadius_GetAndSet_UsingDouble(double expectedValue)
    {
        CircularProgressBarArcInfo circularProgressBarArcInfo = new CircularProgressBarArcInfo();

        circularProgressBarArcInfo.InnerEndRadius = expectedValue;
        double actualValue = circularProgressBarArcInfo.InnerEndRadius;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50,20)]
    [InlineData(-250,130)]
    [InlineData(-40,-60)]
    [InlineData(32,609)]
    [InlineData(0,0)]
    public void ArcInfo_TopLeft_GetAndSet_UsingPointF(float expectedValueX,float expectedValueY)
    {
        CircularProgressBarArcInfo circularProgressBarArcInfo = new CircularProgressBarArcInfo();
        PointF expectedValue = new PointF(expectedValueX,expectedValueY);
        circularProgressBarArcInfo.TopLeft = expectedValue;
        PointF actualValue = circularProgressBarArcInfo.TopLeft;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50, 20)]
    [InlineData(-250, 130)]
    [InlineData(-40, -60)]
    [InlineData(32, 609)]
    [InlineData(0, 0)]
    public void ArcInfo_BottomRight_GetAndSet_UsingPointF(float expectedValueX, float expectedValueY)
    {
        CircularProgressBarArcInfo circularProgressBarArcInfo = new CircularProgressBarArcInfo();
        PointF expectedValue = new PointF(expectedValueX, expectedValueY);
        circularProgressBarArcInfo.BottomRight = expectedValue;
        PointF actualValue = circularProgressBarArcInfo.BottomRight;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(50, 20)]
    [InlineData(-250, 130)]
    [InlineData(-40, -60)]
    [InlineData(32, 609)]
    [InlineData(0, 0)]
    public void ArcInfo_ArcPath_GetAndSet_UsingPathF(float expectedValueX, float expectedValueY)
    {
        CircularProgressBarArcInfo circularProgressBarArcInfo = new CircularProgressBarArcInfo();
        PathF expectedValue = new PathF(expectedValueX, expectedValueY);
        circularProgressBarArcInfo.ArcPath = expectedValue;
        PathF actualValue = circularProgressBarArcInfo.ArcPath;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(255, 0, 0)]
    [InlineData(0, 255, 0)]
    [InlineData(0, 0, 255)]
    [InlineData(255, 255, 0)]
    [InlineData(0, 255, 255)]
    public void ArcInfo_FillPaint_GetAndSet_UsingPaint(byte red, byte green, byte blue)
    {
        CircularProgressBarArcInfo circularProgressBarArcInfo = new CircularProgressBarArcInfo();

        Brush expectedValue = Color.FromRgb(red, green, blue);
        circularProgressBarArcInfo.FillPaint = expectedValue;
        Brush actualValue = circularProgressBarArcInfo.FillPaint;

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(255, 0, 0)]
    [InlineData(0, 255, 0)]
    [InlineData(0, 0, 255)]
    [InlineData(255, 255, 0)]
    [InlineData(0, 255, 255)]
    public void CircularProgressBarBackground_GetAndSet_UsingColor(byte red, byte green, byte blue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        Color expectedValue = Color.FromRgb(red, green, blue);
        circularProgressBar.CircularProgressBarBackground = expectedValue;
        Brush actualValue = circularProgressBar.CircularProgressBarBackground;

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
        InvokePrivateMethod(circularProgressBar, "OnProgressAnimationFinished",70,false);

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
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { Progress = 20 };

        circularProgressBar.SetProgress(70, 0);

        Assert.Equal(70, circularProgressBar.Progress);
    }

    [Fact]
    public void GetAvailableSize_SetsAvailableSize_WhenCalled()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        Assert.Equal(Size.Zero, circularProgressBar.AvailableSize);
        InvokePrivateMethod(circularProgressBar, "GetAvailableSize", 300, 300);

        Assert.NotEqual(Size.Zero,circularProgressBar.AvailableSize);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(275)]
    [InlineData(359.99)]
    [InlineData(0)]
    public void GetCenter_ReturnsPointF_WhenCalled(double sweepAngle)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        SetPrivateField(circularProgressBar, "_actualSweepAngle", sweepAngle);
        var actualValue = InvokePrivateMethod(circularProgressBar, "GetCenter", 320);

        Assert.Equal(PointF.Zero, actualValue);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(275)]
    [InlineData(359.99)]
    [InlineData(0)]
    public void GetCenter_WithStartEndAngle_ReturnsPointF_WhenCalled(double sweepAngle)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        SetPrivateField(circularProgressBar, "_actualSweepAngle", sweepAngle);
        SetPrivateField(circularProgressBar, "_actualStartAngle", 280);
        SetPrivateField(circularProgressBar, "_actualEndAngle", 120);
        var actualValue = InvokePrivateMethod(circularProgressBar, "GetCenter", 320);

        Assert.Equal(PointF.Zero, actualValue);
    }

    [Fact]
    public void OnIndeterminateAnimationUpdate_SetValue_WhenCalled()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        InvokePrivateMethod(circularProgressBar, "OnIndeterminateAnimationUpdate", 120);
        double actualValue = Convert.ToDouble(GetPrivateField(circularProgressBar, "_animationStart"));

        Assert.Equal(120, actualValue);
    }

    [Theory]
    [InlineData(100, 100, 57)]
    [InlineData(50.008728, 50, 270)]
    [InlineData(50, 150, 0)]
    [InlineData(50, 150, 180)]
    [InlineData(50, 150, 360)]
    [InlineData(50.008728, 50, 90)]
    public void CalculateActualCenter_Case1_ReturnsPoint_WhenCalled(float expectedX, float expectedY, int region)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        PointF expectedValue = new PointF(expectedX, expectedY);
        var actualValue = InvokePrivateMethod(circularProgressBar, "CalculateActualCenter", new Point(50,50),new List<int>() { region }, 100);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void CalculateActualCenter_Case2_ReturnsPoint_WhenCalled()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
        var actualValue = InvokePrivateMethod(circularProgressBar, "CalculateActualCenter", new Point(50, 50), new List<int>() { 57, 42 }, 100);

        Assert.Equal(new PointF(50, 50), actualValue);
    }

    [Theory]
    [InlineData(78)]
    [InlineData(270)]
    [InlineData(0)]
    [InlineData(180)]
    [InlineData(360)]
    [InlineData(90)]
    public void CalculateActualCenter_Case3_ReturnsPoint_WhenCalled(int region3)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();

        PointF expectedValue = new PointF(50, 50);
        var actualValue = InvokePrivateMethod(circularProgressBar, "CalculateActualCenter", new Point(50, 50), new List<int>() { 57, 42, region3 }, 100);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void CalculateActualCenter_Case0_ReturnsPoint_WhenCalled()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
        var actualValue = InvokePrivateMethod(circularProgressBar, "CalculateActualCenter", new Point(50, 20), new List<int>(), 100);

        Assert.Equal(new PointF((float)50.008728, 10), actualValue);
    }

    [Fact]
    public void CalculateCapCenter_ReturnsPoint_WhenCalled()
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
        var actualValue = InvokePrivateMethod(circularProgressBar, "CalculateCapCenter", 25, 250);

        Assert.Equal(new Point(226.5769467591625, 105.65456543517486), actualValue);
    }

    [Theory]
    [InlineData(25, 25,0,100)]
    [InlineData(0, 25, 50, 50)]
    [InlineData(100, 100, 0, 100)]
    public void CalculateProgressPercentage_ReturnsDouble(double expectedValue, double indicatorValue, double minimumValue, double maximumValue)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar();
        double actualValue = Convert.ToDouble(InvokePrivateMethod(circularProgressBar, "CalculateProgressPercentage", indicatorValue, minimumValue, maximumValue));

        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData(100, 50)]
    [InlineData(50, 100)]
    [InlineData(100, 100)]
    public void CalculateRadius_SetValue_WhenCalled(double width, double height)
    {
        SfCircularProgressBar circularProgressBar = new SfCircularProgressBar() { AvailableSize = new Size(width, height) };

        InvokePrivateMethod(circularProgressBar, "CalculateRadius");
        double actualValue = Convert.ToDouble(GetPrivateField(circularProgressBar, "_radius"));

        Assert.NotEqual(0, actualValue);
    }

    #endregion
}