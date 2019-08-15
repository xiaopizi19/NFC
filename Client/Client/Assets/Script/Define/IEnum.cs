namespace Define
{
    public enum FSMState : int
    {
        FSM_EMPTY,
        FSM_BORN,
        FSM_IDLE,
        FSM_WALK,
        FSM_RUN,
        FSM_ATTACK,
        FSM_DEAD,
    }

    public enum ICopyType : int
    {
       TYPE_INIT = 0,
       TYPE_LOGIN =1,
    }

    public enum Resp : ushort
    {
        TYPE_YES,
        TYPE_NO,
    }
}