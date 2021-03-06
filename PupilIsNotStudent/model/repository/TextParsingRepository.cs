﻿    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    namespace PupilIsNotStudent.model.repository
    {
        public class TextParsingRepository
        {

            //==========TEXT PARSER==========
            // only Cyrillic letters. Change separators to only one " "
            protected StringBuilder GetPureText(string notClearedText)
            {
                StringBuilder peeledText = new StringBuilder(notClearedText.Length);


                foreach (var symbol in notClearedText)
                {
                    peeledText.Append(
                        IsCyrillic(symbol)
                            ? char.ToLower(symbol)
                            : ' ');
                }
                return peeledText;
            }

            // Takes not cleared(aka raw) text
            public string[] GetSplitWords(string notClearedText)
            {
                return GetPureText(notClearedText).ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }


            public bool IsCyrillic(int letterCode)
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
                    case '’': // ’
                              //case 45:  // - dash
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

            }
        }
    }
