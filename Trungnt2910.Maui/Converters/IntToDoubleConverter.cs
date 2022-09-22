using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Controls.Internals;
using System.Globalization;

namespace Trungnt2910.Maui.Converters;

/// <summary>
/// Converts an <see langword="int"/> to a <see langword="double"/>.
/// </summary>
[Preserve]
public class IntToDoubleConverter : ICommunityToolkitValueConverter
{
    /// <inheritdoc/>
    public object? DefaultConvertReturnValue => 0;

    /// <inheritdoc/>
    public object? DefaultConvertBackReturnValue => 0;

    /// <inheritdoc/>
    public Type FromType => typeof(int);
    
    /// <inheritdoc/>
    public Type ToType => typeof(double);

    /// <inheritdoc/>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (targetType != typeof(double))
        {
            throw new ArgumentOutOfRangeException(nameof(targetType));
        }
        
        if (value is int i)
        {
            return (double)i;
        }

        throw new ArgumentOutOfRangeException(nameof(value));
    }

    /// <inheritdoc/>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (targetType != typeof(int))
        {
            throw new ArgumentOutOfRangeException(nameof(targetType));
        }

        if (value is double d)
        {
            return (int)d;
        }

        throw new ArgumentOutOfRangeException(nameof(value));
    }
}
