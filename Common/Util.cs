using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Project.Donna.Common
{
    public static class Util
    {
        public static void Back(string pMsg) { 
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");
            if (!String.IsNullOrEmpty(pMsg))
            {
                sb.Append("alert(\"" + pMsg + "\");");
            }
            sb.Append("history.back();");
            sb.Append("</script>");

            Tool.End(sb.ToString());
        }

        public static void Move(string pMsg, string pURL)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");
            if (!String.IsNullOrEmpty(pMsg))
            {
                sb.Append("alert(\"" + pMsg + "\");");
            }
            sb.Append("location.href=\"" + pURL + "\";");
            sb.Append("</script>");

            Tool.End(sb.ToString());
        }

        public static void Script(string pScript)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");
            sb.Append(pScript);
            sb.Append("</script>");

            Tool.End(sb.ToString());
        }
    }
}