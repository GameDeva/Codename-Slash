/* Previous Idea, now depracated, since I am using new MapGen System.
 * 
 */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework;

//namespace Codename___Slash
//{
//    // Collision indicator for each tile
//    // And direction of map update 
//    public enum Direction
//    {
//        Left,   // only for left half of tile
//        Right,  // only for right half of tile
//        Top,    // only for top half of tile
//        Bottom, // only for bottom half of tile
//        Full,   // for entire tile
//        None
//    }

//    // Info of each tile on the grid
//    public class TileInfo
//    {
//        // public int cs;
//        public string name;
//        public string texturePath;
//        // public Texture2D Texture { get; set; }
//        public bool collision;

//        //private static TileInfo instance = null;
//        //public static TileInfo Instance { get { if (instance == null) { instance = new TileInfo(); return instance; } return instance; } set { instance = value; } }

//        public void SetTexture(Texture2D texture)
//        {
//            // this.texture = texture;
//        }

//    }

//    public struct Section
//    {
//        public string[,] sectionTiles;

//        public void InitialiseSection(int sectionSize)
//        {
//            sectionTiles = new string[sectionSize, sectionSize];
//        }

//    }

//    public class MapGenerator
//    {
//        // Single creation
//        private static MapGenerator instance;
//        public static MapGenerator Instance { get { if (instance == null) { instance = new MapGenerator(); return instance; } return instance; } set { instance = value; } }


//        public static Dictionary<string, TileInfo> stringToTile;
//        public Dictionary<string, Texture2D> stringToTexture;

//        private const int mapGridSize = 64; // Width and Height
//        private const int TilesInSection = 8; // Width and Height
//        private const int tileSize = 50; // Width and Height

//        public Section[,] mapSections;
//        string[,] arr;

//        //private int currentMapNumber;
//        //private Vector2Int currentHeroPos;
//        //private Deque<Deque<Section>> currentChunk;
//        //private int renderChunkSize;

//        //private List<Section> currentGridsDisplayed;

//        private ContentManager content;

//        // Collider add event
//        public Action<ICollidable, ColliderType> OnAddStaticCollider;

//        public void Initialise(IServiceProvider serivceProvider)
//        {
//            content = new ContentManager(serivceProvider, "Content/");

//            // Create sectionGrid 2d array to reference to each section of the map
//            mapSections = new Section[TilesInSection, TilesInSection];

//            stringToTile = new Dictionary<string, TileInfo>();
//            stringToTexture = new Dictionary<string, Texture2D>();
//        }
        
//        public void InitialiseNewMap(int mapNumber) // , Vector2Int heroPos, int renderChunkSize
//        {
//            arr = new string[mapGridSize,mapGridSize];
            
//            // Load map into the 2d array from the csv file
//            Loader.ReadCSVFileTo2DArray(string.Format("Content/Maps/Map{0}.csv", mapNumber), ref arr);

//            // List<TileInfo> tileInfoList = new List<TileInfo>();
//            // TileInfo grassInfo = new TileInfo();
//            // Test
//            // Loader.ReadXML(string.Format("Content/Maps/TileInfo{0}.xml", mapNumber), ref grassInfo);
//            // stringToTexture.Add("Grass", content.Load<Texture2D>(grassInfo.texturePath));
//            GameInfo info = GameInfo.Instance;

//            Loader.ReadXML(string.Format("Content/TileInfo{0}.xml", mapNumber), ref info);

//            // Load the appropriate tileinfo into dictionary 
//            int dicSize = info.tileInfoList.Count();
//            for(int i = 0; i < dicSize; i++)
//            {
//                stringToTexture.Add(info.tileInfoList[i].name, content.Load<Texture2D>(info.tileInfoList[i].texturePath));
//            }


//            // Loader.XMLToDictionary(string.Format("Content/Maps/TileInfo{0}.xml", mapNumber), ref stringToTile);
//            // Load the textures for each tile into each trileInfo struct 
//            foreach (string key in stringToTile.Keys)
//            {
//                // TODO : add back in
//                // stringToTile[key].Texture = content.Load<Texture2D>(stringToTile[key].texturePath);
//            }

//            // Loop through the mapsize, with increments of the number tiles wide/high in each section
//            for (int j = 0; j < mapGridSize; j += TilesInSection)
//            {
//                for (int i = 0; i < mapGridSize; i+=TilesInSection)
//                {
//                    // Each subgrid loop
//                    //

//                    // Create new section 
//                    Section s = new Section();
//                    // Initalise the 2d array in seciton with sectionCount
//                    s.InitialiseSection(TilesInSection);

//                    for (int y = 0; y < TilesInSection; y++)
//                    {
//                        for (int x = 0; x < TilesInSection; x++)
//                        {
//                            // each tile in a subgrid loop
//                            //

//                            // Add the tile name to each subgrid from the map
//                            s.sectionTiles[x, y] = arr[i + x, j + y];
//                        }
//                    }
                    
//                    // Attach to the sectionsgrid on the map
//                    mapSections[i / TilesInSection, j / TilesInSection] = s;
//                }
//            }

//                    //// Reinitialise values when creating a new map, or newly assign if first time
//                    //ResetHeroPosition(heroPos);
//                    //currentMapNumber = MapNumber;
//                    //currentMap = new Map(1, 8);
//                    //currentGridsDisplayed = new List<Section>();
//                    //currentChunk = null;
//                    //this.renderChunkSize = renderChunkSize;

//                    //// Read XML map data onto the new Map object, based on mapNumber
//                    //// Should update Section Values with TilesInfo
//                    //Loader.ReadXML(string.Format("Content/Maps/{0}.xml", MapNumber), ref currentMap);
//                }

//        public void Draw(SpriteBatch spriteBatch)
//        {
//            // Test by drawing 1 section
//            Rectangle tileRect = new Rectangle(0, 0, tileSize, tileSize);
//            // TileInfo tileInfo = new TileInfo();
            
//            Vector2 origin = new Vector2(0, 0);


//            for (int i = 0; i < 512; i++)
//            {
//                for (int j = 0; j < 512; j++)
//                {
//                    // Get appropriate tile's texture
//                    Texture2D tex = stringToTexture[arr[i, j]];

//                    // Draw texture in appropriate position
//                    spriteBatch.Draw(tex, tileRect, null, Color.Red, 0, origin, SpriteEffects.None, 0);
//                    tileRect.X += tileSize;
//                }
//                tileRect.X = 0;
//                tileRect.Y += tileSize;
//            }


//            //for (int i = 0; i < 8; i++)
//            //{
//            //    for (int j = 0; j < 8; j++)
//            //    {


//            //        for (int y = 0; y < TilesInSection; y++)
//            //        {
//            //            for (int x = 0; x < TilesInSection; x++)
//            //            {
//            //                // Get appropriate tile's texture
//            //                Texture2D tex = stringToTexture[mapSections[i, j].sectionTiles[y, x]];

//            //                // Draw texture in appropriate position
//            //                spriteBatch.Draw(tex, tileRect, null, Color.White, 0, origin, SpriteEffects.None, 0);
//            //                tileRect.X += tileSize;
//            //            }
//            //            tileRect.X = i * tileSize * TilesInSection;
//            //            tileRect.Y += tileSize;
//            //        }
//            //        tileRect.Y = j * tileSize * TilesInSection;
//            //    }
//            //}
//        }




//        //private void UpdateChunkInUse(Direction dir)
//        //{
//        //    if (currentChunk != null)
//        //    {

//        //    } else
//        //    {
//        //        // Create new chunk, then for each deque, add a deque with the relevant Sections based on the heroPos
//        //        currentChunk = new Deque<Deque<Section>>();
//        //        for (int i = 0; i < 3; i++)
//        //        {
//        //            currentChunk.Add(new Deque<Section> { currentMap.MapSections[currentHeroPos.X - 1, currentHeroPos.Y + (i-1)],
//        //                                                currentMap.MapSections[currentHeroPos.X, currentHeroPos.Y + (i-1)],
//        //                                                currentMap.MapSections[currentHeroPos.X + 1, currentHeroPos.Y + (i-1)] });
//        //        }

//        //        foreach (Deque<Section> s in currentChunk) 
//        //        {
//        //            foreach (Section s2 in s)
//        //            {
//        //                int l1 = s2.tilesInfo.GetLength(0);
//        //                int l2 = s2.tilesInfo.GetLength(1);
//        //                for (int i=0; i < l1; i++)
//        //                {
//        //                    for(int j=0; j < l2; j++)
//        //                    {
                                
//        //                        // TODO : Add collision for tiles
//        //                    }
//        //                }
//        //            }
//        //        }

//        //    }

//        //    // Update currentHeroPos based on dir

//        //    // Once the sections have been choosen update the relevant colliders. 
//        //    UpdateTileColiders();
//        //}





//        private void UpdateTileColiders()
//        {

//        } 


//        private void LoadResource()
//        {

//        }
//    }
//}
