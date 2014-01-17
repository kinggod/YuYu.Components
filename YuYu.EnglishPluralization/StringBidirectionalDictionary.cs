﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    internal class StringBidirectionalDictionary : BidirectionalDictionary<string, string>
    {
        internal StringBidirectionalDictionary()
        {
        }

        internal StringBidirectionalDictionary(Dictionary<string, string> firstToSecondDictionary) : base(firstToSecondDictionary)
        {
        }

        internal override bool ExistsInFirst(string value)
        {
            return base.ExistsInFirst(value.ToLowerInvariant());
        }

        internal override bool ExistsInSecond(string value)
        {
            return base.ExistsInSecond(value.ToLowerInvariant());
        }

        internal override string GetFirstValue(string value)
        {
            return base.GetFirstValue(value.ToLowerInvariant());
        }

        internal override string GetSecondValue(string value)
        {
            return base.GetSecondValue(value.ToLowerInvariant());
        }
    }
}
