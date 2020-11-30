using Pangya_GameServer.Common;
using Pangya_GameServer.Flags;
namespace Pangya_GameServer.Models
{
    public class ServerOptions
    {
        ServerOptionsData m_data;
        public ServerOptions(uint property)
        {
            m_data.Set(ServerOptionFlag.MAINTENANCE_FLAG_PAPELSHOP, property);
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
