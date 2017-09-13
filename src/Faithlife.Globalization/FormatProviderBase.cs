using System;

namespace Faithlife.Globalization
{
	/// <summary>
	/// A standard base class for an implementation of IFormatProvider.
	/// </summary>
	internal abstract class FormatProviderBase : IFormatProvider, ICustomFormatter
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FormatProviderBase"/> class.
		/// </summary>
		/// <param name="fpFallback">The fallback format provider. Can be null.</param>
		protected FormatProviderBase(IFormatProvider fpFallback)
		{
			m_fpFallback = fpFallback;
		}

		/// <summary>
		/// Gets the fallback format provider.
		/// </summary>
		/// <value>The fallback format provider.</value>
		/// <remarks>This format provider will be used for format strings that supported
		/// by this format provider.</remarks>
		public IFormatProvider Fallback
		{
			get { return m_fpFallback; }
		}

		/// <summary>
		/// Gets an object that provides formatting services for the specified type.
		/// </summary>
		/// <param name="formatType">An object that specifies the type of format object to get.</param>
		/// <returns>The current instance, if formatType is the same type as the current instance; otherwise, null.</returns>
		public object GetFormat(Type formatType)
		{
			return formatType == typeof(ICustomFormatter) ? this : null;
		}

		/// <summary>
		/// Converts the value of a specified object to an equivalent string representation using specified format and
		/// culture-specific formatting information.
		/// </summary>
		/// <param name="format">A format string containing formatting specifications.</param>
		/// <param name="arg">An object to format.</param>
		/// <param name="formatProvider">An <see cref="T:System.IFormatProvider"></see> object that supplies
		/// format information about the current instance.</param>
		/// <returns>The string representation of the value of arg, formatted as specified by format and formatProvider.</returns>
		public string Format(string format, object arg, IFormatProvider formatProvider)
		{
			if (arg == null)
				throw new ArgumentNullException("arg");

			if (format != null)
			{
				string strResult = FormatCore(format.Trim(), arg);
				if (strResult != null)
					return strResult;
			}

			if (m_fpFallback != null)
			{
				ICustomFormatter cfFallback = m_fpFallback.GetFormat(typeof(ICustomFormatter)) as ICustomFormatter;
				if (cfFallback != null)
					return cfFallback.Format(format, arg, m_fpFallback);
			}

			IFormattable argF = arg as IFormattable;
			return argF != null ? argF.ToString(format, m_fpFallback) : arg.ToString();
		}

		/// <summary>
		/// Overridden in the derived class to format the specified object.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="arg">The object to format.</param>
		/// <returns>The formatted object, or null if this format provider doesn't handle the specified
		/// format string.</returns>
		protected abstract string FormatCore(string format, object arg);

		readonly IFormatProvider m_fpFallback;
	}
}
