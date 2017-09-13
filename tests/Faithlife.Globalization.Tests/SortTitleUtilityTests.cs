using System;
using NUnit.Framework;

namespace Faithlife.Globalization.Tests
{
	[TestFixture]
	public class SortTitleUtilityTests
	{
		[TestCase("Today’s New International Version", "en", "Today's New International Version")]
		[TestCase("A Christmas Carol", "en", "Christmas Carol")]
		[TestCase("Johannine Letters: A Commentary on 1, 2, and 3 John", "en", "Johannine Letters: A Commentary on 0001, 0002, and 0003 John")]
		[TestCase("The Nicene and Post-Nicene Fathers Vol. IV, Catholic Edition", "en", "Nicene and Post-Nicene Fathers Vol. I0004, Catholic Edition")]
		[TestCase("This Week, Volume IX", "en", "This Week, Volume V0009")]
		[TestCase("Introduction to the History of Christianity, With CD-ROM", "en", "Introduction to the History of Christianity, With CD-ROM")]
		[TestCase("La Biblia de las Américas", "es", "Biblia de las Américas")]
		[TestCase("P35 from The Text of the Earliest New Testament Greek Manuscripts", "en", "P0035 from The Text of the Earliest New Testament Greek Manuscripts")]
		[TestCase("10,000 Sermon Illustrations", "en", "0010,000000000000 Sermon Illustrations")]
		[TestCase("10,012 Sermon Illustrations", "en", "0010,00000012 Sermon Illustrations")]
		[TestCase("BR 10:04", "en", "BR 0010:00000004")]
		[TestCase("Horae Homileticae Vol. 6: Psalms, LXXIII-CL", "en", "Horae Homileticae Vol. 0006: Psalms, LXXIII-CL")]
		[TestCase("The $150,000 Fund", "en", "0150,000000000000 Fund")]
		[TestCase("Word Biblical Commentary, Volume 34a: Mark 1-8:26", "en", "Word Biblical Commentary, Volume 0034a: Mark 0001-0008:0026")]
		[TestCase("D.L. Moody: Christian History, Issue 25", "en", "D.L. Moody: Christian History, Issue 0025")]
		[TestCase("¿Es posible el hombre nuevo?", "es", "Es posible el hombre nuevo?")]
		[TestCase("“An Introduction to Testing”", "en", "Introduction to Testing\"")]
		[TestCase("Die Heilige Schrift", "de", "Heilige Schrift")]
		[TestCase("Die Hard", "en", "Die Hard")]
		[TestCase("L’Hôpital’s rule", "fr", "Hôpital's rule")]
		[TestCase("1, 12345 and 789", "en", "0001, 12345 and 0789")]
		[TestCase("I II III IV V VI VII VIII IX X XI", "en", "I I0002 I0003 I0004 V V0006 V0007 V0008 V0009 X X0011")]
		[TestCase("Ia, IIb—IIIc IVd Ve VIf VIIg VIIIa IXb Xc XId", "en", "I0001a, I0002b-I0003c I0004d Ve VIf VIIg V0008a V0009b X0010c X0011d")]
		[TestCase("Iambic Ibid Icarus Identity Vapid", "en", "Iambic Ibid Icarus Identity Vapid")]
		[TestCase("XII XV XLVII LXX CL MCMLXXIV", "en", "X0012 X0015 X0047 LXX CL MCMLXXIV")]
		[TestCase("i ii iii iv v vi viii ix x xi I.V. III.", "en", "i ii iii iv v vi viii ix x xi I.V. III.")]
		[TestCase("CD CD-ROM", "en", "CD CD-ROM")]
		[TestCase("  !Title ", "en", "Title")]
		[TestCase("Cairo Geniza Targumic Fragment: MS C", "en", "Cairo Geniza Targumic Fragment: MS C")]
		[TestCase("Cairo Geniza Targumic Fragment: MS CC", "en", "Cairo Geniza Targumic Fragment: MS CC")]
		[TestCase("LXX", "en", "LXX")]
		[TestCase("3D 3-D", "en", "0003D 0003-D")]
		[TestCase("abc “de”—‘f’ 1–2", "en", "abc \"de\"-'f' 0001-0002")]
		[TestCase("The Bible", "", "The Bible")]
		[TestCase("A Bíblia Sagrada em Português, edição Revista e Corrigida", "pt", "Bíblia Sagrada em Português, edição Revista e Corrigida")]
		[TestCase("  Contiguous  white  \u00A0\t \r\n  space  ", "en", "Contiguous white space")]
		[TestCase("Hyphen--and – dash—test", "en", "Hyphen-and - dash-test")]
		[TestCase("Die Bibel", "af", "Bibel")]
		[TestCase("La Sankta Biblio", "eo", "Sankta Biblio")]
		[TestCase("De Nieuwe Bijbelvertaling", "nl", "Nieuwe Bijbelvertaling")]
		[TestCase("Det Nye Testamentet 2005 (Bokmål)", "nb", "Nye Testamentet 2005 (Bokmål)")]
		[TestCase("1000 Bible Images", "en", "1000 Bible Images")]
		[TestCase("[ A Handbook on 1-2 Kings ]", "en", "Handbook on 0001-0002 Kings ]")]
		[TestCase("[ [ [A Handbook on 1-2 Kings ]", "en", "Handbook on 0001-0002 Kings ]")]
		[TestCase("聖經提要（一）", "zh-Hant", "聖經提要（0001）")]
		[TestCase("聖經提要（一百二十三）", "zh-Hant", "聖經提要（0123）")]
		[TestCase("初信造就（中冊）", "zh-Hant", "初信造就（上2冊）")]
		public void CreateSortTitle(string strTitle, string strLanguage, string strExpectedSortTitle)
		{
			LanguageName language = new LanguageName(strLanguage);
			Assert.AreEqual(strExpectedSortTitle, SortTitleUtility.CreateSortTitle(strTitle, language));

			SortableTitle st = SortTitleUtility.CreateSortableTitle(strTitle, language);
			Assert.AreEqual(strTitle, st.Text);
			Assert.AreEqual(strExpectedSortTitle, st.SortAs);
		}

		[Test]
		public void CreateSortTitleBadArgument()
		{
			Assert.Throws<ArgumentNullException>(() => SortTitleUtility.CreateSortTitle(null, new LanguageName("en-US")));
		}

		[Test]
		public void CreateSortableTitleBadArgument()
		{
			Assert.Throws<ArgumentNullException>(() => SortTitleUtility.CreateSortableTitle(null, new LanguageName("en-US")));
		}
	}
}
