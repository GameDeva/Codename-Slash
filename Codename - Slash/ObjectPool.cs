using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    // Generic Object Pool class 
    // Ensures type of object pooled has a parameterless constructor and and IPoolable interface
    public class ObjectPool<T> where T : IPoolable, new() 
    {

        private int poolSize;
        // Queue of type T in the pool
        private Queue<T> poolQueue;

        public ObjectPool(int poolSize)
        {
            this.poolSize = poolSize;

            // Create queue of given size
            poolQueue = new Queue<T>(poolSize);


            // Fill queue with new objects of type T
            PopulatePool();
        }

        private void PopulatePool() 
        {
            for(int i = 0; i < poolSize; i++)
            {
                // Create new object of type T
                T obj = new T();

                // Method to apply when initially created
                // i.e. to set the gameobject to inatactive
                obj.OnPoolInstantiation();

                // Add the object to the pool
                poolQueue.Enqueue(obj);
                
            }

        }

        public void SpawnFromPool(IArgs args)
        {
            T obj = poolQueue.Dequeue();

            obj.OnSpawnFromPool(args);

            poolQueue.Enqueue(obj);

        }



    }
}
