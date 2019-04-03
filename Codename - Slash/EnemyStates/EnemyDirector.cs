using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Codename___Slash.EnemyStates
{
    // Enemy Director Singleton
    public class EnemyDirector
    {
        // Single creation
        private static EnemyDirector instance;
        public static EnemyDirector Instance { get { if (instance == null) { instance = new EnemyDirector(); return instance; } return instance; } set { instance = value; } }

        private List<Enemy> enemiesAlive;
        private Hero hero;

        // Actions
        public Action<IArgs> OnCreateDoge;
        public Action<IArgs> OnCreateBald;
        public Action<IArgs> OnCreateSkull;
        public Action<IArgs> OnCreateDark;

        // Animations
        public EnemyAnimations DogeAnimations { get; private set; }
        public EnemyAnimations SkullAnimations { get; private set; }
        public EnemyAnimations BaldAnimations { get; private set; }
        public EnemyAnimations DarkAnimations { get; private set; }
        
        // Localbounds
        public Rectangle DogeLocalBounds { get; private set; }
        public Rectangle SkullLocalBounds { get; private set; }
        public Rectangle BaldLocalBounds { get; private set; }
        public Rectangle DarkLocalBounds { get; private set; }

        public void Initialise(Hero hero, ContentManager content)
        {
            enemiesAlive = new List<Enemy>();
            this.hero = hero;

            DogeAnimations = new EnemyAnimations(new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_idle"), 1, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_up"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_down"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_right"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_left"), 3, 0.1f, true));
            SkullAnimations = new EnemyAnimations(new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_idle"), 1, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_up"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_down"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_right"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_left"), 3, 0.1f, true));
            BaldAnimations = new EnemyAnimations(new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_idle"), 1, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_up"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_down"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_right"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_left"), 3, 0.1f, true));
            DarkAnimations = new EnemyAnimations(new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_idle"), 1, 0.1f, true),
                            new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_up"), 3, 0.1f, true),
                            new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_down"), 3, 0.1f, true),
                            new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_right"), 3, 0.1f, true),
                            new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_left"), 3, 0.1f, true));

            // Calculate bounds within texture size.            
            //
            int width = (int)(DogeAnimations.IdleAnimation.FrameWidth);
            int left = (DogeAnimations.IdleAnimation.FrameWidth - width) / 2;
            int height = (int)(DogeAnimations.IdleAnimation.FrameHeight *2);
            int top = DogeAnimations.IdleAnimation.FrameHeight - height+16;
            DogeLocalBounds = new Rectangle(left, top, width, height);
            //             
            width = (int)(DogeAnimations.IdleAnimation.FrameWidth);
            left = (DogeAnimations.IdleAnimation.FrameWidth - width) / 2;
            height = (int)(DogeAnimations.IdleAnimation.FrameHeight * 2);
            top = DogeAnimations.IdleAnimation.FrameHeight - height + 16;
            SkullLocalBounds = new Rectangle(left, top, width, height);
            //             
            width = (int)(DogeAnimations.IdleAnimation.FrameWidth);
            left = (DogeAnimations.IdleAnimation.FrameWidth - width) / 2;
            height = (int)(DogeAnimations.IdleAnimation.FrameHeight * 2);
            top = DogeAnimations.IdleAnimation.FrameHeight - height + 16;
            BaldLocalBounds = new Rectangle(left, top, width, height);
            // 
            width = (int)(DogeAnimations.IdleAnimation.FrameWidth);
            left = (DogeAnimations.IdleAnimation.FrameWidth - width) / 2;
            height = (int)(DogeAnimations.IdleAnimation.FrameHeight * 2);
            top = DogeAnimations.IdleAnimation.FrameHeight - height + 16;
            BaldLocalBounds = new Rectangle(left, top, width, height);
                        
        }

        public void CreateEnemies()
        {
            CreateEnemy("Skull", SkullLocalBounds, new Vector2(100, 100), 100, "chase");
            //CreateEnemy("Skull", SkullLocalBounds, new Vector2(800, 500), 100, "chase");
            //CreateEnemy("Skull", SkullLocalBounds, new Vector2(400, 900), 100, "chase");
            //CreateEnemy("Skull", SkullLocalBounds, new Vector2(900, 100), 100, "chase");

            //CreateEnemy("Doge", DogeLocalBounds, new Vector2(300, 300), 100, "idle");
            //CreateEnemy("Doge", DogeLocalBounds, new Vector2(500, 500), 100, "idle");
            //CreateEnemy("Doge", DogeLocalBounds, new Vector2(700, 100), 100, "idle");
            //CreateEnemy("Doge", DogeLocalBounds, new Vector2(300, 800), 100, "idle");
        }

        public float SqrDistanceToHeroFrom(Vector2 position)
        {
            // TODO: Check for performance bottleneck
            return Vector2.DistanceSquared(position, hero.Position);
        }

        public Vector2 DirectionToHeroNormalised(Vector2 position)
        {
            return Vector2.Normalize(hero.Position - position);
        }

        public Vector2 GetHeroPosition()
        {
            return hero.Position;
        }

        private void OnEnemyDestroyed(Enemy enemy)
        {

        }

        private void CreateEnemy(string type, Rectangle localBounds, Vector2 spawnPoint, float startingHealth, string initialState)
        {
            if(type.Equals("DOGE", StringComparison.OrdinalIgnoreCase)) 
            {
                OnCreateDoge?.Invoke(new ArgsEnemy(spawnPoint, localBounds, startingHealth, initialState));
            }
            else if(type.Equals("Bald", StringComparison.OrdinalIgnoreCase))
            {
                OnCreateBald?.Invoke(new ArgsEnemy(spawnPoint, localBounds, startingHealth, initialState));
            }
            else if (type.Equals("Skull", StringComparison.OrdinalIgnoreCase))
            {
                OnCreateSkull?.Invoke(new ArgsEnemy(spawnPoint, localBounds, startingHealth, initialState));
            }
            else if(type.Equals("Dark", StringComparison.OrdinalIgnoreCase))
            {
                OnCreateDark?.Invoke(new ArgsEnemy(spawnPoint, localBounds, startingHealth, initialState));
            }
            else
            {
                Console.WriteLine("Enemy creation type not recognised");
            }

        }


    }
}
