﻿using Codename___Slash.Collisions;
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
        private float rotationAngle;
        private Vector2 drawSize;

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
        public List<ColliderType> InteractionTypes
        {
            get
            {
                if (interactionTypes == null)
                {
                    List<ColliderType> i = new List<ColliderType>();
                    i.Add(ColliderType.enemy);
                    i.Add(ColliderType.staticEnvironment);
                    interactionTypes = i;
                    return i;
                }
                return interactionTypes;
            }
            set { interactionTypes = value; }
        }

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
            spriteBatch.Draw(bulletTexture, position, null, null, null, rotationAngle, drawSize, Color.White, SpriteEffects.None, 0);
            
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
            drawSize = a.DrawSize;
            // BoundingRect = new Rectangle((int)position.X, (int)position.Y, (int)a.ColliderSize.X, (int)a.ColliderSize.Y);

            // If enemy bullet spawned, then change interactiontypes
            if (!a.IsEnemyBullet)
            {
                ColliderType = ColliderType.heroAttack;

                InteractionTypes.Clear();
                InteractionTypes.Add(ColliderType.enemy);
                InteractionTypes.Add(ColliderType.staticEnvironment);
            }
            else
            { 
                ColliderType = ColliderType.enemyAttack;
                InteractionTypes.Clear();
                InteractionTypes.Add(ColliderType.hero);
                InteractionTypes.Add(ColliderType.heroAttack);
            }

            rotationAngle = (float) Math.Atan2(moveDirection.Y, moveDirection.X);
            
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
            // Add death effect



            IsActive = false;
        }
    }
}


// variable parameters
// 