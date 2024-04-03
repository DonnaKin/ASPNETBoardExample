using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Donna.Common
{
    public static class Check
    {
        public static bool IsGuid(string pValue)
        {
            bool bReturn = Guid.TryParse(pValue, out Guid guid);

            if (guid.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                bReturn = false;
            }

            return bReturn;
        }

        public static bool IsNone(string pValue)
        {
            if (string.IsNullOrEmpty(pValue) || pValue == "")
                return true;
            else
                return false;
        }

        public static int IsNone(string pValue, int pOutput)
        {
            if(Int32.TryParse(pValue, out Int32 iOutput))
            {
                return Int32.Parse(pValue);
            }
            else
            {
                return pOutput;
            }
        }
    }
}