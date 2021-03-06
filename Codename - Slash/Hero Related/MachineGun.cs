﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Codename___Slash
{
    class MachineGun : Weapon
    {
        public MachineGun()
        {
            // NOTE: Below assumes weapons are complete when created, 
            // TODO: For pickup weapons that are half empty, need to refactor
            //          to take these values as parameters
            MaximumAmmoCarry = 1024;
            MaximumMagHold = 64;
            MaxTimeBetweenShots = 0.1f;
            BulletMoveSpeed = 700;
            BulletDecayTime = 1f;
            BulletColliderSize = new Vector2(15, 15);

            CurrentAmmoCarry = MaximumAmmoCarry;
            CurrentMagHold = MaximumMagHold;
            
        }

        public override void Shoot(Vector2 firePoint, Vector2 fireDirection)
        {
            if(currentTimerBetweenShots > MaxTimeBetweenShots)
            {
                CurrentMagHold--;
                // Let UI or others know shot has been fired
                OnShootAction?.Invoke();
                // Create bullet with given arguments
                OnBulletCreated?.Invoke(new ArgsBullet(false, new Vector2(2), firePoint, fireDirection, BulletTexture, BulletDecayTime, BulletMoveSpeed, BulletColliderSize, 10));
                // Reset timer
                currentTimerBetweenShots = 0.0f;
            }
            
        }

    }
}
