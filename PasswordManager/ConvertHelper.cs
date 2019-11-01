using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    class ConvertHelper
    {
        public static string getStringFromBytes(byte[] input)
        {
            StringBuilder builder = new StringBuilder();
            foreach(byte b in input)
            {
                builder.Append(b.ToString());
            }
            return builder.ToString();
        }
    }
}
