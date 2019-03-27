using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash.EnemyStates
{
    public class ShortRangeAttackState : State
    {
        private float attackSpeed;
        private float timeBetweenAttacks;
        private bool startWithAttack;

        public bool InAttack { get; private set; }
        private float currentAttackCounter; // Between 0 and 1
        private float currentTimerToAttack;
        private Vector2 currentAttackPos;

        private Vector2 initialPosition;

        public ShortRangeAttackState(string name, float attackSpeed, float timeBetweenAttacks, bool startWithAttack)
        {
            Name = name;
            this.attackSpeed = attackSpeed;
            this.timeBetweenAttacks = timeBetweenAttacks;
            this.startWithAttack = startWithAttack;
        }

        public override void Enter(Enemy owner)
        {
            owner.DrawColor = Color.Red;
            owner.animator.AttachAnimation(owner.EnemyAnimations.IdleAnimation);

            // 
            InAttack = startWithAttack;
            currentAttackCounter = 0.0f;
            currentTimerToAttack = 0.0f;
            initialPosition = owner.Position;
            currentAttackPos = EnemyDirector.Instance.GetHeroPosition();
        }

        public override void Execute(Enemy owner, float deltaTime)
        {
            if(InAttack)
            {
                if(currentAttackCounter <= 1.0f)
                {
                    // Increase value by speed
                    currentAttackCounter += attackSpeed * deltaTime;
                    // Get lerp value based on function, so that the value peaks at 1 mid way and returns to 0 [to create a forward back effect]
                    float positionLerpValue = (float) (- Math.Pow(currentAttackCounter, 2) + currentAttackCounter) * 4;
                    // Assign appropriate position
                    owner.Position = Vector2.Lerp(initialPosition, currentAttackPos, positionLerpValue);
                } else
                {
                    currentAttackCounter = 0.0f;
                    InAttack = false;
                }
            } else
            {
                // Timer for between attacks
                currentTimerToAttack += deltaTime;
                if (currentTimerToAttack >= timeBetweenAttacks)
                {
                    currentAttackPos = EnemyDirector.Instance.GetHeroPosition();
                    InAttack = true;
                    currentTimerToAttack = 0.0f;
                }
            }


        }

        public override void Exit(Enemy owner)
        {

        }
    }
}
