﻿using System;
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

        private StageManager stageManager;

        ObjectPool<Bullet> bulletPool;
        ObjectPool<Doge> dogePool;
        ObjectPool<Skull> skullPool;
        ObjectPool<Bald> baldPool;
        ObjectPool<Dark> darkPool;


        private List<Bullet> bulletsAlive;
        private List<Enemy> enemiesAlive;

        // Collider add event
        public Action<ICollidable> OnAddCollider;

        public void Initialise(Hero hero)
        {
            //
            stageManager = StageManager.Instance;

            // Add hero's collider
            OnAddCollider?.Invoke(hero);
            
            bulletPool = new ObjectPool<Bullet>(100);


            // Attach all listeners
            stageManager.OnCreateEnemyPools += CreateStageSpecificPools;
            stageManager.OnCompleteStage += DeleteStageSpecificPools;
            hero.WeaponHandler.OnSpawnBullet += SpawnBullet;
            EnemyDirector.Instance.OnCreateDoge += SpawnDoge;
            EnemyDirector.Instance.OnCreateSkull += SpawnSkull;
            EnemyDirector.Instance.OnCreateBald += SpawnBald;
            EnemyDirector.Instance.OnCreateDark += SpawnDark;

            bulletsAlive = new List<Bullet>();
            enemiesAlive = new List<Enemy>();
        }

        // Create pools based on 
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
            // TODO: Manually delete dynamic memeory 

            dogePool = null;
            skullPool = null;
            baldPool = null;
            darkPool = null;
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
                    // Game1.DrawRect(spriteBatch, enemiesAlive[i].BoundingRect);
                }
            }
        }

        
        private void SpawnBullet(IArgs args)
        {
            Bullet bullet = bulletPool.SpawnFromPool(args);
            bulletsAlive.Add(bullet);
            OnAddCollider?.Invoke(bullet);
        }

        // Enemy Spawns
        //
        private void SpawnDoge(IArgs args)
        {
            Doge doge = dogePool.SpawnFromPool(args);
            enemiesAlive.Add(doge);
            OnAddCollider?.Invoke(doge);
        }
        private void SpawnSkull(IArgs args)
        {
            Skull skull = skullPool.SpawnFromPool(args);
            enemiesAlive.Add(skull);
            OnAddCollider?.Invoke(skull);
        }
        private void SpawnBald(IArgs args)
        {
            Bald bald = baldPool.SpawnFromPool(args);
            enemiesAlive.Add(bald);
            OnAddCollider?.Invoke(bald);
        }
        private void SpawnDark(IArgs args)
        {
            Dark dark = darkPool.SpawnFromPool(args);
            enemiesAlive.Add(dark);
            OnAddCollider?.Invoke(dark);
        }
    }
}
