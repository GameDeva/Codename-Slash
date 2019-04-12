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
        
        // ObjectPools for 
        ObjectPool<Bullet> bulletPool;
        ObjectPool<Doge> dogePool;
        ObjectPool<Skull> skullPool;
        ObjectPool<Bald> baldPool;
        ObjectPool<Dark> darkPool;

        // Local lists of bullets and enemies that are active/alive
        private List<Bullet> bulletsAlive;
        private List<Enemy> enemiesAlive;

        // Collider events to add, remove 
        public Action<ICollidable> OnAddCollider;
        public Action<ICollidable> OnRemoveCollider;
        public Action<ColliderType> OnRemoveAllCollidersOfType;

        // Enemy death events
        public Action<Enemy> OnDeath;

        public void Initialise(Hero hero)
        {
            // Add hero's collider
            OnAddCollider?.Invoke(hero);
            
            bulletPool = new ObjectPool<Bullet>(100);
            
            // Attach all listeners
            hero.WeaponHandler.OnSpawnBullet += SpawnBullet;
            EnemyDirector.Instance.OnCreateDoge += SpawnDoge;
            EnemyDirector.Instance.OnCreateSkull += SpawnSkull;
            EnemyDirector.Instance.OnCreateBald += SpawnBald;
            EnemyDirector.Instance.OnCreateDark += SpawnDark;

            bulletsAlive = new List<Bullet>();
            enemiesAlive = new List<Enemy>();
        }

        // Create enemy pools based on the maximum count at any one time during a stage
        public void CreateStageSpecificPools(StageData stageData)
        {
            if(stageData.maxDogeCount > 0)
                dogePool = new ObjectPool<Doge>(stageData.maxDogeCount);
            if (stageData.maxSkullCount > 0)
                skullPool = new ObjectPool<Skull>(stageData.maxSkullCount);
            if (stageData.maxBaldCount > 0)
                baldPool = new ObjectPool<Bald>(stageData.maxBaldCount);
            if (stageData.maxDarkCount > 0)
                darkPool = new ObjectPool<Dark>(stageData.maxDarkCount);
        }

        // Delete pools 
        public void DeleteStageSpecificPools()
        {
            //// Remove all colliders
            //OnRemoveAllCollidersOfType?.Invoke(ColliderType.enemy);

            // Remove all from lists
            enemiesAlive.Clear();
            dogePool = null;
            skullPool = null;
            baldPool = null;
            darkPool = null;
            
        }

        // Update all active objects from each of the pools
        public void Update(float deltaTime)
        {
            // Loop in reverse since elements are moved from list

            for (int i = bulletsAlive.Count - 1; i >= 0; --i)
            {
                if (bulletsAlive[i].IsActive)
                {
                    bulletsAlive[i].Update(deltaTime);
                }
                else
                {
                    // If note active, remove collider and remove from list
                    OnRemoveCollider?.Invoke(bulletsAlive[i]);
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
                    // If note active, remove collider and remove from list
                    OnRemoveCollider?.Invoke(enemiesAlive[i]);
                    enemiesAlive.RemoveAt(i);
                }
            }

        }

        // Draw each active object in the pools
        public void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bulletsAlive.Count; i++)
            {
                if (bulletsAlive[i].IsActive)
                {
                    bulletsAlive[i].Draw(deltaTime, spriteBatch);
                    // Game1.DrawRect(spriteBatch, bulletsAlive[i].BoundingRect); // For Debugging
                }
            }

            for (int i = 0; i < enemiesAlive.Count; i++)
            {
                if (enemiesAlive[i].IsActive)
                {
                    enemiesAlive[i].Draw(deltaTime, spriteBatch);
                    // Game1.DrawRect(spriteBatch, enemiesAlive[i].BoundingRect); // For Debugging
                }
            }
        }

        // Calls ondeath action, with the enemy object and calls the death effect method at given position
        private void OnEnemyDeath(Enemy enemy, Vector2 position)
        {
            OnDeath?.Invoke(enemy);
            SpawnDeathEffect(position);
        }
        
        // Spawns with given arguments and adds a collider
        private void SpawnBullet(IArgs args)
        {
            Bullet bullet = bulletPool.SpawnFromPool(args);
            bulletsAlive.Add(bullet);
            OnAddCollider?.Invoke(bullet);
        }

        // Enemy Spawns for each type
        // 
        // Each doing: 1) Spawning from pool with given arguments 2) adding to aliveList 3) attaching damage and death actions 4) Add collider
        // 
        private void SpawnDoge(IArgs args)
        {
            Doge doge = dogePool.SpawnFromPool(args);
            enemiesAlive.Add(doge);
            doge.OnDamage += SpawnEnemyHitEffect;
            doge.OnDeath += OnEnemyDeath;
            OnAddCollider?.Invoke(doge);
        }
        private void SpawnSkull(IArgs args)
        {
            Skull skull = skullPool.SpawnFromPool(args);
            enemiesAlive.Add(skull);
            skull.OnDamage += SpawnEnemyHitEffect;
            skull.OnDeath += OnEnemyDeath;
            OnAddCollider?.Invoke(skull);
        }
        private void SpawnBald(IArgs args)
        {
            Bald bald = baldPool.SpawnFromPool(args);
            enemiesAlive.Add(bald);
            bald.OnDamage += SpawnEnemyHitEffect;
            bald.OnDeath += OnEnemyDeath;
            OnAddCollider?.Invoke(bald);
        }
        private void SpawnDark(IArgs args)
        {
            Dark dark = darkPool.SpawnFromPool(args);
            enemiesAlive.Add(dark);
            dark.OnDamage += SpawnEnemyHitEffect;
            dark.OnDeath += OnEnemyDeath;
            OnAddCollider?.Invoke(dark);
        }
        
        // Spawns hit effect at position
        private void SpawnEnemyHitEffect(Vector2 position)
        {
            
        }

        // Spawns death effect at position
        private void SpawnDeathEffect(Vector2 position)
        {

        }
    }
}
