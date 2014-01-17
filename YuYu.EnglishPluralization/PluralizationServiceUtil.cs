using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace YuYu.Components
{
    internal class PluralizationServiceUtil
    {
        internal static bool DoesWordContainSuffix(string word, IEnumerable<string> suffixes, CultureInfo culture)
        {
            return suffixes.Any((string s) => word.EndsWith(s, true, culture));
        }

        internal static bool TryGetMatchedSuffixForWord(string word, IEnumerable<string> suffixes, CultureInfo culture, out string matchedSuffix)
        {
            matchedSuffix = null;
            if (PluralizationServiceUtil.DoesWordContainSuffix(word, suffixes, culture))
            {
                matchedSuffix = suffixes.First((string s) => word.EndsWith(s, true, culture));
                return true;
            }
            return false;
        }

        internal static bool TryInflectOnSuffixInWord(string word, IEnumerable<string> suffixes, Func<string, string> operationOnWord, CultureInfo culture, out string newWord)
        {
            newWord = null;
            string text;
            if (PluralizationServiceUtil.TryGetMatchedSuffixForWord(word, suffixes, culture, out text))
            {
                newWord = operationOnWord(word);
                return true;
            }
            return false;
        }
    }
}
