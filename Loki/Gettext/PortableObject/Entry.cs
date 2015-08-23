namespace Loki.Gettext.PortableObject
{
	/// <summary>
	/// Represents an entry of a portable object.
	/// </summary>
	public class Entry
	{
		private Comment[] _comments;

		/// <summary>
		/// Initializes a new instance of the <see cref="Entry"/> class.
		/// </summary>
		public Entry()
		{
			_comments = new Comment[0];
		}

		/// <summary>
		/// Gets or sets the context.
		/// </summary>
		public string Context { get; set; }

		/// <summary>
		/// Gets or sets the original text.
		/// </summary>
		public string OriginalText { get; set; }

		/// <summary>
		/// Gets or sets the translated text.
		/// </summary>
		public string TranslatedText { get; set; }

		/// <summary>
		/// Gets or sets the comments.
		/// </summary>
		public Comment[] Comments
		{
			get { return _comments; }
			set { _comments = value ?? new Comment[0]; }
		}
	}
}