
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace FerkopaUtils
{
    public static class StringExtender
    {
        /// <summary>
        /// A megadott string első karakterét nagybetűssé, a többi karakter kisbetűssé alakítja.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string StringToCapital(this string s)
        {
            if (String.IsNullOrEmpty(s)) return String.Empty;
            var sb = new StringBuilder(s.ToLower());
            sb[0] = Char.ToUpper(sb[0]);
            return sb.ToString();
        }

        public static bool IsBeginWith(this String s, String b)
        {
            if (s.Length >= b.Length &&
                s.Substring(0, b.Length) == b) return true;
            return false;
        }

        public static String DropLastCharIfMatch(this String s, char c)
        {
            return s[s.Length - 1] == c ? s.DropLastChar() : s;
        }

        public static String DropLastChar(this String s)
        {
            return s.Substring(0, s.Length - 1);
        }

        public static String DropFirstChar(this String s)
        {
            return s.Substring(1, s.Length - 1);
        }

        /// <summary>
        /// String átalakítása hexa formátumra, csak 8 bites vagy alacsonyabb stringek esetén működik.
        /// </summary>
        /// <param name="s">Az átalakítandó string.</param>
        /// <returns>A hexa string</returns>
        public static string ToHex(this string s)
        {
            return s.Select(c => (byte) c).Select(b => $"{b:X}").Aggregate(String.Empty, (current, hex) => current + ((hex.Length == 1) ? "0" + hex : hex));
        }


        public static string NormalizeWhiteSpace(this string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            var ss = s.Trim();
            var iswhite = false;
            var sLength = ss.Length;
            var sb = new StringBuilder(sLength);
            foreach (var c in ss.ToCharArray())
            {
                if (Char.IsWhiteSpace(c))
                {
                    if (iswhite)
                    {
                        //Continuing whitespace ignore it.
                    }
                    else
                    {
                        //New WhiteSpace

                        //Replace whitespace with a single space.
                        sb.Append(" ");
                        //Set iswhite to True and any following whitespace will be ignored
                        iswhite = true;
                    }
                }
                else
                {
                    sb.Append(c.ToString(Thread.CurrentThread.CurrentCulture));
                    //reset iswhitespace to false
                    iswhite = false;
                }
            }
            return sb.ToString();
        }

        public static string RemoveHtmlTags(this string htmlText)
        {
            var reg = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
            return reg.Replace(htmlText, "");
        }

        public static bool IsFilePath(this string text)
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Path.GetFullPath(text);
                return true;
            }
            catch (Exception)
            {
               return false;
            }
        }

        public static byte[] ToBytes(this string str)
        {
            var x = Encoding.UTF8.GetBytes(str);
            return x;
        }

        public static string GetString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);           
        }

        public static T As<T>(this string value)
        {
            object v = value;

            // If a string, just return it
            if (typeof(T) == typeof(string))
            {
                return (T)v;
            }
            // Use a TypeConverter if one is available
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(typeof(string)))
            {
                try
                {
                    return (T)converter.ConvertFromString(value);
                }
                catch (Exception ex)
                {
                    string failureMessage
                        = string.Format(
                            CultureInfo.CurrentCulture,
                            "Failed to convert \"{0}\" to {1}: {2}",
                            value,
                            typeof(T).Name,
                            ex.Message);
                    throw new InvalidOperationException(failureMessage);
                }
            }

            // Look for a constructor that takes a string
            var constructor
                = typeof(T).GetConstructor(new[] { typeof(string) });
            if (constructor != null)
            {
                return (T)constructor.Invoke(new object[] { value });
            }

            var message
                = string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot convert {0} into {1}",
                    value,
                    typeof(T).Name);
            throw new InvalidOperationException(message);
        }

        /// <summary>
        /// A megadott stringet aposztrófok közé teszi.
        /// A string belsejében lévő aposztrófokat megduplázza.
        /// </summary>
        public static string ToSqlQuoted(this string str)
        {
            return string.IsNullOrEmpty(str) ? "\'\'" : "'" + str.Replace("'", "''") + "'";
        }

        /// <summary>
        /// A megadott karaktert aposztrófok közé teszi.
        /// </summary>
        public static string ToSqlQuoted(this char chr)
        {
            return (chr == '\'') ? "''''" : "'" + chr + "'";
        }

        /// <summary>
        /// A megadott stringet aposztrófok közé teszi.
        /// A string belsejében lévő aposztrófokat megduplázza.
        /// Kérésre nem teszi aposztrófot a string külső részére.
        /// </summary>
        public static string ToSqlQuoted(this string str, bool withoutApostrophe)
        {
            if (withoutApostrophe) return str.Replace("'", "''");
            return ToSqlQuoted(str);
        }

        public static string Extract(this string str)
        {
            return str.Trim().Substring(str.IndexOf("\""), str.LastIndexOf("\""));
        }
    }
}
