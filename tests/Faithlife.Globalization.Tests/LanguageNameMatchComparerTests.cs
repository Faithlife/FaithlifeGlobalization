using NUnit.Framework;

namespace Faithlife.Globalization.Tests
{
	[TestFixture]
	public class LanguageNameMatchComparerTests
	{
		[TestCase("en-US", "en", "en-NZ", 1)]
		[TestCase("en-US", "en-ES", "en", -1)]
		[TestCase("en-US", "en-ES", "en-NZ", 0)]
		[TestCase("en", "ja", "en-NZ", -1)]
		[TestCase("en", "ja", "es", 0)]
		[TestCase("en", "", "es", 1)]
		[TestCase("en", "en-NZ", "en", -1)]
		[TestCase("cs", "cs", "en-US", 1)]
		[TestCase("cs", "en-US", "cs", -1)]
		[TestCase("cs", "da", "en-US", -1)]
		[TestCase("cs", "en-US", "da", 1)]
		public void Compare(string strBase, string strLeft, string strRight, int nExpected)
		{
			LanguageNameMatchComparer comparer = new LanguageNameMatchComparer(new LanguageName(strBase));
			int nActual = comparer.Compare(new LanguageName(strLeft), new LanguageName(strRight));
			if (nExpected < 0)
				Assert.Less(nActual, 0);
			else if (nExpected > 0)
				Assert.Greater(nActual, 0);
			else
				Assert.AreEqual(0, nActual);
		}
	}
}
