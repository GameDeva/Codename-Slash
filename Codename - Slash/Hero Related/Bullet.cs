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
        private Texture2D bulletTexture;
        private Vector2 position;
        private Vector2 moveDirection;
        private float maxLiveTime;
        private float moveSpeed;

        public Bullet()
        {
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += moveSpeed * moveDirection * deltaTime;

            if (liveTime < maxLiveTime)
                liveTime += deltaTime;
            else
                IsActive = false;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
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
            position = a.Position;
            moveDirection = a.Direction;
            bulletTexture = a.BulletTexture;
            maxLiveTime = a.MaxLiveTime;
            moveSpeed = a.MoveSpeed;

            liveTime = 0f;
            IsActive = true;
        }
    }
}


// variable parameters
// 