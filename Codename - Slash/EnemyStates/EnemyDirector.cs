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
    // Enemy Director Singleton
    public class EnemyDirector
    {
        // Single creation
        private static EnemyDirector instance;
        public static EnemyDirector Instance { get { if (instance == null) { instance = new EnemyDirector(); return instance; } return instance; } set { instance = value; } }

        private StageManager stageManager;
        private PoolManager poolManager;
        private ContentManager content;


        #region Enemy spawner members
        private Random random;
        private bool shouldSpawn;
        private float spawnIntervalTimer;
        private float currentIntervalBetweenSpawns;
        private List<Point> spawnPoints;
        public float probDogeSpwan;
        public float probBaldSpwan;
        public float probSkullSpwan;
        public float probDarkSpwan;
        public float probHealthDrop;
        public float probAmmoDrop;
        public int enemiesToKillToWin;
        public int maxDogeCount;
        public int maxBaldCount;
        public int maxSkullCount;
        public int maxDarkCount;

        public int currentDogeCount;
        public int currentBaldCount;
        public int currentSkullCount;
        public int currentDarkCount;

        public int killCount;
        #endregion

        private Hero hero;

        // Actions
        public Action<IArgs> OnCreateDoge;
        public Action<IArgs> OnCreateBald;
        public Action<IArgs> OnCreateSkull;
        public Action<IArgs> OnCreateDark;
        public Action<int> ScoreIncrement;
        public Action OnBeatAllEnemies;

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
            this.hero = hero;
            this.content = content;
            // stageManager = StageManager.Instance;
            poolManager = PoolManager.Instance;
            random = new Random();
            spawnPoints = new List<Point>();

            // stageManager.OnNewStage += OnNewStage;
            // stageManager.ToggleEnemyGeneration += SpawnToggle;

            poolManager.OnDeath += OnEnemyDeath;


        }

        // On enter new stage
        public void OnNewStage(StageData stageData)
        {
            // Get all stage data 
            enemiesToKillToWin = stageData.enemiesToFight;
            currentIntervalBetweenSpawns = stageData.intervalBetweenSpawn;
            // spawnPoints = new List<Point>(stageData.spawnPointCount);
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
            currentBaldCount = 0;
            currentDarkCount = 0;
            currentDogeCount = 0;
            currentSkullCount = 0;
            killCount = 0;

            LoadEnemyContent(stageData);
        }

        // Get set number of portals with 
        private void SetupSpawnPoints(int count)
        {
            if (spawnPoints.Count > 0)
                spawnPoints.Clear();

            for(int i = 0; i < count; i++)
            {
                spawnPoints.Add(new Point(random.Next(32, Game1.SCREENWIDTH - 31), random.Next(32, Game1.SCREENHEIGHT - 31)));
            }
        }

        private Point GetRandomSpawnPoint()
        {
            if(spawnPoints.Count != 0)
            {
                return spawnPoints[random.Next(0, spawnPoints.Count)];
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
                    CreateEnemy("Doge", DogeLocalBounds, p.ToVector2(), 100, "chase");
                    currentDogeCount++;
                }
                return;
            }
            else if(r < probDogeSpwan + probSkullSpwan && probSkullSpwan != 0) // Skull
            {
                if (currentSkullCount < maxSkullCount)
                {
                    CreateEnemy("Skull", SkullLocalBounds, p.ToVector2(), 100, "chase");
                    currentSkullCount++;
                }
                return;
            }
            else if(r < probDogeSpwan + probSkullSpwan + probBaldSpwan && probBaldSpwan != 0) // Bald
            {
                if (currentBaldCount < maxBaldCount)
                {
                    CreateEnemy("Bald", BaldLocalBounds, p.ToVector2(), 100, "chase");
                    currentBaldCount++;
                }
                return;
            }
            else if(currentDarkCount < maxDarkCount && probDarkSpwan != 0) // Dark
            {
                CreateEnemy("Dark", DarkLocalBounds, p.ToVector2(), 100, "rest");
                currentDarkCount++;
                return;
            }

        }

        // Load in all enemy files only if the probability of spawning in this stage is high
        private void LoadEnemyContent(StageData stageData)
        {
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

        public bool Update(float deltaTime)
        {
            spawnIntervalTimer += deltaTime;
            
            if (killCount >= enemiesToKillToWin)
            {
                return true;
            }

            // Spawning should be allowed, must have finished interval, must not spawn too many enemies
            if (spawnIntervalTimer > currentIntervalBetweenSpawns && (currentDogeCount < maxDogeCount || currentSkullCount < maxSkullCount || currentBaldCount < maxBaldCount || currentDarkCount < maxDarkCount))
            {
                spawnIntervalTimer = 0.0f;
                SpawnEnemy();
            }

            return false;
        
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

        private void OnEnemyDeath(Enemy enemy)
        {
            if (enemy is Doge)
            {
                if(currentDogeCount>0)
                {
                    ScoreIncrement?.Invoke(10);
                    currentDogeCount--;
                }
            } else if(enemy is Skull)
            {
                if (currentSkullCount > 0)
                {
                    ScoreIncrement?.Invoke(25);
                    currentSkullCount--;
                }
            }
            else if (enemy is Bald)
            {
                if (currentBaldCount > 0)
                {
                    ScoreIncrement?.Invoke(50);
                    currentBaldCount--;
                }
            }
            else if (enemy is Dark)
            {
                if (currentDarkCount > 0)
                {
                    currentDarkCount--;
                    ScoreIncrement?.Invoke(100);
                }
            }
            killCount++;
            
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

        private void SpawnToggle(bool val)
        {
            shouldSpawn = val;
        }

    }
}
