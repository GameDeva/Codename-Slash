using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Codename___Slash.EnemyStates;

namespace Codename___Slash
{
    public class GameplayState : GameState
    {

        private Hero hero; // Hero instance for this gameplay session
        private Camera camera; // Camera instance for this gameplay session
        // private MapGenerator mapGenerator;
        private MapGen mapGen;
        private EnemyDirector enemyDirector;
        private PoolManager poolManager;
        private CollisionManager collisionManager;

        private UI ui; // UI instance for this gameplay session
        
        // Initialise the hero on the enter state 
        public override void Enter(Game1 game)
        {
            // Store reference to singleton Managers
            poolManager = PoolManager.Instance;
            collisionManager = CollisionManager.Instance;
            enemyDirector = EnemyDirector.Instance;
            // mapGenerator = MapGenerator.Instance;
            mapGen = new MapGen();

            // TODO : Based on serialization, saved hero could have several bits of data already stored i.e. weapons held, points scored 
            hero = new Hero(new Vector2(800, 500), stateContent);
            ui = new UI(stateContent, ref hero);
            
            camera = new Camera();

            //mapGenerator.Initialise(game.Services);
            //mapGenerator.InitialiseNewMap(1);
            mapGen.Initialise(game.Services);
            mapGen.GetMapData("Walkway");
            mapGen.GetMapData("BattleArena");
            mapGen.LoadMapTextures("Walkway");
            mapGen.LoadMapTextures("BattleArena");

            // Initialise the EnemyDirector Singleton
            // IMPORTANT: Order, collisionmanager must be initalised first
            collisionManager.Initialise();
            enemyDirector.Initialise(hero, stateContent);
            poolManager.Initialise(hero);

            
            EnemyDirector.Instance.CreateEnemies();

            base.Enter(game);
            
        }

        protected override void LoadContent() 
        {
            hero.LoadContent(stateContent);
            ui.LoadContent(stateContent);
        }

        public override void Exit(Game1 game)
        {
            base.Exit(game);
        }

        protected override void InitialiseKeyBindings()
        {
            if(commandManager != null)
            {
                commandManager.AddKeyboardBinding(Keys.W, hero.MoveUp);
                commandManager.AddKeyboardBinding(Keys.D, hero.MoveRight);
                commandManager.AddKeyboardBinding(Keys.A, hero.MoveLeft);
                commandManager.AddKeyboardBinding(Keys.S, hero.MoveDown);
                commandManager.AddKeyboardBinding(Keys.Space, hero.Dash);

                commandManager.AddMouseBinding(MouseButton.LEFT, hero.ShootWeapon);
                commandManager.AddMouseBinding(MouseButton.RIGHT, hero.ShootWeapon);
                commandManager.AddScrollBinding(Scroll.DOWN, hero.PreviousWeapon);
                commandManager.AddScrollBinding(Scroll.UP, hero.NextWeapon);
            }
        }


        public override GameState Update(Game1 game, ref GameTime gameTime, ref InputHandler inputHandler)
        {
            
            // Handle State object Updates
            commandManager.Update();
            hero.Update(gameTime);
            poolManager.Update(gameTime);
            collisionManager.Update();
            Camera.Follow(hero);
            ui.Update();


            base.Update(game, ref gameTime, ref inputHandler);
            return null;
        }

        public override void Draw(ref GameTime gameTime, SpriteBatch spriteBatch)
        {
            // transformMatrix: camera.Transform [add as parameter]

            spriteBatch.Begin();
            // mapGenerator.Draw(spriteBatch);
            mapGen.DrawMap("BattleArena", spriteBatch);
            poolManager.Draw(gameTime, spriteBatch);

            
            
            hero.Draw(gameTime, spriteBatch);
            
            ui.Draw(spriteBatch);
            spriteBatch.End();



            base.Draw(ref gameTime, spriteBatch);

        }
        
    }
}
