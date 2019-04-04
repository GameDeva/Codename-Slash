using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash.Scene_Management
{
    public class Effect : GameObject
    {
        private Animation animation;
        private Animator animator;

        private float currentAliveTime;
        private float maxAliveTime; 

        public void Update()
        {
            
        }

        public override void OnPoolInstantiation()
        {
            animator = new Animator();
        }

        public override void OnSpawnFromPool(IArgs args)
        {
            if (!(args is ArgsEffect a)) { throw new ArgumentException(); }

            animation = a.Animation;
        }

        public override void Update(float deltaTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
