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
    }

    // Info of each tile on the grid
    public struct TileInfo
    {
        public int textureIndex;
        public Direction collision;

    }
    
    public struct Section
    {
        public TileInfo[,] tilesInfo;
        
        public Section(int sectionSize)
        {
            tilesInfo = new TileInfo[sectionSize, sectionSize];
            // TODO : 
        }

    }

    public class Map
    {
        public Section[,] MapSections { get; }
        public Dictionary<int, string> IndexToTextureFile { get; }

        private int mapNumber;
        private int mapSize;

        public Map(int mapSize)
        {
            // Assign parameter values
            this.mapSize = mapSize;
            MapSections = new Section[mapSize, mapSize];
            IndexToTextureFile = new Dictionary<int, string>();
        }


    }


    public class MapGenerator
    {
        private Map currentMap;
        private int currentMapNumber;
        private Vector2Int currentHeroPos;
        private Deque<Deque<Section>> currentChunk;
        private int renderChunkSize;
        private Dictionary<int, Texture2D> IndexToTextureLoaded; // Indexes of the tiles currently loaded from content manager (Used to check to not load again)

        private List<Section> currentGridsDisplayed;

        private ContentManager content;

        public MapGenerator(IServiceProvider serivceProvider)
        {
            content = new ContentManager(serivceProvider, "Content/Maps");
        }

        
        public void InitialiseNewMap(int MapNumber, int mapSize, Vector2Int heroPos, int renderChunkSize)
        {
            // Reinitialise values when creating a new map, or newly assign if first time
            currentMapNumber = MapNumber;
            currentMap = new Map(mapSize);
            currentGridsDisplayed = new List<Section>();
            currentChunk = null;
            currentHeroPos = heroPos;
            this.renderChunkSize = renderChunkSize;

            // Read XML map data onto the new Map object, based on mapNumber
            // Should update Section Values with TilesInfo
            Loader.ReadXML(string.Format("Content/Maps/{0}.txt", MapNumber), ref currentMap);
        }

        public void UpdateMapDisplay(Direction dir)
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
                                int index = s2.tilesInfo[i, j].textureIndex;

                                if (!IndexToTextureLoaded.ContainsKey(index))
                                    IndexToTextureLoaded[index] = content.Load<Texture2D>(string.Format("{0}/Tiles", currentMapNumber));
                                
                                // TODO : Add collision for tiles
                            }
                        }
                    }
                }

            }

            // Update currentHeroPos based on dir
            

        }

        private void LoadResource()
        {

        }


    }
}
