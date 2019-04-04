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
        private Hero hero; // Hero instance for this gameplay session
        // private Camera camera; // Camera instance for this gameplay session
        // private MapGenerator mapGenerator;
        private EnemyDirector enemyDirector;
        private PoolManager poolManager;
        private CollisionManager collisionManager;
        private MapGen mapGen;

        public int CurrentScore { get; private set; }
        public int CurrentStage { get; private set; }
        public StageData CurrentStageData { get; private set; }

        private GameplayUI ui; // UI instance for this gameplay session

        private Timer deathTimer = new Timer(3.0f);

        // Contains saved data if any
        private SaveData currentSaveData;

        // Initialise the hero on the enter state 
        public override void Enter(Game1 game)
        {
            // Store reference to singleton Managers
            mapGen = MapGen.Instance;
            poolManager = PoolManager.Instance;
            collisionManager = CollisionManager.Instance;
            enemyDirector = EnemyDirector.Instance;

            hero = new Hero(stateContent);
            ui = new GameplayUI(stateContent);

            // Initialise the EnemyDirector Singleton
            // IMPORTANT: Order, collisionmanager must be initalised first
            collisionManager.Initialise();
            enemyDirector.Initialise(hero, stateContent);
            poolManager.Initialise(hero);

            mapGen.Initialise(game.Services);
            mapGen.GetMapData("Walkway");
            mapGen.GetMapData("BattleArena");
            
            mapGen.LoadMapTextures("BattleArena");
            mapGen.AssignMapToDraw("BattleArena");

            mapGen.ChangeMapColliders(MapCollider.BattleArena);

            // Continue based on load or new game
            SetupSession();
            ui.Initialise(currentSaveData, ref hero);

            UpdateStageData(1);
            enemyDirector.OnNewStage(CurrentStageData);
            poolManager.CreateStageSpecificPools(CurrentStageData);
            
            // Attach events
            hero.OnDeath += OnHeroDeath;

            base.Enter(game);
            
        }

        protected override void LoadContent() 
        {
            // Load content from all managers
            hero.LoadContent(stateContent);
            ui.LoadContent();
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


        public override GameState Update(Game1 game, float deltaTime, ref InputHandler inputHandler)
        {
            // Handle State object Updates
            commandManager.Update();
            hero.Update(deltaTime);

            // If returns true then level has been complete
            if(enemyDirector.Update(deltaTime))
            {
                return GameOverState;
            }

            collisionManager.Update();
            poolManager.Update(deltaTime);
            // Camera.Follow(hero);
            ui.Update();
            
            // If hero is dead, death timer would have started automatically 
            // Wait for death timer to pass, then return game over state
            if(hero.Dead)
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
            poolManager.Draw(deltaTime, spriteBatch);

            
            
            hero.Draw(deltaTime, spriteBatch);
            
            ui.Draw(spriteBatch);

            // collisionManager.DebugDraw(spriteBatch);
            spriteBatch.End();



            base.Draw(deltaTime, spriteBatch);

        }
        
        private void OnHeroDeath()
        {
            deathTimer.Start();
        }

        // Called when a gameplay session starts
        public void SetupSession()
        {
            // Continue Game from saveddata
            if (currentSaveData != null)
            {
                SetupContinueGame();
            }
            else
            {
                SetupNewGame();
            }

            // Setup session values
            CurrentScore = currentSaveData.currentScore;
            CurrentStage = currentSaveData.stageNumber;
            for(int i = 0; i < hero.WeaponHandler.WeaponsList.Count; i++)
            {
                hero.WeaponHandler.WeaponsList[i].CurrentAmmoCarry = currentSaveData.weaponDataList[i].currentAmmoCarry;
                hero.WeaponHandler.WeaponsList[i].CurrentMagHold = currentSaveData.weaponDataList[i].currentMagHold;
            }
            
        }

        // Uses saveddata to load game
        private void SetupContinueGame()
        {
            // If no save file
            if (!LoadSaveFile())
                currentSaveData = new SaveData();
        }

        // Sets up new game
        private void SetupNewGame()
        {
            if (File.Exists("SaveFile.xml"))
            {
                DeleteSaveFile();
            }
            currentSaveData = new SaveData();
        }

        // Load saved file, returns true if successful
        private bool LoadSaveFile()
        {
            if (File.Exists("SaveFile.xml"))
            {
                Loader.ReadXML("SaveFile.xml", ref currentSaveData);
                return true;
            }
            return false;
        }

        private void UpdateSaveFile(SaveData saveData)
        {
            this.currentSaveData = saveData;
            Loader.ToXmlFile(saveData, "SaveFile.xml");
        }

        private void DeleteSaveFile()
        {
            File.Delete("SaveFile.xml");
        }

        // Load given stage
        public void UpdateStageData(int n)
        {
            StageData s = new StageData();
            Loader.ReadXML(string.Format("Content/StageData/Stage{0}.xml", n), ref s);
            CurrentStageData = s;
        }

    }
}
