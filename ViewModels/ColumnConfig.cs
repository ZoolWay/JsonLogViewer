using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.JsonLogViewer.ViewModels
{
    /// <summary>
    /// Defines a complete set of configured columns a ListView should use through the ConfigToDynamicGridViewConverter.
    /// </summary>
    /// <remarks>
    /// Credits to: https://github.com/9swampy/DynamicPropertyPropertiesListGridViewExample
    /// </remarks>
    public class ColumnConfig
    {
        public IEnumerable<Column> Columns { get; set; }
    }
}
