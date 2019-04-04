using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class AwardsData
    {
        public List<int> scores;

        public AwardsData()
        {
            scores= new List<int>();
        }

        public AwardsData(List<int> scores)
        {
            this.scores = scores;
        }
    }
}
