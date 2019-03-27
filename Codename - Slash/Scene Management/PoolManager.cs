using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Codename___Slash.EnemyStates;

namespace Codename___Slash
{
    public class PoolManager
    {
        ObjectPool<Bullet> bulletPool;
        ObjectPool<Doge> dogePool;

        private List<GameObject> spawnedObjects;

        public void Initialise(Hero hero)
        {

            bulletPool = new ObjectPool<Bullet>(100);
            dogePool = new ObjectPool<Doge>(10);

            hero.WeaponHandler.OnSpawnBullet += SpawnBullet;
            EnemyDirector.Instance.OnCreateDoge += SpawnDoge;

            spawnedObjects = new List<GameObject>();
        }

        public void Update(GameTime gameTime)
        {
            // Update all gameobjects 
            // int size = spawnedObjects.Count;
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                if (spawnedObjects[i].IsActive)
                {
                    spawnedObjects[i].Update(gameTime);
                }
                else
                {
                    spawnedObjects.RemoveAt(i);
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw all gameobjects 
            // int size = spawnedObjects.Count;
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                if (spawnedObjects[i].IsActive)
                {
                    spawnedObjects[i].Draw(gameTime, spriteBatch);
                }
            }
        }

        
        private void SpawnBullet(IArgs args)
        {
            spawnedObjects.Add(bulletPool.SpawnFromPool(args));
        }

        private void SpawnDoge(IArgs args)
        {
            spawnedObjects.Add(dogePool.SpawnFromPool(args));
        }

    }
}
