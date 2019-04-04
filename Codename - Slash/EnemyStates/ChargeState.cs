using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash.EnemyStates
{
    public class ChargeState : NPCState
    {
        float currentTimer;

        private Vector2 currentAttackPos;
        private Vector2 initialPosition;
        float speed;

        public bool Done { get; set; }

        public ChargeState(string name, float speed)
        {
            Name = name;
            this.speed = speed;
        }

        public override void Enter(Enemy owner)
        {
            Done = false;
            currentTimer = 0.0f;
            owner.DrawColor = Color.IndianRed;
            owner.Animator.AttachAnimation(owner.EnemyAnimations.DownAnimation);

            initialPosition = owner.Position;
            currentAttackPos = EnemyDirector.Instance.GetHeroPosition();
        }

        public override void Execute(Enemy owner, float deltaTime)
        {
            if (currentTimer <= 1.0f)
            {
                // Increase value by speed
                currentTimer += speed * deltaTime;
                // Smooth Step Function
                float positionLerpValue = (float)(Math.Pow(currentTimer, 2) * (3 - 2 * currentTimer));
                // Assign appropriate position
                owner.Position = Vector2.Lerp(initialPosition, currentAttackPos, positionLerpValue);
            } else
            {
                Done = true;
            }
        }

        public override void Exit(Enemy owner)
        {
        }

    }
}
