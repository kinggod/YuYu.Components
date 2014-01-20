using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    internal class BidirectionalDictionary<TFirst, TSecond>
    {
        internal Dictionary<TFirst, TSecond> FirstToSecondDictionary { get; set; }
        internal Dictionary<TSecond, TFirst> SecondToFirstDictionary { get; set; }

        internal BidirectionalDictionary()
        {
            this.FirstToSecondDictionary = new Dictionary<TFirst, TSecond>();
            this.SecondToFirstDictionary = new Dictionary<TSecond, TFirst>();
        }

        internal BidirectionalDictionary(Dictionary<TFirst, TSecond> firstToSecondDictionary)
            : this()
        {
            foreach (var key in firstToSecondDictionary.Keys)
                this.AddValue(key, firstToSecondDictionary[key]);
        }

        internal virtual bool ExistsInFirst(TFirst value)
        {
            return this.FirstToSecondDictionary.ContainsKey(value);
        }

        internal virtual bool ExistsInSecond(TSecond value)
        {
            if (this.SecondToFirstDictionary.ContainsKey(value))
                return true;
            return false;
        }

        internal virtual TSecond GetSecondValue(TFirst value)
        {
            if (this.ExistsInFirst(value))
                return this.FirstToSecondDictionary[value];
            return default(TSecond);
        }

        internal virtual TFirst GetFirstValue(TSecond value)
        {
            if (this.ExistsInSecond(value))
                return this.SecondToFirstDictionary[value];
            return default(TFirst);
        }

        internal void AddValue(TFirst firstValue, TSecond secondValue)
        {
            this.FirstToSecondDictionary.Add(firstValue, secondValue);
            if (!this.SecondToFirstDictionary.ContainsKey(secondValue))
                this.SecondToFirstDictionary.Add(secondValue, firstValue);
        }
    }
}
