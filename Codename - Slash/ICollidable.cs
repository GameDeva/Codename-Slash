using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    // ICollidable interface 
    public interface ICollidable
    {
        Rectangle BoundingRect { get; set; } // Bounding Rect of each collidable to perform collision test on
        ColliderType ColliderType { get; set; } // Type of collidable implemented object is
        List<ColliderType> InteractionTypes { get; } // Types of colliders it can interact with

        bool CollisionTest(ICollidable other); // Collision test for each object
        void OnCollision(ICollidable other); // Appropriate collsiion resolution code
    }
}
