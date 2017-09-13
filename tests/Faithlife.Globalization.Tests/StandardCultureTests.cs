using System.Globalization;
using NUnit.Framework;

namespace Faithlife.Globalization.Tests
{
	public class StandardCultureTests
	{
		[TestCase("es-MX")]
		[TestCase("en-NZ")]
		[TestCase("qi-QQ")]
		public void CreateWithCulture(string languageName)
		{
			Culture c = Culture.Create(new LanguageName(languageName), new CultureInfo("en-US"), new CultureInfo("en-CA"));
			Assert.AreEqual(languageName, c.LanguageName.ToString());
			Assert.AreEqual("en-US", c.FormatCultureInfo.Name);
			Assert.AreEqual("en-CA", c.ResourceCultureInfo.Name);
		}

		[TestCase("es-MX")]
		[TestCase("en-NZ")]
		public void Create(string languageName)
		{
			Culture c = Culture.Create(new LanguageName(languageName));
			Assert.AreEqual(languageName, c.LanguageName.ToString());
			Assert.AreEqual(languageName, c.FormatCultureInfo.Name);
			Assert.AreEqual(languageName, c.ResourceCultureInfo.Name);
		}

		[Test]
		public void CreateFallbackSpanish()
		{
			Culture c = Culture.Create(new LanguageName("es-US"));
			Assert.AreEqual("es-US", c.LanguageName.ToString(), "LanguageName is incorrect.");

			Assert.AreEqual("es-US", c.FormatCultureInfo.Name, "FormatCultureInfo.Name is incorrect.");
			Assert.AreEqual("es-US", c.ResourceCultureInfo.Name, "ResourceCultureInfo.Name is incorrect.");
		}

		[Test]
		public void CreateNeutral()
		{
			Culture c = Culture.Create(new LanguageName("en"));
			Assert.AreEqual("en", c.LanguageName.ToString());
			Assert.AreEqual("en-US", c.FormatCultureInfo.Name);
			Assert.AreEqual("en-US", c.ResourceCultureInfo.Name);
		}

		[Test]
		public void CreateAramaic()
		{
			Culture c = Culture.Create(new LanguageName("arc"));
			Assert.AreEqual("arc", c.LanguageName.ToString());
			Assert.AreEqual("he", c.FormatCultureInfo.Name);
			Assert.AreEqual("he", c.ResourceCultureInfo.Name);
		}

		[TestCase("cop-EG")]
		[TestCase("gez")]
		[TestCase("pis")]
		[TestCase("tpi")]
		[TestCase("tsw")]
		[TestCase("x-pc")]
		[TestCase("x-sab")]
		[TestCase("x-tl")]
		public void CreateUnsupported(string languageName)
		{
			Culture c = Culture.Create(new LanguageName(languageName));
			Assert.AreEqual(languageName, c.LanguageName.ToString());
			Assert.AreEqual("", c.FormatCultureInfo.Name);
			Assert.AreEqual("", c.ResourceCultureInfo.Name);
		}
	}
}
