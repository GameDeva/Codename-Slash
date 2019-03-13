using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class GameObjectPool
    {

        private int poolSize;
        // Queue of Gameobjects in the pool
        private Queue<GameObject> poolQueue;

        public GameObjectPool(int poolSize)
        {
            this.poolSize = poolSize;

            // Create queue of given size
            poolQueue = new Queue<GameObject>(poolSize);


            // Fill queue with new objects of gameobjects
            PopulatePool();
        }

        private void PopulatePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                // Create new object of type gameobject
                GameObject obj = new GameObject();

                // Method to apply when initially created
                // i.e. to set the gameobject to inatactive
                obj.OnPoolInstantiation();

                // Add the object to the pool
                poolQueue.Enqueue(obj);

            }

        }

        public void SpawnFromPool()
        {
            GameObject obj = poolQueue.Dequeue();

            obj.OnSpawnFromPool();

            poolQueue.Enqueue(obj);

        }

    }
}
