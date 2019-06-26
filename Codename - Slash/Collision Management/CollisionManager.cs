using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash.Collisions
{
    // Collision types, that can be assigned to each ICollidable, or set to which ones it can collide with
    public enum ColliderType
    {
        hero,
        enemy,
        heroAttack,
        staticEnvironment, 
        interactableObjects, 
        triggerRegions,
        enemyAttack
    }

    // Handles all collisions between ICollidables during game session
    public class CollisionManager
    {
        // Singleton creation
        private static CollisionManager instance;
        public static CollisionManager Instance { get { if (instance == null) { instance = new CollisionManager(); return instance; } return instance; } set { instance = value; } }

        // Reference to Poolmanager to receive events about adding colliders, when objects are pooled
        private PoolManager poolmanager;

        // List of ICollidables that are active in the scene, used to check collisions
        private List<ICollidable> collidablesList = new List<ICollidable>();
        // Hashset of collisions (between 2 ICollidables) that need to be resolved
        private HashSet<Collision> collisionOccuranceList = new HashSet<Collision>(new CollisionComparer());
        
        // Add an ICollidable to Collidable list
        private void AddCollidable(ICollidable c)
        {
            collidablesList.Add(c);
        }

        // Remove an ICollidable to Collidable List
        private void RemoveCollidable(ICollidable c)
        {
            collidablesList.Remove(c);
        }

        // Removes all ICollidables of certain type
        private void RemoveAllOfType(ColliderType c)
        {
            for(int i = collidablesList.Count -1; i >= 0; i--)
            {
                if (collidablesList[i].ColliderType == c)
                    collidablesList.RemoveAt(i);
            }
        }

        // Clears all all ICollidables from the collidable list
        public void RemoveAll()
        {
            collidablesList.Clear();
        }

        public void Initialise()
        {
            // Get reference to singleton instances
            poolmanager = PoolManager.Instance;

            // Attach all events from poolManager
            poolmanager.OnAddCollider += AddCollidable;
            poolmanager.OnRemoveCollider += RemoveCollidable;
            poolmanager.OnRemoveAllCollidersOfType += RemoveAllOfType;
        }

        public void ReInitialise()
        {
            collidablesList.Clear();

        }

        public void Update()
        {
            UpdateCollisions();
            ResolveCollisions();
        }

        // Check for collisions between Collidables list
        private void UpdateCollisions()
        {
            // Clear collisionOccuranceList from previous frame, to add fresh batch
            if (collisionOccuranceList.Count > 0)
            {
                collisionOccuranceList.Clear();
            }

            // Iterate through Icollidable objects and test for collisions between each one
            for (int i = 0; i < collidablesList.Count; i++)
            {
                for (int j = 0; j < collidablesList.Count; j++)
                {
                    ICollidable collidable1 = collidablesList[i];
                    ICollidable collidable2 = collidablesList[j];

                    // Make sure we're not checking an object with itself
                    // Plus if it is one of the collider types it can interact with
                    if (!collidable1.Equals(collidable2) && collidable1.InteractionTypes.Contains(collidable2.ColliderType))
                    {
                        // If the two objects are colliding then add them to the set
                        if (collidable1.CollisionTest(collidable2))
                        {
                            collisionOccuranceList.Add(new Collision(collidable1, collidable2));
                        }
                    }
                }
            }
            
        }
        
        // For each of the collisions in collisionOccuranceList, call the resolve method
        private void ResolveCollisions()
        {
            foreach (Collision c in collisionOccuranceList)
            {
                c.Resolve();
            }
        }

        // 
        // For debugging collisions
        //public void DebugDraw(SpriteBatch spriteBatch)
        //{
        //    foreach (ICollidable c in collidablesList)
        //    {
        //        Game1.DrawRect(spriteBatch, c.BoundingRect);
        //    }
        //}

    }
}

