using System;

namespace GameInterface
{
    public abstract class ICommand
    {
        public Delegate Del{get;set;}
        public abstract Define.Resp Do();
    }
}