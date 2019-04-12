using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class GameManager
    {
        // Singleton creation
        private static GameManager instance;
        public static GameManager Instance { get { if (instance == null) { instance = new GameManager(); return instance; } return instance; } set { instance = value; } }

        // Contains saved data if any
        public SaveData CurrentSaveData { get; private set; }

        // Contains AwardsData if any
        public AwardsData AwardsData { get; private set; }

        // Score of current play session
        public int CurrentScore;
        // Stage of current play session
        public int CurrentStage;
        // Hero instance for current gameplay session
        public Hero Hero { get; private set; }


        #region Game Session saving/loading

        // Load saved file, returns true if successful
        public bool GetSaveData()
        {
            if (File.Exists("SaveFile.xml"))
                return true;
            return false;
        }

        // Set up new save file and begin game
        public void OnNewGame()
        {
            // Overwrite existing save file 
            if (File.Exists("SaveFile.xml"))
            {
                File.Delete("SaveFile.xml");
            }
            
            // Create new hero instance
            Hero = new Hero();
            // Set initial values
            CurrentScore = 0;
            CurrentStage = 1;
            
            // 
            CurrentSaveData = new SaveData();
        }

        // Load save file and begin game
        public void OnContinueGame()
        {
            try {
                // Load save data from xml file
                SaveData s = new SaveData();
                Loader.ReadXML("SaveFile.xml", ref s);
                CurrentSaveData = s;
                
                // Create new hero instance
                Hero = new Hero();

                // Set up session with save data values
                CurrentScore = CurrentSaveData.currentScore;
                CurrentStage = CurrentSaveData.stageNumber;
                for (int i = 0; i < Hero.WeaponHandler.WeaponsList.Count; i++)
                {
                    Hero.WeaponHandler.WeaponsList[i].CurrentAmmoCarry = CurrentSaveData.weaponDataList[i].currentAmmoCarry;
                    Hero.WeaponHandler.WeaponsList[i].CurrentMagHold = CurrentSaveData.weaponDataList[i].currentMagHold;
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found exception: {0}", e.Message);
            }
            
        }

        // Update CurrentSaveData and saves to file
        public void SaveGame()
        {
            // Add stage and score data
            CurrentSaveData.currentScore = CurrentScore;
            CurrentSaveData.stageNumber = CurrentStage;

            // Add weapon data
            CurrentSaveData.weaponDataList = new List<WeaponSaveData>();
            foreach (Weapon w in Hero.WeaponHandler.WeaponsList)
            {
                CurrentSaveData.weaponDataList.Add(new WeaponSaveData(w.CurrentAmmoCarry, w.CurrentMagHold));
            }

            // Update the save file
            UpdateSaveFile();
        }
        
        // Adds data from CurrentSaveData obj to SaveFile.xml
        private void UpdateSaveFile()
        {
            Loader.ToXmlFile(CurrentSaveData, "SaveFile.xml");
        }

        #endregion

        #region Awards Data saving/loading

        public void UpdateAwardsFileWithNewScore(int newScore)
        {
            AwardsData.scores.Add(newScore);
            AwardsData.scores.Sort();

            Loader.ToXmlFile(AwardsData, "AwardsFile.xml");
        }

        // Loads awards file into awardsData variable
        public void LoadAwardsFile()
        {
            if (File.Exists("AwardsFile.xml"))
            {
                AwardsData a = new AwardsData();
                Loader.ReadXML("AwardsFile.xml", ref a);
                a.scores.Sort();
                AwardsData = a;
                return;
            }
            AwardsData = new AwardsData();
        }

        #endregion
    }
}
