using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNMC2013
{
    public static class Extensions
    {
        public static  String titelize(this string str)
        {
            string result;

            result = str[0].ToString().ToUpper();
            result += str.Substring(1).ToLower();
            return result;
        }
    }
}
