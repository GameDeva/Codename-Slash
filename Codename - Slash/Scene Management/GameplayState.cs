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
        // private Camera camera; // Camera instance for this gameplay session
        // private MapGenerator mapGenerator;
        private MapGen mapGen;
        private EnemyDirector enemyDirector;
        private PoolManager poolManager;
        private CollisionManager collisionManager;
        private LevelManager levelManager;

        public int CurrentScore { get; private set; }
        public int CurrentStage { get; private set; }

        private GameplayUI ui; // UI instance for this gameplay session

        private Timer deathTimer = new Timer(3.0f);

        public Action<SaveData> OnSaveData;
        
        // Initialise the hero on the enter state 
        public override void Enter(Game1 game)
        {
            // Store reference to singleton Managers
            poolManager = PoolManager.Instance;
            collisionManager = CollisionManager.Instance;
            enemyDirector = EnemyDirector.Instance;
            mapGen = MapGen.Instance;
            levelManager = LevelManager.Instance;

            // TODO : Based on serialization, saved hero could have several bits of data already stored i.e. weapons held, points scored 
            hero = new Hero(new Vector2(600, 1000), stateContent);
            ui = new GameplayUI(stateContent, ref hero);
            
            // camera = new Camera();

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

            
            mapGen.AssignMapToDraw("BattleArena");
            mapGen.ChangeMapColliders(MapCollider.BattleArena);
             

            EnemyDirector.Instance.CreateEnemies();

            hero.OnDeath += OnHeroDeath;

            SaveGame();

            base.Enter(game);
            
        }

        protected override void LoadContent() 
        {
            hero.LoadContent(stateContent);
            ui.LoadContent();
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


        public override GameState Update(Game1 game, float deltaTime, ref InputHandler inputHandler)
        {
            if(!deathTimer.Running)
            {
                // Handle State object Updates
                commandManager.Update();
                hero.Update(deltaTime);
                poolManager.Update(deltaTime);
                collisionManager.Update();
                // Camera.Follow(hero);
                ui.Update();

            } else if()


            base.Update(game, deltaTime, ref inputHandler);
            return null;
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            // transformMatrix: camera.Transform [add as parameter]

            spriteBatch.Begin();
            // mapGenerator.Draw(spriteBatch);
            mapGen.DrawMap(spriteBatch);
            poolManager.Draw(deltaTime, spriteBatch);

            
            
            hero.Draw(deltaTime, spriteBatch);
            
            ui.Draw(spriteBatch);

            collisionManager.DebugDraw(spriteBatch);
            spriteBatch.End();



            base.Draw(deltaTime, spriteBatch);

        }
        
        private void OnHeroDeath()
        {
            deathTimer.Start();
        }

        private void SaveGame()
        {
            CurrentScore = 1;
            CurrentStage = 10;

            SaveData saveData = new SaveData();
            saveData.currentScore = CurrentScore;
            saveData.stageNumber = CurrentStage;

            // Test
            saveData.weaponDataList = new List<WeaponSaveData>();
            saveData.weaponDataList.Add(new WeaponSaveData(10, 2));
            saveData.weaponDataList.Add(new WeaponSaveData(15, 20));
            saveData.weaponDataList.Add(new WeaponSaveData(10, 2));

            //saveData.weaponDataList = new List<WeaponSaveData>();
            //foreach(Weapon w in hero.WeaponHandler.WeaponsList)
            //{
            //    saveData.weaponDataList.Add(new WeaponSaveData(w.CurrentAmmoCarry, w.CurrentMagHold));
            //}

            Loader.ToXmlFile(saveData, "SaveFile.xml");
        }

    }
}
