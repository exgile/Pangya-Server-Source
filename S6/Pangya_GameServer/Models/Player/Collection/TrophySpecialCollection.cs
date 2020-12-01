using Pangya_GameServer.Models.Player.Data;
using PangyaAPI.BinaryModels;
using System.Collections.Generic;
namespace Pangya_GameServer.Models.Player.Collection
{
    public class TrophySpecialCollection : List<TrophySpecialData>
    {
        /// <summary>
        /// Cria as informações
        /// </summary>
        /// <param name="Code"> 0 == Todas as sessoes, Sessão 5 </param>
        /// <returns></returns>
        public byte[] Build(byte Code = 5)
        {
            var result = new PangyaBinaryWriter();
            result.Write(new byte[] { 0xB4, 0x00, Code });
            result.Write((ushort)Count);
            foreach (var data in this)
            {
                result.Write(data.GetInfo());
            }
            return result.GetBytes();
        }

        public byte[] GetInfo()
        {
            var result = new PangyaBinaryWriter();
            result.Write((ushort)Count);
            foreach (var data in this)
            {
                result.Write(data.GetInfo());
            }
            return result.GetBytes();
        }
    }
}
