using Glimpse.Ado.Message;
using Glimpse.AspNet.Message;
using Glimpse.Core.Message;
using Glimpse.Mvc.AlternateType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Glimpse.Logstash
{
    public class ResultMessageSerializer : IMessageSerializer
    {
        public void Serialize(String app, IMessage message, StreamWriter writer)
        {
            ResultFilter.OnResultExecuted.Message msg = (ResultFilter.OnResultExecuted.Message)message;
            writer.Write(@"{""type"":""HTTP"",""app"":""");
            writer.Write(app);
            writer.Write(@""",""status"":""");
            writer.Write(HttpContext.Current.Response.StatusCode);
            writer.Write(@""",""time"":""");
            writer.Write(msg.Duration.TotalMilliseconds);
            writer.Write(@""",""uri"":""");
            var request = HttpContext.Current.Request;
            writer.Write(request.HttpMethod);
            writer.Write(" ");
            writer.Write(request.Url.AbsolutePath);
            writer.Write(@"""");
            if (HttpContext.Current.Error != null)
            {
                var exception = HttpContext.Current.Error.GetBaseException();
                writer.Write(@"""exception"":""");
                writer.Write(exception.GetType().Name);
                writer.Write(": ");
                writer.Write(exception.Message == null ? "" : exception.Message.Replace('"', '\''));
                writer.Write(@"""");
            }
            writer.Write("}\n");
        }
    }
}
