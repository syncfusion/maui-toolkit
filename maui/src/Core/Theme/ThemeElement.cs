using Microsoft.Maui.Controls.Internals;

namespace Syncfusion.Maui.Toolkit.Themes
{
	/// <summary>
	/// ThemeElement from which SubViews/Styles of element should be inherited.
	/// </summary>
	internal static class ThemeElement
	{
		/// <summary>
		/// The common theme property to get syncfusion theme.
		/// </summary>
		public static BindableProperty CommonThemeProperty = BindableProperty.Create(
			"CommonTheme",
			returnType: typeof(string),
			declaringType: typeof(IThemeElement),
			defaultValue: "Default",
			propertyChanged: OnCommonThemePropertyChanged);

		/// <summary>
		/// The control theme property to get theme of the respective control.
		/// </summary>
		public static BindableProperty ControlThemeProperty = BindableProperty.Create(
			"ControlTheme",
			typeof(string),
			typeof(IThemeElement),
			defaultValue: "Default",
			propertyChanged: OnControlThemeChanged);

		/// <summary>
		/// The property used to cache resource dictionaries which are belonged to control.
		/// </summary>
		private static readonly Dictionary<string, WeakReference<ResourceDictionary>> ControlThemeCache = [];

		/// <summary>
		/// The style target dictionaries.
		/// </summary>
		private static readonly List<WeakReference<ResourceDictionary>> StyleTargetDictionaries = [];

		/// <summary>
		/// The control key property.
		/// </summary>
		private static readonly BindableProperty ControlKeyProperty = BindableProperty.Create(
			"ControlKey",
			typeof(string),
			typeof(IThemeElement),
			defaultValue: string.Empty);

		/// <summary>
		/// The implicit style property.
		/// </summary>
		private static readonly BindableProperty ImplicitStyleProperty = BindableProperty.Create(
			"ImplicitStyle",
			typeof(Style),
			typeof(IThemeElement),
			default(Style),
			propertyChanged: OnImplicitStyleChanged);

		/// <summary>
		/// The Dictionary contains pending dictionaries to which are to be merged.
		/// </summary>
		private static readonly Dictionary<ResourceDictionary, List<ResourceDictionary>> PendingDictionariesToMerge = [];

		///// <summary>
		///// Boolean property to check whether pending dictionaries are merged.
		///// </summary>
		//private static bool isScheduled = false;

		/// <summary>
		/// Holds element of setter object, to call apply method
		/// </summary>
#if NET8_0_OR_GREATER
		private static readonly object[] Elements = new object[2];
#else
        private static Object[] elements = new Object[1];
#endif

		/// <summary>
		///  Call this <see langword="static"/> method in the constructor for which implement IParentThemeElement and IThemeElement interfaces.
		/// </summary>
		/// <param name="element">Element.</param>
		/// <param name="controlKey">Control key.</param>
		internal static void InitializeThemeResources(Element element, string controlKey)
		{
			if (element == null)
			{
				throw new ArgumentNullException(nameof(element));
			}

			Type type = element.GetType();

			element.SetValue(ControlKeyProperty, controlKey);

			////It is mandatory to set following two keys in the constructor
			element.SetDynamicResource(ThemeElement.CommonThemeProperty, "SyncfusionTheme");

			////Here, you should name the key with following pattern: ControlName + Theme. For example, if it is SfChart control, it should be SfChartTheme
			element.SetDynamicResource(ThemeElement.ControlThemeProperty, controlKey);

			if (element as VisualElement == null)
			{
				string? key = type.FullName;
				element.SetDynamicResource(ImplicitStyleProperty, key);
			}
		}

		/// <summary>
		/// Adds the style dictionary.
		/// </summary>
		/// <param name="resourceDictionary">Resource dictionary.</param>
		internal static void AddStyleDictionary(ResourceDictionary resourceDictionary)
		{
			if (resourceDictionary == null)
			{
				throw new ArgumentNullException(nameof(resourceDictionary));
			}

			StyleTargetDictionaries.Add(new WeakReference<ResourceDictionary>(resourceDictionary));
		}

		/// <summary>
		/// Merges the pending dictionaries to existing resource dictionaries.
		/// </summary>
		private static void MergePendingDictionaries()
		{
			if (PendingDictionariesToMerge != null)
			{
				foreach (var pendingDictionaries in PendingDictionariesToMerge)
				{
					var targetDictionary = pendingDictionaries.Key;

					foreach (var themeDictionary in pendingDictionaries.Value)
					{
						targetDictionary.MergedDictionaries.Add(themeDictionary);
					}

					pendingDictionaries.Value.Clear();
				}

				PendingDictionariesToMerge.Clear();
			}

			// isScheduled = false;
		}

		/// <summary>
		/// Called when control theme was changed implicitily
		/// </summary>
		/// <param name="bindable">Bindable.</param>
		/// <param name="oldValue">Old value.</param>
		/// <param name="newValue">New value.</param>
		private static void OnImplicitStyleChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is Element element && newValue is Style style && !ApplyStyle(element, style))
			{
				foreach (Setter setter in style.Setters)
				{
					if (setter.Value is DynamicResource dynamicResource)
					{
						element.SetDynamicResource(setter.Property, dynamicResource.Key);
					}
					else
					{
						element.SetValue(setter.Property, setter.Value);
					}
				}
			}
		}

		/// <summary>
		/// Returns a boolean value indicating whether the Setter's Apply Method is invoked using Reflection or not.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="style"></param>
		/// <returns></returns>
		private static bool ApplyStyle(Element element, Style style)
		{
			var applyMethodInfo = typeof(Style).GetInterface("IStyle")?.GetMethod("Apply");

			if (applyMethodInfo != null)
			{
				Elements![0] = element;
				applyMethodInfo.Invoke(style, Elements);

				return true;
			}

			return false;
		}

		/// <summary>
		/// Implementation of common theme property changed.
		/// </summary>
		/// <param name="bindable">Bindable.</param>
		/// <param name="oldValue">Old value.</param>
		/// <param name="newValue">New value.</param>
		private static void OnCommonThemePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var themeElement = bindable as IThemeElement;

			themeElement?.OnCommonThemeChanged((string)oldValue, (string)newValue);
		}

		/// <summary>
		/// Merges the theme dictionary to pending dictionaries.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="themeDictionary">Theme dictionary.</param>
		private static void MergeThemeDictionary(string key, ResourceDictionary themeDictionary)
		{
			if (themeDictionary == null)
			{
				return;
			}

			int count = StyleTargetDictionaries.Count;

			for (int i = count - 1; i >= 0; i--)
			{
				var weakReference = StyleTargetDictionaries[i];
				weakReference.TryGetTarget(out ResourceDictionary? resourceDictionary);
				if (resourceDictionary != null)
				{
					if (resourceDictionary.TryGetValue(key, out _))
					{
						if (!resourceDictionary.MergedDictionaries.Contains(themeDictionary))
						{
							List<ResourceDictionary>? pendingDictionaries = null;

							if (!PendingDictionariesToMerge.ContainsKey(resourceDictionary))
							{
								pendingDictionaries = [];
								PendingDictionariesToMerge.Add(resourceDictionary, pendingDictionaries);
							}
							else
							{
								pendingDictionaries = PendingDictionariesToMerge[resourceDictionary];
							}

							pendingDictionaries.Add(themeDictionary);

							MergePendingDictionaries();

							//if (!isScheduled && Application.Current != null)
							//{
							//    // 533611 - Theming resource merged dictionary delay in updating the value in the view.
							//    // Application.Current.Dispatcher.Dispatch(MergePendingDictionaries);
							//     MergePendingDictionaries();
							//    //isScheduled = true;
							//}
						}
					}
				}
			}
		}

		/// <summary>
		/// Tries the get theme dictionary from Control.
		/// </summary>
		/// <returns><c>true</c>, if get theme dictionary was tryed, <c>false</c> otherwise.</returns>
		/// <param name="element">Element.</param>
		/// <param name="resourceDictionary">Resource dictionary.</param>
		private static bool TryGetThemeDictionary(VisualElement element, out ResourceDictionary? resourceDictionary)
		{
			resourceDictionary = null;
			if (element != null)
			{
				string key = (string)element.GetValue(ControlKeyProperty);

				if (ControlThemeCache.TryGetValue(key, out WeakReference<ResourceDictionary>? weakReference)
					&& weakReference.TryGetTarget(out resourceDictionary))
				{
					return true;
				}
				else if (ControlThemeCache.ContainsKey(key))
				{
					ControlThemeCache.Remove(key);
				}

				if (element is IParentThemeElement parentElement)
				{
					resourceDictionary = parentElement.GetThemeDictionary();

					if (resourceDictionary != null)
					{
						weakReference = new WeakReference<ResourceDictionary>(resourceDictionary);
						ControlThemeCache.Add(key, weakReference);
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Implementation of control theme changed.
		/// </summary>
		/// <param name="bindable">Bindable.</param>
		/// <param name="oldValue">Old value.</param>
		/// <param name="newValue">New value.</param>
		private static void OnControlThemeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			IThemeElement? themeElement = bindable as IThemeElement;
			if (bindable != null)
			{
				var controlKey = (string)bindable.GetValue(ControlKeyProperty);


				if (bindable is VisualElement visualElement && StyleTargetDictionaries?.Count > 0)
				{
					if (TryGetThemeDictionary(visualElement, out ResourceDictionary? themeDictionary) && themeDictionary != null)
					{
						MergeThemeDictionary(controlKey, themeDictionary);
					}
				}

				themeElement?.OnControlThemeChanged((string)oldValue, (string)newValue);
			}
		}
	}
}
