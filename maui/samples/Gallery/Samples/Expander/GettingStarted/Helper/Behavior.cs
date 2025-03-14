namespace Syncfusion.Maui.ControlsGallery.Expander;

	internal class SampleViewBehavior : Behavior<SampleView>
	{
		#region Fields

		private ScrollView? scrollView;

		private Grid? Layout;

		#endregion

		#region Overrides
		protected override void OnAttachedTo(SampleView bindable)
		{
			this.Layout = bindable.FindByName<Grid>("layout");
			this.scrollView = bindable.FindByName<ScrollView>("scrollView");
#if __MACCATALYST__
			this.Layout!.SizeChanged += this.StackLayout_SizeChanged;
#endif
			base.OnAttachedTo(bindable);
		}

		private void StackLayout_SizeChanged(object? sender, EventArgs e)
		{
			// ScrollView does not scroll unless window is resized.
			// Measure scroll view to update scroll view height when children's height changes dynamically.
			(this.scrollView as IView)!.InvalidateMeasure();
		}

		protected override void OnDetachingFrom(BindableObject bindable)
		{
#if __MACCATALYST__
			this.Layout!.SizeChanged -= this.StackLayout_SizeChanged;
#endif
			this.Layout = null;
			this.scrollView = null;
			base.OnDetachingFrom(bindable);
		}

		#endregion
	}

