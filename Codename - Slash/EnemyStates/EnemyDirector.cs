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

        public void Initialise(Hero hero, ContentManager content)
        {
            enemiesAlive = new List<Enemy>();
            this.hero = hero;

            DogeAnimations = new EnemyAnimations(new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_idle"), 1, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_up"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_down"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_right"), 3, 0.1f, true),
                new Animation(content.Load<Texture2D>("Sprites/Enemies/Doge_left"), 3, 0.1f, true));

            //enemiesAlive.Add(new Doge(DogeAnimations, new Vector2(300, 300)));
            //enemiesAlive.Add(new Doge(DogeAnimations, new Vector2(500, 500)));
            //enemiesAlive.Add(new Doge(DogeAnimations, new Vector2(700, 100)));
            //enemiesAlive.Add(new Doge(DogeAnimations, new Vector2(300, 800)));

        }

        public void Create()
        {

            CreateEnemy("Doge", new Vector2(300, 300), 100, "idle");
            CreateEnemy("Doge", new Vector2(500, 500), 100, "idle");
            CreateEnemy("Doge", new Vector2(700, 100), 100, "idle");
            CreateEnemy("Doge", new Vector2(300, 800), 100, "idle");
        }

        public void Update(GameTime gameTime)
        {

            int size = enemiesAlive.Count();
            for(int i = 0; i < size; i++)
            {
                enemiesAlive[i].Update(gameTime);
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int size = enemiesAlive.Count();
            for (int i = 0; i < size; i++)
            {
                enemiesAlive[i].Draw(gameTime, spriteBatch);
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

        private void CreateEnemy(string type, Vector2 spawnPoint, float startingHealth, string initialState)
        {
            if(type.Equals("DOGE", StringComparison.OrdinalIgnoreCase)) 
            {
                OnCreateDoge?.Invoke(new ArgsEnemy(spawnPoint, startingHealth, initialState));
            }
            else if(type.Equals("Bald", StringComparison.OrdinalIgnoreCase))
            {

            }
            else if (type.Equals("Skull", StringComparison.OrdinalIgnoreCase))
            {

            }
            else if(type.Equals("Dark", StringComparison.OrdinalIgnoreCase))
            {

            } else
            {
                Console.WriteLine("Enemy creation type not recognised");
            }

        }


    }
}
