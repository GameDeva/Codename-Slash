using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public enum ColliderType
    {
        hero,
        enemy,
        heroAttack,
        staticEnvironment, 
        interactableObjects, 
        triggerRegions
    }

    public class CollisionManager
    {
        // Single creation
        private static CollisionManager instance;
        public static CollisionManager Instance { get { if (instance == null) { instance = new CollisionManager(); return instance; } return instance; } set { instance = value; } }

        private PoolManager poolmanager;
        private MapGen mapGenerator;

        private List<ICollidable> heroes = new List<ICollidable>(); // Kept when adding coop mode, with more than 1 hero
        private List<ICollidable> enemyEntities = new List<ICollidable>();
        private List<ICollidable> heroAttacks = new List<ICollidable>();
        private List<ICollidable> staticEnvironmentColliders = new List<ICollidable>();
        private List<ICollidable> interactbleObjectColliders = new List<ICollidable>();
        private List<ICollidable> triggerRegionColliders = new List<ICollidable>();

        private HashSet<Collision> collisionOccuranceList = new HashSet<Collision>(new CollisionComparer());

        // TODO: If there is issue with performance when it comes to switch statements,
        //          when adding or removing colliders, change up the pattern, i.e. separate methods
        private void AddCollidable(ICollidable c, ColliderType colliderType)
        {
            switch (colliderType)
            {
                case ColliderType.hero:
                    heroes.Add(c);
                    break;
                case ColliderType.enemy:
                    enemyEntities.Add(c);
                    break;
                case ColliderType.heroAttack:
                    heroAttacks.Add(c);
                    break;
                case ColliderType.staticEnvironment:
                    staticEnvironmentColliders.Add(c);
                    break;
                case ColliderType.interactableObjects:
                    interactbleObjectColliders.Add(c);
                    break;
                case ColliderType.triggerRegions:
                    triggerRegionColliders.Add(c);
                    break;
            }
        }

        private void RemoveCollidable(ICollidable c, ColliderType colliderType)
        {
            switch (colliderType)
            {
                case ColliderType.hero:
                    heroes.Remove(c);
                    break;
                case ColliderType.enemy:
                    enemyEntities.Remove(c);
                    break;
                case ColliderType.heroAttack:
                    heroAttacks.Remove(c);
                    break;
                case ColliderType.staticEnvironment:
                    staticEnvironmentColliders.Remove(c);
                    break;
            }
        }

        public void Initialise()
        {
            // Get reference to singleton instances
            poolmanager = PoolManager.Instance;
            mapGenerator = MapGen.Instance;

            poolmanager.OnAddDynamicCollider += AddCollidable;
            mapGenerator.OnAddcollider += AddCollidable;
            mapGenerator.OnRemoveAllStaticColliders += RemoveAllStaticColliders;
        }

        public void Update()
        {
            UpdateCollisions();
            ResolveCollisions();
        }

        // Inlined collision checks, for improved performance
        private void UpdateCollisions()
        {
            if (collisionOccuranceList.Count > 0)
            {
                collisionOccuranceList.Clear();
            }

            // 
            // Hero attacks vs Enemies & staticEnv & interactableItems
            foreach (ICollidable heroAttack in heroAttacks)
            {
                // Check collisions with enemies
                foreach (ICollidable enemyEntity in enemyEntities)
                {
                    // If the two objects are colliding then add them to the set
                    if (heroAttack.CollisionTest(enemyEntity))
                    {
                        collisionOccuranceList.Add(new Collision(heroAttack, enemyEntity));
                    }
                }
                // Check collisions with environment
                foreach (ICollidable environment in staticEnvironmentColliders)
                {
                    // If the two objects are colliding then add them to the set
                    if (heroAttack.CollisionTest(environment))
                    {
                        collisionOccuranceList.Add(new Collision(heroAttack, environment));
                    }
                }
                // Check collisions with itneractables
                foreach (ICollidable io in interactbleObjectColliders)
                {
                    // If the two objects are colliding then add them to the set
                    if (heroAttack.CollisionTest(io))
                    {
                        collisionOccuranceList.Add(new Collision(heroAttack, io));
                    }
                }
            }

            // 
            // Enemies vs Enemies & staticEnv
            foreach (ICollidable enemyEntity1 in enemyEntities)
            {
                // Check collisions with enemies
                foreach (ICollidable enemyEntity2 in enemyEntities)
                {
                    // Make sure we're not checking an object with itself
                    if (!enemyEntity1.Equals(enemyEntity2))
                    {
                        // If the two objects are colliding then add them to the set
                        if (enemyEntity1.CollisionTest(enemyEntity2))
                        {
                            collisionOccuranceList.Add(new Collision(enemyEntity1, enemyEntity2));
                        }
                    }


                }

                // Check collisions with environment
                foreach (ICollidable environment in staticEnvironmentColliders)
                {
                    // If the two objects are colliding then add them to the set
                    if (enemyEntity1.CollisionTest(environment))
                    {
                        collisionOccuranceList.Add(new Collision(enemyEntity1, environment));
                    }
                }
            }

            // 
            // Hero vs Enemies & staticEnv & TriggerRegions
            foreach (ICollidable hero1 in heroes)
            {
                // Check collisions with other heroes
                foreach (ICollidable hero2 in heroes)
                {
                    // Make sure we're not checking an object with itself
                    if (!hero1.Equals(hero2))
                    {
                        // If the two objects are colliding then add them to the set
                        if (hero1.CollisionTest(hero2))
                        {
                            collisionOccuranceList.Add(new Collision(hero1, hero2));
                        }
                    }
                }

                // Check collisions with enemies
                foreach (ICollidable enemyEntity2 in enemyEntities)
                {
                    // If the two objects are colliding then add them to the set
                    if (hero1.CollisionTest(enemyEntity2))
                    {
                        collisionOccuranceList.Add(new Collision(hero1, enemyEntity2));
                    }
                }

                // Check collisions with environment
                foreach (ICollidable environment in staticEnvironmentColliders)
                {
                    // If the two objects are colliding then add them to the set
                    if (hero1.CollisionTest(environment))
                    {
                        collisionOccuranceList.Add(new Collision(hero1, environment));
                    }
                }

                // Check collisions with triggers
                foreach (ICollidable trigger in triggerRegionColliders)
                {
                    // If the two objects are colliding then add them to the set
                    if (hero1.CollisionTest(trigger))
                    {
                        collisionOccuranceList.Add(new Collision(hero1, trigger));
                    }
                }
            }
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            foreach (ICollidable staticEnv in staticEnvironmentColliders)
            {
                Game1.DrawRect(spriteBatch, staticEnv.BoundingRect);
            }

        }

        private void ResolveCollisions()
        {
            foreach (Collision c in collisionOccuranceList)
            {
                c.Resolve();
            }
        }

        private void RemoveAllStaticColliders()
        {
            staticEnvironmentColliders.Clear();
        }
    }
}
