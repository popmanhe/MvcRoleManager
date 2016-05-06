using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MvcRoleManager.Security
{
  public  class Util
    {
        public static string  EncryptedAction(object action)
        {
            string serializedAction =  JsonConvert.SerializeObject(action);
         
            byte[] inputBytes = Encoding.UTF8.GetBytes(serializedAction);

            byte[] hash = MD5.Create().ComputeHash(inputBytes);

            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < hash.Length; i++)
            {
                sBuilder.Append(hash[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
