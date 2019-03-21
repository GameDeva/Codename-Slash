using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    // Collision indicator for each tile
    // And direction of map update 
    public enum Direction
    {
        Left,   // only for left half of tile
        Right,  // only for right half of tile
        Top,    // only for top half of tile
        Bottom, // only for bottom half of tile
        Full,   // for entire tile
        None
    }

    public enum TileName
    {
        Grass,
        Dirt,
    }

    // Info of each tile on the grid
    public struct TileInfo
    {
        public TileName name;
        public string textureIndex;
        public Direction collision;
        public bool enemySpawnpoint;
    }
    
    public struct Section
    {
        public TileName[,] tilesInfo;

        public void InitialiseSection(int sectionSize)
        {
            tilesInfo = new TileName[sectionSize, sectionSize];
        }

    }

    public class Map
    {
        public string[][] mapStrings;
        public Section[,] MapSections { get; }
        public TileInfo[] tileInfoList;

        public Map(int mapSize, int sectionSize)
        {
            // Assign parameter values
            MapSections = new Section[mapSize, mapSize];

            // Load map values to 2d string array
            Loader.ReadCSVFileTo2DArray("Map1", ref mapStrings);

            // Initialise each section
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    MapSections[i, j].InitialiseSection(sectionSize);
                }
            }

        }

        public void LoadMapTextures(ContentManager content)
        {
            
        }


    }


    public class MapGenerator
    {
        private const int mapGridSize = 64; // Width and Height
        private const int sectionCount = 8; // Width and Height
        public TileInfo[] currentTileInfoList;

        public Section[,] sectionsGrid;

        private Map currentMap;
        private int currentMapNumber;
        private Vector2Int currentHeroPos;
        private Deque<Deque<Section>> currentChunk;
        private int renderChunkSize;
        
        private List<Section> currentGridsDisplayed;

        private ContentManager content;

        public MapGenerator(IServiceProvider serivceProvider)
        {
            content = new ContentManager(serivceProvider, "Content/Maps");

            sectionsGrid = new Section[sectionCount, sectionCount];
        }
        
        public void InitialiseNewMap(int mapNumber) // , Vector2Int heroPos, int renderChunkSize
        {
            string[,] arr = new string[64,64];
            
            Loader.ReadCSVFileTo2DArray(string.Format("Map{0}.csv", mapNumber), ref arr);
            Loader.ReadXML(string.Format("TileInfo{0}.xml", mapNumber), ref currentTileInfoList);

            for (int i = 0; i < mapGridSize; i+=sectionCount)
            {
                for (int j = 0; j < mapGridSize; j+=sectionCount)
                {
                    // Each subgrid loop
                    // sectionsGrid[i%sectionCount,j%sectionCount]

                    for(int x = 0; x < sectionCount; x++)
                    {
                        for (int y = 0; y < sectionCount; y++)
                        {
                            // each tile in a subgrid loop
                            // arr[i+x, j+y].

                        }
                    }

                }
            }





            //// Reinitialise values when creating a new map, or newly assign if first time
            //ResetHeroPosition(heroPos);
            //currentMapNumber = MapNumber;
            //currentMap = new Map(1, 8);
            //currentGridsDisplayed = new List<Section>();
            //currentChunk = null;
            //this.renderChunkSize = renderChunkSize;

            //// Read XML map data onto the new Map object, based on mapNumber
            //// Should update Section Values with TilesInfo
            //Loader.ReadXML(string.Format("Content/Maps/{0}.xml", MapNumber), ref currentMap);
        }

        public void ResetHeroPosition(Vector2Int heroPos)
        {
            currentHeroPos = heroPos;


        }


        private void UpdateChunkInUse(Direction dir)
        {
            if (currentChunk != null)
            {

            } else
            {
                // Create new chunk, then for each deque, add a deque with the relevant Sections based on the heroPos
                currentChunk = new Deque<Deque<Section>>();
                for (int i = 0; i < 3; i++)
                {
                    currentChunk.Add(new Deque<Section> { currentMap.MapSections[currentHeroPos.X - 1, currentHeroPos.Y + (i-1)],
                                                        currentMap.MapSections[currentHeroPos.X, currentHeroPos.Y + (i-1)],
                                                        currentMap.MapSections[currentHeroPos.X + 1, currentHeroPos.Y + (i-1)] });
                }

                foreach (Deque<Section> s in currentChunk) 
                {
                    foreach (Section s2 in s)
                    {
                        int l1 = s2.tilesInfo.GetLength(0);
                        int l2 = s2.tilesInfo.GetLength(1);
                        for (int i=0; i < l1; i++)
                        {
                            for(int j=0; j < l2; j++)
                            {
                                
                                // TODO : Add collision for tiles
                            }
                        }
                    }
                }

            }

            // Update currentHeroPos based on dir

            // Once the sections have been choosen update the relevant colliders. 
            UpdateTileColiders();
        }

        private void UpdateTileColiders()
        {

        } 


        private void LoadResource()
        {

        }


    }
}
