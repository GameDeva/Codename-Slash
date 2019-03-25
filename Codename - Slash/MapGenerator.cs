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

    // Info of each tile on the grid
    public struct TileInfo
    {
        public string texturePath;
        public Texture2D texture;
        public Direction collision;
        public bool enemySpawnpoint;

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

    }
    
    public struct Section
    {
        public string[,] tilesInfo;

        public void InitialiseSection(int sectionSize)
        {
            tilesInfo = new string[sectionSize, sectionSize];
        }

    }

    public class MapGenerator
    {
        public static Dictionary<string, TileInfo> stringToTile;

        private const int mapGridSize = 64; // Width and Height
        private const int sectionCount = 8; // Width and Height
        
        public Section[,] sectionsGrid;
        
        private int currentMapNumber;
        private Vector2Int currentHeroPos;
        private Deque<Deque<Section>> currentChunk;
        private int renderChunkSize;
        
        private List<Section> currentGridsDisplayed;

        private ContentManager content;

        public MapGenerator(IServiceProvider serivceProvider)
        {
            content = new ContentManager(serivceProvider, "Content/");

            // Create sectionGrid 2d array to reference to each section of the map
            sectionsGrid = new Section[sectionCount, sectionCount];

            stringToTile = new Dictionary<string, TileInfo>();

        }
        
        public void InitialiseNewMap(int mapNumber) // , Vector2Int heroPos, int renderChunkSize
        {
            string[,] arr = new string[64,64];
            
            // Load map into the 2d array from the csv file
            Loader.ReadCSVFileTo2DArray(string.Format("Content/Maps/Map{0}.csv", mapNumber), ref arr);
            
            // Load the appropriate tileinfo into dictionary 
            Loader.XMLToDictionary(string.Format("Content/Maps/TileInfo{0}.xml", mapNumber), ref stringToTile);
            // Load the textures for each tile into each trileInfo struct 
            foreach(string key in stringToTile.Keys)
            {
                stringToTile[key].SetTexture(content.Load<Texture2D>(stringToTile[key].texturePath));
            }

            for (int i = 0; i < mapGridSize; i+=sectionCount)
            {
                for (int j = 0; j < mapGridSize; j+=sectionCount)
                {
                    // Each subgrid loop

                    // Create new section 
                    Section s = new Section();
                    // Initalise the 2d array in seciton with sectionCount
                    s.InitialiseSection(sectionCount);
                    // Attach to the sectionsgrid on the map
                    sectionsGrid[i % sectionCount, j % sectionCount] = s;

                    for (int x = 0; x < sectionCount; x++)
                    {
                        for (int y = 0; y < sectionCount; y++)
                        {
                            // each tile in a subgrid loop


                            // Add the tile name to each subgrid from the map
                            s.tilesInfo[x, y] = arr[i + x, j + y];
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

        public void Draw(SpriteBatch spriteBatch)
        {
            // Test by drawing 1 section
            Rectangle tileRect = new Rectangle(0, 0, 1, 1);
            // TileInfo tileInfo = new TileInfo();

            Vector2 origin = new Vector2(0, 0);
            for (int x = 0; x < sectionCount; x++)
            {
                for (int y = 0; y < sectionCount; y++)
                {
                    // Get appropriate tile
                    TileInfo tileInfo = stringToTile[sectionsGrid[0, 0].tilesInfo[x,y]];

                    tileRect.X = x;
                    tileRect.Y = y;

                    spriteBatch.Draw(tileInfo.texture, tileRect, null, Color.White, 0, origin, SpriteEffects.None, 0);


                }
            }


        }




        //private void UpdateChunkInUse(Direction dir)
        //{
        //    if (currentChunk != null)
        //    {

        //    } else
        //    {
        //        // Create new chunk, then for each deque, add a deque with the relevant Sections based on the heroPos
        //        currentChunk = new Deque<Deque<Section>>();
        //        for (int i = 0; i < 3; i++)
        //        {
        //            currentChunk.Add(new Deque<Section> { currentMap.MapSections[currentHeroPos.X - 1, currentHeroPos.Y + (i-1)],
        //                                                currentMap.MapSections[currentHeroPos.X, currentHeroPos.Y + (i-1)],
        //                                                currentMap.MapSections[currentHeroPos.X + 1, currentHeroPos.Y + (i-1)] });
        //        }

        //        foreach (Deque<Section> s in currentChunk) 
        //        {
        //            foreach (Section s2 in s)
        //            {
        //                int l1 = s2.tilesInfo.GetLength(0);
        //                int l2 = s2.tilesInfo.GetLength(1);
        //                for (int i=0; i < l1; i++)
        //                {
        //                    for(int j=0; j < l2; j++)
        //                    {
                                
        //                        // TODO : Add collision for tiles
        //                    }
        //                }
        //            }
        //        }

        //    }

        //    // Update currentHeroPos based on dir

        //    // Once the sections have been choosen update the relevant colliders. 
        //    UpdateTileColiders();
        //}





        private void UpdateTileColiders()
        {

        } 


        private void LoadResource()
        {

        }
    }
}
