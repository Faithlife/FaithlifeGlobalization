namespace Faithlife.Globalization
{
	/// <summary>
	/// Describes a comparison between two language names.
	/// </summary>
	public enum LanguageNameMatch
	{
		/// <summary>
		/// Comparison failure.
		/// </summary>
		None,

		/// <summary>
		/// The language names use different primary language tag.
		/// </summary>
		DifferentLanguages,

		/// <summary>
		/// The first language name is null or empty.
		/// </summary>
		FirstEmpty,

		/// <summary>
		/// The second language name is null or empty.
		/// </summary>
		SecondEmpty,

		/// <summary>
		/// The language names use the same primary language tag but different region tag.
		/// </summary>
		DifferentRegions,

		/// <summary>
		/// The language names use the same primary language tag, but the first has no region tag.
		/// </summary>
		FirstNeutral,

		/// <summary>
		/// The language names use the same primary language tag, but the second has no region code.
		/// </summary>
		SecondNeutral,

		/// <summary>
		/// The language names use the same primary language and region tags.
		/// </summary>
		Identical
	}
}
