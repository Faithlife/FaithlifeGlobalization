using System;
using System.Linq;
using NUnit.Framework;

namespace Faithlife.Globalization.Tests
{
	[TestFixture]
	public class FindBestCultureMatchTests
	{
		[Test]
		public void BestForNull()
		{
			Assert.Throws<ArgumentNullException>(() => Culture.Create(null));
		}

		[Test]
		public void BestForEmpty()
		{
			Assert.AreEqual(new LanguageName("en-AU"), Culture.Create(new LanguageName("")).FindBestMatch(s_languages));
		}

		[Test]
		public void BestForEnglish()
		{
			Assert.AreEqual(new LanguageName("en"), Culture.Create(new LanguageName("en")).FindBestMatch(s_languages));
		}

		[Test]
		public void BestForNewZealandEnglish()
		{
			Assert.AreEqual(new LanguageName("en-NZ"), Culture.Create(new LanguageName("en-nz")).FindBestMatch(s_languages));
			Assert.AreEqual(1, Culture.Create(new LanguageName("en-nz")).FindBestMatches(s_languages, n => n).Count());
		}

		[Test]
		public void BestForAustraliaEnglish()
		{
			Assert.AreEqual(new LanguageName("en-AU"), Culture.Create(new LanguageName("en-au")).FindBestMatch(s_languages));
			Assert.AreEqual(2, Culture.Create(new LanguageName("en-Au")).FindBestMatches(s_languages, n => n).Count());
		}

		[Test]
		public void BestForUnitedStatesEnglish()
		{
			Assert.AreEqual(new LanguageName("en"), Culture.Create(new LanguageName("en-us")).FindBestMatch(s_languages));
		}

		[Test]
		public void BestForSpanish()
		{
			Assert.AreEqual(new LanguageName("es"), Culture.Create(new LanguageName("es")).FindBestMatch(s_languages));
		}

		[Test]
		public void BestForGerman()
		{
			Assert.AreEqual(new LanguageName("de-NZ"), Culture.Create(new LanguageName("de")).FindBestMatch(s_languages));
		}

		[Test]
		public void BestForGermanViaCulture()
		{
			Assert.AreEqual(new LanguageName("de-NZ"), Culture.Create(new LanguageName("de")).FindBestMatch(
				s_languages.Select(Culture.Create)).LanguageName);
		}

		static readonly LanguageName[] s_languages =
		{
			new LanguageName("en-AU"),
			new LanguageName("en"),
			new LanguageName("de-NZ"),
			new LanguageName("es"),
			new LanguageName("es-NZ"),
			new LanguageName("en-NZ"),
			new LanguageName("en-AU")
		};
	}
}
