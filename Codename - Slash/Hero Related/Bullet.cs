using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Codename___Slash
{
    public class Bullet : GameObject
    {
        public Texture2D bulletTexture;
        public Vector2 position;
        public Vector2 moveDirection;

        private const float moveSpeed = 500.0f;

        public Bullet()
        {
        }

        public override void Update(float deltaTime)
        {
            position += moveSpeed * moveDirection * deltaTime;

            if (liveTime < 1f)
                liveTime += deltaTime;
            else
                IsActive = false;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, position, Color.White);
            
        }

        public override void OnPoolInstantiation()
        {
            
        }

        public override void OnSpawnFromPool(IArgs args)
        {
            // Pattern match with the correct concrete class
            if (!(args is ArgsBullet a)) { throw new ArgumentException(); }
            
            // Assign values
            position = a.position;
            moveDirection = a.direction;
            bulletTexture = a.bulletTexture;

            liveTime = 0f;
            IsActive = true;
        }
    }
}


// variable parameters
// 