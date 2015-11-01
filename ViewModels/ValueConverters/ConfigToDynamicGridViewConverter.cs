using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace Zw.JsonLogViewer.ViewModels.ValueConverters
{
    public class ConfigToDynamicGridViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var config = value as ColumnConfig;
            if (config != null)
            {
                var grdiView = new GridView();

                if (config.Columns != null)
                {
                    foreach (var column in config.Columns)
                    {
                        var binding = new Binding(column.DataField);
                        GridViewColumn gvc = new GridViewColumn { Header = column.Header, DisplayMemberBinding = binding };
                        gvc.SetValue(GridViewSort.PropertyNameProperty, binding.Path.Path);
                        grdiView.Columns.Add(gvc);
                    }
                }
                return grdiView;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
