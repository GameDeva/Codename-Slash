﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Codename___Slash
{
    // GameObject base class for all dynamic in game objects
    public abstract class GameObject : IPoolable
    {
        protected float liveTime;

        public bool IsActive { get; protected set; }

        public abstract void OnPoolInstantiation();

        public abstract void OnSpawnFromPool(IArgs args);

        public abstract void Update(float deltaTime);

        public abstract void Draw(float deltaTime, SpriteBatch spriteBatch);
    }

    
}
