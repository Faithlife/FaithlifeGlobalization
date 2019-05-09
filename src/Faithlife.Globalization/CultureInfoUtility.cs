using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Faithlife.Utility;

namespace Faithlife.Globalization
{
	/// <summary>
	/// Helper methods for working with <see cref="CultureInfo"/>.
	/// </summary>
	public static class CultureInfoUtility
	{
		/// <summary>
		/// Creates a <see cref="CultureInfo"/> that represents the specific culture that is associated with the specified name, or a fallback
		/// culture if that specific culture is not available.
		/// </summary>
		/// <param name="language">The language for which the <see cref="CultureInfo"/> should be created.</param>
		/// <returns>A <see cref="CultureInfo"/> that represents: The invariant culture, if <paramref name="language"/> is the neutral language; or
		/// The best-matching specific culture associated with <paramref name="language"/>, if <paramref name="language"/> is a neutral culture; or
		/// The best-matching culture specified by <paramref name="language"/>, if <paramref name="language"/> is already a specific culture.</returns>
		/// <remarks>This method should be preferred over CultureInfo.CreateSpecificCulture because it avoids
		/// throwing an <see cref="ArgumentException"/> for cultures that aren't implemented by the .NET Framework; instead, it
		/// falls back to the best-matching culture.</remarks>
		public static CultureInfo CreateSpecificCulture(LanguageName language)
		{
			// check for cached culture
			CultureInfo cultureInfo;
			lock (s_lock)
				s_dictCachedCultures.TryGetValue(language, out cultureInfo);

			if (cultureInfo == null)
			{
				// check for known unsupported languages
				switch (language.Language)
				{
				case "arc": // Aramaic
					// Aramaic should use Hebrew when formatting
					language = new LanguageName("he");
					break;

				case "cop": // Coptic
				case "eo": // Esperanto
				case "gez": // Ge'ez/Ethiopic
				case "la": // Latin
				case "pap": // Papiamento
				case "pis": // Pijin
				case "tpi": // Tok Pisin
				case "tsw": // Tsishingini
				case "x-pc": // Phonetic alphabet
				case "x-sab": // South Arabian
				case "x-tl": // Transliterated text
					// avoid exception when we know the language isn't supported
					break;

				default:
					try
					{
#if !NETSTANDARD1_4
						// This can fail in unexpected ways; for example 'zh' will throw an exception for
						//  geopolitical reasons. See also CultureInfo.GetCultureInfo*, consider UseUserOverride, etc.
						cultureInfo = CultureInfo.CreateSpecificCulture(language.FullName);
#else
						cultureInfo = new CultureInfo(language.FullName);
#endif
					}
					catch (ArgumentException)
					{
					}

					// cache culture if found
					if (cultureInfo != null)
						lock (s_lock)
							s_dictCachedCultures[language] = cultureInfo;

					break;
				}
			}

			// find best matching culture from all cultures
			if (cultureInfo == null)
			{
#if !NETSTANDARD1_4
				CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
#else
				CultureInfo[] cultures = new[] { CultureInfo.CurrentCulture, CultureInfo.InvariantCulture };
#endif

				LanguageNameMatchComparer comparer = new LanguageNameMatchComparer(language);
				cultureInfo = cultures
					.Select(ci => (CultureInfo: ci, Language: LanguageName.TryCreate(ci.Name)))
					.Where(item => item.Language.HasValue)
					.OrderByDescending(x => x, ComparisonUtility.CreateComparer<(CultureInfo, LanguageName?)>((x, y) => comparer.Compare(x.Item2.Value, y.Item2.Value)))
					.ThenBy(item => item.CultureInfo.Name, StringComparer.OrdinalIgnoreCase)
					.First().CultureInfo;

				// cache the found culture
				lock (s_lock)
					s_dictCachedCultures[language] = cultureInfo;
			}

			return cultureInfo;
		}

		// CultureInfo objects are cached to improve the performance of CreateSpecificCulture.
		static readonly object s_lock = new object();
		static readonly Dictionary<LanguageName, CultureInfo> s_dictCachedCultures = new Dictionary<LanguageName, CultureInfo>();
	}
}
