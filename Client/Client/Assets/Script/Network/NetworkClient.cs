using System.Net;
using System;
using System.Threading;
using ProtoBuf;
using Protocol;

namespace NetWork
{
    public delegate void Callback();

    public class NetworkClient
    {
        private System.Net.Sockets.TcpClient m_Tcp;
        private string m_IP;
        private int m_Port;
        private bool m_IsConnect = false;
        private Thread m_RecvThread;
        private int m_PacketHeaderSize = 28;
        private int m_RecvLen = 0;
        private int m_DataLen = 0;
        private byte[] m_RecvBuffer = new byte[8192];
        private byte[] m_DataBuffer = new byte[16384];
        private EventWaitHandle m_AllDone;
        public Callback OnConnectSucess;

        public NetworkClient(string ip,int port)
        {
            m_IP = ip;
            m_Port = port;
        }

        void OnClose()
        {
            m_IsConnect = false;
            if(m_RecvThread != null)
            {
                m_RecvThread.Abort();
                m_RecvThread = null;
            }
            if(m_Tcp != null && m_Tcp.Connected)
            {
                m_Tcp.Close();
            }
            m_Tcp = null;
        }

        private void OnError(MessageRetCode retCode)
        {
            GameFrame.IAsync.Instance.Run(() => { IEvent.IEventCenter.SendEvent(IEvent.EventID.IEVENT_NETWORK_CONNECT_FAIL, retCode); });
            OnClose();
        }

        private bool MakeRealPacket()
        {
            if (m_DataLen < m_PacketHeaderSize)
            {
                return false;
            }
            Byte CheckCode = m_DataBuffer[0];
            int nPacketSize = BitConverter.ToUInt16(m_DataBuffer, 8);
            if (nPacketSize > m_DataLen)
            {
                return false;
            }
            byte[] realPacket = new byte[nPacketSize];
            Array.Copy(m_DataBuffer, 0, realPacket, 0, nPacketSize);
            Array.Copy(m_DataBuffer, nPacketSize, m_DataBuffer, 0, m_DataLen - nPacketSize);
            m_DataLen = m_DataLen - nPacketSize;
            NetworkManager.RecvS2C(this, realPacket);
            return true;
        }

        private void OnAsyncRead(IAsyncResult asyncResult)
        {
            System.Net.Sockets.NetworkStream stream = (System.Net.Sockets.NetworkStream)asyncResult.AsyncState;
            m_RecvLen = stream.EndRead(asyncResult);
            if (m_RecvLen < 0)
            {
                return;
            }
            Array.Copy(m_RecvBuffer, 0, m_DataBuffer, m_DataLen, m_RecvLen);
            m_DataLen += m_RecvLen;
            m_RecvLen = 0;
            while (MakeRealPacket()) ;
            m_AllDone.Set();
        }

        private void OnRecive()
        {
            while (true)
            {
                Thread.Sleep(10);
                if(m_Tcp == null|| m_Tcp.Connected == false)
                {
                    OnError(MessageRetCode.MRC_DISCONNECT);
                    return;
                }
                System.Net.Sockets.NetworkStream stream = m_Tcp.GetStream();
                if (stream.CanRead)
                {
                    stream.BeginRead(m_RecvBuffer, 0, m_RecvBuffer.Length, new AsyncCallback(OnAsyncRead),stream);
                    m_AllDone.WaitOne();
                }
            }
        }

        private void OnConnect(IAsyncResult ar)
        {
            if(m_Tcp.Client == null)
            {
                OnError(MessageRetCode.MRC_DISCONNECT);
                return;
            }
            if (!m_Tcp.Client.Connected)
            {
                OnError(MessageRetCode.MRC_DISCONNECT);
                return;
            }
            m_IsConnect = true;
            m_AllDone = new EventWaitHandle(false, EventResetMode.AutoReset);
            m_RecvThread = new Thread(OnRecive);
            m_RecvThread.Start();
            GameFrame.IAsync.Instance.Run(() =>
            {
                if (OnConnectSucess != null)
                {
                    OnConnectSucess.Invoke();
                }
                OnConnectSucess = null;
                IEvent.IEventCenter.SendEvent(IEvent.EventID.IEVENT_NETWORK_CONNECT_SUCESS);
            });
        }

        public void Send(byte[] bytes)
        {
            if(m_IsConnect == false)
            {
                OnError(MessageRetCode.MRC_DISCONNECT);
                return;
            }
            try
            {
                m_Tcp.Client.Send(bytes);
            }
            catch(Exception ex)
            {
                UnityEngine.Debug.LogError(ex.ToString());
            }
        }

        public void Connect()
        {
            m_Tcp = new System.Net.Sockets.TcpClient();
            m_Tcp.BeginConnect(IPAddress.Parse(m_IP), m_Port, OnConnect, m_Tcp);
            GameFrame.IAsync.Instance.Run(() => { IEvent.IEventCenter.SendEvent(IEvent.EventID.IEVENT_NETWORK_CONNECT); });
        }

        public bool IsConnectOK()
        {
            return m_IsConnect;
        }

    }
}