using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    public class StaticCollider : ICollidable
    {
        public Rectangle BoundingRect { get; set; }
        public bool FlaggedForRemoval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ColliderType ColliderType { get; set; }

        public StaticCollider(Rectangle boundingRect, ColliderType colliderType)
        {
            BoundingRect = boundingRect;
            ColliderType = colliderType;
        }

        public bool CollisionTest(ICollidable other)
        {
            if (other != null)
            {
                return BoundingRect.Intersects(other.BoundingRect);
            }
            return false;
        }

        public void OnCollision(ICollidable other)
        {
            // Do nothing
            // The other guy will take care of his explosion/repulsion etc
        }
    }
}
