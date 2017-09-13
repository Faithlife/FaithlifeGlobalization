using System;
using System.Globalization;
using NUnit.Framework;

namespace Faithlife.Globalization.Tests
{
	[TestFixture]
	public class CultureTests
	{
		[Test]
		public void CultureInfoFromCulture()
		{
			Culture cultureEnglishUS = Culture.EnglishUS;
			Assert.AreEqual(0, string.Compare("en-US", cultureEnglishUS.FormatCultureInfo.Name, StringComparison.OrdinalIgnoreCase));
			Assert.AreEqual(0, string.Compare("en-US", cultureEnglishUS.ResourceCultureInfo.Name, StringComparison.OrdinalIgnoreCase));
		}

		[Test]
		public void CreateNull()
		{
			const CultureInfo ci = null;
			Assert.Throws<ArgumentNullException>(() => Culture.Create(ci));
		}

		[Test]
		public void StringComparers()
		{
			const string apple = "Apple";
			const string aeble = "Æble";
			Assert.IsTrue(Culture.Create(new LanguageName("en-US")).StringComparer.Compare(apple, aeble) > 0);
			Assert.IsTrue(Culture.Create(new LanguageName("da-DK")).StringComparer.Compare(apple, aeble) < 0);
		}
	}
}
