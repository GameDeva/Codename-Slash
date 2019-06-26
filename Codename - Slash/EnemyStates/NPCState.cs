using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    // Abstract class to be inherited by any NPCState classes with unique functionality
    public abstract class NPCState
    {
        // Methods that must be overriden with functionality
        public abstract void Enter(Enemy owner);
        public abstract void Exit(Enemy owner);
        public abstract void Execute(Enemy owner, float deltaTime);

        public string Name
        {
            get;
            set;
        }

        // List of transitions a current state has 
        private List<Transition> transitionList = new List<Transition>();
        public List<Transition> TransitionList
        {
            get { return transitionList; }
        }

        // Adds a new transition 
        public void AddTransition(Transition transition)
        {
            transitionList.Add(transition);
        }
    }

    // Transition class that stores the state to go to, with a Func condition to check 
    public class Transition
    {
        public readonly NPCState NextState;
        public readonly Func<bool> Condition;

        public Transition(NPCState nextState, Func<bool> condition)
        {
            NextState = nextState;
            Condition = condition;
        }
    }
    
}
