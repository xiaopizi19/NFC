using System.Collections;
using System.Collections.Generic;
using System;

namespace Define
{
    public class IEventTypeComparer : IEqualityComparer<IEvent.EventID>
    {
        public bool Equals(IEvent.EventID x, IEvent.EventID y)
        {
            return x == y;
        }
        public int GetHashCode(IEvent.EventID obj)
        {
            return (int)obj;
        }
    }

    public class FSMStateTypeComparer : IEqualityComparer<FSMState>
    {
        public bool Equals(FSMState x,FSMState y)
        {
            return x == y;
        }

        public int GetHashCode(FSMState obj)
        {
            return (int)obj;
        }
    }

    public class ICopyTypeTypeComparer : IEqualityComparer<ICopyType>
    {
        public bool Equals(ICopyType x,ICopyType y)
        {
            return x == y;
        }

        public int GetHashCode(ICopyType obj)
        {
            return (int)obj;
        }
    }
    
}