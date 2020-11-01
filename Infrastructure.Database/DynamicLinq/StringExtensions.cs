﻿#if NETSTANDARD1_3
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Infrastructure.Database.DynamicLinq
{
    /// <summary>
    ///
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string s)
        {
            return new string(s.CharsToTitleCase().ToArray());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static IEnumerable<char> CharsToTitleCase(this string s)
        {
            bool newWord = true;
            foreach (char c in s)
            {
                if (newWord) { yield return Char.ToUpper(c); newWord = false; }
                else yield return Char.ToLower(c);
                if (c == ' ') newWord = true;
            }
        }
    }
}
#endif