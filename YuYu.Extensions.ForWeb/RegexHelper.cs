using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace YuYu.Components
{
    internal class RegexHelper
    {
        internal static readonly Regex Tabs = new Regex(@"\t", RegexOptions.Compiled | RegexOptions.Multiline);
        internal static readonly Regex CarriageReturns = new Regex(@"(\r+\n*)|(\r*\n+)", RegexOptions.Compiled | RegexOptions.Multiline);
        internal static readonly Regex Spaces = new Regex(@"\s{2,}", RegexOptions.Compiled | RegexOptions.Multiline);
        internal static readonly Regex SpacesBetweenTags = new Regex(@">\s+<", RegexOptions.Compiled | RegexOptions.Multiline);
        internal static readonly Regex SpacesInTagsa = new Regex(@"</\s+", RegexOptions.Compiled | RegexOptions.Multiline);
        internal static readonly Regex SpacesInTagsb = new Regex(@"\s+/>", RegexOptions.Compiled | RegexOptions.Multiline);
        internal static readonly Regex SpacesInTagsc = new Regex(@"<\s+", RegexOptions.Compiled | RegexOptions.Multiline);
        internal static readonly Regex SpacesInTagsd = new Regex(@"\s+>", RegexOptions.Compiled | RegexOptions.Multiline);
    }
}
