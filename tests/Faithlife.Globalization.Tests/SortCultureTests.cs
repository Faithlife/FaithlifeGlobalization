using System.Collections.Generic;
using Faithlife.Utility;
using NUnit.Framework;

namespace Faithlife.Globalization.Tests
{
	[TestFixture]
	public class SortCultureTests
	{
		[Test]
		public void SortForNull()
		{
			CollectionAssert.AreEqual(new[] { "", "en-AU", "en", "es", "es-NZ", "en-NZ" }, s_cultureNames.Order(CreateComparer(null)));
		}

		[Test]
		public void SortForEmpty()
		{
			CollectionAssert.AreEqual(new[] { "", "en-AU", "en", "es", "es-NZ", "en-NZ" }, s_cultureNames.Order(CreateComparer("")));
		}

		[Test]
		public void SortForEnglish()
		{
			CollectionAssert.AreEqual(new[] { "en", "en-AU", "en-NZ", "", "es", "es-NZ" }, s_cultureNames.Order(CreateComparer("en")));
		}

		[Test]
		public void SortForNewZealandEnglish()
		{
			CollectionAssert.AreEqual(new[] { "en-NZ", "en", "en-AU", "", "es", "es-NZ" }, s_cultureNames.Order(CreateComparer("en-NZ")));
		}

		[Test]
		public void SortForAustraliaEnglish()
		{
			CollectionAssert.AreEqual(new[] { "en-AU", "en", "en-NZ", "", "es", "es-NZ" }, s_cultureNames.Order(CreateComparer("en-AU")));
		}

		[Test]
		public void SortForSpanish()
		{
			CollectionAssert.AreEqual(new[] { "es", "es-NZ", "", "en", "en-AU", "en-NZ" }, s_cultureNames.Order(CreateComparer("es")));
		}

		[Test]
		public void SortForGerman()
		{
			CollectionAssert.AreEqual(new[] { "", "en", "en-AU", "en-NZ", "es", "es-NZ" }, s_cultureNames.Order(CreateComparer("de")));
		}

		private static IComparer<string> CreateComparer(string cultureName) => new Comparer(cultureName);

		private sealed class Comparer : IComparer<string>
		{
			public Comparer(string cultureName)
			{
				m_culture = Culture.Create(new LanguageName(cultureName));
			}

			public int Compare(string x, string y)
			{
				return -m_culture.BestMatchComparison(new LanguageName(x), new LanguageName(y));
			}

			readonly Culture m_culture;
		}

		readonly string[] s_cultureNames = { "en-AU", "en", "", "es", "es-NZ", "en-NZ" };
	}
}
