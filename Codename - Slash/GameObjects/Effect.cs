using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public class Effect : GameObject
    {
        private Animation animation;
        private Animator animator;
        private Vector2 position;

        private float currentAliveTime;
        private float maxAliveTime; 
        
        public override void OnPoolInstantiation()
        {
            animator = new Animator();
        }

        public override void OnSpawnFromPool(IArgs args)
        {
            if (!(args is ArgsEffect a)) { throw new ArgumentException(); }
            
            animator.AttachAnimation(a.Animation);
            animator.ResetAnimator();
            position = a.Position;
            maxAliveTime = a.AliveTime;
            IsActive = true;
        }

        public override void Update(float deltaTime)
        {
            currentAliveTime += deltaTime;
            if (currentAliveTime >= maxAliveTime)
                endEffect();
        }

        private void endEffect()
        {
            currentAliveTime = 0.0f;
            IsActive = false;
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            // Draw that sprite.
            animator.Draw(deltaTime, spriteBatch, position, SpriteEffects.None, Color.White);
        }
    }
}
