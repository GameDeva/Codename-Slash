using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class Stage
    {
        public int stageNumer;

        // 
        public int enemiesToFight;

        // Number of enemies of each type that can be present at 1 time 
        public int maxDogeCount;
        public int maxBaldCount;
        public int maxSkullCount;
        public int maxDarkCount;

        // Probability of spawning each type of enemy, must add up to 1
        public float probDogeSpwan;
        public float probBaldSpwan;
        public float probSkullSpwan;
        public float probDarkSpwan;

        // Probability of pickup drop, on enemy death
        public float probHealthDrop;
        public float probAmmoDrop;

    }
}
