using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OebbLokFinder.Converter
{
    /// <summary>
    /// Converts a number to a Percentage Value.
    /// </summary>
    /// <remarks>
    /// If the number is decimal, float or double it will be multiplied by 100.</remarks>
    internal class NumberToPercentageConverter : IValueConverter
    {

        /// <summary>
        /// Converts a number to a Percentage Value.
        /// </summary>
        /// <remarks>
        /// If the number is decimal, float or double it will be multiplied by 100.</remarks>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value?.ToString(), out var valueDE))
            {
                return $"{valueDE * 100:0.##}%";
            }
            else if (float.TryParse(value?.ToString(), out var valueF))
            {
                return $"{valueF * 100:0.##}%";
            }
            else if (double.TryParse(value?.ToString(), out var valueD))
            {
                return $"{valueD * 100:0.##}%";
            }
            else if (int.TryParse(value?.ToString(), out var valueI))
            {
                return $"{valueI}%";
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Not implemented!!!
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
