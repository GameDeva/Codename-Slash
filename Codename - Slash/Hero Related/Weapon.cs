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
    public class Weapon
    {
        // public delegate void BulletCreated<Bullet>(ref Bullet bulletInstance);

        public Action OnShootAction;
        public Action OnBulletCreated;
        public Action<int> OnReload;
        public Action OutOfAmmoAction;

        public Texture2D WeaponIconTexture { get; private set; }
        public Texture2D WeaponTexture { get; private set; }
        public Texture2D BulletTexture { get; private set; }

        // 
        public int MaximumAmmoCarry { get; protected set; }
        public int MaximumMagHold { get; protected set; }
        public int AmmoPerShot { get; protected set; }
        public float TimeBetweenShots { get; protected set; }
        public float BulletMoveSpeed { get; protected set; }
        public float BulletDecayTime { get; protected set; }


        // 
        public int CurrentAmmoCarry { get; protected set; }
        public int CurrentMagHold { get; protected set; }

        public List<Bullet> BulletsFired { get; protected set; }

        public Weapon(Texture2D weaponIconTexture, Texture2D weaponTexture, Texture2D bulletTexture)
        {
            WeaponIconTexture = weaponIconTexture;
            WeaponTexture = weaponTexture;
            BulletTexture = bulletTexture;

            BulletsFired = new List<Bullet>();

        }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Shoot(Vector2 firePoint, Vector2 fireDirection) { }
        
        // Only override if there is something specific we want to do for each weapon
        public virtual void Reload()
        {
            int amountToRefill = MaximumMagHold - CurrentMagHold;

            // If there is not enough ammo carried to reload, invoke the out of Ammo Delegate
            if (CurrentAmmoCarry < amountToRefill)
            {
                Console.WriteLine("out of ammo");
                OutOfAmmoAction?.Invoke();
                return;
            }

            // TODO: Wait for reloadtime before adding the ammo

            // Fill ammo to the mag
            CurrentMagHold = MaximumMagHold;

            // Reduce from the ammo count 
            CurrentAmmoCarry -= amountToRefill;

            OnReload?.Invoke(amountToRefill);
        }

        // When ammo is picked up
        public virtual void OnAmmoPickup(int pickupValue)
        {
            // Clamp the added ammo to the maximum ammo carry
            CurrentAmmoCarry = MathHelper.Clamp(CurrentAmmoCarry + pickupValue, 0, MaximumAmmoCarry);
        }

    }
}
