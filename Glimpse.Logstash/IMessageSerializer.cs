using Glimpse.Core.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.Logstash
{
    public interface IMessageSerializer
    {

        void Serialize(String app, IMessage message, StreamWriter writer);

    }
}
