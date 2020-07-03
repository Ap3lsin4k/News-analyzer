﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PupilIsNotStudent.model.repository
{

    class TextParsingRepository
    {

        //==========TEXT PARSER==========
        // only Cyrillic letters. Change separators to only one " "
        private StringBuilder getPureText(string notClearedText)
        {
            StringBuilder peeledText = new StringBuilder(notClearedText.Length);


            foreach (var symbol in notClearedText)
            {
                peeledText.Append(
                    isCyrillic(symbol)
                        ? char.ToLower(symbol)
                        : ' ');
            }
            return peeledText;
        }

        // Takes not cleared(aka raw) text
        public string[] getSplitWords(string notClearedText)
        {
            return getPureText(notClearedText).ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static readonly HashSet<int> s_RussianCodes = new HashSet<int>(
            new int[] { 1028, 1108, 1030, 1110, 1031, 1111, 1168, 1169, 32, 39 }
                .Concat(Enumerable
                    .Range(1040, 1103 - 1040 + 1)
                    .Except(new int[] { 1066, 1067, 1098 }))
                .Concat(Enumerable.Range(65, 90 - 65 + 1))
                .Concat(Enumerable.Range(97, 122 - 97 + 1))
        );

        public static bool isRussianCodes(char symbol) => s_RussianCodes.Contains(symbol);


        private bool isCyrillic(int letterCode)
        {
            switch (letterCode)
            {
                case 1028: // Є
                case 1108: // є
                case 1030: // І
                case 1110: // і
                case 1031: // Ї
                case 1111: // ї
                case 1168: // Ґ
                case 1169: // ґ
                case 32:  // " "
                case 39:  // '
                          //case 45:  // -
                    return true;
                default:
                    return
                        1040 <= letterCode && letterCode <= 1103 &&  // Cyrillic
                        letterCode != 1066 &&  // Ъ
                        letterCode != 1067 &&  // Ы
                        letterCode != 1098  // ъ
                        ||
                        65 <= letterCode && letterCode <= 90
                        ||
                        97 <= letterCode && letterCode <= 122
                        ;
            }

        }//*/
    }
}
