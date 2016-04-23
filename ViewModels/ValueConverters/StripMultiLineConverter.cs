using System;
using System.Globalization;
using System.Windows.Data;

namespace Zw.JsonLogViewer.ViewModels.ValueConverters
{
    public class StripMultiLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string data = value as string;
            if (data == null) return value;
            int stripIndex = data.IndexOfAny(new[] { '\r', '\n' });
            if (stripIndex < 0) return data;
            return data.Substring(0, stripIndex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
