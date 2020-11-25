using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PangyaAPI.IFF.Tools
{
    static class Utils
    {
        public static void MessageBoxError(string Message)
        {
            MessageBox.Show(Message, "Pang.IFF.Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
