using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public abstract class Enemy : GameObject
    {
        public float CurrentHealth { get; protected set; }

        public float MoveSpeed { get; protected set; }

        public Vector2 Position { get; set; }

        public Color DrawColor { get; set; }
        public EnemyAnimations EnemyAnimations { get; protected set; }
        public Animator animator { get; protected set; }

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
            animator.AttachAnimation(EnemyAnimations.IdleAnimation);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw that sprite.
            animator.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, DrawColor);
        }
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
