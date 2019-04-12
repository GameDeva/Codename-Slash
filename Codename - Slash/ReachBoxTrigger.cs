using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    public class ReachBoxTrigger : ICollidable
    {
        public Rectangle BoundingRect { get; set; }
        public ColliderType ColliderType { get; set; }

        public bool Active { get; set; } = false;
        public List<ColliderType> interactionTypes;
        public List<ColliderType> InteractionTypes
        {
            get
            {
                if (interactionTypes == null)
                {
                    List<ColliderType> i = new List<ColliderType>(); i.Add(ColliderType.hero); return i;
                }
                return interactionTypes;
            }
        }

        public Action Triggered;

        public ReachBoxTrigger(Rectangle BoundingRect)
        {
            this.BoundingRect = BoundingRect;
            ColliderType = ColliderType.triggerRegions;

        }

        public bool CollisionTest(ICollidable other)
        {
            if (other != null)
            {
                if (other.ColliderType == ColliderType.hero && Active)
                {
                    return BoundingRect.Intersects(other.BoundingRect);
                }
            }
            return false;
        }

        public void OnCollision(ICollidable other)
        {
            Active = false;
            Triggered?.Invoke();
        }
    }
}
