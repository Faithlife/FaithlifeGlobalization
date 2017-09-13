using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Faithlife.Globalization.Numerals
{
	/// <summary>
	/// Provides methods for converting strings of Chinese numerals to and from integers.
	/// </summary>
	public static class ChineseNumerals
	{
		/// <summary>
		/// Gets the numeric value for any single Chinese character.
		/// </summary>
		/// <param name="ch">The Chinese character.</param>
		/// <returns>The numeric value, or <c>null</c> if the value is not a Chinese character, or is simply not supported.</returns>
		public static int? ToInteger(char ch)
		{
			switch (ch)
			{
				case '零':
				case '〇':
					return 0;
				case '一':
				case '壹':
					return 1;
				case '二':
				case '貳':
				case '贰':
				case '兩':
				case '两':
					return 2;
				case '三':
				case '叄':
				case '叁':
				case '參':
					return 3;
				case '四':
				case '肆':
					return 4;
				case '五':
				case '伍':
					return 5;
				case '六':
				case '陸':
				case '陆':
					return 6;
				case '七':
				case '柒':
					return 7;
				case '八':
				case '捌':
					return 8;
				case '九':
				case '玖':
					return 9;
				case '十':
				case '拾':
					return 10;
				case '百':
				case '佰':
					return 100;
				default:
					return null;
			}
		}

		/// <summary>
		/// Converts the Chinese numeral string to an integer.
		/// </summary>
		/// <param name="chineseNumericString">The Chinese numeral string.</param>
		/// <returns>An integer with the value represented by the Chinese numeral, or <c>null</c> if the string was in a bad format, contained non Chinese numerals, or contained Chinese numerals that aren't supported.</returns>
		public static int? ToInteger(string chineseNumericString)
		{
			int? toReturn = null;
			if (!string.IsNullOrWhiteSpace(chineseNumericString) && s_onlyChineseNumeralsRegex.IsMatch(chineseNumericString))
			{
				bool failure = false;
				int? hundredsPlace = null;
				int? tensPlace = null;
				int? onesDigit = null;
				int currentMultiplier = 1;
				foreach (int? currentValue in chineseNumericString.ToCharArray().Select(ToInteger))
				{
					if (currentValue == 100 && !hundredsPlace.HasValue && !tensPlace.HasValue)
					{
						hundredsPlace = onesDigit.GetValueOrDefault(1);
						currentMultiplier = 10;
						onesDigit = null;
					}
					else if (currentValue == 10 && !tensPlace.HasValue)
					{
						tensPlace = onesDigit.GetValueOrDefault(1);
						onesDigit = null;
						currentMultiplier = 1;
					}
					else if (currentValue >= 0 && currentValue < 10 && (!onesDigit.HasValue || (onesDigit == 0 && currentValue != 0)))
					{
						onesDigit = currentValue;
						if (currentValue == 0)
							currentMultiplier = 1;
					}
					else
					{
						failure = true;
						break;
					}
				}

				toReturn = failure ? default(int?) :
					100 * hundredsPlace.GetValueOrDefault(0) +
					10 * tensPlace.GetValueOrDefault(0) +
					currentMultiplier * onesDigit.GetValueOrDefault(0);
			}

			return toReturn;
		}

		/// <summary>
		/// Returns a format provider that renders integers as Chinese numerals.
		/// </summary>
		/// <param name="fpFallback">The format provider to which to fall back. (Can be <c>null</c>.)</param>
		/// <returns>A format provider that renders integers as Chinese numerals.</returns>
		public static IFormatProvider GetFormatProvider(IFormatProvider fpFallback)
		{
			return new OurFormatProvider(fpFallback);
		}

		private sealed class OurFormatProvider : FormatProviderBase
		{
			public OurFormatProvider(IFormatProvider fpFallback)
				: base(fpFallback)
			{
			}

			protected override string FormatCore(string format, object arg)
			{
				if (!(arg is int))
					return null;
				int nInput = (int) arg;
				if (nInput < MinValue || nInput > MaxValue)
					return null;
				if (format == "ZHN")
					return ChineseNumerals.ToString(nInput, ChineseNumeralFormat.Normal);
				if (format == "ZHT")
					return ChineseNumerals.ToString(nInput, ChineseNumeralFormat.FinancialTraditional);
				if (format == "ZHS")
					return ChineseNumerals.ToString(nInput, ChineseNumeralFormat.FinancialSimplified);
				return null;
			}
		}

		/// <summary>
		/// Gets the minimum value that can be represented as a Chinese numeral by this class.
		/// </summary>
		/// <value>The minimum value that can be represented as a Chinese numeral by this class.</value>
		public static int MinValue
		{
			get { return 1; }
		}

		/// <summary>
		/// Gets the maximum value that can be represented as a Chinese numeral by this class.
		/// </summary>
		/// <value>The maximum value that can be represented as a Chinese numeral by this class.</value>
		public static int MaxValue
		{
			get { return 999; }
		}

		/// <summary>
		/// Gets the string representation of <paramref name="number"/>.
		/// </summary>
		/// <param name="number">The number to convert.</param>
		/// <param name="numeralFormat">The numeral format to create the string in.</param>
		/// <returns>A string representing the number in a the format for the given <paramref name="numeralFormat"/>.</returns>
		/// <exception cref="ArgumentOutOfRangeException">Value must be greater than or equal to <see cref="MinValue"/> and less than or equal to <see cref="MaxValue"/>.</exception>
		public static string ToString(int number, ChineseNumeralFormat numeralFormat)
		{
			if (number < MinValue || number > MaxValue)
				throw new ArgumentOutOfRangeException("number");

			StringBuilder sb = new StringBuilder(20);
			int hundredsPlace = number / 100;
			int tensPlace = (number % 100) / 10;
			int onesPlace = (number % 10);

			if (hundredsPlace > 1)
				sb.Append(GetLetterForValue(hundredsPlace, numeralFormat));
			if (hundredsPlace > 0)
				sb.Append(GetLetterForValue(100, numeralFormat));

			// for number=110 we should return "百一十", but for number=111-119 we should drop the '一' before the '十' and return "百十一"
			if (tensPlace > 1 || (tensPlace == 1 && hundredsPlace > 0 && onesPlace == 0))
				sb.Append(GetLetterForValue(tensPlace, numeralFormat));
			if (tensPlace > 0)
				sb.Append(GetLetterForValue(10, numeralFormat));
			if (tensPlace == 0 && hundredsPlace > 0 && onesPlace > 0)
				sb.Append(GetLetterForValue(0, numeralFormat));

			if (onesPlace > 0)
				sb.Append(GetLetterForValue(onesPlace, numeralFormat));
			return sb.ToString();
		}

		private static char GetLetterForValue(int value, ChineseNumeralFormat numeralFormat)
		{
			return s_numberToLetters[value][(int) numeralFormat];
		}

		static readonly Dictionary<int, string> s_numberToLetters = new Dictionary<int, string>
		{
			{ 100, "百佰佰" },
			{ 10, "十拾拾" },
			{ 9, "九玖玖" },
			{ 8, "八捌捌" },
			{ 7, "七柒柒" },
			{ 6, "六陸陆" },
			{ 5, "五伍伍" },
			{ 4, "四肆肆" },
			{ 3, "三參叁" },
			{ 2, "二貳贰" },
			{ 1, "一壹壹" },
			{ 0, "〇零零" },
		};

		static readonly Regex s_onlyChineseNumeralsRegex = new Regex("^[〇零一壹二貳贰兩两三叄叁參四肆五伍六陸陆七柒八捌九玖十拾百佰]+$");
	}
}
