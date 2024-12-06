namespace Syncfusion.Maui.Toolkit.Charts
{
	/// <summary>
	/// Represents a class that handles touch interactions within the chart area.
	/// The <see cref="ChartInteractiveBehavior"/> class provides methods that respond to various touch events, allowing users to customize behavior for actions such as <b>OnTouchDown</b>, <b>OnTouchMove</b>, and <b>OnTouchUp</b>.
	/// </summary>
	/// <remarks>
	/// <para><b>OnTouchDown() - </b>gets called when the user makes the initial contact of a user's finger or touch input device with the Chart Area. </para>
	/// <para><b>OnTouchMove() - </b>gets called when a user's finger or touch input device is in contact with the Chart area and moves across its surface. </para>
	/// <para><b>OnTouchUp() - </b>gets called when a user lifts their finger or releases their touch input from the Chart area. </para>
	/// <para>To utilize these methods, derive a new class from <see cref="ChartInteractiveBehavior"/>.</para>
	/// <para>Then, create the instance for that class, and it must be added in the chart's <see cref="ChartBase.InteractiveBehavior"/> as per the following code sample. </para>
	/// </remarks>
	/// <example>
	/// # [ChartInteractionExt.cs](#tab/tabid-1)
	/// <code><![CDATA[
	/// public class ChartInteractionExt : ChartInteractiveBehavior
	/// {
	///    <!--omitted for brevity-->
	/// }
	/// ]]>
	/// </code>
	/// # [MainPage.xaml](#tab/tabid-2)
	/// <code><![CDATA[
	/// <chart:SfCartesianChart>
	///
	///     <!--omitted for brevity-->
	///
	///     <chart:SfCartesianChart.InteractiveBehavior>
	///          <local:ChartInteractionExt/>
	///     </chart:SfCartesianChart.InteractiveBehavior>
	/// 
	/// </chart:SfCartesianChart>
	///
	/// ]]>
	/// </code>
	/// # [MainPage.xaml.cs](#tab/tabid-3)
	/// <code><![CDATA[
	/// SfCartesianChart chart = new SfCartesianChart();
	/// 
	/// ChartInteractionExt interaction = new ChartInteractionExt();
	/// chart.ChartInteractiveBehavior = interaction;
	/// ]]>
	/// </code>
	/// *** 
	///
	/// </example>
	public partial class ChartInteractiveBehavior : ChartBehavior
	{
	}
}