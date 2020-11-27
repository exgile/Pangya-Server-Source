using PangyaAPI.SqlConnector.DataBase.Procedures;
using PangyaAPI.SqlConnector.DataBase;
using System;
using System.Linq;
using System.Windows.Forms;
namespace PangyaAPI.SqlConnector
{
    public class PlayerConnectorEx
    {
        static readonly PangyaEntities DB;
        static PlayerConnectorEx()
        {
            try
            {
                DB = new PangyaEntities();
            }
            catch
            {
                MessageBox.Show("failed To Connect DB", "Pang.SqlConnector");
            }
        }

        public static USP_LOGIN_SERVER_US_Result USP_LOGIN_SERVER_US(string user, string pwd, string iPAddress, string auth1, string auth2)
        {
            try
            {
                var result = DB.USP_LOGIN_SERVER_US(user, pwd, iPAddress, auth1, auth2).FirstOrDefault();

                if (result == null)
                {
                    throw new Exception("failed to log in");
                }
                else
                {
                    return result;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
