﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash.EnemyStates
{
    public class Bald : Enemy
    {
        private NPCStateMachine stateMachine;

        private float distanceToBeginPowerup = 300.0f;
        private float DistanceOfCharge { get { return distanceToBeginPowerup * 2; } }

        // public Vector2 CurrentChargeToPosition { get; set; }


        public Bald()
        {
            DealDamageValue = 20;
            KillScore = 50;
        }


        public Bald(EnemyAnimations enemyAnimations, Vector2 initialPosition)
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

            EnemyAnimations = EnemyDirector.Instance.BaldAnimations;
            // Position = new Vector2(0);

            stateMachine = new NPCStateMachine(this);

            // Create the states
            IdleState idle = new IdleState("idle");
            FleeState flee = new FleeState("flee");
            ChaseState chase = new ChaseState("chase");
            PowerupState powerup = new PowerupState("powerup", 5.0f);
            ChargeState charge = new ChargeState("charge", 10.0f);

            // Transitions
            // From Ideal


            // From Chase
            chase.AddTransition(new Transition(powerup, () => (EnemyDirector.Instance.SqrDistanceToHeroFrom(Position)) <= Math.Pow(distanceToBeginPowerup, 2)));

            // From Powerup
            powerup.AddTransition(new Transition(charge, () => powerup.Done));

            // From Charge
            charge.AddTransition(new Transition(chase, () => charge.Done));

            // Dead Player - All states back to idle
            // ? 


            // Add the created states to the FSM
            // stateMachine.AddState(idle);
            // stateMachine.AddState(flee);
            stateMachine.AddState(chase);
            stateMachine.AddState(powerup);
            stateMachine.AddState(charge);

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
            throw new NotImplementedException();
        }

        public override void TakeDamage(int damagePoints, Vector2 direction)
        {
            throw new NotImplementedException();
        }

    }
}
