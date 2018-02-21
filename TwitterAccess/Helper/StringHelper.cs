using System.Text;

namespace TwitterAccess
{   
    public class StringHelper
    {
        private const string _AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// Clean a string to be used in a url
        /// </summary>        
        public static string UrlEncode(string str)
        {
            var result = new StringBuilder();
            foreach (char c in str)
            {
                if (_AllowedChars.Contains(c.ToString()))
                {
                    result.Append(c);
                }
                else
                {
                    result.Append('%' + string.Format("{0:X2}", (int)c));
                }
            }
            return result.ToString();
        }
    }
}