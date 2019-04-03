using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash.EnemyStates
{
    public class IdleState : NPCState
    {

        public IdleState(string name)
        {
            Name = name;
        }

        public override void Enter(Enemy owner)
        {
            owner.DrawColor = Color.White;
            owner.Animator.AttachAnimation(owner.EnemyAnimations.IdleAnimation);
        }

        public override void Execute(Enemy owner, float deltaTime)
        {
        }

        public override void Exit(Enemy owner)
        {
        }
    }
}
