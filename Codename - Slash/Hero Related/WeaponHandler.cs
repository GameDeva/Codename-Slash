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
    public class WeaponHandler
    {
        // An delegate of type weapon, that can send arguments as Refs  
        public delegate void ActionRef<Weapon>(ref Weapon previousWeapon, ref Weapon newWeapon);
        public Action<IArgs> OnSpawnBullet; // Action to spawn bullet

        public List<Weapon> WeaponsList { get; private set; } // List of weapons
        // Currently equipped weapon
        public Weapon EquippedWeapon { get { return equippedWeapon; } set { equippedWeapon = value; } } 
        private Weapon equippedWeapon;
        private int equippedWeaponIndex;

        private MouseState mouseState; // 
        private float rotationRadius = 50.0f; // Radius of weapon from hero to rotate around
        private float rotationAngleWeapon = 0.0f; // Rotation angle of weapon
        private Vector2 weaponPostion; // position of weapon
        private Vector2 directionToShoot; // Direction of fire

        public ActionRef<Weapon> OnWeaponSwap; // Weapon Swap Action that takes 2 weapons as a reference
        
        // constructor
        public WeaponHandler()
        {
            WeaponsList = new List<Weapon>();
            WeaponsList.Add(new MachineGun());
            WeaponsList.Add(new Shotgun());
        }

        // 
        public void LoadContent(ContentManager content)
        {
            // Load in textures 
            WeaponsList[0].LoadContent(content.Load<Texture2D>("Sprites/Weapons/mg/mg_UI_icon"), content.Load<Texture2D>("Sprites/Weapons/mg/mg_side"), content.Load<Texture2D>("Sprites/Weapons/mg/bulleta"));
            WeaponsList[1].LoadContent(content.Load<Texture2D>("Sprites/Weapons/Shotgun/shot_side"), content.Load<Texture2D>("Sprites/Weapons/Shotgun/shot_side"), content.Load<Texture2D>("Sprites/Weapons/mg/bulleta"));

            // Equip first weapon
            EquipWeapon(0);
            OnWeaponSwap?.Invoke(ref equippedWeapon, ref equippedWeapon);

        }

        //
        public void Update(Vector2 heroPosition, float deltaTime)
        {
            UpdateWeaponPosition(heroPosition);
            EquippedWeapon.Update(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 heroPosition)
        {
            Rectangle weaponSourceRect = new Rectangle(0, 0, EquippedWeapon.WeaponTexture.Width, EquippedWeapon.WeaponTexture.Height);
            
            // Rectangle shotgunDest = new Rectangle((int)(position.X + 2.0f), (int)position.Y, shotgun.Width, shotgun.Height);
            spriteBatch.Draw(EquippedWeapon.WeaponTexture, weaponPostion, weaponSourceRect, Color.White, rotationAngleWeapon, new Vector2(EquippedWeapon.WeaponTexture.Width / 2, EquippedWeapon.WeaponTexture.Height / 2), 2.0f, mouseState.X < heroPosition.X ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            // new Vector2(position.X + 2.0f, position.Y), new Rectangle(0, 0, 0, 0), Color.White, 0.0f, new Vector2(shotgun.Width/2, shotgun.Height/2), SpriteEffects.None);

        }

        // Updates the weapon position and rotation based on the mouse state and the hero position
        private void UpdateWeaponPosition(Vector2 heroPosition)
        {
            mouseState = Mouse.GetState();

            rotationAngleWeapon = (float)Math.Atan2(mouseState.Position.Y - heroPosition.Y, mouseState.Position.X - heroPosition.X);
            
            weaponPostion = new Vector2((float)(heroPosition.X + rotationRadius * Math.Cos(rotationAngleWeapon)), (float)(heroPosition.Y + rotationRadius * Math.Sin(rotationAngleWeapon)));

            directionToShoot = Vector2.Normalize(new Vector2(mouseState.Position.X, mouseState.Position.Y) - weaponPostion);

        }

        // Attaches next weapon on list
        public void NextWeapon()
        {
            if(equippedWeaponIndex == WeaponsList.Count - 1)
            {
                EquipWeapon(0);
                // equippedWeapon = weaponsList[0];
            } else
            {
                EquipWeapon(equippedWeaponIndex + 1);
                // equippedWeapon = weaponsList[equippedWeaponIndex + 1];
            }

            // Invokes the event of the swap, useful for listeners such as GameplayUI
            OnWeaponSwap?.Invoke(ref equippedWeapon, ref equippedWeapon);

        }

        // Attaches previous weapon on list
        public void PreviousWeapon()
        {
            if (equippedWeaponIndex == 0)
            {
                EquipWeapon(WeaponsList.Count - 1);
                // equippedWeapon = weaponsList[weaponsList.Count - 1];
            }
            else
            {
                EquipWeapon(equippedWeaponIndex - 1);
                // equippedWeapon = weaponsList[equippedWeaponIndex - 1];
            }

            // Invokes the event of the swap, useful for listeners such as GameplayUI
            OnWeaponSwap?.Invoke(ref equippedWeapon, ref equippedWeapon);

        }

        // Equip the given weapon index and attaches the appropriate event
        private void EquipWeapon(int weaponIndex)
        {
            equippedWeapon = WeaponsList[weaponIndex];
            equippedWeaponIndex = weaponIndex;

            equippedWeapon.OnBulletCreated += SpawnBullet;
        }

        // Checks if weapon can be shot and calls appropriate method
        public void ShootEquippedWeapon()
        {
            // If not enough in mag to shoot, reload weapon
            if(EquippedWeapon.CurrentMagHold < 1)
            {
                EquippedWeapon.Reload();
                return;
            }
            // Else shoot based on position and direction, shoot logic handled within each weapon
            EquippedWeapon.Shoot(weaponPostion, directionToShoot);
        }

        // Spawn bullet with specific set of arguments
        private void SpawnBullet(IArgs args)
        {
            OnSpawnBullet?.Invoke(args);
        }


    }
}
