using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    class LevelManager
    {
        // Single creation
        private static LevelManager instance;
        public static LevelManager Instance { get { if (instance == null) { instance = new LevelManager(); return instance; } return instance; } set { instance = value; } }


        private Random random;
        public Stage CurrentStage { get; private set; }

        public LevelManager()
        {

        }

        public void LoadStage()
        {
            Stage s = new Stage();
            Loader.ReadXML("", ref s);
            CurrentStage = s;
        }

        public void Update()
        {

        }

        // Spawn next enemy type based on the probbaility of the current stage
        private void SpawnEnemy()
        {
            double ran = random.NextDouble();
            
        }

    }
}
