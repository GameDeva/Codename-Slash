using Microsoft.Xna.Framework;
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
        public MachineGun(Texture2D weaponTexture, Texture2D bulletTexture) : base(weaponTexture, bulletTexture)
        {
            // NOTE: Below assumes weapons are complete when created, 
            // TODO: For pickup weapons that are half empty, need to refactor
            //          to take these values as parameters
            MaximumAmmoCarry = 256;
            MaximumMagHold = 64;
            TimeBetweenShots = 0.1f;
            BulletMoveSpeed = 500f;
            BulletDecayTime = 1f;

            CurrentAmmoCarry = MaximumAmmoCarry;
            CurrentMagHold = MaximumMagHold;

            AmmoPerShot = 1;
            
        }

        public override void Update(GameTime gameTime)
        {
            if(BulletsFired.Count != 0)
            {
                foreach (Bullet bullet in BulletsFired)
                {

                    bullet.position += BulletMoveSpeed * bullet.moveDirection * (float) gameTime.ElapsedGameTime.TotalSeconds;
                } 
            }

            base.Update(gameTime);
        }

        public override void Shoot(Vector2 firePoint, Vector2 fireDirection)
        {
            // Todo: OBJECT POOLING NEEDS TO HAPPEN PLS. 
            for (int i = 0; i < AmmoPerShot; i++)
            {
                CurrentAmmoCarry--;
                BulletsFired.Add(new Bullet(firePoint, fireDirection));
                OnShoot?.Invoke();
            }

            base.Shoot(firePoint, fireDirection);
        }

    }
}
