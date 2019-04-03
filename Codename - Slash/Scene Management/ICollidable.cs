using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public interface ICollidable
    {
        Rectangle BoundingRect { get; set; }
        bool FlaggedForRemoval { get; set; }
        ColliderType ColliderType { get; set; }
        List<ColliderType> InteractionTypes { get; } // Types of colliders it can interact with

        bool CollisionTest(ICollidable other);
        void OnCollision(ICollidable other);
    }
}
