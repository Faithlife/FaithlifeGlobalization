
#pragma warning disable 618

using System;
using NUnit.Framework;

namespace Faithlife.Globalization.Tests
{
	[TestFixture]
	public class LanguageNameTests
	{
		[Test]
		public void Null()
		{
			Assert.IsTrue(LanguageName.IsValidName(null));
			LanguageName name = new LanguageName(null);
			Assert.AreEqual(0, name.Language.Length);
			Assert.AreEqual(0, name.Region.Length);
		}

		[Test]
		public void Empty()
		{
			Assert.IsTrue(LanguageName.IsValidName(""));
			LanguageName name = new LanguageName("");
			Assert.AreEqual(0, name.Language.Length);
			Assert.AreEqual(0, name.Region.Length);
		}

		[Test]
		public void English()
		{
			Assert.IsTrue(LanguageName.IsValidName("en"));
			LanguageName name = new LanguageName("en");
			Assert.AreEqual("en", name.Language);
			Assert.AreEqual(0, name.Region.Length);
		}

		[Test]
		public void NewZealandEnglish()
		{
			Assert.IsTrue(LanguageName.IsValidName("en-NZ"));
			LanguageName name = new LanguageName("en-NZ");
			Assert.AreEqual("en", name.Language);
			Assert.AreEqual("NZ", name.Region);
		}

		[Test]
		public void MexicanSpanish()
		{
			Assert.IsTrue(LanguageName.IsValidName("es-MX"));
			LanguageName name = new LanguageName("es-MX");
			Assert.AreEqual("es-MX", name.FullName);
			Assert.AreEqual("es-MX", name.ToString());
		}

		[Test]
		public void AzCyrlAz()
		{
			Assert.IsTrue(LanguageName.IsValidName("az-cyrl-az"));
			LanguageName name = new LanguageName("az-cyrl-az");
			Assert.AreEqual("az-Cyrl-AZ", name.ToString());
			Assert.AreEqual("az-Cyrl", name.Language);
			Assert.AreEqual("AZ", name.Region);
		}

		[Test]
		public void Chinese()
		{
			LanguageName name = new LanguageName("zh-chs");
			Assert.AreEqual("zh-Hans", name.ToString());
			name = new LanguageName("ZH-CHT");
			Assert.AreEqual("zh-Hant", name.ToString());
			name = new LanguageName("ZH-HANS");
			Assert.AreEqual("zh-Hans", name.ToString());
			name = new LanguageName("zh-hant");
			Assert.AreEqual("zh-Hant", name.ToString());
		}

		[Test]
		public void Syriac()
		{
			Assert.IsTrue(LanguageName.IsValidName("syr"));
			LanguageName name = new LanguageName("syr");
			Assert.AreEqual("syr", name.Language);
			Assert.AreEqual(0, name.Region.Length);
			Assert.AreEqual("syr", name.ToString());
		}

		[Test]
		public void MartianSyriac()
		{
			Assert.IsTrue(LanguageName.IsValidName("syr-mars"));
			LanguageName name = new LanguageName("syr-mars");
			Assert.AreEqual("syr-Mars", name.Language);
			Assert.AreEqual("", name.Region);
		}

		[Test]
		public void Aramaic()
		{
			Assert.IsTrue(LanguageName.IsValidName("x-arc"));
			LanguageName name = new LanguageName("x-arc");
			Assert.AreEqual("arc", name.Language);
			Assert.AreEqual(0, name.Region.Length);

			Assert.IsTrue(LanguageName.IsValidName("arc"));
			name = new LanguageName("arc");
			Assert.AreEqual("arc", name.Language);
			Assert.AreEqual(0, name.Region.Length);
		}

		[Test]
		public void AramaicPlusRegion()
		{
			Assert.IsTrue(LanguageName.IsValidName("x-arc-gb"));
			LanguageName name = new LanguageName("x-arc-gb");
			Assert.AreEqual("arc", name.Language);
			Assert.AreEqual("GB", name.Region);
		}

		[Test]
		public void Extension()
		{
			Assert.IsTrue(LanguageName.IsValidName("x-abc"));
			LanguageName name = new LanguageName("x-abc");
			Assert.AreEqual("x-abc", name.Language);
			Assert.AreEqual(0, name.Region.Length);
		}

		[Test]
		public void ExtensionPlusRegion()
		{
			Assert.IsTrue(LanguageName.IsValidName("x-abc-gb"));
			LanguageName name = new LanguageName("x-abc-gb");
			Assert.AreEqual("x-abc", name.Language);
			Assert.AreEqual("GB", name.Region);
		}

		[Test]
		public void ExtensionPlusScript()
		{
			Assert.IsTrue(LanguageName.IsValidName("x-abc-abcd"));
			LanguageName name = new LanguageName("x-abc-abcd");
			Assert.AreEqual("x-abc-Abcd", name.Language);
			Assert.AreEqual("", name.Region);
		}

		[Test]
		public void ExtensionPlusScriptAndRegion()
		{
			Assert.IsTrue(LanguageName.IsValidName("x-abc-abcd-ab"));
			LanguageName name = new LanguageName("x-abc-abcd-ab");
			Assert.AreEqual("x-abc-Abcd", name.Language);
			Assert.AreEqual("AB", name.Region);
		}

		[Test]
		public void UNRegion()
		{
			Assert.IsTrue(LanguageName.IsValidName("es-419"));
			LanguageName name = new LanguageName("es-419");
			Assert.AreEqual("es", name.Language);
			Assert.AreEqual("419", name.Region);
		}

		[Test]
		public void PrivateUse()
		{
			Assert.IsTrue(LanguageName.IsValidName("x-lg-sym"));
			LanguageName name = new LanguageName("x-lg-sym");
			Assert.AreEqual("x-lg-sym", name.Language);
			Assert.AreEqual("", name.Region);
		}

		[TestCase("000")]
		[TestCase("es-419-MX")]
		[TestCase("es-MX-419")]
		public void InvalidRegion(string strName)
		{
			Assert.IsFalse(LanguageName.IsValidName(strName));
			Assert.Throws<ArgumentException>(delegate { LanguageName name = new LanguageName(strName); });
		}

		[Test]
		public void BlankRegion()
		{
			Assert.IsFalse(LanguageName.IsValidName("en-"));
			Assert.Throws<ArgumentException>(delegate { LanguageName name = new LanguageName("en-"); });
		}

		[Test]
		public void ExtraHyphen()
		{
			Assert.IsFalse(LanguageName.IsValidName("en-nz-"));
			Assert.Throws<ArgumentException>(delegate { LanguageName name = new LanguageName("en-nz-"); });
		}

		[Test]
		public void AutomaticCaseConversion()
		{
			Assert.AreEqual("en", new LanguageName("eN").Language);
			Assert.AreEqual("en", new LanguageName("En").Language);
			Assert.AreEqual("en", new LanguageName("EN").Language);
			Assert.AreEqual("arc", new LanguageName("Arc").Language);
			Assert.AreEqual("US", new LanguageName("en-us").Region);
			Assert.AreEqual("US", new LanguageName("en-Us").Region);
			Assert.AreEqual("US", new LanguageName("en-uS").Region);
		}

		[Test]
		public void Equal()
		{
			LanguageName name1 = new LanguageName("en-NZ");
			LanguageName name2 = new LanguageName("EN-nz");
			Assert.IsTrue(name1.Equals(name2));
			Assert.IsTrue(name1.Equals((object) name2));
			Assert.IsTrue(name1 == name2);
			Assert.IsFalse(name1 != name2);
			Assert.AreEqual(name1.GetHashCode(), name2.GetHashCode());
			Assert.AreEqual(0, name1.CompareTo(name2));
			Assert.AreEqual(0, ((IComparable) name1).CompareTo(name2));
			Assert.IsFalse(name1 < name2);
			Assert.IsTrue(name1 <= name2);
			Assert.IsFalse(name1 > name2);
			Assert.IsTrue(name1 >= name2);
		}

		[Test]
		public void EqualNoRegion()
		{
			LanguageName name1 = new LanguageName("en");
			LanguageName name2 = new LanguageName("EN");
			Assert.IsTrue(name1.Equals(name2));
			Assert.IsTrue(name1.Equals((object)name2));
			Assert.IsTrue(name1 == name2);
			Assert.IsFalse(name1 != name2);
			Assert.AreEqual(name1.GetHashCode(), name2.GetHashCode());
			Assert.AreEqual(0, name1.CompareTo(name2));
			Assert.AreEqual(0, ((IComparable) name1).CompareTo(name2));
			Assert.IsFalse(name1 < name2);
			Assert.IsTrue(name1 <= name2);
			Assert.IsFalse(name1 > name2);
			Assert.IsTrue(name1 >= name2);
		}

		[Test]
		public void EqualNeutral()
		{
			AssertEqualsNeutral(new LanguageName());
			AssertEqualsNeutral(new LanguageName(null));
			AssertEqualsNeutral(new LanguageName(""));
		}

		private static void AssertEqualsNeutral(LanguageName name1)
		{
			LanguageName name2 = LanguageName.Neutral;
			Assert.IsTrue(name1.Equals(name2));
			Assert.IsTrue(name1.Equals((object) name2));
			Assert.IsTrue(name1 == name2);
			Assert.IsFalse(name1 != name2);
			Assert.AreEqual(name1.GetHashCode(), name2.GetHashCode());
			Assert.AreEqual(0, name1.CompareTo(name2));
			Assert.AreEqual(0, ((IComparable) name1).CompareTo(name2));
			Assert.IsFalse(name1 < name2);
			Assert.IsTrue(name1 <= name2);
			Assert.IsFalse(name1 > name2);
			Assert.IsTrue(name1 >= name2);
		}

		[Test]
		public void NotEqual()
		{
			LanguageName name1 = new LanguageName("en-NZ");
			LanguageName name2 = new LanguageName("ES-nZ");
			Assert.IsFalse(name1.Equals(name2));
			Assert.IsFalse(name1.Equals((object)name2));
			Assert.IsFalse(name1 == name2);
			Assert.IsTrue(name1 != name2);
			Assert.Less(((IComparable) name1).CompareTo(name2), 0);
			Assert.IsTrue(name1 < name2);
			Assert.IsTrue(name1 <= name2);
			Assert.IsFalse(name1 > name2);
			Assert.IsFalse(name1 >= name2);
		}

		[Test]
		public void NotEqualSameLanguage()
		{
			LanguageName name1 = new LanguageName("en-NZ");
			LanguageName name2 = new LanguageName("eN-Au");
			Assert.IsFalse(name1.Equals(name2));
			Assert.IsFalse(name1.Equals((object) name2));
			Assert.IsFalse(name1 == name2);
			Assert.IsTrue(name1 != name2);
			Assert.Greater(((IComparable) name1).CompareTo(name2), 0);
			Assert.IsFalse(name1 < name2);
			Assert.IsFalse(name1 <= name2);
			Assert.IsTrue(name1 > name2);
			Assert.IsTrue(name1 >= name2);
		}

		[Test]
		public void NonEmptyRegionWithEmptyLanguage()
		{
			Assert.Throws<ArgumentException>(delegate { LanguageName name = new LanguageName("-AU"); });
		}

		[TestCase(null, false, "", "")]
		[TestCase("", true, "", "")]
		[TestCase("en", true, "en", "")]
		[TestCase("en-", false, "", "")]
		[TestCase("en-NZ", true, "en", "NZ")]
		[TestCase("en-nz-", false, "", "")]
		[TestCase("es-mx", true, "es", "MX")]
		[TestCase("x-abc-abcd-ab", true, "x-abc-Abcd", "AB")]
		[TestCase("ZH-CHT", true, "zh-Hant", "")]
		[TestCase("es-419", true, "es", "419")]
		public void TryCreateLanguage(string strInput, bool bExpected, string strExpectedLanguage, string strExpectedRegion)
		{
			LanguageName language;
			Assert.AreEqual(bExpected, LanguageName.TryCreate(strInput, out language));
			Assert.AreEqual(strExpectedLanguage, language.Language);
			Assert.AreEqual(strExpectedRegion, language.Region);
			Assert.AreEqual(bExpected ? language : default(LanguageName?), LanguageName.TryCreate(strInput));
		}

		[Test]
		public void NotTooLong()
		{
			new LanguageName("x-" + new string('a', 48));
		}

		[Test]
		public void TooLong()
		{
			Assert.Throws<ArgumentException>(() => new LanguageName("x-" + new string('a', 49)));
		}

		[Test]
		public void NonAscii()
		{
			Assert.Throws<ArgumentException>(() => new LanguageName("x-\u00e9"));
		}
	}
}
