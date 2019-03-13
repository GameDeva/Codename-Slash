using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Codename___Slash
{
    public interface IDamageable
    {
        void TakeDamage(int damagePoints);
        void TakeDamage(int damagePoints, Vector2 direction);


    }
}
