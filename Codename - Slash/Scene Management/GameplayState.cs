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
using System.IO;

namespace Codename___Slash
{
    public class GameplayState : GameState
    {
        // private Camera camera; // Camera instance for this gameplay session
        // private MapGenerator mapGenerator;
        private EnemyDirector enemyDirector;
        private PoolManager poolManager;
        private CollisionManager collisionManager;
        private GameManager gameManager;
        private MapGen mapGen;

        public int CurrentScore { get; private set; }
        public int CurrentStage { get; private set; }
        public StageData CurrentStageData { get; private set; }

        public GameplayUI UI { get; set; } // UI instance for this gameplay session

        private Timer deathTimer = new Timer(1.0f);

        // Initialise the hero on the enter state 
        public override void Enter(Game1 game)
        {
            

            // Store reference to singleton Managers
            mapGen = MapGen.Instance;
            poolManager = PoolManager.Instance;
            collisionManager = CollisionManager.Instance;
            enemyDirector = EnemyDirector.Instance;
            gameManager = GameManager.Instance;

            UI = new GameplayUI(stateContent);

            // Initialise the EnemyDirector Singleton
            // IMPORTANT: Order, collisionmanager must be initalised first
            collisionManager.Initialise();
            enemyDirector.Initialise(gameManager.Hero, stateContent);
            poolManager.Initialise(gameManager.Hero);

            mapGen.Initialise(game.Services);
            mapGen.GetMapData("Walkway");
            mapGen.GetMapData("BattleArena");
            
            mapGen.LoadMapTextures("BattleArena");
            mapGen.AssignMapToDraw("BattleArena");

            mapGen.ChangeMapColliders(MapCollider.BattleArena);

            // Continue based on load or new game
            UI.Initialise(GameManager.Instance.CurrentSaveData, gameManager.Hero);

            UpdateStageData(CurrentStage);
            enemyDirector.OnNewStage(CurrentStageData);
            poolManager.CreateStageSpecificPools(CurrentStageData);
            
            // Attach events
            gameManager.Hero.OnDeath += OnHeroDeath;

            base.Enter(game);
            
        }

        protected override void LoadContent() 
        {
            // Load content from all managers
            gameManager.Hero.LoadContent(stateContent);
            UI.LoadContent();
        }

        protected override void UnloadContent()
        {
            // Unload all obj's content
            poolManager.DeleteStageSpecificPools();

            base.UnloadContent();
        }

        public override void Exit(Game1 game)
        {
            // destroy all objects and pools

            
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

            // If returns true then level has been complete
            if(enemyDirector.Update(deltaTime))
            {
                return GameOverState;
            }

            collisionManager.Update();
            poolManager.Update(deltaTime);
            // Camera.Follow(hero);
            UI.Update();
            
            // If hero is dead, death timer would have started automatically 
            // Wait for death timer to pass, then return game over state
            if(gameManager.Hero.Dead)
            {
                deathTimer.Update(deltaTime);
                // When timer is done
                if(!deathTimer.Running)
                {
                    return GameOverState;
                }
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
            
            UI.Draw(spriteBatch);

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
        public void UpdateStageData(int n)
        {
            StageData s = new StageData();
            Loader.ReadXML(string.Format("Content/StageData/Stage{0}.xml", n), ref s);
            CurrentStageData = s;
        }

    }
}
