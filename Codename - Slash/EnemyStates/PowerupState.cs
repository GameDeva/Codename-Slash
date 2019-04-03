using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash.EnemyStates
{
    public class PowerupState : NPCState
    {
        float currentPowerupTimer;

        float powerupSpeed;
        public bool Done { get; set; }
        
        public PowerupState(string name, float powerupSpeed)
        {
            Name = name;
            this.powerupSpeed = powerupSpeed;
        }

        public override void Enter(Enemy owner)
        {
            //// Update charge to position, so on charge after powerup, charges to this position
            //Skull skull = owner as Skull;
            //skull.CurrentChargeToPosition = EnemyDirector.Instance.GetHeroPosition();

            Done = false;
            currentPowerupTimer = 0.0f;
            owner.DrawColor = Color.LightGreen;
            owner.Animator.AttachAnimation(owner.EnemyAnimations.IdleAnimation);
        }

        public override void Execute(Enemy owner, float deltaTime)
        {
            if (currentPowerupTimer <= 1.0f)
            {
                // Increase value by speed
                currentPowerupTimer += powerupSpeed * deltaTime;
                // Change colour to show 
                owner.DrawColor = Color.Lerp(Color.LightGreen, Color.DarkBlue, currentPowerupTimer);
            }else
            {
                Done = true;
            }
        }

        public override void Exit(Enemy owner)
        {
        }

    }
}
