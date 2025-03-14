using Microsoft.Maui.Controls.Internals;

/// <summary>
/// Helper class for creating bindings in .NET MAUI applications.
/// </summary>
public static class BindingHelper
    {
		/// <summary>
		/// Creates a typed binding for a given source and its properties.
		/// </summary>
		/// <typeparam name="TSource">The type of the binding source.</typeparam>
		/// <typeparam name="TProperty">The type of the property to bind.</typeparam>
		/// <param name="propertyName">The name of the property to bind to.</param>
		/// <param name="getter">A function to get the value of the property from the source.</param>
		/// <param name="setter">An optional action to set the value of the property on the source.</param>
		/// <param name="mode">The binding mode that determines the data flow direction.</param>
		/// <param name="converter">An optional value converter for the binding.</param>
		/// <param name="converterParameter">An optional parameter used by the converter.</param>
		/// <param name="source">The source object for binding.</param>
		/// <returns>A <see cref="BindingBase"/> representing the created binding.</returns>
		public static BindingBase CreateBinding<TSource, TProperty>(
                string propertyName,
                Func<TSource, TProperty> getter,
                Action<TSource, TProperty>? setter = null,
                BindingMode mode = BindingMode.Default,
                IValueConverter? converter = null,
                object? converterParameter = null,
                object? source = null)
        {
            return new TypedBinding<TSource, TProperty>(
                getter: source => (getter(source), true),
                setter,
                handlers:
                [
                        new(static source => source, propertyName),
                ])
            {
                Converter = converter,
                ConverterParameter = converterParameter,
                Mode = mode,
                Source = source,
            };
        }
    }