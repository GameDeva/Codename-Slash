using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    public class GameObject : IPoolable
    {
        public bool IsActive { get; protected set; }

        public virtual void OnPoolInstantiation() { }

        public virtual void OnSpawnFromPool(IArgs args) { }
    }




    public interface IArgs
    {
        
    }

    public class ArgsBullet : IArgs
    {
        public Vector2 position;
        public Vector2 direction;

        public ArgsBullet(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.direction = direction;
        }

    }

    public class ArgsEnemy : IArgs
    {
        public Vector2 position;
        // more args

        public ArgsEnemy(Vector2 position)
        {
            this.position = position;
        }

    }

}
