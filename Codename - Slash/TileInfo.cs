using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    // Info of each tile on the grid
    public class TileInfo
    {
        public int cs;

        public string texturePath;
        public Texture2D texture;
        public Direction collision;
        public bool enemySpawnpoint;

        private static TileInfo instance;
        public static TileInfo Instance { get { if (instance == null) { instance = new TileInfo(); return instance; } return instance; } set { instance = value; } }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

    }
}
