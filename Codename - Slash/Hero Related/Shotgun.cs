using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Codename___Slash
{
    public class Shotgun : Weapon
    {
        
        public Shotgun()
        {
            // NOTE: Below assumes weapons are complete when created, 
            // TODO: For pickup weapons that are half empty, need to refactor
            //          to take these values as parameters
            MaximumAmmoCarry = 64;
            MaximumMagHold = 8;
            MaxTimeBetweenShots = 0.7f;
            BulletMoveSpeed = 1500;
            BulletDecayTime = 0.2f;
            BulletColliderSize = new Vector2(50, 50);

            CurrentAmmoCarry = MaximumAmmoCarry;
            CurrentMagHold = MaximumMagHold;
            

        }

        public override void Shoot(Vector2 firePoint, Vector2 fireDirection)
        {
            if (currentTimerBetweenShots > MaxTimeBetweenShots)
            {
                CurrentMagHold--;
                // Let UI or others know shot has been fired
                OnShootAction?.Invoke();
                // Create bullet with given arguments
                OnBulletCreated?.Invoke(new ArgsBullet(false, firePoint, fireDirection, BulletTexture, BulletDecayTime, BulletMoveSpeed, BulletColliderSize, 50));
                // Reset timer
                currentTimerBetweenShots = 0.0f;
            }

        }

    }
}
