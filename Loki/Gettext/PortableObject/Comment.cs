namespace Loki.Gettext.PortableObject
{
	/// <summary>
	/// Defines a comment of a portable object.
	/// </summary>
	public class Comment
	{
		/// <summary>
		/// Gets or sets the comment type.
		/// </summary>
		public CommentType Type { get; set; }

		/// <summary>
		/// Gets or sets the comment text.
		/// </summary>
		public string Value { get; set; }
	}
}