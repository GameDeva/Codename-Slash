using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public abstract class Command
    {
        abstract public void execute(ref Hero hero);
    }

    class MoveUpCommand : Command
    {
        public override void execute(ref Hero hero)
        {
            // move up
            // hero.MoveUp();
        }
    }


    class MoveDownCommand : Command
    {
        public override void execute(ref Hero hero)
        {
            // move down
            // hero.MoveDown();
        }
    }


    class MoveRightCommand : Command
    {
        public override void execute(ref Hero hero)
        {
            // move right
            // hero.MoveRight();
        }
    }


    class MoveLeftCommand : Command
    {
        public override void execute(ref Hero hero)
        {
            // move left
            // hero.MoveLeft();
        }
    }


    class DashCommand : Command
    {
        public override void execute(ref Hero hero)
        {
            // dash 
            //hero.Dash();
        }
    }


    class ShootCommand : Command
    {
        public override void execute(ref Hero hero)
        {
            // Shoot
            //hero.ShootWeapon();
        }
    }

    class PreviousWeaponCommand : Command
    {
        public override void execute(ref Hero hero)
        {
            // Swap to previous weapon
           // hero.PreviousWeapon();
        }
    }

    class NextWeaponCommand : Command
    {
        public override void execute(ref Hero hero)
        {
            // Swap to previous weapon
            //hero.NextWeapon();
        }
    }

}
