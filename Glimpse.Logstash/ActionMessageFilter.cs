using Glimpse.Mvc.AlternateType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Logstash
{
    public class ActionMessageFilter : IMessageFilter
    {
        public bool Accept(Core.Message.IMessage message)
        {
            ActionInvoker.InvokeActionMethod.Message msg = (ActionInvoker.InvokeActionMethod.Message)message;
            return msg.EventCategory.Name == "Controller";
        }
    }
}
