using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Codename___Slash
{
    public class Bullet : GameObject, ICollidable, IDamageDealer
    {
        private Texture2D bulletTexture;
        private Vector2 position;
        private Vector2 moveDirection;
        private float maxLiveTime;
        private float moveSpeed;
        private int colliderSize = 15;

        public Rectangle BoundingRect
        {
            get
            {
                return new Rectangle(position.ToPoint(), new Point(colliderSize));
            }
            set { }
        }
        public ColliderType ColliderType { get; set; } = ColliderType.heroAttack;
        public int DealDamageValue { get; set; }

        public List<ColliderType> interactionTypes;
        public List<ColliderType> InteractionTypes { get { if (interactionTypes == null) { List<ColliderType> i = new List<ColliderType>(); i.Add(ColliderType.enemy); i.Add(ColliderType.staticEnvironment); return i; } return interactionTypes; } }

        public Bullet()
        {
        }

        public override void Update(float deltaTime)
        {
            position += moveSpeed * moveDirection * deltaTime;

            if (liveTime < maxLiveTime)
                liveTime += deltaTime;
            else
                IsActive = false;

        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTexture, position, null, null, null, 0, new Vector2(colliderSize/5), Color.LightGreen, SpriteEffects.None, 0);
            
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
            colliderSize = (int)a.ColliderSize.X;
            DealDamageValue = (int)a.DamageValue;
            // BoundingRect = new Rectangle((int)position.X, (int)position.Y, (int)a.ColliderSize.X, (int)a.ColliderSize.Y);

            liveTime = 0f;
            IsActive = true;
        }

        public bool CollisionTest(ICollidable other)
        {
            if (other != null && IsActive)
            {
                return BoundingRect.Intersects(other.BoundingRect);
            }
            return false;
        }

        public void OnCollision(ICollidable other)
        {
            IsActive = false;
        }
    }
}


// variable parameters
// 