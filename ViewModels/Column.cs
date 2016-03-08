using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Zw.JsonLogViewer.ViewModels
{
    /// <summary>
    /// Defines a configured column.
    /// </summary>
    /// <remarks>
    /// Credits to: https://github.com/9swampy/DynamicPropertyPropertiesListGridViewExample
    /// </remarks>
    public class Column : PropertyChangedBase
    {
        public string Header { get; set; }

        public string DataField { get; set; }

        public string EntryKey { get; set; }

        public string FilterValue { get; set; }
    }
}
