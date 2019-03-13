using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class Bullet : GameObject
    {
        public Vector2 position;
        public Vector2 moveDirection;

        private float moveSpeed;

        public Bullet()
        {

        }

        //public Bullet(Vector2 spawnPoint, Vector2 moveDirection)
        //{
        //    position = spawnPoint;
        //    this.moveDirection = moveDirection;
        //}

        public override void OnPoolInstantiation()
        {
            
        }

        public override void OnSpawnFromPool(IArgs args)
        {
            // Pattern match with the correct concrete class
            if (!(args is ArgsBullet a)) { throw new ArgumentException(); }
            
            // Assign values
            position = a.position;
            this.moveDirection = a.direction;
        }
    }
}


// variable parameters
// 