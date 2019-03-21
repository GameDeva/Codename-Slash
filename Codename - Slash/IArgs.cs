using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public interface IArgs
    {

    }

    public class ArgsBullet : IArgs
    {
        public Texture2D bulletTexture;
        public Vector2 position;
        public Vector2 direction;

        public ArgsBullet(Vector2 position, Vector2 direction, Texture2D bulletTexture)
        {
            this.position = position;
            this.direction = direction;
            this.bulletTexture = bulletTexture;
        }

    }

    public class ArgsEnemy : IArgs
    {
        public Vector2 position;
        // more args

        public ArgsEnemy(Vector2 position)
        {
            this.position = position;
        }

    }
}
