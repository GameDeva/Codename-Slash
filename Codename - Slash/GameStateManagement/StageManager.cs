using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Codename___Slash
{
    public class StageManager
    {
        // Singleton creation
        private static StageManager instance;
        public static StageManager Instance { get { if (instance == null) { instance = new StageManager(); return instance; } return instance; } set { instance = value; } }

        private MapGen mapGen;
        public StageData CurrentStageData { get; private set; }

        // Trigger regions
        ReachBoxTrigger LeaveWalkwayTrigger;
        ReachBoxTrigger BeginBattleTrigger;
        ReachBoxTrigger LeaveBattleArenaTrigger;

        // 
        public Action<ICollidable> OnAddCollider;
        public Action<ICollidable> OnRemoveCollider;
        public Action<StageData> OnNewStage;
        public Action<StageData> OnCreateEnemyPools;
        public Action OnResetHeroPosition;
        public Action OnCompleteStage;
        public Action<bool> ToggleEnemyGeneration;

        public void Initialise(IServiceProvider serviceProvider, Hero hero)
        {
            // Reference to singletons
            mapGen = MapGen.Instance;

            mapGen.Initialise(serviceProvider);
            mapGen.GetMapData("Walkway");
            mapGen.GetMapData("BattleArena");

            // Create trigger regions
            LeaveWalkwayTrigger = new ReachBoxTrigger(new Rectangle(Game1.SCREENWIDTH, Game1.SCREENHEIGHT / 3, 10, Game1.SCREENHEIGHT / 3));
            BeginBattleTrigger = new ReachBoxTrigger(new Rectangle(Game1.SCREENWIDTH / 2, Game1.SCREENHEIGHT / 2, Game1.SCREENWIDTH / 2, Game1.SCREENHEIGHT / 2));
            LeaveBattleArenaTrigger = new ReachBoxTrigger(new Rectangle(Game1.SCREENWIDTH, Game1.SCREENHEIGHT / 3, 10, Game1.SCREENHEIGHT / 3));

            LeaveWalkwayTrigger.Active = false;
            LeaveBattleArenaTrigger.Active = false;
            BeginBattleTrigger.Active = false;


            // Subscribe events
            LeaveWalkwayTrigger.Triggered += OnReachBattleArena;
            BeginBattleTrigger.Triggered += BeginBattle;
            LeaveBattleArenaTrigger.Triggered += OnStageBegin;
            EnemyDirector.Instance.OnBeatAllEnemies += OnStageEnd;

        }
        

        public void UnloadContent()
        {
            mapGen.UnloadAllMapContent();
        }


        private void OnReachBattleArena()
        {
            mapGen.LoadMapTextures("BattleArena");
            mapGen.AssignMapToDraw("BattleArena");
            OnResetHeroPosition?.Invoke();
            BeginBattleTrigger.Active = true;
            OnRemoveCollider?.Invoke(LeaveWalkwayTrigger);
            OnAddCollider?.Invoke(BeginBattleTrigger);

            mapGen.ChangeMapColliders(MapCollider.BattleArena);
            
        }
        
        // 
        private void BeginBattle()
        {
            OnRemoveCollider?.Invoke(BeginBattleTrigger);
            ToggleEnemyGeneration?.Invoke(true);



        }

        // Beginning of each stage
        public void OnStageBegin()
        {
            mapGen.LoadMapTextures("Walkway");
            mapGen.AssignMapToDraw("Walkway");
            mapGen.ChangeMapColliders(MapCollider.Walkway);
            OnAddCollider?.Invoke(LeaveWalkwayTrigger);

            //
            // TODO: Do on separate thread,  put below after the thread has finished
            LeaveWalkwayTrigger.Active = true;
            // Create all object pools + load appropriate assets
            UpdateStageData(CurrentStageData.stageNumer + 1); // Load next stage

            // Load the enemy content
            OnNewStage?.Invoke(CurrentStageData);
            OnCreateEnemyPools?.Invoke(CurrentStageData);
            

        }
        
        // When stage complete i.e. all enemies are dead
        public void OnStageEnd()
        {
            ToggleEnemyGeneration?.Invoke(true);
            mapGen.ChangeMapColliders(MapCollider.BattleArenaExitOpen);
            LeaveBattleArenaTrigger.Active = true;
            OnCompleteStage?.Invoke();
            OnAddCollider?.Invoke(LeaveBattleArenaTrigger);


        }

        public void NewSession(int stageNumber)
        {
            UpdateStageData(stageNumber);
        }

        public void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            mapGen.DrawMap(spriteBatch);
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
