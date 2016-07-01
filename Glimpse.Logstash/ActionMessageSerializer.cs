using Glimpse.Ado.Message;
using Glimpse.Core.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Glimpse.Logstash
{
    public class ActionMessageSerializer : IMessageSerializer
    {
        public void Serialize(String app, IMessage message, StreamWriter writer)
        {
            CommandExecutedMessage msg = (CommandExecutedMessage)message;
            writer.Write(@"{""type"":""SQL"",""app"":""");
            writer.Write("net");
            writer.Write(@""",""sql"":""");
            writer.Write(msg.CommandText.Replace('"', '\'').Replace("\n", "").Replace("\r", ""));
            writer.Write(@""",""time"":""");
            writer.Write(msg.Duration.TotalMilliseconds);
            writer.Write(@""",""uri"":""");
            var request = HttpContext.Current.Request;
            writer.Write(request.HttpMethod);
            writer.Write(" ");
            writer.Write(request.Url.AbsolutePath);
            writer.Write(@"""}");
            writer.Write('\n');
        }
    }
}
