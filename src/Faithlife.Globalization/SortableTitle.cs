using System;
using Faithlife.Utility;

namespace Faithlife.Globalization
{
	/// <summary>
	/// Encapsulates a title and its "sort as" string.
	/// </summary>
	public struct SortableTitle : IEquatable<SortableTitle>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SortableTitle"/> class.
		/// </summary>
		/// <param name="strText">The title text.</param>
		/// <remarks>The "sort as" string will be the same as the title.</remarks>
		public SortableTitle(string strText)
			: this(strText, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SortableTitle"/> class.
		/// </summary>
		/// <param name="strText">The title text.</param>
		/// <param name="strSortAs">The "sort as" string. If null, the title is used.</param>
		public SortableTitle(string strText, string strSortAs)
		{
			// check arguments (note that "new SortableTitle" can still construct a SortableTitle with null .Text)
			if (strText == null)
				throw new ArgumentNullException("strText");

			m_strText = strText;
			m_strSortAs = strSortAs ?? strText;
		}

		/// <summary>
		/// Gets the text of the title.
		/// </summary>
		/// <value>The text of the title.</value>
		public string Text
		{
			get { return m_strText; }
		}

		/// <summary>
		/// Gets the "sort as" string.
		/// </summary>
		/// <value>The "sort as" string.</value>
		public string SortAs
		{
			get { return m_strSortAs; }
		}

		/// <summary>
		/// Indicates whether this instance is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><c>true</c> if this instance is equal to <paramref name="other" />; otherwise, <c>false</c>.</returns>
		public bool Equals(SortableTitle other)
		{
			return m_strText == other.m_strText && m_strSortAs == other.m_strSortAs;
		}

		/// <summary>
		/// Returns the text of the title of this instance.
		/// </summary>
		/// <returns>A <see cref="T:System.String"/> containing the text of the title.</returns>
		public override string ToString()
		{
			return m_strText;
		}

		/// <summary>
		/// Indicates whether this instance is equal to another object.
		/// </summary>
		/// <param name="obj">An object to compare with this object.</param>
		/// <returns><c>true</c> if <paramref name="obj"/> and this instance
		/// are the same type and represent the same value; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj)
		{
			return obj is SortableTitle && this.Equals((SortableTitle) obj);
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return HashCodeUtility.CombineHashCodes(
				ObjectUtility.GetHashCode(m_strText), ObjectUtility.GetHashCode(m_strSortAs));
		}

		/// <summary>
		/// The empty title.
		/// </summary>
		public static readonly SortableTitle Empty = new SortableTitle("", "");

		/// <summary>
		/// Compares two instances for equality.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> the instances are equal; otherwise, <c>false</c>.</returns>
		public static bool operator ==(SortableTitle left, SortableTitle right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// Compares two instances for inequality.
		/// </summary>
		/// <param name="left">The left instance.</param>
		/// <param name="right">The right instance.</param>
		/// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
		public static bool operator !=(SortableTitle left, SortableTitle right)
		{
			return !left.Equals(right);
		}

		readonly string m_strText;
		readonly string m_strSortAs;
	}
}
