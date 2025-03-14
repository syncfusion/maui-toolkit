namespace Syncfusion.Maui.Toolkit.Expander
{
    /// <summary>
    /// Holds the extension methods.
    /// </summary>
    internal static class Extensions
    {
		#region Internal Methods

		/// <summary>
		/// Remove the specific child from view.
		/// </summary>
		/// <param name="element">Parent element.</param>
		/// <param name="childview">Child element to be removed.</param>
		internal static void RemoveChildrenInView(this SfView element, View childview)
        {
            if (childview != null)
            {
                element.Children.Remove(childview);
            }
        }

        /// <summary>
        /// Remove the child at specific index.
        /// </summary>
        /// <param name="element">Parent element.</param>
        /// <param name="index">Index of child element.</param>
        internal static void RemoveChildAtIndex(this SfView element, int index)
        {
            element.Children.RemoveAt(index);
        }

        /// <summary>
        /// Remove the all childrens of a view.
        /// </summary>
        /// <param name="element">Parent element.</param>
        internal static void RemoveChildrens(this View element)
        {
			if (element is SfView view)
			{
				view.Children.Clear();
			}
        }

        /// <summary>
        /// Add the child element.
        /// </summary>
        /// <param name="element">Parent element.</param>
        /// <param name="view">Child element.</param>
        /// <param name="index">Index of child element to add.</param>
        /// <exception cref="InvalidOperationException">Exception data.</exception>
        internal static void AddChildView(this SfView element, View? view, int index = 0)
        {
            if (view == null || index < 0)
            {
                return;
            }

            int childindex = -1;
            try
            {
                childindex = element.Children.IndexOf(view);
                if (childindex == -1)
                {
                    element.Children.Insert(index, view);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(childindex.ToString() + ",Element:" + element.ToString() + ",Child:" + view.ToString() + ":" + e.ToString());
            }
        }

        /// <summary>
        /// Change the item visibility.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="isVisible">Value of Visibility.</param>
        internal static void SetItemVisibility(this View element, bool isVisible)
        {
            if (isVisible != element.IsVisible)
            {
                element.IsVisible = isVisible;
            }
        }

#if ANDROID
        /// <summary>
        /// This method returns whether the AnimatorDurationScale is disabled in the device settings.
        /// </summary>
        /// <returns> returns whether the AnimatorDurationScale value of the device is zero or not.</returns>
        internal static bool AnimatorDurationScaleIsOff()
        {
            var context = Android.App.Application.Context;
            var durationScale = Android.Provider.Settings.Global.GetFloat(context.ContentResolver, Android.Provider.Settings.Global.AnimatorDurationScale, 1f);
            return durationScale == 0f;
        }
#endif

		#endregion
	}

}
