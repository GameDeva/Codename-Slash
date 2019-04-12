using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    // Comparator class for Collisions used to compare Collisions
    public class CollisionComparer : IEqualityComparer<Collision>
    {
        public bool Equals(Collision a, Collision b)
        {
            if ((a == null) || (b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public int GetHashCode(Collision a)
        {
            return a.GetHashCode();
        }
    }

    // Stores a collision that has occured between 2 ICollidable objects 
    public class Collision
    {
        public ICollidable A;
        public ICollidable B;

        public Collision(ICollidable a, ICollidable b)
        {
            A = a;
            B = b;
        }
        
        // Checks whether this a Collision object is the same as another
        //  by comparing the A and B value of each
        public bool Equals(Collision other)
        {
            if (other == null) return false;

            if ((A.Equals(other.A) && B.Equals(other.B)))
            {
                return true;
            }

            return false;
        }

        // Calls the appropriate OnCollision method
        public void Resolve()
        {
            A.OnCollision(B);
        }

    }
}
