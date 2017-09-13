using System.Collections.Generic;

namespace Faithlife.Globalization
{
	/// <summary>
	/// Compares two <see cref="LanguageName"/> objects according to how well they each match a base language.
	/// </summary>
	public sealed class LanguageNameMatchComparer : IComparer<LanguageName>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LanguageNameMatchComparer"/> class.
		/// </summary>
		/// <param name="language">The language against which other <see cref="LanguageName"/> objects will be matched.</param>
		public LanguageNameMatchComparer(LanguageName language)
		{
			m_language = language;
		}

		/// <summary>
		/// Compares the specified languages according to how well they match the language with which
		/// this <see cref="LanguageNameMatchComparer"/> was initialized.
		/// </summary>
		/// <param name="x">The first language to compare.</param>
		/// <param name="y">The second language to compare.</param>
		/// <returns>A <see cref="int"/> that returns the relative order of the two languages.</returns>
		public int Compare(LanguageName x, LanguageName y)
		{
			LanguageNameMatch matchLeft = LanguageName.Match(m_language, x);
			LanguageNameMatch matchRight = LanguageName.Match(m_language, y);
			int nCompare = matchLeft - matchRight;

			// TODO: use fallback cultures (if the fallback cultures aren't the same language as we are), e.g.
			//  Culture.Current, CultureInfo.CurrentCulture, CultureInfo.CurrentUICulture,
			//  CultureInfo.InstalledUICulture, English (same region), English (U.S.). Perhaps some
			//  cultures have natural fallback languages that could be hard-coded here as well. (case 2453)
			if (nCompare == 0 && matchLeft == LanguageNameMatch.DifferentLanguages)
				return LanguageName.Match(s_languageEnglishUS, x) - LanguageName.Match(s_languageEnglishUS, y);

			return nCompare;
		}

		static readonly LanguageName s_languageEnglishUS = new LanguageName("en-US");

		readonly LanguageName m_language;
	}
}
