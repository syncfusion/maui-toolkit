using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using Font = Microsoft.Maui.Font;

namespace Syncfusion.Maui.Toolkit.Graphics.Internals
{
    /// <summary>
    /// Provides static methods and bindable properties for managing font-related attributes in text elements.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class FontElement
    {
        /// <summary>
        /// Identifies the <see cref="ITextElement.Font"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FontProperty =
            BindableProperty.Create(nameof(ITextElement.Font), typeof(Font), typeof(ITextElement), default(Font),
                                    propertyChanged: OnFontPropertyChanged);
        /// <summary>
        /// Identifies the <see cref="ITextElement.FontFamily"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(ITextElement.FontFamily), typeof(string), typeof(ITextElement), default(string),
                                    propertyChanged: OnFontFamilyChanged);
        /// <summary>
        /// Identifies the <see cref="ITextElement.FontSize"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create(nameof(ITextElement.FontSize), typeof(double), typeof(ITextElement), -1d,
                                    propertyChanged: OnFontSizeChanged,
                                    defaultValueCreator: FontSizeDefaultValueCreator);

        /// <summary>
        /// Identifies the <see cref="ITextElement.FontAttributes"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create(nameof(ITextElement.FontAttributes), typeof(FontAttributes), typeof(ITextElement), FontAttributes.None,
                                    propertyChanged: OnFontAttributesChanged);

        /// <summary>
        /// Identifies the <see cref="ITextElement.FontAutoScalingEnabled"/> bindable property.
        /// </summary>
        public static readonly BindableProperty FontAutoScalingEnabledProperty =
            BindableProperty.Create(nameof(ITextElement.FontAutoScalingEnabled), typeof(bool), typeof(ITextElement), false,
                                    propertyChanged: OnFontAutoScalingEnabledChanged);

        /// <summary>
        /// Bindable property for CanceEvents.
        /// </summary>
        static readonly BindableProperty CancelEventsProperty =
            BindableProperty.Create("CancelEvents", typeof(bool), typeof(FontElement), false);

        /// <summary>
        /// Gets the value of CancelEvents for the specified BindableObject.
        /// </summary>
        /// <param name="bindable">The BindableObject to get the value from.</param>
        /// <returns>The boolean value of CancelEvents.</returns>
        static bool GetCancelEvents(BindableObject bindable) => (bool)bindable.GetValue(CancelEventsProperty);

        /// <summary>
        /// Sets the value of CancelEvents for the specified BindableObject.
        /// </summary>
        /// <param name="bindable">The BindableObject to set the value on.</param>
        /// <param name="value">The boolean value to set.</param>
        static void SetCancelEvents(BindableObject bindable, bool value)
        {
            bindable.SetValue(CancelEventsProperty, value);
        }

        /// <summary>
        /// Handles changes to the <see cref="ITextElement.Font"/> property.
        /// </summary>
        /// <param name="bindable">The BindableObject that changed.</param>
        /// <param name="oldValue">The old value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        static void OnFontPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (GetCancelEvents(bindable))
                return;

            SetCancelEvents(bindable, true);

            if (newValue != null)
            {
                var font = (Font)newValue;
                if (font == Font.Default)
                {
                    bindable.ClearValue(FontFamilyProperty);
                    bindable.ClearValue(FontSizeProperty);
                    bindable.ClearValue(FontAttributesProperty);
                }
                else
                {
                    bindable.SetValue(FontFamilyProperty, font.Family);
                    bindable.SetValue(FontSizeProperty, font.Size);
                    bindable.SetValue(FontAttributesProperty, font.GetFontAttributes());
                }
            }

            SetCancelEvents(bindable, false);
        }

        /// <summary>
        /// Handles changes to font-related properties.
        /// </summary>
        /// <param name="bindable">The BindableObject that changed.</param>
        /// <returns>True if the change was processed, false otherwise.</returns>
        static bool OnChanged(BindableObject bindable)
        {
            if (GetCancelEvents(bindable))
                return false;

            ITextElement fontElement = (ITextElement)bindable;

            SetCancelEvents(bindable, true);
            bindable.SetValue(FontProperty, Font.OfSize(fontElement.FontFamily, fontElement.FontSize, enableScaling: fontElement.FontAutoScalingEnabled).WithAttributes(fontElement.FontAttributes));

            SetCancelEvents(bindable, false);
            return true;
        }

        /// <summary>
        /// Handles changes to the <see cref="ITextElement.FontFamily"/> property.
        /// </summary>
        /// <param name="bindable">The BindableObject that changed.</param>
        /// <param name="oldValue">The old value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable == null || !OnChanged(bindable))
                return;

            ((ITextElement)bindable).OnFontFamilyChanged((string)oldValue, (string)newValue);
        }

        /// <summary>
        /// Handles changes to the <see cref="ITextElement.FontSize"/> property.
        /// </summary>
        /// <param name="bindable">The BindableObject that changed.</param>
        /// <param name="oldValue">The old value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        static void OnFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable == null || !OnChanged(bindable))
                return;

            ((ITextElement)bindable).OnFontSizeChanged((double)oldValue, (double)newValue);
        }

        /// <summary>
        /// Creates the default value for the FontSize property.
        /// </summary>
        /// <param name="bindable">The BindableObject to create the default value for.</param>
        /// <returns>The default FontSize value.</returns>
        static object FontSizeDefaultValueCreator(BindableObject bindable)
        {
            return ((ITextElement)bindable).FontSizeDefaultValueCreator();
        }

        /// <summary>
        /// Handles changes to the <see cref="ITextElement.FontAttributes"/> property.
        /// </summary>
        /// <param name="bindable">The BindableObject that changed.</param>
        /// <param name="oldValue">The old value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
        static void OnFontAttributesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable == null || !OnChanged(bindable))
                return;

            ((ITextElement)bindable).OnFontAttributesChanged((FontAttributes)oldValue, (FontAttributes)newValue);
        }

        /// <summary>
        /// Handles changes to the <see cref="ITextElement.FontAutoScalingEnabled"/> property.
        /// </summary>
        /// <param name="bindable">The BindableObject that changed.</param>
        /// <param name="oldValue">The old value of the property.</param>
        /// <param name="newValue">The new value of the property.</param>
		static void OnFontAutoScalingEnabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable == null || !OnChanged(bindable))
                return;

            ((ITextElement)bindable).OnFontAutoScalingEnabledChanged((bool)oldValue, (bool)newValue);
        }

    }
}
