using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    public abstract class State
    {
        public abstract void Enter(Enemy owner);
        public abstract void Exit(Enemy owner);
        public abstract void Execute(Enemy owner, float deltaTime);

        public string Name
        {
            get;
            set;
        }

        private List<Transition> transitionList = new List<Transition>();
        public List<Transition> TransitionList
        {
            get { return transitionList; }
        }

        public void AddTransition(Transition transition)
        {
            transitionList.Add(transition);
        }

    }

    public class Transition
    {
        public readonly State NextState;
        public readonly Func<bool> Condition;

        public Transition(State nextState, Func<bool> condition)
        {
            NextState = nextState;
            Condition = condition;
        }
    }


}
