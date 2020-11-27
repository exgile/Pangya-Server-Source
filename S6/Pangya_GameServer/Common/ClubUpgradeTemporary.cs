using Pangya_GameServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangya_GameServer.Common
{
    public class TClubUpgradeTemporary
    {
        public WarehouseData PClub;
        public sbyte UpgradeType;
        public byte Count;
        public TClubUpgradeTemporary()
        {
            PClub = new WarehouseData();
        }
        // TClubUpgradeTemporary
        public void Clear()
        {
            PClub = null;
            UpgradeType = -1;
        }

    }
}
