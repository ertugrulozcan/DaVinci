using System;
namespace Ertis.WebService.Helpers
{
    public static class MySqlHelper
    {
        public static bool BooleanConverter(string str)
        {
            if (str == "True" || str == "true" || str == "TRUE")
                return true;

            if (str == "False" || str == "false" || str == "FALSE")
                return false;

            byte tinyint = 1;
            Byte.TryParse(str, out tinyint);
            return tinyint != 0;
        }
    }
}
