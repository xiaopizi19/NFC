using UnityEngine;
using System.Collections;
using Protocol;
using ProtoBuf;
using System.Collections.Generic;
namespace NetWork
{
    public class NetworkManager : GameFrame.ISingleton<NetworkManager>
    {
        private NetworkClient m_Client;
        private static Dictionary<MessageID, MessageCallbackData> m_MessageDispatchs = new Dictionary<MessageID, MessageCallbackData>();
        private static List<MessageRecvData> m_RecvPackets = new List<MessageRecvData>();
        private static List<MessageSendData> m_SendPackets = new List<MessageSendData>();

        private NetworkCtrl m_NetworkCtrl = new NetworkCtrl();
        private static int m_nWritePos = 0;
        private static byte[] m_PacketHeader = new byte[28];
        private string m_LoginIp;
        private int m_Port;
        private ulong m_MainGuid;

        public override void Init()
        {
            m_NetworkCtrl.AddHandlers();
        }

        public void SetIpAddress(string ip,int port)
        {
            m_LoginIp = ip;
            m_Port = port;
        }

        public void SetMainGuid(ulong uGuid)
        {
            m_MainGuid = uGuid;
        }

        public static void RecvS2C(NetworkClient client,byte[] bytes)
        {
            byte[] data = new byte[bytes.Length - 28];
            for(int i = 0; i < data.Length; i++)
            {
                data[i] = bytes[i + 28];
            }
            
            MessageRecvData evet = new MessageRecvData();
            evet.Data = data;
            evet.Client = client;
            evet.MsgID = (MessageID)System.BitConverter.ToInt32(bytes, 4);
            evet.UserData = System.BitConverter.ToUInt32(bytes, 24);
            evet.TargetID = System.BitConverter.ToUInt64(bytes, 16);
            lock (m_RecvPackets)
            {
                m_RecvPackets.Add(evet);
            }
        }



        static void DispatchPacket(MessageRecvData recv)
        {
            Debug.Log("Receive MessageID:" + recv.MsgID);
            MessageCallbackData d = null;
            if(m_MessageDispatchs.TryGetValue(recv.MsgID,out d))
            {
                d.Handler(recv);
            }
        }

        static public void WriteUInt32(System.UInt32 v)
        {
            for(int i = 0; i < 4; i++)
            {
                m_PacketHeader[m_nWritePos++] = (byte)(v >> i * 8 & 0xff);
            }
        }

        static public void WriteUInt64(System.UInt64 v)
        {
            byte[] getdata = System.BitConverter.GetBytes(v);
            for(int i = 0; i < getdata.Length; i++)
            {
                m_PacketHeader[m_nWritePos++] = getdata[i];
            }
        }

        static public bool MakePacketHeader(MessageID messageID,System.UInt64 u64Target,System.UInt32 dwUserData)
        {
            m_nWritePos = 0;
            System.UInt32 CheckCode = 0x88;
            System.UInt32 dwMsgID = (System.UInt32)messageID;
            System.UInt32 dwSize = 3;
            System.UInt32 dwPacketNo = 0;//生成序号 = wCommandID^dwSize+index(每个包自动增长索引); 还原序号 = pHeader->dwPacketNo - pHeader->wCommandID^pHeader->dwSize;
            WriteUInt32(CheckCode);
            WriteUInt32(dwMsgID);
            WriteUInt32(dwSize);
            WriteUInt32(dwPacketNo);
            WriteUInt64(u64Target);
            WriteUInt32(dwUserData);
            return true;
        }

        static void Pack<T>(MessageID messageID,T obj,System.UInt64 u64TargetID,System.UInt32 dwUserData,ref byte[] bytes)
        {
            System.IO.MemoryStream byteMs = new System.IO.MemoryStream();
            MakePacketHeader(messageID, u64TargetID, dwUserData);
            byteMs.Write(m_PacketHeader, 0, 28);
            Serializer.Serialize<T>(byteMs, obj);
            bytes = byteMs.ToArray();
            System.UInt32 nLen = (System.UInt32)bytes.Length;
            int nPos = 8;
            for(int i = 0; i < 4; i++)
            {
                bytes[nPos++] = (System.Byte)(nLen >> i * 8 & 0xff);
            }

        }

        public void Send<T>(MessageID messageID,T obj,System.UInt64 u64TargetID,System.UInt32 dwUserData)
        {
            Send(m_Client, messageID, obj, u64TargetID, dwUserData);
        }

        static void Send<T>(NetworkClient client,MessageID messageID,T obj,System.UInt64 u64TargetID,System.UInt32 dwUserData)
        {
            byte[] bytes = null;
            Pack<T>(messageID, obj, u64TargetID, dwUserData, ref bytes);
            MessageSendData tab = new MessageSendData(client, bytes);
            m_SendPackets.Add(tab);
        }

        static void Send(MessageSendData msg)
        {
            msg.Client.Send(msg.Bytes);
        }

        public void Execute()
        {
            lock (m_RecvPackets)
            {
                if (m_RecvPackets.Count > 0)
                {
                    for(int i = 0; i < m_RecvPackets.Count; i++)
                    {
                        MessageRecvData recv = m_RecvPackets[i];
                        DispatchPacket(recv);
                    }
                    m_RecvPackets.Clear();
                }
            }
            lock (m_SendPackets)
            {
                if (m_SendPackets.Count > 0)
                {
                    for(int i = 0; i < m_SendPackets.Count; i++)
                    {
                        Send(m_SendPackets[i]);
                    }
                    m_SendPackets.Clear();
                }
            }
        }

        public void ConnectLoginServer(Callback OnConnect)
        {
            m_Client = new NetworkClient(m_LoginIp, m_Port);
            m_Client.OnConnectSucess = OnConnect;
            m_Client.Connect();
        }

        public static void AddListener(MessageID id, NetworkCallback handle, object target)
        {
            MessageCallbackData d = null;
            if(m_MessageDispatchs.TryGetValue(id,out d))
            {
                d.Handler = handle;
                d.Target = target;
                d.ID = id;
            }
            else
            {
                d = new MessageCallbackData();
                m_MessageDispatchs[id] = d;
            }
        }

        public static void DelListener(MessageID id,NetworkCallback handle)
        {
            m_MessageDispatchs[id] = null;
        }

        public static void DelListenerByTarget(object target)
        {
            List<MessageCallbackData> list = new List<MessageCallbackData>(m_MessageDispatchs.Values);
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i].Target == target)
                {
                    m_MessageDispatchs.Remove(list[i].ID);
                }
            }
        }

        public void AddMessageCenterHandler()
        {

        }
        public void DelMessageCenterHandler()
        {

        }
        public void ReConnect()
        {
            if(m_Client != null) { m_Client.Connect(); }
        }


    }
}