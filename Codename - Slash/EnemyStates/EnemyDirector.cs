using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Codename___Slash
{
    // Enemy Director Singleton
    public class EnemyDirector
    {
        // Single creation
        private static EnemyDirector instance;
        public static EnemyDirector Instance { get { if (instance == null) { instance = new EnemyDirector(); return instance; } return instance; } set { instance = value; } }

        private StageManager stageManager;
        private ContentManager content;
        // 
        private Random random;
        private bool shouldSpawn;
        private float spawnIntervalTimer;
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
            this.content = content;
            stageManager = StageManager.Instance;
            random = new Random();

            stageManager.OnLoadEnemyContent += LoadEnemyContent;
            stageManager.ToggleEnemyGeneration += SpawnToggle;
            // Calculate bounds within texture size.            
            //

                        
        }

        // Load in all enemy files only if the probability of spawning in this stage is high
        private void LoadEnemyContent(StageData stageData)
        {
            spawnIntervalTimer = 0.0f;

            if (stageData.probDogeSpwan > 0)
            {
                DogeAnimations = new EnemyAnimations(new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_idle"), 1, 0.1f, true),
                    new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_up"), 3, 0.1f, true),
                    new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_down"), 3, 0.1f, true),
                    new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_right"), 3, 0.1f, true),
                    new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_left"), 3, 0.1f, true));

                int width = (int)(DogeAnimations.IdleAnimation.FrameWidth);
                int left = (DogeAnimations.IdleAnimation.FrameWidth - width) / 2;
                int height = (int)(DogeAnimations.IdleAnimation.FrameHeight * 2);
                int top = DogeAnimations.IdleAnimation.FrameHeight - height + 16;
                DogeLocalBounds = new Rectangle(left, top, width, height);
            }

            if (stageData.probSkullSpwan > 0)
            {
                SkullAnimations = new EnemyAnimations(new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_idle"), 1, 0.1f, true),
                    new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_up"), 3, 0.1f, true),
                    new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_down"), 3, 0.1f, true),
                    new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_right"), 3, 0.1f, true),
                    new Animation(content.Load<Texture2D>("Sprites/Enemies/Skull_left"), 3, 0.1f, true));

                int width = (int)(SkullAnimations.IdleAnimation.FrameWidth);
                int left = (SkullAnimations.IdleAnimation.FrameWidth - width) / 2;
                int height = (int)(SkullAnimations.IdleAnimation.FrameHeight * 2);
                int top = SkullAnimations.IdleAnimation.FrameHeight - height + 16;
                SkullLocalBounds = new Rectangle(left, top, width, height);
            }
            if (stageData.probBaldSpwan > 0)
            {
                BaldAnimations = new EnemyAnimations(new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_idle"), 1, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_up"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_down"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_right"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Bald_left"), 3, 0.1f, true));
                int width = (int)(BaldAnimations.IdleAnimation.FrameWidth);
                int left = (BaldAnimations.IdleAnimation.FrameWidth - width) / 2;
                int height = (int)(BaldAnimations.IdleAnimation.FrameHeight * 2);
                int top = BaldAnimations.IdleAnimation.FrameHeight - height + 16;
                BaldLocalBounds = new Rectangle(left, top, width, height);
            }
            if (stageData.probDarkSpwan > 0)
            {
                DarkAnimations = new EnemyAnimations(new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_idle"), 1, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_up"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_down"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_right"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Dark_left"), 3, 0.1f, true));

                int width = (int)(DarkAnimations.IdleAnimation.FrameWidth);
                int left = (DarkAnimations.IdleAnimation.FrameWidth - width) / 2;
                int height = (int)(DarkAnimations.IdleAnimation.FrameHeight * 2);
                int top = DarkAnimations.IdleAnimation.FrameHeight - height + 16;
                DarkLocalBounds = new Rectangle(left, top, width, height);
            }

        }

        public void Update(float deltaTime)
        {
            spawnIntervalTimer += deltaTime;
            
            // Random spawning with rand and values
            if (shouldSpawn && spawnIntervalTimer > 10)
            {
                spawnIntervalTimer = 0.0f;


                CreateEnemy("Skull", SkullLocalBounds, new Vector2(100, 100), 100, "chase");
                CreateEnemy("Skull", SkullLocalBounds, new Vector2(800, 500), 100, "chase");
                CreateEnemy("Skull", SkullLocalBounds, new Vector2(400, 900), 100, "chase");
                CreateEnemy("Skull", SkullLocalBounds, new Vector2(900, 100), 100, "chase");

                CreateEnemy("Doge", DogeLocalBounds, new Vector2(300, 300), 100, "idle");
                CreateEnemy("Doge", DogeLocalBounds, new Vector2(500, 500), 100, "idle");
                CreateEnemy("Doge", DogeLocalBounds, new Vector2(700, 100), 100, "idle");
                CreateEnemy("Doge", DogeLocalBounds, new Vector2(300, 800), 100, "idle");
            }
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

        // Spawn next enemy type based on the probbaility of the current stage
        private void SpawnEnemy()
        {
            double ran = random.NextDouble();
        }

        private void SpawnToggle(bool val)
        {
            shouldSpawn = val;
        }

    }
}
