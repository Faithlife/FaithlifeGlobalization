using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Faithlife.Globalization.Numerals;
using Faithlife.Utility;
using Faithlife.Utility.Invariant;

namespace Faithlife.Globalization
{
	/// <summary>
	/// Provides methods for creating and manipulating sort titles.
	/// </summary>
	public static class SortTitleUtility
	{
		/// <summary>
		/// Creates a sort title for the given title.
		/// </summary>
		/// <param name="strTitle">The title.</param>
		/// <param name="language">The language of the title.</param>
		/// <returns></returns>
		public static string CreateSortTitle(string strTitle, LanguageName language)
		{
			// check arguments
			if (strTitle == null)
				throw new ArgumentNullException("strTitle");

			// trim the title
			string strSortTitle = strTitle.Trim();

			// strip leading punctuation
			strSortTitle = s_reLeadingPunctuation.Replace(strSortTitle, "");

			// normalize punctuation and whitespace
			strSortTitle = strSortTitle
				.Replace('‘', '\'')
				.Replace('’', '\'')
				.Replace('“', '"')
				.Replace('”', '"');
			strSortTitle = s_reDashes.Replace(strSortTitle, "-");
			strSortTitle = s_reWhitespace.Replace(strSortTitle, " ");

			// strip any leading article (if we know how to find them for this language)
			Regex reLeadingArticle = GetLeadingArticleRegex(language);
			if (reLeadingArticle != null)
				strSortTitle = reLeadingArticle.Replace(strSortTitle, "");

			// strip any punctuation that is now at the front (e.g., following the article that was stripped)
			strSortTitle = s_reLeadingPunctuation.Replace(strSortTitle, "");

			// pad any sequences of digits out to four characters
			strSortTitle = s_reDigitSequence.Replace(strSortTitle, m => m.Groups["digits"].Value.PadLeft(4, '0'));

			// encode Roman numerals
			strSortTitle = s_reRomanNumerals.Replace(strSortTitle, m =>
			{
				int nValue = RomanNumerals.ToInteger(m.Groups["roman"].Value);
				Debug.Assert(nValue != 0, "nValue != 0");
				string strFollow = m.Groups["follow"].Value;
				bool bTrailingLetter = strFollow != null && strFollow.Length == 1 && strFollow[0] >= 'a' && strFollow[0] <= 'd';
				return (nValue == 0 ? m.Value :
					nValue == 1 ? (bTrailingLetter ? "I0001" : "I") :
					nValue < 5 ? "I{0:d4}".FormatInvariant(nValue) :
					nValue == 5 ? (bTrailingLetter ? "V0005" : "V") :
					nValue < 10 ? "V{0:d4}".FormatInvariant(nValue) :
					nValue == 10 ? (bTrailingLetter ? "X0010" : "X") :
					"X{0:d4}".FormatInvariant(nValue)) + strFollow;
			});

			strSortTitle = s_reChineseNumerals.Replace(strSortTitle, m => ChineseNumerals.ToInteger(m.Value)?.ToInvariantString().PadLeft(4, '0') ?? m.Value);

			// replace "first", "middle", "last" with character sequences that sort in that order
			strSortTitle = strSortTitle.Replace("上", "上1").Replace("中", "上2").Replace("下", "上3");

			// return the sort title
			return strSortTitle;
		}

		/// <summary>
		/// Creates a <see cref="SortableTitle"/> for <paramref name="strTitle"/> using the sorting rules of <paramref name="language"/>.
		/// </summary>
		/// <param name="strTitle">The title.</param>
		/// <param name="language">The language of the title.</param>
		/// <returns>A new <see cref="SortableTitle"/> containing a sortable title for <paramref name="strTitle"/>.</returns>
		public static SortableTitle CreateSortableTitle(string strTitle, LanguageName language)
		{
			return new SortableTitle(strTitle, CreateSortTitle(strTitle, language));
		}

		/// <summary>
		/// Gets a regular expression that matches a leading article.
		/// </summary>
		/// <param name="language">The language.</param>
		/// <returns>A regular expression that matches a leading article, or null if the specified language is not supported.</returns>
		public static Regex GetLeadingArticleRegex(LanguageName language)
		{
			return s_lazyLanguageArticles.Value.GetValueOrDefault(language.Language);
		}

		// Creates a dictionary that maps from languages to a regex that matches leading articles.
		private static Dictionary<string, Regex> CreateArticleRegexes()
		{
			const RegexOptions options = RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase;
			return new Dictionary<string, Regex>
			{
				{ "af", new Regex(@"^(die|'n)\s+", options) }, // Afrikaans
				{ "da", new Regex(@"^(de[nt]?|e[nt])\s+", options) }, // Swedish
				{ "de", new Regex(@"^(das|de[mnrs]|die|ein(e[mnrs]?)?)\s+", options) }, // German
				{ "en", new Regex(@"^(an?|the)\s+", options) }, // English
				{ "eo", new Regex(@"^la\s+", options) }, // Esperanto
				{ "es", new Regex(@"^(el|las?|los?|una?)\s+", options) }, // Spanish
				{ "fr", new Regex(@"^((la|les?|une?)\s+|l')", options) }, // French
				{ "it", new Regex(@"^((gli|il?|l[aeo]|un[ao]?)\s+|gl'|l'|un')", options) }, // Italian
				{ "nl", new Regex(@"^(de|eene?|het|'[nt])\s+", options) }, // Dutch
				// ASSUME: that all Norwegian dialects share the same articles
				{ "nb", new Regex(@"^(de[int]?|ei?[nt]?)\s+", options) }, // Norwegian Bokmål
				{ "nn", new Regex(@"^(de[int]?|ei?[nt]?)\s+", options) }, // Norwegian Nynorsk
				{ "no", new Regex(@"^(de[int]?|ei?[nt]?)\s+", options) }, // Norwegian
				{ "pt", new Regex(@"^(as?|os?|uma?)\s+", options) }, // Portuguese
				{ "sv", new Regex(@"^(de[nt]?|en|ett)\s+", options) }, // Swedish
			};
		}

		readonly static Lazy<Dictionary<string, Regex>> s_lazyLanguageArticles = new Lazy<Dictionary<string, Regex>>(CreateArticleRegexes);

		// match leading punctuation and whitespace
		readonly static Regex s_reLeadingPunctuation = new Regex(@"^[\p{P}\p{S}\p{Z}]+");

		// match consecutive whitespace
		readonly static Regex s_reWhitespace = new Regex(@"\s+");

		// match (a sequence of) dashes
		readonly static Regex s_reDashes = new Regex(@"\p{Pd}+");

		// match sequences of 1-3 digits, but treat leading zeroes as individual numbers
		readonly static Regex s_reDigitSequence = new Regex(@"(?<=([^0-9]0*|^))(?'digits'([1-9][0-9]{0,2}(?!\d))|0)", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

		// match uppercase Roman numerals (that are not immediately followed by a period)
		readonly static Regex s_reRomanNumerals = new Regex(@"\b(?=[XVI])(?'roman'(XL|X{0,4})(VI{0,4}|IX|IV|I{0,4}))(?'follow'[a-d])?(?=[^\w\.]|$)", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

		// match Chinese numerals
		readonly static Regex s_reChineseNumerals = new Regex(@"[一二三四五六七八九十百]+");
	}
}
