using System;
using System.Globalization;
using NUnit.Framework;

namespace Faithlife.Globalization.Numerals.Tests
{
	[TestFixture]
	public class RomanNumeralsTests
	{
		[Test]
		public void TestValidCharacterToInteger()
		{
			Assert.AreEqual(1, RomanNumerals.ToInteger('I'));
			Assert.AreEqual(1, RomanNumerals.ToInteger('i'));
			Assert.AreEqual(5, RomanNumerals.ToInteger('V'));
			Assert.AreEqual(5, RomanNumerals.ToInteger('v'));
			Assert.AreEqual(10, RomanNumerals.ToInteger('X'));
			Assert.AreEqual(10, RomanNumerals.ToInteger('x'));
			Assert.AreEqual(50, RomanNumerals.ToInteger('L'));
			Assert.AreEqual(50, RomanNumerals.ToInteger('l'));
			Assert.AreEqual(100, RomanNumerals.ToInteger('C'));
			Assert.AreEqual(100, RomanNumerals.ToInteger('c'));
			Assert.AreEqual(500, RomanNumerals.ToInteger('D'));
			Assert.AreEqual(500, RomanNumerals.ToInteger('d'));
			Assert.AreEqual(1000, RomanNumerals.ToInteger('M'));
			Assert.AreEqual(1000, RomanNumerals.ToInteger('m'));
		}

		[Test]
		public void TestInvalidCharacterToInteger()
		{
			Assert.AreEqual(0, RomanNumerals.ToInteger('A'));
			Assert.AreEqual(0, RomanNumerals.ToInteger('a'));
			Assert.AreEqual(0, RomanNumerals.ToInteger('J'));
			Assert.AreEqual(0, RomanNumerals.ToInteger('j'));
			Assert.AreEqual(0, RomanNumerals.ToInteger('Z'));
			Assert.AreEqual(0, RomanNumerals.ToInteger('z'));
			Assert.AreEqual(0, RomanNumerals.ToInteger(' '));
			Assert.AreEqual(0, RomanNumerals.ToInteger('\0'));
		}

		[Test]
		public void TestOneCharacterStringToInteger()
		{
			Assert.AreEqual(1, RomanNumerals.ToInteger("I"));
			Assert.AreEqual(1, RomanNumerals.ToInteger("i"));
			Assert.AreEqual(5, RomanNumerals.ToInteger("V"));
			Assert.AreEqual(5, RomanNumerals.ToInteger("v"));
			Assert.AreEqual(10, RomanNumerals.ToInteger("X"));
			Assert.AreEqual(10, RomanNumerals.ToInteger("x"));
			Assert.AreEqual(50, RomanNumerals.ToInteger("L"));
			Assert.AreEqual(50, RomanNumerals.ToInteger("l"));
			Assert.AreEqual(100, RomanNumerals.ToInteger("C"));
			Assert.AreEqual(100, RomanNumerals.ToInteger("c"));
			Assert.AreEqual(500, RomanNumerals.ToInteger("D"));
			Assert.AreEqual(500, RomanNumerals.ToInteger("d"));
			Assert.AreEqual(1000, RomanNumerals.ToInteger("M"));
			Assert.AreEqual(1000, RomanNumerals.ToInteger("m"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("A"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("a"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("J"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("j"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("Z"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("z"));
			Assert.AreEqual(0, RomanNumerals.ToInteger(" "));
			Assert.AreEqual(0, RomanNumerals.ToInteger(""));
			Assert.AreEqual(0, RomanNumerals.ToInteger(null));
		}

		[Test]
		public void TestStringToInteger()
		{
			Assert.AreEqual(1, RomanNumerals.ToInteger("I"));
			Assert.AreEqual(2, RomanNumerals.ToInteger("II"));
			Assert.AreEqual(3, RomanNumerals.ToInteger("III"));
			Assert.AreEqual(4, RomanNumerals.ToInteger("IV"));
			Assert.AreEqual(5, RomanNumerals.ToInteger("V"));
			Assert.AreEqual(6, RomanNumerals.ToInteger("VI"));
			Assert.AreEqual(7, RomanNumerals.ToInteger("VII"));
			Assert.AreEqual(8, RomanNumerals.ToInteger("VIII"));
			Assert.AreEqual(9, RomanNumerals.ToInteger("IX"));
			Assert.AreEqual(10, RomanNumerals.ToInteger("X"));
			Assert.AreEqual(11, RomanNumerals.ToInteger("XI"));
			Assert.AreEqual(14, RomanNumerals.ToInteger("XIV"));
			Assert.AreEqual(19, RomanNumerals.ToInteger("XIX"));
			Assert.AreEqual(20, RomanNumerals.ToInteger("XX"));
			Assert.AreEqual(31, RomanNumerals.ToInteger("XXXI"));
			Assert.AreEqual(42, RomanNumerals.ToInteger("XLII"));
			Assert.AreEqual(53, RomanNumerals.ToInteger("LIII"));
			Assert.AreEqual(64, RomanNumerals.ToInteger("LXIV"));
			Assert.AreEqual(75, RomanNumerals.ToInteger("LXXV"));
			Assert.AreEqual(86, RomanNumerals.ToInteger("LXXXVI"));
			Assert.AreEqual(97, RomanNumerals.ToInteger("XCVII"));
			Assert.AreEqual(100, RomanNumerals.ToInteger("C"));
			Assert.AreEqual(444, RomanNumerals.ToInteger("CDXLIV"));
			Assert.AreEqual(999, RomanNumerals.ToInteger("CMXCIX"));
			Assert.AreEqual(2006, RomanNumerals.ToInteger("MMVI"));
			Assert.AreEqual(4999, RomanNumerals.ToInteger("MMMMCMXCIX"));
		}

		[Test]
		public void TestOtherCaseStringToInteger()
		{
			Assert.AreEqual(999, RomanNumerals.ToInteger("cmxcix"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("MmvI")); // mixed case not allowed
			Assert.AreEqual(0, RomanNumerals.ToInteger("mmMMcmXCix")); // mixed case not allowed
		}

		[Test]
		public void TestUnusualStringToInteger()
		{
			Assert.AreEqual(0, RomanNumerals.ToInteger("IC"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("IIC"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("IVXLCDM"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("CCCD"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("CCCCCD"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("CCCCCCCD"));
			Assert.AreEqual(0, RomanNumerals.ToInteger("Civil"));
		}

		[Test]
		public void TestIntegerToString()
		{
			Assert.AreEqual("I", RomanNumerals.ToUpperCaseString(1));
			Assert.AreEqual("II", RomanNumerals.ToUpperCaseString(2));
			Assert.AreEqual("III", RomanNumerals.ToUpperCaseString(3));
			Assert.AreEqual("IV", RomanNumerals.ToUpperCaseString(4));
			Assert.AreEqual("V", RomanNumerals.ToUpperCaseString(5));
			Assert.AreEqual("VI", RomanNumerals.ToUpperCaseString(6));
			Assert.AreEqual("VII", RomanNumerals.ToUpperCaseString(7));
			Assert.AreEqual("VIII", RomanNumerals.ToUpperCaseString(8));
			Assert.AreEqual("IX", RomanNumerals.ToUpperCaseString(9));
			Assert.AreEqual("X", RomanNumerals.ToUpperCaseString(10));
			Assert.AreEqual("XI", RomanNumerals.ToUpperCaseString(11));
			Assert.AreEqual("XIV", RomanNumerals.ToUpperCaseString(14));
			Assert.AreEqual("XIX", RomanNumerals.ToUpperCaseString(19));
			Assert.AreEqual("XX", RomanNumerals.ToUpperCaseString(20));
			Assert.AreEqual("XXXI", RomanNumerals.ToUpperCaseString(31));
			Assert.AreEqual("XLII", RomanNumerals.ToUpperCaseString(42));
			Assert.AreEqual("LIII", RomanNumerals.ToUpperCaseString(53));
			Assert.AreEqual("LXIV", RomanNumerals.ToUpperCaseString(64));
			Assert.AreEqual("LXXV", RomanNumerals.ToUpperCaseString(75));
			Assert.AreEqual("LXXXVI", RomanNumerals.ToUpperCaseString(86));
			Assert.AreEqual("XCVII", RomanNumerals.ToUpperCaseString(97));
			Assert.AreEqual("C", RomanNumerals.ToUpperCaseString(100));
			Assert.AreEqual("CDXLIV", RomanNumerals.ToUpperCaseString(444));
			Assert.AreEqual("CMXCIX", RomanNumerals.ToUpperCaseString(999));
			Assert.AreEqual("MMVI", RomanNumerals.ToUpperCaseString(2006));
			Assert.AreEqual("MMMMCMXCIX", RomanNumerals.ToUpperCaseString(4999));

			Assert.AreEqual("i", RomanNumerals.ToLowerCaseString(1));
			Assert.AreEqual("mmmmcmxcix", RomanNumerals.ToLowerCaseString(4999));
		}

		[Test]
		public void TestToStringOutOfRange()
		{
			Assert.Throws<ArgumentOutOfRangeException>(delegate { RomanNumerals.ToUpperCaseString(int.MinValue); });
			Assert.Throws<ArgumentOutOfRangeException>(delegate { RomanNumerals.ToUpperCaseString(RomanNumerals.MinValue - 1); });
			Assert.Throws<ArgumentOutOfRangeException>(delegate { RomanNumerals.ToUpperCaseString(0); });
			Assert.Throws<ArgumentOutOfRangeException>(delegate { RomanNumerals.ToUpperCaseString(RomanNumerals.MaxValue + 1); });
			Assert.Throws<ArgumentOutOfRangeException>(delegate { RomanNumerals.ToUpperCaseString(int.MaxValue); });
		}

		[Test]
		public void TestMinMax()
		{
			Assert.AreEqual(1, RomanNumerals.MinValue);
			Assert.AreEqual(4999, RomanNumerals.MaxValue);
		}

		[Test]
		public void FormatterNoFallback()
		{
			Assert.AreEqual("LXXXVI + xlii = 128", string.Format(RomanNumerals.GetFormatProvider(null),
				"{0:XVI} + {1:xvi} = {2:n0}", 86, 42, 86 + 42));
		}

		[Test]
		public void FormatterFallback()
		{
			DateTime dt = new DateTime(2006, 10, 12);
			Assert.AreEqual("12/10/2006 MMVI", string.Format(RomanNumerals.GetFormatProvider(new CultureInfo("en-NZ")),
				"{0:d} {1:XVI}", dt, dt.Year));
		}

		[Test]
		public void FormatterCascadingFallback()
		{
			DateTime dt = new DateTime(2006, 10, 12);
			Assert.AreEqual("12/10/2006 MMVI", string.Format(RomanNumerals.GetFormatProvider(
				RomanNumerals.GetFormatProvider(new CultureInfo("en-NZ"))),
				"{0:d} {1:XVI}", dt, dt.Year));
		}

		[Test]
		public void FormatterOutOfRange()
		{
			Assert.AreEqual("100000 -100000", string.Format(RomanNumerals.GetFormatProvider(null),
				"{0:XVI} {1:xvi}", 100000, -100000));
		}
	}
}
