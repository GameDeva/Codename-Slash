using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public struct Map
    {
        public string[,] TileMapValues { get; set; }
        private int mapSizeX;
        private int mapSizeY;

        public Dictionary<string, Texture2D> textureDictionary;

        public Map(int mapSizeX, int mapSizeY)
        {
            this.mapSizeX = mapSizeX;
            this.mapSizeY = mapSizeY;
            TileMapValues = new string[mapSizeX, mapSizeY];

            textureDictionary = new Dictionary<string, Texture2D>();
        }


    }
}
