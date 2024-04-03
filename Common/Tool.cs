using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Donna.Common
{
    public static class Tool
    {
        public static void Print(string pText)
        {
            HttpContext.Current.Response.Write(pText);
        }

        public static void End()
        {
            HttpContext.Current.Response.End();
        }

        public static void End(string pMsg)
        {
            Tool.Print(pMsg);
            Tool.End();
        }

        public static void Redirect(string pURL)
        {
            HttpContext.Current.Response.Redirect(pURL);
        }
    }
}