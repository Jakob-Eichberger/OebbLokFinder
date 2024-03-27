using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OebbLokFinder.View.Converter
{
    public class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool negation ? !negation : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value is bool negation ? !negation : null;
    }
}
