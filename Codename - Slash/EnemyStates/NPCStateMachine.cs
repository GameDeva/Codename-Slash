using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    // 
    public class NPCStateMachine
    {
        // The owner of the statemachine instance
        private Enemy owner;
        // List of states available
        private List<NPCState> stateList;
        // Current state the fsm is on
        private NPCState m_CurrentState;

        // Constructor to initialise the owner as null 
        public NPCStateMachine()
            : this(null)
        {
        }

        // 
        public NPCStateMachine(Enemy owner)
        {
            this.owner = owner;
            stateList = new List<NPCState>();
            m_CurrentState = null;
        }

        // Puts state machine in the given state
        public void Initialise(string stateName)
        {
            m_CurrentState = stateList.Find(state => state.Name.Equals(stateName));
            if (m_CurrentState != null)
            {
                m_CurrentState.Enter(owner);
            }
        }

        // Adds new state to list of states
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
