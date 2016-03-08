using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.JsonLogViewer.Events
{
    public class SetDetailPanelKeyMessage : BaseEvent
    {
        public SetDetailPanelKeyMessage(object sender, string key) : base(sender)
        {
            this.Key = key;
        }

        public string Key { get; protected set; }
    }
}
