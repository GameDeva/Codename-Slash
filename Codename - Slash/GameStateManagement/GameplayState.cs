using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Codename___Slash.EnemyStates;
using System.IO;
using Codename___Slash.UIRelated;

namespace Codename___Slash.GameStateManagement
{
    // Gameplay State of the game
    public class GameplayState : GameState
    {
        private Thread stageLoaderThread;

        private SpriteFont hudFont;
        
        // Reference to all the singleton Managers
        private EnemyDirector enemyDirector;
        private PoolManager poolManager;
        private CollisionManager collisionManager;
        private GameManager gameManager;
        // Map Generator object load and draw a map
        private MapGen mapGen;
        
        public GameplayUI GameplayUI { get; set; } // UI instance for this gameplay session

        // Timer before leaving gameplay state after hero's death
        private Timer deathTimer = new Timer(1.0f);
        private bool waitingToBegin = false;


        // Initialise state
        public override void InitialiseState(Game1 game)
        {
            // Store reference to singleton Managers
            mapGen = MapGen.Instance;
            poolManager = PoolManager.Instance;
            collisionManager = CollisionManager.Instance;
            enemyDirector = EnemyDirector.Instance;
            gameManager = GameManager.Instance;

            collisionManager.Initialise();
            enemyDirector.Initialise();
            poolManager.Initialise();

            // Attach events
            enemyDirector.OnStageEnemiesDestroyed += OnStageBegin;
            // gameManager.Hero.OnDeath += OnHeroDeath;


            // Initialise 
            mapGen.Initialise(game.Services);
            mapGen.GetMapData("BattleArena");
            

            base.InitialiseState(game);
        }
        
        // 
        private void OnStageBegin()
        {
            // If all stages complete (true), display game over screen
            if (gameManager.NextStage())
            {

            } else
            {
                stageLoaderThread = new Thread(BackgroundLoadStage);
                stageLoaderThread.Start();
            }
        }

        // Unload previous assets and objects and
        // Load assets and objects needed for next stage
        //  note: should be called on separate thread
        private void BackgroundLoadStage()
        {
            waitingToBegin = true;

            // Save Game
            gameManager.SaveGame();

            // Thread.Sleep(2000);
            poolManager.ClearPoolsForNextStage();
            
            // Load all relevant assets
            enemyDirector.OnNewStage(gameManager.CurrentStageData);
            // Create relevant object pools
            poolManager.CreateStageSpecificPools(gameManager.CurrentStageData);
            
            waitingToBegin = false;
        }


        // Initialise the hero on the enter state 
        public override void Enter(Game1 game)
        {
            // Initialise the EnemyDirector Singleton
            // IMPORTANT: Order, collisionmanager must be initalised first
            collisionManager.ReInitialise();
            enemyDirector.ReInitialise(gameManager.Hero, game.Services);
            poolManager.ReInitialise(gameManager.Hero);

            OnStageBegin();

            GameplayUI = new GameplayUI(stateContentManager);
            // Continue based on load or new game
            GameplayUI.Initialise(GameManager.Instance.CurrentSaveData, gameManager.Hero);
            
            base.Enter(game);
            
        }

        protected override void LoadContent() 
        {
            // Load content for hero
            gameManager.Hero.LoadContent(stateContentManager);
            // Load gameplayui content
            GameplayUI.LoadContent();

            // Load fonts
            hudFont = stateContentManager.Load<SpriteFont>("UI/Fonts/Hud");

            // Load and assign map
            mapGen.LoadMapTextures("BattleArena");
            mapGen.AssignMapToDraw("BattleArena");
            mapGen.ChangeMapColliders(MapCollider.BattleArena);

        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Exit(Game1 game)
        {
            collisionManager.RemoveAll();
            poolManager.ClearAllPools();
            enemyDirector.UnloadContent();

            base.Exit(game);
        }

        protected override void InitialiseKeyBindings()
        {
            if(commandManager != null)
            {
                commandManager.AddKeyboardBinding(Keys.W, gameManager.Hero.MoveUp);
                commandManager.AddKeyboardBinding(Keys.D, gameManager.Hero.MoveRight);
                commandManager.AddKeyboardBinding(Keys.A, gameManager.Hero.MoveLeft);
                commandManager.AddKeyboardBinding(Keys.S, gameManager.Hero.MoveDown);
                commandManager.AddKeyboardBinding(Keys.Space, gameManager.Hero.Dash);

                commandManager.AddMouseBinding(MouseButton.LEFT, gameManager.Hero.ShootWeapon);
                commandManager.AddMouseBinding(MouseButton.RIGHT, gameManager.Hero.ShootWeapon);
                commandManager.AddScrollBinding(Scroll.DOWN, gameManager.Hero.PreviousWeapon);
                commandManager.AddScrollBinding(Scroll.UP, gameManager.Hero.NextWeapon);
            }
        }


        public override GameState Update(Game1 game, float deltaTime, ref InputHandler inputHandler)
        {
            // Handle State object Updates
            commandManager.Update();
            gameManager.Hero.Update(deltaTime);

            // Update enemyDirector
            enemyDirector.Update(deltaTime);

            collisionManager.Update();
            poolManager.Update(deltaTime);
            // Camera.Follow(hero);
            GameplayUI.Update();
            
            // If hero is dead, death timer would have started automatically 
            // Wait for death timer to pass, then return game over state
            if(gameManager.Hero.Dead)
            {
              
                return GameOverState;
            }

            base.Update(game, deltaTime, ref inputHandler);
            return null;
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            // transformMatrix: camera.Transform [add as parameter]

            spriteBatch.Begin();

            mapGen.DrawMap(spriteBatch);
            // stageManager.Draw(deltaTime, spriteBatch);
            foreach (Point point in enemyDirector.Spawnpoints)
            {
                enemyDirector.PortalAnimator.Draw(deltaTime, spriteBatch, point.ToVector2(), SpriteEffects.None, Color.White, 0.2f);
            }

            poolManager.Draw(deltaTime, spriteBatch);
            

            gameManager.Hero.Draw(deltaTime, spriteBatch);
            
            GameplayUI.Draw(spriteBatch);

            // If waiting to begin (loading next stage) 
            if (waitingToBegin)
            {
                spriteBatch.DrawString(hudFont, string.Format("Creating Stage: {0}", gameManager.CurrentStage), new Vector2(Game1.SCREENWIDTH / 2, Game1.SCREENHEIGHT / 2), Color.White);
            }

            // collisionManager.DebugDraw(spriteBatch);
            spriteBatch.End();



            base.Draw(deltaTime, spriteBatch);

        }
        
        private void OnHeroDeath()
        {
            deathTimer.Start();
        }

        //private void SaveGame()
        //{
        //    CurrentScore = 1312;
        //    CurrentStage = 2;

        //    SaveData saveData = new SaveData();
        //    saveData.currentScore = CurrentScore;
        //    saveData.stageNumber = CurrentStage;

        //    // Test
        //    saveData.weaponDataList = new List<WeaponSaveData>();
        //    saveData.weaponDataList.Add(new WeaponSaveData(500, 23));
        //    saveData.weaponDataList.Add(new WeaponSaveData(30, 2));

        //    //saveData.weaponDataList = new List<WeaponSaveData>();
        //    //foreach(Weapon w in hero.WeaponHandler.WeaponsList)
        //    //{
        //    //    saveData.weaponDataList.Add(new WeaponSaveData(w.CurrentAmmoCarry, w.CurrentMagHold));
        //    //}

        //    Loader.ToXmlFile(saveData, "SaveFile.xml");
        //}

        // Load given stage

    }
}
