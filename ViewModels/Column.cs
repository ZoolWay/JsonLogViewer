using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.JsonLogViewer.ViewModels
{
    /// <summary>
    /// Defines a configured column.
    /// </summary>
    /// <remarks>
    /// Credits to: https://github.com/9swampy/DynamicPropertyPropertiesListGridViewExample
    /// </remarks>
    public class Column
    {
        public string Header { get; set; }

        public string DataField { get; set; }
    }
}
