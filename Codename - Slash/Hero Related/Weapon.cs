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
    public abstract class Weapon
    {
        
        public Action OnShootAction;
        public Action<IArgs> OnBulletCreated;
        public Action<int> OnReload;
        public Action OutOfAmmoAction;

        public Texture2D WeaponIconTexture { get; private set; }
        public Texture2D WeaponTexture { get; private set; }
        public Texture2D BulletTexture { get; private set; }

        // 
        public int MaximumAmmoCarry { get; protected set; }
        public int MaximumMagHold { get; protected set; }
        public float MaxTimeBetweenShots { get; protected set; }
        public float BulletMoveSpeed { get; protected set; }
        public float BulletDecayTime { get; protected set; }
        public Vector2 BulletColliderSize { get; protected set; }

        protected float currentTimerBetweenShots;

        // 
        public int CurrentAmmoCarry { get; set; }
        public int CurrentMagHold { get; set; }

        public List<Bullet> BulletsFired { get; protected set; }

        public Weapon()
        {
            BulletsFired = new List<Bullet>();

        }

        public void LoadContent(Texture2D weaponIconTexture, Texture2D weaponTexture, Texture2D bulletTexture)
        {
            WeaponIconTexture = weaponIconTexture;
            WeaponTexture = weaponTexture;
            BulletTexture = bulletTexture;
            
        }

        public virtual void Update(float deltaTime)
        {
            currentTimerBetweenShots += deltaTime;
        }

        public abstract void Shoot(Vector2 firePoint, Vector2 fireDirection);
        
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
