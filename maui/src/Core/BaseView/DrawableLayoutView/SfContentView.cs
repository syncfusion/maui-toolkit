namespace Syncfusion.Maui.Toolkit
{

	/// <summary>
	/// Represents an abstract base class for views that contain a single piece of content.
	/// </summary>
	[ContentProperty(nameof(Content))]
	public abstract class SfContentView : SfView
	{

		/// <summary>
		/// Identifies the <see cref="Content"/> bindable property.
		/// </summary>
		/// <value>
		/// The identifier for <see cref="Content"/> bindable property.
		/// </value>
		public static readonly BindableProperty ContentProperty =
			BindableProperty.Create(nameof(Content), typeof(View), typeof(SfContentView), null, BindingMode.OneWay, null, OnContentPropertyChanged);

		/// <summary>
		/// Gets or sets the content of the view.
		/// </summary>
		public View? Content
		{
			get { return (View)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}

		/// <summary>
		/// Invoked whenever the <see cref="ContentProperty"/> is set for SfContentView.
		/// </summary>
		/// <param name="bindable">The bindable.</param>
		/// <param name="oldValue">The old value.</param>
		/// <param name="newValue">The new value.</param>
		private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SfContentView contentView)
			{
				contentView?.OnContentChanged(oldValue, newValue);
			}
		}

		/// <exclude/>
		protected virtual void OnContentChanged(object oldValue, object newValue)
		{
			if (oldValue != null && oldValue is View oldView)
			{
				Remove(oldView);
			}
			if (newValue != null && newValue is View newView)
			{
				Add(newView);
			}
		}
	}
}