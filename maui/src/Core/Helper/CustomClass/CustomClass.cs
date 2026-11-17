using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Toolkit.Helper
{
	internal partial class SfBorder : Border { }

	internal partial class SfGrid : Grid { }

	internal partial class SfStackLayout : StackLayout { }

	internal partial class SfHorizontalStackLayout : HorizontalStackLayout { }

	internal partial class SfScrollView : ScrollView { }

	internal partial class SfLabel : Label { }

	internal partial class SfImage : Image { }

	internal partial class SfVerticalStackLayout : VerticalStackLayout { }

	internal partial class SfAbsoluteLayout : AbsoluteLayout { }

	internal class SfHorizontalStackBase : SfStackBase
	{
		public SfHorizontalStackBase() => IsHorizontal = true;
	}

	internal partial class SfVerticalStackBase : SfStackBase
	{
		public SfVerticalStackBase() => IsHorizontal = false;
	}

	internal class SfStackBase : StackBase
	{
		internal bool IsHorizontal { get; set; } = true;
		protected override ILayoutManager CreateLayoutManager()
		{
			return IsHorizontal
				? (ILayoutManager)new HorizontalStackLayoutManager(this)
				: new VerticalStackLayoutManager(this);
		}
	}
}

