using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Codename___Slash.EnemyStates;
using Codename___Slash.Collisions;

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
        ObjectPool<Effect> effectPool;

        // Local lists of bullets and enemies that are active/alive
        private List<Bullet> bulletsAlive;
        private List<Enemy> enemiesAlive;
        private List<Effect> effectsAlive;

        // Collider events to add, remove 
        public Action<ICollidable> OnAddCollider;
        public Action<ICollidable> OnRemoveCollider;
        public Action<ColliderType> OnRemoveAllCollidersOfType;

        // Enemy death events
        public Action<Enemy> OnDeath;

        public void Initialise()
        {
            EnemyDirector.Instance.OnCreateDoge += SpawnDoge;
            EnemyDirector.Instance.OnCreateSkull += SpawnSkull;
            EnemyDirector.Instance.OnCreateBald += SpawnBald;
            EnemyDirector.Instance.OnCreateDark += SpawnDark;

            EnemyDirector.Instance.createEffect += SpawnEffect;
            
            bulletsAlive = new List<Bullet>();
            enemiesAlive = new List<Enemy>();
            effectsAlive = new List<Effect>();
        }

        public void ReInitialise(Hero hero)
        {
            // Add hero's collider
            OnAddCollider?.Invoke(hero);
            
            // Create pools that are same for each stage
            bulletPool = new ObjectPool<Bullet>(30);
            effectPool = new ObjectPool<Effect>(5);

            // Attach all listeners
            hero.WeaponHandler.OnSpawnBullet += SpawnBullet;

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
        public void ClearPoolsForNextStage()
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

        public void ClearAllPools()
        {
            enemiesAlive.Clear();
            bulletsAlive.Clear();
            effectsAlive.Clear();

            bulletPool = null;
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

            for (int i = effectsAlive.Count - 1; i >= 0; --i)
            {
                if (effectsAlive[i].IsActive)
                {
                    effectsAlive[i].Update(deltaTime);
                }
                else
                {
                    // If note active, remove from list
                    effectsAlive.RemoveAt(i);
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

            for (int i = 0; i < effectsAlive.Count; i++)
            {
                if (effectsAlive[i].IsActive)
                {
                    effectsAlive[i].Draw(deltaTime, spriteBatch);
                }
            }
        }

        // Calls ondeath action, with the enemy object and calls the death effect method at given position
        private void OnEnemyDeath(Enemy enemy, Vector2 position)
        {
            OnDeath?.Invoke(enemy);
            // SpawnDeathEffect(position);
        }
        
        // Spawns with given arguments and adds a collider
        public void SpawnBullet(IArgs args)
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
            doge.OnDamage += OnEnemyHit;
            doge.OnDeath += OnEnemyDeath;
            OnAddCollider?.Invoke(doge);
        }
        private void SpawnSkull(IArgs args)
        {
            Skull skull = skullPool.SpawnFromPool(args);
            enemiesAlive.Add(skull);
            skull.OnDamage += OnEnemyHit;
            skull.OnDeath += OnEnemyDeath;
            OnAddCollider?.Invoke(skull);
        }
        private void SpawnBald(IArgs args)
        {
            Bald bald = baldPool.SpawnFromPool(args);
            enemiesAlive.Add(bald);
            bald.OnDamage += OnEnemyHit;
            bald.OnDeath += OnEnemyDeath;
            OnAddCollider?.Invoke(bald);
        }
        private void SpawnDark(IArgs args)
        {
            Dark dark = darkPool.SpawnFromPool(args);
            enemiesAlive.Add(dark);
            dark.OnDamage += OnEnemyHit;
            dark.OnDeath += OnEnemyDeath;
            OnAddCollider?.Invoke(dark);
        }
        
        // On enemy takes hit
        private void OnEnemyHit(Vector2 pos)
        {
            // EnemyDirector.Instance.OnEnemyHit(pos);
        }

        // Spawns effect at position
        private void SpawnEffect(IArgs args)
        {
            Effect effect = effectPool.SpawnFromPool(args);
            effectsAlive.Add(effect);
        }
    }
}
