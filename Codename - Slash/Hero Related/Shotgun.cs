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
        
        public Shotgun(Texture2D weaponIconTexture, Texture2D weaponTexture, Texture2D bulletTexture) : base(weaponIconTexture, weaponTexture, bulletTexture)
        {
            // NOTE: Below assumes weapons are complete when created, 
            // TODO: For pickup weapons that are half empty, need to refactor
            //          to take these values as parameters
            MaximumAmmoCarry = 64;
            MaximumMagHold = 8;
            CurrentAmmoCarry = MaximumAmmoCarry;
            CurrentMagHold = MaximumMagHold;
            

        }

        public override void Shoot(Vector2 firePoint, Vector2 fireDirection)
        {
            
        }

        public override void Reload()
        {


            base.Reload();
        }

    }
}
