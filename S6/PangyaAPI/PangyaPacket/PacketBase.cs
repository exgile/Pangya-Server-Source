using PangyaAPI.PangyaClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PangyaAPI.PangyaPacket
{
    public class HandleBase<T> where T : PacketResult, new()
    {
        public HandleBase(Packet packet)
        {
            PacketResult = new T();
            PacketResult.Load(packet);
        }
        public T PacketResult { get; set; }
    }
}
