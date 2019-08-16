using UnityEngine;
using System.Collections;
using System;
using Protocol;

namespace NetWork
{
    public class NetworkCtrl
    {
        private void OnConnectStart()
        {
            //open waiting ui
        }
        private void OnConnectSuccess()
        {
            //close waiting ui
        }
        private void OnConnectFail(MessageRetCode arg)
        {

        }
        public void AddHandlers()
        {
            IEvent.IEventCenter.AddHandler(IEvent.EventID.IEVENT_NETWORK_CONNECT_SUCESS, OnConnectSuccess);
            IEvent.IEventCenter.AddHandler<MessageRetCode>(IEvent.EventID.IEVENT_NETWORK_CONNECT_FAIL, OnConnectFail);
            IEvent.IEventCenter.AddHandler(IEvent.EventID.IEVENT_NETWORK_CONNECT, OnConnectStart);
        }
        public void DelHandlers()
        {
            IEvent.IEventCenter.DelHandler(IEvent.EventID.IEVENT_NETWORK_CONNECT_SUCESS, OnConnectSuccess);
            IEvent.IEventCenter.DelHandler(IEvent.EventID.IEVENT_NETWORK_CONNECT, OnConnectStart);
            IEvent.IEventCenter.DelHandler<MessageRetCode>(IEvent.EventID.IEVENT_NETWORK_CONNECT_FAIL, OnConnectFail);

        }
    }
}