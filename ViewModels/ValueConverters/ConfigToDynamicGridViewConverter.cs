using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Zw.JsonLogViewer.ViewModels.ValueConverters
{
    /// <summary>
    /// Converts a binding to a ColumnConfig to a GridView the ListView can use.
    /// </summary>
    /// <remarks>
    /// Credits to: https://github.com/9swampy/DynamicPropertyPropertiesListGridViewExample
    /// </remarks>
    public class ConfigToDynamicGridViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var config = value as ColumnConfig;
            if (config != null)
            {
                var gridView = new GridView();
                gridView.ColumnHeaderContainerStyle = Application.Current.FindResource("HeaderContainerStyle") as Style;
                gridView.ColumnHeaderTemplate = Application.Current.FindResource("HeaderTemplate") as DataTemplate;
                if (config.Columns != null)
                {
                    foreach (var column in config.Columns)
                    {
                        var binding = new Binding(column.DataField);
                        GridViewColumn gvc = new GridViewColumn { Header = column, DisplayMemberBinding = binding };
                        gvc.SetValue(GridViewSort.PropertyNameProperty, binding.Path.Path);
                        gridView.Columns.Add(gvc);
                    }
                }
                return gridView;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
