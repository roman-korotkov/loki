using System;
using Loki.Gettext.PortableObject;

namespace Loki.Gettext
{
	/// <summary>
	/// Specifies a portable object comment.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
	public class POCommentAttribute : Attribute
	{
		private readonly CommentType _type;
		private readonly string _value;

		/// <summary>
		/// Initializes a new instance of the <see cref="POCommentAttribute"/> class.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="value">The value.</param>
		internal POCommentAttribute(CommentType type, string value)
		{
			_type = type;
			_value = value;
		}

		/// <summary>
		/// Gets the comment type.
		/// </summary>
		public CommentType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Gets the comment value.
		/// </summary>
		public string Value
		{
			get { return _value; }
		}
	}

	/// <summary>
	/// Specifies the translator comment.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
	public class TranslatorComment : POCommentAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TranslatorComment"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public TranslatorComment(string value) 
			: base(CommentType.Translator, value)
		{
		}
	}

	/// <summary>
	/// Specifies the extracted comment.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
	public class ExtractedComment : POCommentAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExtractedComment"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public ExtractedComment(string value) 
			: base(CommentType.Extracted, value)
		{
		}
	}

	/// <summary>
	/// Specifies the extracted.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
	public class Reference : POCommentAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Reference"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Reference(string value) 
			: base(CommentType.Reference, value)
		{
		}
	}

	/// <summary>
	/// Specifies the flag.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
	public class Flag : POCommentAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Flag"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Flag(string value) 
			: base(CommentType.Flag, value)
		{
		}
	}

	/// <summary>
	/// Specifies the previues entry.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property, Inherited = false)]
	public class Previous : POCommentAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Previous"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public Previous(string value)
			: base(CommentType.Previous, value)
		{
		}
	}
}
