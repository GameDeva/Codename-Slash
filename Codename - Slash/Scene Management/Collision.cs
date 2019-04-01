using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
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

    public class Collision
    {
        public ICollidable A;
        public ICollidable B;

        public Collision(ICollidable a, ICollidable b)
        {
            A = a;
            B = b;
        }

        public bool Equals(Collision other)
        {
            if (other == null) return false;

            if ((A.Equals(other.A) && B.Equals(other.B)))
            {
                return true;
            }

            return false;
        }

        public void Resolve()
        {
            A.OnCollision(B);
        }

    }
}
