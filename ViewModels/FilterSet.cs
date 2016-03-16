using System;
using System.Collections.Generic;

namespace Zw.JsonLogViewer.ViewModels
{
    internal class FilterSet
    {
        public Dictionary<string, string> ColumnFilters { get; private set; }
        public string GlobalSearchText { get; set; }

        public FilterSet()
        {
            this.ColumnFilters = new Dictionary<string, string>();
        }
    }
}
