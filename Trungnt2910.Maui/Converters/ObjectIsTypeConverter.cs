using Microsoft.Maui.Controls.Internals;
using System.Globalization;

namespace Trungnt2910.Maui.Converters;

/// <summary>
/// Converts a source object to <see cref="TrueObject"/> if it can be
/// converted to <typeparamref name="TType"/>, else returns <see cref="FalseObject"/>.
/// </summary>
/// <typeparam name="TType">The type to check for.</typeparam>
/// <typeparam name="TResultObject">The type of the return value.</typeparam>
[Preserve]
public class ObjectIsTypeConverter<TType, TResultObject> : IValueConverter
{
    /// <summary>
    /// The object to return if the check succeeds.
    /// </summary>
    public TResultObject? TrueObject { get; set; }
    /// <summary>
    /// The object to return if the check fails.
    /// </summary>
    public TResultObject? FalseObject { get; set; }

    /// <summary>
    /// To be added.
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return (value is TType) ? TrueObject : FalseObject;
    }

    /// <summary>
    /// To be added.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
