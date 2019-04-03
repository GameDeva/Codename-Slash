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
        ObjectPool<Skull> skullPool;
        ObjectPool<Bald> baldPool;
        ObjectPool<Dark> darkPool;


        private List<Bullet> bulletsAlive;
        private List<Enemy> enemiesAlive;

        // Collider add event
        public Action<ICollidable, ColliderType> OnAddDynamicCollider;

        public void Initialise(Hero hero)
        {
            // Add hero's collider
            OnAddDynamicCollider?.Invoke(hero, hero.ColliderType);

            // TODO: Depends on how many there can be on the map
            bulletPool = new ObjectPool<Bullet>(100);
            dogePool = new ObjectPool<Doge>(10);
            skullPool = new ObjectPool<Skull>(10);
            baldPool = new ObjectPool<Bald>(10);
            darkPool = new ObjectPool<Dark>(10);

            // Attach all listeners
            hero.WeaponHandler.OnSpawnBullet += SpawnBullet;
            EnemyDirector.Instance.OnCreateDoge += SpawnDoge;
            EnemyDirector.Instance.OnCreateSkull += SpawnSkull;
            EnemyDirector.Instance.OnCreateBald += SpawnBald;
            EnemyDirector.Instance.OnCreateDark += SpawnDark;

            bulletsAlive = new List<Bullet>();
            enemiesAlive = new List<Enemy>();
        }

        public void Update(float deltaTime)
        {
            // Update all gameobjects 
            // int size = spawnedObjects.Count;
            for (int i = bulletsAlive.Count - 1; i >= 0; --i)
            {
                if (bulletsAlive[i].IsActive)
                {
                    bulletsAlive[i].Update(deltaTime);
                    
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
                    enemiesAlive[i].Update(deltaTime);

                }
                else
                {
                    enemiesAlive.RemoveAt(i);
                }
            }

        }

        public void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            // Draw all gameobjects 
            // int size = spawnedObjects.Count;
            for (int i = 0; i < bulletsAlive.Count; i++)
            {
                if (bulletsAlive[i].IsActive)
                {
                    bulletsAlive[i].Draw(deltaTime, spriteBatch);
                }
            }

            for (int i = 0; i < enemiesAlive.Count; i++)
            {
                if (enemiesAlive[i].IsActive)
                {
                    enemiesAlive[i].Draw(deltaTime, spriteBatch);
                    Game1.DrawRect(spriteBatch, enemiesAlive[i].BoundingRect);
                }
            }
        }

        
        private void SpawnBullet(IArgs args)
        {
            Bullet bullet = bulletPool.SpawnFromPool(args);
            bulletsAlive.Add(bullet);
            OnAddDynamicCollider?.Invoke(bullet, ColliderType.heroAttack);
        }

        // Enemy Spawns
        //
        private void SpawnDoge(IArgs args)
        {
            Doge doge = dogePool.SpawnFromPool(args);
            enemiesAlive.Add(doge);
            OnAddDynamicCollider?.Invoke(doge, ColliderType.enemy);
        }
        private void SpawnSkull(IArgs args)
        {
            Skull skull = skullPool.SpawnFromPool(args);
            enemiesAlive.Add(skull);
            OnAddDynamicCollider?.Invoke(skull, ColliderType.enemy);
        }
        private void SpawnBald(IArgs args)
        {
            Bald bald = baldPool.SpawnFromPool(args);
            enemiesAlive.Add(bald);
            OnAddDynamicCollider?.Invoke(bald, ColliderType.enemy);
        }
        private void SpawnDark(IArgs args)
        {
            Dark dark = darkPool.SpawnFromPool(args);
            enemiesAlive.Add(dark);
            OnAddDynamicCollider?.Invoke(dark, ColliderType.enemy);
        }
    }
}
