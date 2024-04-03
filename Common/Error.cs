using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Project.Donna.Common
{
    public static class Error
    {
        public static void WebHandler(Exception ex)
        {
            if(ex != null && !ex.GetType().ToString().Equals("System.Threading.ThreadAbortException"))
            {
                string strFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Error", DateTime.Now.ToString("yyyyMMdd"));
                string strFilePath = Path.Combine(strFolderPath, "Log.txt");

                if(!Directory.Exists(strFolderPath))
                {
                    Directory.CreateDirectory(strFolderPath);
                }

                using(StreamWriter sw = File.AppendText(strFilePath))
                {
                    sw.WriteLine($"[{DateTime.Now}] Error : {ex.ToString()} ");
                }

                Tool.Print("처리도중 에러가 발생했습니다");
                Util.Move("", "/Error");
                Tool.End();
            }
        }
    }
}