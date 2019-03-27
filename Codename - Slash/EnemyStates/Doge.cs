using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash.EnemyStates
{
    public class Doge : Enemy
    {
        private StateMachine stateMachine;

        private float distanceToBeginChase = 500.0f;
        private float distanceToBeginAttack = 75.0f;

        public Doge()
        {

        }


        public Doge(EnemyAnimations enemyAnimations, Vector2 initialPosition)
        {

        }

        public override void Update(GameTime gameTime)
        {
            stateMachine.Update(gameTime);

            base.Update(gameTime);
        }

        public override void OnPoolInstantiation()
        {
            // Stats
            MoveSpeed = 100.0f;
            DrawColor = Color.White;

            animator = new Animator();

            EnemyAnimations = EnemyDirector.Instance.DogeAnimations;
            // Position = new Vector2(0);

            stateMachine = new StateMachine(this);

            // Create the states
            IdleState idle = new IdleState("idle");
            FleeState flee = new FleeState("flee");
            ChaseState chase = new ChaseState("chase");
            ShortRangeAttackState shortRangeAttack = new ShortRangeAttackState("shortRangeAttack", 2.0f, 1.5f, true);

            // Create the transitions between the states
            idle.AddTransition(new Transition(chase, () => (Math.Pow(distanceToBeginChase, 2) >= EnemyDirector.Instance.SqrDistanceToHeroFrom(Position))));
            // idle.AddTransition(new Transition(shortRangeAttack, () => (Math.Pow(distanceToBeginAttack, 2) >= EnemyDirector.Instance.SqrDistanceToHeroFrom(Position))));
            // flee.AddTransition(new Transition(idle, () => !TaggerSeen));
            chase.AddTransition(new Transition(shortRangeAttack, () => (Math.Pow(distanceToBeginAttack, 2) >= EnemyDirector.Instance.SqrDistanceToHeroFrom(Position))));
            chase.AddTransition(new Transition(idle, () => (Math.Pow(distanceToBeginChase, 2) < EnemyDirector.Instance.SqrDistanceToHeroFrom(Position))));
            // shortRangeAttack.AddTransition(new Transition(idle, () => (Math.Pow(distanceToBeginChase, 2) < EnemyDirector.Instance.SqrDistanceToHeroFrom(Position))));
            shortRangeAttack.AddTransition(new Transition(chase, () => (Math.Pow(distanceToBeginAttack, 2) < EnemyDirector.Instance.SqrDistanceToHeroFrom(Position)) && !shortRangeAttack.InAttack));


            // Add the created states to the FSM
            stateMachine.AddState(idle);
            stateMachine.AddState(flee);
            stateMachine.AddState(chase);
            stateMachine.AddState(shortRangeAttack);

            //// Set the starting state of the FSM
            //stateMachine.Initialise("idle");

            base.OnPoolInstantiation();
        }

        public override void OnSpawnFromPool(IArgs args)
        {
            // Reinitialise enemy
            // Pattern match with the correct concrete class
            if (!(args is ArgsEnemy a)) { throw new ArgumentException(); }

            stateMachine.Initialise(a.InitialState);
            Position = a.Position;
            CurrentHealth = a.StartingHealth;

            // Assign values
            animator.AttachAnimation(EnemyAnimations.IdleAnimation);

            IsActive = true;
        }

        #region Condition Methods for Transitions


        #endregion

    }
}
