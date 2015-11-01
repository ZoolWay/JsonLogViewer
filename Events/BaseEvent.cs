using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zw.JsonLogViewer.Events
{
    public abstract class BaseEvent
    {
        public BaseEvent(object sender)
        {
            this.Sender = sender;
        }

        public object Sender { get; protected set; }
    }
}
