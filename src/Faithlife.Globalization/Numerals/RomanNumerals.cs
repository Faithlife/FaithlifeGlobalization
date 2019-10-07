using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Faithlife.Utility;

namespace Faithlife.Globalization.Numerals
{
	/// <summary>
	/// Provides methods for converting strings of roman numerals to and from integers.
	/// </summary>
	public static class RomanNumerals
	{
		/// <summary>
		/// Converts the specified roman numeral letter to an integer.
		/// </summary>
		/// <param name="ch">The letter.</param>
		/// <returns>The corresponding integer, or zero if the letter isn't a roman numeral.</returns>
		public static int ToInteger(char ch)
		{
			switch (ch)
			{
				case 'I':
				case 'i':
					return 1;
				case 'V':
				case 'v':
					return 5;
				case 'X':
				case 'x':
					return 10;
				case 'L':
				case 'l':
					return 50;
				case 'C':
				case 'c':
					return 100;
				case 'D':
				case 'd':
					return 500;
				case 'M':
				case 'm':
					return 1000;
				default:
					return 0;
			}
		}

		/// <summary>
		/// Converts the specified roman numeral string to an integer.
		/// </summary>
		/// <param name="str">The string.</param>
		/// <returns>The corresponding integer, or zero if the string contains anything that isn't a roman numeral.</returns>
		public static int ToInteger(string str)
		{
			if (string.IsNullOrEmpty(str) || !s_reRomanNumeralString.IsMatch(str))
				return 0;

			int nResult = 0;
			int nLastLetter = 0;
			char[] ach = str.ToCharArray();
			for (int nIndex = ach.Length - 1; nIndex >= 0; nIndex--)
			{
				int nLetter = ToInteger(ach[nIndex]);

				if (nLetter < nLastLetter)
				{
					nResult -= nLetter;
				}
				else
				{
					nResult += nLetter;
					nLastLetter = nLetter;
				}
			}
			return nResult;
		}

		/// <summary>
		/// Converts the specified integer to an upper case roman numeral string.
		/// </summary>
		/// <param name="n">The integer.</param>
		/// <returns>The corresponding upper case roman numeral string.</returns>
		public static string ToUpperCaseString(int n)
		{
			if (n < MinValue || n > MaxValue)
				throw new ArgumentOutOfRangeException("n");

			StringBuilder sb = new StringBuilder(20);
			foreach (NumberToLetters nts in s_ants)
			{
				while (n >= nts.Number)
				{
					sb.Append(nts.Letters);
					n -= nts.Number;
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Converts the specified integer to a lower case roman numeral string.
		/// </summary>
		/// <param name="n">The integer.</param>
		/// <returns>The corresponding lower case roman numeral string.</returns>
		public static string ToLowerCaseString(int n)
		{
			return ToUpperCaseString(n).ToLowerInvariant();
		}

		/// <summary>
		/// Gets the minimum value that can be represented as a roman numeral by this class.
		/// </summary>
		/// <value>The minimum value that can be represented as a roman numeral by this class.</value>
		public static int MinValue
		{
			get { return 1; }
		}

		/// <summary>
		/// Gets the maximum value that can be represented as a roman numeral by this class.
		/// </summary>
		/// <value>The maximum value that can be represented as a roman numeral by this class.</value>
		public static int MaxValue
		{
			get { return 4999; }
		}

		/// <summary>
		/// Returns a format provider that renders integers as roman numerals.
		/// </summary>
		/// <param name="fpFallback">The format provider to which to fallback. (Can be null.)</param>
		/// <returns>A format provider that renders integers as roman numerals.</returns>
		public static IFormatProvider GetFormatProvider(IFormatProvider fpFallback)
		{
			return new OurFormatProvider(fpFallback);
		}

		[StructLayout(LayoutKind.Auto)]
		private struct NumberToLetters
		{
			public NumberToLetters(int n, string str)
			{
				Number = n;
				Letters = str;
			}

			public readonly int Number;
			public readonly string Letters;
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
				if (format != "xvi" && format != "XVI")
					return null;

				int nInput = (int) arg;
				if (nInput < RomanNumerals.MinValue || nInput > RomanNumerals.MaxValue)
					return nInput.ToInvariantString();

				return format[0] == 'x' ?
					RomanNumerals.ToLowerCaseString(nInput) :
					RomanNumerals.ToUpperCaseString(nInput);
			}
		}

		static readonly NumberToLetters[] s_ants =
		{
			new NumberToLetters(1000, "M"),
			new NumberToLetters(900, "CM"),
			new NumberToLetters(500, "D"),
			new NumberToLetters(400, "CD"),
			new NumberToLetters(100, "C"),
			new NumberToLetters(90, "XC"),
			new NumberToLetters(50, "L"),
			new NumberToLetters(40, "XL"),
			new NumberToLetters(10, "X"),
			new NumberToLetters(9, "IX"),
			new NumberToLetters(5, "V"),
			new NumberToLetters(4, "IV"),
			new NumberToLetters(1, "I"),
		};

		static readonly Regex s_reRomanNumeralString = new Regex(@"^(" +
			@"M{0,4}(DC{0,4}|CM|CD|C{0,4})(LX{0,4}|XC|XL|X{0,4})(VI{0,4}|IX|IV|I{0,4})" +
			@"|" +
			@"m{0,4}(dc{0,4}|cm|cd|c{0,4})(lx{0,4}|xc|xl|x{0,4})(vi{0,4}|ix|iv|i{0,4})" +
			@")$",
			RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture);
	}
}
