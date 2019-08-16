using System.Collections;

namespace NetWork
{
    public class MessageRecvData
    {
        public NetworkClient Client;
        public Protocol.MessageID MsgID;
        public System.UInt64 TargetID;
        public System.UInt32 UserData;
        public byte[] Data { get; set; }
    }
}