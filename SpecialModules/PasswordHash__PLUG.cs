using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plotnikov_PR_21_102_DocumentManager.SpecialModules
{
    /// <summary>
    /// Затычка, в будущем заменить на хэширование пароля, сейчас ничего не преобразует
    /// </summary>
    internal class PasswordHash__PLUG
    {
        public static string getHash(string password)
        {
            return password;
        }

        public static bool isPasswordHash(string password, string hash)
        {
            return password.Equals(hash);
        }
    }
}
