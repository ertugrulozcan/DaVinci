using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ertis.Infrastructure.Helpers
{
    public static class CharTool
    {
        /// <summary>
        /// Converts characters to lowercase.
        /// </summary>
        const string _lookupStringL = "---------------------------------!-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~------------------------------------------------------------------------c--------------o-----u----------c--------------o-----u---------------------------------gg----------------ii--------------------------------------------ss-------";

        /// <summary>
        /// Converts characters to uppercase.
        /// </summary>
        const string _lookupStringU = "---------------------------------!-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~------------------------------------------------------------------------Ç--------------Ö-----Ü----------Ç--------------Ö-----Ü---------------------------------ĞĞ----------------İI--------------------------------------------Şş-------";

        /// <summary>
        /// Get lowercase version of this ASCII character.
        /// </summary>
        public static char ToLowerFast(char c)
        {
            return _lookupStringL[c];
        }

        /// <summary>
        /// Get uppercase version of this ASCII character.
        /// </summary>
        public static char ToUpperFast(char c)
        {
            return _lookupStringU[c];
        }

        /// <summary>
        /// Translate uppercase characters to lowercase characters.
        /// </summary>
        public static char ToLowerFastIf(char c)
        {
            if (c >= 'A' && c <= 'Z')
            {
                return (char)(c + 32);
            }
            else
            {
                return c;
            }
        }

        /// <summary>
        /// Translate lowercase ASCII characters to uppercase.
        /// </summary>
        public static char ToUpperFastIf(char c)
        {
            if (c >= 'a' && c <= 'z')
            {
                return (char)(c - 32);
            }
            else
            {
                return c;
            }
        }

        public static string ToLowerString(string str)
        {
            string result = string.Empty;
            foreach (char c in str)
            {
                result += CharTool.ToLowerFast(c);
            }

            return result;
        }

        public static string ToUpperString(string str)
        {
            string result = string.Empty;
            foreach (char c in str)
            {
                result += CharTool.ToUpperFast(c);
            }

            return result;
        }
    }
}
