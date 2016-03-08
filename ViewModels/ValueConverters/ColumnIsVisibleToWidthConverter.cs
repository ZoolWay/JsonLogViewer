using System;
using System.Globalization;
using System.Windows.Data;

namespace Zw.JsonLogViewer.ViewModels.ValueConverters
{
    public class ColumnIsVisibleToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) return Binding.DoNothing;

            var isVisible = (bool)value;
            return isVisible ? Double.NaN : 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
