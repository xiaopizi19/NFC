using System.Collections;

namespace NetWork
{
    public class MessageSendData
    {
        public NetworkClient Client { get; private set; }
        public byte[] Bytes { get; private set; }

        public MessageSendData(NetworkClient client,byte[] bytes)
        {
            this.Client = client;
            this.Bytes = bytes;
        }
    }
}