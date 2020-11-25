using PangyaAPI.BinaryModels;
namespace PangyaAPI.PangyaPacket
{
    public abstract class PacketResult : object, IPacketTransformable
    {
        public abstract void Load(Packet packet);
    }
}