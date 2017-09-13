using System;
using NUnit.Framework;

namespace Faithlife.Globalization.Numerals.Tests
{
	[TestFixture]
	public class ChineseNumeralsTests
	{
		[TestCase(0, '〇')]
		[TestCase(0, '零')]
		[TestCase(1, '一')]
		[TestCase(1, '壹')]
		[TestCase(2, '二')]
		[TestCase(2, '貳')]
		[TestCase(2, '贰')]
		[TestCase(2, '兩')]
		[TestCase(2, '两')]
		[TestCase(3, '三')]
		[TestCase(3, '叄')]
		[TestCase(3, '叁')]
		[TestCase(3, '參')]
		[TestCase(4, '四')]
		[TestCase(4, '肆')]
		[TestCase(5, '五')]
		[TestCase(5, '伍')]
		[TestCase(6, '六')]
		[TestCase(6, '陸')]
		[TestCase(6, '陆')]
		[TestCase(7, '七')]
		[TestCase(7, '柒')]
		[TestCase(8, '八')]
		[TestCase(8, '捌')]
		[TestCase(9, '九')]
		[TestCase(9, '玖')]
		[TestCase(10, '十')]
		[TestCase(10, '拾')]
		[TestCase(100, '百')]
		[TestCase(100, '佰')]
		[TestCase(null, '千')]
		public void TestToIntegerForCharacter(int? expectedValue, char chineseCharacter)
		{
			Assert.AreEqual(expectedValue, ChineseNumerals.ToInteger(chineseCharacter));
		}

		[TestCase(5, "五")]
		[TestCase(50, "五十")]
		[TestCase(100, "百")]
		[TestCase(110, "一百十")]
		[TestCase(110, "一百一")]
		[TestCase(110, "一百一十")]
		[TestCase(110, "百十")]
		[TestCase(110, "百一")]
		[TestCase(110, "百一十")]
		[TestCase(500, "五百")]
		[TestCase(503, "五百〇三")]
		[TestCase(530, "五百三")]
		[TestCase(530, "五百三十")]
		[TestCase(531, "五百三十一")]
		[TestCase(550, "五百五十")]
		[TestCase(550, "五百五")]
		[TestCase(null, "五百〇〇三")]
		[TestCase(null, "五百三十一一")]
		[TestCase(null, "五百三三十一")]
		[TestCase(null, "三十五百一")]
		[TestCase(null, "三十五十一")]
		[TestCase(null, "十一五百三")]
		[TestCase(null, "五百三十一三")]
		[TestCase(null, "五百三一十一")]
		[TestCase(null, "5百五")]
		[TestCase(null, "五百〇5")]
		[TestCase(null, "一百4十")]
		[TestCase(null, "五百三a")]
		public void TestToIntegerForString(int? expectedValue, string chineseNumericString)
		{
			Assert.AreEqual(expectedValue, ChineseNumerals.ToInteger(chineseNumericString));
		}

		[TestCase(632, "陆佰叁拾贰", "陸佰參拾貳", "六百三十二")]
		[TestCase(941, "玖佰肆拾壹", "玖佰肆拾壹", "九百四十一")]
		[TestCase(210, "贰佰壹拾", "貳佰壹拾", "二百一十")]
		[TestCase(211, "贰佰拾壹", "貳佰拾壹", "二百十一")]
		public void TestEachTypeOfString(int value, string financialSimplified, string financialTraditional, string normal)
		{
			Assert.AreEqual(financialSimplified, ChineseNumerals.ToString(value, ChineseNumeralFormat.FinancialSimplified));
			Assert.AreEqual(financialTraditional, ChineseNumerals.ToString(value, ChineseNumeralFormat.FinancialTraditional));
			Assert.AreEqual(normal, ChineseNumerals.ToString(value, ChineseNumeralFormat.Normal));
		}

		[TestCase(1000)]
		[TestCase(0)]
		public void TestArgumentOutOfRangeException(int value)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => ChineseNumerals.ToString(value, ChineseNumeralFormat.Normal));
		}

		[Test]
		public void TestRoundTripForEachType()
		{
			for (int i = ChineseNumerals.MinValue; i <= ChineseNumerals.MaxValue; i++)
			{
				foreach (ChineseNumeralFormat format in Enum.GetValues(typeof(ChineseNumeralFormat)))
					Assert.AreEqual(i, ChineseNumerals.ToInteger(ChineseNumerals.ToString(i, format)), $"Format:{format}");
			}
		}

		[TestCase("ZHN", "六百三十二")]
		[TestCase("ZHT", "陸佰參拾貳")]
		[TestCase("ZHS", "陆佰叁拾贰")]
		public void TestFormat(string format, string expectedValue)
		{
			Assert.AreEqual(expectedValue, string.Format(ChineseNumerals.GetFormatProvider(null), ("{0:" + format + "}"), 632));
		}
	}
}
