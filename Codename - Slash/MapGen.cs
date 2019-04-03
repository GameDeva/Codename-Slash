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
    public class MapGen
    {
        // Single creation
        private static MapGen instance;
        public static MapGen Instance { get { if (instance == null) { instance = new MapGen(); return instance; } return instance; } set { instance = value; } }

        // Values used for this game
        private const int mapGridSizeX = 60; // Width 
        private const int mapGridSizeY = 32; // Height
        private const int tileSize = 32; 

        private ContentManager content;
        private Dictionary<string, Map> mapDictionary;

        private string currentMapToDraw;
        public MapCollider CurrentMapColliderType { get; private set; }

        // Collider add event
        public Action<ICollidable, ColliderType> OnAddcollider; // Either staticCollider or trigger region created in this class
        public Action OnRemoveAllStaticColliders;
        public Action OnRemoveAllTriggerRegions;
        
        public void Initialise(IServiceProvider serivceProvider)
        {
            content = new ContentManager(serivceProvider, "Content/");

            mapDictionary = new Dictionary<string, Map>();
        }

        // Get map info file, and add map details to 2d array and into map dictionary 
        public void GetMapData(string mapName)
        {
            // Load data into array
            string[,] arr = new string[mapGridSizeX, mapGridSizeY];
            Loader.ReadCSVFileTo2DArray(string.Format("Content/Maps/{0}.csv", mapName), ref arr);
            
            // Create map and copy over data
            Map m = new Map(mapGridSizeX, mapGridSizeY);
            m.TileMapValues = arr;
            
            // Add to dictionary
            mapDictionary.Add(mapName, m);
        }

        // Loads the textures needed to draw a given map
        public void LoadMapTextures(string mapName)
        {
            // Only if the map has been added to the dictionary
            if(mapDictionary.ContainsKey(mapName))
            {
                Dictionary<string, Texture2D> stringToTex = mapDictionary[mapName].textureDictionary;
                string[,] map = mapDictionary[mapName].TileMapValues;
                foreach (string tile in map)
                {
                    // Check if already loaded texture
                    if(!stringToTex.ContainsKey(tile))
                    {
                        // Load texture and add based on namecode on tile
                        stringToTex.Add(tile, content.Load<Texture2D>(string.Format("Sprites/TileTextures/tile{0}", tile)));
                    }
                }
            }

        }

        public void AssignMapToDraw(string mapName)
        {
            currentMapToDraw = mapName;
        }

        public void DrawMap(SpriteBatch spriteBatch)
        {
            // Test by drawing 1 section
            Rectangle tileRect = new Rectangle(0, 32, tileSize, tileSize);
            // TileInfo tileInfo = new TileInfo();

            Vector2 origin = new Vector2(0, 0);

            Map map = mapDictionary[currentMapToDraw];

            for (int y = 0; y < mapGridSizeY; y++)
            {
                for (int x = 0; x < mapGridSizeX; x++)
                {
                    // Get appropriate tile's texture
                    Texture2D tex = map.textureDictionary[map.TileMapValues[y, x]];

                    // Draw texture in appropriate position
                    spriteBatch.Draw(tex, tileRect, null, Color.White, 0, origin, SpriteEffects.None, 0);
                    tileRect.X += tileSize;
                }
                tileRect.X = 0;
                tileRect.Y += tileSize;
            }

        }

        // Change the static colliders of the scene/map
        public void ChangeMapColliders(MapCollider colliderType)
        {
            //// If not changing the type and not first time, return
            //if (currentMapColliderType == colliderType)
            //    return;
                        
            //switch (colliderType)
            //{
            //    case MapCollider.BattleArena:
            //        break;
            //    case MapCollider.BattleArenaExitOpen:

            //        break;
            //    case MapCollider.Walkway:

            //        break;

            //}

            CurrentMapColliderType = colliderType;

        }
    }

    // Since the game features limited maps, there will only be 3 types of static collider setups
    // Todo: For expandability, add a system that creates large rectangle colliders using data for each map
    public enum MapCollider
    {
        Walkway, 
        BattleArena,
        BattleArenaExitOpen
    }
}
