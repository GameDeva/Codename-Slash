using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash.EnemyStates
{
    public class ChaseState : State
    {
        public ChaseState(string name)
        {
            Name = name;
        }

        public override void Enter(Enemy owner)
        {
            owner.Animator.AttachAnimation(owner.EnemyAnimations.DownAnimation);
            owner.DrawColor = Color.Yellow;

        }

        public override void Execute(Enemy owner, float deltaTime)
        {
            // Get direction to hero
            Vector2 dir = EnemyDirector.Instance.DirectionToHeroNormalised(owner.Position);
            
            // Move position in that direction by movespeed
            owner.Position += owner.MoveSpeed * dir * deltaTime;
            
            //// Change animation based on direction
            //if (dir.X > 0 && dir.Y > 0)
            //{
            //    owner.animator.AttachAnimation(owner.EnemyAnimations.RightAnimation);
            //}
            //else if (dir.X < 0 && dir.Y < 0)
            //{
            //    owner.animator.AttachAnimation(owner.EnemyAnimations.UpAnimation);
            //}
            //else if (dir.X < 0 && dir.Y < 0)
            //{
            //    owner.animator.AttachAnimation(owner.EnemyAnimations.LeftAnimation);
            //}
            //else if (dir.X > 0 && dir.Y > 0)
            //{
            //    owner.animator.AttachAnimation(owner.EnemyAnimations.DownAnimation);
            //}
        }

        public override void Exit(Enemy owner)
        {

        }
    }
}
