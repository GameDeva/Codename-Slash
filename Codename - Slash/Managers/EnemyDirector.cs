using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Codename___Slash.EnemyStates;

namespace Codename___Slash
{
    // Enemy Director, contains logic and calls to spawn enemies and pickups
    public class EnemyDirector
    {
        // Singleton creation
        private static EnemyDirector instance;
        public static EnemyDirector Instance { get { if (instance == null) { instance = new EnemyDirector(); return instance; } return instance; } set { instance = value; } }

        private PoolManager poolManager;
        private ContentManager content;

        // C# random object used to generate random numbers
        private Random random;

        #region Enemy Spawner members
        private bool shouldSpawn; // Should spawn more enemies
        private float spawnIntervalTimer; // Max Interval time between spawns
        private float currentIntervalBetweenSpawns; // Timer to the interval 

        // List of spawnPoints enemies can be spawned on
        public List<Point> Spawnpoints { get; private set; }

        // Probability of enemies spawning
        public float probDogeSpwan;
        public float probBaldSpwan;
        public float probSkullSpwan;
        public float probDarkSpwan;

        // Probability of ammo or health dropping on enemy death
        public float probHealthDrop;
        public float probAmmoDrop;

        // Number of enemies to kill to complete stage
        public int enemiesToKillThisStage;
        // Total enemies that have been spawned since the beginning of the stage
        public int totalEnemiesSpawned;
        
        // Max number of each type of enemy that can be spawned at 1 time
        public int maxDogeCount;
        public int maxBaldCount;
        public int maxSkullCount;
        public int maxDarkCount;

        // Current number of each type of enemy
        public int currentDogeCount;
        public int currentBaldCount;
        public int currentSkullCount;
        public int currentDarkCount;

        // Number of enemies killed
        public int killCount;
        #endregion

        private Hero hero;

        // Actions
        // On Enemy creation
        public Action<IArgs> OnCreateDoge;
        public Action<IArgs> OnCreateBald;
        public Action<IArgs> OnCreateSkull;
        public Action<IArgs> OnCreateDark;
        // On Score gained
        public Action<int> ScoreIncrement;
        // On enemies destroyed for this stage
        public Action OnStageEnemiesDestroyed;
        // On effect create
        public Action<IArgs> createEffect;

        // Textures
        public Texture2D EnemyBulletTexture { get; private set; }
        // Animations
        public EnemyAnimations DogeAnimations { get; private set; }
        public EnemyAnimations SkullAnimations { get; private set; }
        public EnemyAnimations BaldAnimations { get; private set; }
        public EnemyAnimations DarkAnimations { get; private set; }
        //
        public Animation PortalAnimation { get; private set; }
        public Animator PortalAnimator { get; private set; }
        // 
        public Animation ExplosionAnimation { get; private set; }
        public Animation Bloodanimation { get; private set; }
        
        // Localbounds
        public Rectangle DogeLocalBounds { get; private set; }
        public Rectangle SkullLocalBounds { get; private set; }
        public Rectangle BaldLocalBounds { get; private set; }
        public Rectangle DarkLocalBounds { get; private set; }

        public void Initialise()
        {
            poolManager = PoolManager.Instance;

            // 
            random = new Random();
            Spawnpoints = new List<Point>();
            PortalAnimator = new Animator();
            
            // Attach events 
            poolManager.OnDeath += OnEnemyDeath;
        }

        // Initialise Enemy Director
        public void ReInitialise(Hero hero, IServiceProvider services)
        {

            // Attach references
            this.hero = hero;
            if(content == null)
            {
                // Has its own contentManager, to flush at the end of each stage
                content = new ContentManager(services); 
                content.RootDirectory = "Content";
            }
        }

        // On enter new stage
        // Attach all stage data
        public void OnNewStage(StageData stageData)
        {
            enemiesToKillThisStage = stageData.enemiesToFight;
            currentIntervalBetweenSpawns = stageData.intervalBetweenSpawn;

            probDogeSpwan = stageData.probDogeSpwan;
            probBaldSpwan = stageData.probBaldSpwan;
            probSkullSpwan = stageData.probSkullSpwan;
            probDarkSpwan = stageData.probDarkSpwan;
            maxDogeCount = stageData.maxDogeCount;
            maxSkullCount = stageData.maxSkullCount;
            maxBaldCount = stageData.maxSkullCount;
            maxDarkCount = stageData.maxDarkCount;

            probAmmoDrop = stageData.probAmmoDrop;
            probHealthDrop = stageData.probHealthDrop;

            SetupSpawnPoints(stageData.spawnPointCount);
            
            // Reset old values
            spawnIntervalTimer = 0.0f;
            totalEnemiesSpawned = 0;
            currentBaldCount = 0;
            currentDarkCount = 0;
            currentDogeCount = 0;
            currentSkullCount = 0;
            killCount = 0;

            // Load content based on stage data
            LoadContent(stageData);
        }

        // Set number of portals with 
        private void SetupSpawnPoints(int count)
        {
            if (Spawnpoints.Count > 0)
                Spawnpoints.Clear();

            for(int i = 0; i < count; i++)
            {
                Spawnpoints.Add(new Point(random.Next(200, Game1.SCREENWIDTH - 199), random.Next(200, Game1.SCREENHEIGHT - 199)));
            }
        }

        private Point GetRandomSpawnPoint()
        {
            if(Spawnpoints.Count != 0)
            {
                return Spawnpoints[random.Next(0, Spawnpoints.Count)];
            }
            return new Point(0);
        }

        // Spawn next enemy type based on the probbaility 
        private void SpawnEnemy()
        {
            double r = 1 - random.NextDouble();
            Point p = GetRandomSpawnPoint();

            if(r < probDogeSpwan && probDogeSpwan != 0) // Doge
            {
                if(currentDogeCount < maxDogeCount)
                {
                    CreateEnemy("Doge", DogeLocalBounds, p.ToVector2(), 100, "idle");
                    currentDogeCount++;
                    totalEnemiesSpawned++;
                }
                return;
            }
            else if(r < probDogeSpwan + probSkullSpwan && probSkullSpwan != 0) // Skull
            {
                if (currentSkullCount < maxSkullCount)
                {
                    CreateEnemy("Skull", SkullLocalBounds, p.ToVector2(), 100, "chase");
                    currentSkullCount++;
                    totalEnemiesSpawned++;
                }
                return;
            }
            else if(r < probDogeSpwan + probSkullSpwan + probBaldSpwan && probBaldSpwan != 0) // Bald
            {
                if (currentBaldCount < maxBaldCount)
                {
                    CreateEnemy("Bald", BaldLocalBounds, p.ToVector2(), 100, "idle");
                    currentBaldCount++;
                    totalEnemiesSpawned++;
                }
                return;
            }
            else if(currentDarkCount < maxDarkCount && probDarkSpwan != 0) // Dark
            {
                CreateEnemy("Dark", DarkLocalBounds, p.ToVector2(), 100, "idle");
                currentDarkCount++;
                totalEnemiesSpawned++;
                return;
            }

        }

        // Load in all enemy files only if the probability of spawning in this stage is high
        private void LoadContent(StageData stageData)
        {
            // Texture
            EnemyBulletTexture = content.Load<Texture2D>("Sprites/Enemies/enemyBullet");

            // Portal animation
            PortalAnimation = new Animation(content.Load<Texture2D>("Sprites/Enemies/portal"), 4, 0.6f, true);
            PortalAnimator.AttachAnimation(PortalAnimation);

            // Explosion animation
            ExplosionAnimation = new Animation(content.Load<Texture2D>("Sprites/Enemies/explosion_death"), 39, 0.01f, false);
            
            // Blood animation 
            Bloodanimation = new Animation(content.Load<Texture2D>("Sprites/Enemies/blood"), 6, 0.02f, false);

            // ---
            // Enemy animations
            // ---
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

        // Unloads the enemyDirector's contentManager
        public void UnloadContent()
        {
            content.Unload();
        }

        // Update the enemy director
        public void Update(float deltaTime)
        {
            spawnIntervalTimer += deltaTime;
            // If all enemies killed in current stage, call event
            if (killCount >= enemiesToKillThisStage)
            {
                OnStageEnemiesDestroyed?.Invoke();
            }

            // Spawning should be allowed, must have finished interval, must not spawn too many enemies
            if (spawnIntervalTimer > currentIntervalBetweenSpawns && (currentDogeCount < maxDogeCount || currentSkullCount < maxSkullCount || currentBaldCount < maxBaldCount || currentDarkCount < maxDarkCount) && totalEnemiesSpawned < enemiesToKillThisStage)
            {
                spawnIntervalTimer = 0.0f;
                SpawnEnemy();
            }
        }

        // Returns square distance to hero from given position
        public float SqrDistanceToHeroFrom(Vector2 position)
        {
            // TODO: Check for performance bottleneck
            return Vector2.DistanceSquared(position, hero.Position);
        }
            
        // Returns direction to hero from given position
        public Vector2 DirectionToHeroNormalised(Vector2 position)
        {
            return Vector2.Normalize(hero.Position - position);
        }

        // Returns current position of hero
        public Vector2 GetHeroPosition()
        {
            return hero.Position;
        }

        public void OnEnemyHit(Vector2 pos)
        {
            createEffect?.Invoke(new ArgsEffect(pos, 0.12f, Bloodanimation));
        }

        // On Enemy death, increases score and removes from appropriate counts
        private void OnEnemyDeath(Enemy enemy)
        {
            // Create explosion effect
            createEffect?.Invoke(new ArgsEffect(enemy.Position, ExplosionAnimation.FrameTime * ExplosionAnimation.FrameCount, ExplosionAnimation));
            
            // Increment score
            ScoreIncrement?.Invoke(enemy.KillScore);

            // Increase killCount
            killCount++;
            
            //
            // Decrease enemy type count present
            if (enemy is Doge)
            {
                if(currentDogeCount>0)
                {
                    currentDogeCount--;
                }
            } else if(enemy is Skull)
            {
                if (currentSkullCount > 0)
                {
                    currentSkullCount--;
                }
            }
            else if (enemy is Bald)
            {
                if (currentBaldCount > 0)
                {
                    currentBaldCount--;
                }
            }
            else if (enemy is Dark)
            {
                if (currentDarkCount > 0)
                {
                    currentDarkCount--;
                }
            }

        }

        // Create enemy type with given values
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
