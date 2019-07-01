using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash.EnemyStates
{
    public class Dark : Enemy
    {
        private NPCStateMachine stateMachine;

        private float distanceToBeginChase = 1500.0f;
        private float distanceToBeginShoot = 1000.0f;

        // public Vector2 CurrentChargeToPosition { get; set; }


        public Dark()
        {
            DealDamageValue = 20;
            KillScore = 100;
        }


        public Dark(EnemyAnimations enemyAnimations, Vector2 initialPosition)
        {

        }

        public override void Update(float deltaTime)
        {
            stateMachine.Update(deltaTime);

            base.Update(deltaTime);
        }

        public override void OnPoolInstantiation()
        {
            // Stats
            MoveSpeed = 100.0f;
            DrawColor = Color.White;

            Animator = new Animator();

            EnemyAnimations = EnemyDirector.Instance.DarkAnimations;
            // Position = new Vector2(0);

            stateMachine = new NPCStateMachine(this);

            // Create the states
            IdleState idle = new IdleState("idle");
            ChaseState chase = new ChaseState("chase");
            ShootState shoot = new ShootState("shoot", 200, 2, true, EnemyDirector.Instance.EnemyBulletTexture);

            // Transitions
            idle.AddTransition(new Transition(chase, () => (Math.Pow(distanceToBeginChase, 2) >= EnemyDirector.Instance.SqrDistanceToHeroFrom(Position))));
            chase.AddTransition(new Transition(shoot, () => (Math.Pow(distanceToBeginShoot, 2) >= EnemyDirector.Instance.SqrDistanceToHeroFrom(Position))));
            chase.AddTransition(new Transition(idle, () => (Math.Pow(distanceToBeginChase, 2) < EnemyDirector.Instance.SqrDistanceToHeroFrom(Position))));
            shoot.AddTransition(new Transition(chase, () => (Math.Pow(distanceToBeginShoot, 2) < EnemyDirector.Instance.SqrDistanceToHeroFrom(Position)) && !shoot.InAttack));

            // Dead Player - All states back to idle
            // ? 


            // Add the created states to the FSM
            stateMachine.AddState(idle);
            stateMachine.AddState(chase);
            stateMachine.AddState(shoot);



            // Collider Related
            colliderSize = new Vector2Int(2, 2);
            BoundingRect = new Rectangle((int)Position.X, (int)Position.Y, colliderSize.X, colliderSize.Y);

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

            base.OnSpawnFromPool(args);
        }

        public override void TakeDamage(int damagePoints)
        {
            base.TakeDamage(damagePoints);
        }

        public override void TakeDamage(int damagePoints, Vector2 direction)
        {
            throw new NotImplementedException();
        }
    }
}
