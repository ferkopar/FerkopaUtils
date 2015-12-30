using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace FerkopaUtils
{
    /// <summary>
    /// Olyan logikai típus, aminek van egy "Nem meghatározott" értéke is.
    /// </summary>
    public enum DefaultBool
    {
        /// <summary>
        /// Nem meghatározott
        /// </summary>
        None,

        /// <summary>
        /// Hamis
        /// </summary>
        False,

        /// <summary>
        /// Igaz
        /// </summary>
        True
    }

    public static class Utils
    {
        /// <summary>
        /// "en-US" cultureinfo-t ad vissza
        /// </summary>
        public static CultureInfo CultureInfoEnUs = new CultureInfo("en-US");

        public static string CalculateMd5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// A megadott dátumot tartalmazó stringet dátumra alakítja.
        /// </summary>
        public static DateTime DateStringToDateTime(string str)
        {
            str = str.Trim();
            if (str.Length == 10)
                return new DateTime(
                    int.Parse(str.Substring(0, 4)),
                    int.Parse(str.Substring(5, 2)),
                    int.Parse(str.Substring(8, 2)));
            if (str == string.Empty)
                return DateTime.MinValue;
            throw new Exception("Hibás időformátum.");
        }

        /// <summary>
        /// Szabványos formára alakítja a megadott dátumot.
        /// </summary>
        public static string FormatDate(this DateTime dt)
        {
            return (dt == DateTime.MinValue) ? "NULL" : string.Format("'{0}'", dt.ToString("yyyy-MM-dd"));
        }

        /// <summary>
        /// Szabványos formára alakítja a megadott dátumot.
        /// </summary>
        public static string FormatDate(string s)
        {
            if (!string.IsNullOrEmpty(s)) return "'" + s.Substring(0, 10).Replace('.', '-') + "'";
            return "NULL";
        }

        /// <summary>
        /// Szabványos formára alakítja a megadott időpontot.
        /// </summary>
        public static string FormatTime(DateTime dt)
        {
            return (dt == DateTime.MinValue) ? "NULL" : string.Format("'{0}'", dt.ToString("HH:mm"));
        }

        /// <summary>
        /// Szabványos formára alakítja a megadott időpontot.
        /// </summary>
        public static string FormatTimeLong(DateTime dt)
        {
            return "'" + dt.ToString("HH:mm:ss") + "'";
        }

        public static string GetCusomMd5(string userName, string passWord, string salt = "U37ET77M2KIMB90R8JL31G0YU2QI0Y")
        {
            return CalculateMd5Hash(passWord + salt.Substring(9, 13) + userName + salt.Substring(3, 10));
            //return CalculateMD5Hash(passWord+userName);
        }

        public static Encoding GetFileEncoding(string srcFile)
        {
            // *** Use Default of Encoding.Default (Ansi CodePage)

            Encoding enc = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default

            byte[] buffer = new byte[5];

            FileStream file = new FileStream(srcFile, FileMode.Open);

            file.Read(buffer, 0, 5);

            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)

                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)

                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)

                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)

                enc = Encoding.UTF7;

            return enc;
        }

        /// <summary>
        /// Visszaadja, hogy a megadott dátum melyik negyedévben van.
        /// </summary>
        /// <param name="dt">A dátum.</param>
        /// <returns>A negyedév sorszáma (1 .. 4)</returns>
        public static int GetQuarter(this DateTime dt)
        {
            int month = dt.Month;
            int quarter = 1;
            while ((month -= 3) > 0)
                quarter++;
            return quarter;
        }

        /// <summary>
        /// TickCount lekérdezés.
        /// </summary>
        /// <returns></returns>
        [DllImport("Kernel32.dll")]
        public static extern uint GetTickCount();

        public static bool Is64Bit()
        {
            bool retVal;

            IsWow64Process(Process.GetCurrentProcess().Handle, out retVal);

            return retVal;
        }

        /// <summary>
        /// A megadott évről eldönti hogy szökőév-e
        /// </summary>
        public static bool IsLeapYear(int year)
        {
            return (((year % 4 == 0) && (year % 100 != 0))
                     || (year % 400 == 0));
        }

        public static bool IsNumeric(string val, NumberStyles numberStyle)
        {
            Double result;
            return Double.TryParse(val, numberStyle,
                CultureInfo.CurrentCulture, out result);
        }

        public static bool IsNumeric
        (string val, NumberStyles numberStyle, string cultureName)
        {
            Double result;
            return Double.TryParse(val, numberStyle, new CultureInfo
                        (cultureName), out result);
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);
        #region ObjectTo konverterek

        /// <summary>
        /// A megadott objektumot megpróbája átalakítani bool-ra.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static bool ObjectToBool(object obj, bool defValue = false)
        {
            if (obj is bool)
                return (bool)obj;
            bool r;
            return bool.TryParse(ObjectToStr(obj), out r) ? r : defValue;
        }

        /// <summary>
        /// A megadott objektumot megpróbája átalakítani DateTime típusra.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static DateTime ObjectToDateTime(object obj, DateTime defValue)
        {
            if (obj is DateTime)
                return (DateTime)obj;
            return defValue;
        }

        /// <summary>
        /// A megadott objektumot megpróbája átalakítani decimal-ra.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static decimal ObjectToDecimal(object obj, decimal defValue = 0)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            if (obj is decimal)
                return (decimal)obj;
            if (obj is int)
                return Convert.ToDecimal((int)obj);
            if (obj is double)
                return Convert.ToDecimal((double)obj);
            if (obj is string)
            {
                decimal d;
                if (decimal.TryParse((string)obj, NumberStyles.Number, culture, out d))
                {
                    return d;
                }
                return decimal.TryParse((string)obj, NumberStyles.Number, CultureInfoEnUs, out d) ? d : defValue;
            }
            return defValue;
        }

        /// <summary>
        /// A megadott objektumot megpróbája átalakítani double-re.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static double ObjectToDouble(object obj, double defValue = 0)
        {
            if (obj is double)
                return (double)obj;
            if (obj is decimal)
                return Convert.ToDouble((decimal)obj);
            if (obj is int)
                return Convert.ToInt32((int)obj);
            return defValue;
        }

        /// <summary>
        /// A megadott objektumot megpróbája átalakítani int-re.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static int ObjectToInt(object obj, int defValue = 0)
        {
            if (obj is Int32) return (Int32)obj;
            if (obj is Int16) return (Int16)obj;
            if (obj is UInt16) return (UInt16)obj;
            if (obj is SByte) return (SByte)obj;
            if (obj is Byte) return (Byte)obj;
            var s = ObjectToStr(obj);
            if (s == null) return defValue;
            int i;
            return int.TryParse(s, out i) ? i : defValue;
        }

        /// <summary>
        /// A megadott objektumot megpróbája átalakítani long-ra.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static long ObjectToLong(object obj, long defValue = 0)
        {
            if (obj is Int64)
                return (Int64)obj;
            if (obj is Int32)
                return (Int32)obj;
            if (obj is UInt32)
                return (UInt32)obj;
            if (obj is Int16)
                return (Int16)obj;
            if (obj is UInt16)
                return (UInt16)obj;
            if (obj is SByte)
                return (SByte)obj;
            if (obj is Byte)
                return (Byte)obj;
            if (obj is Decimal)
                return (long)((Decimal)obj);
            var s = obj as string;
            if (s != null)
                try
                {
                    return long.Parse(s.Replace(".", String.Empty));
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {
                }
            return defValue;
        }

        /// <summary>
        /// A megadott objektumot megpróbája átalakítani string-re.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static string ObjectToStr(object obj, string defValue = "")
        {
            var s = obj as string; if (s != null) return s;
            if (obj is Int64)
                return Convert.ToString((Int64)obj);
            if (obj is UInt64)
                return Convert.ToString((UInt64)obj);
            if (obj is Int32)
                return Convert.ToString((Int32)obj);
            if (obj is UInt32)
                return Convert.ToString((UInt32)obj);
            if (obj is Int16)
                return Convert.ToString((Int16)obj);
            if (obj is UInt16)
                return Convert.ToString((UInt16)obj);
            if (obj is SByte)
                return Convert.ToString((SByte)obj);
            if (obj is Byte)
                return Convert.ToString((Byte)obj);
            if (obj is Boolean)
                return Convert.ToString((Boolean)obj);
            if (obj is decimal)
                return Convert.ToString((Decimal)obj, CultureInfo.CurrentCulture);
            if (obj is double)
                return Convert.ToString((Double)obj, CultureInfo.InvariantCulture);
            if (obj is DateTime)
                return Convert.ToString((DateTime)obj, CultureInfo.InvariantCulture);
            return defValue;
        }

        /// <summary>
        /// A megadott objektumot megpróbája átalakítani string-re.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static string ObjectToString(object obj, string defValue = "")
        {
            return ObjectToStr(obj, defValue);
        }
        /// <summary>
        /// A megadott objektumot megpróbája átalakítani uint-re.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static uint ObjectToUInt(object obj, uint defValue = 0)
        {
            if (obj is Int32)
                return (uint)((Int32)obj);
            if (obj is UInt32)
                return (UInt32)obj;
            if (obj is Int16)
                return (uint)((Int16)obj);
            if (obj is UInt16)
                return (UInt16)obj;
            if (obj is SByte)
                return (uint)((SByte)obj);
            if (obj is Byte)
                return (Byte)obj;
            var s = obj as string;
            if (s == null) return defValue;
            try
            {
                return uint.Parse(s);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            // ReSharper restore EmptyGeneralCatchClause
            {
            }

            return defValue;
        }
        /// <summary>
        /// A megadott objektumot megpróbája átalakítani ulong-ra.
        /// Ha nem sikerül, akkor a megadott alapértelmezett értéket használja.
        /// </summary>
        public static ulong ObjectToULong(object obj, ulong defValue = 0)
        {
            if (obj is Int64)
                return (ulong)((Int64)obj);
            if (obj is UInt64)
                return (UInt64)obj;
            if (obj is Int32)
                return (ulong)((Int32)obj);
            if (obj is UInt32)
                return (UInt32)obj;
            if (obj is Int16)
                return (ulong)((Int16)obj);
            if (obj is UInt16)
                return (UInt16)obj;
            if (obj is SByte)
                return (ulong)((SByte)obj);
            if (obj is Byte)
                return (Byte)obj;
            return defValue;
        }
        #endregion ObjectTo konverterek
        /// <summary>
        /// A megadott év és hónap alapján visszaadja a hónap napjainak számát.
        /// </summary>
        public static int MonthDays(int month, int year)
        {
            switch (month)
            {
                case 2:
                    if (IsLeapYear(year)) return (29);
                    return (28);

                case 4:
                case 6:
                case 9:
                case 11:
                    return (30);

                default:
                    return (31);
            }
        }

        /// <summary>
        /// A megadott dátumot és időt DateTime típusra alakítja.
        /// </summary>
        public static DateTime ObjectsToDateTime(object date, object time)
        {
            return DateTime.Parse(
                ObjectToDateTime(date, DateTime.MinValue).Date.ToShortDateString() +
                " " +
                ObjectToStr(time));
        }

        /// <summary>
        /// Vonalkód olvasásnál előforduló olvasási eltérések kiküszöbölésére lett kitalálva ez a funkció.
        /// </summary>
        public static string ToBarcode(string text)
        {
            string retVal = text;

            if (text == string.Empty)
                return text;

            if (
                 //(text.Length == 7 ||
                 //text.Length == 12) &&
                 text.Length <= 20 &&
                 text.Contains("ö") &&
                !text.Contains("0")
               )
            {
                text = text.Replace("ö", "0");
                text = text.Replace("-", "/");

                int asciiVal;
                string indexString;
                bool bBarcode = true;
                for (int index = 0, ubound = text.Length; index < ubound; index++)
                {
                    indexString = Convert.ToString(text[index]);

                    asciiVal = indexString[0];
                    if (asciiVal < 47 || asciiVal > 57)
                    {
                        bBarcode = false;
                        break;
                    }
                }

                if (bBarcode)
                    retVal = text;
            }
            return retVal;
        }

        #region PositionFormToDesktopSize

        ///// <summary>
        ///// A megadott formot a rendelkezésre álló asztal terület 80% százalékára méretezi minden oldalról.
        ///// </summary>
        ///// <param name="form">Az átméretezendő form.</param>
        //public static void PositionFormToDesktopSize(Form form)
        //{
        //    PositionFormToDesktopSize(form, 80);
        //}

        ///// <summary>
        ///// A megadott formot a rendelkezésre álló asztal terület megadott százalékára méretezi minden oldalról.
        ///// </summary>
        ///// <param name="form">Az átméretezendő form.</param>
        ///// <param name="percent">Az átméretezés mértéke.</param>
        //public static void PositionFormToDesktopSize( Form form, int percent )
        //{
        //    PositionFormToDesktopSize(form, percent, new Rectangle());
        //}

        ///// <summary>
        ///// A megadott formot a rendelkezésre álló asztal terület megadott százalékára méretezi minden oldalról.
        ///// </summary>
        ///// <param name="form">Az átméretezendő form.</param>
        ///// <param name="percent">A rendelkezésre álló területből elfoglalt terület százalékban.</param>
        //public static void PositionFormToDesktopSize(Form form, int percent, Rectangle margin)
        //{
        //    Rectangle workRect = System.Windows.Forms.SystemInformation.WorkingArea;
        //    if (workRect.Width <= 1024)
        //    {
        //        form.Size = new Size(workRect.Width, workRect.Height);
        //        form.Location = new Point(0, 0);
        //        form.StartPosition = FormStartPosition.Manual;
        //    }
        //    else
        //    {
        //        float fpercent = (float)((float)percent / (float)100);

        //        float newWidth = (float)workRect.Width * fpercent;
        //        float newHeight = (float)workRect.Height * fpercent;

        //        form.Size = new Size(System.Convert.ToInt32(newWidth), System.Convert.ToInt32(newHeight));
        //        form.StartPosition = FormStartPosition.CenterScreen;
        //    }
        //}

        #endregion PositionFormToDesktopSize

        #region RepositionFormToParent

        ///// <summary>
        ///// Átméretezi a formot a szülő formhoz igazítva, a megadott százalékban.
        ///// </summary>
        ///// <param name="form">Az átméretezendő form.</param>
        ///// <param name="size">A rendelkezésre álló terület.</param>
        ///// <param name="percent">A rendelkezésre álló területből elfoglalt terület százalékban.</param>
        //public static void RepositionFormToParent(Form form, Size clientSize)
        //{
        //    form.StartPosition = FormStartPosition.Manual;
        //    form.WindowState = FormWindowState.Normal;

        //    form.Size = new Size(
        //        (form.Size.Width > clientSize.Width) ?
        //        clientSize.Width : form.Size.Width,
        //        (form.Size.Height > clientSize.Height) ?
        //        clientSize.Height : form.Size.Height);

        //    //form.Location = new Point(
        //    //    ( form.Location.X <= 0 ) ? 1 :
        //    //        ( form.Location.X + form.Size.Width >= clientSize.Width ) ?
        //    //        clientSize.Width - form.Size.Width - 6 :
        //    //        form.Location.X,
        //    //    ( form.Location.Y <= 0 ) ? 1 :
        //    //        ( form.Location.Y + form.Size.Height >= clientSize.Height ) ?
        //    //        clientSize.Height - form.Size.Height :
        //    //        form.Location.Y );

        //    int x = (clientSize.Width - form.Size.Width) / 2;
        //    int y = (clientSize.Height - form.Size.Height) / 2;

        //    form.Location = new Point(x >= 0 ? x : 0, y >= 0 ? y : 0);
        //}

        #endregion RepositionFormToParent

        #region Double számformázó. -> pl: 1,2 Mrd

        internal static NumForm[] NumForms =
    {
            new NumForm(1000000000, "Mrd"),
            new NumForm(1000000, "m"),
            new NumForm(1000, "e")
        };

        /// <summary> Pénzügyi számformázó. </summary>
        /// <param name="value"> számérték </param>
        /// <param name="precision"> pontosság </param>
        /// <returns> Formázott számérték. </returns>
        public static string CurrencyStr(double value, int precision)
        {
            string strPrec = "." + new String('0', precision);
            foreach (NumForm numForm in NumForms)
            {
                if (value >= numForm.Divider)
                {
                    value /= numForm.Divider;
                    return String.Format("{0}{1}", value.ToString(strPrec), numForm.Suffix);
                }
            }
            double TOLERANCE = 0.00001;
            return Math.Abs(value) < TOLERANCE ? "0" : value.ToString(strPrec);
        }

        internal struct NumForm
        {
            public int Divider;
            public string Suffix;

            public NumForm(int divider, string suffix)
            {
                Divider = divider; Suffix = suffix;
            }
        }
        #endregion Double számformázó. -> pl: 1,2 Mrd

        #region BytesToString - Byte tömb átalakítása string formátumra

        /// <summary>
        /// Byte tömb átalakítása string formátumra
        /// </summary>
        /// <param name="bytes">A byte[] tömb.</param>
        /// <returns>A byte-ok hexa formátumban.</returns>
        public static string BytesToHexString(byte[] bytes)
        {
            string result = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = string.Format("{0:X}", bytes[i]);
                result += (hex.Length == 1) ? "0" + hex : hex;
            }
            return result;
        }

        #endregion BytesToString - Byte tömb átalakítása string formátumra

        #region PutBytesToBytesList

        /// <summary>
        /// A megadott byte-okat tartalmazó tömböt a byte listához fűzi.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bytesList"></param>
        public static void PutBytesToBytesList(byte[] bytes, List<byte> bytesList)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytesList.Add(bytes[i]);
            }
        }

        #endregion PutBytesToBytesList



        #region BytesToString

        /// <summary>
        /// A megadott byte tömböt string-gé alakítja.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToString(byte[] bytes)
        {
            return new string(Encoding.Default.GetChars(bytes));
        }

        #endregion BytesToString

        #region SQL formázás

        #region Sql_AddComma

        /// <summary>
        /// Ha a string már nem üres, akkor egy vesszőt ad hozzá.
        /// </summary>
        public static void SqlAddComma(ref string s)
        {
            if (s != string.Empty)
                s += "\r\n, ";
        }

        /// <summary>
        /// Ha a string már nem üres, akkor egy vesszőt ad hozzá.
        /// </summary>
        public static void SqlAddComma(StringBuilder s)
        {
            if (s.Length > 0)
                s.Append("\r\n, ");
        }

        #endregion Sql_AddComma

        #region Sql_NewLine

        /// <summary>
        /// Sortörést ad a stringhez.
        /// </summary>
        public static void SqlNewLine(ref string s)
        {
            s += "\r\n";
        }

        /// <summary>
        /// Sortörést ad a stringhez.
        /// </summary>
        public static void SqlNewLine(StringBuilder s)
        {
            s.Append("\r\n");
        }

        #endregion Sql_NewLine

        #region Sql_AddAnd

        /// <summary>
        /// Ha a string már nem üres, akkor az "AND" szöveget adja hozzá.
        /// </summary>
        public static void SqlAddAnd(ref string s)
        {
            if (s != string.Empty)
                s += " AND \r\n";
        }

        /// <summary>
        /// Ha a string már nem üres, akkor az "AND" szöveget adja hozzá.
        /// </summary>
        public static void SqlAddAnd(StringBuilder s)
        {
            if (s.Length > 0)
                s.Append(" AND \r\n");
        }

        #endregion Sql_AddAnd

        #region Sql_Comparison

        /// <summary>
        /// Az Sql_Comparison függvény által használt összehasonlító operátort jelöl meg.
        /// </summary>
        public enum SqlComparisonType
        {
            /// <summary>
            /// Egyenlő
            /// </summary>
            Equals,

            /// <summary>
            /// Nem egyenlő
            /// </summary>
            NotEquals,

            /// <summary>
            /// Kisebb vagy egyenlő
            /// </summary>
            LessOrEqual,

            /// <summary>
            /// Nagyobb vagy egyenlő
            /// </summary>
            GreaterOrEqual
        }

        ///// <summary>
        ///// SQL lekérdezésbe beágyazható szűrést hoz létre a megadott adatok alapján.
        ///// </summary>
        ///// <param name="where">Ide fog kerülni az eredmény.</param>
        ///// <param name="fieldName">A mező neve.</param>
        ///// <param name="field">A mező.</param>
        ///// <param name="operandus">A összehasonlítandó érték.</param>
        ///// <param name="op">Az operátor.</param>
        //public static void Sql_Comparison(
        //    ref string where,
        //    string fieldName,
        //    Field field,
        //    string operandus,
        //    Sql_ComparisonType op)
        //{
        //    Sql_AddAnd(ref where);
        //    where += fieldName;
        //    switch (op)
        //    {
        //        case Sql_ComparisonType.Equals:
        //            where += " = ";
        //            where += field.GetSqlValue(operandus);
        //            break;
        //        case Sql_ComparisonType.NotEquals:
        //            where += " <> ";
        //            where += field.GetSqlValue(operandus);
        //            break;
        //        case Sql_ComparisonType.GreaterOrEqual:
        //            where += " >= ";
        //            where += field.GetSqlValue(operandus);
        //            break;
        //        case Sql_ComparisonType.LessOrEqual:
        //            where += " <= ";
        //            where += field.GetSqlValue(operandus);
        //            break;
        //    }
        //}

        #endregion Sql_Comparison

        #endregion SQL formázás

        #region FormatMultiLineText

        /// <summary>
        /// A stringben található "|" és "***" jeleket sortöréssé konvertálja.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FormatMultiLineText(string s)
        {
            if (s == null)
                return null;

            s = s.Replace("|", "\r\n");
            s = s.Replace("***", "\r\n");
            return s;
        }

        #endregion FormatMultiLineText

        #region CreateFormat

        /// <summary>
        /// Numerikus formátum létrehozás.
        /// </summary>
        /// <param name="len">A decimális számjegyek száma.</param>
        /// <param name="dec">A tizedes számjegyek száma.</param>
        /// <returns>A formátum.</returns>
        public static string CreateFormat(int len, int dec)
        {
            StringBuilder sb = new StringBuilder(2 * len);

            while (len > 0)
            {
                if (len - 3 > 0)
                {
                    sb.Insert(0, ",###");
                    len -= 3;
                }
                else
                {
                    string s = string.Empty;
                    s = s.PadLeft(len, '#');
                    sb.Insert(0, s);
                    len = 0;
                }
            }

            sb[sb.Length - 1] = '0';

            if (dec > 0)
            {
                string s = string.Empty;
                s = s.PadLeft(dec, '0');

                sb.Append("." + s);
            }

            return sb.ToString();
        }

        #endregion CreateFormat

        #region CreateExcelNumericFormat

        /// <summary>
        /// Numerikus formátum létrehozás Excel exporthoz.
        /// </summary>
        /// <param name="dec">A tizedes számjegyek száma.</param>
        /// <returns>A formátum.</returns>
        public static string CreateExcelNumericFormat(int dec)
        {
            StringBuilder sb = new StringBuilder(6 + dec);
            sb.Append("# ##0");

            if (dec > 0)
            {
                string s = string.Empty;
                s = s.PadLeft(dec, '0');

                sb.Append("." + s);
            }

            return sb.ToString();
        }

        #endregion CreateExcelNumericFormat
       
    }
}