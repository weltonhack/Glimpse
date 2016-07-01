using Glimpse.Ado.Message;
using Glimpse.AspNet.Message;
using Glimpse.Core.Configuration;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;
using Glimpse.Mvc.AlternateType;
using Glimpse.Mvc.Message;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace Glimpse.Logstash
{
    public class GlimpseLogstashAgent
    {
        private static HashSet<Type> acceptedMessages = new HashSet<Type>();

        private string app;

        private string host;

        private int port;

        private Dictionary<Type, IMessageFilter> specificFilters = new Dictionary<Type, IMessageFilter>();

        private Dictionary<Type, IMessageSerializer> messageSerializers = new Dictionary<Type, IMessageSerializer>();

        public GlimpseLogstashAgent()
        {
            var section = ConfigurationManager.GetSection("glimpse") as Section ?? new Section();
            this.app = section.App;
            var logstash = section.Logstash;

            if (logstash != null)
            {
                var logstashParts = logstash.Split(':');
                this.host = logstashParts[0];
                this.port = int.Parse(logstashParts[1]);
                acceptedMessages.Add(typeof(CommandExecutedMessage));
                //acceptedMessages.Add(typeof(ActionInvoker.InvokeActionMethod.Message));
                //acceptedMessages.Add(typeof(IExceptionFilterMessage));
                acceptedMessages.Add(typeof(ResultFilter.OnResultExecuted.Message));
                //specificFilters[typeof(ActionInvoker.InvokeActionMethod.Message)] = new ActionMessageFilter();
                messageSerializers[typeof(CommandExecutedMessage)] = new AdoCommandExecutedMessageSerializer();
                messageSerializers[typeof(ResultFilter.OnResultExecuted.Message)] = new ResultMessageSerializer();
            }
        }

        public void Subscribe()
        {
            //HttpContext.Current.
            var client = new TcpClient();
            client.Connect(host, port);
            var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            IMessageBroker broker = GlimpseConfiguration.GetConfiguredMessageBroker();
            JsonNetSerializer serializer = new JsonNetSerializer(new NullLogger());
            broker.Subscribe<IMessage>(x =>
            {
                if (acceptedMessages.Contains(x.GetType()))
                    //&& (!specificFilters.ContainsKey(x.GetType()) || specificFilters[x.GetType()].Accept(x)))
                {
                    if (messageSerializers.ContainsKey(x.GetType()))
                    {
                        messageSerializers[x.GetType()].Serialize(app, x, writer);
                    }
                }
            });
        }

    }
}
