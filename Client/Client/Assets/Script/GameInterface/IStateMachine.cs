using System.Collections.Generic;

namespace GameInterface
{
    public class IStateMachine<T, F> : IGame where T : class
    {

        private T Owner { get; set; }
        private Dictionary<F, IState<T, F>> mStates;
        private IState<T, F> mCurState;
        private IState<T, F> mPreState;
        private IState<T, F> mGloState;

        public IStateMachine(T owner,IEqualityComparer<F> comparer)
        {
            this.Owner = owner;
            this.mStates = new Dictionary<F, IState<T, F>>(comparer);
        }

        public bool Contains(F fsmID)
        {
            return this.mStates.ContainsKey(fsmID);
        }

        public void AddState(F fsmID, IState<T,F> state)
        {
            if (Contains(fsmID))
                return;

            state.SetFSMID(fsmID);
            state.Owner = Owner;
            mStates.Add(fsmID, state);
        }

        public IState<T, F> GetCurrentState()
        {
            return mCurState;
        }

        public IState<T, F> GetState(F fsmID)
        {
            IState<T, F> v = null;
            if (mStates.TryGetValue(fsmID, out v))
                return v;
            return null;

        }

        public void ChangeState(IState<T, F> newState)
        {
            if(mCurState != null)
            {
                mCurState.Exit();
            }
            mPreState = mCurState;
            mCurState = newState;
            mCurState.Enter();
        }

        public void ChangeState(F newFSM)
        {
            IState<T, F> newState = GetState(newFSM);
            if(newState != null)
            {
                ChangeState(newState);
            }
            else
            {
                UnityEngine.Debug.LogError("null state" + newFSM);
            }
        }

        public void SetCurState(IState<T, F> fsmState)
        {
            mCurState = fsmState;
        }

        public void SetPreState(IState<T, F> fsmState)
        {
            mPreState = fsmState;
        }

        public void SetGloState(IState<T, F> fsmState)
        {
            mGloState = fsmState;
        }

        public void SetCurStateID(F fsmID)
        {
            mCurState = GetState(fsmID);
        }

        public void SetGloStateID(F fsmID)
        {
            mGloState = GetState(fsmID);
        }

        public F GetCurStateID()
        {
            if (mCurState == null)
                return default(F);
            return mCurState.Fsm;
        }
        public F GetPreStateID()
        {
            if (mPreState == null)
                return default(F);
            return mPreState.Fsm;
        }

        public void Startup()
        {
            if (mCurState != null)
                mCurState.Enter();
            if (mGloState != null)
                mGloState.Enter();
        }


        public void Execute()
        {
            if (mCurState != null)
                mCurState.Execute();
            if (mGloState != null)
                mGloState.Execute();
        }
        public void Release()
        {
            if (mCurState != null)
                mCurState.Exit();
            if (mGloState != null)
                mGloState.Exit();

            mCurState = null;
            mGloState = null;
            mStates.Clear();
        }
    }
}