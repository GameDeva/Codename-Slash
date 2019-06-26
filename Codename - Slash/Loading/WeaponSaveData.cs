using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codename___Slash
{
    public class WeaponSaveData
    {
        public int currentAmmoCarry;
        public int currentMagHold;

        public WeaponSaveData(int currentAmmoCarry, int currentMagHold)
        {
            this.currentAmmoCarry = currentAmmoCarry;
            this.currentMagHold = currentMagHold;
        }

        public WeaponSaveData() { }

    }
}
