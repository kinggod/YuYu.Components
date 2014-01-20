using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{

    /// <summary>
    /// Represents a custom pluralization term to be used by the <see cref="EnglishPluralizationService" />
    /// </summary>
    public class CustomPluralizationEntry
    {
        /// <summary>
        /// Get the singular.
        /// </summary>
        public string Singular { get; private set; }

        /// <summary>
        /// Get the plural.
        /// </summary>
        public string Plural { get; private set; }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="singular">A non null or empty string representing the singular.</param>
        /// <param name="plural">A non null or empty string representing the plural.</param>
        public CustomPluralizationEntry(string singular, string plural)
        {
            if (string.IsNullOrEmpty(singular))
                throw new ArgumentNullException("singular");
            if (string.IsNullOrEmpty(plural))
                throw new ArgumentNullException("plural");
            this.Singular = singular;
            this.Plural = plural;
        }
    }
}
