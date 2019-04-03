using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    public class StateMachine
    {
        private Enemy owner;
        private List<NPCState> stateList;

        private NPCState m_CurrentState;

        public StateMachine()
            : this(null)
        {
        }

        public StateMachine(Enemy owner)
        {
            this.owner = owner;
            stateList = new List<NPCState>();
            m_CurrentState = null;
        }

        public void Initialise(string stateName)
        {
            m_CurrentState = stateList.Find(state => state.Name.Equals(stateName));
            if (m_CurrentState != null)
            {
                m_CurrentState.Enter(owner);
            }
        }

        public void AddState(NPCState state)
        {
            stateList.Add(state);
        }

        public void Update(float deltaTime)
        {
            // Null check the current state of the FSM
            if (m_CurrentState == null) return;

            // Check the conditions for each transition of the current state
            foreach (Transition t in m_CurrentState.TransitionList)
            {
                // If the condition has evaluated to true
                // then transition to the next state
                if (t.Condition())
                {
                    m_CurrentState.Exit(owner);
                    m_CurrentState = t.NextState;
                    m_CurrentState.Enter(owner);
                    break;
                }
            }

            // Execute the current state
            m_CurrentState.Execute(owner, deltaTime);
        }
    }

}
