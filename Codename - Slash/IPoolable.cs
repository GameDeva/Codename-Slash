using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    public interface IPoolable
    {
        void OnPoolInstantiation();
        void OnSpawnFromPool(IArgs args);
    }
}
