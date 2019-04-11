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
        public delegate void ActionRef<Weapon>(ref Weapon previousWeapon, ref Weapon newWeapon);
        public Action<IArgs> OnSpawnBullet;

        public List<Weapon> WeaponsList { get; private set; }
        public Weapon EquippedWeapon { get { return equippedWeapon; } set { equippedWeapon = value; } }
        private Weapon equippedWeapon;
        private int equippedWeaponIndex;

        private MouseState mouseState;
        private float rotationRadius = 50.0f;
        private float rotationAngleWeapon = 0.0f;
        private Vector2 weaponPostion;
        private Vector2 directionToShoot;

        public ActionRef<Weapon> OnWeaponSwap;
        
        public WeaponHandler()
        {
            WeaponsList = new List<Weapon>();
            WeaponsList.Add(new MachineGun());
            WeaponsList.Add(new Shotgun());
        }

        public void LoadContent(ContentManager content)
        {
            // Load in textures 
            WeaponsList[0].LoadContent(content.Load<Texture2D>("Sprites/Weapons/mg/mg_UI_icon"), content.Load<Texture2D>("Sprites/Weapons/mg/mg_side"), content.Load<Texture2D>("Sprites/Weapons/mg/bulleta"));
            WeaponsList[1].LoadContent(content.Load<Texture2D>("Sprites/Weapons/Shotgun/shot_side"), content.Load<Texture2D>("Sprites/Weapons/Shotgun/shot_side"), content.Load<Texture2D>("Sprites/Weapons/mg/bulleta"));

            // TODO: move this to own class for weapon swapping
            EquipWeapon(0);
            OnWeaponSwap?.Invoke(ref equippedWeapon, ref equippedWeapon);

        }


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

        private void UpdateWeaponPosition(Vector2 heroPosition)
        {
            mouseState = Mouse.GetState();
            
            // dirToMouse = new Vector2(mouseState.Position.X, mouseState.Position.Y) - heroPosition;

            rotationAngleWeapon = (float)Math.Atan2(mouseState.Position.Y - heroPosition.Y, mouseState.Position.X - heroPosition.X);
            
            weaponPostion = new Vector2((float)(heroPosition.X + rotationRadius * Math.Cos(rotationAngleWeapon)), (float)(heroPosition.Y + rotationRadius * Math.Sin(rotationAngleWeapon)));

            directionToShoot = Vector2.Normalize(new Vector2(mouseState.Position.X, mouseState.Position.Y) - weaponPostion);

        }

        public void NextWeapon()
        {
            Console.WriteLine("Next weapon");

            if(equippedWeaponIndex == WeaponsList.Count - 1)
            {
                EquipWeapon(0);
                // equippedWeapon = weaponsList[0];
            } else
            {
                EquipWeapon(equippedWeaponIndex + 1);
                // equippedWeapon = weaponsList[equippedWeaponIndex + 1];
            }
            OnWeaponSwap?.Invoke(ref equippedWeapon, ref equippedWeapon);

        }

        public void PreviousWeapon()
        {

            Console.WriteLine("previous weapon");

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
            OnWeaponSwap?.Invoke(ref equippedWeapon, ref equippedWeapon);

        }

        private void EquipWeapon(int weaponIndex)
        {
            equippedWeapon = WeaponsList[weaponIndex];
            equippedWeaponIndex = weaponIndex;

            equippedWeapon.OnBulletCreated += SpawnBullet;
        }

        public void ShootEquippedWeapon()
        {
            if(EquippedWeapon.CurrentMagHold < 1)
            {
                ReloadWeapon();
                return;
            }

            EquippedWeapon.Shoot(weaponPostion, directionToShoot);
            
        }

        public void ReloadWeapon()
        {
            EquippedWeapon.Reload();
        }

        private void SpawnBullet(IArgs args)
        {
            OnSpawnBullet?.Invoke(args);

        }


    }
}
