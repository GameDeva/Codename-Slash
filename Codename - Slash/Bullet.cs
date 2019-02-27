using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class Bullet
    {
        public Vector2 position;
        public Vector2 moveDirection;

        private float moveSpeed;

        public Bullet(Vector2 spawnPoint, Vector2 moveDirection)
        {
            position = spawnPoint;
            this.moveDirection = moveDirection;
        }

    }
}
