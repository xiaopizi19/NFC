using System.Collections;
using System;

namespace IEvent
{
    public enum EventID : int
    {
        IEVENT_NETWORK_CONNECT = 1000, //
        IEVENT_NETWORK_CONNECT_FAIL,
        IEVENT_NETWORK_CONNECT_SUCESS,
    }
}