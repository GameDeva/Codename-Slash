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
        public Texture2D WeaponTexture { get; private set; }

        public Weapon(Texture2D weaponTexture)
        {
            WeaponTexture = weaponTexture;

        }

        public virtual void Shoot() { }
        public virtual void Reload() { }

    }
}
