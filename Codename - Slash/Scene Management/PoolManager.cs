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
    // 
    public class PoolManager
    {
        // Single creation
        private static PoolManager instance;
        public static PoolManager Instance { get { if (instance == null) { instance = new PoolManager(); return instance; } return instance; } set { instance = value; } }


        ObjectPool<Bullet> bulletPool;
        ObjectPool<Doge> dogePool;

        private List<Bullet> bulletsAlive;
        private List<Enemy> enemiesAlive;

        // Collider add event
        public Action<ICollidable, ColliderType> OnAddDynamicCollider;

        public void Initialise(Hero hero)
        {
            // Add hero's collider
            OnAddDynamicCollider?.Invoke(hero, hero.ColliderType);

            bulletPool = new ObjectPool<Bullet>(100);
            dogePool = new ObjectPool<Doge>(10);

            hero.WeaponHandler.OnSpawnBullet += SpawnBullet;
            EnemyDirector.Instance.OnCreateDoge += SpawnDoge;

            bulletsAlive = new List<Bullet>();
            enemiesAlive = new List<Enemy>();
        }

        public void Update(GameTime gameTime)
        {
            // Update all gameobjects 
            // int size = spawnedObjects.Count;
            for (int i = bulletsAlive.Count - 1; i >= 0; --i)
            {
                if (bulletsAlive[i].IsActive)
                {
                    bulletsAlive[i].Update(gameTime);
                    
                }
                else
                {
                    bulletsAlive.RemoveAt(i);
                }
            }

            for (int i = enemiesAlive.Count - 1; i >= 0; --i)
            {
                if (enemiesAlive[i].IsActive)
                {
                    enemiesAlive[i].Update(gameTime);

                }
                else
                {
                    enemiesAlive.RemoveAt(i);
                }
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw all gameobjects 
            // int size = spawnedObjects.Count;
            for (int i = 0; i < bulletsAlive.Count; i++)
            {
                if (bulletsAlive[i].IsActive)
                {
                    bulletsAlive[i].Draw(gameTime, spriteBatch);
                }
            }

            for (int i = 0; i < enemiesAlive.Count; i++)
            {
                if (enemiesAlive[i].IsActive)
                {
                    enemiesAlive[i].Draw(gameTime, spriteBatch);
                }
            }
        }

        
        private void SpawnBullet(IArgs args)
        {
            Bullet bullet = bulletPool.SpawnFromPool(args);
            bulletsAlive.Add(bullet);
            OnAddDynamicCollider?.Invoke(bullet, ColliderType.heroAttack);
        }

        private void SpawnDoge(IArgs args)
        {
            Doge doge = dogePool.SpawnFromPool(args);
            enemiesAlive.Add(doge);
            OnAddDynamicCollider?.Invoke(doge, ColliderType.enemy);
        }

    }
}
