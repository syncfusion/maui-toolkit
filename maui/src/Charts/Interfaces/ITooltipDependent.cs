namespace Syncfusion.Maui.Toolkit.Charts
{
	internal interface ITooltipDependent
	{
		#region Properties

		DataTemplate TooltipTemplate { get; set; }

		bool EnableTooltip { get; set; }

		#endregion

		#region Methods

		void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect chartBounds);

		DataTemplate? GetDefaultTooltipTemplate(TooltipInfo info)
		{
			return ChartUtils.GetDefaultTooltipTemplate(info);
		}

		#endregion
	}
}
