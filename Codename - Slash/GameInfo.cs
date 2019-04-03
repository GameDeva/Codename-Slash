using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class GameInfo
    {
        // Single creation
        private static GameInfo instance;
        public static GameInfo Instance { get { if (instance == null) { instance = new GameInfo(); return instance; } return instance; } set { instance = value; } }
        
        // public List<TileInfo> tileInfoList = new List<TileInfo>();


    }
}
