using HeatMeterPrePaySelfHelp.Froms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeatMeterPrePaySelfHelp
{
    static class Program
    {
        // 登陆状态 
        public static int loginSign = -1;
        // 登陆员工号 
        public static string staffId = "";
        // 注册code 
        public static string code = "";

        // MessageBox非阻塞开关
        public static bool MsgBoxNonBlocking = false;
        // MessageBox阻塞提示暂存
        public static string MsgBoxMessage = "";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
            if (Program.loginSign > 0)
            {   
                Application.Run(new WelcomeForm());
            }
        }
    }
}
