﻿using System;
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

        private List<Weapon> weaponsList;
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
            weaponsList = new List<Weapon>();
            

        }

        public void LoadContent(ContentManager content)
        {
            // TODO : Maybe switch texture loading to weapon class? Not sure. 
            weaponsList.Add(new MachineGun(content.Load<Texture2D>("Sprites/Weapons/mg/mg_UI_icon"), content.Load<Texture2D>("Sprites/Weapons/mg/mg_side"), content.Load<Texture2D>("Sprites/Weapons/mg/bulleta")));
            weaponsList.Add(new Shotgun(content.Load<Texture2D>("Sprites/Weapons/Shotgun/shot_side"), content.Load<Texture2D>("Sprites/Weapons/Shotgun/shot_side"), content.Load<Texture2D>("Sprites/Weapons/Shotgun/bulletb")));


            // TODO: move this to own class for weapon swapping
            EquipWeapon(0);

            OnWeaponSwap?.Invoke(ref equippedWeapon, ref equippedWeapon);

        }


        public void Update(Vector2 heroPosition, GameTime gameTime)
        {
            UpdateWeaponPosition(heroPosition);
            EquippedWeapon.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 heroPosition)
        {
            Rectangle weaponSourceRect = new Rectangle(0, 0, EquippedWeapon.WeaponTexture.Width, EquippedWeapon.WeaponTexture.Height);
            
            // Rectangle shotgunDest = new Rectangle((int)(position.X + 2.0f), (int)position.Y, shotgun.Width, shotgun.Height);
            spriteBatch.Draw(EquippedWeapon.WeaponTexture, weaponPostion, weaponSourceRect, Color.White, rotationAngleWeapon, new Vector2(EquippedWeapon.WeaponTexture.Width / 2, EquippedWeapon.WeaponTexture.Height / 2), 2.0f, mouseState.X < heroPosition.X ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            // new Vector2(position.X + 2.0f, position.Y), new Rectangle(0, 0, 0, 0), Color.White, 0.0f, new Vector2(shotgun.Width/2, shotgun.Height/2), SpriteEffects.None);

            // Draw each of the bullets fired and present in screen
            List<Bullet> bullets = EquippedWeapon.BulletsFired;
            if(bullets.Count != 0)
            {
                foreach (Bullet bullet in bullets)
                {
                    spriteBatch.Draw(EquippedWeapon.BulletTexture, bullet.position, Color.White);
                }
            }

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

            if(equippedWeaponIndex == weaponsList.Count - 1)
            {
                EquipWeapon(0);
                // equippedWeapon = weaponsList[0];
            } else
            {
                EquipWeapon(equippedWeaponIndex + 1);
                // equippedWeapon = weaponsList[equippedWeaponIndex + 1];
            }
        }

        public void PreviousWeapon()
        {

            Console.WriteLine("previous weapon");

            if (equippedWeaponIndex == 0)
            {
                EquipWeapon(weaponsList.Count - 1);
                // equippedWeapon = weaponsList[weaponsList.Count - 1];
            }
            else
            {
                EquipWeapon(equippedWeaponIndex - 1);
                // equippedWeapon = weaponsList[equippedWeaponIndex - 1];
            }
        }

        private void EquipWeapon(int weaponIndex)
        {
            equippedWeapon = weaponsList[weaponIndex];
            equippedWeaponIndex = weaponIndex;
        }

        public void ShootEquippedWeapon()
        {
            if(EquippedWeapon.CurrentMagHold < EquippedWeapon.AmmoPerShot)
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


    }
}