using System;
using NUnit.Framework;

namespace Faithlife.Globalization.Tests
{
	[TestFixture]
	public class SortableTitleTests
	{
		[Test]
		public void ConstructorNull()
		{
			Assert.Throws<ArgumentNullException>(delegate { new SortableTitle(null); });
			Assert.Throws<ArgumentNullException>(delegate { new SortableTitle(null, "test"); });
		}

		[Test]
		public void Constructor()
		{
			SortableTitle t = new SortableTitle("Test");
			Assert.AreEqual("Test", t.Text);
			Assert.AreEqual("Test", t.SortAs);
			Assert.AreEqual("Test", t.ToString());

			t = new SortableTitle("The Test", "Test, The");
			Assert.AreEqual("The Test", t.Text);
			Assert.AreEqual("Test, The", t.SortAs);
			Assert.AreEqual("The Test", t.ToString());
		}

		[Test]
		public void SortableTitleDifferent()
		{
			SortableTitle titleA = new SortableTitle("The Apple", "Apple, The");
			Assert.AreEqual("The Apple", titleA.Text);
			Assert.AreEqual("Apple, The", titleA.SortAs);

			SortableTitle titleB = new SortableTitle("Bears");
			Assert.AreEqual("Bears", titleB.Text);
			Assert.AreEqual("Bears", titleB.SortAs);

			Assert.AreNotEqual(titleA, titleB);

			Assert.IsFalse(titleA == titleB);
			Assert.IsTrue(titleA != titleB);
		}

		[Test]
		public void SortableTitleSortAsSame()
		{
			SortableTitle titleA = new SortableTitle("The Apple", "Apple, The");
			Assert.AreEqual("The Apple", titleA.Text);
			Assert.AreEqual("Apple, The", titleA.SortAs);

			SortableTitle titleB = new SortableTitle("Bears", "Apple, The");
			Assert.AreEqual("Bears", titleB.Text);
			Assert.AreEqual("Apple, The", titleB.SortAs);

			Assert.AreNotEqual(titleA, titleB);

			Assert.IsFalse(titleA == titleB);
			Assert.IsTrue(titleA != titleB);
		}

		[Test]
		public void SortableTitleIdentical()
		{
			SortableTitle titleA = new SortableTitle("The Apple", "Apple, The");
			Assert.AreEqual("The Apple", titleA.Text);
			Assert.AreEqual("Apple, The", titleA.SortAs);

			SortableTitle titleB = new SortableTitle("The Apple", "Apple, The");
			Assert.AreEqual("The Apple", titleB.Text);
			Assert.AreEqual("Apple, The", titleB.SortAs);

			Assert.AreEqual(titleA, titleB);
			Assert.AreEqual(titleA.GetHashCode(), titleB.GetHashCode());

			Assert.IsTrue(titleA == titleB);
			Assert.IsFalse(titleA != titleB);
		}

		[Test]
		public void SortableTitleAlmostIdentical()
		{
			SortableTitle titleA = new SortableTitle("The \u00c1pple", "\u00c1pple, The");
			Assert.AreEqual("The \u00c1pple", titleA.Text);
			Assert.AreEqual("\u00c1pple, The", titleA.SortAs);

			SortableTitle titleB = new SortableTitle("The A\u0301pple", "A\u0301pple, The");
			Assert.AreEqual("The A\u0301pple", titleB.Text);
			Assert.AreEqual("A\u0301pple, The", titleB.SortAs);

			Assert.AreNotEqual(titleA, titleB);

			Assert.IsFalse(titleA == titleB);
			Assert.IsTrue(titleA != titleB);
		}
	}
}
