using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YuYu.Components
{
    /// <summary>
    /// Pluralization services to be used by the EF runtime implement this interface.
    /// By default the <see cref="EnglishPluralizationService" /> is used, but the pluralization service to use
    /// can be set in a class derived from <see cref="DbConfiguration" />.
    /// </summary>
    public interface IPluralizationService
    {
        /// <summary>
        /// Pluralize a word using the service.
        /// </summary>
        /// <param name="word">The word to pluralize.</param>
        /// <returns>The pluralized word </returns>
        string Pluralize(string word);

        /// <summary>
        /// Singularize a word using the service.
        /// </summary>
        /// <param name="word">The word to singularize.</param>
        /// <returns>The singularized word.</returns>
        string Singularize(string word);
    }
}
