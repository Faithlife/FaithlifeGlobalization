using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Faithlife.Utility;

namespace Faithlife.Globalization
{
	/// <summary>
	/// Represents a culture.
	/// </summary>
	public sealed class Culture
	{
		/// <summary>
		/// Gets the invariant culture.
		/// </summary>
		/// <value>The invariant culture.</value>
		public static Culture Invariant
		{
			get
			{
				return s_invariantCulture ?? (s_invariantCulture = new Culture(LanguageName.Neutral));
			}
		}

		/// <summary>
		/// Gets the U.S. English culture.
		/// </summary>
		/// <value>The U.S. English culture.</value>
		public static Culture EnglishUS
		{
			get
			{
				return s_englishUSCulture ?? (s_englishUSCulture = new Culture(new LanguageName("en-US")));
			}
		}

		/// <summary>
		/// Creates a culture for the specified language.
		/// </summary>
		/// <param name="lang">The language.</param>
		/// <returns>A culture for the specified language.</returns>
		public static Culture Create(LanguageName lang)
		{
			return new Culture(lang);
		}

		/// <summary>
		/// Creates a culture with the specified CultureInfo.
		/// </summary>
		/// <param name="cultureInfo">The CultureInfo.</param>
		/// <returns>A culture with the specified CultureInfo.</returns>
		public static Culture Create(CultureInfo cultureInfo)
		{
			if (cultureInfo == null)
				throw new ArgumentNullException("cultureInfo");

			return new Culture(new LanguageName(cultureInfo.Name), cultureInfo, cultureInfo);
		}

		/// <summary>
		/// Creates a culture with the specified properties.
		/// </summary>
		/// <param name="language">The language.</param>
		/// <param name="formatCultureInfo">The format CultureInfo.</param>
		/// <param name="resourceCultureInfo">The resource CultureInfo.</param>
		/// <returns>A culture with the specified properties.</returns>
		public static Culture Create(LanguageName language, CultureInfo formatCultureInfo, CultureInfo resourceCultureInfo)
		{
			if (formatCultureInfo == null)
				throw new ArgumentNullException("formatCultureInfo");
			if (resourceCultureInfo == null)
				throw new ArgumentNullException("resourceCultureInfo");

			return new Culture(language, formatCultureInfo, resourceCultureInfo);
		}

		/// <summary>
		/// Gets the language of the culture.
		/// </summary>
		/// <value>The language of the culture.</value>
		public LanguageName LanguageName
		{
			get { return m_language; }
		}

		/// <summary>
		/// Gets the <see cref="CultureInfo" /> that should be used for formatting, text sorting, etc.
		/// </summary>
		/// <value>The <see cref="CultureInfo" /> for this culture.</value>
		public CultureInfo FormatCultureInfo
		{
			get
			{
				if (m_formatCultureInfo == null)
					CreateSpecificCulture();
				return m_formatCultureInfo;
			}
		}

		/// <summary>
		/// Gets the <see cref="CultureInfo" /> that should be used when selecting localized resources.
		/// </summary>
		/// <value>The <see cref="CultureInfo" /> for this culture.</value>
		public CultureInfo ResourceCultureInfo
		{
			get
			{
				if (m_resourceCultureInfo == null)
					CreateSpecificCulture();
				return m_resourceCultureInfo;
			}
		}

		/// <summary>
		/// Gets the best match comparison.
		/// </summary>
		/// <value>The best match comparison.</value>
		public Comparison<LanguageName> BestMatchComparison
		{
			get { return new LanguageNameMatchComparer(LanguageName).Compare; }
		}

		/// <summary>
		/// Gets the string comparer.
		/// </summary>
		/// <value>The string comparer.</value>
		public StringComparer StringComparer
		{
			get
			{
				return m_stringComparer ??
					(m_stringComparer = CreateStringComparer(CaseSensitivity.MatchCase));
			}
		}

		/// <summary>
		/// Gets the string comparer that ignores case.
		/// </summary>
		/// <value>The string comparer that ignores case.</value>
		public StringComparer StringComparerIgnoreCase
		{
			get
			{
				return m_stringComparerIgnoreCase ??
					(m_stringComparerIgnoreCase = CreateStringComparer(CaseSensitivity.IgnoreCase));
			}
		}

		/// <summary>
		/// Finds the best match for this culture among a collection of language names.
		/// </summary>
		/// <param name="cultures">The language names to match.</param>
		/// <returns>The best language name in the collection.</returns>
		public LanguageName FindBestMatch(IEnumerable<LanguageName> cultures)
		{
			return cultures.Max((cultureA, cultureB) => BestMatchComparison(cultureA, cultureB));
		}

		/// <summary>
		/// Finds the best match for this culture among a collection of cultures.
		/// </summary>
		/// <param name="cultures">The cultures to match.</param>
		/// <returns>The best culture in the collection.</returns>
		public Culture FindBestMatch(IEnumerable<Culture> cultures)
		{
			Comparison<LanguageName> fnComparison = BestMatchComparison;
			return cultures.Max(
				(cultureA, cultureB) => fnComparison(cultureA.LanguageName, cultureB.LanguageName));
		}

		/// <summary>
		/// Finds the best match for this culture among a collection of cultures.
		/// </summary>
		/// <param name="cultures">The cultures to match.</param>
		/// <param name="converter">The converter.</param>
		/// <returns>The best culture in the collection.</returns>
		public T FindBestMatch<T>(IEnumerable<T> cultures, Func<T, LanguageName> converter)
		{
			Comparison<LanguageName> fnComparison = BestMatchComparison;
			return cultures.Max(
				(cultureA, cultureB) => fnComparison(converter(cultureA), converter(cultureB)));
		}

		/// <summary>
		/// Finds the best matches for this culture among a collection of cultures.
		/// </summary>
		/// <param name="cultures">The cultures to match.</param>
		/// <param name="converter">The converter.</param>
		/// <returns>The best cultures in the collection.</returns>
		public IEnumerable<T> FindBestMatches<T>(IEnumerable<T> cultures, Func<T, LanguageName> converter)
		{
			ICollection<T> collection = cultures as ICollection<T> ?? cultures.ToList();
			T match = FindBestMatch(collection, converter);
			LanguageName lang = converter(match);
			return collection.Where(c => lang == converter(c));
		}

		/// <summary>
		/// Uses FormatCultureInfo to format a string.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>The formatted string.</returns>
		public string Format(string format, params object[] args)
		{
			return string.Format(FormatCultureInfo, format, args);
		}

		private Culture(LanguageName language)
		{
			m_language = language;
		}

		private Culture(LanguageName language, CultureInfo formatCultureInfo, CultureInfo resourceCultureInfo)
			: this(language)
		{
			m_formatCultureInfo = formatCultureInfo;
			m_resourceCultureInfo = resourceCultureInfo;
		}

		private void CreateSpecificCulture()
		{
			CultureInfo cultureInfo = CultureInfoUtility.CreateSpecificCulture(LanguageName);

			if (m_formatCultureInfo == null)
				m_formatCultureInfo = cultureInfo;
			if (m_resourceCultureInfo == null)
				m_resourceCultureInfo = cultureInfo;
		}

		private StringComparer CreateStringComparer(CaseSensitivity caseSensitivity)
		{
			bool ignoreCase = caseSensitivity == CaseSensitivity.IgnoreCase;
			return StringUtility.CreateComparer(FormatCultureInfo, ignoreCase);
		}

		static Culture s_invariantCulture;
		static Culture s_englishUSCulture;

		readonly LanguageName m_language;
		CultureInfo m_formatCultureInfo;
		CultureInfo m_resourceCultureInfo;
		StringComparer m_stringComparer;
		StringComparer m_stringComparerIgnoreCase;
	}
}
