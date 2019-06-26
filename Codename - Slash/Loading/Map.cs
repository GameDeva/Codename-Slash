using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    // Container of data for a single map
    public struct Map
    {
        // 2D array of strings representing a tile name/key
        public string[,] TileMapValues { get; set; }
        // MapSize in either direction
        private int mapSizeX;
        private int mapSizeY;

        // Dictionary of each unique texture to be used to paint the map
        public Dictionary<string, Texture2D> textureDictionary;

        // 
        public Map(int mapSizeX, int mapSizeY)
        {
            this.mapSizeX = mapSizeX;
            this.mapSizeY = mapSizeY;
            TileMapValues = new string[mapSizeX, mapSizeY];

            textureDictionary = new Dictionary<string, Texture2D>();
        }
        
    }
}
