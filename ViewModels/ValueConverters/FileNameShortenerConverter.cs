using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Zw.JsonLogViewer.ViewModels.ValueConverters
{
    public class FileNameShortenerConverter : IValueConverter
    {

        private static readonly log4net.ILog log = global::log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int MAX_LENGTH = 50;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string filename = value as string;
            if (String.IsNullOrWhiteSpace(filename))
            {
                log.Warn("Invalid filename passed");
                return Binding.DoNothing;
            }
            string directory = Path.GetDirectoryName(filename);
            if (directory.Length > MAX_LENGTH)
            {
                return String.Format("{0}..{1}{2}", directory.Substring(0, MAX_LENGTH), Path.DirectorySeparatorChar, Path.GetFileName(filename));
            }
            return filename;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
