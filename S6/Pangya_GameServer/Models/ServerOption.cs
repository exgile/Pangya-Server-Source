using Pangya_GameServer.Common;
using Pangya_GameServer.Flags;
namespace Pangya_GameServer.Models
{
    public class ServerOptions
    {
        ServerOptionsData m_data;

        public ServerOptions(ServerOptionFlag optionFlag, uint property)
        {
            SetOptions(optionFlag, property);
        }

        public ServerOptionsData OptionsData()
        {
            return m_data;
        }

        public void SetOptions(ServerOptionFlag optionFlag,uint property)
        {
            m_data.Set(optionFlag, property);
        }
    }
}
