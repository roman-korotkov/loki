using System;
using System.Runtime.Serialization;

namespace Loki.Gettext.PortableObject
{
	/// <summary>
	/// Represents an exception occured when reading a portable format file.
	/// </summary>
	[Serializable]
	public class ReadException : Exception
	{
		public int LineNumber { get; set; }
		public string Line { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="lineNumber">The line number.</param>
		/// <param name="line">The line.</param>
		public ReadException(string message, int lineNumber, string line)
			: base(message)
		{
			LineNumber = lineNumber;
			Line = line;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReadException"/> class.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <param name="context">The context.</param>
		protected ReadException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			LineNumber = info.GetInt32("LineNumber");
			Line = info.GetString("Line");
		}
	}
}