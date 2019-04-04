using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public abstract class Enemy : GameObject, ICollidable, IDamageDealer, IDamageable
    {
        public float CurrentHealth { get; protected set; }
        
        public float MoveSpeed { get; protected set; }

        public Vector2 Position { get; set; }

        public Color DrawColor { get; set; }
        public EnemyAnimations EnemyAnimations { get; protected set; }
        public Animator Animator { get; protected set; }


        public float maxHealth = 100;

        // Collider properties
        protected Vector2Int colliderSize;
        private Rectangle localBounds;
        public Rectangle BoundingRect
        {
            get
            {
                int left = (int)Math.Round(Position.X - Animator.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - Animator.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
            set { }
        }
        public abstract bool FlaggedForRemoval { get; set; }
        public ColliderType ColliderType { get { return ColliderType.enemy; } set { value = ColliderType.enemy; } }
        public int DealDamageValue { get; set; }

        public List<ColliderType> interactionTypes;
        public List<ColliderType> InteractionTypes
        {
            get
            {
                if (interactionTypes == null)
                {
                    List<ColliderType> i = new List<ColliderType>(); i.Add(ColliderType.hero);
                    i.Add(ColliderType.heroAttack);
                    i.Add(ColliderType.enemy);
                    i.Add(ColliderType.staticEnvironment); return i;
                }
                return interactionTypes;
            }
        }

        public Action<Vector2> OnDamage;
        public Action<Enemy, Vector2> OnDeath;

        public Enemy()
        {
        }

        public override void OnPoolInstantiation()
        {
            Position = new Vector2(0);
        }

        public override void OnSpawnFromPool(IArgs args)
        {
            // Reinitialise enemy
            // Pattern match with the correct concrete class
            if (!(args is ArgsEnemy a)) { throw new ArgumentException(); }

            // Assign values
            Animator.AttachAnimation(EnemyAnimations.IdleAnimation);
            localBounds = a.LocalBounds;

            IsActive = true;
        }

        public override void Update(float deltaTime)
        {
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            // Draw that sprite.
            Animator.Draw(deltaTime, spriteBatch, Position, SpriteEffects.None, DrawColor);
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
            //// Get rectangle of the intersection/collision depth
            //Rectangle r = Rectangle.Intersect(BoundingRect, other.BoundingRect);

            //// Move the collider in the opposite direction by that amount
            //Position += new Vector2(-r.Width, -r.Height);

            // Take damage if collision with hero attack
            if (other.ColliderType == ColliderType.heroAttack)
            {
                TakeDamage((other as IDamageDealer).DealDamageValue);
            } 
        }
        public virtual void TakeDamage(int damagePoints)
        {
            CurrentHealth -= damagePoints;
            if (CurrentHealth > 0)
            {
                OnDamage?.Invoke(Position);
            }
            else
            {
                IsActive = false;
                OnDeath?.Invoke(this, Position);
            }
        }

        public abstract void TakeDamage(int damagePoints, Vector2 direction);
    }

    public class EnemyAnimations
    {
        public Animation IdleAnimation { get; }
        public Animation UpAnimation { get; }
        public Animation DownAnimation { get; }
        public Animation RightAnimation { get; }
        public Animation LeftAnimation { get; }

        public EnemyAnimations(Animation idleAnimation, Animation upAnimation, Animation downAnimation, Animation rightAnimation, Animation leftAnimation)
        {
            IdleAnimation = idleAnimation;
            UpAnimation = upAnimation;
            DownAnimation = downAnimation;
            RightAnimation = rightAnimation;
            LeftAnimation = leftAnimation;
        }

    }
}
