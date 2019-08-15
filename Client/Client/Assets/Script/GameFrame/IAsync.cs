using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Define;

namespace GameFrame
{
    public class IAsync : IMonoSingle<IAsync>
    {
        static List<Callback> m_Async = new List<Callback>();
        public void Run(Callback callback)
        {
            m_Async.Add(callback);
        }

        public void Excute()
        {
            if (m_Async.Count > 0)
            {
                for(int i = 0; i < m_Async.Count; i++)
                {
                    m_Async[i]();
                }
                m_Async.Clear();
            }
        }
    }
}