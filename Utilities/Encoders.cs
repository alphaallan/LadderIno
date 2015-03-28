using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Encoders
{
    /// <summary>
    /// Extention metods to encode, decode and test
    /// Developed by: Allan Leon A. Leitão
    /// </summary>
    public static class Encorders
    {
        /// <summary>
        /// Decode a UTF-8 character (16 bits) into a Boolean array
        /// </summary>
        /// <param name="input">Character to be decoded</param>
        /// <returns>Decoded boolean array</returns>
        public static bool[] ToBoolArray(this char input)
        {
            bool[] buff = new bool[16]; //Buffer

            //for (int c = 0; c < 16; c++)
            //{
            //    buff[c] = ((input & (int) Math.Pow(2, c)) != 0);
            //}

            buff[0] = ((input & 1) != 0);      // 2^0
            buff[1] = ((input & 2) != 0);      // 2^1
            buff[2] = ((input & 4) != 0);      // 2^2
            buff[3] = ((input & 8) != 0);      // 2^3
            buff[4] = ((input & 16) != 0);     // 2^4
            buff[5] = ((input & 32) != 0);     // 2^5
            buff[6] = ((input & 64) != 0);     // 2^6
            buff[7] = ((input & 128) != 0);    // 2^7
            buff[8] = ((input & 256) != 0);    // 2^8
            buff[9] = ((input & 512) != 0);    // 2^9
            buff[10] = ((input & 1024) != 0);  // 2^10
            buff[11] = ((input & 2048) != 0);  // 2^11
            buff[12] = ((input & 4096) != 0);  // 2^12
            buff[13] = ((input & 8192) != 0);  // 2^13
            buff[14] = ((input & 16384) != 0); // 2^14
            buff[15] = ((input & 32768) != 0); // 2^15

            return buff;
        }

        /// <summary>
        /// Codify a 16 bits Boolean array in to a UTF-8 character 
        /// </summary>
        /// <param name="input">16 positions boolean array</param>
        /// <returns>Codified character</returns>
        public static char ToChar(this bool[] input)
        {
            char buff = (char)0;

            if (input != null && input.Length == 16)
            {
                //for (int c = 0; c < 16; c++)
                //{
                //    if(input[c]) buff += (char) Math.Pow(2, c);
                //}

                if (input[0]) buff += (char)1;      // 2^0
                if (input[1]) buff += (char)2;      // 2^1
                if (input[2]) buff += (char)4;      // 2^2
                if (input[3]) buff += (char)8;      // 2^3
                if (input[4]) buff += (char)16;     // 2^4
                if (input[5]) buff += (char)32;     // 2^5
                if (input[6]) buff += (char)64;     // 2^6
                if (input[7]) buff += (char)128;    // 2^7
                if (input[8]) buff += (char)256;    // 2^8
                if (input[9]) buff += (char)512;    // 2^9
                if (input[10]) buff += (char)1024;  // 2^10
                if (input[11]) buff += (char)2048;  // 2^11
                if (input[12]) buff += (char)4096;  // 2^12
                if (input[13]) buff += (char)8192;  // 2^13
                if (input[14]) buff += (char)16384; // 2^14
                if (input[15]) buff += (char)32768; // 2^15
            }

            return buff;
        }

        public static bool ToBool(this string input)
        {
            bool buff;
            if (!string.IsNullOrEmpty(input)) Boolean.TryParse(input.Trim(), out buff);
            else buff = false;
            return buff;
        }

        public static Int16 ToShort(this string input)
        {
            short buff;
            if (!string.IsNullOrEmpty(input)) Int16.TryParse(input.Trim(), out buff);
            else buff = 0;
            return buff;
        }

        public static Int32 ToInt(this string input)
        {
            int buff;
            if (!string.IsNullOrEmpty(input)) Int32.TryParse(input.Trim(), out buff);
            else buff = 0;
            return buff;
        }

        public static decimal ToDecimal(this string input)
        {
            decimal buff;
            if (!string.IsNullOrEmpty(input)) Decimal.TryParse(input.Trim(), out buff);
            else buff = 0;
            return buff;
        }

        public static double ToDouble(this string input)
        {
            double buff;
            if(!string.IsNullOrEmpty(input)) Double.TryParse(input.Trim(), out buff);
            else buff = 0;
            return buff;
        }

        public static T ToEnum<T>(this string input) where T : struct, IConvertible
        {
            T buff;
            try
            {
                buff = (T)Enum.Parse(typeof(T), input);
            }
            catch
            {
                buff = (T)Activator.CreateInstance(typeof(T));
            }
            return buff;
        }
    }
}
