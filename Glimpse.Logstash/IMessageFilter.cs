using Glimpse.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Logstash
{
    public interface IMessageFilter
    {

        bool Accept(IMessage message);

    }
}
