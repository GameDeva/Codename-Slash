using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class State
    {
        private List<Transition> transitionList;

        public State()
        {

        }



    }

    public class Transition
    {
        State currentState;
        State nextState;
        
        // Construct transition 2 states, one that it transitions from and one it transitions to
        public Transition(State currentState, State nextState)
        {
            this.currentState = currentState;
            this.nextState = nextState;
        }

    }

    public class StateMachine
    {
        private List<State> stateList; // List of all States in the State Machine
        private State currentState; // The current state of the State Machine
        
        public StateMachine()
        {
            stateList = new List<State>();


        }
        
    }
}
