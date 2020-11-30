using Pangya_GameServer.Models.Player;
using System;

namespace Pangya_GameServer.GPlayer
{
   public partial class Session
    {
        #region Methods

        public void SetGameID(ushort ID)
        {
            GameID = ID;
        }

        public bool InventoryLoad()
        {
            if (UserInfo.GetUID != 0)
            {
                Inventory = new Inventory(UserInfo.GetUID);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal byte[] GetGameInfomations(int v)
        {
            throw new NotImplementedException();
        }

        internal byte[] GetLobbyInfo()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
