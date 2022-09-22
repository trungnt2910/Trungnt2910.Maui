using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Controls.Internals;
using System.Globalization;

namespace Trungnt2910.Maui.Converters;

/// <summary>
/// Converts a number (<see langword="int"/>, <see langword="double"/>,...)
/// to a <see cref="Thickness"/>.
/// </summary>
[Preserve]
public class NumberToThicknessConverter : ICommunityToolkitValueConverter
{
    /// <summary>
    /// The options used for this converter.
    /// </summary>
    public NumberToThicknessOptions ConverterOptions { get; set; } = NumberToThicknessOptions.All;

    /// <inheritdoc/>
    public object? DefaultConvertReturnValue => Thickness.Zero;
    
    /// <inheritdoc/>
    public object? DefaultConvertBackReturnValue => 0;

    /// <inheritdoc/>
    public Type FromType => typeof(object);

    /// <inheritdoc/>
    public Type ToType => typeof(Thickness);

    /// <summary>
    /// To be added
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (targetType != typeof(Thickness))
        {
            throw new ArgumentOutOfRangeException(nameof(targetType));
        }
        double numericValue = double.Parse(value?.ToString() ?? string.Empty);
        var resultThickness = new Thickness();
        if (ConverterOptions.HasFlag(NumberToThicknessOptions.Left))
        {
            resultThickness.Left = numericValue;
        }
        if (ConverterOptions.HasFlag(NumberToThicknessOptions.Right))
        {
            resultThickness.Right = numericValue;
        }
        if (ConverterOptions.HasFlag(NumberToThicknessOptions.Top))
        {
            resultThickness.Top = numericValue;
        }
        if (ConverterOptions.HasFlag(NumberToThicknessOptions.Bottom))
        {
            resultThickness.Bottom = numericValue;
        }
        return resultThickness;
    }

    /// <summary>
    /// To be added
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Enumerates the available options for the conversion between a number and a <see cref="Thickness"/>
/// by <see cref="NumberToThicknessConverter"/>
/// </summary>
[Flags]
public enum NumberToThicknessOptions
{
    /// <summary>
    /// Sets the <see cref="Thickness.Left"/> property of the resulting <see cref="Thickness"/>.
    /// Other properties remain 0.
    /// </summary>
    Left = 1,
    /// <summary>
    /// Sets the <see cref="Thickness.Top"/> property of the resulting <see cref="Thickness"/>.
    /// Other properties remain 0.
    /// </summary>
    Top = 2,
    /// <summary>
    /// Sets the <see cref="Thickness.Right"/> property of the resulting <see cref="Thickness"/>.
    /// Other properties remain 0.
    /// </summary>
    Right = 4,
    /// <summary>
    /// Sets the <see cref="Thickness.Bottom"/> property of the resulting <see cref="Thickness"/>.
    /// Other properties remain 0.
    /// </summary>
    Bottom = 8,
    /// <summary>
    /// Sets the <see cref="Thickness.Left"/> and <see cref="Thickness.Right"/> property of the resulting <see cref="Thickness"/>.
    /// Other properties remain 0.
    /// </summary>
    LeftRight = Left | Right,
    /// <summary>
    /// Sets the <see cref="Thickness.Top"/> and <see cref="Thickness.Bottom"/> property of the resulting <see cref="Thickness"/>.
    /// Other properties remain 0.
    /// </summary>
    TopBottom = Top | Bottom,
    /// <summary>
    /// Sets all properties of the resulting <see cref="Thickness"/>.
    /// </summary>
    All = Left | Top | Right | Bottom
}