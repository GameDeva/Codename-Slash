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

        }

    }

    public class Map
    {
        public Section[,] MapSections { get; }
        public Dictionary<int, string> IndexToTextureFile { get; }

        private List<Texture2D> textureList;
        private int textureCount;
        private int mapNumber;
        private int mapSize;

        public Map()
        {
            // Assign parameter values
            MapSections = new Section[mapSize, mapSize];
            IndexToTextureFile = new Dictionary<int, string>();
        }

        public void LoadMapTextures(ContentManager content)
        {
            foreach (KeyValuePair<int, string> pair in IndexToTextureFile)
            {
                content.Load<Texture2D>(pair.Value);
            }
            
        }


    }


    public class MapGenerator
    {
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
        }
        
        public void InitialiseNewMap(int MapNumber, Vector2Int heroPos, int renderChunkSize)
        {
            // Reinitialise values when creating a new map, or newly assign if first time
            ResetHeroPosition(heroPos);
            currentMapNumber = MapNumber;
            currentMap = new Map();
            currentGridsDisplayed = new List<Section>();
            currentChunk = null;
            this.renderChunkSize = renderChunkSize;

            // Read XML map data onto the new Map object, based on mapNumber
            // Should update Section Values with TilesInfo
            Loader.ReadXML(string.Format("Content/Maps/{0}.xml", MapNumber), ref currentMap);
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
                                int index = s2.tilesInfo[i, j].textureIndex;
                                
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
