using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Codename___Slash
{
    // Collision indicator for each tile
    enum TileCollision
    {
        Left,   // Collisions only for left half of tile
        Right,  // Collisions only for right half of tile
        Top,    // Collisions only for top half of tile
        Bottom, // Collisions only for bottom half of tile
        Full,   // Collisions for entire tile
    }

    // Info of each tile on the grid
    struct TileInfo
    {
        public int textureIndex;
        public TileCollision collision;

    }
    
    struct SectionGrid
    {
        public TileInfo[,] textureIndexMap;
        
        public SectionGrid(int gridSize)
        {
            textureIndexMap = new TileInfo[gridSize, gridSize];

        }

    }

    class Map
    {
        private int mapNumber;
        private int mapSize;
        private SectionGrid[,] mapSections;
        private Dictionary<int, Texture2D> currentIndexToTexDic;

        public Map(int mapSize)
        {
            // Assign parameter values
            this.mapSize = mapSize;
            mapSections = new SectionGrid[mapSize, mapSize];
            currentIndexToTexDic = new Dictionary<int, Texture2D>();
        }


    }


    class MapGenerator
    {
        private Map currentMap;
        private int currentMapNumber;
        
        private ContentManager content;

        public MapGenerator(IServiceProvider serivceProvider)
        {
            content = new ContentManager(serivceProvider);
        }

        
        public void InitialiseNewMap(int MapNumber, int mapSize)
        {
            // Reinitialise values when creating a new map, or newly assign if first time
            currentMapNumber = MapNumber;
            currentMap = new Map(mapSize);

            // Read XML map data onto the new Map object, based on mapNumber
            Loader.ReadXML(string.Format("Content/Levels/{0}.txt", MapNumber), ref currentMap);
        }

        public void UpdateMapDisplay()
        {


        }

        private void LoadResource()
        {

        }


    }
}
