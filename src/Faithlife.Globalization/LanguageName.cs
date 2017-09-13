using System;
using System.Text.RegularExpressions;
using Faithlife.Utility;

namespace Faithlife.Globalization
{
	/// <summary>
	/// Encapsulates a language name; specifically, a language and region tag.
	/// </summary>
	/// <remarks>The script tag is supported, but is merged with the language tag, so that az-Cyrl-AZ is
	/// split into az-Cyrl and AZ.</remarks>
	public struct LanguageName : IEquatable<LanguageName>, IComparable<LanguageName>, IComparable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LanguageName"/> class.
		/// </summary>
		/// <param name="strName">The language tag (as defined by RFC 4646).</param>
		/// <remarks>The null string is treated as the neutral language.</remarks>
		public LanguageName(string strName)
		{
			if (!TryParse(strName ?? string.Empty, out m_strLanguage, out m_strRegion))
				throw new ArgumentException("The language tag '{0}' is not supported.".FormatInvariant(strName), nameof(strName));
		}

		/// <summary>
		/// The neutral language name (empty language/region).
		/// </summary>
		public static readonly LanguageName Neutral = new LanguageName("");

		/// <summary>
		/// Gets the "primary language" tag.
		/// </summary>
		/// <value>The "primary language" tag.</value>
		public string Language
		{
			get { return m_strLanguage ?? string.Empty; }
		}

		/// <summary>
		/// Gets the region tag.
		/// </summary>
		/// <value>The region tag.</value>
		public string Region
		{
			get { return m_strRegion ?? string.Empty; }
		}

		/// <summary>
		/// Gets the full language tag (as defined by RFC 4646).
		/// </summary>
		/// <value>The full language tag (as defined by RFC 4646).</value>
		public string FullName
		{
			get
			{
				string strLanguage = Language;
				string strRegion = Region;

				if (strRegion.Length == 0 || strLanguage.Length == 0)
					return strLanguage;
				else
					return strLanguage + "-" + strRegion;
			}
		}

		/// <summary>
		/// Tries to create a <see cref="LanguageName"/> from the specified language tag.
		/// </summary>
		/// <param name="strName">The language tag (as defined by RFC 4646).</param>
		/// <returns>The language if <paramref name="strName"/> could be successfully parsed as a RFC 4646 language tag.</returns>
		/// <remarks>TryCreate fails when strName is null.</remarks>
		public static LanguageName? TryCreate(string strName)
		{
			LanguageName language;
			return TryCreate(strName, out language) ? (LanguageName?) language : null;
		}

		/// <summary>
		/// Tries to create a <see cref="LanguageName"/> from the specified language tag.
		/// </summary>
		/// <param name="strName">The language tag (as defined by RFC 4646).</param>
		/// <param name="language">The <see cref="LanguageName"/> initialized from <paramref name="strName"/>.</param>
		/// <returns><c>true</c> if <paramref name="strName"/> could be successfully parsed as a RFC 4646 language tag.</returns>
		/// <remarks>TryCreate fails when strName is null.</remarks>
		public static bool TryCreate(string strName, out LanguageName language)
		{
			string strLanguage;
			string strRegion;
			if (TryParse(strName, out strLanguage, out strRegion))
			{
				language = new LanguageName(strLanguage, strRegion);
				return true;
			}
			else
			{
				language = LanguageName.Neutral;
				return false;
			}
		}

		/// <summary>
		/// Determines whether the specified string is a valid language name.
		/// </summary>
		/// <param name="strName">Language name to check.</param>
		/// <returns><c>true</c> if <paramref name="strName"/> is a valid language name; otherwise, <c>false</c>.</returns>
		[Obsolete("Use TryCreate.")]
		public static bool IsValidName(string strName)
		{
			// check if the name matches the pattern for RFC 4646 names (or is null/empty, which signifies the "neutral" language)
			return string.IsNullOrEmpty(strName) || (s_reLanguageTag.IsMatch(strName) && strName.Length <= MaximumNameLength);
		}

		/// <summary>
		/// Matches two language names.
		/// </summary>
		/// <param name="strFirst">The first language name.</param>
		/// <param name="strSecond">The second language name.</param>
		/// <returns>A <see cref="LanguageNameMatch" /> that describes the comparison
		/// between the two language names.</returns>
		/// <remarks>When the looked-for language name is used first and the available
		/// language name is used second, the default ordering of <see cref="LanguageNameMatch" />
		/// indicates the attractiveness of the match.</remarks>
		public static LanguageNameMatch Match(string strFirst, string strSecond)
		{
			return Match(new LanguageName(strFirst), new LanguageName(strSecond));
		}

		/// <summary>
		/// Matches two language names.
		/// </summary>
		/// <param name="first">The first language name.</param>
		/// <param name="second">The second language name.</param>
		/// <returns>A <see cref="LanguageNameMatch" /> that describes the comparison
		/// between the two language names.</returns>
		/// <remarks>When the looked-for language name is used first and the available
		/// language name is used second, the default ordering of <see cref="LanguageNameMatch" />
		/// indicates the attractiveness of the match.</remarks>
		public static LanguageNameMatch Match(LanguageName first, LanguageName second)
		{
			string strLanguageA = first.Language;
			string strRegionA = first.Region;
			string strLanguageB = second.Language;
			string strRegionB = second.Region;

			if (strLanguageA.Length == 0)
				return strLanguageB.Length == 0 ? LanguageNameMatch.Identical : LanguageNameMatch.FirstEmpty;
			if (strLanguageB.Length == 0)
				return LanguageNameMatch.SecondEmpty;

			if (strLanguageA != strLanguageB)
				return LanguageNameMatch.DifferentLanguages;

			if (strRegionA.Length == 0)
				return strRegionB.Length == 0 ? LanguageNameMatch.Identical : LanguageNameMatch.FirstNeutral;
			if (strRegionB.Length == 0)
				return LanguageNameMatch.SecondNeutral;

			if (strRegionA != strRegionB)
				return LanguageNameMatch.DifferentRegions;
			else
				return LanguageNameMatch.Identical;
		}

		/// <summary>
		/// Returns the language tag as defined by RFC 4646.
		/// </summary>
		/// <returns>The language tag as defined by RFC 4646.</returns>
		public override string ToString()
		{
			return FullName;
		}

		/// <summary>
		/// Indicates whether this instance is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><c>true</c> if this instance is equal to <paramref name="other" />; otherwise, <c>false</c>.</returns>
		public bool Equals(LanguageName other)
		{
			return Language == other.Language && Region == other.Region;
		}

		/// <summary>
		/// Indicates whether this instance is equal to another object.
		/// </summary>
		/// <param name="obj">An object to compare with this object.</param>
		/// <returns><c>true</c> if <paramref name="obj"/> and this instance
		/// are the same type and represent the same value; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return obj is LanguageName && Equals((LanguageName) obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return HashCodeUtility.CombineHashCodes(Language.GetHashCode(), Region.GetHashCode());
		}

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>An integer that indicates the relative order of the objects being compared.</returns>
		public int CompareTo(LanguageName other)
		{
			int nCompare = string.CompareOrdinal(Language, other.Language);
			if (nCompare != 0)
				return nCompare;

			return string.CompareOrdinal(Region, other.Region);
		}

		/// <summary>
		/// Compares the current instance with another object of the same type.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns>An integer that indicates the relative order of the objects being compared.</returns>
		/// <exception cref="ArgumentException">obj is not the same type as this instance. </exception>
		int IComparable.CompareTo(object obj)
		{
			return ComparableImpl.CompareToObject(this, obj);
		}

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> the instances are equal; otherwise, <c>false</c>.</returns>
		public static bool operator ==(LanguageName left, LanguageName right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
		public static bool operator !=(LanguageName left, LanguageName right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for less than.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> if the left instance is less than the right; otherwise, <c>false</c>.</returns>
		public static bool operator <(LanguageName left, LanguageName right)
		{
			return left.CompareTo(right) < 0;
		}

		/// <summary>
		/// Compares two instances for greater than.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> if the left instance is greater than the right; otherwise, <c>false</c>.</returns>
		public static bool operator >(LanguageName left, LanguageName right)
		{
			return left.CompareTo(right) > 0;
		}

		/// <summary>
		/// Compares two instances for less than or equal to.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> if the left instance is less than or equal to the right; otherwise, <c>false</c>.</returns>
		public static bool operator <=(LanguageName left, LanguageName right)
		{
			return left.CompareTo(right) <= 0;
		}

		/// <summary>
		/// Compares two instances for greater than or equal to.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> if the left instance is greater than or equal to the right; otherwise, <c>false</c>.</returns>
		public static bool operator >=(LanguageName left, LanguageName right)
		{
			return left.CompareTo(right) >= 0;
		}

		/// <summary>
		/// The maximum length of a language name.
		/// </summary>
		public const int MaximumNameLength = 50;

		// Initializes with the specified language and region.
		private LanguageName(string strLanguage, string strRegion)
		{
			m_strLanguage = strLanguage;
			m_strRegion = strRegion;
		}

		// Parses a language name.
		private static bool TryParse(string strName, out string strLanguage, out string strRegion)
		{
			// check if empty
			if (string.IsNullOrEmpty(strName))
			{
				// use empty language/region if null or empty; only empty string represents a valid language
				strLanguage = string.Empty;
				strRegion = string.Empty;
				return strName != null;
			}
			else
			{
				// we handle the most common languages with a simple string comparison, rather than by using a regular expression to parse them; this is much faster.
				if (strName.Length == 2 && IsLanguageNameCharacter(strName[0]) && IsLanguageNameCharacter(strName[1]))
				{
					// e.g. en
					strLanguage = strName;
					strRegion = string.Empty;
					return true;
				}
				if (strName.Length == 3 && IsLanguageNameCharacter(strName[0]) && IsLanguageNameCharacter(strName[1]) && IsLanguageNameCharacter(strName[2]))
				{
					// e.g. arc
					strLanguage = strName;
					strRegion = string.Empty;
					return true;
				}
				if (strName.Length == 4 && strName[0] == 'x' && strName[1] == '-' && IsLanguageNameCharacter(strName[2]) && IsLanguageNameCharacter(strName[3]))
				{
					// e.g., x-tl
					strLanguage = strName;
					strRegion = string.Empty;
					return true;
				}
				if (strName.Length == 5 && IsLanguageNameCharacter(strName[0]) && IsLanguageNameCharacter(strName[1]) &&
					strName[2] == '-' && IsAsciiLetter(strName[3]) && IsAsciiLetter(strName[4]))
				{
					// e.g., en-US, en-us
					strLanguage = strName.Substring(0, 2);
					strRegion = strName.Substring(3, 2).ToUpperInvariant();
					return true;
				}

				// use regular expression to match language
				Match match = s_reLanguageTag.Match(strName);
				if (!match.Success || strName.Length > MaximumNameLength)
				{
					strLanguage = string.Empty;
					strRegion = string.Empty;
					return false;
				}

				// extract language and (optional) region
				GetLanguageFromMatch(match, out strLanguage, out strRegion);
				return true;
			}
		}

		// Returns the Language and Region from the results of matching s_reLanguageTag successfully against a string.
		private static void GetLanguageFromMatch(Match match, out string strLanguage, out string strRegion)
		{
			strLanguage = match.Groups["language"].ToString().ToLowerInvariant();
			strRegion = match.Groups["region"].ToString().ToUpperInvariant();

			// check for script
			Group group = match.Groups["script"];
			if (group.Success)
			{
				// case-correct script
				string strScript = group.ToString();
				if (strScript.Length == 3) // hack for zh-CHS and zh-CHT
					strScript = (strScript[2] == 'S' || strScript[2] == 's') ? "Hans" : "Hant";
				else
					strScript = strScript.Substring(0, 1).ToUpperInvariant() + strScript.Substring(1).ToLowerInvariant();
				strLanguage += "-" + strScript;
			}

			// check for overrides
			if (strLanguage == "x-arc")
				strLanguage = "arc";
		}

		// Returns true if 'ch' is a character that can be used in a language name without needing to change case (i.e., [a-z]).
		private static bool IsLanguageNameCharacter(char ch)
		{
			return ch >= 'a' && ch <= 'z';
		}

		// Returns true if 'ch' is an ASCII letter (i.e., [A-Za-z]).
		private static bool IsAsciiLetter(char ch)
		{
			return (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
		}

		static readonly Regex s_reLanguageTag = new Regex(
			@"^(" +
				@"((?'language'([xX]-)?([a-zA-Z]{2,3}))" +
					@"(-(?'script'[a-zA-Z]{4}|[cC][hH][sStT]))?" +
						@"(-(?'region'([a-zA-Z]{2}|[0-9]{3})))?)" +
			@"|" +
				@"(?'language'[xX](-[a-zA-Z0-9]+)+)" +
			@")$",
			RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);

		readonly string m_strLanguage;
		readonly string m_strRegion;
	}
}
