using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public interface IArgs
    {

    }

    public class ArgsBullet : IArgs
    {
        public Texture2D BulletTexture { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 Direction { get; private set; }
        public float MaxLiveTime { get; private set; }
        public float MoveSpeed { get; private set; }
        public Vector2 ColliderSize { get; private set; }

        public ArgsBullet(Vector2 position, Vector2 direction, Texture2D bulletTexture, float maxLiveTime, float moveSpeed, Vector2 colliderSize)
        {
            Position = position;
            Direction = direction;
            BulletTexture = bulletTexture;
            MaxLiveTime = maxLiveTime;
            MoveSpeed = moveSpeed;
            ColliderSize = colliderSize;
        }

    }

    public class ArgsEnemy : IArgs
    {
        public Vector2 Position { get; private set; }
        public float StartingHealth { get; private set; }
        public string InitialState { get; private set; }
        public Rectangle LocalBounds { get; private set; }

        public ArgsEnemy(Vector2 position, Rectangle localBounds, float startingHealth, string initialState)
        {
            Position = position;
            LocalBounds = localBounds;
            StartingHealth = startingHealth;
            InitialState = initialState;
        }

    }
}
