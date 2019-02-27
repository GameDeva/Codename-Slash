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
        
        private List<Weapon> weaponsList;
        private Weapon equippedWeapon;

        private MouseState mouseState;
        private float rotationRadius = 50.0f;
        private float rotationAngleWeapon = 0.0f;
        private Vector2 weaponPostion;
        private Vector2 directionToShoot;

        public WeaponHandler()
        {
            weaponsList = new List<Weapon>();
            

        }

        public void LoadContent(ContentManager content)
        {
            // TODO : Maybe switch texture loading to weapon class? Not sure. 
            weaponsList.Add(new MachineGun(content.Load<Texture2D>("Sprites/Weapons/Shotgun/shot_side"), content.Load<Texture2D>("Sprites/Weapons/Shotgun/bulletb")));
            equippedWeapon = weaponsList[0];
            

        }


        public void Update(Vector2 heroPosition, GameTime gameTime)
        {
            UpdateWeaponPosition(heroPosition);
            equippedWeapon.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 heroPosition)
        {
            Rectangle weaponSourceRect = new Rectangle(0, 0, equippedWeapon.WeaponTexture.Width, equippedWeapon.WeaponTexture.Height);
            
            // Rectangle shotgunDest = new Rectangle((int)(position.X + 2.0f), (int)position.Y, shotgun.Width, shotgun.Height);
            spriteBatch.Draw(equippedWeapon.WeaponTexture, weaponPostion, weaponSourceRect, Color.White, rotationAngleWeapon, new Vector2(equippedWeapon.WeaponTexture.Width / 2, equippedWeapon.WeaponTexture.Height / 2), 2.0f, mouseState.X < heroPosition.X ? SpriteEffects.FlipVertically : SpriteEffects.None, 0);
            // new Vector2(position.X + 2.0f, position.Y), new Rectangle(0, 0, 0, 0), Color.White, 0.0f, new Vector2(shotgun.Width/2, shotgun.Height/2), SpriteEffects.None);

            // Draw each of the bullets fired and present in screen
            List<Bullet> bullets = equippedWeapon.BulletsFired;
            if(bullets.Count != 0)
            {
                foreach (Bullet bullet in bullets)
                {
                    spriteBatch.Draw(equippedWeapon.BulletTexture, bullet.position, Color.White);
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

        public void ShootEquippedWeapon()
        {
            if(equippedWeapon.CurrentMagHold < equippedWeapon.AmmoPerShot)
            {
                ReloadWeapon();
            }

            equippedWeapon.Shoot(weaponPostion, directionToShoot);
            
        }

        public void ReloadWeapon()
        {
            equippedWeapon.Reload();
        }


    }
}
