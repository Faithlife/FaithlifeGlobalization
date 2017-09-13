namespace Faithlife.Globalization.Numerals
{
	/// <summary>
	/// The different types of Chinese numerals.
	/// </summary>
	public enum ChineseNumeralFormat
	{
		/// <summary>
		/// The normal representation of a chinese numeral. Traditional and Simplified Chinese use the same characters for the values we care about.
		/// </summary>
		Normal = 0,

		/// <summary>
		/// The financial traditional representation of a Chinese numeral.
		/// </summary>
		///
		FinancialTraditional = 1,

		/// <summary>
		/// The financial simplified representation of a Chinese numeral.
		/// </summary>
		FinancialSimplified = 2,
	}
}
