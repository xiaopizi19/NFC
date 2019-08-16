using System.Collections;
using UnityEngine;

namespace NetWork
{
    public class MessageCallbackData
    {
        public Protocol.MessageID ID;
        public NetworkCallback Handler;
        public object Target;
    }
}