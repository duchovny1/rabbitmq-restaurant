using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
    public interface IEventBus
    {
        void Publish(string message, string topic_name);

        void PublishMessage(string message, string routing_key);
    }
}
