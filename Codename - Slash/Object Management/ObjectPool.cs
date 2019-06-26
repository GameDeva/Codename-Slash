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

        public int PoolSize { get; private set; }
        // Queue of type T in the pool
        private Queue<T> poolQueue;

        public ObjectPool(int poolSize)
        {
            PoolSize = poolSize;

            // Create queue of given size
            poolQueue = new Queue<T>(poolSize);


            // Fill queue with new objects of type T
            PopulatePool();
        }

        // Initially create all object in pool
        private void PopulatePool() 
        {
            for(int i = 0; i < PoolSize; i++)
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
        
        // Returns the next object that is avaible in the pool to be used
        public T SpawnFromPool(IArgs args)
        {
            T obj = poolQueue.Dequeue();

            obj.OnSpawnFromPool(args);

            poolQueue.Enqueue(obj);

            return obj;
        }



    }
}
